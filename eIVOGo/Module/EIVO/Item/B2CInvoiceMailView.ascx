<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.EIVO.Item.InvoiceReceiptView" %>
<%@ Register Src="InvoiceProductPrintView.ascx" TagName="InvoiceProductPrintView"
    TagPrefix="uc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="eIVOGo.Module.Common" %>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
  <tr>
      <%    if (_item.Organization.LogoURL != null)
            { %>
      <th colspan="2">
        <img id="logo" style="width:300px;"  src='<%= eIVOGo.Properties.Settings.Default.mailLinkAddress + VirtualPathUtility.ToAbsolute("~/" +_item.Organization.LogoURL) %>' />
      </th>
      <%    } %>
    <th nowrap="nowrap" colspan="4" class="Head_style_a tdleft">
        <%# _item.Organization.CompanyName %>電子發票開立通知</th>
  </tr>
    <tr>
        <th width="100">
            客戶編號
        </th>
        <td class="tdleft" colspan="3">
            <%# SharedFunction.StringMask(_item.InvoiceBuyer.CustomerID, 4, 3, 'X')%>
        </td>
        <th width="100">
            檢查號碼
        </th>
        <td class="tdleft">
            <%# _item.CheckNo %>
        </td>
    </tr>
    <tr>
        <th width="100">
            發票號碼
        </th>
        <td class="tdleft" colspan="3">
            <%# _item.TrackCode+_item.No %>
        </td>
        <th width="100">
            發票日期
        </th>
        <td class="tdleft">
            <%# ValueValidity.ConvertChineseDateString(_item.InvoiceDate.Value)%>
        </td>
    </tr>
    <tr>
        <th width="100">
            個人識別碼
        </th>
        <td class="tdleft">
            <%#  _item.InvoiceBuyer.IsB2C() ? (string.IsNullOrEmpty(_item.InvoiceBuyer.Name) ? " " : _item.InvoiceBuyer.Name) : "*" %>
        </td>
        <th width="100">
            隨機碼
        </th>
        <td class="tdleft">
            <%#  _item.InvoiceBuyer.IsB2C() ? (string.IsNullOrEmpty(_item.RandomNo) ? " " : _item.RandomNo) : "*" %>
        </td>
        <th width="100">
            總金額
        </th>
        <td class="tdleft">
            <%# String.Format("{0:##,###,###,###}", _item.InvoiceAmountType.TotalAmount) %>
        </td>
    </tr>
    <tr>
        <th width="100">
            買 受 人
        </th>
        <td class="tdleft" colspan="3">
           <%# _item.InvoiceBuyer.IsB2C() ? "*" : _item.InvoiceBuyer.Name %> 
        </td>
        <th width="100">
            統一編號
        </th>
        <td class="tdleft">
            <%# _item.InvoiceBuyer.IsB2C() ? "*" : _item.InvoiceBuyer.ReceiptNo %>
        </td>
    </tr>
    <tr>
        <th width="100">
            地 址
        </th>
        <td colspan="5" class="tdleft">
            *
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" id="table01" style="margin-top: 5px">
    <tr>
        <th nowrap="nowrap">
            品名
        </th>
        <th nowrap="nowrap">
            數量
        </th>
        <th nowrap="nowrap">
            單價
        </th>
        <th nowrap="nowrap">
            金額
        </th>
    </tr>
    <asp:Repeater ID="rpList" runat="server" EnableViewState="false">
        <ItemTemplate>
            <uc1:InvoiceProductPrintView ID="productView" runat="server" />
        </ItemTemplate>
    </asp:Repeater>
</table>
