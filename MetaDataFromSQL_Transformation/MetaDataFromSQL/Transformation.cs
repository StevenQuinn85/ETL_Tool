using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;

namespace MetaDataFromSQL
{
    class Transformation
    {
        DataTable acceptedData; 
        DataTable rejectedData;
        MetaData DataPropertiesForDataSet;

        public Transformation()
        {

        }

        public void TransformData(DataSet DataSet, MetaData DataProperities)
        {
            DataTable Table = new DataTable();
            DataPropertiesForDataSet = DataProperities;
            bool rowAcceptable = true;
            bool nullsAcceptable = false;
            bool replaceNull = false;
            string CurrentColumnName;
            Stopwatch watch;
            string Errors = "";
            Dictionary<string, string> ErrorsList = new Dictionary<string, string>();

            Table = DataSet.Tables[DataProperities.DataSetName];
            acceptedData = Table.Clone();
            rejectedData = Table.Clone();
            rejectedData.Columns.Add("Errors");
            watch = new Stopwatch();

            for (int i = 0; i < Table.Rows.Count; i++)
            {
                watch.Start();
                DataRow row = Table.Rows[i];
                //Identify the row for the error list
                Errors = "Row: " + i.ToString();


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
                                           Errors +=  " Column: " + j.ToString() + " Has a null value|"; 
                                        }


                            }
                    }

                    //Check 2.0 Is the intended data type a string
                    if (DataPropertiesForDataSet.DataTypes[CurrentColumnName].Equals("nvarchar"))
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
                    if (DataPropertiesForDataSet.DataTypes[CurrentColumnName].Equals("int"))
                    {
                        //Check 2.1 - check it can be converted to int
                        if (!CheckValueCanBeConvertedToInt(Table.Rows[i][j].ToString()))
                        {
                            rowAcceptable = false;
                            Errors += " Column: " + j.ToString() + " value cannot be converted to int|";
                        }
                            //Action - Convert value ???

                    }

                    //Check 4.0 Is the intended data type a decimal value
                    if (DataPropertiesForDataSet.DataTypes[CurrentColumnName].Equals("decimal"))
                    {
                        //Check 2.1 - check it can be converted to decimal
                        if (!CheckValueCanBeConvertedToFloat(Table.Rows[i][j].ToString()))
                        {
                            rowAcceptable = false;
                            Errors +=  " Column: " + j.ToString() + " value cannot be converted to decimal|";
                        }
                        //Action - Convert value ???

                    }

                    //Check 5.0 Is the intended data type a date value
                    if (DataPropertiesForDataSet.DataTypes[CurrentColumnName].Equals("date"))
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

                if (rowAcceptable)
                {
                    acceptedData.ImportRow(Table.Rows[i]);
                    //Reset row acceptable value
                    rowAcceptable = true;
                }
                else
                {
                    rejectedData.ImportRow(Table.Rows[i]);

                    ErrorsList.Add("Row: " + i.ToString(), Errors);
                    //Reset row acceptable value
                    rowAcceptable = true;
                }


            }

            watch.Stop();

        }

        public DataTable AcceptableData()
        {
            return acceptedData;
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

        private bool CheckMaxLength(object Value, string Column)
        {
            string Text = Value.ToString();
            char[] ArrayOfCharacter = Text.ToCharArray();
            bool CorrectMaxValue = false;

            if (ArrayOfCharacter.Count() <= DataPropertiesForDataSet.MaxLength[Column])
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

            if (ArrayOfCharacter.Count() >= DataPropertiesForDataSet.MinLength[Column])
            {
                CorrectMinValue = true;
            }

            return CorrectMinValue;
        }

        private string UpdateNullValue(string ColumnName)
        {
            string newValue = DataPropertiesForDataSet.ReplaceValue[ColumnName];

            return newValue;
        }

        private bool NullAction( string ColumnName)
        {
            bool replaceNullValue = false;

            if (DataPropertiesForDataSet.Action[ColumnName].Equals("Replace"))
            {
                replaceNullValue = true;
            }

            return replaceNullValue;
        }

        private bool CheckIfNullIsPermitted(string Value, string ColumnName)
        {
            bool checkIfNullIsPermitted = false;

            if (DataPropertiesForDataSet.NullPermitted[ColumnName].Equals(true))
            {
                checkIfNullIsPermitted = true;
            }
            

            return checkIfNullIsPermitted;
        }
    }
}
