<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ELTManagement.Login" %>

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
                <div id="LogOutOption"><a href="Login.aspx">Log Out</a></div>
            </div>

            <!--Center Content is the only div to change-->
            <div id="CenterContentLogin">
                <div id="PageTitle">
                    <h2>ELT Management Login</h2>
                </div>
                <div id="PageContent">

                    <table>
                        <tr>
                            <td>
                                <label>Username: </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Username" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Password: </label>
                            </td>
  
                           <td>
                                <asp:TextBox ID="txt_password" runat="server" TextMode="Password"></asp:TextBox></td>
                       
                                </tr>
                                <tr>
                                    <td>
                                <asp:Button ID="btn_login" runat="server" Text="Login" OnClick="btn_login_Click" />
                            </td>
                            <td><asp:Label ID="lbl_error" runat="server"></asp:Label></td>
                        </tr>
                    </table>

                </div>


            </div>
            <!-- closing wrapper-->

        </div>
    </form>
</body>
</html>
