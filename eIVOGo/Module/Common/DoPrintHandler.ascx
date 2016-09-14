<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.Common.ActionHandler" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Module.Base" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc2" %>
<input type="button" class="btn" name="btnPrint" value="資料列印" onclick="$('form').prop('target','prnFrame');<%= this.GetPostBackEventReference(null) %>" />
<cc2:DocumentDataSource ID="dsEntity" runat="server">
</cc2:DocumentDataSource>
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
<script runat="server">

    public void DoPrint(String content, bool hasPrintMode = false)
    {
        Page.Items["hasPrintMode"] = hasPrintMode;
        Page.Items["pageContent"] = content;
        Server.Transfer("~/Published/PrintPageContent.aspx");
    }

    public void DoPrintInvoice()
    {
        Model.Security.MembershipManagement.UserProfileMember _userProfile = Business.Helper.WebPageUtility.UserProfile;

        String keyCodeFile = Path.Combine(Logger.LogPath, "ORCodeKey.txt");
        if (File.Exists(keyCodeFile))
        {
            if (!String.IsNullOrEmpty(File.ReadAllText(keyCodeFile)))
            {
                String[] ar = Request.GetItemSelection();
                if (ar != null && ar.Count() > 0)
                {
                    if (_userProfile.EnqueueDocumentPrint(dsEntity.CreateDataManager(), ar.Select(a => int.Parse(a))))
                        Server.Transfer("~/SAM/NewPrintInvoiceAsPDF.aspx");
                    else
                        Page.AlertOnly("資料已列印請重新選擇!!");
                }
                else
                {
                    Page.AlertOnly("請選擇列印資料!!");
                }
            }
            else
            {
                Page.AlertOnly("QRCode金鑰檔無內容，無法列印!!");
            }
        }
        else
        {
            Page.AlertOnly("無QRCode金鑰檔，無法列印!!");
        }
    }

    public void DoPrintAllowance()
    {
        Model.Security.MembershipManagement.UserProfileMember _userProfile = Business.Helper.WebPageUtility.UserProfile;

        String[] ar = Request.GetItemSelection();
        if (ar != null && ar.Count() > 0)
        {
            if (_userProfile.EnqueueDocumentPrint(dsEntity.CreateDataManager(), ar.Select(a => int.Parse(a))))
                Server.Transfer("~/SAM/PrintAllowanceAsPDF.aspx");
            else
                Page.AlertOnly("資料已列印請重新選擇!!");
        }
        else
        {
            Page.AlertOnly("請選擇列印資料!!");
        }
    }      
    
</script>