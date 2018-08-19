using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace ELTManagement
{
    public partial class AdminAddNewUser : System.Web.UI.Page
    {
        //Create an AppConfig object to get the connection string
        AppConfig AppData;
        //Create an SQL Connection
        SqlConnection conn;

        //Create variables to hold the login information
        internal string username, password, role;
        //Create a string value to hold the outcome of the sql command
        internal string updateInfo;

        //Bool for checking if the username alrady exists
        bool userIdExists;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Set the SQL connection
            AppData = new AppConfig();
            conn = new SqlConnection();
            conn.ConnectionString = AppData.ConnectionString;

            if (!IsPostBack)
            {
                //Set the options for the role dropdown list
                EnterRoleOptions();
            }
        }

        protected void btn_Execute_Click(object sender, EventArgs e)
        {
            //Run a check to make sure that a username and password was entered
            //If entered set the username and password to be this values

            if (!string.IsNullOrEmpty(txt_Password.Text))
            {
                password = txt_Password.Text;
            }
            else
            {
                lbl_error.Text = "** Please Enter Password";
            }

            if (!string.IsNullOrEmpty(txt_Username.Text))
            {
                username = txt_Username.Text;            
                
                //Check that the username is unique to the admin table
                userIdExists = CheckForExistingID(username);
            }
            else
            {
                lbl_error.Text = "** Please Enter Username";
            }



            role = drp_role.SelectedItem.ToString();

            //Check that the required details are entered and attempt to load entry to database AdminTable
            if ((!string.IsNullOrEmpty(txt_Password.Text) && (!string.IsNullOrEmpty(txt_Username.Text)) && !userIdExists))
            {
                //Step the SQL Command details
                string commandText = "INSERT INTO [Project].[dbo].[AdminTable] (Username, Password, Role) VALUES (@Username, @Password, @Role)";
                SqlCommand comm = new SqlCommand();
                comm.CommandText = commandText;
                comm.Connection = conn;
                comm.Parameters.AddWithValue("@Username", username);
                comm.Parameters["@Username"].SqlDbType = System.Data.SqlDbType.NVarChar;
                comm.Parameters.AddWithValue("@Password", password);
                comm.Parameters["@Password"].SqlDbType = System.Data.SqlDbType.NVarChar;
                comm.Parameters.AddWithValue("@Role", role);
                comm.Parameters["@Role"].SqlDbType = System.Data.SqlDbType.NVarChar;

                try
                {
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                    updateInfo = "New User added";
                }
                catch (Exception ex)
                {
                    lbl_error.Text = ex.Message;
                    throw;
                }
            }

            Session["UpdateInfo"] = updateInfo;
            Response.Redirect("AdminOptions.aspx");
        }

        private bool CheckForExistingID(string username)
        {
            bool exists = false;

            //Step the SQL Command details
            string commandText = "SELECT COUNT(*) FROM [Project].[dbo].[AdminTable] WHERE Username =@Username";
            SqlCommand comm = new SqlCommand();
            comm.CommandText = commandText;
            comm.Connection = conn;
            comm.Parameters.AddWithValue("@Username", username);
            comm.Parameters["@Username"].SqlDbType = System.Data.SqlDbType.NVarChar;

            //Variable to hold the number of usernames returned by command;
            int count =0;
            try
            {
                conn.Open();
                count = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();
            }
            catch (Exception ex)
            {
                lbl_error.Text = ex.Message;
            }

            if (count >0)
            {
                exists = true;
            }
            else
            {
                lbl_error.Text = "User Id already exists";
            }

            return exists;
        }

        private void EnterRoleOptions()
        {
            //Create a list and add the two roles for the system access
            //Make 'User' the first option in the list as it will be the default access level
            List<string> roles = new List<string>();
            roles.Add("User");roles.Add("Admin");

            //Set the drop list data source to be the list of roles.
            drp_role.DataSource = roles;
            drp_role.DataBind();
        }
    }
}