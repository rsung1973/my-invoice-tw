<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>

<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "查詢結果"); %>

<div class="border_gray" style="overflow-x: auto; max-width: 1024px;">
    <%  var recordCount = _model.Count();
        if (recordCount > 0)
        { %>
    <%  Html.RenderPartial("~/Views/InvoiceProcess/Module/ItemList.ascx", _model); %>
    <input type="hidden" name="pageIndex" />
    <nav aria-label="Page navigation">
        <ul class="pagination" id="itemPagination"></ul>
    </nav>
    <script>
        $(function () {
            var obj = $('#itemPagination').twbsPagination({
                totalPages: <%= (recordCount+_pageSize-1) / _pageSize %>,
                        totalRecordCount: <%= recordCount %>,
                        visiblePages: 10,
                        first: '最前',
                        prev: '上頁',
                        next: '下頁',
                        last: '最後',
                        initiateStartPageClick: false,
                        onPageClick: function (event, page) {
                            uiInvoiceQuery.inquire(page,function(data){
                                var $node = $('.itemList').next();
                                $('.itemList').remove();
                                $node.before(data);
                            });
                        }
                    });
                });
    </script>
    <%      if(ViewBag.ResultAction!=null)
            {
                Html.RenderPartial((String)ViewBag.ResultAction);
            } %>
    <%  }
        else
        { %>
    <font color="red">查無資料!!</font>
    <%  } %>
</div>

<script runat="server">

    IEnumerable<InvoiceItem> _model;
    int _pageSize;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (IEnumerable<InvoiceItem>)this.Model;
        _pageSize = (int)ViewBag.PageSize;
    }

</script>

