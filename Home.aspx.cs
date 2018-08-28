using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Text;

namespace ELTManagement
{
    public partial class Home : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAppConfiguration();
                LoadRecentImports();
                
            }

        }

        private void LoadRecentImports()
        {
            //Get the connection string from an App Config object
            ETLComponents.AppConfig Appdata = new ETLComponents.AppConfig();
            SqlConnection conn = new SqlConnection(Appdata.ConnectionString);
            string commandText = "SELECT TOP 8 H.ProcessName,S.StatusName,H.StartTime,H.EndTime,H.RowsInserted,H.RowsDeleted FROM [Project].[dbo].[JobRunHistory] H JOIN [dbo].[JobStatus] S ON H.StatusId = S.StatusID ORDER BY H.StartTime DESC; ";
            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;
            comm.CommandText = commandText;

            StringBuilder tableBuilder = new StringBuilder();

            tableBuilder.AppendLine("<table border = 1><tr><th>Process Name</th><th>Status</th><th>Start Time</th><th>End Time</th><th>Rows Inserted</th><th>Rows Deleted</th></tr>");

            try
            {
                conn.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tableBuilder.Append("<tr>");
                        tableBuilder.Append("<td>");
                        tableBuilder.Append(reader["ProcessName"].ToString());
                        tableBuilder.Append("</td>");
                        tableBuilder.Append("<td>");
                        tableBuilder.Append(reader["StatusName"].ToString());
                        tableBuilder.Append("</td>");
                        tableBuilder.Append("<td>");
                        tableBuilder.Append(reader["StartTime"].ToString());
                        tableBuilder.Append("</td>");
                        tableBuilder.Append("<td>");
                        tableBuilder.Append(reader["EndTime"].ToString());
                        tableBuilder.Append("</td>");
                        tableBuilder.Append("<td>");
                        tableBuilder.Append(reader["RowsInserted"].ToString());
                        tableBuilder.Append("</td>");
                        tableBuilder.Append("<td>");
                        tableBuilder.Append(reader["RowsDeleted"].ToString());
                        tableBuilder.Append("</td>");
                        tableBuilder.Append("</tr>");
                    }
                }

                conn.Close();
            }
            catch (Exception)
            {

                throw;
            }

            tableBuilder.Append("</table>");
            lit_RecentImportsTable.Text = tableBuilder.ToString();
        }

        //This method loads the connection string to the BackEnd DB
        //and the location of the log files
        private void LoadAppConfiguration()
        {
            //Retrieve the Connection String and Log File Location
            string FileLocation = HttpContext.Current.Server.MapPath("/").ToString();

            //remove the final '\ETLManagement'
            int positionOf_E = FileLocation.LastIndexOf("E");
            FileLocation = FileLocation.Remove(positionOf_E, 14);

            //add the root location where the app data is stored
            FileLocation += @"root\appdata.txt";


            StreamReader reader = new StreamReader(FileLocation);

            string[] appData = new string[2];
            int count = 0;

            //read the contents of the file to retrive the location data
            while (!reader.EndOfStream)
            {
                if (count<=1)
                {
                    appData[count] = reader.ReadLine();
                    count++;
                }

            }

            //The string in position zero will be the connection string
            string[] lineOne = appData[0].Split('#');
            string connectionString = lineOne[1];
            //The string in position one will be the log location
            string[] lineTwo = appData[1].Split('#');
            string logFileLocation = lineTwo[1];

        }
    }
}