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

<table class="table01 itemList">
    <thead>
        <tr>
            <th style="min-width: 120px;">公司名稱</th>
            <th style="min-width: 120px;">角色</th>
            <th style="min-width: 120px;">會員名稱</th>
            <th style="min-width: 120px;">ID</th>
            <th style="min-width: 240px;">電子郵件</th>
            <th style="min-width: 150px" aria-sort="other">管理</th>
        </tr>
    </thead>
    <tbody>
        <%  int idx = 0;
            foreach (var item in _items)
            {
                idx++;
                Html.RenderPartial("~/Views/Account/Module/DataItem.ascx", item);
            }
        %>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="5">&nbsp;
<%  if (_sort != null && _sort.Length > 0)
    { %>
<script>
    $(function () {
        initSort(<%= JsonConvert.SerializeObject(_sort) %>,1);
    });
</script>
<%  } %>
<script>
    $(function () {
        buildSort(uiAccountQuery.inquire,<%= (int)ViewBag.PageIndex + 1 %>,1);
    });
</script>
            </td>
            <td>
                <a class="btn" onclick="uiAccountQuery.edit();">新增</a>
            </td>
        </tr>
    </tfoot>
</table>

<script runat="server">

    IEnumerable<UserProfile> _items;
    IEnumerable<UserProfile> _model;
    IOrderedEnumerable<UserProfile> _order;
    int[] _sort;
    int _pageSize;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (IEnumerable<UserProfile>)this.Model;
        _sort = (int[])ViewBag.Sort;
        _pageSize = (int)ViewBag.PageSize;

        if (_sort != null && _sort.Length>0)
        {
            sorting();

            if (_order == null)
            {
                _items = _model.Skip((int)ViewBag.PageIndex * _pageSize)
                    .Take(_pageSize);
            }
            else
            {
                _items = _order.Skip((int)ViewBag.PageIndex * _pageSize)
                    .Take(_pageSize);
            }
        }
        else
        {
            _items = _model.Skip((int)ViewBag.PageIndex * _pageSize)
                .Take(_pageSize);
        }
    }

    void sorting()
    {
        foreach (var idx in _sort)
        {
            switch (idx)
            {
                case 1:
                    _order = _order == null ? _model.OrderBy(i => i.UserRole.Count>0 ? i.UserRole.First().OrganizationCategory.Organization.CompanyName : null) : _order.ThenBy(i => i.UserRole.Count>0 ? i.UserRole.First().OrganizationCategory.Organization.CompanyName : null);
                    break;
                case -1:
                    _order = _order == null ? _model.OrderByDescending(i => i.UserRole.Count>0 ? i.UserRole.First().OrganizationCategory.Organization.CompanyName : null) : _order.ThenByDescending(i => i.UserRole.Count>0 ? i.UserRole.First().OrganizationCategory.Organization.CompanyName : null);
                    break;
                case 2:
                    _order = _order == null ? _model.OrderBy(i => i.UserRole.Count>0 ? i.UserRole.First().RoleID : 0 ) : _order.ThenBy(i => i.UserRole.Count>0 ? i.UserRole.First().RoleID : 0);
                    break;
                case -2:
                    _order = _order == null ? _model.OrderByDescending(i => i.UserRole.Count>0 ? i.UserRole.First().RoleID : 0) : _order.ThenByDescending(i => i.UserRole.Count>0 ? i.UserRole.First().RoleID : 0);
                    break;
                case 3:
                    _order = _order == null ? _model.OrderBy(i => i.UserName) : _order.ThenBy(i => i.UserName);
                    break;
                case -3:
                    _order = _order == null ? _model.OrderByDescending(i => i.UserName) : _order.ThenByDescending(i => i.UserName);
                    break;
                case 4:
                    _order = _order == null ? _model.OrderBy(i => i.PID) : _order.ThenBy(i => i.PID);
                    break;
                case -4:
                    _order = _order == null ? _model.OrderByDescending(i => i.PID) : _order.ThenByDescending(i => i.PID);
                    break;
                case 5:
                    _order = _order == null ? _model.OrderBy(i => i.EMail) : _order.ThenBy(i => i.EMail);
                    break;
                case -5:
                    _order = _order == null ? _model.OrderByDescending(i => i.EMail) : _order.ThenByDescending(i => i.EMail);
                    break;
                case 6:
                    _order = _order == null ? _model.OrderBy(i => i.LevelID) : _order.ThenBy(i => i.LevelID);
                    break;
                case -6:
                    _order = _order == null ? _model.OrderByDescending(i => i.LevelID) : _order.ThenByDescending(i => i.LevelID);
                    break;
            }
        }
    }

</script>
