<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminEditUsers.aspx.cs" Inherits="ELTManagement.AdminEditUsers" %>

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
                <label id="lbl_UpdateInfo" runat="server" text=""></label>
                <div id="PageTitleCenter">
                    <h2>Edit Users</h2>
                </div>
                <div id="PageContentCenter">
                                <div id="EditUsers">

            <asp:GridView ID="GridViewEditUser" runat="server" AutoGenerateColumns="false" DataKeyNames="Username" OnSelectedIndexChanged="GridViewEditUser_SelectedIndexChanged" OnRowEditing="GridViewEditUser_RowEditing" OnRowDeleting="GridViewEditUser_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="Username" HeaderText="Username" />
                    <asp:BoundField DataField="Password" HeaderText="Password"/>
                    <asp:BoundField DataField="Role" HeaderText="Role" />
                    <asp:CommandField ShowEditButton="true" />
                    <asp:CommandField ShowDeleteButton="true" />
                </Columns>
            </asp:GridView>
                                </div>
                </div>

            </div>
            <!-- closing wrapper-->
        </div>
    </form>
</body>
</html>
