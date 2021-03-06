﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="3_DirectConnectAccessSourceDetails.aspx.cs" Inherits="ELTManagement.DataEntryForms._3_DirectConnectAccessSourceDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ETL Management</title>
    <link href="mystyle.css" rel="stylesheet" />
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
                    <h2>Enter Access Source Details</h2>
                </div>
                <div id="PageContent">
                    <table>
                        <tr>
                            <td>
                                <label>Enter File Location </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_FileLocation" runat="server" Width="250"></asp:TextBox></td>
                            <td><div id="tip_FileLocation" class="toolTip"><asp:Label ID="lbl_FileLocationTip" runat="server">?</asp:Label></div></td>
                        </tr>
                        <tr>
                            <td><label>Password required</label></td>
                            <td>
                                <asp:CheckBox ID="PasswordRequired" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Enter Password</label></td>
                            <td>
                                <asp:TextBox ID="txt_password" runat="server" Width="250"></asp:TextBox></td>
                            <td><div id="tip_Password" class="toolTip"><asp:Label ID="lbl_PasswordTip" runat="server">?</asp:Label></div></td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <asp:Button ID="btn_Test" runat="server" Text="Test" OnClick="btn_Test_Click" /></td>
                            <td>
                                <asp:Panel ID="ConnectionResult" runat="server">
                                    <asp:Label ID="lbl_result" runat="server" Text=""></asp:Label>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="Panel1" runat="server">
                                    <asp:Label ID="Label1" runat="server" Text="Select Table"></asp:Label>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="Panel2" runat="server">
                                    <asp:DropDownList ID="TableDropDownList" runat="server" OnSelectedIndexChanged="TableDropDownList_SelectedIndexChanged"></asp:DropDownList></asp:Panel>
                            </td>
                            <td><div id="tip_Table" class="toolTip"><asp:Label ID="lbl_TableTip" runat="server">?</asp:Label></div></td>
                        </tr>
                    </table>
                </div>
                <div id="NavButtons">
                    <asp:Button ID="btn_back" runat="server" Text="Back" OnClick="btn_Back_Click" Style="width: 47px; height: 26px;" />
                    &nbsp;
                    <asp:Button ID="btn_Next" runat="server" Text="Next" OnClick="btn_Next_Click" Style="width: 47px" />
                </div>

            </div>
            <!-- closing wrapper-->
            <div id="footer">ETL Management 2018 | LYIT | <a href="AdminLogin.aspx">Admin Login</a></div>
        </div>
    </form>
</body>
</html>
