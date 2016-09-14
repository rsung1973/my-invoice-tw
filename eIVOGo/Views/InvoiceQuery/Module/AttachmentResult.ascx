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

<% Html.RenderPartial("Module/InvoiceReport", Model); %>
<% if(!models.InquiryHasError) {  %>
<uc6:functiontitlebar id="resultTitle" runat="server" itemname="查詢結果" />
<div class="border_gray">
    <% 
       Html.RenderPartial("~/Views/Module/InvoiceAttachmentList.ascx", models); 
    %>
    <!--按鈕-->
</div>
<%   if(models.Items.Count() > 0) {   %>
<table id="tblAction" width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <input type="button" value="選取下載" name="btnDownload" class="btn" />&nbsp;&nbsp;
            <input type="button" value="全部下載" name="btnDownloadAll" class="btn" onclick="$('form').prop('action', '<%= Url.Action("DownloadAll") %>    ').submit();" />
        </td>
    </tr>
</table>
<script>
    $(function() {

        var $formDownload;
        var $data;

        $('input[name="btnDownload"]').on('click',function(evt) {
            var $items = $('input[name="chkItem"]:checked');
            if($items.length > 0) {
                if($formDownload==undefined) {
                    $data = $('<input name="data" type="hidden"/>');
                    $formDownload = $('<form target="_blank" method="post">')
                        .append($data);
                    $('body').append($formDownload);
                }
                $formDownload.prop('action', '<%= Url.Action("DownloadAttachment") %>');
                var data = [];
                $items.each(function(idx,elmt) {
                    data[data.length] = $(elmt).val();
                });
                $data.val(JSON.stringify(data));
                $formDownload.submit();
            } else {
                alert('請選擇下載資料項!!');
            }
        });
    });
</script>
<%  } %>
<% } %>

<script runat="server">
    ModelSource<InvoiceItem> models;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = TempData.GetModelSource<InvoiceItem>();
        models.DataSourcePath = VirtualPathUtility.ToAbsolute("~/InvoiceQuery/AttachmentGridPage");
        
    }

</script>