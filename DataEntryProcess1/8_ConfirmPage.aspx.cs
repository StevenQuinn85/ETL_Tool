using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace DataEntryProcess1
{
    public partial class _8_ConfirmPage : System.Web.UI.Page
    {
        //int NumberOfColumns;
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        List<string> c_Names = new List<string>();
        List<string> c_Order = new List<string>();
        List<string> c_DataTypes = new List<string>();
        List<string> c_MinLenth = new List<string>();
        List<string> c_MaxLength = new List<string>();
        List<string> c_Nullable = new List<string>();
        List<string> c_NullAction = new List<string>();
        List<string> c_ReplaceValue = new List<string>();
        List<string> lst_PrimaryKeys = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];
            c_Names = (List<string>)(Session["ColumnNames"]);
            c_Order = (List<string>)(Session["ColumnOrder"]);
            c_DataTypes = (List<string>)(Session["DataTypes"]);
            lst_PrimaryKeys = (List<string>)(Session["PrimaryKeys"]);
            c_MinLenth = (List<string>)(Session["MinLength"]);
            c_MaxLength = (List<string>)(Session["MaxLength"]);
            c_Nullable = (List<string>)(Session["NullPermitted"]);
            c_NullAction = (List<string>)(Session["NullAction"]);
            c_ReplaceValue = (List<string>)(Session["ReplacementValue"]);

            DisplaySourceDetails();
            DisplayDestinationDetails();
            DisplayMetaData();
            DisplayPrimaryKeys();
        }

        private void DisplayPrimaryKeys()
        {
            StringBuilder TableText = new StringBuilder();

            TableText.AppendLine("<h4>Primary Keys</h4>");
            TableText.AppendLine("<table border = '" + "1" + "' cellpadding = '" + "2" + "'>");

            for (int i = 0; i < lst_PrimaryKeys.Count(); i++)
            {
                TableText.AppendLine("<tr>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(lst_PrimaryKeys[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("</tr>");
            }
            TableText.AppendLine("</table> </br>");
            PrimaryKeyDetails.Text = TableText.ToString();
        }

        private void DisplayDestinationDetails()
        {
            StringBuilder TableText = new StringBuilder();
            TableText.AppendLine("<h4>Data Destination Details</h4>");
            TableText.AppendLine("<table border = '" + "1" + "' cellpadding = '" + "2" + "'>");


            if (DataProperties["Destination Type"].Equals("SQL Server"))
            {
                string[] tableFields = {"Database Name", "Server Name", "UserName", "Password", "Table Name"};

                string[] desDetails = {DataProperties["Des_DB"], DataProperties["Des_Server_Name"], DataProperties["Des_Username"], DataProperties["Des_Password"], DataProperties["Des_Table_Name"] };

                for (int i = 0; i < desDetails.Count(); i++)
                {
                    TableText.AppendLine("<tr>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(tableFields[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(desDetails[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("</tr>");
                }

                TableText.AppendLine("</table> </br>");
            }
            else if (DataProperties["Destination Type"].Equals("Oracle Database"))
            {
                string[] tableFields = { "Table Name", "Data Source", "UserName", "Password", "Connection String"};

                string[] desDetails = { DataProperties["Destination Table Name"], DataProperties["Destination Data Source"], DataProperties["Destination Username"], DataProperties["Destination Password"], DataProperties["Destination Connection String"] };

                for (int i = 0; i < desDetails.Count(); i++)
                {
                    TableText.AppendLine("<tr>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(tableFields[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(desDetails[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("</tr>");
                }


            }
        else if (DataProperties["Destination Type"].Equals("Access Database"))
            {
                string[] tableFields = { "File Location", "Table Name", "Password", "Table Name" };

                string[] desDetails = { DataProperties["Destination File Location"], DataProperties["Destination Table Name"], DataProperties["Destination Password"], DataProperties["Destination Connection String"]};

                for (int i = 0; i < desDetails.Count(); i++)
                {
                    TableText.AppendLine("<tr>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(tableFields[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(desDetails[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("</tr>");
                }

            }



            TableText.AppendLine("</table> </br>");
            DataDestinationDetails.Text = TableText.ToString();
        }

        private void DisplayMetaData()
        {
            StringBuilder TableText = new StringBuilder();
            string[] tableFields = { "Column Order", "Column Name", "Data Type", "Min Length", "Max Length", "Nulls Permitted", "Null Action", "Replace Value" };
            TableText.AppendLine("<h4>Metadata Details</h4>");
            TableText.AppendLine("<table border = '" + "1" + "' cellpadding = '" + "2" + "'>");
            TableText.AppendLine("<tr>");

            for (int i = 0; i < tableFields.Count(); i++)
            {
                TableText.AppendLine("<th>");
                TableText.AppendLine(tableFields[i]);
                TableText.AppendLine("</th>");
            }

            TableText.AppendLine("</tr>");

            for (int i = 0; i < c_Order.Count(); i++)
            {
                TableText.AppendLine("<tr>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_Order[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_Names[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_DataTypes[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_MinLenth[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_MaxLength[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_Nullable[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_NullAction[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("<td>");
                TableText.AppendLine(c_ReplaceValue[i]);
                TableText.AppendLine("</td>");
                TableText.AppendLine("</tr>");
            }

            TableText.AppendLine("</table> </br>");

            MetaDataDisplay.Text = TableText.ToString();
        }

        private void DisplaySourceDetails()
        {
            StringBuilder TableText = new StringBuilder();

            TableText.AppendLine("<h4>Data Source Details</h4>");
            TableText.AppendLine("<table border = '" + "1" + "' cellpadding = '" + "2" + "'>");

            if (DataProperties["Import Type"].Equals("Feed File"))
            {
                string[] tableFields = { "Dataset Name", "File Location", "File Name", "Delimiter Character" };

                string[] sourceDetails = { DataProperties["Dataset Name"], DataProperties["File Location"], DataProperties["File Name"], DataProperties["Delimiter Character"] };

                for (int i = 0; i < sourceDetails.Count(); i++)
                {
                    TableText.AppendLine("<tr>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(tableFields[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("<td>");
                    TableText.AppendLine(sourceDetails[i]);
                    TableText.AppendLine("</td>");
                    TableText.AppendLine("</tr>");
                }


            }
            else if (DataProperties["Import Type"].Equals("Direct Connect"))
            {
                if (DataProperties["Source Database Type"].Equals("SQL Server"))
                {
                    string[] tableFields = { "Database", "Server Name", "Table Name", "Connection String", "Username", "Password" };

                    string[] sourceDetails = { DataProperties["Source Database Name"], DataProperties["Source Server Name"], DataProperties["Source Table Name"], DataProperties["Source Connection String"], DataProperties["Source Username"], DataProperties["Source Password"] };

                    for (int i = 0; i < sourceDetails.Count(); i++)
                    {
                        TableText.AppendLine("<tr>");
                        TableText.AppendLine("<td>");
                        TableText.AppendLine(tableFields[i]);
                        TableText.AppendLine("</td>");
                        TableText.AppendLine("<td>");
                        TableText.AppendLine(sourceDetails[i]);
                        TableText.AppendLine("</td>");
                        TableText.AppendLine("</tr>");
                    }

                }
                else if (DataProperties["Source Database Type"].Equals("Oracle Database"))
                {
                    string[] tableFields = { "Data Source", "Table Name", "Connection String", "Username", "Password" };

                    string[] sourceDetails = { DataProperties["Source Data Source"], DataProperties["Source Table Name"], DataProperties["Source Connection String"], DataProperties["Source Username"], DataProperties["Source Password"] };

                    for (int i = 0; i < sourceDetails.Count(); i++)
                    {
                        TableText.AppendLine("<tr>");
                        TableText.AppendLine("<td>");
                        TableText.AppendLine(tableFields[i]);
                        TableText.AppendLine("</td>");
                        TableText.AppendLine("<td>");
                        TableText.AppendLine(sourceDetails[i]);
                        TableText.AppendLine("</td>");
                        TableText.AppendLine("</tr>");
                    }

                }
                else if (DataProperties["Source Database Type"].Equals("Access Database"))
                {
                    string[] tableFields = { "Source Table Name", "Source File Location", "Connection String", "Password" };

                    string[] sourceDetails = { DataProperties["Source Table Name"], DataProperties["Source File Location"], DataProperties["Source Connection String"], DataProperties["Source Password"] };

                    for (int i = 0; i < sourceDetails.Count(); i++)
                    {
                        TableText.AppendLine("<tr>");
                        TableText.AppendLine("<td>");
                        TableText.AppendLine(tableFields[i]);
                        TableText.AppendLine("</td>");
                        TableText.AppendLine("<td>");
                        TableText.AppendLine(sourceDetails[i]);
                        TableText.AppendLine("</td>");
                        TableText.AppendLine("</tr>");
                    }

                }
            }

                TableText.AppendLine("</table> </br>");
            SourceDetailsDisplay.Text = TableText.ToString();
        }
    }
}