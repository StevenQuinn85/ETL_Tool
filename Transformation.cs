using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;

namespace ETLComponents
{
    //This class will clean the raw data that is extracted from the source and 
    //provide an accepted dataset
    public class Transformation
    {
        DataTable acceptedData; 
        MetaData MetaDataRules;

        //private ints to hold the number of rows accepted or rejected
        private int rowsAcceptedCount, rowsRejectedCount;

        //Dictionary containing the reject rows and reason for rejection
        Dictionary<string, string> errorsList = new Dictionary<string, string>();



        public Transformation()
        {

        }

        public void TransformData(DataSet DataSet, DataProperties DataProp)
        {
            //Reset the counts of Rows accepted vs rejected
            rowsAcceptedCount = 0; rowsRejectedCount = 0;

            DataTable Table = new DataTable();
            MetaDataRules = DataProp.Meta;
            bool rowAcceptable = true;
            bool nullsAcceptable = false;
            bool replaceNull = false;
            string CurrentColumnName;
            string Errors = "";
            string rowId = "";


            if (DataProp.ImportType.Equals("Direct Connect"))
            {
            Table = DataSet.Tables[DataProp.SourceTableName];
            }
            else
            {
                Table = DataSet.Tables[DataProp.DataSetName];
            }

            
            acceptedData = Table.Clone();

            for (int i = 0; i < Table.Rows.Count; i++)
            {
                
                DataRow row = Table.Rows[i];
                //Identify the row for the error list

                foreach (var item in row.ItemArray)
                {
                    rowId += item + "|";
                }




                for (int j = 0; j < Table.Columns.Count; j++)
                {
                    //Copy Current Column Name to feed to different methods
                    CurrentColumnName = Table.Columns[j].ToString();

                    //Check 1.0 - check if value is null
                    if (string.IsNullOrEmpty(row[j].ToString()))
                    {
                        // Check 1.1 Check if Null is Permitted?
                        nullsAcceptable = CheckIfNullIsPermitted(row[j].ToString(), CurrentColumnName);
                        if (!nullsAcceptable)
                        {
                            //Check1.2 Check what action to take against Null Values.
                            replaceNull = NullAction(CurrentColumnName);
                            if (replaceNull)
                            {
                                //Replace the Null value with default value for this field
                                Table.Rows[i][j] = UpdateNullValue(CurrentColumnName);

                            }
                            else
                            {
                                //Reject the data Row
                                rowAcceptable = false;
                                Errors += " Column: " + j.ToString() + " Has a null value|";
                            }
                        }
                    }

                    //If after the first check the value is null the remain check for data type can be skipped
                    //If the value is not null proceed with the checks
                    if (!string.IsNullOrEmpty(row[j].ToString()))
                    {

                        //Check 2.0 Is the intended data type a string
                        if (MetaDataRules.DataType[CurrentColumnName].Equals("Text"))
                        {
                            //Check 2.1 - check min length
                            if (!CheckMinLength(Table.Rows[i][j], CurrentColumnName))
                            {
                                rowAcceptable = false;
                                Errors += " Column: " + j.ToString() + " value below minimum threshold|";

                            }

                            //Check 2.1 - check max length
                            if (!CheckMaxLength(Table.Rows[i][j], CurrentColumnName))
                            {
                                rowAcceptable = false;
                                Errors += " Column: " + j.ToString() + " value above maximum threshold|";
                            }
                        }

                        //Check 3.0 Is the intended data type an integer value
                        if (MetaDataRules.DataType[CurrentColumnName].Equals("Int"))
                        {
                            //Check 2.1 - check it can be converted to int
                            if (!CheckValueCanBeConvertedToInt(Table.Rows[i][j].ToString()))
                            {
                                rowAcceptable = false;
                                Errors += " Column: " + j.ToString() + " value cannot be converted to int|";
                            }

                        }

                        //check 4.0 Is the intended data type an Big Int
                        if (MetaDataRules.DataType[CurrentColumnName].Equals("BitInt"))
                        {
                            if (!CheckValueCanBeConvertedToBigInt(Table.Rows[i][j].ToString()))
                            {
                                rowAcceptable = false;
                                Errors += " Column: " + j.ToString() + " value cannot be converted to Big int|";
                            }
                        }

                        //Check 5.0 Is the intended data type a decimal value
                        if (MetaDataRules.DataType[CurrentColumnName].Equals("Decimal"))
                        {
                            //Check 2.1 - check it can be converted to decimal
                            if (!CheckValueCanBeConvertedToFloat(Table.Rows[i][j].ToString()))
                            {
                                rowAcceptable = false;
                                Errors += " Column: " + j.ToString() + " value cannot be converted to decimal|";
                            }
                            //Action - Convert value ???

                        }

                        //Check 6.0 Is the intended data type a date value
                        if (MetaDataRules.DataType[CurrentColumnName].Equals("Date"))
                        {
                            //Check 2.1 - check it can be converted to date
                            if (!CheckValueCanBeConvertedToDate(Table.Rows[i][j].ToString()))
                            {
                                rowAcceptable = false;
                                Errors += " Column: " + j.ToString() + " value cannot be converted to date|";
                            }
                            else
                            {
                                //Action - Convert value to date format
                                Table.Rows[i][j] = ConvertValueToDateFormat(Table.Rows[i][j].ToString());
                            }
                        }

                    }

                    //if (rowAcceptable)
                    //{
                    //    acceptedData.ImportRow(Table.Rows[i]);
                    //    rowsAcceptedCount++;

                    //    //Reset row acceptable value
                    //    rowAcceptable = true;
                    //    rowId = "";
                    //    Errors = "";
                    //}
                    //else
                    //{
                    //    errorsList.Add("Row: " + rowId, Errors);

                    //    rowsRejectedCount++;

                    //    //Reset row acceptable value
                    //    rowAcceptable = true;
                    //    rowId = "";
                    //    Errors = "";
                    //}

                }

                if (rowAcceptable)
                {
                    acceptedData.ImportRow(Table.Rows[i]);
                    rowsAcceptedCount++;

                    //Reset row acceptable value
                    rowAcceptable = true;
                    rowId = "";
                    Errors = "";
                }
                else
                {
                    errorsList.Add("Row: " + rowId, Errors);

                    rowsRejectedCount++;

                    //Reset row acceptable value
                    rowAcceptable = true;
                    rowId = "";
                    Errors = "";
                }
            }

        }



