<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierSelector.ascx.cs" Inherits="eIVOGo.Module.SCM.Item.SupplierSelector" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<asp:DropDownList ID="selector" runat="server" EnableViewState="false" DataSourceID="dsEntity"
    ondatabound="selector_DataBound" >
</asp:DropDownList>
<cc1:SupplierDataSource ID="dsEntity" 
    runat="server">
</cc1:SupplierDataSource>

