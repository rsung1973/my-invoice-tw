<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.SCM.View.ExchangeGoodsList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<%@ Import Namespace="Utility" %>
<%@ Register Src="../../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc4" %>
<%@ Register Src="ShowRefusalReason.ascx" TagName="ShowRefusalReason" TagPrefix="uc5" %>
<%@ Register Src="RefuseDocument.ascx" TagName="RefuseDocument" TagPrefix="uc6" %>
<%@ Register src="../Action/PrintEntityPreview.ascx" tagname="PrintEntityPreview" tagprefix="uc8" %>
<%@ Register src="../Action/PrintInvoicePreview.ascx" tagname="PrintInvoicePreview" tagprefix="uc9" %>
<div class="border_gray">
    <h2>單據清冊</h2>
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="EXCHANGE_GOODS_SN" DataSourceID="dsEntity" EnableViewState="False" ClientIDMode="Static"
        OnRowCommand="gvEntity_RowCommand">
        <Columns>
<asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <input type="radio" name="rbSN" value='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>' />
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:TemplateField HeaderText="日期" SortExpression="EXCHANGE_GOODS_DATETIME">
                <ItemTemplate>
                    <%# ValueValidity.ConvertChineseDateString(Eval("EXCHANGE_GOODS_DATETIME")) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="換貨單號碼" SortExpression="EXCHANGE_GOODS_NUMBER">
                <ItemTemplate>
                    <a href="#" onclick="<%# doPrintExchange.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>">
                        <%# Eval("EXCHANGE_GOODS_NUMBER") %></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="訂單號碼" SortExpression="EXCHANGE_GOODS_SN">
                <ItemTemplate>
                    <a href="#" onclick="<%# doPrintOrder.GetPostBackEventReference(String.Format("{0}",((EXCHANGE_GOODS)Container.DataItem).BUYER_ORDERS_SN)) %>">
                        <%# ((EXCHANGE_GOODS)Container.DataItem).BUYER_ORDERS.BUYER_ORDERS_NUMBER %></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="發票號碼" SortExpression="BUYER_ORDERS_SN">
                <ItemTemplate>
                    <a onclick="<%# doPrintInvoice.GetPostBackEventReference(String.Format("{0}",((EXCHANGE_GOODS)Container.DataItem).BUYER_ORDERS.CDS_Document.BUYER_SHIPMENT.INVOICE_SN)) %>" href="#"><%# ((EXCHANGE_GOODS)Container.DataItem).BUYER_ORDERS.CDS_Document.BUYER_SHIPMENT.INVOICE_SN.HasValue ? ((EXCHANGE_GOODS)Container.DataItem).BUYER_ORDERS.CDS_Document.BUYER_SHIPMENT.InvoiceItem.TrackCode + ((EXCHANGE_GOODS)Container.DataItem).BUYER_ORDERS.CDS_Document.BUYER_SHIPMENT.InvoiceItem.No : ""%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="購物平台" SortExpression="EG_WW_STATUS">
                <ItemTemplate>
                    <%# ((EXCHANGE_GOODS)Container.DataItem).BUYER_ORDERS.MARKET_RESOURCE.MARKET_RESOURCE_NAME %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="買受人" SortExpression="EG_BS_STATUS">
                <ItemTemplate>
                    <%# ((EXCHANGE_GOODS)Container.DataItem).BUYER_ORDERS.BUYER_DATA.BUYER_NAME %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="金額" SortExpression="EG_REASON">
                <ItemTemplate>
                    <%# String.Format("{0:##,###,###,###,##0}", ((EXCHANGE_GOODS)Container.DataItem).BUYER_ORDERS.ORDERS_AMOUNT) %>
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
<cc1:ExchangeGoodsDataSource ID="dsEntity" runat="server">
</cc1:ExchangeGoodsDataSource>
<uc3:ActionHandler ID="doDelete" runat="server" />
<uc3:ActionHandler ID="doEdit" runat="server" />
<uc3:ActionHandler ID="doCreate" runat="server" />
<uc3:ActionHandler ID="doShowRefusal" runat="server" />
<uc3:ActionHandler ID="doPrintExchange" runat="server" />
<uc3:ActionHandler ID="doPrintOrder" runat="server" />
<uc4:PageAnchor ID="ToEditBuyerOrder" runat="server" TransferTo="~/SCM/EditBuyerOrder.aspx" />
<uc5:ShowRefusalReason ID="showRefusal" runat="server" />
<uc6:RefuseDocument ID="refuseDoc" runat="server" />
<uc8:PrintEntityPreview ID="printExchange" runat="server" PreviewControlPath="~/Module/SCM/View/ExchangeGoodsPreview.ascx" />
<uc8:PrintEntityPreview ID="printBuyerOrder" runat="server" PreviewControlPath="~/Module/SCM/View/BuyerOrderPreview.ascx" />
<uc3:ActionHandler ID="doPrintInvoice" runat="server" />
<uc9:PrintInvoicePreview ID="printInvoice" runat="server" />
