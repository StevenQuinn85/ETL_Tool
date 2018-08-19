using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ELTManagement
{
    public class Results
    {
        //This class will store the results of each import process
        //The results will be displayed on the Import Results web page

        //private strings to store statistics 
        //Statistics about the Raw Data Imported
        internal int rawDataRecordsImported, rawDataRecordsOmitted;
        internal List<string> errorsInRawData = new List<string>();
        //Statistics about the transformationProcess
        internal int acceptedRecords_transform, rejectedRecords_transform;
        internal Dictionary<string, string> transformationErrorsList = new Dictionary<string, string>();
        //Statistics about the insertion process
        internal int recordsInsertedToDestination, recordDeletedFromDestination, recordsRejectedFromDestination;
        internal List<string> destinationRejectionReasons = new List<string>();

        internal string importStartTime, importEndTime;
        protected string dataSetName;

        protected DateTime Timestamp = new DateTime();

        protected string logFileLocation;

        //variables to store whether or not the import was succesful and any related error
        private bool importSuccessful;
        private string failureError;

        //variable to hold the id of the newly created import record in the Job History table
        private int jobRecordId;

        public Results()
        {
            //set all values to zero
            rawDataRecordsImported = 0;
            RawDataRecordsOmitted = 0;

            acceptedRecords_transform = 0;
            rejectedRecords_transform = 0;

            recordsInsertedToDestination = 0;
            recordsRejectedFromDestination = 0;
            recordDeletedFromDestination = 0;

            ImportSuccessful = true;
        }

        //method to update the job history table with information about the start of an import
        public void UpdateJobHistoryTableStart()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["BackEndDB"].ConnectionString;
            string commandText = "INSERT INTO [dbo].[JobRunHistory] (ProcessName, StatusId, StartTime,LogName) VALUES (@ProcessName, @StatusId, @StartTime, @LogName)";
            SqlCommand comm = new SqlCommand(commandText, conn);
            //Insert the name of the dataset being import, the statusid is 1 which representing running, and the start time of the import.
            comm.Parameters.AddWithValue("@ProcessName", dataSetName);
            comm.Parameters["@ProcessName"].SqlDbType = SqlDbType.NVarChar;
            comm.Parameters.AddWithValue("@StatusId", 1);
            comm.Parameters["@StatusId"].SqlDbType = SqlDbType.Int;
            comm.Parameters.AddWithValue("@StartTime", importStartTime);
            comm.Parameters["@StartTime"].SqlDbType = SqlDbType.DateTime;
            comm.Parameters.AddWithValue("@LogName", logFileLocation);
            comm.Parameters["@LogName"].SqlDbType = SqlDbType.NVarChar;

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

            //retrieve the jobId to be used by subsequent SQL queries

            commandText = "SELECT JobId FROM [dbo].[JobRunHistory] WHERE ProcessName = @ProcessName and StartTime = @StartTime";
            comm.CommandText = commandText;

            try
            {
                conn.Open();
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        jobRecordId = Convert.ToInt32(reader["JobId"].ToString());
                    }
                    
                }
                    conn.Close();
            }
            catch (Exception)
            {

                
                throw;
            }

        }

        //method to update the job history table with information about the start of an import
        public void UpdateJobHistoryTableEnd()
        {
            int importStatus = 0;

            if (!importSuccessful)
            {
                importStatus = 2;
            }
            else
            {
                importStatus = 4;
            }

            SqlConnection conn = new SqlConnection();
            SqlCommand comm = new SqlCommand();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["BackEndDB"].ConnectionString;
            string commandText = "	  UPDATE [dbo].[JobRunHistory] SET [StatusId] = @Status, [EndTime] = @EndTime,  [RowsInserted] = @Inserted, [RowsDeleted] = @Deleted, [RowsRejected] = @Rejected WHERE JobId = @JobId";
            comm.Connection = conn;
            comm.CommandText = commandText;

            comm.Parameters.AddWithValue("@Status", importStatus);
            comm.Parameters["@Status"].SqlDbType = SqlDbType.Int;

            comm.Parameters.AddWithValue("@EndTime", importEndTime);
            comm.Parameters["@EndTime"].SqlDbType = SqlDbType.DateTime;
            
            comm.Parameters.AddWithValue("@Inserted", recordsInsertedToDestination);
            comm.Parameters["@Inserted"].SqlDbType = SqlDbType.Int;

            comm.Parameters.AddWithValue("@Deleted", recordDeletedFromDestination);
            comm.Parameters["@Deleted"].SqlDbType = SqlDbType.Int;

            //variable to calculate all the records rejected during the ETL process
            int rejected = recordsRejectedFromDestination + rejectedRecords_transform + rawDataRecordsOmitted;
            comm.Parameters.AddWithValue("@Rejected", rejected);
            comm.Parameters["@Rejected"].SqlDbType = SqlDbType.Int;

            comm.Parameters.AddWithValue("@JobId", jobRecordId);
            comm.Parameters["@JobId"].SqlDbType = SqlDbType.Int;

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

        public void SetImportStartTime()
        {
            Timestamp = DateTime.Now;
            ImportStartTime = Timestamp.ToString();
        }

        public string ImportStartTime
        {
            get { return importStartTime; }
            set { importStartTime = value; }
        }

        public void SetImportEndTime()
        {
            Timestamp = DateTime.Now;
            ImportEndTime = Timestamp.ToString();
        }

        public void NameLogFile(string name)
        {
            dataSetName = name;

            string path = @"C:\Users\StevenQuinn\Documents\Dev\Logs\";
            Timestamp = DateTime.Now;
            string fileName = dataSetName + Timestamp.Year + "_"+ Timestamp.Month+ "_" +Timestamp.Day + "_" +Timestamp.Hour + "_" +Timestamp.Minute + "_" +Timestamp.Second + ".txt";
            string location = path + fileName;

            logFileLocation = location;

        }

        public void ProduceLogFile()
        {

            string status = importSuccessful ? "Successful" : "Failed";
            
            using (StreamWriter writer = new StreamWriter(logFileLocation))
            {
                writer.WriteLine();
                //DataSetName
                writer.WriteLine("IMPORT DETAILS");
                writer.WriteLine("Status: " + status);

                if (!importSuccessful)
                {
                    writer.WriteLine(failureError);
                    writer.WriteLine();
                        }

                writer.WriteLine("Dataset imported: " + dataSetName);
                //Start and End Time
                writer.WriteLine("Started: " + ImportStartTime);
                writer.WriteLine("Completed: " + ImportEndTime);

                //Raw Data Imported
                writer.WriteLine("");
                writer.WriteLine("RAW DATA DETAILS");
                writer.WriteLine("Raw data imported: " + RawDataRecordsImported);
                writer.WriteLine("Raw data records omitted: " + RawDataRecordsOmitted);

                if (ErrorsInRawData.Count()>0)
                {
                    writer.WriteLine("Raw records omitted");

                    foreach (var item in ErrorsInRawData)
                    {
                        writer.WriteLine(item);

                    }
                }

                //Transformation
                writer.WriteLine("");
                writer.WriteLine("TRANSFORMATION DETAILS");
                writer.WriteLine("Records Accepted: " + AcceptedRecords_transform);
                writer.WriteLine("Records Rejected: " + RejectedRecords_transform);

                if (TransformationErrorsList.Count> 0)
                {
                    writer.WriteLine("Errors in Transformation");

                    foreach (var item in TransformationErrorsList)
                    {
                        writer.WriteLine(item);
                    }
                }


                //DataLoad
                writer.WriteLine("");
                writer.WriteLine("DATA LOAD DETAILS");
                writer.WriteLine("Records Inserted to Destination: " + RecordsInsertedToDestination);
                writer.WriteLine("Records Rejected by Destination: " + RecordsRejectedFromDestination);
                writer.WriteLine("Records Deleted from Destination: " + RecordDeletedFromDestination);

                if (DestinationRejectionReasons.Count>0)
                {
                    writer.WriteLine("Rejection Reason");
                    foreach (var item in DestinationRejectionReasons)
                    {
                        writer.WriteLine(item);
                    }
                }
            }
        }

        public string ImportEndTime
        {
            get { return importEndTime; }
            set { importEndTime = value; }
        }

        //Properties to access the import statistics 
        public int RawDataRecordsImported
        {
            get
            {
                return rawDataRecordsImported;
            }

            set
            {
                rawDataRecordsImported = value;
            }
        }

        public int RawDataRecordsOmitted
        {
            get
            {
                return rawDataRecordsOmitted;
            }

            set
            {
                rawDataRecordsOmitted = value;
            }
        }

        public List<string> ErrorsInRawData
        {
            get
            {
                return errorsInRawData;
            }

            set
            {
                errorsInRawData = value;
            }
        }

        public Dictionary<string, string> TransformationErrorsList
        {
            get
            {
                return transformationErrorsList;
            }

            set
            {
                transformationErrorsList = value;
            }
        }

        public int AcceptedRecords_transform
        {
            get
            {
                return acceptedRecords_transform;
            }

            set
            {
                acceptedRecords_transform = value;
            }
        }

        public int RejectedRecords_transform
        {
            get
            {
                return rejectedRecords_transform;
            }

            set
            {
                rejectedRecords_transform = value;
            }
        }

        public int RecordsInsertedToDestination
        {
            get
            {
                return recordsInsertedToDestination;
            }

            set
            {
                recordsInsertedToDestination = value;
            }
        }

        public int RecordDeletedFromDestination
        {
            get
            {
                return recordDeletedFromDestination;
            }

            set
            {
                recordDeletedFromDestination = value;
            }
        }

        public int RecordsRejectedFromDestination
        {
            get
            {
                return recordsRejectedFromDestination;
            }

            set
            {
                recordsRejectedFromDestination = value;
            }
        }

        public List<string> DestinationRejectionReasons
        {
            get
            {
                return destinationRejectionReasons;
            }

            set
            {
                destinationRejectionReasons = value;
            }
        }

        public bool ImportSuccessful
        {
            get
            {
                return importSuccessful;
            }

            set
            {
                importSuccessful = value;
            }
        }

        public string FailureError
        {
            get
            {
                return failureError;
            }

            set
            {
                failureError = value;
            }
        }
    }

}