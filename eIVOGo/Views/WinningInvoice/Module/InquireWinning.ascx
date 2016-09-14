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
<%@ Register Src="~/Module/Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc3" %>
<!--路徑名稱-->
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
    <tr>
        <td width="30">
            <img runat="server" enableviewstate="false" id="img6" src="~/images/path_left.gif"
                alt="" width="30" height="29" />
        </td>
        <td bgcolor="#ecedd5">
            首頁 > 中獎統計表
        </td>
        <td width="18">
            <img runat="server" enableviewstate="false" id="img2" src="~/images/path_right.gif"
                alt="" width="18" height="29" />
        </td>
    </tr>
</table>
<!--交易畫面標題-->
<h1>
    <img runat="server" enableviewstate="false" id="img3" src="~/images/icon_search.gif"
        width="29" height="28" border="0" align="absmiddle" />中獎統計表</h1>
<div id="border_gray">
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


