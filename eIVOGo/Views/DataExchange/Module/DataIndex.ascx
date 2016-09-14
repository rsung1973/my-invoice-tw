<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 資料管理" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="functionTitle" runat="server" ItemName="資料修改" />
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">各項資料
            </th>
        </tr>
        <tr>
            <th>買受人
            </th>
            <td class="tdleft">
                <input type="button" name="btnBuyerSample" value="下載範本" />&nbsp;&nbsp;
                <input name="InvoiceBuyer" id="InvoiceBuyer" type="file" style="display:inline;" />&nbsp;&nbsp;
                <input type="button" name="btnUpdateBuyer" value="傳送更新" />
            </td>
        </tr>
        <tr>
            <th>發票字軌
            </th>
            <td class="tdleft">
                <input type="button" name="btnTrackCodeSample" value="下載範本" />&nbsp;&nbsp;
                <input name="TrackCode" id="TrackCode" type="file" style="display: inline;" />&nbsp;&nbsp;
                <input type="button" name="btnUpdateTrackCode" value="傳送更新" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<script>
    $(function() {

        var $formDownload;
        var $data;
        var $oriAction;

        function prepareTarget() {
            if($formDownload==undefined) {
                $data = $('<input name="data" type="hidden"/>');
                $formDownload = $('form').append($data);
                $oriAction = $formDownload.prop('action');
                $formDownload.prop('enctype','multipart/form-data');
            }
        }

        $('input[name="btnBuyerSample"]').on('click',function(evt) {
            prepareTarget();
            $data.val('InvoiceBuyer');
            $formDownload.prop('action', '<%= VirtualPathUtility.ToAbsolute("~/Helper/GetSample.ashx") %>');
            $formDownload.submit();
            $formDownload.prop('action',$oriAction);
        });

        $('input[name="btnUpdateBuyer"]').on('click',function(evt) {
            prepareTarget();
            var $file = $('input:file[name="InvoiceBuyer"]');
            if($file.length>0 && $file.val()!='' ) {
                $formDownload.prop('action', '<%= Url.Action("UpdateBuyer") %>');
                $formDownload.submit();
                $formDownload.prop('action',$oriAction);
            } else {
                alert('請選擇檔案!!');
            }
        });

        $('input[name="btnTrackCodeSample"]').on('click', function (evt) {
            prepareTarget();
            $data.val('TrackCode');
            $formDownload.prop('action', '<%= VirtualPathUtility.ToAbsolute("~/Helper/GetSample.ashx") %>');
            $formDownload.submit();
            $formDownload.prop('action', $oriAction);
        });

        $('input[name="btnUpdateTrackCode"]').on('click', function (evt) {
            prepareTarget();
            var $file = $('input:file[name="TrackCode"]');
            if ($file.length > 0 && $file.val() != '') {
                $formDownload.prop('action', '<%= Url.Action("UpdateTrackCode") %>');
                $formDownload.submit();
                $formDownload.prop('action', $oriAction);
            } else {
                alert('請選擇檔案!!');
            }
        });

        <% if(ViewBag.AlertMessage!=null) { %>
        alert('<%= HttpUtility.JavaScriptStringEncode(ViewBag.AlertMessage) %>');
        <% } %>
    });
</script>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += views_dataexchange_module_dataindex_ascx_PreRender;
    }

    void views_dataexchange_module_dataindex_ascx_PreRender(object sender, EventArgs e)
    {
        Page.Form.Enctype = "multipart/form-data";
    }
</script>

