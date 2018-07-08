using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DataEntryProcess1
{
    public partial class _2_FeedFileSourceDetails : System.Web.UI.Page
    {
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();
        //String variables to hold the Source Details.
        string dataSetName, fileLocation, fileName, delimiterChar;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];

        }

        protected void btn_next_Click(object sender, EventArgs e)
        {
            dataSetName = txt_DataSetName.Text;
            fileLocation = txt_FileLocation.Text;
            fileName = txt_FileName.Text;
            delimiterChar = txt_Delimiter.Text;

            AddDataSetName(dataSetName);
            AddFileLocation(fileLocation);
            AddFileName(fileName);
            AddDelimiter(delimiterChar);
            

            Session["DataProperties"] = DataProperties;
            Response.Redirect("4_DestinationSelection.aspx");
        }

        private void AddDelimiter(string delimiterChar)
        {
            if (!DataProperties.ContainsKey("Delimter Character"))
            {
                DataProperties.Add("Delimiter Character", delimiterChar);
            }
            else
            {
                DataProperties["Delimiter Character"] = delimiterChar;
            }
        }

        private void AddFileName(string fileName)
        {
            if (!DataProperties.ContainsKey("File Name"))
            {
                DataProperties.Add("File Name", fileName);
            }
            else
            {
                DataProperties["File Name"] = fileName;
            }
        }

        private void AddFileLocation(string fileLocation)
        {
            if (!DataProperties.ContainsKey("File Location"))
            {
                DataProperties.Add("File Location", fileLocation);
            }
            else
            {
                DataProperties["File Location"] = fileName;
            }
        }

        private void AddDataSetName(string dataSetName)
        {
            if (!DataProperties.ContainsKey("Dataset Name"))
            {
                DataProperties.Add("Dataset Name", dataSetName);
            }
            else
            {
                DataProperties["Dataset Name"] = dataSetName;
            }

        }
    }
}