<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="8_ConfirmPageSuccess.aspx.cs" Inherits="ELTManagement._8_ConfirmPageSuccess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <link rel="stylesheet" type="text/css" href="mystyle.css"/>
    <title>ETL Management</title>
</head>
<body>
    <form id="form1" runat="server">
                     <!--wrapper div-->
        <div id="wrapper">

            <div id="banner">
                <div id="logo"></div>
                <div id="banner_title"><h1>ETL Management</h1></div>
            </div>

            <div id="banner_lower">
            </div>


                <div id="menu">
                    <h2>Home</h2>
                    <a href="ImportData.aspx">Import Data</a>
                    <a href="Configure.aspx">Configure</a>
                    <a href="Reporting.aspx">Reporting</a>
                    <a href="ErrorLogs.aspx">Error Logs</a>
                </div>

            <!--Center Content is the only div to change-->
            <div id="CenterContent">
                <div id="PageTitle"><h2>Success</h2></div>
                <div id ="PageContent">
                    <h4>The Data Import Process has been created</h4>
                    <a href ="Home.aspx">Return to main screen</a>
                </div>

            
            </div>
            <!-- closing wrapper-->
        </div>
    </form>
</body>
</html>
