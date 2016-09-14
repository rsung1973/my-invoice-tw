<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceAllowanceMailView.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.Item.InvoiceAllowanceMailView" %>
<%@ Register Src="InvoiceAllowanceMailPrintView.ascx" TagName="InvoiceAllowanceMailPrintView"
    TagPrefix="uc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="eIVOGo.Module.Common" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
    <tr>
        <th nowrap="nowrap" colspan="4" class="Head_style_a">
            Google台灣（美商科高國際有限公司台灣分公司）折讓開立通知
        </th>
    </tr>
    <tr>
        <th width="100">
            客戶編號
        </th>
        <td class="tdleft">
            <%# SharedFunction.StringMask(_item.InvoiceItem.InvoiceBuyer.CustomerID, 4, 3, 'X')%>
        </td>
        <th width="100">
            折讓日期
        </th>
        <td class="tdleft">
            <%# ValueValidity.ConvertChineseDateString(_item.AllowanceDate)%>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title" style="margin-top: 5px">
    <tr>
        <th nowrap="nowrap" colspan="4" class="Head_style_a">
            原開立銷貨發票單位
        </th>
    </tr>
    <tr>
        <th width="100">
            統一編號
        </th>
        <td class="tdleft">
            <%# SharedFunction.StringMask(_item.SellerId, 4, 3, 'X')%>
        </td>
        <th width="100">
            名 稱
        </th>
        <td class="tdleft">
            <%# _sellerOrg.CompanyName%>
        </td>
    </tr>
    <tr>
        <th width="100">
            營業所在地址
        </th>
        <td colspan="3" class="tdleft">
            <%# _sellerOrg.Addr%>
        </td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="table01" style="margin-top: 5px">
    <tr>
        <th nowrap="nowrap" colspan="6">
            開立發票
        </th>
        <th nowrap="nowrap" colspan="5">
            退貨或折讓內容
        </th>
    </tr>
    <tr>
        <th nowrap="nowrap">
            聯式
        </th>
        <th nowrap="nowrap">
            年
        </th>
        <th nowrap="nowrap">
            月
        </th>
        <th nowrap="nowrap">
            日
        </th>
        <th nowrap="nowrap">
            字軌
        </th>
        <th nowrap="nowrap">
            號碼
        </th>
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
            金額<br />
            (不含稅之進貨額)
        </th>
        <th nowrap="nowrap">
            營業稅額
        </th>
    </tr>
    <uc1:InvoiceAllowanceMailPrintView ID="printView" runat="server" />
</table>
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
