<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.template.ContentControlTemplate" %>
<%@ Register src="~/Module/UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="~/Module/UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>

<%@ Register src="UserRoleList.ascx" tagname="UserRoleList" tagprefix="uc3" %>



<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 系統維護" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="使用者角色維護" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc3:UserRoleList ID="UserRoleList1" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += new EventHandler(module_sys_maintainuserrole_ascx_Load);
    }

    void module_sys_maintainuserrole_ascx_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && Page.PreviousPage != null && Page.PreviousPage.Items["uid"] != null)
        {
            UserRoleList1.UID = int.Parse((String)Page.PreviousPage.Items["uid"]);
        }
        
    }
</script>
