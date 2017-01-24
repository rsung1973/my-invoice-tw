<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>

<tr>
    <td><%= _model.UserRole.Count>0 ? _model.UserRole.First().OrganizationCategory.Organization.CompanyName : null %></td>
    <td><%= _model.UserRole.Count>0 ? ((Naming.EIVOUserRoleID)_model.UserRole.First().RoleID).ToString() : null %></td>
    <td><%= _model.UserName %></td>
    <td><%= _model.PID %></td>
    <td><%= _model.EMail %></td>
    <td>
        <div class="btn-group dropdown" data-toggle="dropdown">
            <button class="btn bg-color-blueLight" data-toggle="dropdown" aria-expanded="false">請選擇功能</button>
            <button class="btn bg-color-blueLight dropdown-toggle" data-toggle="dropdown" aria-expanded="true"><span class="caret"></span></button>
            <ul class="dropdown-menu">
                <li><a class="btn" onclick="uiAccountQuery.edit(<%= _model.UID %>);">編輯</a></li>
                <%  if (_model.LevelID == (int)Naming.MemberStatusDefinition.Mark_To_Delete)
                    { %>
                <li><a class="btn" onclick="uiAccountQuery.activate(<%= _model.UID %>);">啟用</a></li>
                <%  }
                    else
                    { %>
                <li><a class="btn" onclick="uiAccountQuery.sendConfirmation(<%= _model.UID %>);">重送確認信</a></li>
                <li><a class="btn" onclick="uiAccountQuery.deactivate(<%= _model.UID %>);">停用</a></li>
                <li><a class="btn" onclick="uiAccountQuery.delete(<%= _model.UID %>);">刪除</a></li>
                <%  } %>
            </ul>
        </div>
    </td>
</tr>


<script runat="server">

    UserProfile _model;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (UserProfile)this.Model;
    }

</script>
