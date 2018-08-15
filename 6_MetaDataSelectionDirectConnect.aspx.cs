using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ELTManagement
{
    public partial class _6_MetaDataSelectionDirectConnect : System.Web.UI.Page
    {
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Session["DataProperties"] = DataProperties;

            if (DataProperties["Destination Type"].Equals("SQL Server"))
            {
                Response.Redirect("5_DirectConnectSQLDestinationDetails.aspx");
            }
            else if (DataProperties["Destination Type"].Equals("Oracle Database"))
            {
                Response.Redirect("5_DirecConnectOracleDestinationDetails.aspx");
            }
            else if (DataProperties["Destination Type"].Equals("Access Database"))
            {
                Response.Redirect("5_DirectConnectAccessDestinationDetails.aspx");
            }
            else
            {
                Response.Redirect("4_DestinationSelection.aspx");
            }
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedIndex == 0)
            {
                //User select manual entry
                Session["DataProperties"] = DataProperties;
                Response.Redirect("7_ManualEnterNumOfColumns.aspx");
            }
            else if (RadioButtonList1.SelectedIndex == 1)
            {
                //User selects to extract from Source
                Session["DataProperties"] = DataProperties;
                Response.Redirect("7_EnterMetaDataDirectConnect.aspx");
            }
        }
    }
}