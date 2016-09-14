<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductsAttributeNameSelector.ascx.cs" Inherits="eIVOGo.Module.SCM_SYS.Item.ProductsAttributeNameSelector" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<asp:DropDownList ID="selector" runat="server" DataSourceID="dsEntity" DataTextField="PRODUCTS_ATTR_NAME" DataValueField="PRODUCTS_ATTR_NAME_SN" EnableViewState="false" 
    ondatabound="selector_DataBound">
</asp:DropDownList>
<cc1:ProductsAttributeNameDataSource ID="dsEntity" 
    runat="server">
</cc1:ProductsAttributeNameDataSource>

