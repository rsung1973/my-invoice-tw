<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateBuyerOrder.ascx.cs"
    Inherits="eIVOGo.Module.SCM.CreateBuyerOrder" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Src="Item/MarketResourceSelector.ascx" TagName="MarketResourceSelector"
    TagPrefix="uc3" %>
<%@ Register Src="Item/MarketResourceSelectorView.ascx" TagName="MarketResourceSelectorView"
    TagPrefix="uc4" %>
<%@ Register Src="Item/SelectBuyer.ascx" TagName="SelectBuyer" TagPrefix="uc5" %>
<%@ Register Src="Item/WarehouseSelector.ascx" TagName="WarehouseSelector" TagPrefix="uc6" %>
<%@ Register Src="../Common/EnumSelector.ascx" TagName="EnumSelector" TagPrefix="uc7" %>
<%@ Register Src="Item/ProductQueryByField.ascx" TagName="ProductQueryByField" TagPrefix="uc9" %>
<%@ Register Src="Item/BODetailsEditList.ascx" TagName="BODetailsEditList" TagPrefix="uc8" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc10" %>
<%@ Register Src="../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc11" %>


<!--路徑名稱-->
<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 新增訂單" />
<!--交易畫面標題-->
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增訂單" />
<uc4:MarketResourceSelectorView ID="marketResource" runat="server" />
<!--按鈕-->
<uc5:SelectBuyer ID="buyer" runat="server" />
<!--按鈕-->
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        訂單設定</h2>
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th width="15%">
                    <span style="color: red">*</span>出貨倉儲
                </th>
                <td class="tdleft" width="35%">
                    <uc6:WarehouseSelector ID="warehouse" runat="server" />
                </td>
                <th class="tdleft" width="15%">
                    <span style="color: red">*</span>訂單類型
                </th>
                <td class="tdleft">
                    <uc7:EnumSelector ID="buyerOrderType" runat="server" TypeName="Model.Locale.Naming+BuyerOrderTypeDefinition, Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                        SelectedValue='<%#  _item.BUYER_ORDER_TYPE %>' />
                </td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        料品資訊
    </h2>
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th class="Head_style_a" colspan="8">
                    新增訂單料品
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
            <tr>
                <th width="15%">
                    促銷專案代號
                </th>
                <td class="tdleft" width="35%">
                    <uc9:ProductQueryByField ID="productQueryByPromotionID" runat="server" />
                </td>
                <th class="tdleft" width="15%">
                    促銷專案名稱
                </th>
                <td class="tdleft">
                    <uc9:ProductQueryByField ID="productQueryByPromotionName" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>
    <uc8:BODetailsEditList ID="boDetails" runat="server" />
    <!--表格 結束-->
</div>
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        訂單折扣</h2>
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th width="15%">
                    <span style="color: red">*</span>折扣金額
                </th>
                <td class="tdleft" width="35%">
                    <asp:TextBox ID="discountAmt" runat="server" Text='<%#  _item.ORDERS_DISCOUNT_AMOUNT %>'></asp:TextBox>
                </td>
                <th class="tdleft" width="15%">
                    折扣說明
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="discountDescription" runat="server" Columns="40" Text='<%#  _item.DISCOUNT_DESCRIPTION %>'></asp:TextBox>
                </td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<table border="0" cellspacing="0" cellpadding="0" width="100%">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnCalc" runat="server" Text="總價試算" OnClick="btnCalc_Click" />
                &nbsp;
            </td>
        </tr>
    </tbody>
</table>
<div id="divResult" runat="server" visible="false" enableviewstate="false" class="border_gray">
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
                    <%# String.Format("{0:##,###,###,###,##0}", _item.ORDERS_ORIGINAL_AMOUNT)%>
                </td>
                <td align="center">
                    <%# String.Format("{0:##,###,###,###,##0}", _item.ORDERS_DISCOUNT_AMOUNT)%>
                </td>
                <td align="center">
                    <%# String.Format("{0:##,###,###,###,##0}", _item.ORDERS_AMOUNT)%>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<!--按鈕-->
<table border="0" cellspacing="0" cellpadding="0" width="100%" runat="server" id="tblPreview"
    visible="false" enableviewstate="false">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnConfirm" runat="server" Text="訂單預覽" OnClick="btnConfirm_Click" />
            </td>
        </tr>
    </tbody>
</table>
<cc1:BuyerOrdersDataSource ID="dsEntity" runat="server">
</cc1:BuyerOrdersDataSource>
<uc10:DataModelContainer ID="modelItem" runat="server" />
<uc11:PageAnchor ID="ToPreviewBuyerOrder" runat="server" TransferTo="~/SCM/PreviewBuyerOrder.aspx" />
