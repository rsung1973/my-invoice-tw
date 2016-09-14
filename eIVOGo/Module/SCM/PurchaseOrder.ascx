<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrder.ascx.cs"
    Inherits="eIVOGo.Module.SCM.PurchaseOrder" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register src="Item/ProductQueryByField.ascx" tagname="ProductQueryByField" tagprefix="uc1" %>
<%@ Register Src="Item/SupplierSelectorView.ascx" TagName="SupplierSelectorView" TagPrefix="uc2" %>
<%@ Register Src="Item/WarehouseSelectorView.ascx" TagName="WarehouseSelectorView" TagPrefix="uc3" %>
<%@ Register Src="PODetailsEditList.ascx" TagName="PODetailsEditList" TagPrefix="uc4" %>
<%@ Register src="../Common/DataModelContainer.ascx" tagname="DataModelContainer" tagprefix="uc7" %>


<uc5:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 新增採購單" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="新增採購單" />
<div class="border_gray" id="divPurchaseNo" visible="false" runat="server">
    <!--表格 開始-->
    <h2>採購單</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th width="15%">
                採購單號
            </th>
            <td class="tdleft">
                <asp:Label ID="lblPurchaseNo" runat="server" Text='<%# _item.PURCHASE_ORDER_NUMBER %>'></asp:Label>
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<uc2:SupplierSelectorView ID="supplierView" runat="server" />
<!--按鈕-->

<uc3:WarehouseSelectorView ID="warehouseView" runat="server" />
<div class="border_gray">
    <!--表格 開始-->
    <h2>料品資訊</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="4" class="Head_style_a">
                新增採購料品
            </th>
        </tr>
        <tr>
            <th width="15%">
                料品Barcode
            </th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="txtBarcode" CssClass="textfield" runat="server"></asp:TextBox>
                <asp:Button ID="btnAddProd" CssClass="btn" Text="新增料品" runat="server" OnClick="btnAddProd_Click" />
            </td>
            <th width="15%" class="tdleft">
                料品名稱
            </th>
            <td class="tdleft">
                <uc1:ProductQueryByField ID="productQuery" runat="server" />
            </td>
        </tr>
    </table>
    <uc4:PODetailsEditList ID="poDetails" runat="server" />
    <!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnPOPreview" CssClass="btn" Text="採購單預覽" runat="server" 
                onclick="btnPOPreview_Click" />
        </td>
    </tr>
</table>
<cc1:PurchaseDataSource ID="dsPurchase" runat="server">
</cc1:PurchaseDataSource>
<uc7:DataModelContainer ID="DMContainer" runat="server" />