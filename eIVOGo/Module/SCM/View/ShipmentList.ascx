<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShipmentList.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.ShipmentList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Item/SupplierProductsItem.ascx" TagName="SupplierProductsItem"
    TagPrefix="uc2" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Register Src="../../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc4" %>
<%@ Register Src="../Action/PrintEntityPreview.ascx" TagName="PrintEntityPreview"
    TagPrefix="uc5" %>
<%@ Register Src="../Action/PrintInvoicePreview.ascx" TagName="PrintInvoicePreview"
    TagPrefix="uc6" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="DocID" DataSourceID="dsEntity" EnableViewState="False" OnRowCommand="gvEntity_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="日期" SortExpression="SHIPMENT_DATETIME">
                <ItemTemplate>
                    <%# ValueValidity.ConvertChineseDateString(((CDS_Document)Container.DataItem).BUYER_SHIPMENT.SHIPMENT_DATETIME) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="出貨單號" SortExpression="BUYER_SHIPMENT_NUMBER">
                <ItemTemplate>
                    <%# ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? String.Format("<a onclick=\"{0}\" href=\"#\">{1}</a>",doPrintOrder.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))), ((CDS_Document)Container.DataItem).BUYER_SHIPMENT.BUYER_SHIPMENT_NUMBER)
                                                                                                : ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? String.Format("<a onclick=\"{0}\" href=\"#\">{1}</a>",doPrintOrderFromExchange.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))), ((CDS_Document)Container.DataItem).BUYER_SHIPMENT.BUYER_SHIPMENT_NUMBER)
                                                                                                                        : ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.PurchaseOrderReturned ? String.Format("<a onclick=\"{0}\" href=\"#\">{1}</a>",doPrintOrderFromReturnedPO.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))), ((CDS_Document)Container.DataItem).BUYER_SHIPMENT.BUYER_SHIPMENT_NUMBER)
                                                                                                : ""%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="發票號碼" SortExpression="INVOICE_SN">
                <ItemTemplate>
                    <a onclick="<%# doPrintInvoice.GetPostBackEventReference(String.Format("{0}",((CDS_Document)Container.DataItem).BUYER_SHIPMENT.INVOICE_SN)) %>"
                        href="#">
                        <%# ((CDS_Document)Container.DataItem).BUYER_SHIPMENT.INVOICE_SN.HasValue ? ((CDS_Document)Container.DataItem).BUYER_SHIPMENT.InvoiceItem.TrackCode + ((CDS_Document)Container.DataItem).BUYER_SHIPMENT.InvoiceItem.No : ""%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="轉入單據種類" SortExpression="">
                <ItemTemplate>
                    <%# ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? "訂單"
                                                                                                : ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? "換貨單"
                                                                                                                        : ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.PurchaseOrderReturned ? "採購退貨單"
                                                                                                : ""%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="出貨倉儲" SortExpression="BUYER_SHIPMENT_SN">
                <ItemTemplate>
                    <%# ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? ((CDS_Document)Container.DataItem).BUYER_ORDERS.WAREHOUSE.WAREHOUSE_NAME
                                                                                                : ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? ((CDS_Document)Container.DataItem).EXCHANGE_GOODS.BUYER_ORDERS.WAREHOUSE.WAREHOUSE_NAME
                                                                                                                        : ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.PurchaseOrderReturned ? ((CDS_Document)Container.DataItem).PURCHASE_ORDER_RETURNED.WAREHOUSE.WAREHOUSE_NAME
                                                                                                : ""%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="買受人" SortExpression="BUYER_SHIPMENT_SN">
                <ItemTemplate>
                    <%# ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? ((CDS_Document)Container.DataItem).BUYER_ORDERS.BUYER_DATA.BUYER_NAME
                                                                                                : ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? ((CDS_Document)Container.DataItem).EXCHANGE_GOODS.BUYER_ORDERS.BUYER_DATA.BUYER_NAME
                                                : ""%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="金額" SortExpression="BUYER_SHIPMENT_SN">
                <ItemTemplate>
                    <%# ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? String.Format("{0:##,###,###,###,##0}", ((CDS_Document)Container.DataItem).BUYER_ORDERS.ORDERS_AMOUNT)
                                                                                                : ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? String.Format("{0:##,###,###,###,##0}", ((CDS_Document)Container.DataItem).EXCHANGE_GOODS.EXCHANGE_GOODS_OUTBOND_DETAILS.Sum(o => o.GR_BS_QUANTITY * o.BO_UNIT_PRICE))
                                                                                                                        : ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.PurchaseOrderReturned ? String.Format("{0:##,###,###,###,##0}", ((CDS_Document)Container.DataItem).PURCHASE_ORDER_RETURNED.PURCHASE_ORDER_RETURNED_DETAILS.Sum(p => p.POR_QUANTITY * p.POR_UNIT_PRICE))
                                                                                                : ""%>
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
<cc1:DocumentDataSource ID="dsEntity" runat="server">
</cc1:DocumentDataSource>
<uc3:ActionHandler ID="doDelete" runat="server" />
<uc3:ActionHandler ID="doEdit" runat="server" />
<uc3:ActionHandler ID="doCreate" runat="server" />
<uc3:ActionHandler ID="doShowRefusal" runat="server" />
<uc3:ActionHandler ID="doPrintOrder" runat="server" />
<uc3:ActionHandler ID="doPrintOrderFromExchange" runat="server" />
<uc3:ActionHandler ID="doPrintOrderFromReturnedPO" runat="server" />
<uc3:ActionHandler ID="doPrintInvoice" runat="server" />
<uc4:PageAnchor ID="ToShowInvoice" runat="server" TransferTo="~/Published/PrintInvoicePage.aspx" />
<uc5:PrintEntityPreview ID="printShipment" runat="server" PreviewControlPath="~/Module/SCM/View/SingleShipmentPreview.ascx" />
<uc5:PrintEntityPreview ID="printShipmentFromExchange" runat="server" PreviewControlPath="~/Module/SCM/View/SingleShipmentPreviewFromExchange.ascx" />
<uc5:PrintEntityPreview ID="printShipmentFromReturnedPO" runat="server" PreviewControlPath="~/Module/SCM/View/SingleShipmentPreviewFromReturnedPO.ascx" />
<uc6:PrintInvoicePreview ID="printInvoice" runat="server" />
