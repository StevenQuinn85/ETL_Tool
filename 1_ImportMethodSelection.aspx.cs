using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ELTManagement.DataEntryForms
{
    public partial class _1_ImportMethodSelection : System.Web.UI.Page
    {
        //This form will capture the data entry method
        //Either Feed File (import data from a file)
        //Or Direct Connect (connect to DB)

        //The import type will be stored in this string variable
        string importType;
        

        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)(Session["DataProperties"]);
        }



        protected void btn_Next_Click(object sender, EventArgs e)
        {
            //Set the import source to be a Feed File
            if (radio_ImportType.SelectedIndex == 0)
            {
                importType = "Feed File";
                AddImportType(importType);
                Session["DataProperties"] = DataProperties;
                Response.Redirect("2_FeedFileSourceDetails.aspx");
            }
            //Or set the import type to be a direct connect to a database
            else if (radio_ImportType.SelectedIndex == 1)
            {
                importType = "Direct Connect";
                AddImportType(importType);
                Session["DataProperties"] = DataProperties;
                Response.Redirect("2_DirectConnectSourceSelection.aspx");
            }
        }

        // This method will add the import type to the Data Properties Dictionary
        //It will check if the value already exists and either update or add
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

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Session["DataProperties"] = DataProperties;
            Response.Redirect("1_DataSetName.aspx");
        }

    }
}