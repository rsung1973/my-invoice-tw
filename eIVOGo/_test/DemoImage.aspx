<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>Hello,Test image inline page  within email...</div>
        <img src="content/Hydrangeas.jpg" /><br />
        <img src="content/Tulips.jpg" />
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += _test_demoimage_aspx_PreRender;
    }

    void _test_demoimage_aspx_PreRender(object sender, EventArgs e)
    {
        
    }

    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }
</script>
