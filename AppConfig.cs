using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ETLComponents
{
    //This class will retrieve the connection string to the backend BD
    //and the location of where to store the log files. This information 
    //will be store in the appdata.txt file in 
    public class AppConfig
    {
        private string connectionString, logFileLocation; 

        public AppConfig()
        {
            LoadAppConfiguration();
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
                if (count <= 1)
                {
                    appData[count] = reader.ReadLine();
                    count++;
                }

            }

            //The string in position zero will be the connection string
            string[] lineOne = appData[0].Split('#');
             connectionString = lineOne[1];
            //The string in position one will be the log location
            string[] lineTwo = appData[1].Split('#');
             logFileLocation = lineTwo[1];

        }

        public string ConnectionString
        {
            get
            {
                return connectionString;
            }

        }

        public string LogFileLocation
        {
            get
            {
                return logFileLocation;
            }


        }
    }
}