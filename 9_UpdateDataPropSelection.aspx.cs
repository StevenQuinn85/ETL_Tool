using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace ELTManagement
{
    public partial class _9_UpdateDataPropSelection : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection();
        string datasetName;
        int processId;
        string updateInfo = "";

        //Create an appdata object to hold the connection string details
        ETLComponents.AppConfig appData;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get connection details for back end DB
            appData = new ETLComponents.AppConfig();
            conn.ConnectionString = appData.ConnectionString;


            if (!Page.IsPostBack)
            {
                updateInfo = (string)Session["UpdateInfo"];
                //Removing the update statement 2018-08-27
                //lbl_UpdateInfo.InnerText = updateInfo;
                //reset the update information
                updateInfo = "";
                //Create a list of import process and add to the drop downlist
                InputConversionNames();
            }
        }

        protected void btn_next_Click(object sender, EventArgs e)
        {
            datasetName = DropList_Process.SelectedItem.Value;
            processId = QueryProcessID(datasetName);
            Session["Processid"] = processId;
            Session["datasetName"] = datasetName;

            Response.Redirect("9_EditProcessDetails.aspx");
        }

        private int QueryProcessID(string datasetName)
        {
            int processnumber;
            string commandText = "SELECT ProcessId FROM[dbo].[Program_DataProperties] WHERE DataSetName = @DataSetName";
            SqlCommand comm = new SqlCommand(commandText, conn);
            comm.Parameters.AddWithValue("@DataSetName", datasetName);

            try
            {
                conn.Open();
                processnumber = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();
            }
            catch (Exception)
            {

                throw;
            }

            return processnumber;
        }

        private void InputConversionNames()
        {
            List<string> listOfConversions = new List<string>();
            string commandText = "SELECT DISTINCT [DataSetName] FROM [Project].[dbo].[Program_DataProperties]";

            SqlCommand comm = new SqlCommand(commandText, conn);

            using (conn)
            {
                using (comm)
                {
                    conn.Open();

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listOfConversions.Add(reader["DataSetName"].ToString());
                        }
                    }

                    conn.Close();
                }
            }

            DropList_Process.DataSource = listOfConversions;
            DropList_Process.DataBind();
        }

        protected void DropList_Process_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}