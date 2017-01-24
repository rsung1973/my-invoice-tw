<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc1" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="Business.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.UploadManagement" %>
<%@ Import Namespace="eIVOGo.Controllers" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>

<table class="table01 importList">
    <thead>
        <tr>
            <th style="min-width: 50px">類別</th>
            <th style="min-width: 200px">相對營業人名稱</th>
            <th style="min-width: 80px">統一編號</th>
            <th style="min-width: 120px">聯絡人電子郵件</th>
            <th style="min-width: 200px">地址</th>
            <th style="min-width: 120px">電話</th>
            <th style="min-width: 120px">店號</th>
            <th style="min-width: 100px">匯入狀態</th>
        </tr>
    </thead>
    <tbody>
        <%  int idx = 0;
            int? pageIndex = (int?)ViewBag.PageIndex ?? 0;
            var _items = _mgr.ItemList.Skip(pageIndex.Value * Uxnet.Web.Properties.Settings.Default.PageSize).Take(Uxnet.Web.Properties.Settings.Default.PageSize);
                foreach (var item in _items)
                {
                    idx++;  %>
                    <tr>
                        <td><%= ((eIVOGo.Helper.BusinessCounterpartUploadManager)_mgr).BusinessType.ToString() %></td>
                        <td><%= item.Columns[0] %></td>
                        <td><%= item.Columns[1] %></td>
                        <td><%= item.Columns[2]%></td>
                        <td><%= item.Columns[3]%></td>
                        <td><%= item.Columns[4]%></td>
                        <td><%= item.Columns[5]%></td>
                        <td><%= String.Format("{0}{1}", item.UploadStatus.ToString(),item.Status)%></td>
                    </tr>
        <%      }
        %>
            <tfoot>
                <tr>
                    <td colspan="6" align="right">匯入總筆數：<%= _mgr.ItemCount %>筆</td>
                    <td colspan="2">成功：<%= _mgr.IsValid?_mgr.ItemCount:_mgr.ItemCount-_mgr.ErrorList.Count %>失敗：<%= _mgr.IsValid?0:_mgr.ErrorList.Count %></td>
                </tr>
            </tfoot>
    </tbody>
</table>

<script runat="server">

    ModelStateDictionary _modelState;
    ModelSource<InvoiceItem> models;
    BusinessCounterpartUploadManager _mgr;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _modelState = (ModelStateDictionary)ViewBag.ModelState;
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        var profile = Context.GetUser();
        _mgr = (BusinessCounterpartUploadManager)profile["UploadManager"];
    }

</script>
