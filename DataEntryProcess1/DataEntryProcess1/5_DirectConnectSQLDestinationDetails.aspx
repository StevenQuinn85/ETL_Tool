﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="5_DirectConnectSQLDestinationDetails.aspx.cs" Inherits="DataEntryProcess1._5_DirectConnectSQLDestinationDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <link rel="stylesheet" type="text/css" href="mystyle.css"/>
    <title>ETL Admin</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
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
                    <a href="WebForm1.aspx">Import Data</a>
                    <a href="ConfigureImport.aspx">Configurations</a>
                    <a href="#">OPTION 3</a>
                    <a href="#">OPTION 4</a>
                </div>

            <!--Center Content is the only div to change-->
 <div id="CenterContent">
  <h2>Enter SQL Server Destination Details</h2>
                <br/>
                <table>
                    <tr>
                        <td><label>Enter Server Name</label></td>                        
                        <td><asp:TextBox ID="txt_ServerName" runat="server" Width ="250"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><label>Enter Database Name</label></td>                        
                        <td><asp:TextBox ID="txt_DatabaseName" runat="server" Width ="250"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="PasswordRequired" runat="server" /></td>
                    </tr>
                                        <tr>
                        <td><label>Enter User Name</label></td>                        
                        <td><asp:TextBox ID="txt_username" runat="server" Width ="250"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><label>Enter Password</label></td>                        
                        <td><asp:TextBox ID="txt_password" runat="server" Width ="250"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <br/><asp:Button ID="btn_Test" runat="server" Text="Test" OnClick="btn_Test_Click" /></td>
                        <td>
                            <asp:Panel ID="ConnectionResult" runat="server">
                                <asp:Label ID="lbl_result" runat="server" Text=""></asp:Label></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="Panel1" runat="server">
                                <asp:Label ID="Label1" runat="server" Text="Select Table"></asp:Label>
                                </asp:Panel></td>
                        <td>
                               <asp:Panel ID="Panel2" runat="server"><asp:DropDownList ID="TableDropDownList" runat="server" OnSelectedIndexChanged="TableDropDownList_SelectedIndexChanged"></asp:DropDownList></asp:Panel>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button ID="btn_Next" runat="server" Text="Next" OnClick="btn_Next_Click" style="width: 47px" Visible="False" />

            </div>
            <!-- closing wrapper-->
    </div>
        </div>
    </form>
</body>
</html>
