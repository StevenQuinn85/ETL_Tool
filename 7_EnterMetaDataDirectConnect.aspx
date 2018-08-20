<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="7_EnterMetaDataDirectConnect.aspx.cs" Inherits="ELTManagement._7_EnterMetaDataDirectConnect" %>

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
                    <h2>Enter MetaData</h2>
                </div>
                <div id="PageContentMeta">
                    <asp:Panel ID="MetaDataPanel" runat="server">
                    </asp:Panel>
                </div>
                <div id="NavButtonsMeta">
                    <asp:Button ID="btn_back" runat="server" Text="Back" OnClick="btn_Back_Click" Height="26px" />
                    &nbsp;
                    <asp:Button ID="btn_Next" runat="server" Text="Next" OnClick="btn_Next_Click" Width="46px" />
                </div>

            </div>
            <!-- closing wrapper-->
            <div id="footer">ETL Management 2018 | LYIT | <a href="AdminLogin.aspx">Admin Login</a></div>
        </div>
    </form>
</body>
</html>
