﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="7_ManualEnterNumOfColumns.aspx.cs" Inherits="ELTManagement._7_ManualEnterNumOfColumns" %>

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
                <div id="PageTitle"><h2>How many columns are in the source ?</h2></div>
                <div id ="PageContentMeta">
                                                <asp:Label ID="Label2" runat="server" Text="Number of Columns"></asp:Label>
                            <asp:TextBox ID="txt_numberofcolumns" runat="server"></asp:TextBox>
                </div>
                <div id="NavButtons">
                    <asp:Button ID="btn_back" runat="server" Text="Back" OnClick="btn_Back_Click" Style="width: 47px; height: 26px;" />
                           &nbsp;
                    <asp:Button ID="btn_Next" runat="server" Text="Next" OnClick="btn_Next_Click" Style="width: 47px" />
                </div>
            
            </div>
            <!-- closing wrapper-->
        </div>
    </form>
</body>
</html>
