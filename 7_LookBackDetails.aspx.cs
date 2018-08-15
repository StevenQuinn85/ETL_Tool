using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ELTManagement
{
    public partial class _7_LookBackDetails : System.Web.UI.Page
    {
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        List<string> c_Names = new List<string>();
        List<string> c_Order = new List<string>();
        List<string> c_DataTypes = new List<string>();
        List<string> c_MinLenth = new List<string>();
        List<string> c_MaxLength = new List<string>();
        List<string> c_Nullable = new List<string>();
        List<string> c_NullAction = new List<string>();
        List<string> c_ReplaceValue = new List<string>();
        List<string> lst_PrimaryKeys = new List<string>();

        string lookBackColumn, useLookBack, lookBackPeriod;

        //If the user needs to move to the previous page this variable will collect
        //the previous page name
        string previousPage;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];
            c_Names = (List<string>)(Session["ColumnNames"]);
            c_Order = (List<string>)(Session["ColumnOrder"]);
            c_DataTypes = (List<string>)(Session["DataTypes"]);
            lst_PrimaryKeys = (List<string>)(Session["PrimaryKeys"]);
            c_MinLenth = (List<string>)(Session["MinLength"]);
            c_MaxLength = (List<string>)(Session["MaxLength"]);
            c_Nullable = (List<string>)(Session["NullPermitted"]);
            c_NullAction = (List<string>)(Session["NullAction"]);
            c_ReplaceValue = (List<string>)(Session["ReplacementValue"]);

            previousPage = (string)(Session["ReturnPageName"]);

            if (!IsPostBack)
            {
                PopulateDateDropDown();
            }
        }

        private void PopulateDateDropDown()
        {
            List<string> LookBackColumns = new List<string>();
            int count = 0;

            foreach (string item in c_DataTypes)
            {
                if (item.Equals("Date"))
                {
                    LookBackColumns.Add(c_Names[count]);
                    count++;
                }
                else
                {
                    count++;
                }
            }

            if (LookBackColumns.Count().Equals(0))
            {
                LookBackColumns.Add("No Date Type Columns Available");
            }

            drp_LookBackDates.DataSource = LookBackColumns;
            drp_LookBackDates.DataBind();
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Session["DataProperties"] = DataProperties;

            //Return the user back to the screen before entering metadata
            Response.Redirect(previousPage);   
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            if (chk_UseLookBack.Checked)
            {
                useLookBack = "Yes";

            }
            else
            {
                useLookBack = "No";
                lookBackPeriod = "N/A";
                lookBackColumn = "0";
            }

            AddLookbackOption(useLookBack);

            lookBackColumn = drp_LookBackDates.SelectedValue.ToString();
            AddLookbackColumn(lookBackColumn);


            lookBackPeriod = (txt_LookBackPeriod.Text);
            AddLookbackPeriod(lookBackPeriod);

            Session["ColumnNames"] = c_Names;
            Session["ColumnOrder"] = c_Order;
            Session["DataTypes"] = c_DataTypes;
            Session["PrimaryKeys"] = lst_PrimaryKeys;
            Session["MinLength"] = c_MinLenth;
            Session["MaxLength"] = c_MaxLength;
            Session["NullPermitted"] = c_Nullable;
            Session["NullAction"] = c_NullAction;
            Session["ReplacementValue"] = c_ReplaceValue;

            Session["DataProperties"] = DataProperties;

            Response.Redirect("8_ConfirmPage.aspx");
        }

        protected void chk_UseLookBack_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void AddLookbackOption(string option)
        {
            if (!DataProperties.ContainsKey("Lookback Option"))
            {
                DataProperties.Add("Lookback Option", option);
            }
            else
            {
                DataProperties["Lookback Option"] = option;
            }
        }

        private void AddLookbackColumn(string column)
        {
            if (!DataProperties.ContainsKey("Lookback Column"))
            {
                DataProperties.Add("Lookback Column", column);
            }
            else
            {
                DataProperties["Lookback Column"] = column;
            }
        }

        private void AddLookbackPeriod(string period)
        {
            if (!DataProperties.ContainsKey("Lookback Period"))
            {
                DataProperties.Add("Lookback Period", period);
            }
            else
            {
                DataProperties["Lookback Period"] = period.ToString();
            }
        }
    }
}