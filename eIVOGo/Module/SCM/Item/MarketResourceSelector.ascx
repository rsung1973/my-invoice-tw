<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.DataModel.ItemSelector" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<asp:DropDownList ID="selector" runat="server" DataSourceID="dsEntity" DataTextField="MARKET_RESOURCE_NAME" DataValueField="MARKET_RESOURCE_SN" EnableViewState="false" 
    ondatabound="selector_DataBound">
</asp:DropDownList>
<cc1:MarketResourceDataSource ID="dsEntity" 
    runat="server">
</cc1:MarketResourceDataSource>

