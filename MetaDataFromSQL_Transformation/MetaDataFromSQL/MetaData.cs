using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;

namespace MetaDataFromSQL
{
    class MetaData
    {

        private string datasetName, destinationTable, sourceTable, delimiterCharacter, sourceFileLocation, filename, connectionString, sourceDB, destinationDB, sourceDBType, destinationDBType;
        List<string> primaryKeyList = new List<string>();
        string[] primaryKey;
        private int numOfColumns, process_id;
       
        SqlConnection conn = new SqlConnection(@"Server=WINDOWS-I92V0KI\SQLEXPRESS;Database=ThirdYear;Trusted_Connection=True;");
        public List<ColumnInfo> ColumnData = new List<ColumnInfo>();
        public List<string> ColumnNames = new List<string>();
        public Dictionary<string, string> DataTypes = new Dictionary<string, string>();
        public Dictionary<string, int> MinLength = new Dictionary<string, int>();
        public Dictionary<string, int> MaxLength = new Dictionary<string, int>();
        public Dictionary<string, bool> NullPermitted = new Dictionary<string, bool>();
        public Dictionary<string, string> Action = new Dictionary<string, string>();
        public Dictionary<string, string> ReplaceValue = new Dictionary<string, string>();

        string importProcessName;

        public MetaData(string ImportProcessName)
        {
            importProcessName = ImportProcessName;
        }

        public void GetMetaData()
        {
            SqlCommand com = new SqlCommand("SELECT * FROM Program_MetaData WHERE PROCESSNAME = '" + importProcessName + "'", conn);

            conn.Open();
            using (com)
            {
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    process_id = reader.GetInt32(0);
                    datasetName = reader.GetString(1);
                    sourceTable = reader.GetString(2);
                    destinationTable = reader.GetString(3);
                    sourceDB = reader.GetString(4);
                    destinationDB = reader.GetString(5);
                    sourceFileLocation = reader.GetString(6);
                    filename = reader.GetString(7);
                    sourceDBType = reader.GetString(8);
                    destinationDBType = reader.GetString(9);
                    numOfColumns = reader.GetInt32(10);
                    connectionString = reader.GetString(11);
                    delimiterCharacter = reader.GetString(12);

                }
            }
            conn.Close();

            GetColumnInfo();
            PopulateColumnMetaData();
            GetPrimaryKeys();
        }



        private void GetColumnInfo()
        {
            string query = "SELECT C.[ColumnName], C.[DataType], C.[MinLength], C.[MaxLength], C.[Nullable], C.[Action], C.[ReplaceValue] FROM [dbo].[Program_ColumnData] C JOIN [dbo].[Program_MetaData] M ON C.[Processid] = M.[Process_id] WHERE M.[PROCESSNAME] = '" + importProcessName + "' order by columnorder asc";
            SqlCommand comm = new SqlCommand(query, conn);

            conn.Open();

            using (comm)
            {
                SqlDataReader reader = comm.ExecuteReader();

                int count = 0;

                while (reader.Read())
                {
                    ColumnData.Add(new ColumnInfo());

                    ColumnData[count].ColumnName = reader.GetString(0);
                    ColumnNames.Add(reader.GetString(0));
                    ColumnData[count].DataType = reader.GetString(1);
                    ColumnData[count].MinLength = reader.GetInt32(2);
                    ColumnData[count].MaxLength = reader.GetInt32(3);
                    ColumnData[count].NullPermitted = (reader.GetString(4).Equals("True")) ? true : false;
                    ColumnData[count].Action = reader.GetString(5);
                    ColumnData[count].ReplaceValue = reader.GetString(6);
                    count++;
                }
            }
            conn.Close();

        }

        private void PopulateColumnMetaData()
        {
            foreach (var item in ColumnData)
            {
                DataTypes.Add(item.ColumnName, item.DataType);
                MinLength.Add(item.ColumnName, item.MinLength);
                MaxLength.Add(item.ColumnName, item.MaxLength);
                NullPermitted.Add(item.ColumnName, item.NullPermitted);
                Action.Add(item.ColumnName,item.Action);
                ReplaceValue.Add(item.ColumnName, item.ReplaceValue);
            }
        }

        private void GetPrimaryKeys()
        {
            string query = "SELECT PK.PrimaryKey FROM Program_PrimaryKeyData PK JOIN Program_MetaData MD ON PK.Process_id = MD.Process_id WHERE MD.ProcessName = '" + importProcessName + "'";
            SqlCommand comm = new SqlCommand(query, conn);

            conn.Open();

            using (comm)
            {
                SqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    primaryKeyList.Add(reader.GetString(0));
                }
            }
            conn.Close();

            primaryKey = new string[primaryKeyList.Count];

            for (int i = 0; i < primaryKeyList.Count; i++)
            {
                primaryKey[i] = primaryKeyList[i];
            }

            
        }




        #region Public Properties
        //Properties that represent the MetaData object for each data import process

        //databaseName, delimiterCharacter, sourceType,

        public string SourceDataBase
        {
            get { return sourceDB; }
        }

        public string DestinationTable
        {
            get { return destinationTable; }
        }

        public string DelimiterCharacter
        {
            get { return delimiterCharacter; }
        }
      
        public string SourceFileLocation
        {
            get { return sourceFileLocation; }
        }

        public string ConnectionString
        {
            set { connectionString = @"Data Source = WINDOWS - I92V0KI\SQLEXPRESS; Initial Catalog = FootballInformation; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"; }
            get { return connectionString; }
        }

        public string[] PrimaryKey
        {
            get { return primaryKey; }
        }

        public int NumberOfColumns
        {
            get { return numOfColumns; }
        }


        public string DataSetName
        {
           get { return datasetName;}
        }

        public string SourceTable
        {
            get { return sourceTable; }
        }

        #endregion
    }
}
