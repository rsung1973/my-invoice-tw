<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestInsertMode.aspx.cs" Inherits="eIVOGo.Published.TestInsertMode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:DetailsView ID="DetailsView1" runat="server" AllowPaging="True" 
            AutoGenerateRows="False" DataKeyNames="RoleID" DataSourceID="SqlDataSource1" 
            GridLines="None">
            <Fields>
                <asp:BoundField DataField="RoleID" HeaderText="RoleID" ReadOnly="True" 
                    SortExpression="RoleID" />
                <asp:BoundField DataField="SiteMenu" HeaderText="SiteMenu" 
                    SortExpression="SiteMenu" />
                <asp:BoundField DataField="Role" HeaderText="Role" SortExpression="Role" />
                <asp:CommandField ButtonType="Button" ShowDeleteButton="True" 
                    ShowEditButton="True" ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:Model.Properties.Settings.eInvoiceConnectionString %>" 
            DeleteCommand="DELETE FROM [UserRoleDefinition] WHERE [RoleID] = @RoleID" 
            InsertCommand="INSERT INTO [UserRoleDefinition] ([RoleID], [SiteMenu], [Role]) VALUES (@RoleID, @SiteMenu, @Role)" 
            SelectCommand="SELECT * FROM [UserRoleDefinition]" 
            UpdateCommand="UPDATE [UserRoleDefinition] SET [SiteMenu] = @SiteMenu, [Role] = @Role WHERE [RoleID] = @RoleID">
            <DeleteParameters>
                <asp:Parameter Name="RoleID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="RoleID" Type="Int32" />
                <asp:Parameter Name="SiteMenu" Type="String" />
                <asp:Parameter Name="Role" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="SiteMenu" Type="String" />
                <asp:Parameter Name="Role" Type="String" />
                <asp:Parameter Name="RoleID" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
    
    </div>
    </form>
</body>
</html>
