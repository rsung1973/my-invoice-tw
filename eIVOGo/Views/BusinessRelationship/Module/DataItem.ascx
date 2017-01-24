<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>

<tr>
    <td><%= _model.BusinessMaster.CompanyName %></td>
    <td><%= _model.Counterpart.CompanyName %></td>
    <td><%= _model.Counterpart.ReceiptNo %></td>
    <td><%= _model.BusinessType.Business %></td>
    <td><%= _model.Counterpart.ContactEmail %></td>
    <td><%= _model.Counterpart.Addr %></td>
    <td><%= _model.Counterpart.Phone %></td>
    <td><%= _model.Counterpart.OrganizationExtension!=null ? _model.Counterpart.OrganizationExtension.CustomerNo : null %></td>
    <td><%= _model.CurrentLevel.HasValue ? _model.LevelExpression.Expression : "已啟用" %>
        <br />
        <%= _model.BusinessID == (int)Naming.InvoiceCenterBusinessType.銷項 && _model.Counterpart.OrganizationStatus.Entrusting==true ? "自動接收" : null %>
        <br />
        <%= _model.Counterpart.OrganizationStatus.EntrustToPrint.HasValue ? (_model.Counterpart.OrganizationStatus.EntrustToPrint == true ? "主動列印":"停用列印"):"未設定" %>
    </td>
    <td>
    <div class="btn-group dropdown" data-toggle="dropdown">
        <button class="btn bg-color-blueLight" data-toggle="dropdown" aria-expanded="false">請選擇功能</button>
        <button class="btn bg-color-blueLight dropdown-toggle" data-toggle="dropdown" aria-expanded="true"><span class="caret"></span></button>
        <ul class="dropdown-menu">
            <li><a class="btn" onclick="uiInquireBusiness.edit(<%= _model.RelativeID %>);">編輯</a></li>
            <%  if (_model.CurrentLevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete)
                { %>
            <li><a class="btn" onclick="uiInquireBusiness.activate(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>);">啟用</a></li>
            <%  }
                else
                { %>
            <li><a class="btn" onclick="uiInquireBusiness.deactivate(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>);">停用</a></li>
            <%  } %>            
            <%  if(_model.Counterpart.OrganizationStatus.Entrusting!=true)
                { %>
            <li><a class="btn" onclick="uiInquireBusiness.entrusting(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>,true);">設定自動接收</a></li>
            <%  }
                else
                { %>
            <li><a class="btn" onclick="uiInquireBusiness.entrusting(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>,false);">停用自動接收</a></li>
            <%  } %>
            <%  if(_model.Counterpart.OrganizationStatus.EntrustToPrint!=true)
                { %>
            <li><a class="btn" onclick="uiInquireBusiness.entrustToPrint(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>,true);">啟用列印</a></li>
            <%  }
                else
                { %>
            <li><a class="btn" onclick="uiInquireBusiness.entrustToPrint(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>,false);">停用列印</a></li>
            <%  } %>
            <li><a class="btn" onclick="uiInquireBusiness.deleteItem(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>);">刪除</a></li>
            <li><a class="btn" onclick="uiInquireBusiness.inquireUser(<%= _model.RelativeID %>);">管理使用者</a></li>
        </ul>
    </div>
</td>
</tr>


<script runat="server">

    BusinessRelationship _model;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (BusinessRelationship)this.Model;
    }

</script>
