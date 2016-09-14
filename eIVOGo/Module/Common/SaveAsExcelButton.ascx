<%@ Control Language="c#" Inherits="Uxnet.Web.Module.Common.DownloadingButton2" %>
<asp:button id="_btnDownload" runat="server" Text="Csv¤U¸ü" onclick="_btnDownload_Click"></asp:button>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.ContentGeneratorUrl = "~/Published/ContentPage.aspx";
    }

    public String clientClick
    {
        set { this._btnDownload.OnClientClick = value; } 
    }
</script>
