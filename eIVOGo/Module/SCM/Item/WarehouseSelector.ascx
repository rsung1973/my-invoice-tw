<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.DataModel.ItemSelector" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<asp:DropDownList ID="selector" runat="server" DataSourceID="dsEntity" DataTextField="WAREHOUSE_NAME"
    DataValueField="WAREHOUSE_SN" EnableViewState="false" OnDataBound="selector_DataBound">
</asp:DropDownList>
<cc1:WarehouseDataSource ID="dsEntity" runat="server">
</cc1:WarehouseDataSource>
