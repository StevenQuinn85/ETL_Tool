<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportResults.aspx.cs" Inherits="ELTManagement.ImportResults" %>

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
                    <h2>Import Results</h2>
                </div>
                <div id="PageContentCenter">
                    <h4>Import Details</h4>
                    <table>
                        <tr>
                            <td>Status:</td>
                            <td>
                                <asp:Label ID="lbl_status" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Start Time:</td>
                            <td>
                                <asp:Label ID="lbl_StartTime" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>End Time:</td>
                            <td>
                                <asp:Label ID="lbl_EndTime" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                    <h4>Raw Data Details</h4>
                    <table>
                        <tr>
                            <td>Records Imported:</td>
                            <td>
                                <label id="lbl_rawImported" runat="server"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>Records Rejected:</td>
                            <td>
                                <label id="lbl_rawOmmited" runat="server"></label>
                            </td>
                        </tr>
                    </table>
                    <h4>Transformation Details</h4>
                    <table>
                        <tr>
                            <td>Records Accepted:</td>
                            <td>
                                <label id="lbl_transformAccpeted" runat="server"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>Records Rejected:</td>
                            <td>
                                <label id="lbl_transformRejected" runat="server"></label>
                            </td>
                        </tr>
                    </table>

                    <h4>Data Load Details:</h4>
                    <table>
                        <tr>
                            <td>Records Inserted:</td>
                            <td>
                                <label id="lbl_DLInsert" runat="server"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>Records Rejected:</td>
                            <td>
                                <label id="lbl_DLReject" runat="server"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>Records Deleted:</td>
                            <td>
                                <label id="lbl_DLDelete" runat="server"></label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="NavButtonsCenter">
                    <asp:Button ID="btn_finish" runat="server" Text="Finish" OnClick="btn_finish_Click" />
                </div>

            </div>
            <!-- closing wrapper-->
            <div id="footer">ETL Management 2018 | LYIT | <a href="AdminLogin.aspx">Admin Login</a></div>
        </div>
    </form>
</body>
</html>
