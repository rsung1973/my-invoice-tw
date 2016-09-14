<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceCancellationUploadList.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.Item.InvoiceCancellationUploadList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.InvoiceManagement" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<!--表格 開始-->
<asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
    GridLines="None" CellPadding="0" CssClass="table01" ClientIDMode="Static" EnableViewState="False"
    AllowPaging="false" ShowFooter="True">
    <Columns>
        <asp:TemplateField HeaderText="序號">
            <ItemTemplate>
                <%#  ((GoogleInvoiceItem)Container.DataItem).Invoice!=null?((GoogleInvoiceItem)Container.DataItem).Invoice.InvoicePurchaseOrder.OrderNo.Substring(0,11):"" %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Google ID">
            <ItemTemplate>
                <%# ((GoogleInvoiceItem)Container.DataItem).Invoice!=null?((GoogleInvoiceItem)Container.DataItem).Invoice.InvoiceBuyer.CustomerID:"" %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="發票號碼">
            <ItemTemplate>
                <%# ((GoogleInvoiceItem)Container.DataItem).Invoice != null ? ((GoogleInvoiceItem)Container.DataItem).Invoice.TrackCode + ((GoogleInvoiceItem)Container.DataItem).Invoice.No : ""%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="作廢原因">
            <ItemTemplate>
                <%# ((GoogleInvoiceItem)Container.DataItem).Invoice!=null?((GoogleInvoiceItem)Container.DataItem).Invoice.InvoiceCancellation.CancelReason:"" %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="買受人名稱">
            <ItemTemplate>
                <%# ((GoogleInvoiceItem)Container.DataItem).Invoice!=null?((GoogleInvoiceItem)Container.DataItem).Invoice.InvoiceBuyer.CustomerName:"" %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="買受人統編" FooterText="總計">
            <ItemTemplate>
                <%# ((GoogleInvoiceItem)Container.DataItem).Invoice!=null?
                ((GoogleInvoiceItem)Container.DataItem).Invoice.InvoiceBuyer.IsB2C()? "N/A" : ((GoogleInvoiceItem)Container.DataItem).Invoice.InvoiceBuyer.ReceiptNo
                            :"" %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="未稅金額">
            <ItemTemplate>
                <%# ((GoogleInvoiceItem)Container.DataItem).Invoice!=null? String.Format("{0:##,###,###,###,###}", ((GoogleInvoiceItem)Container.DataItem).Invoice.InvoiceAmountType.SalesAmount):"" %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <FooterTemplate>
                匯入總筆數：<%# ((GoogleInvoiceCancellationUploadManager)_mgr).ItemList.Count %></FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="稅額">
            <ItemTemplate>
                <%# ((GoogleInvoiceItem)Container.DataItem).Invoice!=null? String.Format("{0:##,###,###,###,###}", ((GoogleInvoiceItem)Container.DataItem).Invoice.InvoiceAmountType.TaxAmount):"" %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <FooterStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="含稅金額">
            <ItemTemplate>
                <%# ((GoogleInvoiceItem)Container.DataItem).Invoice!=null? String.Format("{0:##,###,###,###,###}", ((GoogleInvoiceItem)Container.DataItem).Invoice.InvoiceAmountType.TotalAmount):"" %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <FooterTemplate>
                成功：<%# ((GoogleInvoiceCancellationUploadManager)_mgr).ItemList.Where(i=>i.UploadStatus==Naming.UploadStatusDefinition.等待匯入 || i.UploadStatus==Naming.UploadStatusDefinition.匯入成功).Count()  %></FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="匯入狀態">
            <ItemTemplate>
                <%# String.IsNullOrEmpty(((GoogleInvoiceItem)Container.DataItem).Status)? ((GoogleInvoiceItem)Container.DataItem).UploadStatus.ToString() : String.Format("<font color='red'>{0}</font>",((GoogleInvoiceItem)Container.DataItem).Status)%></ItemTemplate>
            <ItemStyle HorizontalAlign="Left" />
            <FooterTemplate>
                失敗：<%# ((GoogleInvoiceCancellationUploadManager)_mgr).ItemList.Where(i=>i.UploadStatus==Naming.UploadStatusDefinition.資料錯誤 || i.UploadStatus==Naming.UploadStatusDefinition.匯入失敗).Count()  %></FooterTemplate>
        </asp:TemplateField>
    </Columns>
    <FooterStyle HorizontalAlign="Center" />
    <PagerStyle HorizontalAlign="Center" />
    <SelectedRowStyle />
    <HeaderStyle />
    <AlternatingRowStyle CssClass="OldLace" />
    <RowStyle />
    <EditRowStyle />
</asp:GridView>
<uc2:PagingControl ID="pagingList" runat="server" />
<center>
    <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"
        Text="查無資料!!" EnableViewState="false"></asp:Label>
</center>
