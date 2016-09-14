<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PromotionalProjectMaintainAdd.ascx.cs"
    Inherits="eIVOGo.Module.SCM.PromotionalProjectMaintainAdd" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc4" %>
<%@ Register src="../Common/PageAnchor.ascx" tagname="PageAnchor" tagprefix="uc7" %>
<%--<%@ Register src="PromotionalProjectMaintainPODetails.ascx" tagname="PromotionalProjectMaintainPODetails" tagprefix="uc7" %>--%>
<%@ Register src="Item/ProductQuery.ascx" tagname="ProductQuery" tagprefix="uc3" %>
<%@ Register src="Item/ProductQueryByField.ascx" tagname="ProductQueryByField" tagprefix="uc8" %>
<%@ Register src="../Common/DataModelContainer.ascx" tagname="DataModelContainer" tagprefix="uc9" %>
<%@ Register src="Item/PromoDetailsEditList.ascx" tagname="PromoDetailsEditList" tagprefix="uc10" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc2" %>


<!--路徑名稱-->
<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 新增促銷專案" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增促銷專案" />
<div class="border_gray">
    <h2>
        促銷專案</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" id="left_title">
        <tr>
            <th colspan="4" class="Head_style_a">
                新增促銷專案
            </th>
        </tr>
        <tr>
            <th width="15%">
                <span style="color: red">*</span>促銷專案名稱
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtPromotionalName" runat="server" ></asp:TextBox>
            </td>
            <th width="15%">
                專案代號
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtPromotionalNo" runat="server" ></asp:TextBox>
            </td>
        </tr>        
        <tr>
            <th width="15%">
                折扣（百分比）
            </th>
            <td class="tdleft" colspan="3">
                <asp:TextBox ID="txtPromotionalPercent" runat="server" ></asp:TextBox>
                ％
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        料品資訊</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="4" class="Head_style_a">
                新增促銷專案料品
            </th>
        </tr>
        <tr>
            <th width="15%">
                料品Barcode
            </th>
            <td width="35%" class="tdleft">
                &nbsp;<asp:TextBox ID="txtGoodsBarcode" runat="server"></asp:TextBox>
                &nbsp;<asp:Button ID="btnAdd" class="btn" runat="server" Text="新增料品" OnClick="btnAdd_Click" />
            </td>
            <th width="15%" class="tdleft">
                料品名稱
            </th>
            <td class="tdleft">
                <uc8:ProductQueryByField ID="productQueryByName" runat="server" />
            </td>
        </tr>
    </table>
    <uc10:PromoDetailsEditList ID="promoDetails" runat="server" />
    
<!--按鈕-->

</div><table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnOk" class="btn" runat="server" Text="確定" 
                OnClick="btnOk_Click" style="height: 21px" />
        </td>
    </tr>
</table>
<uc9:DataModelContainer ID="modelItem" runat="server" />
<uc7:PageAnchor ID="ToAddSalesPromo" runat="server" 
    TransferTo="~/SCM_SYSTEM/Promotional_Project_Maintain.aspx" />


<cc2:SalesPromotionDataSource ID="dsEntity" runat="server">
</cc2:SalesPromotionDataSource>
<cc2:SalesPromotionDataSource ID="dsUpdate" runat="server" Isolated="True">
</cc2:SalesPromotionDataSource>


