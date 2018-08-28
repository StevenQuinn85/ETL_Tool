using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using System.Configuration;

namespace ELTManagement
{
    public partial class _9_DeleteProcess : System.Web.UI.Page
    {
        //The application will pull it's SQL Connection to the back end DB from this object
        ETLComponents.AppConfig AppData;

        //Deleting a process will require a connection to the Back End DB
        SqlConnection conn = new SqlConnection();

        protected void Page_Load(object sender, EventArgs e)
        {            
            //Use the connection string from the app data object to connect to the back end DB
            AppData = new ETLComponents.AppConfig();
            conn.ConnectionString = AppData.ConnectionString;

            if (!IsPostBack)
            {
                PopulateDeleteProcessDataGrid();
            }
        }

        private void PopulateDeleteProcessDataGrid()
        {


            //Create a delete command
            string commandText = "SELECT  [ProcessId],[DataSetName] FROM [dbo].[Program_DataProperties]";
            
            //Link the delete command and SQL connection to a SQL Command
            SqlCommand comm = new SqlCommand(commandText, conn);
            
            //Create a Data adapter and Dataset to populate the Grid view with
            SqlDataAdapter da = new SqlDataAdapter(comm);
            DataSet ds = new DataSet();

            try
            {
            conn.Open();
            da.Fill(ds);
            conn.Close();
            }
            catch (Exception)
            {

                throw;
            }


            DeleteProcessGridView.DataSource = ds;
            DeleteProcessGridView.DataBind();
        }

        protected void DeleteProcessGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int processId = Convert.ToInt32(DeleteProcessGridView.DataKeys[e.RowIndex].Value.ToString());

            string updateInfo;
            GridViewRow currentRow = (GridViewRow)DeleteProcessGridView.Rows[e.RowIndex];
            Label lbldeleteid = (Label)currentRow.FindControl("lblID");

            //Delete from the data properities table
            SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[Program_DataProperties] WHERE ProcessId = @ProcessId", conn);
            cmd.Parameters.AddWithValue("@ProcessId", processId);
            cmd.Parameters["@ProcessId"].SqlDbType = SqlDbType.Int;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                updateInfo = "Record Successfully Deleted";
            }
            catch (Exception ex)
            {

                updateInfo = "Error " + ex.Message;
            }

            //Delete from the Primary Key table
            cmd.CommandText = "  DELETE FROM [Project].[dbo].[Program_PrimaryKeyData] WHERE ProcessId =@ProcessId";

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                updateInfo = "Record Successfully Deleted";
            }
            catch (Exception ex)
            {

                updateInfo = "Error " + ex.Message;
            }

            //Delete from the MetaData table
            cmd.CommandText = "  DELETE FROM [Project].[dbo].[Program_Metadata] WHERE ProcessId = @ProcessId";

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                updateInfo = "Record Successfully Deleted";
            }
            catch (Exception ex)
            {

                updateInfo = "Error " + ex.Message;
            }

            //Redirect to selection page.
            Session["UpdateInfo"] = updateInfo;
            Response.Redirect("Configure.aspx");
        }
    }
}