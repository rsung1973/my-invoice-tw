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
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<!--路徑名稱-->


<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 > 捐贈統計表" />
<!--交易畫面標題-->
<h1>
    <img runat="server" enableviewstate="false" id="img3" src="~/images/icon_search.gif"
        width="29" height="28" border="0" align="absmiddle" />捐贈統計表
</h1>
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <% 
                ((CommonInquiry<InvoiceItem>)this.Model).Render(Html);
        %>
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<% 
    ((CommonInquiry<InvoiceItem>)this.Model).RenderAlert(Html);
%>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <input type="button" value="查詢" name="btnQuery" class="btn" onclick="$('form').prop('action', '<%= Url.Action("InquireReport") %>    ').submit();" />
        </td>
    </tr>
</table>
