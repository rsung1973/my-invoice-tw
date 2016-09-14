<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeGoodsQuery.ascx.cs"
    Inherits="eIVOGo.Module.SCM.ExchangeGoodsQuery" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc4" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc4" %>
<%@ Register Src="../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc8" %>
<%@ Register Src="View/BuyerOrderList.ascx" TagName="BuyerOrderList" TagPrefix="uc9" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc10" %>
<%@ Register Src="Item/WarehouseSelector.ascx" TagName="WarehouseSelector" TagPrefix="uc11" %>
<%@ Register Src="Item/MarketResourceSelector.ascx" TagName="MarketResourceSelector"
    TagPrefix="uc12" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Model.SCMDataEntity" %>
<%@ Register Src="Item/BuyerQuery.ascx" TagName="BuyerQuery" TagPrefix="uc7" %>
<%@ Register Src="../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc13" %>
<%@ Register Src="View/ExchangeGoodsList.ascx" TagName="ExchangeGoodsList" TagPrefix="uc14" %>
<%@ Register Src="Item/DeliveryCompanySelector.ascx" TagName="DeliveryCompanySelector"
    TagPrefix="uc15" %>
<%@ Register src="../Common/ClearInputField.ascx" tagname="ClearInputField" tagprefix="uc16" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 換貨作業" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="換貨作業" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="top_table">
    <tr>
        <td>
            <asp:button id="btnAdd" runat="server" text="新增換貨單" class="btn" onclick="btnAdd_Click" />
        </td>
    </tr>
</table>
<div class="border_gray">
    <!--表格 開始-->
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th class="Head_style_a" colspan="2">
                    查詢條件
                </th>
            </tr>
            <tr>
                <th>
                    換貨單號
                </th>
                <td class="tdleft">
                    <asp:textbox id="orderNo" runat="server" text='<%# _item.EXCHANGE_GOODS_NUMBER %>'> </asp:textbox>
                </td>
            </tr>
            <tr>
                <th>
                    訂單號碼
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="buyerOrderNo" runat="server" Text='<%# _item.BUYER_ORDERS.BUYER_ORDERS_NUMBER %>'></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th>
                    BarCode
                </th>
                <td class="tdleft">
                    <asp:textbox id="barcode" runat="server"> </asp:textbox>
                </td>
            </tr>
            <tr>
                <th>
                    料品名稱
                </th>
                <td class="tdleft">
                    <font color="blue">
                        <asp:textbox id="txtName" runat="server"> </asp:textbox>可輸入關鍵字查詢</font>
                </td>
            </tr><tr>
                <th>
                    買受人名稱
                </th>
                <td class="tdleft">
                    <uc7:BuyerQuery ID="buyerQuery" runat="server" />
                </td>
            </tr><tr>
                <th width="20%">
                    購物平台
                </th>
                <td class="tdleft">
                    &nbsp;<uc12:MarketResourceSelector ID="marketResource" runat="server" />
                </td>
            </tr><tr>
                <th width="20%">
                    日期區間
                </th>
                <td class="tdleft">
                    自&nbsp;<uc10:CalendarInputDatePicker ID="orderDateFrom" runat="server" />
                    &nbsp; &nbsp; 至&nbsp;<uc10:CalendarInputDatePicker ID="orderDateTo" runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    結案入庫狀態
                </th>
                <td class="tdleft">
                    &nbsp;<asp:dropdownlist id="inboundStatus" runat="server">
                        <asp:listitem Value="">全部</asp:listitem>
                        <asp:listitem value="1">已結案入庫</asp:listitem>
                        <asp:listitem value="0">未結案入庫</asp:listitem>
                    </asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <th>
                    結案出貨狀態
                </th>
                <td class="tdleft">
                    &nbsp;<asp:dropdownlist id="outboundStatus" runat="server">
                        <asp:listitem Value="">全部</asp:listitem>
                        <asp:listitem value="1">已結案出貨</asp:listitem>
                        <asp:listitem value="0">未結案出貨</asp:listitem>
                    </asp:dropdownlist>
                </td>
            </tr>
 <tr>
                <th>
                    單據刪除狀態
                </th>
                <td class="tdleft">
                    &nbsp;<asp:dropdownlist id="docStatus" runat="server">
                        <asp:listitem Value="">全部</asp:listitem>
                        <asp:listitem value="1204">已刪除</asp:listitem>
                        <asp:listitem Value="">未刪除</asp:listitem>
                    </asp:dropdownlist>
                </td>
            </tr>            
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:button id="btnQuery" runat="server" text="查詢" class="btn" onclick="btnQuery_Click" />
        &nbsp;&nbsp;
            <uc16:ClearInputField ID="ClearInputField1" runat="server" />
        </td>
    </tr>
</table>
<uc6:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
<uc14:ExchangeGoodsList ID="itemList" runat="server" Visible="false" />
<uc8:DataModelContainer ID="modelItem" runat="server" />
<cc1:BuyerShipmentDataSource ID="dsEntity" runat="server">
</cc1:BuyerShipmentDataSource>
<uc13:PageAnchor ID="ToCreateExchangeGoods" runat="server" TransferTo="~/SCM/BuyerOrderQueryForExchangeGoods.aspx" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        marketResource.Selector.DataBound += new EventHandler(market_DataBound);
    }

    void market_DataBound(object sender, EventArgs e)
    {
        marketResource.Selector.Items.Insert(0, new ListItem("全部", ""));
    }

</script>
