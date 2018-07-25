using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ELTManagement
{
    public partial class _4_DestinationSelection : System.Web.UI.Page
    {
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();
        string destinationType;

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

            if (DataProperties["Import Type"].Equals("Feed File"))
            {
            Response.Redirect("2_FeedFileSourceDetails.aspx");
            }
            else if (DataProperties["Import Type"].Equals("Direct Connect"))
            {
                if (DataProperties["Source Database Type"].Equals("SQL Server"))
                {
                    Response.Redirect("3_DirectConnectSQLSourceDetails.aspx");
                }
                else if (DataProperties["Source Database Type"].Equals("Oracle Database"))
                {
                    Response.Redirect("3_DirectConnectOracleSourceDetails.aspx");
                }
                else if (DataProperties["Source Database Type"].Equals("Access Database"))
                {
                    Response.Redirect("3_DirectConnectAccessSourceDetails.aspx");
                }
                else
                {
                    Response.Redirect("2_DirectConnectSourceSelection.aspx");
                }
            }
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            if (ServerSelectionList.SelectedIndex == 0)
            {
                destinationType = "SQL Server";
                AddDestinationType(destinationType);
                Session["DataProperties"] = DataProperties;
                Response.Redirect("5_DirectConnectSQLDestinationDetails.aspx");
            }
            else if (ServerSelectionList.SelectedIndex == 1)
            {
                destinationType = "Oracle Database";
                AddDestinationType(destinationType);
                Session["DataProperties"] = DataProperties;
                Response.Redirect("5_DirecConnectOracleDestinationDetails.aspx");
            }
            else if (ServerSelectionList.SelectedIndex == 2)
            {
                destinationType = "Access Database";
                AddDestinationType(destinationType);
                Session["DataProperties"] = DataProperties;
                Response.Redirect("5_DirectConnectAccessDestinationDetails.aspx");
            }
        }

        private void AddDestinationType(string destinationType)
        {
            if (!DataProperties.ContainsKey("Destination Type"))
            {
                DataProperties.Add("Destination Type", destinationType);
            }
            else
            {

                DataProperties["Destination Type"] = destinationType;
            }
        }
    }
}