<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuyerOrderQueryForShipment.ascx.cs"
    Inherits="eIVOGo.Module.SCM.BuyerOrderQueryForShipment" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc8" %>
<%@ Register Src="View/BuyerOrderListForShipment.ascx" TagName="BuyerOrderListForShipment"
    TagPrefix="uc9" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc10" %>
<%@ Register Src="Item/WarehouseSelector.ascx" TagName="WarehouseSelector" TagPrefix="uc11" %>
<%@ Register Src="Item/MarketResourceSelector.ascx" TagName="MarketResourceSelector"
    TagPrefix="uc12" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Model.SCMDataEntity" %>
<%@ Register Src="Item/BuyerQuery.ascx" TagName="BuyerQuery" TagPrefix="uc7" %>
<%@ Register Src="../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc13" %>
<%@ Register src="../Common/ClearInputField.ascx" tagname="ClearInputField" tagprefix="uc1" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 新增出貨單" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增出貨單" />
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        單據查詢</h2>
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th class="Head_style_a" colspan="2">
                    &nbsp;單據轉入
                </th>
            </tr>
            <tr>
                <th>
                    單據種類
                </th>
                <td class="tdleft">
<asp:RadioButtonList ID="rbChange" runat="server" 
                        RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="True" 
                        onselectedindexchanged="rbChange_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="~/SCM/BuyerOrderShipmentQuery.aspx">訂單</asp:ListItem>
                        <asp:ListItem Value="~/SCM/ExchangeQueryForShipment.aspx">換貨單</asp:ListItem>
                        <asp:ListItem Value="~/SCM/ReturnedQueryForShipment.aspx">採購退貨單</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th>
                    單據號碼
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="orderNo" runat="server" Text='<%# _item.BUYER_ORDERS.BUYER_ORDERS_NUMBER %>'></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    買受人名稱
                </th>
                <td class="tdleft">
                    &nbsp;<uc7:BuyerQuery ID="buyerQuery" runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    BarCode
                </th>
                <td class="tdleft">
                    &nbsp;<asp:TextBox ID="barcode" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    料品名稱
                </th>
                <td class="tdleft">
                    <font color="blue">
                        <asp:TextBox ID="txtName" runat="server" Style="margin-bottom: 0px"></asp:TextBox>可輸入關鍵字查詢</font>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    日期區間
                </th>
                <td class="tdleft">
                    自&nbsp;<uc10:CalendarInputDatePicker ID="orderDateFrom" runat="server" />
                    &nbsp; 至&nbsp;<uc10:CalendarInputDatePicker ID="orderDateTo" runat="server" />
                    &nbsp;
                </td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" runat="server" Text="選擇單據" class="btn" OnClick="btnQuery_Click" />
        </td>
    </tr>
</table>
<uc6:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
<uc9:BuyerOrderListForShipment ID="itemList" runat="server" Visible="false" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tblNext" runat="server"
    visible="false" enableviewstate="false">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnTransfer" runat="server" Text="轉入出貨單" class="btn" OnClick="btnTransfer_Click" />
        &nbsp;&nbsp;
            <uc1:ClearInputField ID="ClearInputField1" runat="server" />
        </td>
    </tr>
</table>
<uc8:DataModelContainer ID="modelItem" runat="server" />
<cc1:BuyerOrdersDataSource ID="dsEntity" runat="server">
</cc1:BuyerOrdersDataSource>
<uc13:PageAnchor ID="ToCreateShipment" runat="server" TransferTo="~/SCM/EditShipment.aspx" />
