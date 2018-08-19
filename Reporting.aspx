<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reporting.aspx.cs" Inherits="ELTManagement.Reporting" %>

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
            <div id="CenterContentReporting">
                <div id="PageTitle">
                    <h2>Reporting</h2>
                </div>
                <div id="PageContent">

                    <div id="reportingTable">
                        <asp:Literal ID="ReportTable" runat="server"></asp:Literal>

                    </div>

                    <div id="reportingFilters">
                        <%-- This div will contain filters that the user can use to search for record of historical imports --%>

                        <%-- A drop will display the configured import processes --%>
                        <h4>Select Import Process</h4>
                        <asp:DropDownList ID="drp_ImportProcesses" runat="server"></asp:DropDownList>
                        <br />
                        <h4>Select Start Date</h4>
                        <asp:Calendar ID="cal_StartDate" runat="server"></asp:Calendar>
                        <br />
                        <h4>Select End Date</h4>
                        <asp:Calendar ID="cal_EndDate" runat="server"></asp:Calendar>
                        <br />
                        <asp:Button ID="btn_search" runat="server" Text="Search" OnClick="btn_search_Click" />
                        <br />

                    </div>
                </div>

                <div id="NavButtons">
                </div>

            </div>
            <!-- closing wrapper-->
            <div id="footer">ETL Management 2018 | LYIT | <a href="AdminLogin.aspx">Admin Login</a></div>
        </div>
    </form>
</body>
</html>
