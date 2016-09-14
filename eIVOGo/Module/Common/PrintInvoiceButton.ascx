<%@ Control Language="c#" Inherits="Uxnet.Web.Module.Common.PrintingButton2" %>
<asp:button id="_btnPrintIt" runat="server" Text="¦C¦Lµo²¼" CssClass="btn" onclick="_btnPrintIt_Click" />
<iframe id="prnFrame" name="prnFrame" width="100%" height="2048" runat="server"></iframe>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.ContentGeneratorUrl = "~/Published/PrintPage.aspx";
    }
</script>
