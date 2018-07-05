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

            DataProperties.Add("Dataset Name", dataSetName);
            DataProperties.Add("File Location", fileLocation);
            DataProperties.Add("File Name", fileName);
            DataProperties.Add("Delimiter Character", delimiterChar);

            Session["DataProperties"] = DataProperties;

            Response.Redirect("EnterDataPropDestination.aspx");
        }


    }
}