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
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 系統管理維護" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="functionTitle" runat="server" ItemName="店家資料維護" />
<!--路徑名稱-->
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
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
        </tr>    </table>
    <!--表格 結束-->
</div>
<% 
    ((CommonInquiry<Organization>)this.Model).RenderAlert(Html);
%>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <input type="button" value="查詢" name="btnQuery" class="btn" onclick="$('form').prop('action', '<%= Url.Action("Inquire") %>    ').submit();" />
        </td>
    </tr>
</table>


