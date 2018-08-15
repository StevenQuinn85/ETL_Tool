using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
    using System.Text;

namespace ELTManagement
{
    public partial class ErrorLogsContent : System.Web.UI.Page
    {
        private string logLocation;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                logLocation = Request.QueryString["logName"];
            }

            if (!string.IsNullOrEmpty(logLocation))
            {
                if (File.Exists(logLocation))
                {
                    PopulateLogContent();
                }
                else
                {
                    Display404Error();
                }
                
            }
        }

        private void Display404Error()
        {
            string logNotFoundError = "<h2> 404 Error - Page Not Found </h2>";

            LogContents.Text = logNotFoundError;
        }

        private void PopulateLogContent()
        {
            StringBuilder contentText = new StringBuilder();
            StreamReader reader = new StreamReader(logLocation);

            using (reader)
            {
                while (!reader.EndOfStream)
                {
contentText.Append("<p>" + reader.ReadLine() + "</p>");
                }
                
            }

            LogContents.Text = contentText.ToString();
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("ErrorLogs.aspx");
        }
    }
}