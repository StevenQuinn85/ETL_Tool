<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="9_UpdateDataPropSelection.aspx.cs" Inherits="ELTManagement._9_UpdateDataPropSelection" %>

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
                <label id="lbl_UpdateInfo" runat="server" text=""></label>
                <div id="PageTitleCenter">
                    <h2>Select Process To Edit</h2>
                </div>
                <div id="PageContentCenter">
                    <asp:DropDownList ID="DropList_Process" runat="server" OnSelectedIndexChanged="DropList_Process_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div id="NavButtons">
                    <asp:Button ID="btn_next" runat="server" Text="Next" OnClick="btn_next_Click" />
                </div>

            </div>
            <!-- closing wrapper-->
            <div id="footer">ETL Management 2018 | LYIT | <a href="AdminLogin.aspx">Admin Login</a></div>
        </div>
    </form>
</body>
</html>
