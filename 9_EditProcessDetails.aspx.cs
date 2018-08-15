using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Data;

namespace ELTManagement
{
    public partial class _9_EditProcessDetails : System.Web.UI.Page
    {

        //string to hold the SQL Commands to update the Source and Destination details.
        string SQLSourceCommand;
        string SQLDestinationCommand;
        SqlConnection conn = new SqlConnection();

        //A list of Text boxes that will hold the Source information that will be used to provide
        //data for the update command 
        List<TextBox> SourceInformation = new List<TextBox>();
        List<TextBox> DestinationInformation = new List<TextBox>();

        string datasetName;
        string updateInfo;
        //variables to hold all of the import process details
        string DataSetName, ImportType, SourceDBType, SourceTableName, SourceDatabase, SourceServerName, SourceDataSource, SourceFileLocation, SourceUserName, SourcePassword, SourceConnectionString, SourceFileName, SourceDelimiter, DestinationType, DestinationFileLocation, DestinationConnectionString, DestinationDatabase, DestinationServerName, DestinationUsername, DestinationPassword, DestinationTableName, DestinationDataSource, UseLookBack;

        int ProcessId;

        //List of possible column to be selected for look back filter
        List<string> listOfLookBackColumns = new List<string>();

        protected void GridViewMetaData_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }

        protected void GridViewPrimaryKey_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["BackEndDB"].ConnectionString;

            datasetName = (string)Session["datasetName"];
            ProcessId = (int)Session["ProcessId"];
            string[] ImportTypeValues = { "Direct Connect", "Feed File" };

            LoadDatasetDetails(datasetName);
            SetSourceDetails();
            SetDestinationDetails();

