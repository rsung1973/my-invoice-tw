<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceAllowanceMailPrintView.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.Item.InvoiceAllowanceMailPrintView" %>
<%@ Import Namespace="Model.DataEntity" %>
<%--<%# !String.IsNullOrEmpty(Item.Brief)? String.Format("<tr><td height=\"15\">{0}</td></tr>",Item.Brief) :""  %>--%>
<asp:Repeater ID="rpList" runat="server" EnableViewState="false">
    <ItemTemplate>
        <tr>
            <td height="15" valign="top">
                二
            </td>
            <td align="right" valign="top">
                <%# _item.InvoiceItem.InvoiceDate.Value.Year%>
            </td>
             <td align="right" valign="top">
                <%#_item.InvoiceItem.InvoiceDate.Value.Month%>
            </td>
             <td align="right" valign="top">
                <%#_item.InvoiceItem.InvoiceDate.Value.Day%>
            </td>
            <td align="right" valign="top">
                <%#_item.InvoiceItem.TrackCode%>
            </td>
            <td align="right" valign="top">
                <%#_item.InvoiceItem.No%>
            </td>
             <td align="right" valign="top">
                <%#((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.InvoiceProductItem.InvoiceProduct.Brief%>
            </td>
            <td align="right" valign="top">
                <%#((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.Piece%>
            </td>
            <td align="right" valign="top">
                <%#((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.UnitCost%>
            </td>
            <td align="right" valign="top">
                <%#((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.Amount%>
            </td>
            <td align="right" valign="top">
                <%#((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.Tax%>
            </td>
           <%-- <td align="right" valign="top">
                <%# String.Format("{0:##,###,###,###}", ((InvoiceAllowanceDetail)Container.DataItem).UnitCost)%>
            </td>
            <td align="right" valign="top">
                <%# String.Format("{0:##,###,###,###}", ((InvoiceAllowanceDetail)Container.DataItem).CostAmount)%>
            </td>--%>
        </tr>
    </ItemTemplate>
</asp:Repeater>
