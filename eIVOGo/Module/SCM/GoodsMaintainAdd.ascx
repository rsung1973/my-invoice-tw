<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoodsMaintainAdd.ascx.cs" Inherits="eIVOGo.Module.SCM.GoodsMaintainAdd" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc4" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc4" %>
<%@ Register Src="Item/ChooseProductsNameAttribute.ascx" TagName="ChooseProductsNameAttribute" TagPrefix="uc7" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc2" %>
<%@ Register Src="Item/ProductsAttributeMappingList.ascx" TagName="ProductsAttributeMappingList" TagPrefix="uc8" %>
<%@ Register Src="SupplierProductsList.ascx" TagName="SupplierProductsList" TagPrefix="uc9" %>
<%@ Register src="Item/SupplierSelector.ascx" tagname="SupplierSelector" tagprefix="uc10" %>
<%@ Register src="Item/ProductAttributeEditList.ascx" tagname="ProductAttributeEditList" tagprefix="uc11" %>
<%@ Register src="../Common/DataModelContainer.ascx" tagname="DataModelContainer" tagprefix="uc12" %>



<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 >  料品資料維護" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="料品資料維護" />
<div id="border_gray">
    <h2>
        料品資訊</h2>
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" id="left_title">
        <tr>
            <th width="15%">
                <span style="color: red">*</span>料品Barcode：
            </th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="PRODUCTS_BARCODE" Text='<%# DataBinder.Eval(_item,"PRODUCTS_BARCODE") %>'
                    runat="server"></asp:TextBox>
            </td>
            <th width="15%" class="tdleft">
                <span style="color: red">*</span>料品名稱：
            </th>
            <td class="tdleft">
                <asp:TextBox ID="PRODUCTS_NAME" Text='<%# DataBinder.Eval(_item,"PRODUCTS_NAME") %>'
                    runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            
            <th width="15%">
                料品進價：
            </th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="SELL_PRICE" Text='<%# DataBinder.Eval(_item,"SELL_PRICE") %>' runat="server"></asp:TextBox>
            </td>
            <th width="15%" class="tdleft">
                料品售價：
            </th>
            <td class="tdleft">
                <asp:TextBox ID="BUY_PRICE" Text='<%# DataBinder.Eval(_item,"BUY_PRICE") %>' runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <!--表格 結束-->
    <!--按鈕-->
</div>
<uc11:ProductAttributeEditList ID="productAttribute" runat="server" />
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        供應商貨號設定</h2>
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th width="15%">
                    供應商
                </th>
                <td class="tdleft" width="35%">
                    <uc10:SupplierSelector ID="supplier" runat="server" />
                </td>
                <th class="tdleft" width="15%">
                    <span style="color: red">*</span>供應商貨號
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="supplierProductNumber" runat="server" Columns="40"></asp:TextBox>
&nbsp;</td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnOk" class="btn" runat="server" Text="確定" OnClick="btnOk_Click" />&nbsp;&nbsp;
            <input name="Reset" type="reset" class="btn" value="重填" />
        </td>
    </tr>
</table>
<cc2:ProductsDataSource ID="dsEntity" runat="server">
</cc2:ProductsDataSource>
<uc12:DataModelContainer ID="modelItem" runat="server" />

