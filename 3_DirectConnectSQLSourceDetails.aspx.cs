using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace ELTManagement.DataEntryForms
{
    public partial class _3_DirectConnectSQLSourceDetails : System.Web.UI.Page
    {
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();
        string connectionString, dbName, serverName, tableName, username, password;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];
            btn_Next.Visible = false;
            Panel1.Visible = false; Panel2.Visible = false;

            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
            }
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Session["DataProperties"] = DataProperties;
            Response.Redirect("2_DirectConnectSourceSelection.aspx");
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            dbName = txt_DatabaseName.Text;
            serverName = txt_ServerName.Text;
            username = txt_username.Text;
            password = txt_password.Text;
            tableName = TableDropDownList.SelectedValue.ToString();

            if (PasswordRequired.Checked)
            {
                connectionString = "Server=" + serverName + ";Database=" + dbName + ";User Id=" + username + ";Password=" + password;
            }
            else
            {
                connectionString = "Server=" + serverName + ";Database=" + dbName + ";Trusted_Connection=True;";
            }

            AddSourceDatabaseName(dbName);
            AddSourceServerName(serverName);
            AddSourceUsername(username);
            AddSourcePassword(password);
            AddSourceTableName(tableName);
            AddSourceConnectionString(connectionString);



            Session["DataProperties"] = DataProperties;
            Response.Redirect("4_DestinationSelection.aspx");

        }

        protected void btn_Test_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_DatabaseName.Text))
            {
                dbName = txt_DatabaseName.Text;
            }

            if (!string.IsNullOrEmpty(txt_ServerName.Text))
            {
                serverName = txt_ServerName.Text;
            }

            if (PasswordRequired.Checked)
            {
                if (!string.IsNullOrEmpty(txt_username.Text))
                {
                    username = txt_username.Text;
                }
                if (!string.IsNullOrEmpty(txt_password.Text))
                {
                    password = txt_password.Text;
                }

                connectionString = "Server=" + serverName + ";Database=" + dbName + ";User Id=" + username + ";Password=" + password;
            }
            else
            {
                connectionString = "Server=" + serverName + ";Database=" + dbName + ";Trusted_Connection=True;";
            }

            if (TestSQLConnection(connectionString))
            {
                lbl_result.Text = "Connection Successful";
                btn_Next.Visible = true;
                Panel1.Visible = true; Panel2.Visible = true;
                TableDropDownList.DataSource = GetTableNames();
                TableDropDownList.DataBind();
            }
            else
            {
                lbl_result.Text = "Please Check Data";
            }
        }

        protected void TableDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableName = TableDropDownList.SelectedValue.ToString();
        }

        private bool TestSQLConnection(string connectionDetails)
        {
            bool success = false;

            try
            {
                SqlConnection conn = new SqlConnection(connectionDetails);
                conn.Open();

                if (conn.State == ConnectionState.Open)
                {
                    success = true;
                }
                conn.Close();
            }
            catch (Exception)
            {

                success = false;
            }


            return success;
        }

        private List<string> GetTableNames()
        {
            List<string> TableNames = new List<string>();
            string firstEntry = "";
            TableNames.Add(firstEntry);
            SqlConnection conn = new SqlConnection(connectionString);
            string commString = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG = '" + dbName + "'";
            SqlCommand comm = new SqlCommand(commString, conn);

            using (conn)
            {
                conn.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TableNames.Add(reader["TABLE_NAME"].ToString());
                    }
                }


                conn.Close();
            }


            return TableNames;
        }

        private void AddSourceConnectionString(string connectionString)
        {
            if (!DataProperties.ContainsKey("Source Connection String"))
            {
                DataProperties.Add("Source Connection String", connectionString);
            }
            else
            {
                DataProperties["Source Connection String"] = connectionString;
            }
        }

        private void AddSourceTableName(string tableName)
        {
            if (!DataProperties.ContainsKey("Source Table Name"))
            {
                DataProperties.Add("Source Table Name", tableName);
            }
            else
            {
                DataProperties["Source Table Name"] = tableName;
            }
        }

        private void AddSourcePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                password = "N/A";
            }

            if (!DataProperties.ContainsKey("Source Password"))
            {
                DataProperties.Add("Source Password", password);
            }
            else
            {
                DataProperties["Source Password"] = password;
            }
        }

        private void AddSourceUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = "N/A";
            }

            if (!DataProperties.ContainsKey("Source Username"))
            {
                DataProperties.Add("Source Username", username);
            }
            else
            {
                DataProperties["Source Username"] = username;
            }
        }

        private void AddSourceServerName(string serverName)
        {
            if (!DataProperties.ContainsKey("Source Server Name"))
            {
                DataProperties.Add("Source Server Name", serverName);
            }
            else
            {
                DataProperties["Source Server Name"] = serverName;
            }
        }

        private void AddSourceDatabaseName(string dbName)
        {
            if (!DataProperties.ContainsKey("Source Database Name"))
            {
                DataProperties.Add("Source Database Name", dbName);
            }
            else
            {
                DataProperties["Source Database Name"] = dbName;
            }
        }
    }
}