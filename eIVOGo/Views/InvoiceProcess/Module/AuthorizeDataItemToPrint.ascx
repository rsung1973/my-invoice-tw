<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>

<tr>
    <td>
        <%  if (_model.CDS_Document.DocumentPrintLog.Count > 0 && _model.CDS_Document.DocumentAuthorization == null)
            { %>
                <input name="chkItem" type="checkbox" value="<%= _model.InvoiceID %>" />
        <%  }
            else
            { %>
                <span style="color:blue">可列印</span>
        <%  } %>
    </td>
    <td><%= String.Format("{0:yyyy/MM/dd}",_model.InvoiceDate) %></td>
    <td><%= _model.InvoiceBuyer.CustomerID %></td>
    <td><%= _model.InvoicePurchaseOrder!=null ? _model.InvoicePurchaseOrder.OrderNo : null %></td>
    <td><%= _model.InvoiceSeller.CustomerName %></td>
    <td><%= _model.InvoiceSeller.ReceiptNo %></td>
    <td><a onclick="showInvoiceModal(<%= _model.InvoiceID %>);"><%= _model.TrackCode %><%= _model.No %></a></td>
    <td align="right"><%= String.Format("{0:.}",_model.InvoiceAmountType.SalesAmount) %></td>
    <td align="right"><%= String.Format("{0:.}",_model.InvoiceAmountType.TaxAmount) %></td>
    <td align="right"><%= String.Format("{0:.}",_model.InvoiceAmountType.TotalAmount) %></td>
    <td><%= _model.InvoiceWinningNumber!=null ? _model.InvoiceWinningNumber.UniformInvoiceWinningNumber.PrizeType : "N/A" %></td>
    <td><%= _model.InvoiceBuyer.IsB2C() ? "" : _model.InvoiceBuyer.ReceiptNo %></td>
    <td><%= _model.InvoiceBuyer.CustomerName %></td>
    <td><%= _model.InvoiceBuyer.ContactName %></td>
    <td><%= _model.InvoiceBuyer.Address %></td>
    <td><%= _model.InvoiceBuyer.EMail %></td>
    <td><%= String.Join("", _model.InvoiceDetails.Select(t => t.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark)) %></td>
    <td><%= _model.CDS_Document.SMSNotificationLogs.Any() ? "是" : "否" %></td>
    <td><%= _model.InvoiceCarrier!=null ? _model.InvoiceCarrier.CarrierNo : null %></td>
    <td>
        <%--<div class="btn-group dropdown" data-toggle="dropdown">
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
        </div>--%>
    </td>
</tr>


<script runat="server">

    InvoiceItem _model;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (InvoiceItem)this.Model;
    }

</script>