            if (!IsPostBack)
            {
                PopulateMetaDataGrid();
                PopulatePKDataGrid();
                SetAddNewMetaDataTable();
                SetLookBackDetails();
            }

        }

        protected void Update_LookBack(object sender, EventArgs e)
        {
            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;

            string useLookBack,lookBackColumn;
            int lookBackPeriod;

            if (chk_UseLookBack.Checked)
            {
                useLookBack = "Yes";
                lookBackColumn = drpLookBackColumns.SelectedValue.ToString();
                lookBackPeriod = Convert.ToInt32(txt_lookBackPeriod.Text);
            }
            else
            {
                useLookBack = "No";
                lookBackColumn = "N/A";
                lookBackPeriod = 0;
            }

            string commandText = "UPDATE [dbo].[Program_DataProperties] SET UseLookBack = @UseLookback, LookBackPeriod = @LookBackPeriod, LookBackColumnName = @LookBackColumnName WHERE ProcessId = @ProcessId";
            comm.Parameters.AddWithValue("@ProcessId", ProcessId);
            comm.Parameters.AddWithValue("@UseLookback", useLookBack);

            comm.Parameters.AddWithValue("@LookBackPeriod", lookBackPeriod);
            comm.Parameters["@LookBackPeriod"].SqlDbType = SqlDbType.Int;

            comm.Parameters.AddWithValue("@LookBackColumnName", lookBackColumn);

            comm.CommandText = commandText;

            try
            {
                conn.Open();

                comm.ExecuteNonQuery();

                conn.Close();

                updateInfo = "Record updated successfully";
            }
            catch (Exception ex)
            {
                updateInfo = "Error" + ex.Message;
                throw;
            }


            Session["UpdateInfo"] = updateInfo;
            Response.Redirect("9_UpdateDataPropSelection.aspx");
        }

        private void SetLookBackDetails()
        {
            //populate the drop down with possible look back column names
            PopulateLookBackDropDown();

            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;
            comm.CommandText = "SELECT UseLookBack,LookBackPeriod,LookBackColumnName FROM Program_DataProperties WHERE ProcessId = @ProcessId";
            comm.Parameters.AddWithValue("@ProcessId", ProcessId);

            string lookBackColumn;


            try
            {
            conn.Open();
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
            {
                if (reader["UseLookBack"].Equals("Yes"))
                {

                    chk_UseLookBack.Checked = true;
                    txt_lookBackPeriod.Text = reader["LookBackPeriod"].ToString();
                    lookBackColumn = reader["LookBackColumnName"].ToString();

                    if (listOfLookBackColumns.Contains(lookBackColumn))
                    {
                    drpLookBackColumns.SelectedValue = lookBackColumn;
                    }
                    else
                    {
                        drpLookBackColumns.SelectedIndex = 0;
                    }

                }
            }
            conn.Close();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }



        }

        private void PopulateLookBackDropDown()
        {

            listOfLookBackColumns.Add("");
            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;
            comm.CommandText = "SELECT ColumnName FROM [Program_Metadata] WHERE DataType = 'Date' and ProcessId = @ProcessId";
            comm.Parameters.AddWithValue("@ProcessId", ProcessId);

            //populate the drop down with possible look back column names

            try
            {
                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    
                        listOfLookBackColumns.Add(reader["ColumnName"].ToString());
                }

                drpLookBackColumns.DataSource = listOfLookBackColumns;
                drpLookBackColumns.DataBind();
                conn.Close();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }


        }

        #region PrimarKey GridViewCode
        private void PopulatePKDataGrid()
        {
            string commandText = "SELECT [ProcessId], [PrimaryKey] FROM Program_PrimaryKeyData WHERE ProcessId = @ProcessId";
            SqlCommand comm = new SqlCommand(commandText, conn);
            comm.Parameters.AddWithValue("@ProcessId", ProcessId);
            SqlDataAdapter da = new SqlDataAdapter(comm);
            DataSet ds = new DataSet();
            conn.Open();
            da.Fill(ds);
            conn.Close();

            GridViewPrimaryKey.DataSource = ds;
            GridViewPrimaryKey.DataBind();
        }

        protected void GridViewPK_PageIndexChanging(object sender, EventArgs e)
        {

        }

        protected void GridViewPK_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int processid = Convert.ToInt32(GridViewMetaData.DataKeys[e.RowIndex].Value.ToString());

            GridViewRow currentRow = (GridViewRow)GridViewMetaData.Rows[e.RowIndex];
            Label lbldeleteid = (Label)currentRow.FindControl("lblID");

            SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[Program_PrimaryKeyData] WHERE ProcessId = @ProcessId", conn);
            cmd.Parameters.AddWithValue("@ProcessId", processid);
            cmd.Parameters["@ProcessId"].SqlDbType = SqlDbType.Int;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                updateInfo = "Record Successfully Deleted";
            }
            catch (Exception ex)
            {

                updateInfo = "Error " + ex.Message;
            }

            //Redirect to selection page.
            Session["UpdateInfo"] = updateInfo;
            Response.Redirect("9_UpdateDataPropSelection.aspx");
        }


        protected void GridViewPK_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }



        #endregion

        #region MetaData GridViewCode
        private void PopulateMetaDataGrid()
        {
            string commandText = "SELECT [ProcessId],[ColumnName],[DataType],[MinLength],[MaxLength],[NullsPermitted],[NullAction],[ReplaceValue],[ColumnOrder] FROM [Project].[dbo].[Program_Metadata] WHERE ProcessId = @ProcessId order by ColumnOrder asc;";
            SqlCommand comm = new SqlCommand(commandText, conn);
            comm.Parameters.AddWithValue("@ProcessId", ProcessId);
            SqlDataAdapter da = new SqlDataAdapter(comm);
            DataSet ds = new DataSet();
            conn.Open();
            da.Fill(ds);
            conn.Close();

            GridViewMetaData.DataSource = ds;
            GridViewMetaData.DataBind();
        }

        protected void GridViewMetaData_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void GridViewMetaData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int processid = Convert.ToInt32(GridViewMetaData.DataKeys[e.RowIndex].Value.ToString());
            //the primary key for the MetaData Table is a composite key (ProcessID and ColumnName)
            //This method will pull a list of all original Column Name values

            List<string> OriginalColumNames = GetOriginalColumnNames(processid);

            GridViewRow currentRow = (GridViewRow)GridViewMetaData.Rows[e.RowIndex];
            Label lbldeleteid = (Label)currentRow.FindControl("lblID");
            
            SqlCommand cmd = new SqlCommand("DELETE FROM [Project].[dbo].[Program_Metadata] where ProcessId = @ProcessId AND ColumnName = @ColumnName", conn);
            cmd.Parameters.AddWithValue("@ProcessId", processid);
            cmd.Parameters["@ProcessId"].SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@ColumnName", OriginalColumNames[e.RowIndex]);

            //Also need to issue command to delete from the PrimaryKey Table if is a primary Key;

            cmd.CommandText = "DELETE FROM Program_Metadata where ProcessId = @ProcessId AND ColumnName = @ColumnName";

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                updateInfo = "Record Successfully Deleted";

            }
            catch (Exception ex)
            {

                updateInfo = "Error " + ex.Message;
            }


            //Redirect to selection page.
            Session["UpdateInfo"] = updateInfo;
            Response.Redirect("9_UpdateDataPropSelection.aspx");

        }
        protected void GridViewMetaData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewMetaData.EditIndex = e.NewEditIndex;
            PopulateMetaDataGrid();

        }
        protected void GridViewMetaData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            int processid = Convert.ToInt32(GridViewMetaData.DataKeys[e.RowIndex].Value.ToString());
            //the primary key for the MetaData Table is a composite key (ProcessID and ColumnName)
            //This method will pull a list of all original Column Name values

            List<string> OriginalColumNames = GetOriginalColumnNames(processid);


            GridViewRow row = (GridViewRow)GridViewMetaData.Rows[e.RowIndex];
            Label lblID = (Label)row.FindControl("lblID");
            //TextBox txtname=(TextBox)gr.cell[].control[];  
            TextBox textColumnName = (TextBox)row.Cells[0].Controls[0];
            TextBox textDataType = (TextBox)row.Cells[1].Controls[0];
            TextBox txt_minLength = (TextBox)row.Cells[2].Controls[0];
            TextBox txt_maxLength = (TextBox)row.Cells[3].Controls[0];
            TextBox txt_NullsPermitted = (TextBox)row.Cells[4].Controls[0];
            TextBox txt_NullAction = (TextBox)row.Cells[5].Controls[0];
            TextBox txt_ReplaceValue = (TextBox)row.Cells[6].Controls[0];
            TextBox txt_ColumnOrder = (TextBox)row.Cells[7].Controls[0];

            GridViewMetaData.EditIndex = -1;
            conn.Open();
            SqlCommand cmd = new SqlCommand("update [dbo].[Program_Metadata] set ColumnName='" + textColumnName.Text + "',DataType='" + textDataType.Text + "',MinLength='" + Convert.ToInt32(txt_minLength.Text) + "',MaxLength='" + Convert.ToInt32(txt_maxLength.Text) + "',NullsPermitted = '" + txt_NullsPermitted.Text + "',NullAction = '" + txt_NullAction.Text + "',ReplaceValue = '" + txt_ReplaceValue.Text + "',ColumnOrder = '" + Convert.ToInt32(txt_ColumnOrder.Text) + "' where processid='" + processid + "' and ColumnName = '" + OriginalColumNames[e.RowIndex] + "'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            PopulateMetaDataGrid();
        }

        private List<string> GetOriginalColumnNames(int processid)
        {
            string commandText = "select ColumnName from [dbo].[Program_Metadata] where ProcessId = '" + processid + "' order by ColumnOrder asc; ";
            List<string> columns = new List<string>();
            SqlCommand comm = new SqlCommand(commandText, conn);

            try
            {
                conn.Open();
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(reader["ColumnName"].ToString());
                    }
                }
                conn.Close();
            }
            catch (Exception)
            {

                throw;
            }

            return columns;
        }

        protected void GridViewMetaData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewMetaData.PageIndex = e.NewPageIndex;
            PopulateMetaDataGrid();

        }
        protected void GridViewMetaData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewMetaData.EditIndex = -1;
            PopulateMetaDataGrid();
        }

        protected void Add_MetaData_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            string commandText;
            int count = 0;
            string errorText = "";
            //This method will add a new record to the Meta Data Type
            //If the record is a Primary Key it will also add it to Primary Key Table

            //Step One Add to Meta Data Table
            string columnName;
            columnName = txt_ColumnName.Text;

            //Step two check that the column Name doesn't already exist

            commandText = "SELECT COUNT(*) FROM [Project].[dbo].[Program_Metadata] WHERE ProcessId = @ProcessId and ColumnName = @ColumnName";
            cmd.CommandText = commandText;
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@ProcessId", ProcessId);
            cmd.Parameters["@ProcessId"].SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@ColumnName", columnName);
            conn.Open();
            count = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();

            if (count > 0)
            {
                //The column already exists issue a warning
                errorText = "Column Name already exists for this import process";
                Lbl_AddMetaError.Text = errorText;
            }
            else
            {
                //else enter the column to the Meta Data Table and Primary Key Table
                commandText = "INSERT INTO Program_Metadata (ProcessId, ColumnName, DataType, MinLength, MaxLength, NullsPermitted, NullAction, ReplaceValue, ColumnOrder) VALUES(@ProcessId, @ColumnName, @DataType, @MinLength, @MaxLength, @NullsPermitted, @NullAction, @ReplaceValue, @ColumnOrder)";
                cmd.CommandText = commandText;
                cmd.Parameters.AddWithValue("@DataType", drp_DataType.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@MinLength", Convert.ToInt32(txt_MinLength.Text));
                cmd.Parameters.AddWithValue("@MaxLength", Convert.ToInt32(txt_MaxLength.Text));
                cmd.Parameters.AddWithValue("@NullsPermitted", drp_NullPermitted.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@NullAction", drp_NullAction.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@ReplaceValue", txt_ReplaceValue.Text);
                cmd.Parameters.AddWithValue("@ColumnOrder", Convert.ToInt32(txt_ColumnOrder.Text));

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    updateInfo = "Record successfully added";
                }
                catch (Exception ex)
                {

                    updateInfo = ex.Message;
                }

                //step three if the option of Primary Key is selected the column needs to be added to the primary key table
                if (check_PrimaryKey.Checked)
                {
                    UpdatePrimaryKeyRecord(cmd);
                }
                
            }


            //Redirect to selection page.
            Session["UpdateInfo"] = updateInfo;
            Response.Redirect("9_UpdateDataPropSelection.aspx");

        }

        private void UpdatePrimaryKeyRecord(SqlCommand command)
        {
            SqlCommand cmd = command;
            cmd.Connection = conn;
            int count;
            string errorText;

            cmd.CommandText = "SELECT COUNT(*) FROM [Project].[dbo].[Program_PrimaryKeyData] WHERE ProcessId = @ProcessId AND PrimaryKey = @ColumnName";
            count = 0;

            conn.Open();
            count = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();


            if (count == 0)
            {
                //Enter Record to Primary Key Table
                cmd.CommandText = "  INSERT INTO [Project].[dbo].[Program_PrimaryKeyData] (ProcessId, PrimaryKey) VALUES (@ProcessId, @ColumnName)";

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception)
                {

                    throw;
                }


            }
            else
            {
                //Else issue a warning
                errorText = "Primary Key already exists for this import process";
                Lbl_PrimaryKeyError.Text = errorText;
            }
        }

        private void SetAddNewMetaDataTable()
        {
            //Set the default values for min and max length
            txt_MinLength.Text = "0";
            txt_MaxLength.Text = "50";

            //Set the options for the drop down data type
            List<string> dataTypes = new List<string>();
            dataTypes.Add("Text");
            dataTypes.Add("Int");
            dataTypes.Add("Decimal");
            dataTypes.Add("Date");
            drp_DataType.DataSource = dataTypes;
            drp_DataType.DataBind();

            //Set the options for the drop down Nulls Permitted
            List<string> options = new List<string>();
            options.Add("Yes");
            options.Add("No");
            drp_NullPermitted.DataSource = options;
            drp_NullPermitted.DataBind();

            //Set the values for the drop down Null action
            List<string> actions = new List<string>();
            actions.Add("Reject");
            actions.Add("Replace");
            actions.Add("Accept");
            drp_NullAction.DataSource = actions;
            drp_NullAction.DataBind();

        }
        #endregion

        protected void Update_Click(object sender, EventArgs e)
        {
            DataSetName = SourceInformation[0].Text;

            if (ImportType.Equals("Feed File"))
            {
                SourceFileName = SourceInformation[1].Text;
                SourceFileLocation = SourceInformation[2].Text;
                SourceDelimiter = SourceInformation[3].Text;
                SQLSourceCommand = "UPDATE [dbo].[Program_DataProperties] SET DataSetName = @DataSetName, SourceFileLocation = @SourceFileLocation, SourceFileName = @SourceFileName, SourceDelimiter = @SourceDelimiter WHERE ProcessId = @ProcessID";
            }

            else if (ImportType.Equals("Direct Connect"))
            {
                SourceUserName = SourceInformation[1].Text;
                SourcePassword = SourceInformation[2].Text;
                SourceConnectionString = SourceInformation[3].Text;
                SourceTableName = SourceInformation[4].Text;

                if (SourceDBType.Equals("SQL Server"))
                {
                    SourceDatabase = SourceInformation[5].Text;
                    SourceServerName = SourceInformation[6].Text;

                    SQLSourceCommand = "UPDATE [dbo].[Program_DataProperties] SET SourceUserName = @SourceUserName, SourcePassword = @SourcePassword, SourceConnectionString = @SourceConnectionString, SourceTableName = @SourceTableName, SourceDatabase = @SourceDatabase, SourceServerName = @SourceServerName WHERE ProcessId = @ProcessID";

                }
                else if (SourceDBType.Equals("Oracle Database"))
                {
                    SourceDataSource = SourceInformation[5].Text;

                    SQLSourceCommand = "UPDATE [dbo].[Program_DataProperties] SET SourceUserName = @SourceUserName, SourcePassword = @SourcePassword, SourceConnectionString = @SourceConnectionString, SourceTableName = @SourceTableName, SourceDataSource = @SourceDataSource WHERE ProcessId = @ProcessID";
                }
                else if (SourceDBType.Equals("Access Database"))
                {
                    SourceFileLocation = SourceInformation[5].Text;

                    SQLSourceCommand = "UPDATE [dbo].[Program_DataProperties] SET SourceUserName = @SourceUserName, SourcePassword = @SourcePassword, SourceConnectionString = @SourceConnectionString, SourceTableName = @SourceTableName, SourceFileLocation = @SourceFileLocation WHERE ProcessId = @ProcessID";
                }
            }

            DestinationConnectionString = DestinationInformation[0].Text;
            DestinationTableName = DestinationInformation[1].Text;
            DestinationUsername = DestinationInformation[2].Text;
            DestinationPassword = DestinationInformation[3].Text;

            if (DestinationType.Equals("SQL Server"))
            {
                DestinationServerName = DestinationInformation[4].Text;
                DestinationDatabase = DestinationInformation[5].Text;
                SQLDestinationCommand = "UPDATE [dbo].[Program_DataProperties] SET DestinationConnectionString = @DestinationConnectionString, DestinationTableName = @DestinationTableName, DestinationUsername = @DestinationUsername, DestinationPassword = @DestinationPassword, DestinationServerName = @DestinationServerName, DestinationDatabase = @DestinationDatabase WHERE ProcessId = @ProcessID";
            }
            else if (DestinationType.Equals("Oracle Database"))
            {
                DestinationDataSource = DestinationInformation[4].Text;
                SQLDestinationCommand = "UPDATE [dbo].[Program_DataProperties] SET DestinationConnectionString = @DestinationConnectionString, DestinationTableName = @DestinationTableName, DestinationUsername = @DestinationUsername, DestinationPassword = @DestinationPassword, DestinationDataSource = @DestinationDataSource WHERE ProcessId = @ProcessID";

            }
            else if (DestinationType.Equals("Access Database"))
            {
                DestinationFileLocation = DestinationInformation[4].Text;
                SQLDestinationCommand = "UPDATE [dbo].[Program_DataProperties] SET DestinationConnectionString = @DestinationConnectionString, DestinationTableName = @DestinationTableName, DestinationUsername = @DestinationUsername, DestinationPassword = @DestinationPassword, DestinationFileLocation = @DestinationFileLocation WHERE ProcessId = @ProcessID";
            }

            //Execute Source Update
            SourceUpdate();
            //Execute Destination Update
            DestinationUpdate();

            Session["UpdateInfo"] = updateInfo;
            Response.Redirect("9_UpdateDataPropSelection.aspx");
        }

        //This method connnects to the Back End DB and pulls down the data for the import process.
        private void LoadDatasetDetails(string datasetName)
        {
            string commandText = "SELECT [ProcessId],[DataSetName],[ImportType],[SourceDBType],[SourceTableName],[SourceDatabase],[SourceServerName],[SourceDataSource],[SourceFileLocation],[SourceUserName],[SourcePassword],[SourceConnectionString],[SourceFileName],[SourceDelimiter],[DestinationType],[DestinationFileLocation],[DestinationConnectionString],[DestinationDatabase],[DestinationServerName],[DestinationUsername],[DestinationPassword],[DestinationTableName],[DestinationDataSource],[UseLookBack],[LookBackPeriod] FROM [dbo].[Program_DataProperties] WHERE DataSetName = '" + datasetName + "'";
            SqlConnection conn = new SqlConnection(@"Data Source=WINDOWS-I92V0KI\SQLEXPRESS;Initial Catalog=Project;Integrated Security=True;");
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
                            DataSetName = reader["DataSetName"].ToString();
                            ImportType = reader["ImportType"].ToString();

                            SourceDBType = reader["SourceDBType"].ToString();
                            SourceTableName = reader["SourceTableName"].ToString();
                            SourceDatabase = reader["SourceDatabase"].ToString();
                            SourceServerName = reader["SourceServerName"].ToString();
                            SourceDataSource = reader["SourceDataSource"].ToString();
                            SourceFileLocation = reader["SourceFileLocation"].ToString();
                            SourceUserName = reader["SourceUserName"].ToString();
                            SourcePassword = reader["SourcePassword"].ToString();
                            SourceConnectionString = reader["SourceConnectionString"].ToString();
                            SourceFileName = reader["SourceFileName"].ToString();
                            SourceDelimiter = reader["SourceDelimiter"].ToString();

                            DestinationType = reader["DestinationType"].ToString();
                            DestinationConnectionString = reader["DestinationConnectionString"].ToString();
                            DestinationTableName = reader["DestinationTableName"].ToString();
                            DestinationUsername = reader["DestinationUsername"].ToString();
                            DestinationPassword = reader["DestinationPassword"].ToString();
                            DestinationType = reader["DestinationType"].ToString();
                            DestinationDatabase = reader["DestinationDatabase"].ToString();
                            DestinationServerName = reader["DestinationServerName"].ToString();
                            DestinationDataSource = reader["DestinationDataSource"].ToString();
                            DestinationFileLocation = reader["DestinationFileLocation"].ToString();
                        }
                    }

                    conn.Close();
                }
            }

            //Import Type will be read only. If the user has to reset the import type, it would be best to delete the entire import
            //process and create a new one.
            Lbl_ImportType.Text = ImportType;
        }

        //This method creates a table of source data that can be edited by the user
        private void SetSourceDetails()
        {

            if (ImportType.Equals("Feed File"))
            {
                Label Lbl_SourceFileName = new Label();
                Label Lbl_SourceFileLocation = new Label();
                Label Lbl_SourceDelimiter = new Label();
                Label Lbl_DataSetName = new Label();
                Lbl_DataSetName.Text = "Dataset Name:";
                Lbl_SourceFileName.Text = "Source File Name:";
                Lbl_SourceFileLocation.Text = "Source File Location:";
                Lbl_SourceDelimiter.Text = "Source Delimiter Character:";
                TextBox txt_SourceFileName = new TextBox();
                TextBox txt_SourceFileLocation = new TextBox();
                TextBox txt_SourceDelimiter = new TextBox();
                TextBox txt_DataSetName = new TextBox();
                txt_DataSetName.Text = DataSetName;
                txt_SourceFileName.Text = SourceFileName;
                txt_SourceFileLocation.Text = SourceFileLocation;
                txt_SourceDelimiter.Text = SourceDelimiter;

                SourceDetailsPanel.Controls.Add(new LiteralControl("<table><tr><td>"));
                SourceDetailsPanel.Controls.Add(Lbl_DataSetName);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td><td>"));
                SourceDetailsPanel.Controls.Add(txt_DataSetName);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td></tr>"));
                SourceDetailsPanel.Controls.Add(new LiteralControl("<tr><td>"));
                SourceDetailsPanel.Controls.Add(Lbl_SourceFileName);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td><td>"));
                SourceDetailsPanel.Controls.Add(txt_SourceFileName);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td></tr>"));
                SourceDetailsPanel.Controls.Add(new LiteralControl("<tr><td>"));
                SourceDetailsPanel.Controls.Add(Lbl_SourceFileLocation);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td><td>"));
                SourceDetailsPanel.Controls.Add(txt_SourceFileLocation);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td></tr>"));
                SourceDetailsPanel.Controls.Add(new LiteralControl("<tr><td>"));
                SourceDetailsPanel.Controls.Add(Lbl_SourceDelimiter);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td><td>"));
                SourceDetailsPanel.Controls.Add(txt_SourceDelimiter);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td></tr></table>"));

                //Add the textboxes to the list of text boxes that will be used to provide
                //data for the update command
                SourceInformation.Add(txt_DataSetName);
                SourceInformation.Add(txt_SourceFileName);
                SourceInformation.Add(txt_SourceFileLocation);
                SourceInformation.Add(txt_SourceDelimiter);

                foreach (TextBox item in SourceInformation)
                {
                    item.Width = 350;//Increase the size of the textbox
                }
            }
            else if (ImportType.Equals("Direct Connect"))
            {
                Label Lbl_SourceDBType = new Label();
                Label Lbl_SourceTableName = new Label();
                Label Lbl_SourceUserName = new Label();
                Label Lbl_SourcePassword = new Label();
                Label Lbl_SourceConnectionString = new Label();
                Lbl_SourceDBType.Text = "Source Database Type";
                Lbl_SourceUserName.Text = "Source Username:";
                Lbl_SourcePassword.Text = "Source Password:";
                Lbl_SourceTableName.Text = "Source Table Name";
                Lbl_SourceConnectionString.Text = "Source Connection String:";
                TextBox txt_SourceDBType = new TextBox();
                TextBox txt_SourceUsername = new TextBox();
                TextBox txt_SourcePassword = new TextBox();
                TextBox txt_SourceTableName = new TextBox();
                TextBox txt_SourceConnectionString = new TextBox();
                txt_SourceDBType.Text = SourceDBType;
                txt_SourceUsername.Text = SourceUserName;
                txt_SourcePassword.Text = SourcePassword;
                txt_SourceTableName.Text = SourceTableName;
                txt_SourceConnectionString.Text = SourceConnectionString;

                SourceInformation.Add(txt_SourceDBType); //0
                SourceInformation.Add(txt_SourceUsername); //1
                SourceInformation.Add(txt_SourcePassword); //2
                SourceInformation.Add(txt_SourceConnectionString); //3
                SourceInformation.Add(txt_SourceTableName); //4

                //Create the table to hold the source details
                SourceDetailsPanel.Controls.Add(new LiteralControl("<table><tr><td>"));

                if (SourceDBType.Equals("SQL Server"))
                {
                    //Add the Labels and Textboxes for SQL Server Database - Database and server name.
                    Label Lbl_SourceDatabaseName = new Label();
                    Label Lbl_SourceServerName = new Label();

                    Lbl_SourceDatabaseName.Text = "Source Database Name:";
                    Lbl_SourceServerName.Text = "Source Server Name:";

                    TextBox txt_SourceDatabaseName = new TextBox();
                    TextBox txt_SourceServerName = new TextBox();

                    txt_SourceDatabaseName.Text = SourceDatabase;
                    txt_SourceServerName.Text = SourceServerName;


                    SourceInformation.Add(txt_SourceDatabaseName); // 5
                    SourceInformation.Add(txt_SourceServerName); //6

                    foreach (TextBox item in SourceInformation)
                    {
                        item.Width = 350;//Increase the size of the textbox
                    }

                    SourceDetailsPanel.Controls.Add(new LiteralControl("<tr><td>"));
                    SourceDetailsPanel.Controls.Add(Lbl_SourceDatabaseName);
                    SourceDetailsPanel.Controls.Add(new LiteralControl("</td><td>"));
                    SourceDetailsPanel.Controls.Add(txt_SourceDatabaseName);
                    SourceDetailsPanel.Controls.Add(new LiteralControl("</td></tr>"));
                    SourceDetailsPanel.Controls.Add(new LiteralControl("<tr><td>"));
                    SourceDetailsPanel.Controls.Add(Lbl_SourceServerName);
                    SourceDetailsPanel.Controls.Add(new LiteralControl("</td><td>"));
                    SourceDetailsPanel.Controls.Add(txt_SourceServerName);
                    SourceDetailsPanel.Controls.Add(new LiteralControl("</td></tr>"));


                }
                else if (SourceDBType.Equals("Oracle Database"))
                {
                    //Add the Labels and Textboxes for Oracle Database
                    //Add the Labels and Textboxes for SQL Server Database - Database and server name.
                    Label Lbl_SourceDataSource = new Label();

                    Lbl_SourceDataSource.Text = "Source Data Source:";

                    TextBox txt_SourceDataSource = new TextBox();

                    txt_SourceDataSource.Text = SourceDataSource;

                    SourceInformation.Add(txt_SourceDataSource); // 5

                    foreach (TextBox item in SourceInformation)
                    {
                        item.Width = 350;//Increase the size of the textbox
                    }

                    SourceDetailsPanel.Controls.Add(new LiteralControl("<tr><td>"));
                    SourceDetailsPanel.Controls.Add(Lbl_SourceDataSource);
                    SourceDetailsPanel.Controls.Add(new LiteralControl("</td><td>"));
                    SourceDetailsPanel.Controls.Add(txt_SourceDataSource);
                    SourceDetailsPanel.Controls.Add(new LiteralControl("</td></tr>"));

                }
                else if (SourceDBType.Equals("Access Database"))
                {
                    //Add the Labels and Textboxes for Access Database

                    Label Lbl_SourceFileLocation = new Label();

                    Lbl_SourceFileLocation.Text = "Source File Location:";

                    TextBox txt_SourceFileLocation = new TextBox();

                    txt_SourceFileLocation.Text = SourceFileLocation;

                    SourceInformation.Add(txt_SourceFileLocation); // 5

                    foreach (TextBox item in SourceInformation)
                    {
                        item.Width = 350;//Increase the size of the textbox
                    }

                    SourceDetailsPanel.Controls.Add(new LiteralControl("<tr><td>"));
                    SourceDetailsPanel.Controls.Add(Lbl_SourceFileLocation);
                    SourceDetailsPanel.Controls.Add(new LiteralControl("</td><td>"));
                    SourceDetailsPanel.Controls.Add(txt_SourceFileLocation);
                    SourceDetailsPanel.Controls.Add(new LiteralControl("</td></tr>"));

                }

                foreach (TextBox item in SourceInformation)
                {
                    item.Width = 350;//Increase the size of the textbox
                }

                //Add the username, password, table and connection string details
                SourceDetailsPanel.Controls.Add(new LiteralControl("<tr><td>"));
                SourceDetailsPanel.Controls.Add(Lbl_SourceDBType);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td><td>"));
                SourceDetailsPanel.Controls.Add(txt_SourceDBType);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td></tr>"));
                SourceDetailsPanel.Controls.Add(new LiteralControl("<tr><td>"));
                SourceDetailsPanel.Controls.Add(Lbl_SourceTableName);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td><td>"));
                SourceDetailsPanel.Controls.Add(txt_SourceTableName);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td></tr>"));
                SourceDetailsPanel.Controls.Add(new LiteralControl("<tr><td>"));
                SourceDetailsPanel.Controls.Add(Lbl_SourceUserName);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td><td>"));
                SourceDetailsPanel.Controls.Add(txt_SourceUsername);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td></tr>"));
                SourceDetailsPanel.Controls.Add(new LiteralControl("<tr><td>"));
                SourceDetailsPanel.Controls.Add(Lbl_SourcePassword);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td><td>"));
                SourceDetailsPanel.Controls.Add(txt_SourcePassword);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td></tr>"));
                SourceDetailsPanel.Controls.Add(new LiteralControl("<tr><td>"));
                SourceDetailsPanel.Controls.Add(Lbl_SourceConnectionString);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td><td>"));
                SourceDetailsPanel.Controls.Add(txt_SourceConnectionString);
                SourceDetailsPanel.Controls.Add(new LiteralControl("</td></tr></table>"));
            }

        }

        //This method creates a table of destination data that can be edited by the user
        private void SetDestinationDetails()
        {
            Label Lbl_DestinationTableName = new Label();
            Label Lbl_DestinationUsername = new Label();
            Label Lbl_DestinationPassword = new Label();
            Label Lbl_ConnectionString = new Label();
            Lbl_DestinationTableName.Text = "Destination Table Name:";
            Lbl_DestinationUsername.Text = "Destination Username:";
            Lbl_DestinationPassword.Text = "DestinationPassword:";
            Lbl_ConnectionString.Text = "Destination Connection String:";
            TextBox txt_DestinationTableName = new TextBox();
            TextBox txt_DestinationUsername = new TextBox();
            TextBox txt_DestinationPassword = new TextBox();
            TextBox txt_DestinationConnectionString = new TextBox();
            txt_DestinationTableName.Text = DestinationTableName;
            txt_DestinationUsername.Text = DestinationUsername;
            txt_DestinationPassword.Text = DestinationPassword;
            txt_DestinationConnectionString.Text = DestinationConnectionString;
            //Add the textboxes to a list of text boxes holding the destination information

            DestinationInformation.Add(txt_DestinationConnectionString);//0
            DestinationInformation.Add(txt_DestinationTableName);//1
            DestinationInformation.Add(txt_DestinationUsername);//2
            DestinationInformation.Add(txt_DestinationPassword);//3

            DestinationDetailsPanel.Controls.Add(new LiteralControl("<table><tr><td>"));

            if (DestinationType.Equals("SQL Server"))
            {
                //Add the Labels and Textboxes for SQL Server Database
                Label Lbl_DestinationServerName = new Label();
                Label Lbl_DestinationDatabasename = new Label();
                Lbl_DestinationServerName.Text = "Destination Server Name:";
                Lbl_DestinationDatabasename.Text = "Destination Database Name:";
                TextBox txt_DestinationServerName = new TextBox();
                TextBox txt_DestinationDatabaseName = new TextBox();

                txt_DestinationDatabaseName.Text = DestinationDatabase;
                txt_DestinationServerName.Text = DestinationServerName;
                DestinationInformation.Add(txt_DestinationServerName); //4
                DestinationInformation.Add(txt_DestinationDatabaseName);//5

                foreach (TextBox item in DestinationInformation)
                {
                    item.Width = 350;
                }

                DestinationDetailsPanel.Controls.Add(new LiteralControl("<tr>"));
                DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
                DestinationDetailsPanel.Controls.Add(Lbl_DestinationServerName);
                DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
                DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
                DestinationDetailsPanel.Controls.Add(txt_DestinationServerName);
                DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
                DestinationDetailsPanel.Controls.Add(new LiteralControl("</tr>"));

                DestinationDetailsPanel.Controls.Add(new LiteralControl("<tr>"));
                DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
                DestinationDetailsPanel.Controls.Add(Lbl_DestinationDatabasename);
                DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
                DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
                DestinationDetailsPanel.Controls.Add(txt_DestinationDatabaseName);
                DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
                DestinationDetailsPanel.Controls.Add(new LiteralControl("</tr>"));

            }
            else if (DestinationType.Equals("Oracle Database"))
            {
                //Add the Labels and Textboxes for Oracle Database
                Label Lbl_DestinationDataSource = new Label();

                Lbl_DestinationDataSource.Text = "Destination Data Source:";

                TextBox txt_DestinationDataSource = new TextBox();

                txt_DestinationDataSource.Text = DestinationDataSource;

                DestinationInformation.Add(txt_DestinationDataSource);//4

                foreach (TextBox item in DestinationInformation)
                {
                    item.Width = 350;
                }

                DestinationDetailsPanel.Controls.Add(new LiteralControl("<tr>"));
                DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
                DestinationDetailsPanel.Controls.Add(Lbl_DestinationDataSource);
                DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
                DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
                DestinationDetailsPanel.Controls.Add(txt_DestinationDataSource);
                DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
                DestinationDetailsPanel.Controls.Add(new LiteralControl("</tr>"));

            }
            else if (DestinationType.Equals("Access Database"))
            {
                //Add the Labels and Textboxes for Access Database

                Label Lbl_DestinationFileLocation = new Label();

                Lbl_DestinationFileLocation.Text = "Destination File Location:";

                TextBox txt_DestinationFileLocation = new TextBox();

                txt_DestinationFileLocation.Text = DestinationFileLocation; //4

                DestinationInformation.Add(txt_DestinationFileLocation);

                foreach (TextBox item in DestinationInformation)
                {
                    item.Width = 350;
                }

                DestinationDetailsPanel.Controls.Add(new LiteralControl("<tr>"));
                DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
                DestinationDetailsPanel.Controls.Add(Lbl_DestinationFileLocation);
                DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
                DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
                DestinationDetailsPanel.Controls.Add(txt_DestinationFileLocation);
                DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
                DestinationDetailsPanel.Controls.Add(new LiteralControl("</tr>"));

            }

            foreach (TextBox item in DestinationInformation)
            {
                item.Width = 350;
            }

            //Add the Labels and Textboxes for TableName, Username, password and connection string
            DestinationDetailsPanel.Controls.Add(new LiteralControl("<tr>"));
            DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
            DestinationDetailsPanel.Controls.Add(Lbl_DestinationTableName);
            DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
            DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
            DestinationDetailsPanel.Controls.Add(txt_DestinationTableName);
            DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
            DestinationDetailsPanel.Controls.Add(new LiteralControl("</tr>"));

            DestinationDetailsPanel.Controls.Add(new LiteralControl("<tr>"));
            DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
            DestinationDetailsPanel.Controls.Add(Lbl_DestinationUsername);
            DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
            DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
            DestinationDetailsPanel.Controls.Add(txt_DestinationUsername);
            DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
            DestinationDetailsPanel.Controls.Add(new LiteralControl("</tr>"));

            DestinationDetailsPanel.Controls.Add(new LiteralControl("<tr>"));
            DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
            DestinationDetailsPanel.Controls.Add(Lbl_DestinationPassword);
            DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
            DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
            DestinationDetailsPanel.Controls.Add(txt_DestinationPassword);
            DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
            DestinationDetailsPanel.Controls.Add(new LiteralControl("</tr>"));

            DestinationDetailsPanel.Controls.Add(new LiteralControl("<tr>"));
            DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
            DestinationDetailsPanel.Controls.Add(Lbl_ConnectionString);
            DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
            DestinationDetailsPanel.Controls.Add(new LiteralControl("<td>"));
            DestinationDetailsPanel.Controls.Add(txt_DestinationConnectionString);
            DestinationDetailsPanel.Controls.Add(new LiteralControl("</td>"));
            DestinationDetailsPanel.Controls.Add(new LiteralControl("</tr>"));

            DestinationDetailsPanel.Controls.Add(new LiteralControl("</td></tr></table>"));

        }



        private void DestinationUpdate()
        {
            SqlCommand comm = new SqlCommand(SQLDestinationCommand, conn);

            comm.Parameters.AddWithValue("@DestinationConnectionString", DestinationConnectionString);
            comm.Parameters.AddWithValue("@DestinationTableName", DestinationTableName);
            comm.Parameters.AddWithValue("@DestinationUsername", DestinationUsername);
            comm.Parameters.AddWithValue("@DestinationPassword", DestinationPassword);
            comm.Parameters.AddWithValue("@DestinationServerName", DestinationServerName);
            comm.Parameters.AddWithValue("@DestinationDatabase", DestinationDatabase);
            comm.Parameters.AddWithValue("@DestinationFileLocation", DestinationFileLocation);
            comm.Parameters.AddWithValue("@DestinationDataSource", DestinationDataSource);
            comm.Parameters.AddWithValue("@ProcessID", ProcessId);

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();
                updateInfo = "Update Successful";

            }
            catch (Exception ex)
            {

                updateInfo = "Error: " + ex.Message;
            }

        }

        private void SourceUpdate()
        {
            SqlCommand comm = new SqlCommand(SQLSourceCommand, conn);
            //Parameters for feed file import
            comm.Parameters.AddWithValue("@DataSetName", DataSetName);
            comm.Parameters.AddWithValue("@SourceFileLocation", SourceFileLocation);
            comm.Parameters.AddWithValue("@SourceFileName", SourceFileName);
            comm.Parameters.AddWithValue("@SourceDelimiter", SourceDelimiter);
            comm.Parameters.AddWithValue("@ProcessID", ProcessId);

            //parameters for direct connect import
            comm.Parameters.AddWithValue("@SourceTableName", SourceTableName);
            comm.Parameters.AddWithValue("@SourceConnectionString", SourceConnectionString);
            comm.Parameters.AddWithValue("@SourceUserName", SourceUserName);
            comm.Parameters.AddWithValue("@SourcePassword", SourcePassword);
            //comm.Parameters.AddWithValue("@SourceFileLocation", SourceFileLocation);
            comm.Parameters.AddWithValue("@SourceDatabase", SourceDatabase);
            comm.Parameters.AddWithValue("@SourceServerName", SourceServerName);
            comm.Parameters.AddWithValue("@SourceDataSource", SourceDataSource);


            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();
                updateInfo = "Update Successful";
            }
            catch (Exception ex)
            {
                updateInfo = "Error: " + ex.Message;
            }

        }
    }
}