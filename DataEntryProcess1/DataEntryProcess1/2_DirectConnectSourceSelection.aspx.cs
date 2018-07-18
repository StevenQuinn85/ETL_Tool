using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DataEntryProcess1
{
    public partial class _2_DirectConnectSourceSelection : System.Web.UI.Page
    {
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();
        private string sourceDBType;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            if (ServerSelectionList.SelectedIndex == 0)
            {
                sourceDBType = "SQL Server";
                DataProperties.Add("Source Database Type",sourceDBType);
                Session["DataProperties"] = DataProperties;
                Response.Redirect("3_DirectConnectSQLSourceDetails.aspx");
            }
            else if (ServerSelectionList.SelectedIndex == 1)
            {
                sourceDBType = "Oracle Database";
                DataProperties.Add("Source Database Type", sourceDBType);
                Session["DataProperties"] = DataProperties;
                Response.Redirect("3_DirectConnectOracleSourceDetails.aspx");
            }
            else if (ServerSelectionList.SelectedIndex == 2)
            {
                sourceDBType = "Access Database";
                DataProperties.Add("Source Database Type", sourceDBType);
                Session["DataProperties"] = DataProperties;
                Response.Redirect("");
            }
        }
    }
}