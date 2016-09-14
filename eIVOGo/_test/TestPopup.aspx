<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/_test/MyPopupModal.ascx" TagPrefix="uc1" TagName="PopupModal" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<!DOCTYPE html>
<script runat="server">

    protected void btnTest_Click(object sender, EventArgs e)
    {
        myPopup.Show();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        Response.Redirect("DownloadText.aspx");
    }
    
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div>
        <%= DateTime.Now %>
        <br />
        <asp:Button ID="btnTest" runat="server" Text="Button" OnClick="btnTest_Click" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc1:PopupModal ID="myPopup" runat="server" Visible="false" />
            <asp:Button ID="btnExport" runat="server" Text="Download" OnClick="btnExport_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        myPopup.Done += myPopup_Done;
    }

    void myPopup_Done(object sender, EventArgs e)
    {
        myPopup.Close();
        this.AjaxAlert("Ok...");
    }
</script>
