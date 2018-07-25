using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

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
        Importer FileReader;

        //The Direct Conenct Object will be used to connect to SQL Server, Oracle or Access DB
        DirectConnect DirectConnection;

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

            //STEP ONE
            //Generate a Data Properties Object
            DataProp = new DataProperties(conn);
            DataProp.Populate(id);

            //STEP TWO
            //Import data from source
            RawData = ImportData();

            //STEP THREE 
            //Transform Data
            Transform.TransformData(RawData, DataProp);
            AcceptableData = Transform.AcceptableData();

            ImportStatistics.TransformationErrorsList = Transform.ErrorsList;
            ImportStatistics.AcceptedRecords_transform = Transform.RowsAcceptedCount;
            ImportStatistics.RejectedRecords_transform = Transform.RowsRejectedCount;



            //STEP FOUR 
            //Load Data To final destination
            LoadData.Load(DataProp, AcceptableData);

            ImportStatistics.RecordsInsertedToDestination = LoadData.RecordInserted;
            ImportStatistics.RecordDeletedFromDestination = LoadData.RecordDeleted;
            ImportStatistics.RecordsRejectedFromDestination = LoadData.RecordsRejected;
            ImportStatistics.DestinationRejectionReasons = LoadData.RejectionReasons;



            //STEP FIVE
            //Direct User to results page
            
        }

        //Method to import data from source
        private DataSet ImportData()
        {
            DataSet SourceContents = new DataSet();

            if (DataProp.ImportType.Equals("Feed File"))
            {
                FileReader = new Importer();
                SourceContents = FileReader.ImportFile(DataProp);

                ImportStatistics.RawDataRecordsImported = FileReader.RecordsImported;
                ImportStatistics.RawDataRecordsOmitted = FileReader.RecordsRejected;
                ImportStatistics.ErrorsInRawData = FileReader.ListOfRejections;
            }
            else if (DataProp.ImportType.Equals("Direct Connect"))
            {
                DirectConnection = new DirectConnect();

                if (DataProp.SourceDBType.Equals("SQL Server"))
                {
                    SourceContents = DirectConnection.SQLImport(DataProp);
                }
                else if (DataProp.SourceDBType.Equals("Oracle Database"))
                {

                }
                else if (DataProp.SourceDBType.Equals("Access Database"))
                {
                    SourceContents = DirectConnection.AccessImport(DataProp);
                }

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