<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CancelInvoiceItemList.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.CancelInvoiceItemList" %>
<%@ Register Src="PNewInvalidInvoicePreview.ascx" TagName="PNewInvalidInvoicePreview"
    TagPrefix="uc4" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc2" %>
<%@ Register Src="~/Module/Ajax/PagingControl.ascx" TagPrefix="uc1" TagName="PagingControl" %>
<%@ Register Src="~/Module/View/DataField/InvoiceNoPreview.ascx" TagPrefix="uc1" TagName="InvoiceNoPreview" %>

<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>

<!--表格 開始-->
<asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
    GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" ClientIDMode="Static"
    EnableViewState="False" DataSourceID="dsEntity" ShowFooter="True" ItemType="Model.DataEntity.InvoiceItem">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                全選<input id="chkAll" name="chkAll" type="checkbox" onclick="$('input[id$=\'chkItem\']').attr('checked', $('input[id$=\'chkAll\']').is(':checked'));" />
            </HeaderTemplate>
            <ItemTemplate>
                <input id="chkItem" name="chkItem" type="checkbox" value='<%# ((InvoiceItem)Container.DataItem).InvoiceID  %>' />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="作廢原因">
            <ItemTemplate>
                <input class='textfield' type='text' name='cancelReason' />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="日期">
            <ItemTemplate>
                <%# ValueValidity.ConvertChineseDateString(((InvoiceItem)Container.DataItem).InvoiceDate)%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="開立發票營業人">
            <ItemTemplate>
                <%# ((InvoiceItem)Container.DataItem).CDS_Document.DocumentOwner.Organization.CompanyName%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="發票買受人">
            <ItemTemplate>
                <%# ((InvoiceItem)Container.DataItem).InvoiceBuyer.CustomerName %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                共<%# this.Select().Count() %>筆
            </FooterTemplate>
            <FooterStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="發票號碼" FooterText="總計金額：">
            <ItemTemplate>
                <uc1:InvoiceNoPreview runat="server" ID="InvoiceNoPreview" Item="<%# Item %>" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="金額">
            <ItemTemplate>
                <%#String.Format("{0:0,0.00}", ((InvoiceItem)Container.DataItem).InvoiceAmountType.TotalAmount)%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                <%# String.Format("{0:##,###,###,##0}", this.Select().Sum(i=>i.InvoiceAmountType.TotalAmount)) %>
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
    <FooterStyle />
    <PagerStyle HorizontalAlign="Center" />
    <SelectedRowStyle />
    <HeaderStyle />
    <AlternatingRowStyle CssClass="OldLace" />
    <PagerTemplate>
    </PagerTemplate>
    <RowStyle />
    <EditRowStyle />
    <EmptyDataTemplate>
        <center><font color="red">查無資料!!</font></center>
    </EmptyDataTemplate>
</asp:GridView>
<uc1:PagingControl runat="server" ID="pagingList" />

<!--按鈕-->

<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
<uc1:DataModelCache ID="rejectInput" runat="server" KeyName="rejectInput" />
<uc1:DataModelCache ID="rejectItem" runat="server" KeyName="rejectItem" />
<uc2:PageAnchor ID="NextAction" runat="server" RedirectTo="CancelInvoicePreview.aspx" />
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += module_eivo_cancelinvoiceitemlist_ascx_PreRender;
    }

    void module_eivo_cancelinvoiceitemlist_ascx_PreRender(object sender, EventArgs e)
    {
        pagingList.RecordCount = this.Select().Count();
        gvEntity.PageIndex = pagingList.CurrentPageIndex;
        pagingList.Visible = pagingList.RecordCount > 0;
    }

    protected override void gvEntity_DataBound(object sender, EventArgs e)
    {

    }

</script>
