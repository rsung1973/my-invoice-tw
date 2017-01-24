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
        if(recordCount>0)
        { %>
            <%  Html.RenderPartial("~/Views/BusinessRelationship/Module/ItemList.ascx",_model); %>
            <nav aria-label="Page navigation">
                <ul class="pagination" id="businessPagination"></ul>
            </nav>
            <script>
                $(function () {
                    var obj = $('#businessPagination').twbsPagination({
                                totalPages: <%= (recordCount+Uxnet.Web.Properties.Settings.Default.PageSize-1) / Uxnet.Web.Properties.Settings.Default.PageSize %>,
                                totalRecordCount: <%= recordCount %>,
                                visiblePages: 10,
                                first: '最前',
                                prev: '上頁',
                                next: '下頁',
                                last: '最後',
                                initiateStartPageClick: false,
                                onPageClick: function (event, page) {
                                    uiInquireBusiness.inquireBusiness(page,function(data){
                                        var $node = $('.businessList').next();
                                        $('.businessList').remove();
                                        $node.before(data);
                                    });
                                }
                            });
                        });
            </script>
    <%  }
        else
        { %>
            <font color="red">查無資料!!</font>
            <%  Html.RenderPartial("~/Views/BusinessRelationship/Module/ItemList.ascx",_model); %>
    <%  } %>
</div>

<script runat="server">

    IEnumerable<BusinessRelationship> _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (IEnumerable<BusinessRelationship>)this.Model;
    }

</script>

