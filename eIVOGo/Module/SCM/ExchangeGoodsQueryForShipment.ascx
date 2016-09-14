<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeGoodsQueryForShipment.ascx.cs"
    Inherits="eIVOGo.Module.SCM.ExchangeGoodsQueryForShipment" %>
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
<%@ Register Src="View/ExchangeGoodsListForShipment.ascx" TagName="ExchangeGoodsListForShipment" TagPrefix="uc14" %>
<%@ Register Src="Item/DeliveryCompanySelector.ascx" TagName="DeliveryCompanySelector"
    TagPrefix="uc15" %>
<%@ Register src="../Common/ClearInputField.ascx" tagname="ClearInputField" tagprefix="uc16" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 換貨作業" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="換貨作業" />
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
                    單據種類
                </th>
                <td class="tdleft">
<asp:RadioButtonList ID="rbChange" runat="server" 
                        RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="True" 
                        onselectedindexchanged="rbChange_SelectedIndexChanged">
                        <asp:ListItem Value="~/SCM/BuyerOrderShipmentQuery.aspx">訂單</asp:ListItem>
                        <asp:ListItem Selected="True" Value="~/SCM/ExchangeQueryForShipment.aspx">換貨單</asp:ListItem>
                        <asp:ListItem Value="~/SCM/ReturnedQueryForShipment.aspx">採購退貨單</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th>
                    換貨單號
                </th>
                <td class="tdleft">
                    <asp:textbox id="orderNo" runat="server" text=''> </asp:textbox>
                </td>
            </tr>
            <tr>
                <th>
                    訂單號碼
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="buyerOrderNo" runat="server" Text=''></asp:TextBox>
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
<uc14:ExchangeGoodsListForShipment ID="itemList" runat="server" Visible="false" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tblNext" runat="server"
    visible="false" enableviewstate="false">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnTransfer" runat="server" Text="轉入出貨單" class="btn" OnClick="btnTransfer_Click" />
        </td>
    </tr>
</table>
<uc8:DataModelContainer ID="modelItem" runat="server" Suffix=".Exchange" />
<cc1:ExchangeGoodsDataSource ID="dsEntity" runat="server">
</cc1:ExchangeGoodsDataSource>
<uc13:PageAnchor ID="ToCreateShipment" runat="server" TransferTo="~/SCM/EditShipmentFromExchange.aspx" />
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
