using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
namespace ELTManagement
{
    public partial class Home : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAppConfiguration();
            }

        }

        //This method loads the connection string to the BackEnd DB
        //and the location of the log files
        private void LoadAppConfiguration()
        {
            //Retrieve the Connection String and Log File Location
            string FileLocation = HttpContext.Current.Server.MapPath("/").ToString();

            //remove the final '\ETLManagement'
            int stepOne = FileLocation.LastIndexOf("E");
            FileLocation = FileLocation.Remove(stepOne, 14);

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