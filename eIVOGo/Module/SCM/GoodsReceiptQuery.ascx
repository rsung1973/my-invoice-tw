<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoodsReceiptQuery.ascx.cs"
    Inherits="eIVOGo.Module.SCM.GoodsReceiptQuery" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc4" %>
<%@ Register Src="../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc8" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc10" %>
<%@ Register Src="Item/WarehouseSelector.ascx" TagName="WarehouseSelector" TagPrefix="uc11" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Model.SCMDataEntity" %>
<%@ Register Src="../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc13" %>
<%@ Register Src="View/GoodsReceiptList.ascx" TagName="GoodsReceiptList" TagPrefix="uc14" %>
<%@ Register Src="Item/SupplierSelector.ascx" TagName="SupplierSelector" TagPrefix="uc16" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 入庫作業" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="入庫作業" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="top_table">
    <tr>
        <td>
            <asp:Button ID="btnAdd" runat="server" Text="新增入庫單" class="btn" OnClick="btnAdd_Click" />
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
                        <asp:ListItem Value="100">採購單</asp:ListItem>
                        <asp:ListItem Value="104">退、換貨單</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    入庫倉儲
                </th>
                <td class="tdleft">
                    &nbsp;<uc11:WarehouseSelector ID="warehouse" runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    入庫單號碼
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="orderNo" runat="server" Text='<%# _item.WAREHOUSE_WARRANT_NUMBER %>'></asp:TextBox>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th>
                    供應商
                </th>
                <td class="tdleft">
                    &nbsp;
                    <uc16:SupplierSelector ID="supplierSN" runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    供應商貨號
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="supplierProductNo" runat="server"></asp:TextBox>
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
                <th width="20%">
                    日期區間
                </th>
                <td class="tdleft">
                    自&nbsp;<uc10:CalendarInputDatePicker ID="orderDateFrom" runat="server" />
                    &nbsp; &nbsp; 至&nbsp;<uc10:CalendarInputDatePicker ID="orderDateTo" runat="server" />
                </td>
            </tr>
            <tr>
                <th width="20%">
                    單據刪除狀態
                </th>
                <td class="tdleft">
                    <asp:RadioButtonList ID="docStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="1204">已刪除</asp:ListItem>
                        <asp:ListItem Value="">未刪除</asp:ListItem>
                    </asp:RadioButtonList>
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
<uc14:GoodsReceiptList ID="itemList" runat="server" Visible="false" />
<uc8:DataModelContainer ID="modelItem" runat="server" />
<cc1:WarehouseWarrantDataSource ID="dsEntity" runat="server">
</cc1:WarehouseWarrantDataSource>
<uc13:PageAnchor ID="ToCreateWarrant" runat="server" TransferTo="~/SCM/WarehouseWarrantManagementFromPO.aspx" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        warehouse.Selector.DataBound += new EventHandler(warehouse_DataBound);
        supplierSN.Selector.DataBound += new EventHandler(Selector_DataBound);
    }

    void Selector_DataBound(object sender, EventArgs e)
    {
        supplierSN.Selector.Items.Insert(0, new ListItem("全部", ""));
    }

    void warehouse_DataBound(object sender, EventArgs e)
    {
        warehouse.Selector.Items.Insert(0, new ListItem("全部", ""));
        this.warehouse.Selector.SelectedIndex = 0;
    }


</script>
