<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaintainGoodsReceiptFromPO.ascx.cs"
    Inherits="eIVOGo.Module.SCM.MaintainGoodsReceiptFromPO" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc4" %>
<%@ Register Src="WarehouseList.ascx" TagName="WarehouseList" TagPrefix="uc7" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc8" %>
<%@ Register Src="Item/SupplierSelector.ascx" TagName="SupplierSelector" TagPrefix="uc9" %>
<%@ Register Src="View/PurchaseOrderList.ascx" TagName="PurchaseOrderList" TagPrefix="uc10" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc2" %>
<%@ Register Src="../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc11" %>
<%@ Register Src="../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc12" %>
<%@ Register Src="Item/WarehouseSelector.ascx" TagName="WarehouseSelector" TagPrefix="uc13" %>
<%@ Register src="../Common/ClearInputField.ascx" tagname="ClearInputField" tagprefix="uc14" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 新增入庫單" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增入庫單" />
<div id="border_gray">
    <!--表格 開始-->
    <h2>
        入庫單查詢</h2>
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th class="Head_style_a" colspan="2">
                    單據轉入
                </th>
            </tr>
            <tr>
                <th>
                    單據種類
                </th>
                <td class="tdleft">
                    <asp:RadioButtonList ID="rbChange" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                        AutoPostBack="True" OnSelectedIndexChanged="rbChange_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="~/SCM/WarehouseWarrantManagementFromPO.aspx">採購單</asp:ListItem>
                        <asp:ListItem Value="~/SCM/WarehouseWarrantManagementFromExchange.aspx">換貨單</asp:ListItem>
                        <asp:ListItem Value="~/SCM/WarehouseWarrantManagementFromReturn.aspx">退貨單</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th>
                    單據號碼
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="PONo" runat="server"></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th>
                    入庫倉儲
                </th>
                <td class="tdleft">
                    <uc13:WarehouseSelector ID="warehouse" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="20%">
                    供應商
                </th>
                <td class="tdleft">
                    &nbsp;<uc9:SupplierSelector ID="SupplierSN" runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    供應商貨號
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="ProductNo" runat="server"></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th>
                    BarCode
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="Barcode" runat="server"></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th>
                    料品名稱
                </th>
                <td class="tdleft">
                    <font color="blue">
                        <asp:TextBox ID="ProductName" runat="server"></asp:TextBox>
                        可輸入關鍵字查詢</font>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    日期區間
                </th>
                <td class="tdleft">
                    自&nbsp;<uc8:CalendarInputDatePicker ID="PODateFrom" runat="server" />
                    &nbsp;&nbsp; 至&nbsp;<uc8:CalendarInputDatePicker ID="PODateTo" runat="server" />
                    &nbsp;&nbsp;
                </td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
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
<uc10:PurchaseOrderList ID="itemList" runat="server" Visible="false" />
<table border="0" cellspacing="0" cellpadding="0" width="100%" id="tblNext" runat="server" enableviewstate="false" visible="false">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnTransfer" runat="server" Text="轉入入庫單" class="btn" OnClick="btnTransfer_Click"  />
            &nbsp;&nbsp;
                <uc14:ClearInputField ID="ClearInputField1" runat="server" />
            </td>
        </tr>
    </tbody>
</table>
<cc2:WarehouseWarrantDataSource ID="dsWW" runat="server">
</cc2:WarehouseWarrantDataSource>
<uc11:PageAnchor ID="ToCreateReceipt" runat="server" TransferTo="~/SCM/EditGoodsReceipt.aspx" />
<uc12:DataModelContainer ID="modelItem" runat="server" />
