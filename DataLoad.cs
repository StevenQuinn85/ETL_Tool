using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.OracleClient;


namespace ELTManagement
{
    public class DataLoad
    {
        //This class will handle the insertion (blast and load) to the clean data to the destination

        //The class will require the destination details stored in the Data Properties object
        DataProperties Prop;

        //DataTable Object to hold the contents of dataset that is being imports
        DataTable Table = new DataTable();

        DataSet Records = new DataSet();

        SqlConnection conn = new SqlConnection();

        //private ints to count the number of records inserted, deleted or rejected
        private int recordInserted, recordDeleted, recordsRejected;
        private List<string> rejectionReasons = new List<string>();



        public DataLoad()
        {

        }

        public void Load(DataProperties DestinationDetails, DataSet Data)
        {
            Prop = DestinationDetails;
            Records = Data;
            Table = Data.Tables[DestinationDetails.DataSetName];

            if (DestinationDetails.DestinationType.Equals("SQL Server"))
            {
                LoadToSQLDestination();
            }

        }

        private void LoadToSQLDestination()
        {
            
            conn.ConnectionString = Prop.DestinationConnectionString;

            //reset the count of records inserted, deleted, rejected
            recordInserted = 0; recordDeleted = 0; recordsRejected = 0;

            foreach (DataTable item in Records.Tables)
            {
                foreach (DataRow row in item.Rows)
                {
                    string Clause = CreateWhereClause(row, Prop.DestinationTableName, Prop.Meta.PrimaryKey);
                    string CountClause = "Select COUNT(*) FROM " + Prop.DestinationTableName + " WHERE " + Clause;
                    List<string> ColumnNames = Prop.Meta.ColumnName;

                    bool RecordExists = CheckForRecord(CountClause);

                    if (RecordExists)
                    {
                        // Delete Existing Record Prior to Inserting new Record
                        DeleteRecord_SQL(row);

                    }
                    //Insert the record to the destination db

                        InsertToDataBase_SQL(row);
                }
            }

        }

        //Method to create a where clause for each record in the dataset
        private string CreateWhereClause(DataRow Row, string DestinationTable, List<string> PKeys)
        {
            string Clause = null;
            int i = 0;

            foreach (string item in PKeys)
            {
                if (i == 0)
                {
                    Clause += item + " = '" + Row[item].ToString() + "'";
                    i++;
                }
                else
                    Clause += " AND " + item + " = '" + Row[item].ToString() + "'";
            }

            Clause += ";";

            return Clause;
        }

        private bool CheckForRecord(string SQLCommand)
        {
            bool exists = false;
            conn.Open();

            SqlCommand Comm = new SqlCommand(SQLCommand, conn);

            Int32 count = Convert.ToInt32(Comm.ExecuteScalar());

            conn.Close();

            if (count > 0)
            {
                exists = true;
            }


            return exists;
        }

        private void InsertToDataBase_SQL(DataRow RowToInsert)
        {
            SqlCommand comm;

            List<string> Parameters = new List<string>();

            List<string> ParametersForInsert = Prop.Meta.ColumnName;

            string ColumnNames = "";

            int finalentry = ParametersForInsert.ToArray().Count() -1;
            int count = 0;
            foreach (string item in ParametersForInsert)
            {
                if (count == finalentry)
                {
                    ColumnNames += item;
                }
                else
                {
                ColumnNames += item + ",";
                count++;
                }

            }


            string command = "INSERT INTO " + Prop.DestinationTableName + " (" + ColumnNames + ") VALUES (";

            count = 0;

            foreach (string item in ParametersForInsert)
            {
                if (count == finalentry)
                {
                    command += "@" + item + ");";
                    Parameters.Add("@" + item);
                }
                else
                {
                    command += "@" + item + ",";
                    Parameters.Add("@" + item);
                    count++;
                }
            }

            comm = new SqlCommand(command, conn);

            count = 0;

            foreach (string item in Parameters)
            {
                comm.Parameters.AddWithValue(item, RowToInsert[ParametersForInsert[count]]);

                string columnName = item.Remove(0, 1);

                if (Prop.Meta.DataType[columnName].Equals("Date"))
                {
                    comm.Parameters[item].DbType = DbType.Date;
                }

                count++;
            }


            conn.Open();

            try
            {
                comm.ExecuteNonQuery();
                recordInserted++;
            }
            catch (Exception ex)
            {

                recordsRejected ++;
                rejectionReasons.Add(ex.Message);
            }

            conn.Close();

        }

        private void DeleteRecord_SQL(DataRow RowToInsert)
        {
            string whereClause = CreateWhereClause(RowToInsert, Prop.DestinationTableName, Prop.Meta.PrimaryKey);

            SqlCommand comm = new SqlCommand("DELETE FROM " + Prop.DestinationTableName + " WHERE " + whereClause, conn);

            conn.Open();
            try
            {
                comm.ExecuteNonQuery();
                recordDeleted++;
            }
            catch (Exception ex)
            {
                rejectionReasons.Add(ex.Message);

            }

            conn.Close();

        }

        public int RecordInserted
        {
            get
            {
                return recordInserted;
            }

        }

        public int RecordDeleted
        {
            get
            {
                return recordDeleted;
            }

        }

        public int RecordsRejected
        {
            get
            {
                return recordsRejected;
            }

        }

        public List<string> RejectionReasons
        {
            get
            {
                return rejectionReasons;
            }

        }
    }
}