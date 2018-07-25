using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ELTManagement
{
    public class MetaData
    {
        SqlConnection conn;

        public List<string> ColumnName = new List<string>();
        public List<string> PrimaryKey = new List<string>();

        public Dictionary<string, int> ColumnOrder = new Dictionary<string, int>();
        public Dictionary<string, string> DataType = new Dictionary<string, string>();
        public Dictionary<string, int> MinLength = new Dictionary<string, int>();
        public Dictionary<string, int> MaxLength = new Dictionary<string, int>();
        public Dictionary<string, bool> NullsPermitted = new Dictionary<string, bool>();
        public Dictionary<string, string> NullAction = new Dictionary<string, string>();
        public Dictionary<string, string> ReplaceValues = new Dictionary<string, string>();

        public MetaData(SqlConnection Conn)
        {
            conn = Conn;

        }

        internal void PopulateMetaData(object processId)
        {
            string commandText_Meta = "SELECT ColumnName, DataType,MinLength,MaxLength,NullsPermitted,NullAction,ReplaceValue,ColumnOrder FROM [Project].[dbo].[Program_Metadata] WHERE ProcessId = @ProcessId ORDER BY ColumnOrder ASC";
            SqlCommand comm = new SqlCommand(commandText_Meta, conn);
            comm.Parameters.AddWithValue("@ProcessId", processId);

            conn.Open();
            using (SqlDataReader reader = comm.ExecuteReader())
            {
                while (reader.Read())
                {
                    string columnName = reader["ColumnName"].ToString();

                    ColumnName.Add(reader["ColumnName"].ToString());
                    ColumnOrder.Add(columnName, Convert.ToInt32(reader["ColumnOrder"]));
                    DataType.Add(columnName, reader["DataType"].ToString());
                    MinLength.Add(columnName, Convert.ToInt32(reader["MinLength"]));
                    MaxLength.Add(columnName, Convert.ToInt32(reader["MaxLength"]));
                    NullAction.Add(columnName, reader["NullAction"].ToString());
                    ReplaceValues.Add(columnName, reader["ReplaceValue"].ToString());

                    string nullPermit = reader["NullsPermitted"].ToString();

                    if (nullPermit.Equals("Yes"))
                    {
                        NullsPermitted.Add(columnName, true);
                    }
                    else if (nullPermit.Equals("No"))
                    {
                        NullsPermitted.Add(columnName, false);
                    }

                }
            }

            conn.Close();
        }

        internal void PopulatePrimaryKeys(object processId)
        {
            string commandText = "SELECT PrimaryKey FROM [Project].[dbo].[Program_PrimaryKeyData] WHERE ProcessId = @ProcessId";
            SqlCommand comm = new SqlCommand(commandText, conn);
            comm.Parameters.AddWithValue("@ProcessId", processId);

            conn.Open();
            using (SqlDataReader reader = comm.ExecuteReader())
            {
                while (reader.Read())
                {
                    PrimaryKey.Add(reader["PrimaryKey"].ToString());

                }
            }

            conn.Close();
        }
    }
}