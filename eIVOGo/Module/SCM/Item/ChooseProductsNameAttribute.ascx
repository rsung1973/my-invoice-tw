<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChooseProductsNameAttribute.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Item.ChooseProductsNameAttribute" %>
<%@ Register Src="../../SCM_SYS/Item/ProductsAttributeNameSelector.ascx" TagName="ProductsAttributeNameSelector"
    TagPrefix="uc1" %>
<%@ Register Src="../../UI/PopupModal.ascx" TagName="PopupModal" TagPrefix="uc2" %>
<%@ Register Src="../../SCM_SYS/Item/ProductsAttributeNameItem.ascx" TagName="ProductsAttributeNameItem"
    TagPrefix="uc3" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <!--路徑名稱-->
        <!--交易畫面標題-->
        <uc1:ProductsAttributeNameSelector ID="attrSN" runat="server" />
        <asp:Button ID="btnAddNew" runat="server" Text="新增屬性名稱" 
            OnClick="btnAddNew_Click" CausesValidation="False" />
        <!--按鈕-->
    </ContentTemplate>
</asp:UpdatePanel>
<uc3:ProductsAttributeNameItem ID="addItem" runat="server" />
