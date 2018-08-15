using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.OracleClient;

namespace ELTManagement
{
    class DataBaseExtraction: Extraction
    {
        public DataBaseExtraction(DataProperties DataProp): base (DataProp)
        {

        }

        public DataSet SQLImport()
        {
            //DataSet Object that will contain contents of file
            //DataSet FileContents = new DataSet();
            RawData = new DataSet();
            DateTime LookBackDate = new DateTime();

            SqlConnection conn = new SqlConnection(dataProp.SourceConnectionString);
            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;

            string commandText = "Select * from " + dataProp.SourceTableName;

            if (dataProp.UseLookBack)
            {
                commandText += " WHERE " + dataProp.LookBackColumnName + " >= @LookBackPeroid";

                //Set the look back date to be one second prior to midnight on the date of the look back period.
                LookBackDate = DateTime.Now.AddDays(-dataProp.LookBackPeriod - 1).AddHours(-DateTime.Now.Hour + 23).AddMinutes(-DateTime.Now.Minute + 59).AddSeconds(-DateTime.Now.Second + 59);

                comm.Parameters.AddWithValue("@LookBackPeroid", LookBackDate);
                comm.Parameters["@LookBackPeroid"].SqlDbType = SqlDbType.DateTime;
            }

            comm.CommandText = commandText;

            //Pass the SQL Command with the command text, parameters and connection into the adapter
            SqlDataAdapter Adapter = new SqlDataAdapter(comm);

            Adapter.FillSchema(RawData, SchemaType.Source, dataProp.SourceTableName);
            Adapter.Fill(RawData, dataProp.SourceTableName);

            rowImportedFromSource = RawData.Tables[0].Rows.Count;

            return RawData;
        }

        internal DataSet AccessImport()
        {
            //DataSet Object that will contain contents of file
            //DataSet FileContents = new DataSet();
            RawData = new DataSet();
            DateTime LookBackDate = new DateTime();

            OleDbConnection conn = new OleDbConnection(dataProp.SourceConnectionString);
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = conn;

            string commandText = "Select * from " + dataProp.SourceTableName;

            if (dataProp.UseLookBack)
            {
                commandText += " WHERE " + dataProp.LookBackColumnName + " >= @LookBackPeroid";

                //Set the look back date to be one second prior to midnight on the date of the look back period.
                LookBackDate = DateTime.Now.AddDays(-dataProp.LookBackPeriod - 1).AddHours(-DateTime.Now.Hour + 23).AddMinutes(-DateTime.Now.Minute + 59).AddSeconds(-DateTime.Now.Second + 59);

                comm.Parameters.AddWithValue("@LookBackPeroid", LookBackDate);
                comm.Parameters["@LookBackPeroid"].OleDbType = OleDbType.DBDate;
            }

            comm.CommandText = commandText;

            OleDbDataAdapter Adapter = new OleDbDataAdapter(comm);

            Adapter.FillSchema(RawData, SchemaType.Source, dataProp.SourceTableName);
            Adapter.Fill(RawData, dataProp.SourceTableName);

            rowImportedFromSource = RawData.Tables[0].Rows.Count;

            return RawData;
        }

        internal DataSet OracleImport()
        {
            //DataSet Object that will contain contents of file
            //DataSet FileContents = new DataSet();
            RawData = new DataSet();
            DateTime LookBackDate = new DateTime();

            OracleConnection conn_oracle = new OracleConnection(dataProp.SourceConnectionString);
            OracleCommand comm = new OracleCommand();
            comm.Connection = conn_oracle;

            string commandText = "Select * from " + dataProp.SourceTableName;

            if (dataProp.UseLookBack)
            {
                commandText += " WHERE " + dataProp.LookBackColumnName + " >= :LookBackPeroid";

                //Set the look back date to be one second prior to midnight on the date of the look back period.
                LookBackDate = DateTime.Now.AddDays(-dataProp.LookBackPeriod - 1).AddHours(-DateTime.Now.Hour + 23).AddMinutes(-DateTime.Now.Minute + 59).AddSeconds(-DateTime.Now.Second + 59);

                comm.Parameters.Add(new OracleParameter("LookBackPeroid", LookBackDate));
                comm.Parameters["LookBackPeroid"].OracleType = OracleType.DateTime;
            }

            comm.CommandText = commandText;

            //Pass the SQL Command with the command text, parameters and connection into the adapter
            OracleDataAdapter Adapter_Oracle = new OracleDataAdapter(comm);

            Adapter_Oracle.FillSchema(RawData, SchemaType.Source, dataProp.SourceTableName);
            Adapter_Oracle.Fill(RawData, dataProp.SourceTableName);

            rowImportedFromSource = RawData.Tables[0].Rows.Count;

            return RawData;
        }
    }
}
