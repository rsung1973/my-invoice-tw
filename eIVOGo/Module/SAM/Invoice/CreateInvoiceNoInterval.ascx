<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.template.ContentControlTemplate" %>
<%@ Register src="~/Module/UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="~/Module/UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>
<%@ Register src="InvoiceNoIntervalItem.ascx" tagname="InvoiceNoIntervalItem" tagprefix="uc3" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Register src="~/Module/Common/CrossPageMessage.ascx" tagname="CrossPageMessage" tagprefix="uc4" %>


<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 新增發票號碼" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增發票號碼" />
<uc3:InvoiceNoIntervalItem ID="intervalItem" runat="server" />
<uc4:CrossPageMessage ID="pageMsg" runat="server" />

<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        intervalItem.Done += new EventHandler(intervalItem_Done);
    }

    void intervalItem_Done(object sender, EventArgs e)
    {
        pageMsg.Message = "發票號碼區段設定完成!!";
        Server.Transfer("~/SAM/MaintainInvoiceNoInterval.aspx");
    }
</script>


