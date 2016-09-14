<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SingleShipmentPreviewFromExchange.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.SingleShipmentPreviewFromExchange" %>
<%@ Register Src="../../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Item/DeliveryCompanySelectorView.ascx" TagName="DeliveryCompanySelectorView"
    TagPrefix="uc4" %>
<%@ Register Src="../Item/ExchangeOutboundDetailsList.ascx" TagName="ExchangeOutboundDetailsList"
    TagPrefix="uc5" %>
<%@ Register Src="../../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc6" %>
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        倉儲資訊</h2>
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th width="20%">
                    出貨倉儲
                </th>
                <td class="tdleft">
                    <%# _item.EXCHANGE_GOODS.OUTBOUND_WAREHOUSE.WAREHOUSE_NAME %>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    轉入單據號碼
                </th>
                <td class="tdleft">
                    <%# _item.EXCHANGE_GOODS.EXCHANGE_GOODS_NUMBER %>
                </td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<div class="border_gray">
    <h2>
        配送資訊</h2>
    <table class="table01" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th nowrap>
                    宅配公司名稱
                </th>
                <th nowrap>
                    統編
                </th>
                <th nowrap>
                    地址
                </th>
                <th nowrap>
                    電話
                </th>
                <th nowrap>
                    傳真
                </th>
                <th nowrap>
                    聯絡人
                </th>
                <th width="120" nowrap>
                    聯絡人Email
                </th>
            </tr>
            <tr>
                <td align="center">
                    <%# _item.BUYER_SHIPMENT.DELIVERY_COMPANY.DELIVERY_COMPANY_NAME %>
                </td>
                <td align="center">
                    <%# _item.BUYER_SHIPMENT.DELIVERY_COMPANY.DELIVERY_COMPANY_BAN%>
                </td>
                <td align="center">
                    <%# _item.BUYER_SHIPMENT.DELIVERY_COMPANY.DELIVERY_COMPANY_ADDRESS%>
                </td>
                <td align="center">
                    <%# _item.BUYER_SHIPMENT.DELIVERY_COMPANY.DELIVERY_COMPANY_PHONE%>
                </td>
                <td align="center">
                    <%# _item.BUYER_SHIPMENT.DELIVERY_COMPANY.DELIVERY_COMPANY_FAX%>
                </td>
                <td align="center">
                    <%# _item.BUYER_SHIPMENT.DELIVERY_COMPANY.CONTACT_NAME%>
                </td>
                <td width="120" align="center">
                    <%# _item.BUYER_SHIPMENT.DELIVERY_COMPANY.CONTACT_EMAIL%>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<uc5:ExchangeOutboundDetailsList ID="outboundDetails" runat="server" />
<uc3:DataModelContainer ID="modelItem" runat="server" Suffix=".Exchange" />
<cc1:DocumentDataSource ID="dsEntity" runat="server">
</cc1:DocumentDataSource>
