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
        ETLComponents.AppConfig AppData;
        //Create an SQL Connection
        SqlConnection conn;
        //Create a string value to hold the status of any updates
        string updateInfo;


        protected void Page_Load(object sender, EventArgs e)
        {
            //Set the SQL connection
            AppData = new ETLComponents.AppConfig();
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

        //Method to control the editing for data 
        protected void GridViewEditUser_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewEditUser.EditIndex = e.NewEditIndex;
            PopulateUserDetails();
        }

        //Method to provide delete capabilites for the edit user gridview
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
            updateInfo = "User Deleted Successfully";
            }
            catch (Exception ex)
            {
                updateInfo = ex.Message;
                throw;
            }

            Session["UpdateInfo"] = updateInfo;
            Response.Redirect("AdminOptions.aspx");

        }

        //Method to provide Edit capabilites for the edit user gridview
        protected void GridViewEditUser_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string originalUsername = GridViewEditUser.DataKeys[e.RowIndex].Value.ToString();
            //variable to hold any new values entered by the admin
            TextBox newUserName, newPassword, newRole;
            GridViewRow row = (GridViewRow)GridViewEditUser.Rows[e.RowIndex];

            newUserName = (TextBox)row.Cells[0].Controls[0];
            newPassword = (TextBox)row.Cells[1].Controls[0];
            newRole = (TextBox)row.Cells[2].Controls[0];

            //Create SQL commnad to update the values for that entry
            string commandText = "UPDATE [Project].[dbo].[AdminTable] SET Username = @Username, Password = @Password, Role = @Role WHERE Username = @OriginalUsername";
            SqlCommand comm = new SqlCommand();
            comm.CommandText = commandText;
            comm.Connection = conn;

            //Add Parameters
            comm.Parameters.AddWithValue("@OriginalUsername", originalUsername );
            comm.Parameters["@OriginalUsername"].SqlDbType = SqlDbType.NVarChar;

            comm.Parameters.AddWithValue("@Username", newUserName.Text);
            comm.Parameters["@Username"].SqlDbType = SqlDbType.NVarChar;

            comm.Parameters.AddWithValue("@Password", newPassword.Text);
            comm.Parameters["@Password"].SqlDbType = SqlDbType.NVarChar;

            comm.Parameters.AddWithValue("@Role", newRole.Text);
            comm.Parameters["@Role"].SqlDbType = SqlDbType.NVarChar;

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();
                updateInfo = "User Updated Successfully";
            }
            catch (Exception ex)
            {
                updateInfo = ex.Message;
                throw;
            }

            Session["UpdateInfo"] = updateInfo;
            Response.Redirect("AdminOptions.aspx");
        }

        //Method to provide cancel edit capabilites for the edit user gridview
        protected void GridViewEditUser_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewEditUser.EditIndex = -1;
            PopulateUserDetails();
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminOptions.aspx");
        }
    }
}