<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="2_FeedFileSourceDetails.aspx.cs" Inherits="ELTManagement.DataEntryForms._2_FeedFileSourceDetails" %>

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
                <a href="ImportData.aspx">ImportData</a>
                <a href="Configure.aspx">Configure</a>
                <a href="Reporting.aspx">Reporting</a>
                <a href="ErrorLogs.aspx">Error Logs</a>
            </div>

            <!--Center Content is the only div to change-->
            <div id="CenterContent">
                <div id="PageTitle">
                    <h2>Enter Source Details</h2>
                </div>
                <div id="PageContent">
                    <table>
                        <tr>
                            <td>
                                <asp:Label Text="FileLocation" runat="server" /></td>
                            <td>
                                <asp:TextBox runat="server" ID="txt_FileLocation" Width="250px" /></td>
                            <td><div id="tip_FileLocation" class="toolTip"><asp:Label ID="lbl_FileLocationTip" runat="server">?</asp:Label></div></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label Text="File Name" runat="server" /></td>
                            <td>
                                <asp:TextBox runat="server" ID="txt_FileName" Width="250px" /></td>
                            <td><div id="tip_FileName" class="toolTip"><asp:Label ID="lbl_FileNameTip" runat="server">?</asp:Label></div></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label Text="DelimiterChar" runat="server" /></td>
                            <td>
                                <asp:TextBox runat="server" ID="txt_Delimiter" Width="250px" /></td>
                            <td><div id="tip_Delimiter" class="toolTip"><asp:Label ID="lbl_DelimterTip" runat="server">?</asp:Label></div></td>
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
