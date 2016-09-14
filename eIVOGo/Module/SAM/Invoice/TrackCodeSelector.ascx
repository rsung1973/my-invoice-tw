<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackCodeSelector.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Invoice.TrackCodeSelector" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<asp:DropDownList ID="selector" runat="server" EnableViewState="False" DataTextField="TrackCode" DataValueField="TrackID"
    ondatabound="selector_DataBound">
</asp:DropDownList>
<cc1:InvoiceTrackCodeDataSource ID="dsEntity" runat="server">
</cc1:InvoiceTrackCodeDataSource>


