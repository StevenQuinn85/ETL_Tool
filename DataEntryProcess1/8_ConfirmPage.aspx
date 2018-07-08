<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="8_ConfirmPage.aspx.cs" Inherits="DataEntryProcess1._8_ConfirmPage" %>

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
            <h3>Confirm Data Properties</h3>
                <!--Table For Table properties-->
                <div id ="DataPropertiesTable">
                    <asp:Literal ID="SourceDetailsDisplay" runat="server"></asp:Literal>
                    <br>
                    <asp:Literal ID="DataDestinationDetails" runat="server"></asp:Literal>
                    <br>
                    <asp:Literal ID="PrimaryKeyDetails" runat="server"></asp:Literal>
                    <br>
                    <asp:Literal ID="MetaDataDisplay" runat="server"></asp:Literal>
                </div>
            
            </div>
            <!-- closing wrapper-->
        </div>
    </div>
    </form>
</body>
</html>
