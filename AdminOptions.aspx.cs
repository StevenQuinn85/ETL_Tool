using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ELTManagement
{
    public partial class AdminOptions : System.Web.UI.Page
    {
        //Create a string value to hold the status of any updates
        string updateInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
                //reset the update information
                updateInfo = "";
            lbl_UpdateInfo.InnerText = updateInfo;

            if (!IsPostBack)
            {
            //If landing on the page for the first time check if there is an update message to display
            updateInfo = (string)Session["UpdateInfo"];
                if (!string.IsNullOrEmpty(updateInfo))
                {
                    lbl_UpdateInfo.InnerText = updateInfo;
                }
            lbl_UpdateInfo.InnerText = updateInfo;

            }

        }
    }
}