<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReturnedGoodsList.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.ReturnedGoodsList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Item/SupplierProductsItem.ascx" TagName="SupplierProductsItem"
    TagPrefix="uc2" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<%@ Import Namespace="Utility" %>
<%@ Register Src="ShowRefusalReason.ascx" TagName="ShowRefusalReason" TagPrefix="uc5" %>
<%@ Register Src="RefuseDocument.ascx" TagName="RefuseDocument" TagPrefix="uc6" %>
<%@ Register Src="../Action/PrintEntityPreview.ascx" TagName="PrintEntityPreview"
    TagPrefix="uc8" %>
<%@ Register Src="../Action/PrintInvoicePreview.ascx" TagName="PrintInvoicePreview"
    TagPrefix="uc9" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="GOODS_RETURNED_SN" DataSourceID="dsEntity" EnableViewState="False"
        OnRowCommand="gvEntity_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="日期" SortExpression="">
                <ItemTemplate>
                    <%# ValueValidity.ConvertChineseDateString(Eval("GOODS_RETURNED_DATETIME"))%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="退貨單號碼" SortExpression="">
                <ItemTemplate>
                    <a href="#" onclick="<%# doPrintReturn.GetPostBackEventReference(String.Format("{0}",((GOODS_RETURNED)Container.DataItem).GOODS_RETURNED_SN)) %>">
                        <%# ((GOODS_RETURNED)Container.DataItem).GOODS_RETURNED_NUMBER%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="訂單號碼" SortExpression="">
                <ItemTemplate>
                    <a href="#" onclick="<%# doPrintOrder.GetPostBackEventReference(String.Format("{0}",((GOODS_RETURNED)Container.DataItem).BUYER_ORDERS_SN)) %>">
                        <%# ((GOODS_RETURNED)Container.DataItem).GOODS_RETURNED_NUMBER%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="發票號碼" SortExpression="">
                <ItemTemplate>
                    <a onclick="<%# doPrintInvoice.GetPostBackEventReference(String.Format("{0}",((GOODS_RETURNED)Container.DataItem).BUYER_ORDERS.CDS_Document.BUYER_SHIPMENT.INVOICE_SN)) %>" href="#"><%# ((GOODS_RETURNED)Container.DataItem).BUYER_ORDERS.CDS_Document.BUYER_SHIPMENT.INVOICE_SN.HasValue ? ((GOODS_RETURNED)Container.DataItem).BUYER_ORDERS.CDS_Document.BUYER_SHIPMENT.InvoiceItem.TrackCode + ((GOODS_RETURNED)Container.DataItem).BUYER_ORDERS.CDS_Document.BUYER_SHIPMENT.InvoiceItem.No : ""%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="購物平台" SortExpression="MARKET_RESOURCE_SN">
                <ItemTemplate>
                    <%# ((GOODS_RETURNED)Container.DataItem).BUYER_ORDERS.MARKET_RESOURCE.MARKET_RESOURCE_NAME %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="買受人" SortExpression="BUYER_SN">
                <ItemTemplate>
                    <%# ((GOODS_RETURNED)Container.DataItem).BUYER_ORDERS.BUYER_DATA.BUYER_NAME %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="金額" SortExpression="ORDERS_AMOUNT">
                <ItemTemplate>
                    <%# String.Format("{0:##,###,###,###,##0}",((GOODS_RETURNED)Container.DataItem).BUYER_ORDERS.ORDERS_AMOUNT) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="結案入庫" SortExpression="BO_SHIPMENT_STATUS">
                <ItemTemplate>
                    <%# ((GOODS_RETURNED)Container.DataItem).GR_STATUS==1?"是":"否" %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <center><font color="red">查無資料!!</font></center>
        </EmptyDataTemplate>
        <EmptyDataRowStyle CssClass="noborder" />
        <PagerTemplate>
            <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
    </asp:GridView>
</div>
<cc1:ReturnedGoodsDataSource ID="dsEntity" runat="server">
</cc1:ReturnedGoodsDataSource>
<uc3:ActionHandler ID="doDelete" runat="server" />
<uc3:ActionHandler ID="doEdit" runat="server" />
<uc3:ActionHandler ID="doCreate" runat="server" />
<uc3:ActionHandler ID="doShowRefusal" runat="server" />
<uc6:RefuseDocument ID="refuseDoc" runat="server" />
<uc5:ShowRefusalReason ID="showRefusal" runat="server" />
<uc3:ActionHandler ID="doPrintOrder" runat="server" />
<uc8:PrintEntityPreview ID="printBuyerOrder" runat="server" PreviewControlPath="~/Module/SCM/View/BuyerOrderPreview.ascx"  />
<uc3:ActionHandler ID="doPrintReturn" runat="server" />
<uc8:PrintEntityPreview ID="printReturn" runat="server" PreviewControlPath="~/Module/SCM/View/ReturnedGoodsPreview.ascx" />
<uc3:ActionHandler ID="doPrintInvoice" runat="server" />
<uc9:PrintInvoicePreview ID="printInvoice" runat="server" />
