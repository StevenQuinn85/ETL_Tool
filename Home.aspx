<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ELTManagement.Home" %>

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
                <div id="HomePageRight">
                    <h3>Recent Imports</h3>
                    <asp:Literal ID="lit_RecentImportsTable" runat="server"></asp:Literal>
                </div>
                <div id="HomePageLeft">
                    <h3>ETL Management System</h3>
                    <p>ETL Management provides a dynamic system for Importing, Cleaning and Loading source data a destination Database.</p>
                    <p>A record of all executed imports is retained by the system.  Details of past imports can be generated from the Reporting page. </p>
                    <p>Any errors occured during an import are records in the log files that accompany each import. These are available from the Error Logs page. </p>
                    <p>Click Here to Open <a href="UserGuide/ETL%20Management%20User%20Guide.pdf" target="_blank">User Guide</a> </p>
                </div>
            </div>
            <!-- closing wrapper-->
            <div id="footer">
                ETL Management 2018 | LYIT | <a href="AdminLogin.aspx">Admin Login</a>
            </div>

        </div>


    </form>
</body>
</html>
