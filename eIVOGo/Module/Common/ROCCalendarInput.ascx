<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.Common.ROCCalendarInput" %>
<asp:TextBox ID="txtDate" CssClass="textfield" runat="server" Columns="10" />
<img runat="server" id="imgCalendar" src="~/images/date.gif" width="15" height="15"
    align="absMiddle" style="cursor: pointer" />
<asp:Label ID="errorMsg" runat="server" Text="請選擇日期!!" Visible="false" EnableViewState="false"
    ForeColor="Red"></asp:Label>