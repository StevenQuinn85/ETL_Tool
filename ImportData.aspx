<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportData.aspx.cs" Inherits="ELTManagement.ImportData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="mystyle.css" />
    <title>ETL Management</title>

</head>
<body>
    <form id="form1" runat="server">
        <%-- Add a timeout to the script manager to accomodate long running imports timeout = 360000 --%>
        <asp:ScriptManager ID="manager" runat="server" AsyncPostBackTimeout="36000"></asp:ScriptManager>
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
                <div id="PageTitleCenter">
                    <h2>Perform Data Import</h2>
                </div>
                <div id="PageContentCenter">
                    <h3>Select Dataset to Import</h3>
                    <br />
                    <asp:DropDownList ID="drp_Datasets" runat="server"></asp:DropDownList>
                </div>
                <div id="NavButtons">
                </div>

                <%--                    Inserting a div with a script to display a processing image whilst the system is completing and import
                    Code taking from http://www.dotnetcurry.com/ShowArticle.aspx?ID=227--%>

                <div id="ProcessingImage" class="center">
                    <asp:UpdateProgress ID="LoadingProgress" AssociatedUpdatePanelID="" runat="server">
                        <ProgressTemplate>
                            <img src="Images/ProcessingImage.gif" />
                        </ProgressTemplate>

                    </asp:UpdateProgress>

                    <asp:UpdatePanel runat="server" ID="LoadingPanel">

                        <ContentTemplate>
                            <div id="ExecuteButton" class="center">
                                <asp:Button ID="btn_Execute" runat="server" Text="Execute" OnClick="btn_Execute_Click" />
                            </div>

                            <div>
                                <asp:Label ID="lbl_processing" runat="server" Text=""></asp:Label>
                            </div>
                        </ContentTemplate>

                    </asp:UpdatePanel>
                    <%-- End of script--%>
                </div>
            </div>
            <!-- closing wrapper-->
            <div id="footer">ETL Management 2018 | LYIT | <a href="AdminLogin.aspx">Admin Login</a></div>
        </div>
    </form>
</body>
</html>
