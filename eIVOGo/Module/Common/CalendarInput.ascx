<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.Common.CalendarInput" %>
<asp:TextBox id="txtDate" CssClass="textfield" runat="server" Columns="10" 
    ReadOnly="True" />
<span class="contant"><img runat="server" id="imgCalendar" src="~/images/date.gif" width="15" height="15"
		align="absMiddle" style="CURSOR:pointer"/>
    <asp:Label ID="errorMsg" runat="server" Text="請選擇日期!!" Visible="false" EnableViewState="false" ForeColor="Red"></asp:Label>
