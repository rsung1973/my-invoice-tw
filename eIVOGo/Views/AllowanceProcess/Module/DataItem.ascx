<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>

<tr>
    <td><input name="chkItem" type="checkbox" value="<%= _model.AllowanceID %>" /></td>
    <td><%= String.Format("{0:yyyy/MM/dd}",_model.AllowanceDate) %></td>
    <td><%= _model.InvoiceAllowanceBuyer.CustomerID %></td>
    <td><%= _model.InvoiceAllowanceSeller.CustomerName %></td>
    <td><%= _model.InvoiceAllowanceSeller.ReceiptNo %></td>
    <td><a onclick="showAllowanceModal(<%= _model.AllowanceID %>);"><%= _model.AllowanceNumber %></a></td>
    <td align="right"><%= String.Format("{0:.}",_model.TotalAmount-_model.TaxAmount) %></td>
    <td align="right"><%= String.Format("{0:.}",_model.TaxAmount) %></td>
    <td align="right"><%= String.Format("{0:.}",_model.TotalAmount) %></td>
    <td><%= _model.InvoiceAllowanceBuyer.IsB2C() ? "" : _model.InvoiceAllowanceBuyer.ReceiptNo %></td>
    <td></td>
<%--    <td>
        <div class="btn-group dropdown" data-toggle="dropdown">
            <button class="btn bg-color-blueLight" data-toggle="dropdown" aria-expanded="false">請選擇功能</button>
            <button class="btn bg-color-blueLight dropdown-toggle" data-toggle="dropdown" aria-expanded="true"><span class="caret"></span></button>
            <ul class="dropdown-menu">
                <%  if (_model.CurrentLevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete)
                    { %>
                <li><a class="btn" onclick="uiInquireBusiness.activate(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>);">啟用</a></li>
                <%  }
                    else
                    { %>
                <li><a class="btn" onclick="uiInquireBusiness.deactivate(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>);">停用</a></li>
                <%  } %>
                <%  if (_model.Counterpart.OrganizationStatus.Entrusting != true)
                    { %>
                <li><a class="btn" onclick="uiInquireBusiness.entrusting(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>,true);">設定自動接收</a></li>
                <%  }
                    else
                    { %>
                <li><a class="btn" onclick="uiInquireBusiness.entrusting(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>,false);">停用自動接收</a></li>
                <%  } %>
                <%  if (_model.Counterpart.OrganizationStatus.EntrustToPrint != true)
                    { %>
                <li><a class="btn" onclick="uiInquireBusiness.entrustToPrint(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>,true);">啟用列印</a></li>
                <%  }
                    else
                    { %>
                <li><a class="btn" onclick="uiInquireBusiness.entrustToPrint(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>,false);">停用列印</a></li>
                <%  } %>
                <li><a class="btn" onclick="uiInquireBusiness.deleteItem(<%= _model.BusinessID %>,<%= _model.MasterID %>,<%= _model.RelativeID %>);">刪除</a></li>
            </ul>
        </div>
    </td>--%>
</tr>


<script runat="server">

    InvoiceAllowance _model;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (InvoiceAllowance)this.Model;
    }

</script>
