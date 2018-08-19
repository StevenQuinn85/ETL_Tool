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
                    <asp:Literal ID ="lit_RecentImportsTable" runat="server"></asp:Literal>
                </div>
                <div id="HomePageLeft">
                    <h3>ETL Management System</h3>
                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut ut mauris sit amet purus iaculis interdum. Integer finibus dignissim euismod. Duis id lectus mi. Suspendisse sed libero sit amet libero ullamcorper vestibulum nec sed nulla. Sed augue elit, varius at nisl id, faucibus dapibus metus. Donec sed leo aliquet, porttitor elit in, congue ante. Vestibulum vel lobortis eros. Suspendisse potenti.</p>
                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut ut mauris sit amet purus iaculis interdum. Integer finibus dignissim euismod. Duis id lectus mi. Suspendisse sed libero sit amet libero ullamcorper vestibulum nec sed nulla. Sed augue elit, varius at nisl id, faucibus dapibus metus. Donec sed leo aliquet, porttitor elit in, congue ante. Vestibulum vel lobortis eros. Suspendisse potenti.</p>
                   
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
