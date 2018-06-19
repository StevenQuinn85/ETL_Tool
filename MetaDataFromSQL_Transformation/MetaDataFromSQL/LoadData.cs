using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MetaDataFromSQL
{
    class LoadData
    {
        //Class to Load data (Insert or update) into destination table

        //DataTable Object to hold the contents of dataset that is being imports
        DataTable Table = new DataTable();
        
        MetaData dataProperties;
        InsertUpdateCommands ExportToDb = new InsertUpdateCommands();
        CheckIfRecordExists RecordChecker = new CheckIfRecordExists();

        public LoadData(DataSet FileContents, MetaData DataProperties )
        {
            dataProperties = DataProperties;
            Table = FileContents.Tables[dataProperties.DataSetName];

            foreach (DataTable item in FileContents.Tables)
            {
                foreach (DataRow row in item.Rows )
                {
                    string Clause = CreateWhereClause(row, dataProperties.DestinationTable, dataProperties.PrimaryKey);
                    string CountClause = "Select COUNT(*) FROM " + dataProperties.DestinationTable + " WHERE " + Clause;
                    string ColumnNames = "";

                    int count = 0;

                    foreach (string name in dataProperties.ColumnNames)
                    {
                        if (count < dataProperties.ColumnNames.Count -1)
                        {
                            ColumnNames += name + ",";
                        }
                        else
                        {
                            ColumnNames += name;
                        }
                        count++;
                    }
                    


                    bool RecordExists = RecordChecker.CheckForRecord(dataProperties.ConnectionString, CountClause);

                    if (RecordExists)
                    {
                        // Update Record
                        //ExportToDb.UpdateDataBase(row, dataProperties.ConnectionString, dataProperties.DestinationTable, ColumnNames, Clause);
                        //delete record - blast
                        ExportToDb.DeleteEntry(Clause, dataProperties);

                        //then insert reload
                        ExportToDb.InsertToDataBase(row, dataProperties.ConnectionString, dataProperties.DestinationTable, ColumnNames);

                    }

                    else
                    {
                        ExportToDb.InsertToDataBase(row, dataProperties.ConnectionString, dataProperties.DestinationTable, ColumnNames);
                    }

                }
            }

        }

        public string CreateWhereClause(DataRow Row, string DestinationTable, string[] PKeys)
        {
            string Clause = null;
            int i = 0;

            foreach (string item in PKeys)
            {
                if (i == 0)
                {
                    Clause += item + " = '" + Row[item].ToString() + "'";
                    i++;
                }
                else
                    Clause += " AND " + item + " = '" + Row[item].ToString() + "'";
            }

            Clause += ";";

            return Clause;
        }



    }
}
