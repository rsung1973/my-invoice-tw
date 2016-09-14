<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Base.InvoiceItemList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceDate.ascx" TagPrefix="uc1" TagName="InvoiceDate" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/BuyerCustomerID.ascx" TagPrefix="uc1" TagName="BuyerCustomerID" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/PurchaseOrderNo.ascx" TagPrefix="uc1" TagName="PurchaseOrderNo" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceSeller.ascx" TagPrefix="uc1" TagName="InvoiceSeller" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceSellerReceiptNo.ascx" TagPrefix="uc1" TagName="InvoiceSellerReceiptNo" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceNoPreview.ascx" TagPrefix="uc1" TagName="InvoiceNoPreview" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/SalesAmount.ascx" TagPrefix="uc1" TagName="SalesAmount" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/TaxAmount.ascx" TagPrefix="uc1" TagName="TaxAmount" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/TotalAmount.ascx" TagPrefix="uc1" TagName="TotalAmount" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/WinningInvoice.ascx" TagPrefix="uc1" TagName="WinningInvoice" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceDonation.ascx" TagPrefix="uc1" TagName="InvoiceDonation" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/BuyerReceiptNo.ascx" TagPrefix="uc1" TagName="BuyerReceiptNo" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceRemark.ascx" TagPrefix="uc1" TagName="InvoiceRemark" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/SMSNotification.ascx" TagPrefix="uc1" TagName="SMSNotification" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceCarrierNo.ascx" TagPrefix="uc1" TagName="InvoiceCarrierNo" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<!--表格 開始-->
<asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
    GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" ClientIDMode="Static"
    EnableViewState="False" DataSourceID="dsEntity" ShowFooter="True">
    <Columns>
        <asp:TemplateField HeaderText="日期">
            <ItemTemplate>
                <uc1:InvoiceDate runat="server" ID="InvoiceDate" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="GoogleID/客戶ID">
            <ItemTemplate>
                <uc1:BuyerCustomerID runat="server" ID="BuyerCustomerID" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="序號">
            <ItemTemplate>
                <uc1:PurchaseOrderNo runat="server" ID="PurchaseOrderNo" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="開立發票營業人">
            <ItemTemplate>
                <uc1:InvoiceSeller runat="server" ID="InvoiceSeller" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="統編">
            <ItemTemplate>
                <uc1:InvoiceSellerReceiptNo runat="server" ID="InvoiceSellerReceiptNo" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="發票">
            <ItemTemplate>
                <uc1:InvoiceNoPreview runat="server" ID="InvoiceNoPreview" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="未稅金額">
            <ItemTemplate>
                <uc1:SalesAmount runat="server" ID="SalesAmount" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="稅額">
            <ItemTemplate>
                <uc1:TaxAmount runat="server" ID="TaxAmount" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="含稅金額">
            <ItemTemplate>
                <uc1:TotalAmount runat="server" ID="TotalAmount" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="是否中獎">
            <ItemTemplate>
                <uc1:WinningInvoice runat="server" ID="WinningInvoice" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>共<%# _totalRecordCount %>筆</FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="捐贈單位" Visible="false">
            <ItemTemplate>
                <uc1:InvoiceDonation runat="server" ID="InvoiceDonation" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="買受人統編" FooterText="總計金額：">
            <ItemTemplate>
                <uc1:BuyerReceiptNo runat="server" ID="BuyerReceiptNo" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="備註">
            <ItemTemplate>
                <uc1:InvoiceRemark runat="server" ID="InvoiceRemark" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                <%# String.Format("{0:##,###,###,##0}", _subtotal) %>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="簡訊通知">
            <ItemTemplate>
                <uc1:SMSNotification runat="server" ID="SMSNotification" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="載具資訊">
            <ItemTemplate>
                <uc1:InvoiceCarrierNo runat="server" ID="InvoiceCarrierNo" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
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
