<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnterpriseGroupMemberList.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.Lists.EnterpriseGroupMemberList" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc4" %>
<%@ Register Src="~/Module/Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc5" %>
    <br />
    <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
        GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" EnableViewState="False"
        DataSourceID="dsEntity" ShowFooter="True" DataKeyNames="CompanyID">
        <Columns>
            <asp:TemplateField HeaderText="買受人名稱" SortExpression="CompanyName">
                <ItemTemplate>
                    <%# ((Organization)Container.DataItem).CompanyName %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="統編" SortExpression="ReceiptNo">
                <ItemTemplate>
                    <%# ((Organization)Container.DataItem).ReceiptNo%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="地址" SortExpression="Addr">
                <ItemTemplate>
                    <%# ((Organization)Container.DataItem).Addr%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="電話" SortExpression="Phone">
                <ItemTemplate>
                    <%# ((Organization)Container.DataItem).Phone%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="傳真" SortExpression="Fax">
                <ItemTemplate>
                    <%# ((Organization)Container.DataItem).Fax%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="聯絡人" SortExpression="ContactName">
                <ItemTemplate>
                    <%# ((Organization)Container.DataItem).ContactName%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="聯絡人Email" SortExpression="ContactEmail">
                <ItemTemplate>
                    <%# ((Organization)Container.DataItem).ContactEmail.Replace(",", ",\n")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="買受人狀態" SortExpression="CurrentLevel" Visible="false">
                <ItemTemplate>
                    <%# ((Organization)Container.DataItem).OrganizationStatus.LevelExpression.Expression %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <FooterStyle CssClass="total-count" />
        <PagerStyle HorizontalAlign="Center" />
        <SelectedRowStyle />
        <HeaderStyle />
        <AlternatingRowStyle CssClass="OldLace" />
        <PagerTemplate>
            <uc2:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
        <RowStyle />
        <EditRowStyle />
    </asp:GridView>

<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<uc4:DataModelCache ID="modelItem" runat="server" KeyName="CompanyID" />
