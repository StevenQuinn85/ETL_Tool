using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ELTManagement
{
    public class DataProperties
    {
        public MetaData Meta;

        protected SqlConnection conn;

        protected string dataSetName, importType, sourcefileLocation, sourcefileName, sourceDelimiterChar, sourceDBType, sourceConnectionString, sourceUsername, sourcePassword, sourceTableName, sourceDataSource, sourceDBName, sourceServerName;
        protected string destinationType, destinationFileLocation, /*destinationDelimiterChar*/ destinationDBType, destinationConnectionString, destinationUsername, destinationPassword, destinationTableName, destinationDataSource, destinationDBName, destinationServerName;
        protected bool useLookBack = false;

        public DataProperties(SqlConnection Conn)
        {
            conn = Conn;
            Meta = new MetaData(conn);
        }

        public void Populate(string processID)
        {
            PopulateDataProperties(processID);
            Meta.PopulateMetaData(processID);
            Meta.PopulatePrimaryKeys(processID);            
        }

        private void PopulateDataProperties(string processID)
        {
            string commandText = "SELECT * FROM [dbo].[Program_DataProperties] WHERE ProcessId = @ProcessID";
            SqlCommand comm = new SqlCommand(commandText, conn);
            comm.Parameters.AddWithValue("@ProcessId", processID);

            conn.Open();

            using (SqlDataReader reader = comm.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataSetName = reader["DataSetName"].ToString();
                    ImportType = reader["ImportType"].ToString();
                    SourcefileLocation = reader["SourceFileLocation"].ToString();
                    SourcefileName = reader["SourceFileName"].ToString();
                    SourceDelimiterChar = reader["SourceDelimiter"].ToString();
                    SourceDBType = reader["SourceDBType"].ToString();
                    SourceConnectionString = reader["SourceConnectionString"].ToString();
                    SourceUsername = reader["SourceUserName"].ToString();
                    SourcePassword = reader["SourcePassword"].ToString();
                    SourceTableName = reader["SourceTableName"].ToString();
                    SourceDataSource = reader["SourceDataSource"].ToString();
                    SourceDBName = reader["SourceDatabase"].ToString();
                    SourceServerName = reader["SourceServerName"].ToString();
                    DestinationType = reader["DestinationType"].ToString();
                    DestinationFileLocation = reader["DestinationFileLocation"].ToString();
                    DestinationConnectionString = reader["DestinationConnectionString"].ToString();
                    DestinationUsername = reader["DestinationUsername"].ToString();
                    DestinationPassword = reader["DestinationPassword"].ToString();
                    DestinationTableName = reader["DestinationTableName"].ToString();
                    DestinationDataSource = reader["DestinationDataSource"].ToString();
                    DestinationDBName = reader["DestinationDatabase"].ToString();
                    DestinationServerName = reader["DestinationServerName"].ToString();

                    string lookBack = reader["UseLookBack"].ToString();

                    if (lookBack.Equals("Yes"))
                    {
                        UseLookBack = true;
                    }


                    
                }
            }
            conn.Close();
        }

        public string DataSetName
        {
            set { dataSetName = value; }
            get { return dataSetName; }
        }
        public string ImportType
        {
            set { importType = value; }
            get { return importType; }
        }
        public string SourcefileLocation
        {
            set { sourcefileLocation = value; }
            get { return sourcefileLocation; }
        }
        public string SourcefileName
        {
            set { sourcefileName = value; }
            get { return sourcefileName; }
        }
        public string SourceDelimiterChar
        {
            set { sourceDelimiterChar = value; }
            get { return sourceDelimiterChar; }
        }
        public string SourceDBType
        {
            set { sourceDBType = value; }
            get { return sourceDBType; }
        }
        public string SourceConnectionString
        {
            set { sourceConnectionString = value; }
            get { return sourceConnectionString; }
        }
        public string SourceUsername
        {
            set { sourceUsername = value; }
            get { return sourceUsername; }
        }
        public string SourcePassword
        {
            set { sourcePassword = value; }
            get { return sourcePassword; }
        }
        public string SourceTableName
        {
            set { sourceTableName = value; }
            get { return sourceTableName; }
        }
        public string SourceDataSource
        {
            set { sourceDataSource = value; }
            get { return sourceDataSource; }
        }
        public string SourceDBName
        {
            set { sourceDBName = value; }
            get { return sourceDBName; }
        }
        public string SourceServerName
        {
            set { sourceServerName = value; }
            get { return sourceServerName; }
        }
        public string DestinationType
        {
            set { destinationType = value; }
            get { return destinationType; }
        }
        public string DestinationFileLocation
        {
            set { destinationFileLocation = value; }
            get { return destinationFileLocation; }
        }
        public string DestinationDBType
        {
            set { destinationDBType = value; }
            get { return destinationDBType; }
        }
        public string DestinationConnectionString
        {
            set { destinationConnectionString = value; }
            get { return destinationConnectionString; }
        }
        public string DestinationUsername
        {
            set { destinationUsername = value; }
            get { return destinationUsername; }
        }
        public string DestinationPassword
        {
            set { destinationPassword = value; }
            get { return destinationPassword; }
        }
        public string DestinationTableName
        {
            set { destinationTableName = value; }
            get { return destinationTableName; }
        }
        public string DestinationDataSource
        {
            set { destinationDataSource = value; }
            get { return destinationDataSource; }
        }
        public string DestinationDBName
        {
            set { destinationDBName = value; }
            get { return destinationDBName; }
        }
        public string DestinationServerName
        {
            set { destinationServerName = value; }
            get { return destinationServerName; }
        }
        public bool UseLookBack
        {
            set { useLookBack = value; }
            get { return useLookBack; }
        }
    }
}