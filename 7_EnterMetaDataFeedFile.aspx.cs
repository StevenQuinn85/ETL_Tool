using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

namespace ELTManagement
{
    public partial class _7_EnterMetaDataFeedFile : System.Web.UI.Page
    {
        int NumberOfColumns;
        List<TextBox> lst_ColumnNames = new List<TextBox>();
        List<TextBox> lst_ColumnOrder = new List<TextBox>();
        List<DropDownList> lst_DataType = new List<DropDownList>();
        List<TextBox> lst_MinLength = new List<TextBox>();
        List<TextBox> lst_MaxLength = new List<TextBox>();
        List<DropDownList> lst_Nullable = new List<DropDownList>();
        List<DropDownList> lst_NullAction = new List<DropDownList>();
        List<TextBox> lst_ReplaceValue = new List<TextBox>();
        List<CheckBox> lst_PrimaryKeysCheckboxes = new List<CheckBox>();
        List<string> lst_PrimaryKeys = new List<string>();

        //List to hold the Data Properties that will be passed to the final page
        Dictionary<string, string> DataProperties = new Dictionary<string, string>();

        List<string> c_Names = new List<string>();
        List<string> c_Order = new List<string>();
        List<string> c_DataTypes = new List<string>();
        List<string> c_MinLenth = new List<string>();
        List<string> c_MaxLength = new List<string>();
        List<string> c_Nullable = new List<string>();
        List<string> c_NullAction = new List<string>();
        List<string> c_ReplaceValue = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //List of DataProperites so far
            DataProperties = (Dictionary<string, string>)Session["DataProperties"];
            //File Location From Previous Page
            string fileLocation = (string)(Session["FileLocation"]);

            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
            }

            StringBuilder TableText = new StringBuilder();
            string delimter = DataProperties["Source Delimiter"];

            string[] Columns = GetColumnNames(fileLocation, delimter);

            NumberOfColumns = Columns.Count();


            Label lbl_Header = new Label();
            lbl_Header.Style.Add("margin-right", "10px");
            lbl_Header.Style.Add("margin-bottom", "5x");
            lbl_Header.Style.Add("font-weight", "bold");
            lbl_Header.Text = "Order";
            MetaDataPanel.Controls.Add(lbl_Header);

            Label lbl_Header1 = new Label();
            lbl_Header1.Style.Add("margin-right", "10px");
            lbl_Header1.Style.Add("font-weight", "bold");
            lbl_Header1.Text = "Column Name";
            MetaDataPanel.Controls.Add(lbl_Header1);

            Label lbl_Header2 = new Label();
            lbl_Header2.Style.Add("margin-right", "10px");
            lbl_Header2.Style.Add("font-weight", "bold");
            lbl_Header2.Text = "Primary Key";
            MetaDataPanel.Controls.Add(lbl_Header2);

            Label lbl_Header3 = new Label();
            lbl_Header3.Style.Add("margin-right", "20px");
            lbl_Header3.Style.Add("font-weight", "bold");
            lbl_Header3.Text = "Data Type";
            MetaDataPanel.Controls.Add(lbl_Header3);

            Label lbl_Header4 = new Label();
            lbl_Header4.Style.Add("margin-right", "10px");
            lbl_Header4.Style.Add("font-weight", "bold");
            lbl_Header4.Text = "Min Length";
            MetaDataPanel.Controls.Add(lbl_Header4);

            Label lbl_Header5 = new Label();
            lbl_Header5.Style.Add("margin-right", "10px");
            lbl_Header5.Style.Add("font-weight", "bold");
            lbl_Header5.Text = "Max Length";
            MetaDataPanel.Controls.Add(lbl_Header5);

            Label lbl_Header6 = new Label();
            lbl_Header6.Style.Add("margin-right", "10px");
            lbl_Header6.Style.Add("font-weight", "bold");
            lbl_Header6.Text = "Nulls Permitted";
            MetaDataPanel.Controls.Add(lbl_Header6);

            Label lbl_Header7 = new Label();
            lbl_Header7.Style.Add("margin-right", "30px");
            lbl_Header7.Style.Add("font-weight", "bold");
            lbl_Header7.Text = "Null Action";
            MetaDataPanel.Controls.Add(lbl_Header7);

            Label lbl_Header8 = new Label();
            lbl_Header8.Style.Add("margin-right", "10px");
            lbl_Header8.Style.Add("font-weight", "bold");
            lbl_Header8.Text = "Default Value";
            MetaDataPanel.Controls.Add(lbl_Header8);

            MetaDataPanel.Controls.Add(new LiteralControl("</br>"));


            int count;

