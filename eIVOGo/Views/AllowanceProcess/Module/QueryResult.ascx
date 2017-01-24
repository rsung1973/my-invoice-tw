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
    <%  Html.RenderPartial("~/Views/AllowanceProcess/Module/ItemList.ascx", _model); %>
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
                            uiAllowanceQuery.inquire(page,function(data){
                                var $node = $('.itemList').next();
                                $('.itemList').remove();
                                $node.before(data);
                            });
                        }
                    });
                });
    </script>
    <table id="tblAction" width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="Bargain_btn">
                <button type="button" class="btn" name="paperStyle" value="A4" onclick="uiAllowanceQuery.print('A4');">A4格式列印</button>
<%--                <input type="button" class="btn" name="btnPrint" value="熱感紙規格列印" onclick="uiAllowanceQuery.print('POS');" />
                (列印買受人地址：<input name="printBuyerAddr" type="radio" value="true" checked="checked" />是 
                <input name="printBuyerAddr" type="radio" value="false" />否)--%>
                <input type="button" class="btn" name="btnPrint" value="Excel下載" onclick="uiAllowanceQuery.download();" />
            </td>
        </tr>
    </table>
    <%  }
        else
        { %>
    <font color="red">查無資料!!</font>
    <%  } %>
</div>

<script runat="server">

    IEnumerable<InvoiceAllowance> _model;
    int _pageSize;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (IEnumerable<InvoiceAllowance>)this.Model;
        _pageSize = (int)ViewBag.PageSize;
    }

</script>

