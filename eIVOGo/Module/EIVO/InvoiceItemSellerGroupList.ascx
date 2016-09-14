<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceItemSellerGroupList.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.InvoiceItemSellerGroupList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<asp:GridView ID="gvEntity" runat="server" EnableViewState="False" CssClass="table01"
    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" ShowFooter="True"
    OnSorting="gvInvoice_Sorting" Width="100%">
    <AlternatingRowStyle CssClass="OldLace" />
    <Columns>
        <asp:TemplateField HeaderText="統編" SortExpression="ReceiptNo">
            <ItemTemplate>
                <%# Eval("Seller.ReceiptNo") %>
            </ItemTemplate>
            <FooterStyle HorizontalAlign="Right" CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="營業人名稱" SortExpression="CompanyName">
            <ItemTemplate>
                <%# Eval("Seller.CompanyName") %></ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="營業人地址" SortExpression="Addr">
            <ItemTemplate>
                <%# Eval("Seller.Addr") %>
            </ItemTemplate>
            <FooterTemplate>
                總計&nbsp;&nbsp;營業人數量：<%# _queryItems.Count() %>&nbsp;&nbsp;張數：</FooterTemplate>
            <FooterStyle HorizontalAlign="Right" CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="電子發票張數" SortExpression="TotalCount">
            <ItemTemplate>
                <%# Eval("TotalCount") %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <FooterTemplate>
                <%# _queryItems.Sum(i=>i.TotalCount)%>&nbsp;&nbsp; 金額：
            </FooterTemplate>
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="金額" SortExpression="Summary">
            <ItemTemplate>
                <%# String.Format("{0:##,###,###,##0}",Eval("Summary")) %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <FooterTemplate>
                <%# String.Format("{0:##,###,###,##0}",_queryItems.Sum(i => i.Summary))%></FooterTemplate>
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <div align="center"><font color="red">查無資料!!</font></div>
    </EmptyDataTemplate>
    <PagerTemplate>
        
        <uc2:PagingControl ID="pagingList" runat="server" />
    </PagerTemplate>
</asp:GridView>
<cc1:OrganizationDataSource ID="dsOrg" runat="server">
</cc1:OrganizationDataSource>
