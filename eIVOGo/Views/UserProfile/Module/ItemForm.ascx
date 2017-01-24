<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="eIVOGo.Controllers" %>
<%@ Import Namespace="eIVOGo.Helper" %>

<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
    <%  if (_profile.IsSystemAdmin())
        { %>
<%--            <tr>
                <th width="20%">
                    <font color="red">*</font>會　　員
                </th>
                <td colspan="2" class="tdleft">
                    <%  Html.RenderPartial("~/Views/DataFlow/SellerSelector.ascx", _profile.InitializeOrganizationQuery(models)); %>                
                </td>
            </tr>--%>
    <%  } %>
    <tr>
        <th width="20%">
            <font color="red">*</font>帳　　號
        </th>
        <td colspan="2" class="tdleft">
            <input class="form-control" name="PID" type="text" placeholder="" value="<%= _viewModel.PID %>" />
            <input type="hidden" name="KeyID" value="<%= _viewModel.KeyID %>" />
            <input type="hidden" name="SellerID" value="<%= _viewModel.SellerID %>" />
            <input type="hidden" name="DefaultRoleID" value="<%= _viewModel.DefaultRoleID %>" />
        </td>
    </tr>
    <tr>
        <th width="20%">
            <font color="red">*</font>姓　　名
        </th>
        <td colspan="2" class="tdleft">
            <input class="form-control" name="UserName" type="text" placeholder="" value="<%= _viewModel.UserName %>" />
        </td>
    </tr>
    <tr>
        <th width="20%">
            <font color="red">*</font>密　　碼
        </th>
        <td width="20%" class="tdleft">
            <input class="form-control" name="Password" type="password" placeholder="" />
        <td rowspan="2" class="tdleft" style="color: red;">長度最少需要 6 個字元，由英文、數字組成。<br />
            密碼欄位若保持空白，更新後不修改原密碼。</td>
    </tr>
    <tr>
        <th width="20%">
            <font color="red">*</font>重新輸入密碼
        </th>
        <td class="tdleft">
            <input class="form-control" name="Password1" type="password" placeholder="" />
        </td>
    </tr>
    <tr>
        <th width="20%">
            <font color="red">*</font>常用電子郵件
        </th>
        <td colspan="2" class="tdleft">
            <input class="form-control" name="EMail" type="text" placeholder="" value="<%= _viewModel.EMail %>" />
        </td>
    </tr>
    <tr>
        <th width="20%">
            <font color="red">*</font>住　　址
        </th>
        <td colspan="2" class="tdleft">
            <input class="form-control" name="Address" type="text" placeholder="" value="<%= _viewModel.Address %>" />
        </td>
    </tr>
    <tr>
        <th width="20%">電話（日）
        </th>
        <td colspan="2" class="tdleft">
            <input class="form-control" name="Phone" type="text" placeholder="" value="<%= _viewModel.Phone %>" />
        </td>
    </tr>
    <tr>
        <th width="20%">電話（夜）
        </th>
        <td colspan="2" class="tdleft">
            <input class="form-control" name="Phone2" type="text" placeholder="" value="<%= _viewModel.Phone2 %>" />
        </td>
    </tr>
    <tr>
        <th width="20%">行動電話
        </th>
        <td colspan="2" class="tdleft">
            <input class="form-control" name="MobilePhone" type="text" placeholder="" value="<%= _viewModel.MobilePhone %>" />
        </td>
    </tr>
</table>


<script runat="server">

    UserProfileViewModel _viewModel;
    ModelSource<InvoiceItem> models;
    Model.Security.MembershipManagement.UserProfileMember _profile;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        _viewModel = (UserProfileViewModel)this.Model;
        _profile = Business.Helper.WebPageUtility.UserProfile;
    }

</script>
