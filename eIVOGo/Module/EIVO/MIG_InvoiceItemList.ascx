<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MIG_InvoiceItemList.ascx.cs" Inherits="eIVOGo.Module.EIVO.MIG_InvoiceItemList" %>

<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/View/DataField/InvoiceNoPreview.ascx" TagPrefix="uc1" TagName="InvoiceNoPreview" %>
<%@ Register Src="~/Module/Ajax/PagingControl.ascx" TagPrefix="uc1" TagName="PagingControl" %>


<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<!--表格 開始-->
<asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
    GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" ClientIDMode="Static"
    EnableViewState="False" DataSourceID="dsEntity" ShowFooter="True" DataKeyNames="InvoiceID" ItemType="Model.DataEntity.InvoiceItem">
    <Columns>
        <asp:TemplateField HeaderText="日期">
            <ItemTemplate>
                <%# ValueValidity.ConvertChineseDateString(((InvoiceItem)Container.DataItem).InvoiceDate)%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="客戶 ID / Google ID" Visible="false">
            <ItemTemplate><%# ((InvoiceItem)Container.DataItem).InvoiceBuyer.CustomerID%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="序號">
            <ItemTemplate><%# ((InvoiceItem)Container.DataItem).InvoicePurchaseOrder == null ? "" : ((InvoiceItem)Container.DataItem).InvoicePurchaseOrder.OrderNo%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="開立發票營業人">
            <ItemTemplate>
                <%# ((InvoiceItem)Container.DataItem).InvoiceSeller.Organization.CompanyName%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="統編">
            <ItemTemplate>
                <%# ((InvoiceItem)Container.DataItem).InvoiceSeller.Organization.ReceiptNo%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="發票">
            <ItemTemplate>
                <uc1:InvoiceNoPreview runat="server" ID="InvoiceNoPreview" Item="<%# Item %>" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="未稅金額">
            <ItemTemplate><%#String.Format("{0:0,0.00}", ((InvoiceItem)Container.DataItem).InvoiceAmountType.SalesAmount)%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="稅額">
            <ItemTemplate><%#String.Format("{0:0,0.00}", ((InvoiceItem)Container.DataItem).InvoiceAmountType.TaxAmount)%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="含稅金額">
            <ItemTemplate><%#String.Format("{0:0,0.00}", ((InvoiceItem)Container.DataItem).InvoiceAmountType.TotalAmount)%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="是否中獎">
            <ItemTemplate>
                <%# ((InvoiceItem)Container.DataItem).InvoiceWinningNumber != null ? ((InvoiceItem)Container.DataItem).InvoiceWinningNumber.UniformInvoiceWinningNumber.PrizeType : "N/A"%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>共<%# Select().Count() %>筆</FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="捐贈愛心碼" Visible="false">
            <ItemTemplate>
                <%# Item.InvoiceDonation!=null ? Item.InvoiceDonation.AgencyCode : null%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="買受人統編" FooterText="總計金額：">
            <ItemTemplate><%# Item.InvoiceBuyer.IsB2C() ? "N/A" : Item.InvoiceBuyer.ReceiptNo %></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="備註">
            <ItemTemplate><%# String.Join("",((InvoiceItem)Container.DataItem).InvoiceDetails.Select(d=> d.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark))%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                <%# String.Format("{0:##,###,###,##0}", Select().Sum(i => i.InvoiceAmountType.TotalAmount)) %>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="簡訊通知">
            <ItemTemplate><%# Item.Organization.OrganizationStatus.SetToNotifyCounterpartBySMS==true ? "是" : "否"%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="載具資訊">
            <ItemTemplate><%# ((InvoiceItem)Container.DataItem).InvoiceCarrier == null ? null : ((InvoiceItem)Container.DataItem).InvoiceCarrier.CarrierNo%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="資料作業">
            <ItemTemplate>
                <input type="button" name="btnMIG" value="MIG下載" class="btn" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <FooterStyle />
    <EmptyDataTemplate>
        <center><font color="red">查無資料!!</font></center>
    </EmptyDataTemplate>
    <PagerStyle HorizontalAlign="Center" />
    <SelectedRowStyle />
    <HeaderStyle />
    <AlternatingRowStyle CssClass="OldLace" />
    <PagerTemplate>
    </PagerTemplate>
    <RowStyle />
    <EditRowStyle />
</asp:GridView>
<uc1:PagingControl runat="server" ID="pagingList" />
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>

<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += module_eivo_mig_invoiceitemlist_ascx_PreRender;
    }

    void module_eivo_mig_invoiceitemlist_ascx_PreRender(object sender, EventArgs e)
    {
        pagingList.RecordCount = this.Select().Count();
        gvEntity.PageIndex = pagingList.CurrentPageIndex;
        pagingList.Visible = pagingList.RecordCount > 0;
    }

    protected override void gvEntity_DataBound(object sender, EventArgs e)
    {

    }    

</script>
