using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ELTManagement
{
    public partial class ImportData : System.Web.UI.Page
    {
        //The Import Process will use the code in the Program file to execute the ETL process.
        Program Code;

        //Results Object to be passed to the results page
        Results StatsAboutImport = new Results();

        SqlConnection conn = new SqlConnection();

        string datasetName;
        int processId;

        protected void Page_Load(object sender, EventArgs e)
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["BackEndDB"].ConnectionString;
            Code = new Program(conn);

            if (!IsPostBack)
            {
                InputConversionNames();
                drp_Datasets.Width = 175;

            }
            
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

            drp_Datasets.DataSource = listOfConversions;
            drp_Datasets.DataBind();
        }

        protected void btn_Execute_Click(object sender, EventArgs e)
        {
 
            datasetName = drp_Datasets.SelectedValue.ToString();
            processId = QueryProcessID(datasetName);

            //Call Commands from Program file to perform Data Import
            Code.PerformDataImport(processId);
            StatsAboutImport = Code.ImportStatistics;

            //Direct User to results page
            Session["Results"] = StatsAboutImport;
            Response.Redirect("ImportResults.aspx");
        }

    }
}