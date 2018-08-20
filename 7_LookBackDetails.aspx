<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="7_LookBackDetails.aspx.cs" Inherits="ELTManagement._7_LookBackDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="mystyle.css" />
    <title>ETL Management</title>
</head>
<body>
    <form id="form1" runat="server">
        <!--wrapper div-->
        <div id="wrapper">

            <div id="banner">
                <div id="logo"></div>
                <div id="banner_title">
                    <h1>ETL Management</h1>
                </div>
            </div>

            <div id="banner_lower">
                <div id="LogOutOption"><a href="Login.aspx">Log Out</a></div>
            </div>


            <div id="menu">
                <h2>Menu</h2>
                <a href="Home.aspx">Home</a>
                <a href="ImportData.aspx">Import Data</a>
                <a href="Configure.aspx">Configure</a>
                <a href="Reporting.aspx">Reporting</a>
                <a href="ErrorLogs.aspx">Error Logs</a>
            </div>

            <!--Center Content is the only div to change-->
            <div id="CenterContent">
                <div id="PageTitle">
                    <h2>Configure Look Back</h2>
                </div>
                <div id="PageContent">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_UseLookBack" runat="server" Text="Use Lookback period ?"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_UseLookBack" runat="server" OnCheckedChanged="chk_UseLookBack_CheckedChanged" /></td>
                            <td><div id="tip_lookback" class="toolTip"><asp:Label ID="lbl_LookBackTip" runat="server">?</asp:Label></div></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_SelectLookBackColumn" runat="server" Text="Select look back column"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drp_LookBackDates" runat="server"></asp:DropDownList>
                            </td>
                            <td><div id="tip_Columns" class="toolTip"><asp:Label ID="lbl_ColumnsTip" runat="server">?</asp:Label></div></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_LookBackPeriod" runat="server" Text="Enter Look Back Period"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_LookBackPeriod" runat="server"></asp:TextBox>
                            </td>
                            <td><div id="tip_Period" class="toolTip"><asp:Label ID="lbl_PeriodTip" runat="server">?</asp:Label></div></td>
                        </tr>
                    </table>
                </div>
                <div id="NavButtons">
                    <asp:Button ID="btn_back" runat="server" Text="Back" OnClick="btn_Back_Click" Style="width: 47px; height: 26px;" />
                    &nbsp;
                    <asp:Button ID="btn_Next" runat="server" Text="Next" OnClick="btn_Next_Click" />
                </div>

            </div>
            <!-- closing wrapper-->
            <div id="footer">ETL Management 2018 | LYIT | <a href="AdminLogin.aspx">Admin Login</a></div>
        </div>
    </form>
</body>
</html>
