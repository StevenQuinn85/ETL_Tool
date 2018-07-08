<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="4_DestinationSelection.aspx.cs" Inherits="DataEntryProcess1._4_DestinationSelection" %>

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
  
                <h2>Select Destination Type</h2>
                <br/>
                <asp:RadioButtonList ID="ServerSelectionList" runat="server">
                    <asp:ListItem>SQL Server</asp:ListItem>
                    <asp:ListItem>Oracle Database</asp:ListItem>
                    <asp:ListItem>Access</asp:ListItem>
                    <asp:ListItem>Text File</asp:ListItem>
                </asp:RadioButtonList>



                <asp:Button ID="btn_Next" runat="server" Text="Next" OnClick="btn_Next_Click" style="width: 47px" />
            </div>
            <!-- closing wrapper-->
        </div>
    </div>
    </form>
</body>
</html>
