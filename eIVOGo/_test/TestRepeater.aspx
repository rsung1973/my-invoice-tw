<%@ Page Language="C#" AutoEventWireup="true" %>
<asp:repeater id="rpList" runat="server" EnableViewState="false">
    <ItemTemplate>
        Index:<%# Container.ItemIndex%>
    </ItemTemplate>
</asp:repeater>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += _test_testrepeater_aspx_PreRender;
    }

    void _test_testrepeater_aspx_PreRender(object sender, EventArgs e)
    {
        rpList.DataSource = Enumerable.Range(0, 20);
        rpList.DataBind();
    }
</script>