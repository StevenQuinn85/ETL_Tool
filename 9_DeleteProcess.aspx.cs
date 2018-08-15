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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDeleteProcessDataGrid();

            }
        }

        private void PopulateDeleteProcessDataGrid()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["BackEndDB"].ConnectionString;
            string commandText = "SELECT  [ProcessId],[DataSetName] FROM [dbo].[Program_DataProperties]";
            SqlCommand comm = new SqlCommand(commandText, conn);
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
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["BackEndDB"].ConnectionString;
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