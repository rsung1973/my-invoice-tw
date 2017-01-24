<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Controllers" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Register Src="~/Module/jQuery/EnumSelector.ascx" TagPrefix="uc1" TagName="EnumSelector" %>

<!--交易畫面標題-->
<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "電子發票字軌維護"); %>
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">查詢條件
            </th>
        </tr>
        <tr>
            <th nowrap="nowrap" width="120">
                <span class="red">*</span> 發票年度（民國年）
            </th>
            <td class="tdleft">
                <select name="year">
                    <%  for (int yy = 2012; yy <= DateTime.Now.Year + 1; yy++)
                        { %>
                            <option value="<%= yy %>"><%= yy-1911 %></option>
                    <%  } %>
                    </select>
                    <script>
                        $(function () {
                            $('select[name="year"]').val(<%= DateTime.Now.Year %>);
                        });
                    </script>
            </td>
        </tr>
        <tr>
            <th>每頁資料筆數
            </th>
            <td class="tdleft" colspan="3">
                <input name="pageSize" type="text" value="<%= Request["pageSize"] ?? Uxnet.Web.Properties.Settings.Default.PageSize.ToString() %>" />
            </td>
        </tr>    
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<table border="0" cellspacing="0" cellpadding="0" width="100%" class="queryAction">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <button type="button" onclick="uiTrackCodeQuery.inquire();" >查詢</button>
            </td>
        </tr>
    </tbody>
</table>
<!--表格 開始-->
<%  Html.RenderPartial("~/Views/TrackCode/ScriptHelper/Common.ascx"); %>
<script runat="server">

    ModelSource<InvoiceItem> models;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
    }
</script>

