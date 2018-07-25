using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ELTManagement
{
    public partial class _7_ManualEnterNumOfColumns : System.Web.UI.Page
    {
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Session["DataProperties"] = DataProperties;

            if (DataProperties["Import Type"].Equals("Feed File"))
            {
                Response.Redirect("6_MetaDataSelectionFeedFile.aspx");
            }
            else if (DataProperties["Import Type"].Equals("Direct Connect"))
            {
                Response.Redirect("6_MetaDataSelectionDirectConnect.aspx");
            }
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            Session["NumOfColumns"] = txt_numberofcolumns.Text;
            Session["DataProperties"] = DataProperties;
            Response.Redirect("7_ManuallyEnterMetaData.aspx");
        }
    }
}