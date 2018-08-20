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
    public partial class _5_DirectConnectSQLDestinationDetails : System.Web.UI.Page
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
                //ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                //Configure the text for the tool tips
                ConfigureToolTips();
            }
        }

        private void ConfigureToolTips()
        {

            lbl_DatabaseTip.ToolTip = "Enter the destination database name";
            lbl_ServerTip.ToolTip = "Enter the datasource or server where the database is hosted";
            lbl_UsernameTip.ToolTip = "Enter the username required to access the database";
            lbl_PasswordTip.ToolTip = "Enter the password required to access the database";
            lbl_TableTip.ToolTip = "Select the destination table";
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

        protected void btn_Back_Click(object sender, EventArgs e)
        {

            Session["DataProperties"] = DataProperties;

            Response.Redirect("4_DestinationSelection.aspx");


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


            AddDestinationDatabaseName(dbName);
            AddDestinationServerName(serverName);
            AddDestinationUsername(username);
            AddDestinationPassword(password);
            AddDestinationTableName(tableName);
            AddDestinationConnectionString(connectionString);

            Session["DataProperties"] = DataProperties;

            if (DataProperties["Import Type"].Equals("Feed File"))
            {
                Response.Redirect("6_MetaDataSelectionFeedFile.aspx");
            }
            else if (DataProperties["Import Type"].Equals("Direct Connect"))
            {
                Response.Redirect("6_MetaDataSelectionDirectConnect.aspx");
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

        private void AddDestinationServerName(string serverName)
        {

            if (!DataProperties.ContainsKey("Destination Server Name"))
            {
                DataProperties.Add("Destination Server Name", serverName);
            }
            else
            {
                DataProperties["Destination Server Name"] = serverName;
            }

        }

        private void AddDestinationConnectionString(string connectionString)
        {
            if (!DataProperties.ContainsKey("Destination Connection String"))
            {
                DataProperties.Add("Destination Connection String", connectionString);
            }
            else
            {
                DataProperties["Destination Connection String"] = connectionString;
            }
        }

        private void AddDestinationTableName(string tableName)
        {
            if (!DataProperties.ContainsKey("Destination Table Name"))
            {
                DataProperties.Add("Destination Table Name", tableName);
            }
            else
            {
                DataProperties["Destination Table Name"] = tableName;
            }
        }

        private void AddDestinationPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                password = "N/A";
            }

            if (!DataProperties.ContainsKey("Destination Password"))
            {
                DataProperties.Add("Destination Password", password);
            }
            else
            {
                DataProperties["Destination Password"] = password;
            }
        }

        private void AddDestinationUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = "N/A";
            }

            if (!DataProperties.ContainsKey("Destination Username"))
            {
                DataProperties.Add("Destination Username", username);
            }
            else
            {
                DataProperties["Destination Username"] = username;
            }
        }

        private void AddDestinationDatabaseName(string databaseName)
        {
            if (!DataProperties.ContainsKey("Destination Database"))
            {
                DataProperties.Add("Destination Database", databaseName);
            }
            else
            {
                DataProperties["Destination Database"] = databaseName;
            }
        }
    }
}