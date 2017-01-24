<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Newtonsoft.Json" %>

<div class="border_gray" style="overflow-x: auto; max-width: 1024px;">
    <table border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <td class="Bargain_btn">
                <button type="button" onclick="uiHandling.processPack();">整理郵件號碼</button>
            </td>
        </tr>
    </table>
    <table class="table01 itemList">
        <thead>
            <tr>
                <th style="min-width: 80px;">寄送日期</th>
                <th style="min-width: 120px;">GoogleID</th>
                <th style="min-width: 120px;">發票號碼</th>
                <th style="min-width: 320px;">掛號號碼</th>
                <th style="min-width: 120px;">收件人</th>
                <th style="min-width: 280px;">收件人地址</th>
                <th style="min-width: 60px;">備考</th>
                <th style="min-width: 150px;">&nbsp;</th>
            </tr>
        </thead>
        <tbody>
            <%  Html.RenderPartial("~/Views/Handling/MailTracking/ItemList.ascx", _model);  %>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="8" class="Bargain_btn">
                    <button type="button" onclick="uiHandling.inquireTracking();">存檔</button>&nbsp;
                    <button type="button" onclick="uiHandling.download();">下載</button>
                </td>
            </tr>
        </tfoot>
    </table>
</div>

<script runat="server">

    IEnumerable<InvoiceItem> _items;
    IEnumerable<InvoiceItem> _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (IEnumerable<InvoiceItem>)this.Model;
        _items = _model;

    }

</script>
