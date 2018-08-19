using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace ELTManagement
{
    public partial class Reporting : System.Web.UI.Page
    {
        private string importName;
        private DateTime StartDate = new DateTime();
        private DateTime EndDate = new DateTime();
        private TimeSpan oneWeek = new TimeSpan(7,0,0,0);

        //private SqlConnection conn = new SqlConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InputConversionNames();

                DateTime lastWeek = DateTime.Today.AddDays(-60);
                cal_StartDate.TodaysDate = lastWeek;
                cal_StartDate.SelectedDate = cal_StartDate.TodaysDate;

                DateTime today = DateTime.Today;
                cal_EndDate.TodaysDate = today;
                cal_EndDate.SelectedDate = cal_EndDate.TodaysDate;
            }
        }

        private void InputConversionNames()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["BackEndDB"].ConnectionString;
            List<string> listOfConversions = new List<string>();
            string commandText = "SELECT DISTINCT [DataSetName] FROM [Project].[dbo].[Program_DataProperties]";

            SqlCommand comm = new SqlCommand(commandText, conn);

            using (conn)
            {
                using (comm)
                {
                    conn.Open();

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listOfConversions.Add(reader["DataSetName"].ToString());
                        }
                    }

                    conn.Close();
                }
            }

            drp_ImportProcesses.DataSource = listOfConversions;
            drp_ImportProcesses.DataBind();
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["BackEndDB"].ConnectionString;
            SqlCommand comm = new SqlCommand();

            importName = drp_ImportProcesses.SelectedValue.ToString();

            StartDate = cal_StartDate.SelectedDate;
            EndDate = cal_EndDate.SelectedDate.AddDays(1);

            comm = GenerateSQLQueryForReport();

            comm.Connection = conn;

           ReportTable.Text = GenerateReportTableString(comm);


        }

        private string GenerateReportTableString(SqlCommand comm)
        {
            //String builder to create the HTMl code for the report table
            StringBuilder tableText = new StringBuilder();

            //Strings to hold the values extracted from the sql command
            string processName, status, startTime, endTime, duration, insert, delete, reject;

            //Add the first line of text to create the table and header rows;
            tableText.Append("<table border = '" + "1" + "' cellpadding = '" + "2" + "'> <tr><th> Process Name </th ><th> Status </th ><th> Start Time </th > <th> End Time </th ><th> Duration(seconds) </th><th> Inserted </th><th> Deleted </th><th> Rejected </th ></tr> ");
            int count = 0;
            try
            {
                comm.Connection.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        processName = reader["ProcessName"].ToString();
                        status = reader["StatusName"].ToString();
                        startTime = reader["StartTime"].ToString();
                        endTime = reader["EndTime"].ToString();
                        duration = reader["Duration In Seconds"].ToString();
                        insert = reader["RowsInserted"].ToString();
                        delete = reader["RowsDeleted"].ToString();
                        reject = reader["RowsRejected"].ToString();

                        string tableRow = "<tr>" + "<td>" +processName+ "</td>" + "<td>" + status + "</td>" + "<td>" + startTime + "</td>" + "<td>" + endTime + "</td>" + "<td>" + duration + "</td>" + "<td>" + insert + "</td>" + "<td>" + delete + "</td>" + "<td>" + reject + "</td> </tr>";
                        tableText.Append(tableRow);
                        
                    }
                }

                tableText.Append("</table>");

                comm.Connection.Close();
            }
            catch (Exception)
            {

                throw;
            }

            return tableText.ToString();
        }

        private SqlCommand GenerateSQLQueryForReport()
        {
            SqlCommand comm = new SqlCommand();
            string[] parametersForQuery = {"ProcessName", "StartTime", "EndTime"};

            string commandText = "SELECT H.[JobID],H.[ProcessName],S.StatusName,H.[StartTime],H.[EndTime], DATEDIFF(SECOND, H.StartTime, H.EndTime) AS 'Duration In Seconds',H.[RowsInserted],H.[RowsDeleted],H.[RowsRejected] FROM [Project].[dbo].[JobRunHistory] H JOIN [dbo].[JobStatus] S ON H.StatusId = S.StatusID ";

            int count = 0;
            foreach (string item in parametersForQuery)
            {
                if (count == 0)
                {
                    commandText += "WHERE " + item + " = @" + item;
                }
                else if (count == 1)
                {
                    commandText += " AND " + item + " >= @" + item;
                }
                else
                {
                    commandText += " AND " + item + " <= @" + item;
                }

                count++;
            }

            comm.Parameters.AddWithValue("@ProcessName", importName);
            comm.Parameters["@ProcessName"].SqlDbType = System.Data.SqlDbType.NVarChar;
            comm.Parameters.AddWithValue("@StartTime", StartDate);
            comm.Parameters["@StartTime"].SqlDbType = System.Data.SqlDbType.Date;
            comm.Parameters.AddWithValue("@EndTime",EndDate);
            comm.Parameters["@EndTime"].SqlDbType = System.Data.SqlDbType.Date;

            commandText += " ORDER BY STARTTIME DESC";

            comm.CommandText = commandText;

            return comm;
        }
    }
}