<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.SCM.View.BuyerOrderList" %>
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
<%@ Register src="../Action/PrintInvoicePreview.ascx" tagname="PrintInvoicePreview" tagprefix="uc9" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="BUYER_ORDERS_SN" DataSourceID="dsEntity" EnableViewState="False"
        OnRowCommand="gvEntity_RowCommand">
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <input type="radio" name="rbSN" value='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="日期" SortExpression="BO_DATETIME">
                <ItemTemplate>
                    <%# ValueValidity.ConvertChineseDateString(Eval("BO_DATETIME"))%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="訂單號碼" SortExpression="BUYER_ORDERS_NUMBER">
                <ItemTemplate>
                    <a href="#" onclick="<%# doPrintOrder.GetPostBackEventReference(String.Format("{0}",((BUYER_ORDERS)Container.DataItem).BUYER_ORDERS_SN)) %>">
                        <%# ((BUYER_ORDERS)Container.DataItem).BUYER_ORDERS_NUMBER%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="出貨單號" SortExpression="">
                <ItemTemplate>
                    <%# ((BUYER_ORDERS)Container.DataItem).CDS_Document.BUYER_SHIPMENT != null ? String.Format("<a onclick=\"{0}\" href=\"#\">{1}</a>", doPrintOrder.GetPostBackEventReference(String.Format("{0}", Eval(gvEntity.DataKeyNames[0]))),((BUYER_ORDERS)Container.DataItem).CDS_Document.BUYER_SHIPMENT.BUYER_SHIPMENT_NUMBER) 
                        : "N/A"%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="發票號碼" SortExpression="">
                <ItemTemplate>
                    <a onclick="<%# doPrintInvoice.GetPostBackEventReference(String.Format("{0}",((BUYER_ORDERS)Container.DataItem).CDS_Document.BUYER_SHIPMENT.INVOICE_SN)) %>" href="#"><%# ((BUYER_ORDERS)Container.DataItem).CDS_Document.BUYER_SHIPMENT.INVOICE_SN.HasValue ? ((BUYER_ORDERS)Container.DataItem).CDS_Document.BUYER_SHIPMENT.InvoiceItem.TrackCode + ((BUYER_ORDERS)Container.DataItem).CDS_Document.BUYER_SHIPMENT.InvoiceItem.No : ""%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="購物平台" SortExpression="MARKET_RESOURCE_SN">
                <ItemTemplate>
                    <%# ((BUYER_ORDERS)Container.DataItem).MARKET_RESOURCE.MARKET_RESOURCE_NAME %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="買受人" SortExpression="BUYER_SN">
                <ItemTemplate>
                    <%# ((BUYER_ORDERS)Container.DataItem).BUYER_DATA.BUYER_NAME %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ORDERS_AMOUNT" HeaderText="金額" SortExpression="ORDERS_AMOUNT"
                DataFormatString="{0:##,###,###,###,##0}" />
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
<cc1:BuyerOrdersDataSource ID="dsEntity" runat="server">
</cc1:BuyerOrdersDataSource>
<uc3:ActionHandler ID="doDelete" runat="server" />
<uc3:ActionHandler ID="doEdit" runat="server" />
<uc3:ActionHandler ID="doCreate" runat="server" />
<uc3:ActionHandler ID="doShowRefusal" runat="server" />
<uc6:RefuseDocument ID="refuseDoc" runat="server" />
<uc5:ShowRefusalReason ID="showRefusal" runat="server" />
<uc3:ActionHandler ID="doPrintOrder" runat="server" />
<uc8:PrintEntityPreview ID="printBuyerOrder" runat="server" PreviewControlPath="~/Module/SCM/View/BuyerOrderPreview.ascx" />
<uc3:ActionHandler ID="doPrintShipment" runat="server" />
<uc8:PrintEntityPreview ID="printShipment" runat="server" PreviewControlPath="~/Module/SCM/View/SingleShipmentPreview.ascx" />
<uc3:ActionHandler ID="doPrintInvoice" runat="server" />
<uc9:PrintInvoicePreview ID="printInvoice" runat="server" />

