<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="2_FeedFileSourceDetails.aspx.cs" Inherits="DataEntryProcess1._2_FeedFileSourceDetails" %>

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
                <h3>Enter Source Details</h3>
                               <table>
                                <tr>
                                    <td><asp:Label Text="Dataset Name" runat="server" /></td>
                                    <td><asp:TextBox runat="server" ID ="txt_DataSetName" /></td>
                                </tr>
                                <tr>
                                    <td><asp:Label Text="FileLocation" runat="server" /></td>
                                    <td><asp:TextBox runat="server" ID ="txt_FileLocation" /></td>
                                </tr>
                                <tr>
                                    <td><asp:Label Text="File Name" runat="server" /></td>
                                    <td><asp:TextBox runat="server" ID ="txt_FileName" /></td>
                                </tr>
                                <tr>
                                    <td><asp:Label Text="DelimiterChar" runat="server" /></td>
                                    <td><asp:TextBox runat="server" ID ="txt_Delimiter" /></td>
                                </tr>
                            </table>
            
                               <asp:Button ID="btn_next" runat="server" OnClick="btn_next_Click" Text="Next" />
            
            </div>
            <!-- closing wrapper-->
        </div>
    </div>
    </form>
</body>
</html>
