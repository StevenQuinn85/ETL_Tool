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
        OleDbConnection conn_Access = new OleDbConnection();
        OracleConnection conn_Oracle = new OracleConnection();

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
            else if (DestinationDetails.DestinationType.Equals("Access Database"))
            {
                LoadToAccessDestination();
            }
            else if (DestinationDetails.DestinationType.Equals("Oracle Database"))
            {
                LoadToOracle();
            }


        }

        private void LoadToOracle()
        {
            conn_Oracle.ConnectionString = Prop.DestinationConnectionString;

            //reset the count of records inserted, deleted, rejected
            recordInserted = 0; recordDeleted = 0; recordsRejected = 0;

            foreach (DataTable item in Records.Tables)
            {
                foreach (DataRow row in item.Rows)
                {
                    //changing create where clause to return an sql command

                    bool RecordExists = CheckForRecord_Oracle(row, Prop.DestinationTableName, Prop.Meta.PrimaryKey);

                    if (RecordExists)
                    {
                        // Delete Existing Record Prior to Inserting new Record
                        DeleteRecord_Oracle(row, Prop.DestinationTableName, Prop.Meta.PrimaryKey);

                    }
                    //Insert the record to the destination db

                    InsertToDataBase_Oracle(row);
                }
            }
        }



        private void LoadToAccessDestination()
        {
            conn_Access.ConnectionString = Prop.DestinationConnectionString;
            //reset the count of records inserted, deleted, rejected
            recordInserted = 0; recordDeleted = 0; recordsRejected = 0;

            foreach (DataTable item in Records.Tables)
            {
                foreach (DataRow row in item.Rows)
                {
                    //changing create where clause to return an sql command

                    bool RecordExists = CheckForRecord_Access(row, Prop.DestinationTableName, Prop.Meta.PrimaryKey);

                    if (RecordExists)
                    {
                        // Delete Existing Record Prior to Inserting new Record
                        DeleteRecord_Access(row, Prop.DestinationTableName, Prop.Meta.PrimaryKey);

                    }
                    //Insert the record to the destination db

                    InsertToDataBase_Access(row);
                }
            }

        }

        private void InsertToDataBase_Access(DataRow row)
        {
            OleDbCommand comm = new OleDbCommand();

            List<string> Parameters = new List<string>();

            List<string> ParametersForInsert = Prop.Meta.ColumnName;

            string ColumnNames = "";

            int finalentry = ParametersForInsert.ToArray().Count() - 1;
            int count = 0;
            foreach (string item in ParametersForInsert)
            {
                if (count == finalentry)
                {
                    ColumnNames += "["+item+"]";
                }
                else
                {
                    ColumnNames += "[" + item + "],";
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

            comm.CommandText = command;
            comm.Connection = conn_Access;

            count = 0;

            //add the paramater values and db type
            foreach (string item in Parameters)
            {
                comm.Parameters.AddWithValue(item, row[ParametersForInsert[count]]);

                //remove the first character "@" to get the original column name
                string columnName = item.Remove(0, 1);

                if (Prop.Meta.DataType[columnName].Equals("Date"))
                {
                    comm.Parameters[item].DbType = DbType.Date;
                }
                else if (Prop.Meta.DataType[columnName].Equals("Int"))
                {
                    comm.Parameters[item].DbType = DbType.Int32;
                }
                else if (Prop.Meta.DataType[columnName].Equals("BigInt"))
                {
                    comm.Parameters[item].DbType = DbType.Int64;
                }
                else if (Prop.Meta.DataType[columnName].Equals("Decimal"))
                {
                    comm.Parameters[item].DbType = DbType.Decimal;
                }
                else
                {
                    //Default value should be string/text
                    comm.Parameters[item].DbType = DbType.String;
                }

                count++;
            }

            conn_Access.Open();

            try
            {
                comm.ExecuteNonQuery();
                recordInserted++;
            }
            catch (Exception ex)
            {

                string rowId = "";
                recordsRejected++;

                foreach (var item in row.ItemArray)
                {
                    rowId += "|" + item.ToString();
                }

                rejectionReasons.Add("Row: " + rowId + " - " + ex.Message);
            }

            conn_Access.Close();
        }

        private void InsertToDataBase_Oracle(DataRow row)
        {
            OracleCommand comm = new OracleCommand();

            List<string> Parameters = new List<string>();

            List<string> ParametersForInsert = Prop.Meta.ColumnName;

            string ColumnNames = "";

            int finalentry = ParametersForInsert.ToArray().Count() - 1;
            int count = 0;
            foreach (string item in ParametersForInsert)
            {
                if (count == finalentry)
                {
                    ColumnNames += item;
                    Parameters.Add(item);
                }
                else
                {
                    ColumnNames += item + ",";
                    Parameters.Add(item);
                    count++;
                }

            }

            string command = "INSERT INTO " + Prop.DestinationTableName + " (" + ColumnNames + ") VALUES (";

            count = 0;

            foreach (string item in Parameters)
            {
                if (count == finalentry)
                {
                    command += ":" + item + ")";
                }
                else
                {
                command += ":" + item + ",";
                }
                count++;
            }

            count = 0;
            //add the paramater values and db type
            foreach (string item in Parameters)
            {
                comm.Parameters.Add(new OracleParameter(item, row[Parameters[count]]));

                //remove the first character "@" to get the original column name
                //string columnName = item.Remove(0, 1);

                if (Prop.Meta.DataType[item].Equals("Date"))
                {
                    comm.Parameters[item].OracleType = OracleType.DateTime;
                }
                else if (Prop.Meta.DataType[item].Equals("Int"))
                {
                    comm.Parameters[item].OracleType = OracleType.Int32;
                }
                else if (Prop.Meta.DataType[item].Equals("BigInt"))
                {
                    comm.Parameters[item].OracleType = OracleType.Number;
                }
                else if (Prop.Meta.DataType[item].Equals("Decimal"))
                {
                    comm.Parameters[item].OracleType = OracleType.Float;
                }
                else
                {
                    //Default value should be string/text
                    comm.Parameters[item].OracleType = OracleType.NVarChar;
                }

                count++;

            }

            comm.CommandText = command;
            comm.Connection = conn_Oracle;


            conn_Oracle.Open();

            try
            {
                comm.ExecuteNonQuery();
                recordInserted++;
            }
            catch (Exception ex)
            {
                string rowId = "";
                recordsRejected++;

                foreach (var item in row.ItemArray)
                {
                    rowId += "|" + item.ToString();
                }

                rejectionReasons.Add("Row: " + rowId + " - " + ex.Message);
            }

            conn_Oracle.Close();


        }

        private void DeleteRecord_Access(DataRow row, string DesTableName, List<string> PKeys)
        {
            OleDbCommand comm = new OleDbCommand();

            string commandText = "DELETE FROM " + Prop.DestinationTableName + " WHERE ";

            int i = 0;

            foreach (string item in PKeys)
            {
                if (i == 0)
                {
                    commandText += item + " = " + "@" + item;
                    comm.Parameters.AddWithValue("@" + item, row[item]);

                    i++;
                }
                else
                {
                    commandText += " AND " + item + " = " + "@" + item;
                    comm.Parameters.AddWithValue("@" + item, row[item]);
                }

                if (Prop.Meta.DataType[item].Equals("Int"))
                {
                    comm.Parameters["@" + item].DbType = DbType.Int32;
                }
                else if(Prop.Meta.DataType[item].Equals("Date"))
                {
                    comm.Parameters["@" + item].DbType = DbType.Date;
                }
            }

            comm.Connection = conn_Access;
            comm.CommandText = commandText;

            try
            {
                conn_Access.Open();
                comm.ExecuteNonQuery();
                conn_Access.Close();
                recordDeleted++;
            }
            catch (Exception ex)
            {
                rejectionReasons.Add(ex.Message);

                throw;
            }

        }

        private void DeleteRecord_Oracle(DataRow row, string destinationTableName, List<string> PKeys)
        {
            OracleCommand comm = new OracleCommand();

            string commandText = "DELETE FROM " + Prop.DestinationTableName + " WHERE ";

            int i = 0;

            foreach (string item in PKeys)
            {
                if (i == 0)
                {
                    commandText += item + " = " + ":" + item;
                    comm.Parameters.Add(new OracleParameter(item, row[item]));

                    i++;
                }
                else
                {
                    commandText += " AND " + item + " = " + ":" + item;
                    comm.Parameters.Add(new OracleParameter(item, row[item]));
                }

                if (Prop.Meta.DataType[item].Equals("Int"))
                {
                    comm.Parameters[item].OracleType = OracleType.Int32;
                }
                else if (Prop.Meta.DataType[item].Equals("Date"))
                {
                    comm.Parameters[item].OracleType = OracleType.DateTime;
                }
            }

            comm.Connection = conn_Oracle;
            comm.CommandText = commandText;

            try
            {
                conn_Oracle.Open();
                comm.ExecuteNonQuery();
                conn_Oracle.Close();
                recordDeleted++;
            }
            catch (Exception ex)
            {
                rejectionReasons.Add(ex.Message);

                throw;
            }

        }

        private bool CheckForRecord_SQL(DataRow row, string TableName, List<string>PKeys )
        {
            bool exists = false;
            int i = 0;
            SqlCommand comm = new SqlCommand();

            string commandText = "SELECT COUNT(*) FROM " + TableName + " WHERE ";

            foreach (string item in PKeys)
            {
                if (i == 0)
                {
                    commandText += item + " = " + "@" + item;
                    comm.Parameters.AddWithValue("@" + item, row[item]);

                    i++;
                }
                else
                {
                    commandText += " AND " + item + " = " + "@" + item;
                    comm.Parameters.AddWithValue("@" + item, row[item]);
                }

                if (Prop.Meta.DataType[item].Equals("Int"))
                {
                    comm.Parameters["@" + item].SqlDbType = SqlDbType.Int;
                }
                else if(Prop.Meta.DataType[item].Equals("BigInt"))
                {
                    comm.Parameters[item].SqlDbType = SqlDbType.BigInt;
                }
                else if (Prop.Meta.DataType[item].Equals("Date"))
                {
                    comm.Parameters["@" + item].SqlDbType = SqlDbType.Date;
                }
                else
                {
                    comm.Parameters["@" + item].SqlDbType = SqlDbType.NVarChar;
                }
            }

            comm.CommandText = commandText;
            comm.Connection = conn;
            i = 0;
            try
            {
                conn.Open();
                i = Convert.ToInt32(comm.ExecuteScalar());
                conn.Close();
            }
            catch (Exception)
            {

                throw;
            }

            if (i > 0)
            {
                exists = true;
            }

            return exists;
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

                    bool RecordExists = CheckForRecord_SQL(row, Prop.DestinationTableName, Prop.Meta.PrimaryKey);

                    if (RecordExists)
                    {
                        // Delete Existing Record Prior to Inserting new Record
                        DeleteRecord_SQL(row, Prop.DestinationTableName, Prop.Meta.PrimaryKey);

                    }
                    //Insert the record to the destination db

                    InsertToDataBase_SQL(row);
                }
            }
        }

        private bool CheckForRecord_Oracle(DataRow row, string destinationTableName, List<string> PKeys)
        {
            bool exists = false;
            int i = 0;
            int count = 0;
            OracleCommand comm = new OracleCommand();
            comm.Connection = conn_Oracle;

            string commandText = "SELECT COUNT(*) FROM " + destinationTableName + " WHERE ";

            foreach (string item in PKeys)
            {
                if (i == 0)
                {
                    commandText += item + " = " + ":" + item;
                    comm.Parameters.AddWithValue(":" + item, row[item]);

                    i++;
                }
                else
                {
                    commandText += " AND " + item + " = " + ":" + item;
                    comm.Parameters.AddWithValue(":" + item, row[item]);
                }

                if (Prop.Meta.DataType[item].Equals("Int"))
                {
                    comm.Parameters[":" + item].OracleType = OracleType.Int32;
                }
                else if (Prop.Meta.DataType[item].Equals("Date"))
                {
                    comm.Parameters[":" + item].OracleType = OracleType.DateTime;
                }
            }

            comm.CommandText = commandText;

            try
            {
                conn_Oracle.Open();
                count = Convert.ToInt32(comm.ExecuteScalar());
                conn_Oracle.Close();
            }
            catch (Exception)
            {

                throw;
            }

            if (count > 0)
            {
                exists = true;
            }

            return exists;

        }


        //Method to create a where clause for each record in the dataset
        private bool CheckForRecord_Access(DataRow Row, string DestinationTable, List<string> PKeys)
        {
            bool exists = false;
            int count = 0;


            OleDbCommand comm = new OleDbCommand();
            comm.Connection = conn_Access;
            string Clause = "SELECT COUNT(*) FROM " + DestinationTable + " WHERE ";
            int i = 0;

            foreach (string item in PKeys)
            {
                if (i == 0)
                {
                    Clause += item + " = " + "@" + item;
                    comm.Parameters.AddWithValue("@" + item, Row[item]);

                    i++;
                }
                else
                {
                    Clause += " AND " + item + " = " + "@" + item;
                    comm.Parameters.AddWithValue("@" + item, Row[item]);
                }

                if (Prop.Meta.DataType[item].Equals("Int"))
                {
                    comm.Parameters["@" + item].DbType = DbType.Int32;
                }
            }

            comm.CommandText = Clause;

            try
            {
                conn_Access.Open();
                count = Convert.ToInt32(comm.ExecuteScalar());
                conn_Access.Close();
            }
            catch (Exception)
            {

                throw;
            }

            if (count >0 )
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

            int finalentry = ParametersForInsert.ToArray().Count() - 1;
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

            //add the paramater values and db type
            foreach (string item in Parameters)
            {
                comm.Parameters.AddWithValue(item, RowToInsert[ParametersForInsert[count]]);

                //remove the first character "@" to get the original column name
                string columnName = item.Remove(0, 1);

                if (Prop.Meta.DataType[columnName].Equals("Date"))
                {
                    comm.Parameters[item].SqlDbType = SqlDbType.Date;
                }
                else if (Prop.Meta.DataType[columnName].Equals("Int"))
                {
                    comm.Parameters[item].SqlDbType = SqlDbType.Int;
                }
                else if (Prop.Meta.DataType[columnName].Equals("BigInt"))
                {
                    comm.Parameters[item].SqlDbType = SqlDbType.BigInt;
                }
                else if (Prop.Meta.DataType[columnName].Equals("Decimal"))
                {
                    comm.Parameters[item].SqlDbType = SqlDbType.Decimal;
                }
                else
                {
                    //Default value should be string/text
                    comm.Parameters[item].SqlDbType = SqlDbType.NVarChar;
                }

                count++;
            }

            comm.Connection = conn;
            conn.Open();

            try
            {
                comm.ExecuteNonQuery();
                recordInserted++;
            }
            catch (Exception ex)
            {
                string rowid = "";

                foreach (var item in RowToInsert.ItemArray)
                {
                    rowid += item.ToString();
                }


                recordsRejected++;
                rejectionReasons.Add(ex.Message);
            }

            conn.Close();

        }

        private void DeleteRecord_SQL(DataRow Row, string DestinationTable, List<string> PKeys)
        {
            string commandText = "DELETE FROM " + Prop.DestinationTableName + " WHERE ";

            SqlCommand comm = new SqlCommand();
        
            int i = 0;

            foreach (string item in PKeys)
            {
                if (i == 0)
                {
                    commandText += item + " = " + "@" + item;
                    comm.Parameters.AddWithValue("@" + item, Row[item]);

                    i++;
                }
                else
                {
                    commandText += " AND " + item + " = " + "@" + item;
                    comm.Parameters.AddWithValue("@" + item, Row[item]);
                }


                if (Prop.Meta.DataType[item].Equals("Date"))
                {
                    comm.Parameters["@" + item].SqlDbType = SqlDbType.Date;
                }
                else if (Prop.Meta.DataType[item].Equals("Int"))
                {
                    comm.Parameters["@" + item].SqlDbType = SqlDbType.Int;
                }
                else if (Prop.Meta.DataType[item].Equals("BigInt"))
                {
                    comm.Parameters["@" + item].SqlDbType = SqlDbType.BigInt;
                }
                else if (Prop.Meta.DataType[item].Equals("Decimal"))
                {
                    comm.Parameters["@" + item].SqlDbType = SqlDbType.Decimal;
                }
                else
                {
                    //Default value should be string/text
                    comm.Parameters["@" + item].SqlDbType = SqlDbType.NVarChar;
                }
            }

            comm.CommandText = commandText;
            comm.Connection = conn;

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                recordDeleted++;
                conn.Close();
            }
            catch (Exception ex)
            {
                rejectionReasons.Add(ex.Message);

            }

            

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