<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminLogin.aspx.cs" Inherits="ELTManagement.AdminLogin" %>

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
            <div id="LogOutOption"><a href="Login.aspx">Log Out</a></div>
            <div id="banner_lower">
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
            <div id="CenterContentLogin">
                <div id="PageTitle">
                    <h2>Admin Login</h2>
                </div>
                <div id="PageContent">
                    <table>
                        <tr>
                            <td>
                                <label>Username: </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Username" runat="server" Width="200px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Password: </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_password" runat="server" TextMode="Password" Width="200px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_login" runat="server" Text="Login" OnClick="btn_login_Click" />
                            </td>
                            <td>
                                <asp:Label ID="lbl_error" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>


            </div>

            <!-- closing wrapper-->
            <div id="footer">ETL Management 2018 | LYIT | <a href="AdminLogin.aspx">Admin Login</a></div>
        </div>
    </form>
</body>
</html>
