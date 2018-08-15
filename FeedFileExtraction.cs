using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace ELTManagement
{
    class FeedFileExtraction:Extraction
    {
        //This class contains the code to open all file and read entries.
        //It returns the entries as a dataset object.

        static StreamReader Reader;

        private int rowRejectedFromSource;
        string rejectionMessage;
        List<string> listOfRejections = new List<string>();

        public FeedFileExtraction(DataProperties DataProp): base (DataProp) 
        {
            
        }

        public DataSet ImportFile()
        {
            //Set the import details to be zero
            rowImportedFromSource = 0;
            rowRejectedFromSource = 0;
            
            string FileLocation = dataProp.SourcefileLocation;
            string TableName = dataProp.DataSetName;
            string DelimiterChar = dataProp.SourceDelimiterChar;
            int numberOfColumns = dataProp.Meta.ColumnName.Count;
            //DataSet Object that will contain contents of file
            RawData = new DataSet();
            //Add the tablename to the FileContents Dataset
            RawData.Tables.Add(TableName);
            //Stream Reader Object to open and read file contents
            Reader = new StreamReader(FileLocation);

            //Get the Column Names from the file 
            string[] Columns = GetColumns(FileLocation, DelimiterChar);

            //Add the Column to the first row of the table
            RawData = SetTableColumns(RawData, Columns, TableName);

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
                    RawData = AddEntry(row, RawData, DelimiterChar, TableName, numberOfColumns);
                    i++;
                }
            }
            //Return the Dataset consisting of the File Contents
            return RawData;
        }

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
                rowImportedFromSource++;
            }
            else
            {
                //Create a reject message
                rowRejectedFromSource++;
                //To gather the row number add the rows imported and reject, plus 1 to account for headers in file
                int rowNumber = rowImportedFromSource + rowRejectedFromSource + 1;
                rejectionMessage = "Row: " + rowNumber.ToString() + " Rejected.  Invalid number of columns";
                listOfRejections.Add(rejectionMessage);
            }

            return fileContents;
        }


        internal int RecordsRejected
        {
            get
            {
                return rowRejectedFromSource;
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
