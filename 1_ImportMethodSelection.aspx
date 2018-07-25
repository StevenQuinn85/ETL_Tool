﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="1_ImportMethodSelection.aspx.cs" Inherits="ELTManagement.DataEntryForms._1_ImportMethodSelection" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../mystyle.css" rel="stylesheet" />
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
                <div id="PageTitle">
                    <h2>Select Import Method</h2>
                </div>
                <div id="PageContent">
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                        <asp:ListItem>Feed File Import</asp:ListItem>
                        <asp:ListItem>Direct Connect</asp:ListItem>
                    </asp:RadioButtonList>

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
