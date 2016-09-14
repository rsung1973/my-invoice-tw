<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewInvoicePOSProductPrintView.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.Item.NewInvoicePOSProductPrintView" %>
<%@ Import Namespace="Model.DataEntity" %>
<%--<%# !String.IsNullOrEmpty(Item.Brief)? String.Format("<tr><td height=\"15\">{0}</td></tr>",Item.Brief) :""  %>--%>
<asp:Repeater ID="rpList" runat="server" EnableViewState="false">
    <ItemTemplate>
        <div >
            <table style="width:4.8cm">
                <tr>
                    <td style="width:25%" >
             <p style=" text-align:right; float:left;font-size:8pt;"><%# Item.Brief%></p>
                        </td>
                        <td style="width:25%">
           <p style="text-align:right;float:left;font-size:8pt;"><%# String.Format("{0:##,###,###,###}", ((InvoiceProductItem)Container.DataItem).Piece)%></p>
                            </td>
                        <td style="width:25%">
            <p style="text-align:right;float:left;font-size:8pt;"><%# String.Format("{0:##,###,###,###}", ((InvoiceProductItem)Container.DataItem).UnitCost) %></p>
                            </td>
                        <td style="width:25%">
            <p style=" text-align:right; /*margin-left: 6px;*/float:right;font-size:8pt;"><%# String.Format("{0:##,###,###,###}", ((InvoiceProductItem)Container.DataItem).CostAmount)%></p>
                            </td>
        </tr>
                    </table>
        </div>
    </ItemTemplate>
</asp:Repeater>
