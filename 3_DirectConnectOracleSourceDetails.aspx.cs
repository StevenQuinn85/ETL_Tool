using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;

namespace ELTManagement.DataEntryForms
{
    public partial class _3_DirectConnectOracleSourceDetails : System.Web.UI.Page
    {
        string connectionString, dataSource, dBName, username, password, tableName;
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

        protected void btn_Test_Click(object sender, EventArgs e)
        {
            btn_Next.Visible = false;

            if (!string.IsNullOrEmpty(txt_DataSource.Text))
            {
                dataSource = txt_DataSource.Text;
            }

            if (!string.IsNullOrEmpty(txt_dbName.Text))
            {
                dBName = txt_dbName.Text;
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

                connectionString = "Data Source = " + dataSource + "; User Id =" + dBName + ";Password = " + password + ";";
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

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Session["DataProperties"] = DataProperties;
            Response.Redirect("2_DirectConnectSourceSelection.aspx");
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            dataSource = txt_DataSource.Text;
            username = txt_username.Text;
            dBName = txt_dbName.Text;
            password = txt_password.Text;

            if (PasswordRequired.Checked)
            {
                connectionString = "Data Source = " + dataSource + "; User Id =" + dBName + ";Password = " + password + ";";
            }
            else
            {
                connectionString = "Data Source =" + dataSource + "; Integrated Security = yes;";
            }

            AddSourceDataSource(dataSource);
            AddSourceDBName(dBName);
            AddSourceUsername(username);
            AddSourcePassword(password);
            AddSourceTableName(tableName);
            AddSourceConnectionString(connectionString);

            Session["DataProperties"] = DataProperties;
            Response.Redirect("4_DestinationSelection.aspx");
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

        private void AddSourceDBName(string dBName)
        {
            if (!DataProperties.ContainsKey("Source Database Name"))
            {
                DataProperties.Add("Source Database Name", dBName);
            }
            else
            {
                DataProperties["Source Database Name"] = dBName;
            }
        }

        private void AddSourceDataSource(string dataSource)
        {
            if (!DataProperties.ContainsKey("Source Data Source"))
            {
                DataProperties.Add("Source Data Source", dataSource);
            }
            else
            {
                DataProperties["Source Data Source"] = dataSource;
            }
        }

        protected void TableDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableName = TableDropDownList.SelectedValue.ToString();
        }
    }
}