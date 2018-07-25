<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportData.aspx.cs" Inherits="ELTManagement.ImportData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <link rel="stylesheet" type="text/css" href="mystyle.css"/>
    <title>ETL Management</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="manager" runat="server"></asp:ScriptManager>
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
                <div id="PageTitle"><h2>Perform Data Import</h2></div>
                <div id ="PageContent">
                    <h3>Select Dataset to Import</h3>
                    <br />
                    <asp:DropDownList ID="drp_Datasets" runat="server"></asp:DropDownList>
                </div>
                <div id="NavButtons">
                    <asp:Button ID="btn_Execute" runat="server" Text="Execute" OnClick="btn_Execute_Click" />
                </div>

            </div>

            <!-- closing wrapper-->
        </div>
    </form>
</body>
</html>
