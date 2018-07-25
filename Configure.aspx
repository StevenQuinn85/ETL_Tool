<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configure.aspx.cs" Inherits="ELTManagement.Configure" %>

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
                    <a href="ImportData.aspx">ImportData</a>
                    <a href="Configure.aspx">Configure</a>
                    <a href="Reporting.aspx">Reporting</a>
                    <a href="ErrorLogs.aspx">ErrorLogs</a>
                </div>
            <!--Center Content is the only div to change-->
            <div id="CenterContent">
                <div id ="PageTitle">
                <h2>Select Configuration Type</h2>
                </div>

                <div id="PageContent">
                  <a href="1_DataSetName.aspx">Configure New Process</a>
                    <br /><br />
                    <a href="9_UpdateDataPropSelection.aspx">Update Exisiting Process</a>
                </div>

            </div>
            <!-- closing wrapper-->
        </div>
    </form>
</body>
</html>
