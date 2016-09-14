<%@ Control Language="C#" AutoEventWireup="true"  %>
<input id="printBack" name="printBack" type="checkbox" value="True" <%# String.IsNullOrEmpty(Request["printBack"])?"":"checked=\"checked\""  %> />