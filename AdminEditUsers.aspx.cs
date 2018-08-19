using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace ELTManagement
{
    public partial class AdminEditUsers : System.Web.UI.Page
    {
        //Create an AppConfig object to get the connection string
        AppConfig AppData;
        //Create an SQL Connection
        SqlConnection conn;


        protected void Page_Load(object sender, EventArgs e)
        {
            //Set the SQL connection
            AppData = new AppConfig();
            conn = new SqlConnection();
            conn.ConnectionString = AppData.ConnectionString;

            if (!IsPostBack)
            {
                //When the page first loads enter the user details in the gridview
                PopulateUserDetails();
            }
        }

        private void PopulateUserDetails()
        {
            string commandText = "SELECT Username,Password,Role FROM [Project].[dbo].[AdminTable];";
            SqlCommand comm = new SqlCommand(commandText, conn);

            SqlDataAdapter da = new SqlDataAdapter(comm);
            DataSet ds = new DataSet();
            conn.Open();
            da.Fill(ds);
            conn.Close();

            GridViewEditUser.DataSource = ds;
            GridViewEditUser.DataBind();
        }

        protected void GridViewEditUser_PageIndexChanging_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }

        protected void GridViewEditUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridViewEditUser_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void GridViewEditUser_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)GridViewEditUser.Rows[e.RowIndex];
            string username = GridViewEditUser.DataKeys[e.RowIndex].Value.ToString();
            string commandText = "DELETE FROM[Project].[dbo].[AdminTable] WHERE Username =@Username";


            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;
            comm.CommandText = commandText;
            comm.Parameters.AddWithValue("@Username", username);
            comm.Parameters["@Username"].SqlDbType = SqlDbType.NVarChar;

            try
            {
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
            PopulateUserDetails();
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}