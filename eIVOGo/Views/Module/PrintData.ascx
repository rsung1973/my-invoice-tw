<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Module.Base" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<input type="button" class="btn" name="btnPrint" value="資料列印" onclick="$('form').prop('target','prnFrame').prop('action', '<%= Url.Action(ViewBag.PrintAction ?? "PrintResult") %>').submit();" />
<% if(Page.Items["prnFrame"]==null) 
   { %>
<script>
    $(function () {
        var $dialog;

        if ($('iframe[name="prnFrame"]').length == 0) {
            $('form').append($('<iframe name="prnFrame" width="0" height="0"/>'));
        }
        $('iframe[name="prnFrame"]').load(function () {
            $('form').prop('target', '');
            if ($dialog)
                $dialog.dialog("close");
        }).ready(function () {
            $('form').prop('target', '');
            if ($dialog)
                $dialog.dialog("close");
        });

        $("input[name='btnPrint']").click(function () {
            if ($dialog) {
                $dialog.dialog();
            } else {
                $dialog = $("<div align='center'>列印作業即將進行，請稍後...!!</div>")
                    .dialog({
                        width: 480,
                        modal: true,
                        close: function () {
                            $('form').prop('target', '');
                            $('input[name="__EVENTTARGET"]').val('');
                        }
                    });
            }
        });
    });

</script>
<%      
    Page.Items["prnFrame"] = this;
   } %>
