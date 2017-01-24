<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>

    <table id="tblAction" width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="Bargain_btn">
                <button type="button" class="btn" name="btnNotify" onclick="uiInvoiceQuery.C0401();">下載C0401</button>
                <button type="button" class="btn" name="btnNotify" onclick="uiInvoiceQuery.C0701();">下載C0701</button>
                <%  if (_viewModel.Cancelled == true)
                    { %>
                <button type="button" class="btn" name="btnNotify" onclick="uiInvoiceQuery.C0501();">下載C0501</button>
                <%  } %>
            </td>
        </tr>
    </table>

<script runat="server">

    InquireInvoiceViewModel _viewModel;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _viewModel = (InquireInvoiceViewModel)ViewBag.ViewModel;
    }

</script>