            for (int i = 0; i < NumberOfColumns; i++)
            {
                //why not i, test this
                count = i;

                TextBox txt_columnName = new TextBox();
                txt_columnName.Width = 100;
                txt_columnName.Text = Columns[count];
                txt_columnName.Style.Add("margin-right", "45px");
                lst_ColumnNames.Add(txt_columnName);

                TextBox txt_columnOrder = new TextBox();
                lst_ColumnOrder.Add(txt_columnOrder);
                txt_columnOrder.Width = 25;
                txt_columnOrder.Style.Add("margin-right", "25px");
                txt_columnOrder.Text = count.ToString();

                TextBox txt_minLength = new TextBox();
                lst_MinLength.Add(txt_minLength);
                txt_minLength.Width = 25;
                txt_minLength.Style.Add("margin-right", "65px");
                txt_minLength.Text = "0";
                TextBox txt_maxLength = new TextBox();
                lst_MaxLength.Add(txt_maxLength);
                txt_maxLength.Width = 25;
                txt_maxLength.Style.Add("margin-right", "45px");
                txt_maxLength.Text = "50";
                Label Column1 = new Label();
                TextBox txt_replaceValue = new TextBox();
                lst_ReplaceValue.Add(txt_replaceValue);
                txt_replaceValue.Width = 120;

                CheckBox PK = new CheckBox();
                lst_PrimaryKeysCheckboxes.Add(PK);
                PK.Checked = false;
                PK.Style.Add("margin-right", "40px");

                DropDownList NullsAccepted = new DropDownList();
                lst_Nullable.Add(NullsAccepted);
                List<string> options = new List<string>();
                options.Add("Yes");
                options.Add("No");
                NullsAccepted.DataSource = options;
                NullsAccepted.DataBind();
                NullsAccepted.Style.Add("margin-right", "65px");

                DropDownList DataTypeOptions = new DropDownList();
                lst_DataType.Add(DataTypeOptions);
                List<string> dataTypes = new List<string>();
                dataTypes.Add("Text");
                dataTypes.Add("Int");
                dataTypes.Add("BigInt");
                dataTypes.Add("Decimal");
                dataTypes.Add("Date");
                DataTypeOptions.DataSource = dataTypes;
                DataTypeOptions.DataBind();
                DataTypeOptions.Style.Add("margin-right", "40px");

                DropDownList NullsAction = new DropDownList();
                lst_NullAction.Add(NullsAction);
                List<string> actions = new List<string>();
                actions.Add("Reject");
                actions.Add("Replace");
                actions.Add("Accept");
                NullsAction.DataSource = actions;
                NullsAction.DataBind();
                NullsAction.Style.Add("margin-right", "35px");



                MetaDataPanel.Controls.Add(txt_columnOrder);

                MetaDataPanel.Controls.Add(txt_columnName);

                MetaDataPanel.Controls.Add(PK);

                MetaDataPanel.Controls.Add(DataTypeOptions);

                MetaDataPanel.Controls.Add(txt_minLength);

                MetaDataPanel.Controls.Add(txt_maxLength);

                MetaDataPanel.Controls.Add(NullsAccepted);

                MetaDataPanel.Controls.Add(NullsAction);

                MetaDataPanel.Controls.Add(txt_replaceValue);
                MetaDataPanel.Controls.Add(new LiteralControl("<br />"));
            }

            MetaDataPanel.Controls.Add(new LiteralControl("<br />"));
            MetaDataPanel.Controls.Add(new LiteralControl("<br />"));


        }

        private string[] GetColumnNames(string filelocation, string delimiter)
        {
            StreamReader Reader = new StreamReader(filelocation);
            string[] Columns = Reader.ReadLine().Split(delimiter.ToCharArray());

            return Columns;

        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            //Populate all of the list of meta data to be passed to confirm page.

            for (int i = 0; i < lst_PrimaryKeysCheckboxes.Count; i++)
            {
                if (lst_PrimaryKeysCheckboxes[i].Checked)
                {
                    lst_PrimaryKeys.Add(lst_ColumnNames[i].Text);
                }
            }

            foreach (TextBox item in lst_ColumnNames)
            {
                c_Names.Add(item.Text);
            }
            foreach (TextBox item in lst_ColumnOrder)
            {
                c_Order.Add(item.Text);
            }
            foreach (DropDownList item in lst_DataType)
            {
                c_DataTypes.Add(item.Text);
            }
            foreach (TextBox item in lst_MinLength)
            {
                c_MinLenth.Add(item.Text);
            }
            foreach (TextBox item in lst_MaxLength)
            {
                c_MaxLength.Add(item.Text);
            }
            foreach (DropDownList item in lst_Nullable)
            {
                c_Nullable.Add(item.Text);
            }
            foreach (DropDownList item in lst_NullAction)
            {
                c_NullAction.Add(item.Text);
            }
            foreach (TextBox item in lst_ReplaceValue)
            {
                c_ReplaceValue.Add(item.Text);
            }

            //Use Page.Session functionality to pass all of the collected data to final screen.
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

            //Passing the name of the page to return to if the user press back on the next page
            Session["ReturnPageName"] = "6_MetaDataSelectionDirectConnect.aspx";

            if (DataProperties["Import Type"].Equals("Direct Connect"))
            {
                Response.Redirect("7_LookBackDetails.aspx");
            }
            else
            {
                Response.Redirect("8_ConfirmPage.aspx");
            }
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Session["DataProperties"] = DataProperties;
            Response.Redirect("6_MetaDataSelectionFeedFile.aspx");
        }
    }
}