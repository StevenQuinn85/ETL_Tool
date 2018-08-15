using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ELTManagement
{
    public partial class _8_ConfirmPageFailure : System.Web.UI.Page
    {
        //A string to collect any errors with the data load.
        string details;

        protected void Page_Load(object sender, EventArgs e)
        {
            details = (string)(Session["Errors"]);

            lbl_ErrorDetails.Text = details;
        }
    }
}