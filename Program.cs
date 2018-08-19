using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data;
using System.IO;


namespace ELTManagement
{
    public class Program
    {
        //This is a program class that will perform the execution of the ETL processes.

        //It will use a single SQL Connection to communicate with the Back End DB
        SqlConnection conn = new SqlConnection();

        //The program will use a custom object called Data Properties which holds all the details 
        //about the data import process
        DataProperties DataProp;

        //The Importer object is used to read feed files and import the contents.
        //Importer FileReader;
        FeedFileExtraction FileReader;

        //The Direct Conenct Object will be used to connect to SQL Server, Oracle or Access DB
        //DirectConnect DirectConnection;
        DataBaseExtraction DirectConnection;

        //The Program will read data from the source into a DataSet called Raw Data;
        DataSet RawData = new DataSet();

        //The Program will use a Datset to hold the transformed data
        DataSet AcceptableData;

        //The Program will perform Transformation on the raw data, prior to insertion
        Transformation Transform = new Transformation();

        //The Program will use the Data Load object to insert to the destination
        DataLoad LoadData = new DataLoad();

        //Result Class to hold details about the import process
        public Results ImportStatistics = new Results();

        public Program(SqlConnection BackEndConnection)
        {
            conn = BackEndConnection;
        }

        //Method to perform the ETL process
        public void PerformDataImport(int processId)
        {
            string id = processId.ToString();

            //TimeStamp the start of the import
            ImportStatistics.SetImportStartTime();
           

            //STEP ONE
            //Generate a Data Properties Object
            DataProp = new DataProperties(conn);
            DataProp.Populate(id);

            //STEP TWO
            //CREATE LOG FILE
            ImportStatistics.NameLogFile(DataProp.DataSetName);
            ImportStatistics.UpdateJobHistoryTableStart();



            //Check if the required feed file or database connection is available be proceeding
            if (SourceAvailable())
            {
                //STEP THREE
                //Import data from source
                RawData = ImportData();

                if (RawData != null)
                {
                    //STEP FOUR 
                    //Transform Data
                    Transform.TransformData(RawData, DataProp);
                    AcceptableData = Transform.AcceptableData();

                    ImportStatistics.TransformationErrorsList = Transform.ErrorsList;
                    ImportStatistics.AcceptedRecords_transform = Transform.RowsAcceptedCount;
                    ImportStatistics.RejectedRecords_transform = Transform.RowsRejectedCount;



                    //STEP FIVE 
                    //Load Data To final destination
                    LoadData.Load(DataProp, AcceptableData);

                    ImportStatistics.RecordsInsertedToDestination = LoadData.RecordInserted;
                    ImportStatistics.RecordDeletedFromDestination = LoadData.RecordDeleted;
                    ImportStatistics.RecordsRejectedFromDestination = LoadData.RecordsRejected;
                    ImportStatistics.DestinationRejectionReasons = LoadData.RejectionReasons;

                    //Timestamp the end of the import
                    ImportStatistics.SetImportEndTime();
                }
                else
                {
                    ImportStatistics.FailureError = "No data retrieved from source";
                    ImportStatistics.ImportSuccessful = false;
                    
                }

            }

            //STEP SIX WRITE TO LOG FILE
            //Timestamp the end of the import
            ImportStatistics.SetImportEndTime();
            //Produce Log
            ImportStatistics.ProduceLogFile();
            ImportStatistics.UpdateJobHistoryTableEnd();
        }

        private bool SourceAvailable()
        {
            bool SourceAvailable = false;

            if (DataProp.ImportType.Equals("Feed File"))
            {
                SourceAvailable = File.Exists(DataProp.SourcefileLocation) ? true : false;

                if (!SourceAvailable)
                {
                    ImportStatistics.ImportSuccessful = false;
                    ImportStatistics.FailureError = "Source File " + DataProp.SourcefileLocation + " not available";
                }
            }
            else if (DataProp.ImportType.Equals("Direct Connect"))
            {
                if (DataProp.SourceDBType.Equals("SQL Server"))
                {
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = DataProp.SourceConnectionString;

                    try
                    {
                        conn.Open();

                        SourceAvailable = conn.State == ConnectionState.Open ? true : false;

                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        ImportStatistics.FailureError += ex.Message + " | ";
                        throw;
                    }
                }
                else if (DataProp.SourceDBType.Equals("Oracle Database"))
                {
                    OracleConnection conn = new OracleConnection();
                    conn.ConnectionString = DataProp.SourceConnectionString;

                    try
                    {
                        conn.Open();

                        SourceAvailable = conn.State == ConnectionState.Open ? true : false;

                        conn.Close();
                    }
                    catch (Exception ex )
                    {

                        ImportStatistics.FailureError += ex.Message + " | ";
                        throw;
                    }

                }
                else if (DataProp.SourceDBType.Equals("Access Database"))
                {
                    OleDbConnection conn = new OleDbConnection();
                    conn.ConnectionString = DataProp.SourceConnectionString;

                    try
                    {
                        conn.Open();

                        SourceAvailable = conn.State == ConnectionState.Open ? true : false;

                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        ImportStatistics.FailureError += ex.Message + " | ";
                        throw;
                    }
                }

                if (!SourceAvailable)
                {
                    ImportStatistics.ImportSuccessful = false;
                    ImportStatistics.FailureError = "Source Database " + DataProp.SourceConnectionString + " not available";
                }
            }

            return SourceAvailable;
        }

        //Method to import data from source
        private DataSet ImportData()
        {
            DataSet SourceContents = new DataSet();
            
            if (DataProp.ImportType.Equals("Feed File"))
            {
                if (File.Exists(DataProp.SourcefileLocation))
                {
                    FileReader = new FeedFileExtraction(DataProp);
                    SourceContents = FileReader.ImportFile();

                    //Gather Stats for feed file import
                    ImportStatistics.RawDataRecordsImported = FileReader.RowImportedFromSource;
                    ImportStatistics.RawDataRecordsOmitted = FileReader.RecordsRejected;
                    ImportStatistics.ErrorsInRawData = FileReader.ListOfRejections;
                }
                else
                {
                    ImportStatistics.ImportSuccessful = false;
                }
            }
            else if (DataProp.ImportType.Equals("Direct Connect"))
            {
                DirectConnection = new DataBaseExtraction(DataProp);

                if (DataProp.SourceDBType.Equals("SQL Server"))
                {
                    SourceContents = DirectConnection.SQLImport();
                }
                else if (DataProp.SourceDBType.Equals("Oracle Database"))
                {
                    SourceContents = DirectConnection.OracleImport();
                }
                else if (DataProp.SourceDBType.Equals("Access Database"))
                {
                    SourceContents = DirectConnection.AccessImport();
                }
                //Gather Stats for direct connect import
                ImportStatistics.RawDataRecordsImported = DirectConnection.RowImportedFromSource;
            }
            else
            {
                //Error Statement
            }

            return SourceContents;

        }
    }
}