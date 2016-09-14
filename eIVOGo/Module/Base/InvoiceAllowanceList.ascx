<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceAllowanceList.ascx.cs"
    Inherits="eIVOGo.Module.Base.InvoiceAllowanceList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/EIVO/AllowanceField/AllowanceDate.ascx" TagPrefix="uc2" TagName="AllowanceDate" %>
<%@ Register Src="~/Module/EIVO/AllowanceField/AllowanceSeller.ascx" TagPrefix="uc2" TagName="AllowanceSeller" %>
<%@ Register Src="~/Module/EIVO/AllowanceField/AllowanceSellerReceiptNo.ascx" TagPrefix="uc2" TagName="AllowanceSellerReceiptNo" %>
<%@ Register Src="~/Module/EIVO/AllowanceField/AllowanceNoPreview.ascx" TagPrefix="uc2" TagName="AllowanceNoPreview" %>
<%@ Register Src="~/Module/EIVO/AllowanceField/TotalAmount.ascx" TagPrefix="uc2" TagName="TotalAmount" %>

<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Utility" %>
<!--表格 開始-->
<asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
    GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" ClientIDMode="Static"
    EnableViewState="False" ShowFooter="True" DataSourceID="dsEntity">
    <Columns>
        <asp:TemplateField HeaderText="日期">
            <ItemTemplate>
                <uc2:AllowanceDate runat="server" ID="AllowanceDate" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="開立發票營業人">
            <ItemTemplate>
                <uc2:AllowanceSeller runat="server" ID="AllowanceSeller" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="統編">
            <ItemTemplate>
                <uc2:AllowanceSellerReceiptNo runat="server" ID="AllowanceSellerReceiptNo" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="折讓號碼">
            <ItemTemplate>
                <uc2:AllowanceNoPreview runat="server" ID="AllowanceNoPreview" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                共<%# _totalRecordCount %>筆
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="金額">
            <ItemTemplate>
                <uc2:TotalAmount runat="server" ID="TotalAmount" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>總計金額：<%# String.Format("{0:##,###,###,##0}", _subtotal) %></FooterTemplate>
        </asp:TemplateField>
    </Columns>
    <FooterStyle />
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
<center>
    <span style="font-size:larger;color:red;"><%# _totalRecordCount==0 ? "查無資料":null %></span>
</center>
<cc1:DocumentDataSource ID="dsEntity" runat="server">
</cc1:DocumentDataSource>
