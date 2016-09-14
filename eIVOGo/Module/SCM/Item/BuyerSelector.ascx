<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuyerSelector.ascx.cs" Inherits="eIVOGo.Module.SCM.Item.BuyerSelector" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<asp:RadioButtonList ID="selector" runat="server" DataSourceID="dsEntity" 
    DataTextField="BUYER_NAME" DataValueField="BUYER_SN" EnableViewState="False" 
    ondatabound="selector_DataBound" RepeatLayout="Flow">
</asp:RadioButtonList>
<cc1:BuyerDataSource ID="dsEntity" runat="server">
</cc1:BuyerDataSource>

