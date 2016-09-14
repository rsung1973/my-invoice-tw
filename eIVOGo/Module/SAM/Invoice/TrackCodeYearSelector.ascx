<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackCodeYearSelector.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Invoice.TrackCodeYearSelector" %>
<asp:DropDownList ID="selector" runat="server" EnableViewState="false">
</asp:DropDownList>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (!this.IsPostBack)
        {
            selector.SelectedValue = DateTime.Now.Year.ToString();
        }
    }
</script>