using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DataEntryProcess1
{
    public partial class _1_ImportTypeSelection : System.Web.UI.Page
    {
        string importType;
        //Creating a Dictionary Structure to hold all the data properties
        //This Dictionary will be passed to all the pages in the data entry
        //process to collect data for insert to the DB.
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedIndex == 0)
            {
                importType = "Feed File";
                AddImportType(importType);
                Session["DataProperties"] = DataProperties;
                Response.Redirect("2_FeedFileSourceDetails.aspx");
            }
            else if (RadioButtonList1.SelectedIndex == 1)
            {
                importType = "Direct Connect";
                AddImportType(importType);
                Session["DataProperties"] = DataProperties;
                Response.Redirect("2_DirectConnectSourceSelection.aspx");
            }
        }

        private void AddImportType(string importType)
        {
            if (!DataProperties.ContainsKey("Import Type"))
            {
                DataProperties.Add("Import Type", importType);
            }
            else
            {
                DataProperties["Import Type"] = importType;
            }
        }
    }
}