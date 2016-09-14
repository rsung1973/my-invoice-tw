<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuyerOrderPreview.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.BuyerOrderPreview" %>
<%@ Register Src="../Item/BODetailsList.ascx" TagName="BODetailsList" TagPrefix="uc8" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Register src="../Item/MarketResourcePreview.ascx" tagname="MarketResourcePreview" tagprefix="uc10" %>
<%@ Register src="../Item/BuyerPreview.ascx" tagname="BuyerPreview" tagprefix="uc11" %>
<%@ Register src="../../Common/DataModelContainer.ascx" tagname="DataModelContainer" tagprefix="uc12" %>
<!--路徑名稱-->
<!--交易畫面標題-->
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        訂單資訊</h2>
    <table class="table01" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th width="20%">
                    出貨倉儲
                </th>
                <td class="tdleft">
                    <%# _item.WAREHOUSE!=null?_item.WAREHOUSE.WAREHOUSE_NAME : null%>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    訂單類型
                </th>
                <td class="tdleft">
                    <%# Enum.GetName(typeof(Naming.BuyerOrderTypeDefinition), (Naming.BuyerOrderTypeDefinition)_item.BUYER_ORDER_TYPE)%>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    購物平台
                </th>
                <td class="tdleft">
                    <%# _item.MARKET_RESOURCE.MARKET_RESOURCE_NAME %>
                </td>
            </tr>
            <uc10:MarketResourcePreview ID="resourcePreview" runat="server" />
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<uc11:BuyerPreview ID="buyerPreview" runat="server" />
<!--按鈕-->
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        料品資訊
    </h2>
    <uc8:BODetailsList ID="boDetails" runat="server" />
    <!--表格 結束-->
</div>
<!--按鈕-->
<div class="border_gray">
    <h2>
        合計</h2>
    <table class="table01" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th nowrap>
                    原始總售價
                </th>
                <th nowrap>
                    折扣金額
                </th>
                <th nowrap>
                    銷售總金額
                </th>
            </tr>
            <tr>
                <td align="center">
                    <%# String.Format("{0:##,###,###,###,##0}", _item.ORDERS_ORIGINAL_AMOUNT) %>
                </td>
                <td align="center">
                    <%# String.Format("{0:##,###,###,###,##0}", _item.ORDERS_DISCOUNT_AMOUNT) %>
                </td>
                <td align="center">
                    <%# String.Format("{0:##,###,###,###,##0}", _item.ORDERS_AMOUNT) %>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<!--按鈕-->
<cc1:BuyerOrdersDataSource ID="dsEntity" runat="server">
</cc1:BuyerOrdersDataSource>
<uc12:DataModelContainer ID="modelItem" runat="server" />



