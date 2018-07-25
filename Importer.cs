using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace ELTManagement
{
    public class Importer
    {
        //This class contains the code to open all file and read entries.
        //It returns the entries as a dataset object.

        static StreamReader Reader;

        private int recordsRejected;
        private int recordsImported;
        string rejectionMessage;
        List<string> listOfRejections = new List<string>();



        public Importer()
        {
        }

        public DataSet ImportFile(DataProperties Data)
        {
            //Set the import details to be zero
            recordsImported = 0;
            recordsRejected = 0;

            string FileLocation = Data.SourcefileLocation;
            string TableName = Data.DataSetName;
            string DelimiterChar = Data.SourceDelimiterChar;
            int numberOfColumns = Data.Meta.ColumnName.Count;
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
                //Omit the first line as this is the column headers
                if (i == 0)
                {
                    i++;
                }
                else
                {
                    FileContents = AddEntry(row, FileContents, DelimiterChar, TableName, numberOfColumns);
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
        private DataSet AddEntry(string row, DataSet fileContents, string delimterChar, string tableName, int numOfCols)
        {

            string[] entry = row.Split(delimterChar.ToCharArray());

            //Only Accept entries with the correct number of columns
            if (entry.Count() == numOfCols)
            {
                foreach (string item in entry)
            {
                item.Trim();
            }
                //Add entry to dataset
            fileContents.Tables[tableName].Rows.Add(entry);
                recordsImported++;
            }
            else
            {
                //Create a reject message
                recordsRejected++;
                rejectionMessage = "Row: " + recordsImported + 1 + " Rejected.  Invalid number of columns";
                listOfRejections.Add(rejectionMessage);
            }



            return fileContents;
        }

        internal int RecordsImported
        {
            get
            {
                return recordsImported;
            }

        }

        internal int RecordsRejected
        {
            get
            {
                return recordsRejected;
            }

        }

        public List<string> ListOfRejections
        {
            get
            {
                return listOfRejections;
            }
        }

    }
}
