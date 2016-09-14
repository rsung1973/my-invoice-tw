<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierDetails.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.SupplierDetails" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        供應商</h2>
    <asp:GridView ID="gvEntity" runat="server" Width="100%" CellPadding="0" EnableViewState="False"
        GridLines="None" AutoGenerateColumns="False" CssClass="table01" DataSourceID="dsEntity">
        <Columns>
            <asp:TemplateField HeaderText="供應商名稱">
                <ItemTemplate>
                    <%# ((SUPPLIER)Container.DataItem).SUPPLIER_NAME %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="統編">
                <ItemTemplate>
                    <%# ((SUPPLIER)Container.DataItem).SUPPLIER_BAN%></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="地址">
                <ItemTemplate>
                    <%# ((SUPPLIER)Container.DataItem).SUPPLIER_ADDRESS%></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="電話">
                <ItemTemplate>
                    <%# ((SUPPLIER)Container.DataItem).SUPPLIER_PHONE%></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="傳真">
                <ItemTemplate>
                    <%# ((SUPPLIER)Container.DataItem).SUPPLIER_FAX%></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="聯絡人">
                <ItemTemplate>
                    <%# ((SUPPLIER)Container.DataItem).SUPPLIER_CONTACT_NAME%></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="聯絡人Email">
                <ItemTemplate>
                    <%# ((SUPPLIER)Container.DataItem).CONTACT_EMAIL%></ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <AlternatingRowStyle CssClass="OldLace" />
        <PagerTemplate>
            <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
    </asp:GridView>
    <!--表格 結束-->
</div>
<cc1:SupplierDataSource ID="dsEntity" runat="server">
</cc1:SupplierDataSource>
