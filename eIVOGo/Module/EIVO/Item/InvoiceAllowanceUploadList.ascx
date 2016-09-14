<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceAllowanceUploadList.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.Item.InvoiceAllowanceUploadList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.InvoiceManagement" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<!--表格 開始-->
<asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
    GridLines="None" CellPadding="0" CssClass="table01" ClientIDMode="Static" EnableViewState="False"
    AllowPaging="false" ShowFooter="True">
    <Columns>
        <asp:TemplateField HeaderText="序號">
            <ItemTemplate>
                <%# ((GoogleInvoiceAllowance)Container.DataItem).Columns[0] %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Google ID">
            <ItemTemplate>
                <%# ((GoogleInvoiceAllowance)Container.DataItem).Columns[2] %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="折讓號碼">
            <ItemTemplate>
                <%# ((GoogleInvoiceAllowance)Container.DataItem).Columns[3] %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="原發票號碼">
            <ItemTemplate>
                <%# ((GoogleInvoiceAllowance)Container.DataItem).Columns[5] %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="買受人名稱">
            <ItemTemplate>
                <%# ((GoogleInvoiceAllowance)Container.DataItem).Allowance != null && ((GoogleInvoiceAllowance)Container.DataItem).Allowance.InvoiceItem!=null ? ((GoogleInvoiceAllowance)Container.DataItem).Allowance.InvoiceItem.InvoiceBuyer.CustomerName : ((GoogleInvoiceAllowance)Container.DataItem).Columns[13]%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="買受人統編">
            <ItemTemplate>
                <%# String.IsNullOrEmpty(((GoogleInvoiceAllowance)Container.DataItem).Columns[10]) ? "N/A" : ((GoogleInvoiceAllowance)Container.DataItem).Columns[10]%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="未稅金額" FooterText="總計">
            <ItemTemplate>
                <%# ((GoogleInvoiceAllowance)Container.DataItem).Allowance!=null ? String.Format("{0:##,###,###,###,###}", ((GoogleInvoiceAllowance)Container.DataItem).Allowance.TotalAmount - ((GoogleInvoiceAllowance)Container.DataItem).Allowance.TaxAmount) : ""%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <FooterStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="稅額">
            <ItemTemplate>
                <%# ((GoogleInvoiceAllowance)Container.DataItem).Allowance!=null ? String.Format("{0:##,###,###,###,###}", ((GoogleInvoiceAllowance)Container.DataItem).Allowance.TaxAmount) :"" %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <FooterStyle HorizontalAlign="Right" />
            <FooterTemplate>
                匯入總筆數：<%# ((GoogleAllowanceUploadManager)_mgr).ItemList.Count %></FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="含稅金額">
            <ItemTemplate>
                <%# ((GoogleInvoiceAllowance)Container.DataItem).Allowance!=null ? String.Format("{0:##,###,###,###,###}", ((GoogleInvoiceAllowance)Container.DataItem).Allowance.TotalAmount) :"" %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <FooterStyle HorizontalAlign="Right" />
            <FooterTemplate>
                成功：<%# ((GoogleAllowanceUploadManager)_mgr).ItemList.Where(i=>i.UploadStatus==Naming.UploadStatusDefinition.等待匯入 || i.UploadStatus==Naming.UploadStatusDefinition.匯入成功).Count()  %></FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="匯入狀態">
            <ItemTemplate>
                <%# String.IsNullOrEmpty(((GoogleInvoiceAllowance)Container.DataItem).Status)? ((GoogleInvoiceAllowance)Container.DataItem).UploadStatus.ToString() : String.Format("<font color='red'>{0}</font>",((GoogleInvoiceAllowance)Container.DataItem).Status)%></ItemTemplate>
            <ItemStyle HorizontalAlign="Left" />
            <FooterTemplate>
                失敗：<%# ((GoogleAllowanceUploadManager)_mgr).ItemList.Where(i=>i.UploadStatus==Naming.UploadStatusDefinition.資料錯誤 || i.UploadStatus==Naming.UploadStatusDefinition.匯入失敗).Count()  %></FooterTemplate>
        </asp:TemplateField>
    </Columns>
    <FooterStyle HorizontalAlign="Center" />
    <PagerStyle HorizontalAlign="Center" />
    <SelectedRowStyle />
    <HeaderStyle />
    <AlternatingRowStyle CssClass="OldLace" />
    <RowStyle />
    <EditRowStyle />
</asp:GridView>
<uc2:PagingControl ID="pagingList" runat="server" />
<center>
    <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"
        Text="查無資料!!" EnableViewState="false"></asp:Label>
</center>
