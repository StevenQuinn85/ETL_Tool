using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace ELTManagement.DataEntryForms
{
    public partial class _1_DataSetName : System.Web.UI.Page
    {
        //This will be the first form in the Data Entry Process

        //The Data set name will be stored in this string variable
        string datasetName;

        //Creating a Dictionary Structure to hold all the data properties
        //This Dictionary will be passed to all the pages in the data entry
        //process to collect data for insert to the DB.
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        //The application will pull it's SQL Connection to the back end DB from this object
        AppConfig AppData;


        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorLabel.Visible = false;

            if (!IsPostBack)
            {
                DataProperties = (Dictionary<string, string>)(Session["DataProperties"]);
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
            }
        }

        protected void btn_Confirm_Click(object sender, EventArgs e)
        {
            //check if the text box for new dataset name is null
            if (!string.IsNullOrEmpty(txt_DatasetName.Text))
            {
                //if not set the dataset name to be the entry
                datasetName = txt_DatasetName.Text;
                
                //Check if the name already exists
                if (!CheckIfNameAlreadyExists(datasetName))
                {
                    AddDataSetName(datasetName);
                    Session["DataProperties"] = DataProperties;
                    Response.Redirect("1_ImportMethodSelection.aspx");
                }
                else
                {
                    ErrorLabel.Visible = true;
                    ErrorLabel.Text = "** Dataset Name already exists. Please Enter unique name";
                }
            }
            else
            {
                //If a datasetname has not been entered through an error
                ErrorLabel.Visible = true;
                ErrorLabel.Text = "**Please enter dataset name";
            }



        }

        //This method will check if the name already exists
        private bool CheckIfNameAlreadyExists(string datasetName)
        {
            bool exists = false;
            int count = 0;
            string commandText = "SELECT COUNT(*) FROM [dbo].[Program_DataProperties] WHERE DataSetName = @DataSetName";
            
            //Create an SQL Connection object
            SqlConnection conn = new SqlConnection();

            //Pull the back end DB connection from the AppData object
            AppData = new AppConfig();
            conn.ConnectionString = AppData.ConnectionString;

            //Create an SQL Command to quer the back end database
            SqlCommand comm = new SqlCommand(commandText, conn);

            comm.Parameters.AddWithValue("@DataSetName", datasetName);
            comm.Parameters["@DataSetName"].SqlDbType = System.Data.SqlDbType.NVarChar;

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

            //If the count is more than 0 it means that a record with the same process name already exists
            if (count > 0)
            {
                exists = true;

            }

            return exists;

        }


        // This method will add the import type to the Data Properties Dictionary
        //It will check if the value already exists and either update or add
        private void AddDataSetName(string datasetName)
        {
            if (!DataProperties.ContainsKey("Dataset Name"))
            {
                DataProperties.Add("Dataset Name", datasetName);
            }
            else
            {
                DataProperties["Dataset Name"] = datasetName;
            }
        }

    }
}