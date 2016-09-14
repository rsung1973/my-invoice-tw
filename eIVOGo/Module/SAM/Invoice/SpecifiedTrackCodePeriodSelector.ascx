<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpecifiedTrackCodePeriodSelector.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Invoice.SpecifiedTrackCodePeriodSelector" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<asp:DropDownList ID="selector" runat="server" EnableViewState="False" DataTextField="Expression" DataValueField="PeriodNo"
    ondatabound="selector_DataBound">
</asp:DropDownList>
<cc1:InvoiceTrackCodeDataSource ID="dsEntity" runat="server">
</cc1:InvoiceTrackCodeDataSource>


