<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>


<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<!--交易畫面標題-->
<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "中獎號碼維護"); %>
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">查詢條件
            </th>
        </tr>
        <tr>
            <th width="200px">發票年度（民國年）
            </th>
            <td class="tdleft">
                <select name="Year">
                    <%  for (int y = DateTime.Today.Year; y >= 2011; y--)
                        { %>
                    <option value="<%= y %>"><%= y-1911 %></option>
                    <%  } %>
                </select>
<%--                <script>
                    $('select[name="Year"]').on('change', function (evt) {
                        $.post('<%= Url.Action("TrackCodeSelector","InvoiceNo") %>', { 'Year': $('select[name="Year"]').val(), 'PeriodNo': $('select[name="PeriodNo"]').val(), 'SelectIndication': '全部' }, function (data) {
                            $('#trackCodeSelector').html(data);
                        });
                    });
                </script>--%>
            </td>
        </tr>
        <tr>
            <th>發票期別</th>
            <td class="tdleft">
                <select name="PeriodNo">
                    <%  for (int y = 1; y < 7; y++)
                        { %>
                    <option value="<%= y %>"><%= String.Format("{0:00}-{1:00}月",y*2-1,y*2) %></option>
                    <%  } %>
                </select>
<%--                <script>
                        $('select[name="PeriodNo"]').val('<%= (DateTime.Today.Month + 1) / 2 %>');
                        $('select[name="PeriodNo"]').on('change', function (evt) {
                            $.post('<%= Url.Action("TrackCodeSelector","InvoiceNo") %>', { 'Year': $('select[name="Year"]').val(), 'PeriodNo': $('select[name="PeriodNo"]').val() }, function (data) {
                                $('#trackCodeSelector').html(data);
                            });
                        });
                </script>--%>
            </td>
        </tr>
<%--        <tr>
            <th>字軌
            </th>
            <td class="tdleft" id="trackCodeSelector">
                <% Html.RenderAction("TrackCodeSelector", "InvoiceNo", new { Year = DateTime.Today.Year, PeriodNo = (DateTime.Today.Month + 1) / 2, SelectIndication = "全部" }); %>
            </td>
        </tr>--%>
    </table>
    <!--表格 結束-->
</div>

<!--按鈕-->
<table border="0" cellspacing="0" cellpadding="0" width="100%" class="queryAction">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <button type="button" onclick="uiWinningNo.inquire();">查詢</button>&nbsp;&nbsp;
            </td>
        </tr>
    </tbody>
</table>
<!--表格 開始-->
<%  Html.RenderPartial("~/Views/WinningNumber/ScriptHelper/Common.ascx"); %>
<%  Html.RenderPartial("~/Views/WinningNumber/ScriptHelper/MatchWinning.ascx"); %>
<script runat="server">


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

    }
</script>

