<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarketAttributeSelector.ascx.cs" Inherits="eIVOGo.Module.SCM.Item.MarketAttributeSelector" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<asp:DropDownList ID="selector" runat="server" DataSourceID="dsEntity" DataTextField="MARKET_ATTR_NAME" DataValueField="MARKET_ATTR_SN" EnableViewState="false" 
    ondatabound="selector_DataBound">
</asp:DropDownList>
<cc1:MarketAttributeDataSource ID="dsEntity" 
    runat="server">
</cc1:MarketAttributeDataSource>

