<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicAddTextbox.ascx.cs" Inherits="eIVOGo.Module.UI.DynamicAddTextbox" %>
<asp:HiddenField ID="HiddenField1" runat="server" />
<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
<asp:Button ID="btnAdd" runat="server" Text="新增欄位" CssClass="btn" onclick="btnAdd_Click" />
