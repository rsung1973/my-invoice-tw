<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControl1.ascx.cs" Inherits="eIVOGo.Published.WebUserControl1" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>

<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    DataSourceID="dsOrg" ondatabinding="GridView1_DataBinding" 
    ondatabound="GridView1_DataBound" onload="GridView1_Load">
    <Columns>
        <asp:TemplateField HeaderText="CompanyID">
        <ItemTemplate>
            
        </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<cc1:OrganizationDataSource ID="dsOrg" runat="server">
</cc1:OrganizationDataSource>

