<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuyerOrderQuery.ascx.cs"
    Inherits="eIVOGo.Module.SCM.BuyerOrderQuery" %>
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
<%@ Register src="../Common/ClearInputField.ascx" tagname="ClearInputField" tagprefix="uc14" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 訂單作業" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="訂單作業" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="top_table">
    <tr>
        <td>
            <asp:Button ID="btnAdd" runat="server" Text="新增訂單" class="btn" OnClick="btnAdd_Click" />
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
                    訂單號碼
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="orderNo" runat="server" Text='<%# _item.BUYER_ORDERS_NUMBER %>'></asp:TextBox>
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
            <tr>
                <th>
                    出貨倉儲
                </th>
                <td class="tdleft">
                    &nbsp;<uc11:WarehouseSelector ID="warehouse" runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    出貨狀態
                </th>
                <td class="tdleft">
                    <asp:RadioButtonList ID="rbShipment" runat="server" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                        <asp:ListItem>全部</asp:ListItem>
                        <asp:ListItem Value="0">未出貨</asp:ListItem>
                        <asp:ListItem Value="1">已出貨</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th>
                    退、換貨狀態
                </th>
                <td class="tdleft">
                    <asp:RadioButtonList ID="rbReturn" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem>全部</asp:ListItem>
                        <asp:ListItem Value="0">未退、換貨</asp:ListItem>
                        <asp:ListItem Value="1">已退貨</asp:ListItem>
                        <asp:ListItem Value="2">已換貨</asp:ListItem>
                    </asp:RadioButtonList>
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
            <asp:Repeater ID="rpAttr" runat="server" Visible="false" EnableViewState="false">
                <ItemTemplate>
                    <tr>
                        <th>
                            <%# ((MARKET_ATTRIBUTE)Container.DataItem).MARKET_ATTR_NAME %>
                        </th>
                        <td class="tdleft">
                            <input class="textfield" name='<%# String.Format("MARKET_ATTR_SN${0}",((MARKET_ATTRIBUTE)Container.DataItem).MARKET_ATTR_SN) %>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" runat="server" Text="查詢" class="btn" OnClick="btnQuery_Click" />
        &nbsp;
            <uc14:ClearInputField ID="ClearInputField1" runat="server" />
        </td>
    </tr>
</table>
<uc6:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
<uc9:BuyerOrderList ID="itemList" runat="server" Visible="false" />
<uc8:DataModelContainer ID="modelItem" runat="server" />
<cc1:BuyerDataSource ID="dsEntity" runat="server">
</cc1:BuyerDataSource>
<uc13:PageAnchor ID="ToCreateBuyerOrder" runat="server" RedirectTo="~/SCM/EditBuyerOrder.aspx" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        warehouse.Selector.DataBound += new EventHandler(warehouse_DataBound);
        marketResource.Selector.DataBound += new EventHandler(market_DataBound);
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
