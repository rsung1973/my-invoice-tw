<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductsDataSelector.ascx.cs" Inherits="eIVOGo.Module.SCM.Item.ProductsDataSelector" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<asp:CheckBoxList ID="selector" runat="server" DataSourceID="dsEntity"
    DataTextField="PRODUCTS_NAME" DataValueField="PRODUCTS_SN" 
    EnableViewState="False" RepeatLayout="Flow"></asp:CheckBoxList>
<asp:Label ID="emptyMsg" runat="server" Text="資料不存在!!" ForeColor="Red" Visible="false" EnableViewState="false"></asp:Label>
<cc1:ProductsDataSource ID="dsEntity" runat="server">
</cc1:ProductsDataSource>

