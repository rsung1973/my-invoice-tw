<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.DataModel.ItemSelector" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<asp:DropDownList ID="selector" runat="server" DataSourceID="dsEntity" DataTextField="DELIVERY_COMPANY_NAME" DataValueField="DELIVERY_COMPANY_SN" EnableViewState="false" 
    ondatabound="selector_DataBound">
</asp:DropDownList>
<cc1:DeliveryCompanyDataSource ID="dsEntity" 
    runat="server">
</cc1:DeliveryCompanyDataSource>

