using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace MetaDataFromSQL
{
    public class Importer
    {
        //This class contains the code to open all file and read entries.
        //It returns the entries as a dataset object.

        static StreamReader Reader;

        public Importer()
        {
        }

        public DataSet ImportFile(string FileLocation, string TableName, string DelimiterChar, int NumberOfColumns)
        {
            //DataSet Object that will contain contents of file
            DataSet FileContents = new DataSet();
            //Add the tablename to the FileContents Dataset
            FileContents.Tables.Add(TableName);
            //Stream Reader Object to open and read file contents
            Reader = new StreamReader(FileLocation);

            //Get the Column Names from the file 
            string[] Columns = GetColumns(FileLocation, DelimiterChar);

            //Add the Column to the first row of the table
            FileContents = SetTableColumns(FileContents, Columns, TableName);

            //Add the rest of the contents, omit the first line containing headers
            int i = 0;
            foreach (string row in File.ReadAllLines(FileLocation))
            {
                if (i == 0)
                {
                    i++;
                }
                else
                {
                    FileContents = AddEntry(row, FileContents, DelimiterChar, TableName);
                    i++;
                }
            }
            //Return the Dataset consisting of the File Contents
            return FileContents;
        }


        //Method to extract the columns/headers to the dataset
        private static string[] GetColumns(string fileLocation, string delimiterChar)
        {
            string[] Columns = Reader.ReadLine().Split(delimiterChar.ToCharArray());

            return Columns;
        }

        //Method to add the columns/headers to the dataset
        private static DataSet SetTableColumns(DataSet fileContents, string[] columns, string TableName)
        {
            //Boolean value to check if the column already exists.
            //bool exist = false;

            foreach (string item in columns)
            {
                string CleanColumn = item.Trim();

                if (!fileContents.Tables[TableName].Columns.Contains(CleanColumn))
                {
                    fileContents.Tables[TableName].Columns.Add(CleanColumn);
                    //exist = true;
                }
            }

            return fileContents;
        }

        //Method to add each row to the dataset
        private static DataSet AddEntry(string row, DataSet fileContents, string delimterChar, string tableName)
        {
            delimterChar.Trim();

            char[] limit = delimterChar.ToCharArray();

            char delimit = limit[0];

            string[] entry = row.Split(delimit);

            fileContents.Tables[tableName].Rows.Add(entry);

            return fileContents;
        }
    }
}
