<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="POReturnedQueryForShipment.ascx.cs"
    Inherits="eIVOGo.Module.SCM.POReturnedQueryForShipment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc4" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc4" %>
<%@ Register Src="../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc8" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc10" %>
<%@ Register Src="Item/WarehouseSelector.ascx" TagName="WarehouseSelector" TagPrefix="uc11" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Model.SCMDataEntity" %>
<%@ Register Src="../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc13" %>
<%@ Register Src="View/PurchaseOrderReturnedListForShipment.ascx" TagName="PurchaseOrderReturnedListForShipment"
    TagPrefix="uc14" %>
<%@ Register Src="../Common/ClearInputField.ascx" TagName="ClearInputField" TagPrefix="uc16" %>
<%@ Register Src="Item/SupplierSelector.ascx" TagName="SupplierSelector" TagPrefix="uc1" %>


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
                    <asp:RadioButtonList ID="rbChange" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                        AutoPostBack="True" OnSelectedIndexChanged="rbChange_SelectedIndexChanged">
                        <asp:ListItem Value="~/SCM/BuyerOrderShipmentQuery.aspx">訂單</asp:ListItem>
                        <asp:ListItem Value="~/SCM/ExchangeQueryForShipment.aspx">換貨單</asp:ListItem>
                        <asp:ListItem Selected="True" Value="~/SCM/ReturnedQueryForShipment.aspx">採購退貨單</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th>
                    退貨倉儲
                </th>
                <td class="tdleft">
                    <uc11:WarehouseSelector ID="warehouse" runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    供應商
                </th>
                <td class="tdleft">
                    <uc1:SupplierSelector ID="supplier" runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    採購退貨單號碼
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="orderNo" runat="server" Text=''> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    BarCode
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="barcode" runat="server"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    料品名稱
                </th>
                <td class="tdleft">
                    <font color="blue">
                        <asp:TextBox ID="txtName" runat="server"> </asp:TextBox>可輸入關鍵字查詢</font>
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
            &nbsp;&nbsp;
            <uc16:ClearInputField ID="ClearInputField1" runat="server" />
        </td>
    </tr>
</table>
<uc6:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
<uc14:PurchaseOrderReturnedListForShipment ID="itemList" runat="server" Visible="false" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tblNext" runat="server"
    visible="false" enableviewstate="false">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnTransfer" runat="server" Text="轉入出貨單" class="btn" OnClick="btnTransfer_Click" />
        </td>
    </tr>
</table>
<uc8:DataModelContainer ID="modelItem" runat="server" />
<cc1:PurchaseOrderReturnDataSource ID="dsEntity" runat="server">
</cc1:PurchaseOrderReturnDataSource>
<uc13:PageAnchor ID="ToCreateShipment" runat="server" TransferTo="~/SCM/EditShipmentFromReturnedPO.aspx" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        warehouse.Selector.DataBound += new EventHandler(warehouse_DataBound);
        supplier.Selector.DataBound += new EventHandler(supplier_DataBound);
    }

    void warehouse_DataBound(object sender, EventArgs e)
    {
        warehouse.Selector.Items.Insert(0, new ListItem("全部", ""));
    }

    void supplier_DataBound(object sender, EventArgs e)
    {
        supplier.Selector.Items.Insert(0, new ListItem("全部", ""));
    }

</script>