        public DataSet AcceptableData()
        {
            DataSet CleanData = new DataSet();
            CleanData.Tables.Add(acceptedData);
            return CleanData;
        }


        private object ConvertValueToDateFormat(string Value)
        {
            string FormattedValue;
            DateTime Entry = Convert.ToDateTime(Value);

            FormattedValue = Entry.ToShortDateString();

            return FormattedValue;

        }

        private bool CheckValueCanBeConvertedToDate(string Value)
        {
            bool ValueCanBeConverted = false;
            DateTime testValue = new DateTime();

            if (DateTime.TryParse(Value, out testValue))
            {
                ValueCanBeConverted = true;
            }

            return ValueCanBeConverted;
        }

        private bool CheckValueCanBeConvertedToFloat(string Value)
        {
            bool ValueCanBeConverted = false;
            decimal testValue;

            if (decimal.TryParse(Value, out testValue))
            {
                ValueCanBeConverted = true;
            }

            return ValueCanBeConverted;
        }

        private bool CheckValueCanBeConvertedToInt(string Value)
        {
            bool ValueCanBeConverted = false;
            int testValue;

            if (int.TryParse(Value, out testValue))
            {
                ValueCanBeConverted = true;
            }

            return ValueCanBeConverted;
        }

        private bool CheckValueCanBeConvertedToBigInt(string value)
        {
            bool ValueCanBeConverted = false;
            Int64 testValue;

            if (Int64.TryParse(value, out testValue))
            {
                ValueCanBeConverted = true;
            }

            return ValueCanBeConverted;
        }

        private bool CheckMaxLength(object Value, string Column)
        {
            string Text = Value.ToString();
            char[] ArrayOfCharacter = Text.ToCharArray();
            bool CorrectMaxValue = false;

            if (ArrayOfCharacter.Count() <= MetaDataRules.MaxLength[Column])
            {
                CorrectMaxValue = true;
            }

            return CorrectMaxValue;
        }

        private bool CheckMinLength(object Value, string Column)
        {
            string Text = Value.ToString();
            char[] ArrayOfCharacter = Text.ToCharArray();
            bool CorrectMinValue = false;

            if (ArrayOfCharacter.Count() >= MetaDataRules.MinLength[Column])
            {
                CorrectMinValue = true;
            }

            return CorrectMinValue;
        }

        private string UpdateNullValue(string ColumnName)
        {
            string newValue = MetaDataRules.ReplaceValues[ColumnName];

            return newValue;
        }

        private bool NullAction( string ColumnName)
        {
            bool replaceNullValue = false;

            if (MetaDataRules.NullAction[ColumnName].Equals("Replace"))
            {
                replaceNullValue = true;
            }

            return replaceNullValue;
        }

        private bool CheckIfNullIsPermitted(string Value, string ColumnName)
        {
            bool checkIfNullIsPermitted = false;

            if (MetaDataRules.NullsPermitted[ColumnName].Equals(true))
            {
                checkIfNullIsPermitted = true;
            }
            

            return checkIfNullIsPermitted;
        }

        public Dictionary<string, string> ErrorsList
        {
            get
            {
                return errorsList;
            }

        }

        public int RowsAcceptedCount
        {
            get
            {
                return rowsAcceptedCount;
            }

        }

        public int RowsRejectedCount
        {
            get
            {
                return rowsRejectedCount;
            }

        }
    }
}
