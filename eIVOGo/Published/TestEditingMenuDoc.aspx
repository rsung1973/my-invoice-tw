<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="Visitor" %>
<%@ Register src="../Module/SYS/MaintainMenuNodes.ascx" tagname="MaintainMenuNodes" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:MaintainMenuNodes ID="menuNodes" runat="server" />
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += new EventHandler(published_testeditingmenudoc_aspx_Load);
    }

    void published_testeditingmenudoc_aspx_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && Page.PreviousPage != null && Page.PreviousPage.Items["menuPath"] != null)
        {
            menuNodes.MenuPath = Page.PreviousPage.Items["menuPath"] as String;
        }
    }
</script>