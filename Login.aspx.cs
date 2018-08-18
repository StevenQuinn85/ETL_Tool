﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace ELTManagement
{
    public partial class Login : System.Web.UI.Page
    {
        internal string username, password;
        internal bool authenticationSuccessful = false;
        SqlConnection conn;
        AppConfig AppData;

        protected void Page_Load(object sender, EventArgs e)
        {

                AppData = new AppConfig();
                conn = new SqlConnection();
                conn.ConnectionString = AppData.ConnectionString;


        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_password.Text))
            {
                password = txt_password.Text;
            }
            else
            {
                lbl_error.Text = "** Please Enter Password";
            }

            if (!string.IsNullOrEmpty(txt_Username.Text))
            {
                username = txt_Username.Text;
            }
            else
            {
                lbl_error.Text = "** Please Enter Username";
            }

            if ((!string.IsNullOrEmpty(txt_password.Text) && (!string.IsNullOrEmpty(txt_Username.Text))))
            {
            authenticationSuccessful = CheckCredentials(username,password);

            if (authenticationSuccessful)
            {
                Response.Redirect("Home.aspx");

            }
            else
            {
                lbl_error.Text = "** Please Check Login Credentials";
            }
            }


        }

        private bool CheckCredentials(string username, string password)
        {
            bool success = false;
            string commandtext = "SELECT COUNT(*) FROM [Project].[dbo].[AdminTable] WHERE Username = @Username and Password = @Password;";
            SqlCommand comm = new SqlCommand();
            int count = 0;

            comm.Connection = conn;
            comm.CommandText = commandtext;
            comm.Parameters.AddWithValue("@Username", username);
            comm.Parameters["@Username"].SqlDbType = System.Data.SqlDbType.NVarChar;
            comm.Parameters.AddWithValue("@Password", password);
            comm.Parameters["@Password"].SqlDbType = System.Data.SqlDbType.NVarChar;

            try
            {
                conn.Open();
                 count = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();
            }
            catch (Exception)
            {                
                throw;
            }

            if (count>0)
            {
                success = true;
            }

            return success;
        }
    }
}