using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;

namespace MetaDataFromSQL
{
    class InsertUpdateCommands
    {
        public InsertUpdateCommands()
        {

        }

        //public void InsertToDataBase(DataRow RowToInsert, String Connection, string TableName, string ColumnNames)
        //{
        //    SqlCommand InsertCommand;
        //    SqlConnection Conn = new SqlConnection(Connection);
        //    string command = "INSERT INTO " + TableName + " (" + ColumnNames + ") VALUES (";
        //    string Values = null;
        //    int count = 0;
        //    int finalentry = RowToInsert.ItemArray.Count() -1;

        //    foreach (var item in RowToInsert.ItemArray)
        //    {
        //        if (count == finalentry)
        //        {
        //            Values += "'" + item + "');";
        //        }
        //        else
        //        {
        //            Values += "'" + item + "'" + ", ";
        //            count++;
        //        }
        //    }

        //    command += Values;

        //    InsertCommand = new SqlCommand(command, Conn);

        //    Conn.Open();

        //    try
        //    {
        //        InsertCommand.ExecuteNonQuery();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }



        //    Conn.Close();

        //}



        public void InsertToDataBase(DataRow RowToInsert, String Connection, string TableName, string ColumnNames)
        {
            SqlCommand InsertCommand;
            SqlConnection Conn = new SqlConnection(Connection);
            List<string> Parameters = new List<string>();

            string[] ParametersForInsert = ColumnNames.Split(",".ToArray());

            string command = "INSERT INTO " + TableName + " (" + ColumnNames + ") VALUES (";

            int count = 0;
            int finalentry = ParametersForInsert.ToArray().Count() -1;
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

            InsertCommand = new SqlCommand(command, Conn);

            count = 0;

            foreach (string item in Parameters)
            {

                InsertCommand.Parameters.AddWithValue(item, RowToInsert[ParametersForInsert[count]]);
                count++;
                

            }
                     

            Conn.Open();

            try
            {
                InsertCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }



            Conn.Close();

        }


        public void UpdateDataBase(DataRow RowToInsert, String Connection, string TableName, string ColumnNames, string Clause)
        {
            SqlCommand UpdateCommand;
            SqlConnection Conn = new SqlConnection(Connection);
            List<string> Parameters = new List<string>();

            string[] ParametersForInsert = ColumnNames.Split(",".ToArray());

            string command = "UPDATE " + TableName + " SET ";

            int count = 0;
            int finalentry = ParametersForInsert.ToArray().Count() - 1;
            foreach (string item in ParametersForInsert)
            {
                if (count == finalentry)
                {
                    command += item + " = @" + item;
                    Parameters.Add("@" + item);
                }
                else
                {
                    command += item + " = @" + item + ",";
                    Parameters.Add("@" + item);
                    count++;
                }
            }

            // Add Where Clause to command

            command += " WHERE " + Clause;


            UpdateCommand = new SqlCommand(command, Conn);

            count = 0;

            foreach (string item in Parameters)
            {
                UpdateCommand.Parameters.AddWithValue(item, RowToInsert[ParametersForInsert[count]]);
                count++;
            }


            Conn.Open();

            try
            {
                UpdateCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }



            Conn.Close();

        }

        public void DeleteEntry(string Clause, MetaData DataProperties)
        {
            SqlConnection conn = new SqlConnection(DataProperties.ConnectionString);
            SqlCommand com = new SqlCommand("DELETE FROM " + DataProperties.DestinationTable + " WHERE " + Clause, conn);

            conn.Open();
            try
            {
                com.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            conn.Close();
        }
    }
}
