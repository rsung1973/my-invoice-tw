<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.DonatedInvoiceList" %>
<%@ Import Namespace="eIVOGo.Module.Inquiry" %>    
<table border="0" cellspacing="0" cellpadding="0" width="100%" class="table01">
        <tbody>
            <tr>
                <th nowrap>
                    社福機構統編
                </th>
                <th nowrap>
                    社福名稱
                </th>
                <th nowrap>
                    發票號碼
                </th>
                <th nowrap>
                    是否中獎
                </th>
            </tr>
            <asp:Repeater ID="rpList" runat="server" EnableViewState="false">
                <AlternatingItemTemplate>
                    <tr id="tr1" class="OldLace" runat="server" visible="<%# ((_QueryItem)Container.DataItem).InvoiceID.HasValue %>">
                        <td align="middle">
                            <%# ((_QueryItem)Container.DataItem).Agency!=null ? ((_QueryItem)Container.DataItem).Agency.ReceiptNo : "" %>
                        </td>
                        <td>
                            <%# ((_QueryItem)Container.DataItem).Agency!=null ? ((_QueryItem)Container.DataItem).Agency.CompanyName : "" %>
                        </td>
                        <td align="middle">
                            <%# ((_QueryItem)Container.DataItem).InvoiceNo %>
                        </td>
                        <td class="red" align="middle">
                            <%# ((_QueryItem)Container.DataItem).Winnable %>
                        </td>
                    </tr>
                    <tr id="tr2" runat="server" visible="<%# !((_QueryItem)Container.DataItem).InvoiceID.HasValue %>">
                        <td class="total-count" colspan="4" align="right">
                            總計&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;發票張數：<%# String.Format("{0:##,###,###,###}",((_QueryItem)Container.DataItem).InvoiceCount) %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;中獎總張數：<%# String.Format("{0:##,###,###,###,##0}",((_QueryItem)Container.DataItem).WinningInvoiceCount) %></td>
                    </tr>
                </AlternatingItemTemplate>
                <ItemTemplate>
                    <tr id="tr1" runat="server" visible="<%# ((_QueryItem)Container.DataItem).InvoiceID.HasValue %>">
                        <td align="middle">
                            <%# ((_QueryItem)Container.DataItem).Agency!=null ?((_QueryItem)Container.DataItem).Agency.ReceiptNo : "" %>
                        </td>
                        <td>
                            <%# ((_QueryItem)Container.DataItem).Agency!=null ?((_QueryItem)Container.DataItem).Agency.CompanyName : "" %>
                        </td>
                        <td align="middle">
                            <%# ((_QueryItem)Container.DataItem).InvoiceNo %>
                        </td>
                        <td class="red" align="middle">
                            <%# ((_QueryItem)Container.DataItem).Winnable %>
                        </td>
                    </tr>
                    <tr id="tr2" runat="server" visible="<%# !((_QueryItem)Container.DataItem).InvoiceID.HasValue %>">
                        <td class="total-count" colspan="4" align="right">
                            總計&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;發票張數：<%# String.Format("{0:##,###,###,###}",((_QueryItem)Container.DataItem).InvoiceCount) %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;中獎總張數：<%# String.Format("{0:##,###,###,###,##0}",((_QueryItem)Container.DataItem).WinningInvoiceCount) %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
