<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateExchangeGoods.ascx.cs"
    Inherits="eIVOGo.Module.SCM.CreateExchangeGoods" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Src="../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/DeliveryCompanySelectorView.ascx" TagName="DeliveryCompanySelectorView"
    TagPrefix="uc4" %>
<%@ Register Src="Item/BODetailsListForShipment.ascx" TagName="BODetailsListForShipment"
    TagPrefix="uc5" %>
<%@ Register Src="../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc6" %>
<%@ Register src="Item/WarehouseSelector.ascx" tagname="WarehouseSelector" tagprefix="uc7" %>
<%@ Register src="Item/ExchagneInboundDetailsEditList.ascx" tagname="ExchagneInboundDetailsEditList" tagprefix="uc8" %>
<%@ Register Src="Item/ProductQueryByField.ascx" TagName="ProductQueryByField" TagPrefix="uc9" %>
<%@ Register Src="Item/ExchangeOutboundDetailsEditList.ascx" TagName="ExchangeOutboundDetailsEditList" TagPrefix="uc10" %>


<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 新增換貨單" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增換貨單" />
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
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        換貨資訊</h2>
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th>
                    <span style="color: red">*</span>換貨原因
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="Reason" runat="server" TextMode="MultiLine" Text='<%# _item.EG_REASON %>'></asp:TextBox>
                </td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        入庫倉儲資訊
    </h2>
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th class="Head_style_a" colspan="2">
                    選擇入庫倉儲
                </th>
            </tr>
            <tr>
                <th width="15%">
                    <span style="color: red">*</span>入庫倉儲
                </th>
                <td class="tdleft">
                    <uc7:WarehouseSelector ID="inboundWarehouse" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<uc8:ExchagneInboundDetailsEditList ID="inboundDetails" 
    runat="server" />
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        出貨倉儲資訊
    </h2>
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th class="Head_style_a" colspan="2">
                    選擇出貨倉儲
                </th>
            </tr>
            <tr>
                <th width="15%">
                    <span style="color: red">*</span>出貨倉儲
                </th>
                <td class="tdleft">
                    <uc7:WarehouseSelector ID="outboundWarehouse" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        出貨料品資訊
    </h2>
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th class="Head_style_a" colspan="8">
                    新增出貨料品 
                </th>
            </tr>
            <tr>
                <th width="15%">
                    料品Barcode
                </th>
                <td class="tdleft" width="35%">
                    <asp:TextBox ID="Barcode" runat="server"></asp:TextBox>
                    <asp:Button ID="btnAddItem" runat="server" Text="新增料品" OnClick="btnAddItem_Click" />
                </td>
                <th class="tdleft" width="15%">
                    料品名稱
                </th>
                <td class="tdleft">
                    <uc9:ProductQueryByField ID="productQueryByName" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>
    <uc10:ExchangeOutboundDetailsEditList ID="outboundDetails" runat="server" />
    <!--表格 結束-->
</div>
<table border="0" cellspacing="0" cellpadding="0" width="100%">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnReturn" runat="server" Text="回上頁" OnClick="btnReturn_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="btnConfirm" runat="server" Text="換貨單預覽" OnClick="btnConfirm_Click" />
            </td>
        </tr>
    </tbody>
</table>
<uc3:DataModelContainer ID="exchangeGoods" runat="server" />
<cc1:BuyerOrdersDataSource ID="dsEntity" runat="server">
</cc1:BuyerOrdersDataSource>
<uc6:PageAnchor ID="ToPreviewExchange" runat="server" TransferTo="~/SCM/PreviewExchangeGoods.aspx" />
<uc6:PageAnchor ID="ToInquireExchange" runat="server" TransferTo="~/SCM/BuyerOrderQueryForExchangeGoods.aspx" />
