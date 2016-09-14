<%@ Control Language="c#" Inherits="Uxnet.Web.Module.Common.OpenPrintingButton" Codebehind="OpenPrintingButton.ascx.cs" %>
<asp:button id="_btnPrintIt" runat="server" Text="¦C¦Lµo²¼" onclick="_btnPrintIt_Click" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.UrlToPrint = "~/Published/PrintPage.aspx";
    }
</script>
