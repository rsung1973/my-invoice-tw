<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProdItemSelect.ascx.cs"
    Inherits="eIVOGo.Module.SCM.ProdItemSelect" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="PupProdNameSelect.ascx" TagName="PupProdNameSelect" TagPrefix="uc2" %>
<%@ Register Src="Item/ProductQuery.ascx" TagName="ProductQuery" TagPrefix="uc3" %>
<%@ Register src="Item/ProductQueryByField.ascx" tagname="ProductQueryByField" tagprefix="uc1" %>
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
<!--表格 結束-->
<cc1:PurchaseDataSource ID="dsPurchase" runat="server">
</cc1:PurchaseDataSource>
