using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;

namespace ELTManagement
{
    public partial class _5_DirecConnectOracleDestinationDetails : System.Web.UI.Page
    {
        string connectionString, dataSource, dbName, username, password, tableName;
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];

            btn_Next.Visible = false;
            Panel1.Visible = false; Panel2.Visible = false;
            PasswordRequired.Checked = true;


            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
            }
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Session["DataProperties"] = DataProperties;

            Response.Redirect("4_DestinationSelection.aspx");

        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            dataSource = txt_DataSource.Text;
            dbName = txt_dbName.Text;
            username = txt_username.Text;
            password = txt_password.Text;

            if (PasswordRequired.Checked)
            {
                connectionString = "Data Source = " + dataSource + "; User Id =" + dbName + ";Password = " + password + ";";
            }
            else
            {
                connectionString = "Data Source =" + dataSource + "; Integrated Security = yes;";
            }

            AddDestinationDataSource(dataSource);
            AddDestinationDBName(dbName);
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

        private void AddDestinationDBName(string dbName)
        {
            if (!DataProperties.ContainsKey("Destination Database"))
            {
                DataProperties.Add("Destination Database", dbName);
            }
            else
            {
                DataProperties["Destination Database"] = dbName;
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

        protected void btn_Test_Click(object sender, EventArgs e)
        {
            btn_Next.Visible = false;

            if (!string.IsNullOrEmpty(txt_DataSource.Text))
            {
                dataSource = txt_DataSource.Text;
            }

            if (!string.IsNullOrEmpty(txt_dbName.Text))
            {
                dbName = txt_dbName.Text;
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

                connectionString = "Data Source = " + dataSource + "; User Id =" + dbName + ";Password = " + password + ";";
            }
            else
            {
                connectionString = "Data Source =" + dataSource + "; Integrated Security = yes;";
            }

            if (TestOracleConnection(connectionString))
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

        private void AddDestinationDataSource(string dataSource)
        {
            if (!DataProperties.ContainsKey(""))
            {
                DataProperties.Add("Destination Data Source", dataSource);
            }
            else
            {
                DataProperties["Destination Data Source"] = dataSource;
            }
        }

        //Pull the table names from the selected database and provide users with a selection
        private List<string> GetTableNames()
        {
            List<string> TableNames = new List<string>();
            List<string> owners = new List<string>();
            string firstEntry = "";
            TableNames.Add(firstEntry);
            OracleConnection conn = new OracleConnection(connectionString);

            //select tables names from user created databases, removing the system databases
            string commString = "SELECT TABLE_NAME, Owner FROM all_tables WHERE OWNER NOT IN ('SYS', 'XDB', 'SYSTEM', 'CTXSYS', 'MDSYS', 'APEX_040000')";
            OracleCommand comm = new OracleCommand(commString, conn);

            using (conn)
            {
                conn.Open();

                using (OracleDataReader reader = comm.ExecuteReader())
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
        //This method tests the connection details provided to ensure they are valid
        private bool TestOracleConnection(string connectionDetails)
        {
            bool success = false;

            try
            {
                OracleConnection conn = new OracleConnection(connectionDetails);
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
    }
}