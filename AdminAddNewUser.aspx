<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminAddNewUser.aspx.cs" Inherits="ELTManagement.AdminAddNewUser" %>

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
                <div id="LogOutOption"><a href="Home.aspx">Admin Log Out</a></div>
            </div>


            <div id="menu">
                <h2>Home</h2>
                <a href="Home.aspx">Home</a>
                <a href="ImportData.aspx">Import Data</a>
                <a href="Configure.aspx">Configure</a>
                <a href="Reporting.aspx">Reporting</a>
                <a href="ErrorLogs.aspx">Error Logs</a>
            </div>

            <!--Center Content is the only div to change-->
            <div id="CenterContent">
                <div id="PageTitle">
                    <h2>Enter User Details</h2>
                </div>
                <div id="PageContent">
                    <table>
                        <tr>
                            <th>Username</th>
                            <th>Password</th>
                            <th>Role</th>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txt_Username" runat="server"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txt_Password" runat="server"></asp:TextBox></td>
                            <td>
                                <asp:DropDownList ID="drp_role" runat="server"></asp:DropDownList></td>
                        </tr>
                    </table>
                    <asp:Label ID="lbl_error" runat="server"></asp:Label>
                </div>
                <div id="NavButtons">
                    <asp:Button ID="btn_Execute" runat="server" Text="Enter" OnClick="btn_Execute_Click" />
                </div>

            </div>
            <!-- closing wrapper-->
            <div id="footer">ETL Management 2018 | LYIT | <a href="AdminLogin.aspx">Admin Login</a></div>
        </div>
    </form>
</body>
</html>
