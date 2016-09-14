<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/JsGrid/InquireInvoiceTemplate.ascx" TagPrefix="uc4" TagName="InquireInvoiceTemplate" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Module.Base" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>

<%   if(models.Items.Count() > 0) {   %>
<table id="tblAction" width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <input type="button" value="啟動下載" name="btnDownload" class="btn" />&nbsp;&nbsp;
            <%= _userProfile["assignDownload"]!=null && File.Exists((String)_userProfile["assignDownload"]) ? _userProfile["assignDownload"] : null %>
        <% if(models.Items.Count() <= 50000) { %>
            <% Html.RenderPartial("~/Views/Module/PrintData.ascx"); %>
            <input type="button" value="CSV下載" name="btnCsv" class="btn" onclick="$('form').prop('action', '<%= Url.Action("DownloadCSV") %>    ').submit();" />
            <input type="button" value="Excel下載" name="btnXlsx" class="btn" onclick="$('form').prop('action', '<%= Url.Action("CreateXlsx") %>    ').submit();" />
        <% } %>
        </td>
    </tr>
</table>
<script>
    $(function() {
        $('input[name="btnDownload"]').on('click',function(evt) {
            $.post('<%= Url.Action("AssignDownload") %>',$('form').serializeArray(),function(html) {
                alert(html);
            });
        });
    });
</script>
<%  } %>

<script runat="server">
    Model.Security.MembershipManagement.UserProfileMember _userProfile;
    ModelSource<InvoiceItem> models;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = TempData.GetModelSource<InvoiceItem>();
        models.DataSourcePath = VirtualPathUtility.ToAbsolute("~/InvoiceQuery/AttachmentGridPage");
        _userProfile = Business.Helper.WebPageUtility.UserProfile;
    }

</script>