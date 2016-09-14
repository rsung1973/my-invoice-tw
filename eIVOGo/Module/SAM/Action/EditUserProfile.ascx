<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.template.ContentControlTemplate" %>
<%@ Register src="~/Module/UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="~/Module/UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Register src="../../Entity/UserProfileItem.ascx" tagname="UserProfileItem" tagprefix="uc3" %>


<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 會員管理" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="會員管理-編輯帳號" />

<uc3:UserProfileItem ID="editItem" runat="server" />


<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        editItem.BindData();
        editItem.Done += new EventHandler(editItem_Done);
    }

    void editItem_Done(object sender, EventArgs e)
    {
        this.AjaxAlertAndRedirect("作業完成!!", VirtualPathUtility.ToAbsolute("~/SAM/UserProfileManager.aspx"));
    }
</script>


