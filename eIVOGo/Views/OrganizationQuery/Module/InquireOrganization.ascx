<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
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
<!--交易畫面標題-->
<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "店家資料維護"); %>
<!--路徑名稱-->
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">查詢條件
            </th>
        </tr>
        <tr>
            <th nowrap="nowrap" width="120">統編
            </th>
            <td class="tdleft">
                <input name="receiptNo" type="text" value="<%= Request["receiptNo"] %>" />
            </td>
        </tr>
        <tr>
            <th nowrap="nowrap" width="120">店家名稱
            </th>
            <td class="tdleft">
                <input name="companyName" type="text" value="<%= Request["companyName"] %>" />
            </td>
        </tr>
        <tr>
            <th nowrap="nowrap" width="120">店家狀態
            </th>
            <td class="tdleft">
                <select name="organizationStatus">
                    <option value="">全部</option>
                    <option value="1103">已啟用</option>
                    <option value="1101">已停用</option>
                </select>
                <% if (Request["organizationStatus"] != null)
                    { %>
                <script>
                    $(function () {
                        $('select[name="organizationStatus"]').val('<%= Request["organizationStatus"] %>');
                    });
                </script>
                <% } %>
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<% 
    ((CommonInquiry<Organization>)this.Model).RenderAlert(Html);
%>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="queryAction">
    <tr>
        <td class="Bargain_btn">
            <input type="button" value="查詢" name="btnQuery" class="btn" onclick="inquireCompany()" />
        </td>
    </tr>
</table>
<script>
    var $result;
    function inquireCompany(pageNum, onPaging) {
        $('form').ajaxForm({
            url: "<%= Url.Action("InquireCompany","OrganizationQuery") %>" + "?pageIndex=" + pageNum,
            beforeSubmit: function () {
                showLoading();
            },
            success: function (data) {
                if (data) {
                    if (onPaging) {
                        onPaging(data);
                    } else {
                        if ($result)
                            $result.remove();
                        $result = $(data);
                        $('.queryAction').after($result);
                    }
                }
                hideLoading();
            },
            error: function () {
                hideLoading();
            }
        }).submit();
    }

</script>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ViewBag.ActionName = "首頁 > 系統管理維護";
    }
</script>

