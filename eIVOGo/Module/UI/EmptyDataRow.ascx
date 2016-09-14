<%@ Control Language="C#" AutoEventWireup="true" %>
<center>
    <font color="red">查無資料!! </font>
</center>
<script runat="server">
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        if (this.Visible)
        {
            GridView control = this.NamingContainer.NamingContainer as GridView;
            if (control != null)
                control.CssClass = "";
        }
    }
</script>
