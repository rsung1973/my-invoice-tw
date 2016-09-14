<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateShipmentFromExchange.ascx.cs"
    Inherits="eIVOGo.Module.SCM.CreateShipmentFromExchange" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register src="../Common/DataModelContainer.ascx" tagname="DataModelContainer" tagprefix="uc3" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<%@ Register src="Item/DeliveryCompanySelectorView.ascx" tagname="DeliveryCompanySelectorView" tagprefix="uc4" %>
<%@ Register src="Item/ExchangeOutboundDetailsList.ascx" tagname="ExchangeOutboundDetailsList" tagprefix="uc5" %>
<%@ Register src="../Common/PageAnchor.ascx" tagname="PageAnchor" tagprefix="uc6" %>


<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 新增出貨單" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增出貨單" />
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
<uc4:DeliveryCompanySelectorView ID="delivery" 
        runat="server" />
<uc5:ExchangeOutboundDetailsList ID="outboundDetails" runat="server" />
<table border="0" cellspacing="0" cellpadding="0" width="100%" >
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnReturn" runat="server" Text="回上頁" 
                    onclick="btnReturn_Click"  />
                &nbsp;&nbsp;
                <asp:Button ID="btnConfirm" runat="server" Text="出貨單預覽" OnClick="btnConfirm_Click" />
            </td>
        </tr>
    </tbody>
</table>
<uc3:DataModelContainer ID="singleShipment" runat="server" Suffix=".Exchange" />
<cc1:ExchangeGoodsDataSource ID="dsEntity" runat="server">
</cc1:ExchangeGoodsDataSource>
<uc6:PageAnchor ID="ToPreviewShipment" runat="server" TransferTo="~/SCM/PreviewShipmentFromExchange.aspx" />
<uc6:PageAnchor ID="ToInquireShipment" runat="server" TransferTo="~/SCM/ExchangeQueryForShipment.aspx" />


