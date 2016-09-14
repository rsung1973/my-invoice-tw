<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewInvoiceProductPrintView.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.Item.NewInvoiceProductPrintView" %>
<%@ Import Namespace="Model.DataEntity" %>
<%--<%# !String.IsNullOrEmpty(Item.Brief)? String.Format("<tr><td height=\"15\">{0}</td></tr>",Item.Brief) :""  %>--%>
<asp:Repeater ID="rpList" runat="server" EnableViewState="false">
    <ItemTemplate>
        <div class="content_box">
            <p class="productname" style="width: 115px;"><%# Item.Brief%></p>
            <p class="quantity" style="width: 50px;"><%# String.Format("{0:##,###,###,###}", ((InvoiceProductItem)Container.DataItem).Piece)%></p>
            <p class="price" style="width: 80px;"><%# String.Format("{0:##,###,###,###}", ((InvoiceProductItem)Container.DataItem).UnitCost) %></p>
            <p class="totalPrice" style="width: 90px;"><%# String.Format("{0:##,###,###,###}", ((InvoiceProductItem)Container.DataItem).CostAmount)%></p>
        </div>
    </ItemTemplate>
</asp:Repeater>
