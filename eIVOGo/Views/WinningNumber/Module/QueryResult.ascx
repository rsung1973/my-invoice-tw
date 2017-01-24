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
            <%  Html.RenderPartial("~/Views/WinningNumber/Module/ItemList.ascx",_model);
                Html.RenderPartial("~/Views/WinningNumber/ResultAction/DoMatch.ascx");
                %>
    <%  }
        else
        { %>
            <font color="red">查無資料!!</font>
            <%  Html.RenderPartial("~/Views/WinningNumber/Module/ItemList.ascx",_model); %>
    <%  } %>
</div>


<script runat="server">

    IEnumerable<UniformInvoiceWinningNumber> _model;
    int _pageSize;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (IEnumerable<UniformInvoiceWinningNumber>)this.Model;
    }

</script>

