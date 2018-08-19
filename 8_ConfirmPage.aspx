<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="8_ConfirmPage.aspx.cs" Inherits="ELTManagement._8_ConfirmPage" %>

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
                    <h2>Confirm Data Properties</h2>
                </div>
                <div id="PageContent">
                    <div id="DataPropertiesTable">
                        <asp:Literal ID="SourceDetailsDisplay" runat="server"></asp:Literal>
                        <br />
                        <asp:Literal ID="DataDestinationDetails" runat="server"></asp:Literal>
                        <br />
                        <asp:Literal ID="PrimaryKeyDetails" runat="server"></asp:Literal>
                        <br />
                        <asp:Literal ID="MetaDataDisplay" runat="server"></asp:Literal>
                        <br />
                        <asp:Literal ID="LookBackDisplay" runat="server"></asp:Literal>
                    </div>

                </div>
                <div id="NavButtons">
                    <asp:Button ID="btn_Back" runat="server" Text="Back" OnClick="btn_Back_Click" />
                    &nbsp;
                    <asp:Button ID="btn_Confirm" runat="server" Text="Confirm" OnClick="btn_Confirm_Click" />
                    <br />
                    <asp:Label ID="lbl_complete" runat="server"></asp:Label>
                    <br />
                </div>

            </div>
            <!-- closing wrapper-->
            <div id="footer">ETL Management 2018 | LYIT | <a href="AdminLogin.aspx">Admin Login</a></div>
        </div>
    </form>
</body>
</html>
