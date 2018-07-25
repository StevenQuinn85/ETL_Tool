<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="9_EditProcessDetails.aspx.cs" Inherits="ELTManagement._9_EditProcessDetails" %>

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
            </div>


            <div id="menuUpdatePage">
                <h2>Home</h2>
                <a href="ImportData.aspx">Import Data</a>
                <a href="Configure.aspx">Configure</a>
                <a href="Reporting.aspx">Reporting</a>
                <a href="ErrorLogs.aspx">Error Logs</a>
            </div>

            <!--Center Content is the only div to change-->
            <div id="CenterContentUpdatePage">
                <div id="PageTitle">
                    <h2>Edit Import Process Details</h2>
                </div>
                <h3>Data Properties</h3>
                <table>
                    <tr>
                        <td>Import Type:</td>
                        <td>
                            <asp:Label ID="Lbl_ImportType" runat="server" Text=""></asp:Label>
                        </td>

                    </tr>
                </table>

                <br />
                <h3>Source Details</h3>
                <asp:Panel ID="SourceDetailsPanel" runat="server"></asp:Panel>
                <div id="UpdateSource" class="right">
                    <br />
                    <asp:Button ID="Update1" runat="server" Text="Update" OnClick="Update_Click" /><br />
                </div>
                <br />
                <h3>Destination Details</h3>
                <asp:Panel ID="DestinationDetailsPanel" runat="server"></asp:Panel>

                <div id="Update_Destination" class="right">
                    <br />
                    <asp:Button ID="Update" runat="server" Text="Update" OnClick="Update_Click" /><br />
                </div>


                <br />
                <h3>Primary Key</h3>
                <asp:GridView ID="GridViewPrimaryKey" runat="server" AutoGenerateColumns="false" DataKeyNames="ProcessID" OnPageIndexChanging="GridViewPK_PageIndexChanging" OnRowDeleting="GridViewPK_RowDeleting" OnSelectedIndexChanged="GridViewPrimaryKey_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="PrimaryKey" HeaderText="Primary Key" />
                        <asp:CommandField ShowDeleteButton="true" />
                    </Columns>
                </asp:GridView>
                <h3>Metadata Details</h3>


                <asp:GridView ID="GridViewMetaData" runat="server" AutoGenerateColumns="false" DataKeyNames="ProcessID" OnPageIndexChanging="GridViewMetaData_PageIndexChanging" OnRowCancelingEdit="GridViewMetaData_RowCancelingEdit" OnRowDeleting="GridViewMetaData_RowDeleting" OnRowEditing="GridViewMetaData_RowEditing" OnRowUpdating="GridViewMetaData_RowUpdating">
                    <Columns>
                        <asp:BoundField DataField="ColumnName" HeaderText="Column Name" />
                        <asp:BoundField DataField="DataType" HeaderText="Data Type" ControlStyle-Width="50px" />
                        <asp:BoundField DataField="MinLength" HeaderText="Min Length" />
                        <asp:BoundField DataField="MaxLength" HeaderText="Max Length" />
                        <asp:BoundField DataField="NullsPermitted" HeaderText="Nulls Permitted" ControlStyle-Width="50px" />
                        <asp:BoundField DataField="NullAction" HeaderText="Null Action" ControlStyle-Width="50px" />
                        <asp:BoundField DataField="ReplaceValue" HeaderText="Replace Value" ControlStyle-Width="50px" />
                        <asp:BoundField DataField="ColumnOrder" HeaderText="Column Order" />
                        <asp:CommandField ShowEditButton="true" />
                        <asp:CommandField ShowDeleteButton="true" />
                    </Columns>
                </asp:GridView>
                <br />
                <h3>Add New Metadata Record</h3>
                <table>
                   <tr><th>Order</th>
                       <th>Column Name</th>
                       <th>Primary Key</th>
                       <th>Data Type</th>
                       <th>Min Length</th>
                       <th>Max Length</th>
                       <th>Nulls Permitted</th>
                       <th>Null Action</th>
                       <th>Default Value</th>
                   </tr>
                    <tr><td>
                        <asp:TextBox ID="txt_ColumnOrder" runat="server" Width ="30px"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txt_ColumnName" runat="server"></asp:TextBox></td>
                        <td>
                            <asp:CheckBox ID="check_PrimaryKey" runat="server" /></td>
                        <td>
                            <asp:DropDownList ID="drp_DataType" runat="server"></asp:DropDownList></td>
                        <td>
                            <asp:TextBox ID="txt_MinLength" runat="server" Width ="30px"></asp:TextBox></td>
                        <td><asp:TextBox ID="txt_MaxLength" runat="server" Width ="30px"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="drp_NullPermitted" runat="server"></asp:DropDownList></td>
                        <td>
                            <asp:DropDownList ID="drp_NullAction" runat="server"></asp:DropDownList></td>
                        <td><asp:TextBox ID="txt_ReplaceValue" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <div id="Add_MetaDataRecord" class="right">
                    <br />
                    <asp:Button ID="Add_MetaData" runat="server" Text="Insert" OnClick="Add_MetaData_Click"/><br />
                    <asp:Label Text="" runat="server" ID="Lbl_AddMetaError" />
                    <br />
                    <asp:Label Text="" runat="server" ID="Lbl_PrimaryKeyError" />
                    <br />
                </div>

            </div>
            <!-- closing wrapper-->
        </div>
    </form>
</body>
</html>
