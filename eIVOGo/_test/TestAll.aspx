<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/_test/MyPopupModal.ascx" TagPrefix="uc1" TagName="PopupModal" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Import Namespace="Utility" %>
<%@ Register src="~/Module/Inquiry/InquiryItem/UrlRadioDirective.ascx" tagname="UrlRadioDirective" tagprefix="uc2" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
        <uc2:UrlRadioDirective ID="urlGo" runat="server" EnableViewState="false" DefaultName="login" />
    <div>
        <%= DateTime.Now %>
        <br />
        <%= _data %>
        <% if(_data=="hello") {  %>
        <%# System.Threading.Thread.CurrentThread %>
        <% } %>
    </div>
    </form>
</body>
</html>
<script runat="server">
    String _data;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += _test_testall_aspx_Load;
        this.PreRender += _test_testall_aspx_PreRender;

        urlGo.NamingDirection = new String[] { 
            "TestAll", "~/_test/TestAll.aspx",
            "login", "~/login.aspx"};

        Page.ClientScript.RegisterStartupScript(this.GetType(), "initialScript", "");
        
    }

    void _test_testall_aspx_PreRender(object sender, EventArgs e)
    {
        _data = "Hello,Testing...";
        this.DataBind();
    }

    void _test_testall_aspx_Load(object sender, EventArgs e)
    {
        this.AjaxAlert(Page.ClientScript.IsStartupScriptRegistered(this.GetType(), "initialScript").ToString());
    }

</script>
