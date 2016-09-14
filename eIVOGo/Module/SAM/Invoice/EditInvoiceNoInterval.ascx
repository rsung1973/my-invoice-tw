<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.template.ContentControlTemplate" %>
<%@ Register src="~/Module/UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="~/Module/UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>
<%@ Register src="EditInvoiceNoIntervalItem.ascx" tagname="EditInvoiceNoIntervalItem" tagprefix="uc3" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>


<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 修改發票號碼" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="修改發票號碼" />
<uc3:EditInvoiceNoIntervalItem ID="intervalItem" runat="server" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        intervalItem.Done += new EventHandler(intervalItem_Done);
        intervalItem.BindData();
    }

    void intervalItem_Done(object sender, EventArgs e)
    {
        this.AjaxAlert("發票號碼區段設定完成!!");
    }
</script>


