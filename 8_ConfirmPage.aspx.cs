using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace ELTManagement
{
    public partial class _8_ConfirmPage : System.Web.UI.Page
    {
        //SQL Connection to load data to backend DB

        SqlConnection conn = new SqlConnection();

        //Connection details for the backend DB will come from a Appconfig object
        AppConfig AppData;

        //int NumberOfColumns;
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        List<string> c_Names = new List<string>();
        List<string> c_Order = new List<string>();
        List<string> c_DataTypes = new List<string>();
        List<string> c_MinLenth = new List<string>();
        List<string> c_MaxLength = new List<string>();
        List<string> c_Nullable = new List<string>();
        List<string> c_NullAction = new List<string>();
        List<string> c_ReplaceValue = new List<string>();
        List<string> lst_PrimaryKeys = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Pull the DB connection details
            AppData = new AppConfig();

            //conn.ConnectionString = ConfigurationManager.ConnectionStrings["BackEndDB"].ConnectionString;
            conn.ConnectionString = AppData.ConnectionString;

            DataProperties = (Dictionary<string, string>)Session["DataProperties"];

            //Receive all of the data properties entered by the user
            c_Names = (List<string>)(Session["ColumnNames"]);
            c_Order = (List<string>)(Session["ColumnOrder"]);
            c_DataTypes = (List<string>)(Session["DataTypes"]);
            lst_PrimaryKeys = (List<string>)(Session["PrimaryKeys"]);
            c_MinLenth = (List<string>)(Session["MinLength"]);
            c_MaxLength = (List<string>)(Session["MaxLength"]);
            c_Nullable = (List<string>)(Session["NullPermitted"]);
            c_NullAction = (List<string>)(Session["NullAction"]);
            c_ReplaceValue = (List<string>)(Session["ReplacementValue"]);

            //Display the details entered by the user in different sections
            DisplaySourceDetails();
            DisplayDestinationDetails();
            DisplayMetaData();
            DisplayPrimaryKeys();

            //If the import type is direct connect - check if look back options need to be displayed
            if (DataProperties["Import Type"].Equals("Direct Connect"))
            {
                if (DataProperties.ContainsKey("Lookback Option"))
                {
                    if (DataProperties["Lookback Option"].Equals("Yes"))
                    {
                        DisplayLookBackOption();
                    }
                }

            }


        }

        //Populate the lookback details
        private void DisplayLookBackOption()
        {
            StringBuilder TableText = new StringBuilder();

            TableText.AppendLine("<h4>Look Back Options</h4>");
            TableText.AppendLine("<table border = '" + "1" + "' cellpadding = '" + "2" + "'>");
            TableText.AppendLine("<tr>");
            TableText.AppendLine("<td>Use Lookback:");
            TableText.AppendLine("</td>");
            TableText.AppendLine("<td>");
            TableText.AppendLine(DataProperties["Lookback Option"]);
            TableText.AppendLine("</td>");
            TableText.AppendLine("</tr>");

            TableText.AppendLine("<tr>");
            TableText.AppendLine("<td>Lookback Period:");
            TableText.AppendLine("</td>");
            TableText.AppendLine("<td>");
            TableText.AppendLine(DataProperties["Lookback Period"]);
            TableText.AppendLine("</td>");
            TableText.AppendLine("</tr>");

            TableText.AppendLine("<tr>");
            TableText.AppendLine("<td>Lookback Column: ");
            TableText.AppendLine("</td>");
            TableText.AppendLine("<td>");
            TableText.AppendLine(DataProperties["Lookback Column"]);
            TableText.AppendLine("</td>");
            TableText.AppendLine("</tr>");
            TableText.AppendLine("</table>");

            LookBackDisplay.Text = TableText.ToString();
        }

        //Poplulate the primary key details
        private void DisplayPrimaryKeys()
        {
            StringBuilder TableText = new StringBuilder();

            TableText.AppendLine("<h4>Primary Keys</h4>");
            TableText.AppendLine("<table border = '" + "1" + "' cellpadding = '" + "2" + "'>");

            for (int i = 0; i < lst_PrimaryKeys.Count(); i++)
            {
                TableText.AppendLine("<tr>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(lst_PrimaryKeys[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("</tr>");
            }
            TableText.AppendLine("</table> </br>");

            PrimaryKeyDetails.Text = TableText.ToString();
        }

        //Populate the destination details
        private void DisplayDestinationDetails()
        {
            StringBuilder TableText = new StringBuilder();
            TableText.AppendLine("<h4>Data Destination Details</h4>");
            TableText.AppendLine("<table border = '" + "1" + "' cellpadding = '" + "2" + "'>");


            if (DataProperties["Destination Type"].Equals("SQL Server"))
            {
                string[] tableFields = { "Database Name", "Server Name", "UserName", "Password", "Table Name", "Connection String" };

                string[] desDetails = { DataProperties["Destination Database"], DataProperties["Destination Server Name"], DataProperties["Destination Username"], DataProperties["Destination Password"], DataProperties["Destination Table Name"], DataProperties["Destination Connection String"] };

                for (int i = 0; i < desDetails.Count(); i++)
                {
                    TableText.AppendLine("<tr>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(tableFields[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(desDetails[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("</tr>");
                }

                TableText.AppendLine("</table> </br>");
            }
            else if (DataProperties["Destination Type"].Equals("Oracle Database"))
            {
                string[] tableFields = { "Table Name", "Data Source", "UserName", "Password", "Connection String" };

                string[] desDetails = { DataProperties["Destination Table Name"], DataProperties["Destination Data Source"], DataProperties["Destination Username"], DataProperties["Destination Password"], DataProperties["Destination Connection String"] };

                for (int i = 0; i < desDetails.Count(); i++)
                {
                    TableText.AppendLine("<tr>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(tableFields[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(desDetails[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("</tr>");
                }


            }
            else if (DataProperties["Destination Type"].Equals("Access Database"))
            {
                string[] tableFields = { "File Location", "Table Name", "Password", "Connection String" };

                string[] desDetails = { DataProperties["Destination File Location"], DataProperties["Destination Table Name"], DataProperties["Destination Password"], DataProperties["Destination Connection String"] };

                for (int i = 0; i < desDetails.Count(); i++)
                {
                    TableText.AppendLine("<tr>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(tableFields[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(desDetails[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("</tr>");
                }

            }



            TableText.AppendLine("</table> </br>");
            DataDestinationDetails.Text = TableText.ToString();
        }
        //Populate the Metadata details
        private void DisplayMetaData()
        {
            StringBuilder TableText = new StringBuilder();
            string[] tableFields = { "Column Order", "Column Name", "Data Type", "Min Length", "Max Length", "Nulls Permitted", "Null Action", "Replace Value" };
            TableText.AppendLine("<h4>Metadata Details</h4>");
            TableText.AppendLine("<table border = '" + "1" + "' cellpadding = '" + "2" + "'>");
            TableText.AppendLine("<tr>");

            for (int i = 0; i < tableFields.Count(); i++)
            {
                TableText.AppendLine("<th>");
                TableText.AppendLine(tableFields[i]);
                TableText.AppendLine("</th>");
            }

            TableText.AppendLine("</tr>");

            for (int i = 0; i < c_Order.Count(); i++)
            {
                TableText.AppendLine("<tr>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_Order[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_Names[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_DataTypes[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_MinLenth[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_MaxLength[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_Nullable[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_NullAction[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_ReplaceValue[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("</tr>");
            }

            TableText.AppendLine("</table> </br>");

            MetaDataDisplay.Text = TableText.ToString();
        }
        //Poplulate the source details
        private void DisplaySourceDetails()
        {
            StringBuilder TableText = new StringBuilder();

            TableText.AppendLine("<h4>Data Source Details</h4>");
            TableText.AppendLine("<table border = '" + "1" + "' cellpadding = '" + "2" + "'>");

            if (DataProperties["Import Type"].Equals("Feed File"))
            {
                string[] tableFields = { "Dataset Name", "File Location", "File Name", "Delimiter Character" };

                string[] sourceDetails = { DataProperties["Dataset Name"], DataProperties["Source File Location"], DataProperties["Source File Name"], DataProperties["Source Delimiter"] };

                for (int i = 0; i < sourceDetails.Count(); i++)
                {
                    TableText.AppendLine("<tr>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(tableFields[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(sourceDetails[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("</tr>");
                }


            }
            else if (DataProperties["Import Type"].Equals("Direct Connect"))
            {
                if (DataProperties["Source Database Type"].Equals("SQL Server"))
                {
                    string[] tableFields = { "Database", "Server Name", "Table Name", "Connection String", "Username", "Password" };

                    string[] sourceDetails = { DataProperties["Source Database Name"], DataProperties["Source Server Name"], DataProperties["Source Table Name"], DataProperties["Source Connection String"], DataProperties["Source Username"], DataProperties["Source Password"] };

                    for (int i = 0; i < sourceDetails.Count(); i++)
                    {
                        TableText.AppendLine("<tr>");
                        TableText.AppendLine("<td>");
                        TableText.AppendLine(tableFields[i]);
                        TableText.AppendLine("</td>");
                        TableText.AppendLine("<td>");
                        TableText.AppendLine(sourceDetails[i]);
                        TableText.AppendLine("</td>");
                        TableText.AppendLine("</tr>");
                    }

                }
                else if (DataProperties["Source Database Type"].Equals("Oracle Database"))
                {
                    string[] tableFields = { "Data Source", "Table Name", "Connection String", "Username", "Password" };

                    string[] sourceDetails = { DataProperties["Source Data Source"], DataProperties["Source Table Name"], DataProperties["Source Connection String"], DataProperties["Source Username"], DataProperties["Source Password"] };

                    for (int i = 0; i < sourceDetails.Count(); i++)
                    {
                        TableText.AppendLine("<tr>");
                        TableText.AppendLine("<td>");
                        TableText.AppendLine(tableFields[i]);
                        TableText.AppendLine("</td>");
                        TableText.AppendLine("<td>");
                        TableText.AppendLine(sourceDetails[i]);
                        TableText.AppendLine("</td>");
                        TableText.AppendLine("</tr>");
                    }

                }
                else if (DataProperties["Source Database Type"].Equals("Access Database"))
                {
                    string[] tableFields = { "Source Table Name", "Source File Location", "Connection String", "Password" };

                    string[] sourceDetails = { DataProperties["Source Table Name"], DataProperties["Source File Location"], DataProperties["Source Connection String"], DataProperties["Source Password"] };

                    for (int i = 0; i < sourceDetails.Count(); i++)
                    {
                        TableText.AppendLine("<tr>");
                        TableText.AppendLine("<td>");
                        TableText.AppendLine(tableFields[i]);
                        TableText.AppendLine("</td>");
                        TableText.AppendLine("<td>");
                        TableText.AppendLine(sourceDetails[i]);
                        TableText.AppendLine("</td>");
                        TableText.AppendLine("</tr>");
                    }
                }
            }

            TableText.AppendLine("</table> </br>");
            SourceDetailsDisplay.Text = TableText.ToString();
        }

        protected void btn_Confirm_Click(object sender, EventArgs e)
        {
            string processID;
            string errorDetails = "";
            bool loadSuccessful = true;


            try
            {
                LoadDataProperties();
                processID = RetrieveProcessId();
                LoadMetaData(processID);
                LoadPrimaryKeys(processID);
            }
            catch (Exception ex)
            {
                loadSuccessful = false;
                errorDetails = ex.Message;
            }


            if (loadSuccessful)
            {

                //go to a success page
                Response.Redirect("8_ConfirmPageSuccess.aspx");
            }
            else
            {
                //go to an unsuccessful page
                Session["Errors"] = errorDetails;
                Response.Redirect("8_ConfirmPageFailure.aspx");
                
            }
        }

        //Once the data properties have been loaded to the Database, the metadata and primary key details need to be inserted
        //using the same processId. This method returns the processId
        private string RetrieveProcessId()
        {
            string commandText = "SELECT [ProcessId] FROM [dbo].[Program_DataProperties] WHERE [DataSetName] = '" + DataProperties["Dataset Name"] + "'";
            string processId;
            SqlCommand comm = new SqlCommand(commandText, conn);

            try
            {

                conn.Open();
                processId = comm.ExecuteScalar().ToString();
                conn.Close();
            }
            catch (Exception)
            {

                throw;
            }

            return processId;

        }

        //This method loads the data properties entried by the user to the back end DB
        private void LoadDataProperties()
        {
            string commandText = "INSERT INTO [dbo].[Program_DataProperties] ([DataSetName],[ImportType],[SourceDBType],[SourceTableName],[SourceDatabase],[SourceServerName],[SourceDataSource],[SourceFileLocation],[SourceUserName],[SourcePassword],[SourceConnectionString],[SourceFileName],[SourceDelimiter],[DestinationType],[DestinationFileLocation],[DestinationConnectionString],[DestinationDatabase],[DestinationServerName],[DestinationUsername],[DestinationPassword],[DestinationTableName],[DestinationDataSource],[UseLookBack],[LookBackPeriod],[LookBackColumnName]) VALUES (@DataSetName,@ImportType,@SourceDBType,@SourceTableName,@SourceDatabase,@SourceServerName,@SourceDataSource,@SourceFileLocation,@SourceUserName,@SourcePassword,@SourceConnectionString,@SourceFileName,@SourceDelimiter,@DestinationType,@DestinationFileLocation,@DestinationConnectionString,@DestinationDatabase,@DestinationServerName,@DestinationUsername,@DestinationPassword,@DestinationTableName,@DestinationDataSource,@UseLookBack,@LookBackPeriod,@LookbackColumnName)";
            string[] textParameters = { "@DataSetName", "@ImportType", "@SourceDBType", "@SourceTableName", "@SourceDatabase", "@SourceServerName", "@SourceDataSource", "@SourceFileLocation", "@SourceUserName", "@SourcePassword", "@SourceConnectionString", "@SourceFileName", "@SourceDelimiter", "@DestinationType", "@DestinationFileLocation", "@DestinationConnectionString", "@DestinationDatabase", "@DestinationServerName", "@DestinationUsername", "@DestinationPassword", "@DestinationTableName", "@DestinationDataSource", "@UseLookBack", "@LookbackColumnName" };

            SqlCommand comm = new SqlCommand();
            comm.CommandText = commandText;
            comm.Connection = conn;

            //Set all parameter names and initial values to n/a
            foreach (string item in textParameters)
            {
                comm.Parameters.AddWithValue(item, "N/A");
                comm.Parameters[item].SqlDbType = System.Data.SqlDbType.NVarChar;
            }

            if (DataProperties.ContainsKey("Lookback Period") && DataProperties["Lookback Option"].Equals("Yes"))
            {
            //set look back parameter to be type int
            comm.Parameters.AddWithValue("@LookBackPeriod", Convert.ToInt32(DataProperties["Lookback Period"]));
                comm.Parameters["@LookBackPeriod"].SqlDbType = System.Data.SqlDbType.Int;
            }
            else
            {
                //set look back to be default
                comm.Parameters.AddWithValue("@LookBackPeriod", 0);
                comm.Parameters["@LookBackPeriod"].SqlDbType = System.Data.SqlDbType.Int;
            }


            //Move through the Data Properties object and add the values to the parameters
            foreach (var item in DataProperties)
            {
                if (item.Key.Equals("Dataset Name"))
                {
                    comm.Parameters["@DataSetName"].Value = item.Value;
                }
                else if (item.Key.Equals("Import Type"))
                {
                    comm.Parameters["@ImportType"].Value = item.Value;
                }
                else if (item.Key.Equals("Source Database Type"))
                {
                    comm.Parameters["@SourceDBType"].Value = item.Value;
                }
                else if (item.Key.Equals("Source Table Name"))
                {
                    comm.Parameters["@SourceTableName"].Value = item.Value;
                }
                else if (item.Key.Equals("Source Database Name"))
                {
                    comm.Parameters["@SourceDatabase"].Value = item.Value;
                }
                else if (item.Key.Equals("Source Server Name"))
                {
                    comm.Parameters["@SourceServerName"].Value = item.Value;
                }
                else if (item.Key.Equals("Source Data Source"))
                {
                    comm.Parameters["@SourceDataSource"].Value = item.Value;
                }
                else if (item.Key.Equals("Source File Location"))
                {
                    comm.Parameters["@SourceFileLocation"].Value = item.Value;
                }
                else if (item.Key.Equals("Source Username"))
                {
                    comm.Parameters["@SourceUsername"].Value = item.Value;
                }
                else if (item.Key.Equals("Source Password"))
                {
                    comm.Parameters["@SourcePassword"].Value = item.Value;
                }
                else if (item.Key.Equals("Source File Name"))
                {
                    comm.Parameters["@SourceFileName"].Value = item.Value;
                }
                else if (item.Key.Equals("Source Delimiter"))
                {
                    comm.Parameters["@SourceDelimiter"].Value = item.Value;
                }
                else if (item.Key.Equals("Source Connection String"))
                {
                    comm.Parameters["@SourceConnectionString"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Type"))
                {
                    comm.Parameters["@DestinationType"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Data Source"))
                {
                    comm.Parameters["@DestinationDataSource"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Username"))
                {
                    comm.Parameters["@DestinationUsername"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Password"))
                {
                    comm.Parameters["@DestinationPassword"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Table Name"))
                {
                    comm.Parameters["@DestinationTableName"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Connection String"))
                {
                    comm.Parameters["@DestinationConnectionString"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Type"))
                {
                    comm.Parameters["@DestinationType"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Data Source"))
                {
                    comm.Parameters["@DestinationDataSource"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Username"))
                {
                    comm.Parameters["@DestinationUsername"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Password"))
                {
                    comm.Parameters["@DestinationPassword"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Table Name"))
                {
                    comm.Parameters["@DestinationTableName"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Connection String"))
                {
                    comm.Parameters["@DestinationConnectionString"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination File Location"))
                {
                    comm.Parameters["@DestinationFileLocation"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Database"))
                {
                    comm.Parameters["@DestinationDatabase"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Server Name"))
                {
                    comm.Parameters["@DestinationServerName"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination File Location"))
                {
                    comm.Parameters["@DestinationFileLocation"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Database"))
                {
                    comm.Parameters["@DestinationDatabase"].Value = item.Value;
                }
                else if (item.Key.Equals("Destination Server Name"))
                {
                    comm.Parameters["@DestinationServerName"].Value = item.Value;
                }
            }

            //set the parameters for lookback option and period
            if (DataProperties.ContainsKey("Lookback Option"))
            {
                comm.Parameters["@UseLookBack"].Value = DataProperties["Lookback Option"];
            }
            else
            {
                comm.Parameters["@UseLookBack"].Value = "No";
            }

            if (DataProperties.ContainsKey("Lookback Column"))
            {
                comm.Parameters["@LookbackColumnName"].Value = DataProperties["Lookback Column"];
            }
            else
            {
                comm.Parameters["@LookbackColumnName"].Value = "N/A";
            }


            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();
                lbl_complete.Text = "Complete";
            }
            catch (Exception error)
            {

                lbl_complete.Text = "Fail" + error.Message;
            }
        }

        //This method loads the metadata information entried by the user to the back end DB
        private void LoadMetaData(string processID)
        {
            string[] stringParametersForCommand = { "@ColumnName", "@DataType", "@NullsPermitted", "@NullAction", "@ReplaceValue" };
            string[] intParametersForCommand = { "@ProcessId", "@Min", "@Max", "@ColumnOrder" };
            string commandText = "INSERT INTO[dbo].[Program_Metadata]([ProcessId],[ColumnName],[DataType],[MinLength],[MaxLength],[NullsPermitted],[NullAction],[ReplaceValue],[ColumnOrder]) VALUES (@ProcessId, @ColumnName, @DataType, @Min, @Max, @NullsPermitted, @NullAction, @ReplaceValue, @ColumnOrder)";
            SqlCommand comm = new SqlCommand(commandText, conn);

            foreach (string item in stringParametersForCommand)
            {
                comm.Parameters.AddWithValue(item, "N/A");
                comm.Parameters[item].SqlDbType = System.Data.SqlDbType.NVarChar;
            }

            foreach (string item in intParametersForCommand)
            {
                comm.Parameters.AddWithValue(item, 0);
                comm.Parameters[item].SqlDbType = System.Data.SqlDbType.Int;
            }


            for (int i = 0; i < c_Names.Count; i++)
            {
                comm.Parameters["@ProcessId"].Value = processID;
                comm.Parameters["@ColumnName"].Value = c_Names[i];
                comm.Parameters["@DataType"].Value = c_DataTypes[i];
                comm.Parameters["@Min"].Value = c_MinLenth[i];
                comm.Parameters["@Max"].Value = c_MaxLength[i];
                comm.Parameters["@NullsPermitted"].Value = c_Nullable[i];
                comm.Parameters["@NullAction"].Value = c_NullAction[i];
                comm.Parameters["@ReplaceValue"].Value = c_ReplaceValue[i];
                comm.Parameters["@ColumnOrder"].Value = c_Order[i];

                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();
            }
        }

        //This method loads the primary key details entried by the user to the back end DB
        private void LoadPrimaryKeys(string processID)
        {
            string commandText = "INSERT INTO [dbo].[Program_PrimaryKeyData]([ProcessId],[PrimaryKey]) VALUES( @ProcessId, @PKValue)";
            SqlCommand comm = new SqlCommand(commandText, conn);

            comm.Parameters.AddWithValue("@ProcessId", Convert.ToInt32(processID));
            comm.Parameters["@ProcessId"].SqlDbType = System.Data.SqlDbType.Int;

            comm.Parameters.AddWithValue("@PKValue", "N/A");
            comm.Parameters["@PKValue"].SqlDbType = System.Data.SqlDbType.NVarChar;

            foreach (string item in lst_PrimaryKeys)
            {
                comm.Parameters["@PKValue"].Value = item;

                try
                {
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }


    }
}