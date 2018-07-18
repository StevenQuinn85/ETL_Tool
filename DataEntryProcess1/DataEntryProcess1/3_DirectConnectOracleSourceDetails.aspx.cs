using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;

namespace DataEntryProcess1
{
    public partial class _3_DirectConnectOracleSourceDetails : System.Web.UI.Page
    {
        string connectionString, dataSource, username, password, tableName;
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];
            bntNext.Visible = false;
            Panel1.Visible = false; Panel2.Visible = false;
            PasswordRequired.Checked = true;
        }

        protected void btn_Test_Click(object sender, EventArgs e)
        {
            bntNext.Visible = false;

            if (!string.IsNullOrEmpty(txt_DataSource.Text))
            {
                dataSource = txt_DataSource.Text;
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

                connectionString = "Data Source = " + dataSource + "; User Id =" + username + ";Password = " + password + ";";
            }
            else
            {
                connectionString = "Data Source =" + dataSource + "; Integrated Security = yes;";
            }

            if (TestSQLConnection(connectionString))
            {
                lbl_result.Text = "Connection Successful";
                bntNext.Visible = true;
                Panel1.Visible = true; Panel2.Visible = true;
                TableDropDownList.DataSource = GetTableNames();
                TableDropDownList.DataBind();
            }
            else
            {
                lbl_result.Text = "Please Check Data";
            }
        }

        protected void bntNext_Click(object sender, EventArgs e)
        {
            dataSource = txt_DataSource.Text;
            username = txt_username.Text;
            password = txt_password.Text;

            if (PasswordRequired.Checked)
            {
                connectionString = "Data Source = " + dataSource + "; User Id =" + username + ";Password = " + password + ";";
            }
            else
            {
                connectionString = "Data Source =" + dataSource + "; Integrated Security = yes;";
            }

            DataProperties.Add("Data Source", dataSource);
            DataProperties.Add("Username", username);
            DataProperties.Add("Password", password);
            DataProperties.Add("Table Name", tableName);

            Session["DataProperties"] = DataProperties;
            Response.Redirect("");
        }

        protected void TableDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableName = TableDropDownList.SelectedValue.ToString();
        }

        private List<string> GetTableNames()
        {
            List<string> TableNames = new List<string>();
            List<string> owners = new List<string>();
            string firstEntry = "";
            TableNames.Add(firstEntry);
            OracleConnection conn = new OracleConnection(connectionString);
            string commString = "SELECT TABLE_NAME,Owner FROM all_tables";
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

        private bool TestSQLConnection(string connectionDetails)
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