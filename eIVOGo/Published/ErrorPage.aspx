<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="Visitor" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<div class="border_gray">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                系統發生錯誤
            </th>
        </tr>
        <tr>
            <th>
                原因
            </th>
            <td class="tdleft">
                <asp:Literal ID="msg" runat="server" EnableViewState="false"></asp:Literal>
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="Bargain_btn"><input type="button" value="回上頁" onclick="window.history.go(-1);" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>

    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += new EventHandler(published_errorpage_aspx_Load);
    }

    void published_errorpage_aspx_Load(object sender, EventArgs e)
    {
        Exception ex = Page.PreviousPage.Items["error"] as Exception;
        if (ex != null)
        {
            msg.Text = ex.Message;
        }
    }
</script>