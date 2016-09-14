<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeGoodsPreview.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.ExchangeGoodsPreview" %>
<%@ Register Src="../../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Item/DeliveryCompanySelectorView.ascx" TagName="DeliveryCompanySelectorView"
    TagPrefix="uc4" %>
<%@ Register Src="../Item/ExchangeInboundDetailsList.ascx" TagName="ExchangeInboundDetailsList"
    TagPrefix="uc5" %>
<%@ Register Src="../../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc6" %>
<%@ Register Src="../Item/ExchangeOutboundDetailsList.ascx" TagName="ExchangeOutboundDetailsList"
    TagPrefix="uc1" %>
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        訂單資訊</h2>
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th width="15%">
                    訂單單號
                </th>
                <td class="tdleft">
                    <%# _item.BUYER_ORDERS.BUYER_ORDERS_NUMBER %>
                </td>
            </tr>
            <tr>
                <th width="15%">
                    發票號碼
                </th>
                <td class="tdleft">
                    <%# _item.BUYER_ORDERS.CDS_Document.BUYER_SHIPMENT.InvoiceItem.TrackCode + _item.BUYER_ORDERS.CDS_Document.BUYER_SHIPMENT.InvoiceItem.No %>
                </td>
            </tr>
            <tr>
                <th width="15%">
                    購物平台
                </th>
                <td class="tdleft">
                    <%# _item.BUYER_ORDERS.MARKET_RESOURCE.MARKET_RESOURCE_NAME %>
                </td>
            </tr>
            <tr>
                <th width="15%">
                    買受人
                </th>
                <td class="tdleft">
                    <%# _item.BUYER_ORDERS.BUYER_DATA.BUYER_NAME %>
                </td>
            </tr>
            <tr>
                <th width="15%">
                    入庫倉儲
                </th>
                <td class="tdleft">
                    <%# _item.INBOUND_WAREHOUSE.WAREHOUSE_NAME %>
                </td>
            </tr>
            <tr>
                <th width="15%">
                    換貨原因
                </th>
                <td class="tdleft">
                    <%# _item.EG_REASON %>
                </td>
            </tr>
            <tr>
                <th width="15%">
                    出貨倉儲
                </th>
                <td class="tdleft">
                    <%# _item.OUTBOUND_WAREHOUSE.WAREHOUSE_NAME %>
                </td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<uc5:ExchangeInboundDetailsList ID="inboundDetails" runat="server" />
<uc1:ExchangeOutboundDetailsList ID="outboundDetails" runat="server" />
<uc3:DataModelContainer ID="modelItem" runat="server" />
<cc1:ExchangeGoodsDataSource ID="dsEntity" runat="server">
</cc1:ExchangeGoodsDataSource>
