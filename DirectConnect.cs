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
    public class DirectConnect
    {
        //Count of rows imported
        private int rowImportedFromSource = 0;


        //Direct Connect will use this method to import from SQL Server databases
        public DataSet SQLImport(DataProperties Properties)
        {
            //DataSet Object that will contain contents of file
            DataSet FileContents = new DataSet();
            DateTime LookBackDate = new DateTime();

            SqlConnection conn = new SqlConnection(Properties.SourceConnectionString);
            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;

            string commandText = "Select * from " + Properties.SourceTableName;

            if (Properties.UseLookBack)
            {
                commandText += " WHERE " + Properties.LookBackColumnName + " >= @LookBackPeroid";
                
                //Set the look back date to be one second prior to midnight on the date of the look back period.
                LookBackDate = DateTime.Now.AddDays(-Properties.LookBackPeriod -1).AddHours(-DateTime.Now.Hour +23).AddMinutes(-DateTime.Now.Minute +59).AddSeconds(-DateTime.Now.Second +59);

                comm.Parameters.AddWithValue("@LookBackPeroid", LookBackDate);
                comm.Parameters["@LookBackPeroid"].SqlDbType = SqlDbType.DateTime;
            }

            comm.CommandText = commandText;

            //Pass the SQL Command with the command text, parameters and connection into the adapter
            SqlDataAdapter Adapter = new SqlDataAdapter(comm);

            Adapter.FillSchema(FileContents, SchemaType.Source, Properties.SourceTableName);
            Adapter.Fill(FileContents, Properties.SourceTableName);

            rowImportedFromSource = FileContents.Tables[0].Rows.Count;

            return FileContents;
        }

        public DataSet AccessImport(DataProperties Properties)
        {
            //DataSet Object that will contain contents of file
            DataSet FileContents = new DataSet();
            DateTime LookBackDate = new DateTime();

            OleDbConnection conn = new OleDbConnection(Properties.SourceConnectionString);
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = conn;

            string commandText = "Select * from " + Properties.SourceTableName;

            if (Properties.UseLookBack)
            {
                commandText += " WHERE " + Properties.LookBackColumnName + " >= @LookBackPeroid";

                //Set the look back date to be one second prior to midnight on the date of the look back period.
                LookBackDate = DateTime.Now.AddDays(-Properties.LookBackPeriod - 1).AddHours(-DateTime.Now.Hour + 23).AddMinutes(-DateTime.Now.Minute + 59).AddSeconds(-DateTime.Now.Second + 59);

                comm.Parameters.AddWithValue("@LookBackPeroid", LookBackDate);
                comm.Parameters["@LookBackPeroid"].OleDbType = OleDbType.DBDate;
            }

            comm.CommandText = commandText;

            OleDbDataAdapter Adapter = new OleDbDataAdapter(comm);

            Adapter.FillSchema(FileContents, SchemaType.Source, Properties.SourceTableName);
            Adapter.Fill(FileContents, Properties.SourceTableName);

            rowImportedFromSource = FileContents.Tables[0].Rows.Count;
           
            return FileContents;
        }

        public DataSet OracleImport(DataProperties Properties)
        {
            //DataSet Object that will contain contents of file
            DataSet FileContents = new DataSet();
            DateTime LookBackDate = new DateTime();

            OracleConnection conn_oracle = new OracleConnection(Properties.SourceConnectionString);
            OracleCommand comm = new OracleCommand();
            comm.Connection = conn_oracle;

            string commandText = "Select * from " + Properties.SourceTableName;

            if (Properties.UseLookBack)
            {
                commandText += " WHERE " + Properties.LookBackColumnName + " >= :LookBackPeroid";

                //Set the look back date to be one second prior to midnight on the date of the look back period.
                LookBackDate = DateTime.Now.AddDays(-Properties.LookBackPeriod - 1).AddHours(-DateTime.Now.Hour + 23).AddMinutes(-DateTime.Now.Minute + 59).AddSeconds(-DateTime.Now.Second + 59);

                comm.Parameters.Add(new OracleParameter("LookBackPeroid", LookBackDate));
                comm.Parameters["LookBackPeroid"].OracleType = OracleType.DateTime;
            }

            comm.CommandText = commandText;

            //Pass the SQL Command with the command text, parameters and connection into the adapter
            OracleDataAdapter Adapter_Oracle = new OracleDataAdapter(comm);

            Adapter_Oracle.FillSchema(FileContents, SchemaType.Source, Properties.SourceTableName);
            Adapter_Oracle.Fill(FileContents, Properties.SourceTableName);

            rowImportedFromSource = FileContents.Tables[0].Rows.Count;

            return FileContents;
        }

        public int RowImportedFromSource
        {
            get
            {
                return rowImportedFromSource;
            }

        }
    }
}