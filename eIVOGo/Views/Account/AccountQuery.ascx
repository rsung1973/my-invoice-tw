<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Controllers" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Register Src="~/Module/jQuery/EnumSelector.ascx" TagPrefix="uc1" TagName="EnumSelector" %>

<!--交易畫面標題-->
<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "使用者管理"); %>
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="4" class="Head_style_a">查詢條件
            </th>
        </tr>
        <tr>
            <th nowrap="nowrap" width="120">
                營業人名稱
            </th>
            <td colspan="3" class="tdleft">
                <input name="SellerID" type="hidden" value="<%= _viewModel.SellerID %>" />
                <%= _orgItem!=null ? _orgItem.CompanyName + "(" + _orgItem.ReceiptNo + ")" : null %>
            </td>
        </tr>
        <%--<tr>
            <th nowrap="nowrap" width="120">
                角色
            </th>
            <td  colspan="3" class="tdleft">
                <uc1:EnumSelector runat="server" SelectorIndication="全部" ID="RoleID" FieldName="RoleID" TypeName="Model.Locale.Naming+EIVOUserRoleID, Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
            </td>
        </tr>--%>
        <tr>
            <th nowrap="nowrap" width="120">帳號
            </th>
            <td class="tdleft">
                <input name="PID" type="text" value="<%= Request["PID"] %>" class="form-control" />
                <input name="RoleID" type="hidden" value="<%= _viewModel.RoleID %>" />
            </td>
            <th nowrap="nowrap" width="120">
                姓名
            </th>
            <td class="tdleft">
                <input name="UserName" type="text" value="<%= Request["UserName"] %>" class="form-control" />
            </td>
            
        </tr>
        <tr>
            <th nowrap="nowrap" width="120">會員狀態
            </th>
            <td class="tdleft">
                <uc1:EnumSelector runat="server" SelectorIndication="全部" ID="Status" FieldName="LevelID" TypeName="Model.Locale.Naming+BusinessRelationshipStatus, Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
            </td>
            <th>每頁資料筆數
            </th>
            <td class="tdleft">
                <input name="pageSize" type="text" value="<%= Request["pageSize"] ?? Uxnet.Web.Properties.Settings.Default.PageSize.ToString() %>" />
            </td>
        </tr>    
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<table border="0" cellspacing="0" cellpadding="0" width="100%" class="queryAction">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <button type="button" onclick="uiAccountQuery.inquire();" >查詢</button>
            </td>
        </tr>
    </tbody>
</table>
<!--表格 開始-->
<%  Html.RenderPartial("~/Views/Account/ScriptHelper/Common.ascx"); %>
<script runat="server">

    ModelSource<InvoiceItem> models;
    UserAccountQueryViewModel _viewModel;
    Model.Security.MembershipManagement.UserProfileMember _profile;
    Organization _orgItem;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        _viewModel = (UserAccountQueryViewModel)ViewBag.ViewModel;
        _profile = Business.Helper.WebPageUtility.UserProfile;
        _orgItem = models.GetTable<Organization>().Where(o => o.CompanyID == _viewModel.SellerID).FirstOrDefault();
    }
</script>

