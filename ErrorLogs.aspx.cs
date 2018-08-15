using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace ELTManagement
{
    public partial class ErrorLogs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateErrorLogsTable();
            }
        }

        private void PopulateErrorLogsTable()
        {
            StringBuilder tableText = new StringBuilder();
            SqlConnection conn = new SqlConnection();
            SqlCommand comm = new SqlCommand();
            string commandText = "  SELECT ProcessName, StartTime, LogName FROM [Project].[dbo].[JobRunHistory] WHERE LogName IS NOT NULL ORDER BY StartTime DESC";
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["BackEndDB"].ConnectionString;

            string processName, startTime, logLocation;

            comm.Connection = conn;
            comm.CommandText = commandText;

            tableText.Append("<table border = '" + "1" + "' cellpadding = '" + "2" + "'><tr><th>Process Name </th><th>Start Time</th><th>Log File</th></tr >");

            try
            {
                comm.Connection.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        processName = reader["ProcessName"].ToString();
                        startTime = reader["StartTime"].ToString();
                        logLocation = reader["LogName"].ToString();

                        tableText.Append("<tr><td>"+processName+"</td><td>"+startTime+ "</td><td><a href='ErrorLogsContent.aspx?logName="+logLocation+"'>Open Log</a></td></tr>");
                    }

                    
                }

                comm.Connection.Close();
            }
            catch (Exception)
            {



                throw;
            }

            tableText.Append("</table>");
            ErrorLogsTable.Text = tableText.ToString();

        }
    }
}