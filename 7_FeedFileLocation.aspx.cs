using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace ELTManagement
{
    public partial class _7_FeedFileLocation : System.Web.UI.Page
    {
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];
            txt_sampleFileLocation.Width = 500;

            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
            }
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            if (File.Exists(txt_sampleFileLocation.Text))
            {
                            Session["FileLocation"] = txt_sampleFileLocation.Text;
            Session["DataProperties"] = DataProperties;
            Response.Redirect("7_EnterMetaDataFeedFile.aspx");
            }
            else
            {
                lbl_FeedFileError.Text = "Check Feed File Exists";
            }

        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Session["DataProperties"] = DataProperties;
            Response.Redirect("6_MetaDataSelectionFeedFile.aspx");
        }
    }
}