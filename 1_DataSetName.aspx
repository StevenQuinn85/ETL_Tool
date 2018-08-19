<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="1_DataSetName.aspx.cs" Inherits="ELTManagement.DataEntryForms._1_DataSetName" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ETL Management</title>
    <link href="../mystyle.css" rel="stylesheet" />
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
                                    <asp:Label ID="Label1" runat="server" Text="Dataset Name: "></asp:Label>
                <asp:TextBox ID="txt_DatasetName" runat="server" Width ="250px"></asp:TextBox>
                <asp:Label ID ="ErrorLabel" runat ="server"></asp:Label>
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
