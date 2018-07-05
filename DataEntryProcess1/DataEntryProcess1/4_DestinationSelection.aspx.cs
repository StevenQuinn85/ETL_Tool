using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DataEntryProcess1
{
    public partial class _4_DestinationSelection : System.Web.UI.Page
    {
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();
        string destinationType;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            if (ServerSelectionList.SelectedIndex == 0)
            {
                destinationType = "SQL Server";
                DataProperties.Add("Destination Database Type", destinationType);
                Session["DataProperties"] = DataProperties;
                Response.Redirect("5_DirectConnectSQLDestinationDetails.aspx");
            }
            else if (ServerSelectionList.SelectedIndex == 1)
            {
                destinationType = "Oracle Database";
                DataProperties.Add("Destination Database Type", destinationType);
                Session["DataProperties"] = DataProperties;
                Response.Redirect("");
            }
            else if (ServerSelectionList.SelectedIndex == 2)
            {
                destinationType = "Access Database";
                DataProperties.Add("Destination Database Type", destinationType);
                Session["DataProperties"] = DataProperties;
                Response.Redirect("");
            }
        }
    }
}