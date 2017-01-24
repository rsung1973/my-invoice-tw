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
            <%  Html.RenderPartial("~/Views/InvoiceNo/VacantNo/ItemList.ascx",_model); %>
    <table id="tblAction" width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="Bargain_btn">
                <input type="button" class="btn" name="btnPrint" value="下載" onclick="uiVacantNoQuery.download();" />
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

    List<InquireVacantNoResult> _model;
    int _pageSize;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (List<InquireVacantNoResult>)this.Model;
    }

</script>

