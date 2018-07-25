using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ELTManagement
{
    public partial class ImportResults : System.Web.UI.Page
    {

        //Results object to receive the details about the import process
        Results Stats = new Results();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Stats = (Results)Session["Results"];
                UpdateStatsTable();
            }

        }

        private void UpdateStatsTable()
        {
            lbl_rawImported.InnerText = Stats.RawDataRecordsImported.ToString();
            lbl_rawOmmited.InnerText = Stats.RawDataRecordsOmitted.ToString();

            lbl_transformAccpeted.InnerText = Stats.AcceptedRecords_transform.ToString();
            lbl_transformRejected.InnerText = Stats.RejectedRecords_transform.ToString();

            lbl_DLInsert.InnerText = Stats.RecordsInsertedToDestination.ToString();
            lbl_DLReject.InnerText = Stats.RecordsRejectedFromDestination.ToString();
            lbl_DLDelete.InnerText = Stats.RecordDeletedFromDestination.ToString();



        }

        protected void btn_finish_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
    }
}