<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShipmentQuery.ascx.cs"
    Inherits="eIVOGo.Module.SCM.ShipmentQuery" %>
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
<%@ Register Src="View/ShipmentList.ascx" TagName="ShipmentList" TagPrefix="uc14" %>
<%@ Register src="Item/DeliveryCompanySelector.ascx" tagname="DeliveryCompanySelector" tagprefix="uc15" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 出貨作業" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="出貨作業" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="top_table">
    <tr>
        <td>
            <asp:Button ID="btnAdd" runat="server" Text="新增出貨單" class="btn" OnClick="btnAdd_Click" />
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
                    轉入單據種類
                </th>
                <td class="tdleft">
                    &nbsp;<asp:DropDownList ID="orderType" runat="server">
                        <asp:ListItem>全部</asp:ListItem>
                        <asp:ListItem Value="103">訂單</asp:ListItem>
                        <asp:ListItem Value="104">換貨單</asp:ListItem>
                        <asp:ListItem Value="102">採購退貨單</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    出貨倉儲
                </th>
                <td class="tdleft">
                    &nbsp;<uc11:WarehouseSelector ID="warehouse" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="20%">
                    購物平台
                </th>
                <td class="tdleft">
                    &nbsp;<uc12:MarketResourceSelector ID="marketResource" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="20%">
                    宅配公司
                </th>
                <td class="tdleft">
                   <uc15:DeliveryCompanySelector ID="delivery" 
                        runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    出貨單號
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="orderNo" runat="server" Text='<%# _item.BUYER_SHIPMENT.BUYER_SHIPMENT_NUMBER %>'></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th>
                    BarCode
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="barcode" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    料品名稱
                </th>
                <td class="tdleft">
                    <font color="blue">
                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>可輸入關鍵字查詢</font>
                </td>
            </tr>
            <tr>
                <th>
                    買受人名稱
                </th>
                <td class="tdleft">
                    <uc7:BuyerQuery ID="buyerQuery" runat="server" />
                </td>
            </tr>
            <tr>
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
            <asp:Button ID="btnQuery" runat="server" Text="查詢" class="btn" OnClick="btnQuery_Click" />
        </td>
    </tr>
</table>
<uc6:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
<uc14:ShipmentList ID="itemList" runat="server" Visible="false" />
<uc8:DataModelContainer ID="modelItem" runat="server" />
<cc1:BuyerShipmentDataSource ID="dsEntity" runat="server">
</cc1:BuyerShipmentDataSource>
<uc13:PageAnchor ID="ToCreateShipment" runat="server" TransferTo="~/SCM/BuyerOrderShipmentQuery.aspx" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        warehouse.Selector.DataBound += new EventHandler(warehouse_DataBound);
        marketResource.Selector.DataBound += new EventHandler(market_DataBound);
        delivery.Selector.DataBound += new EventHandler(Selector_DataBound);
    }

    void Selector_DataBound(object sender, EventArgs e)
    {
        delivery.Selector.Items.Insert(0, new ListItem("全部", ""));
    }

    void warehouse_DataBound(object sender, EventArgs e)
    {
        warehouse.Selector.Items.Insert(0, new ListItem("全部", ""));
    }

    void market_DataBound(object sender, EventArgs e)
    {
        marketResource.Selector.Items.Insert(0, new ListItem("全部", ""));
    }

</script>
