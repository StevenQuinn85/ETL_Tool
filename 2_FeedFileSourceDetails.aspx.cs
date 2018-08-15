using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ELTManagement.DataEntryForms
{
    public partial class _2_FeedFileSourceDetails : System.Web.UI.Page
    {
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        //String variables to hold the Source Details.
        string fileLocation, fileName, delimiterChar;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];

            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
            }
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Session["DataProperties"] = DataProperties;
            Response.Redirect("1_ImportMethodSelection.aspx");
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            fileLocation = txt_FileLocation.Text;
            fileName = txt_FileName.Text;
            delimiterChar = txt_Delimiter.Text;

            AddFileLocation(fileLocation);
            AddFileName(fileName);
            AddDelimiter(delimiterChar);


            Session["DataProperties"] = DataProperties;
            Response.Redirect("4_DestinationSelection.aspx");
        }

        private void AddDelimiter(string delimiterChar)
        {
            if (!DataProperties.ContainsKey("Source Delimiter"))
            {
                DataProperties.Add("Source Delimiter", delimiterChar);
            }
            else
            {
                DataProperties["Source Delimiter"] = delimiterChar;
            }
        }

        private void AddFileName(string fileName)
        {
            if (!DataProperties.ContainsKey("Source File Name"))
            {
                DataProperties.Add("Source File Name", fileName);
            }
            else
            {
                DataProperties["Source File Name"] = fileName;
            }
        }

        private void AddFileLocation(string fileLocation)
        {
            if (!DataProperties.ContainsKey("Source File Location"))
            {
                DataProperties.Add("Source File Location", fileLocation);
            }
            else
            {
                DataProperties["Source File Location"] = fileName;
            }
        }
    }
}