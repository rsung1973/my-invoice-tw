<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        Response.Clear();
        Response.ContentType = "message/rfc822";
        Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}({1:yyyy-MM-dd}).txt", Server.UrlEncode("測試文件"), DateTime.Today));
        Response.HeaderEncoding = Encoding.GetEncoding(950);

        using (System.IO.StreamWriter Output = new System.IO.StreamWriter(Response.OutputStream, Encoding.GetEncoding(950)))
        {
            Output.WriteLine("測試文件");
        }


        Response.Flush();
        Response.End();
    }
</script>
