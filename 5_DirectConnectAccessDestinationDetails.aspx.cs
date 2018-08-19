using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;

namespace ELTManagement
{
    public partial class _5_DirectConnectAccessDestinationDetails : System.Web.UI.Page
    {
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();
        string connectionString, fileLocation, password, tableName;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];

            btn_Next.Visible = false;
            Panel1.Visible = false; Panel2.Visible = false;
            PasswordRequired.Checked = false;

            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
            }
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
           

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                success = false;
            }

            conn.Close();
            return success;
        }

        private void AddTableName(string tableName)
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
            {
                Session["DataProperties"] = DataProperties;

                Response.Redirect("4_DestinationSelection.aspx");
            }
        }

        private void AddPassword(string password)
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

        private void AddConnectionString(string connectionString)
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

        private void AddFileLocation(string fileLocation)
        {
            if (!DataProperties.ContainsKey("Destination File Location"))
            {
                DataProperties.Add("Destination File Location", fileLocation);
            }
            else
            {
                DataProperties["Destination File Location"] = fileLocation;
            }
        }
    }
}