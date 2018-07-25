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
            //Add the tablename to the FileContents Dataset

            string commandText = "Select * from " + Properties.SourceTableName;

            if (Properties.UseLookBack)
            {
                //Use look back query
            }


            SqlConnection conn = new SqlConnection(Properties.SourceConnectionString);
            SqlDataAdapter Adapter = new SqlDataAdapter(commandText, conn);

            Adapter.FillSchema(FileContents, SchemaType.Source, Properties.SourceTableName);
            Adapter.Fill(FileContents, Properties.SourceTableName);

            rowImportedFromSource = FileContents.Tables[0].Rows.Count;

            return FileContents;
        }

        public DataSet AccessImport(DataProperties Properties)
        {
            //DataSet Object that will contain contents of file
            DataSet FileContents = new DataSet();
            string commandText = "Select * from " + Properties.SourceTableName;
            OleDbConnection conn = new OleDbConnection(Properties.SourceConnectionString);
            OleDbDataAdapter Adapter = new OleDbDataAdapter(commandText, conn);

            Adapter.FillSchema(FileContents, SchemaType.Source, Properties.SourceTableName);
            Adapter.Fill(FileContents, Properties.SourceTableName);

            rowImportedFromSource = FileContents.Tables[0].Rows.Count;
           
            return FileContents;
        }

        public DataSet OracleImport(DataProperties Properties)
        {
            //DataSet Object that will contain contents of file
            DataSet FileContents = new DataSet();

            string commandText = "Select * from " + Properties.SourceTableName;

            OracleConnection conn_oracle = new OracleConnection(Properties.SourceConnectionString);
            OracleDataAdapter Adapter_Oracle = new OracleDataAdapter(commandText, conn_oracle);

            //OleDbConnection conn_oracle = new OleDbConnection(Properties.SourceConnectionString);
            //OleDbDataAdapter Adapter_Oracle = new OleDbDataAdapter(commandText, conn_oracle);

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