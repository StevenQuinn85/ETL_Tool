<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="7_ManualEnterNumOfColumns.aspx.cs" Inherits="DataEntryProcess1._7_ManualEnterNumOfColumns" %>

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
                                <h2>How many columns are in the source ?</h2>

                            <asp:Label ID="Label2" runat="server" Text="Number of Columns"></asp:Label>
                            <asp:TextBox ID="txt_numberofcolumns" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btn_Next" runat="server" Text="Next" OnClick="btn_Next_Click" />

            </div>
            <!-- closing wrapper-->
        </div>
    </div>
    </form>
</body>
</html>
