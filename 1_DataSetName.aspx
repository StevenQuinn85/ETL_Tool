<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="1_DataSetName.aspx.cs" Inherits="ELTManagement.DataEntryForms._1_DataSetName" %>

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
                <div id="banner_title"><h1>ETL Management</h1></div>
            </div>

            <div id="banner_lower">
            <div id="LogOutOption"><a href="Login.aspx">Log Out</a></div>
            </div>


                <div id="menu">
                    <h2>Menu</h2>
                                    <a href="Home.aspx">Home</a>
                    <a href="ImportData.aspx">ImportData</a>
                    <a href="Configure.aspx">Configure</a>
                    <a href="Reporting.aspx">Reporting</a>
                    <a href="ErrorLogs.aspx">Error Logs</a>
                </div>

            <!--Center Content is the only div to change-->
            <div id="CenterContent">
                <div id="PageTitle"><h2>Enter Dataset Name</h2></div>

                <div id ="PageContent">

                    <table>
                        <tr>
                            <td><asp:Label ID="Label1" runat="server" Text="Dataset Name: "></asp:Label></td>
                            <td><asp:TextBox ID="txt_DatasetName" runat="server" Width ="250px"></asp:TextBox></td>
                            <td><div id="tip_DataSet" class="toolTip"><asp:Label ID="lbl_DataSetTip" runat="server">?</asp:Label></div></td>
                            <td><asp:Label ID ="ErrorLabel" runat ="server"></asp:Label></td>
                        </tr>
                    </table>
                
                <br />

                </div>
                <div id ="NavButtons">
                                <asp:Button ID="btn_Next" runat="server" Text="Next" OnClick="btn_Confirm_Click" /></div>
            </div>
            <!-- closing wrapper-->
                            <div id="footer">ETL Management 2018 | LYIT | <a href="AdminLogin.aspx">Admin Login</a></div>
        </div>
    </form>
</body>
</html>
