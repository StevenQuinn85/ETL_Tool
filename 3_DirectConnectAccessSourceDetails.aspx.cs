using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;

namespace ELTManagement.DataEntryForms
{
    public partial class _3_DirectConnectAccessSourceDetails : System.Web.UI.Page
    {
        string connectionString, fileLocation, password, tableName;
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];

            btn_Next.Visible = false;
            Panel1.Visible = false; Panel2.Visible = false;
            PasswordRequired.Checked = false;

            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                //Configure the text for the tool tips
                ConfigureToolTips();
            }

        }

        private void ConfigureToolTips()
        {
            lbl_FileLocationTip.ToolTip = "Enter the location of the Access Database (including filename and extension)";
            lbl_PasswordTip.ToolTip = "Enter any required password to access the database";
            lbl_TableTip.ToolTip = "Select the source table from database"; 
        }

        protected void btn_Test_Click(object sender, EventArgs e)
        {
            fileLocation = txt_FileLocation.Text;
            password = txt_password.Text;

            if (PasswordRequired.Checked)
            {
                connectionString = "Data Source =" + fileLocation + "; Provider = Microsoft.ACE.OLEDB.12.0; Jet OLEDB:Database Password=" + password + ";";
            }
            else
            {
                connectionString = "Data Source =" + fileLocation + "; Provider = Microsoft.ACE.OLEDB.12.0;";
            }


            if (TestAccessConnection(connectionString))
            {
                lbl_result.Text = "Connection Successful";
                btn_Next.Visible = true;
                Panel1.Visible = true; Panel2.Visible = true;
                TableDropDownList.DataSource = GetTableNames(connectionString);
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
            fileLocation = txt_FileLocation.Text;
            password = txt_password.Text;
            tableName = TableDropDownList.SelectedValue.ToString();
            if (PasswordRequired.Checked)
            {
                connectionString = "Data Source =" + fileLocation + "; Provider = Microsoft.ACE.OLEDB.12.0; Jet OLEDB:Database Password=" + password + ";";
            }
            else
            {
                connectionString = "Data Source =" + fileLocation + "; Provider = Microsoft.ACE.OLEDB.12.0;";
            }


            fileLocation = txt_FileLocation.Text;
            password = txt_password.Text;
            tableName = TableDropDownList.SelectedValue.ToString();
            if (PasswordRequired.Checked)
            {
                connectionString = "Data Source =" + fileLocation + "; Provider = Microsoft.ACE.OLEDB.12.0; Jet OLEDB:Database Password=" + password + ";";
            }
            else
            {
                connectionString = "Data Source =" + fileLocation + "; Provider = Microsoft.ACE.OLEDB.12.0;";
            }

            AddFileLocation(fileLocation);
            AddConnectionString(connectionString);
            AddPassword(password);
            AddTableName(tableName);

            Session["DataProperties"] = DataProperties;
            Response.Redirect("4_DestinationSelection.aspx");
        }

        protected void TableDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableName = TableDropDownList.SelectedValue.ToString();
        }

        private List<string> GetTableNames(string connectionDetails)
        {
            List<string> TableNames = new List<string>();
            string firstEntry = "";
            TableNames.Add(firstEntry);
            DataTable HostTable;

            OleDbConnection conn = new OleDbConnection(connectionDetails);
            conn.Open();
            using (conn)
            {
                HostTable = conn.GetSchema("Tables");
            }

            conn.Close();


            foreach (DataRow item in HostTable.Rows)
            {
                if (!item["TABLE_NAME"].ToString().Contains("MSys"))
                {
                    TableNames.Add(item["TABLE_NAME"].ToString());
                }

            }

            return TableNames;
        }

        private bool TestAccessConnection(string connectionDetails)
        {
            bool success = false;

            OleDbConnection conn = new OleDbConnection(connectionDetails);
            conn.Open();

            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    success = true;
                }
            }
            catch (Exception)
            {

                success = false;
            }

            conn.Close();
            return success;
        }

        private void AddTableName(string tableName)
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

        private void AddPassword(string password)
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

        private void AddConnectionString(string connectionString)
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

        private void AddFileLocation(string fileLocation)
        {
            if (!DataProperties.ContainsKey("Source File Location"))
            {
                DataProperties.Add("Source File Location", fileLocation);
            }
            else
            {
                DataProperties["Source File Location"] = fileLocation;
            }
        }
    }
}