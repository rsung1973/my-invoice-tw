<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Base.InvoiceItemList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceDate.ascx" TagPrefix="uc2" TagName="InvoiceDate" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceSeller.ascx" TagPrefix="uc2" TagName="InvoiceSeller" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceSellerReceiptNo.ascx" TagPrefix="uc2" TagName="InvoiceSellerReceiptNo" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceNoPreview.ascx" TagPrefix="uc2" TagName="InvoiceNoPreview" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/TotalAmount.ascx" TagPrefix="uc2" TagName="TotalAmount" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/WinningInvoice.ascx" TagPrefix="uc2" TagName="WinningInvoice" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceDonation.ascx" TagPrefix="uc2" TagName="InvoiceDonation" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<!--表格 開始-->
<script type="text/javascript">
    $(function () {
        var chkBox = $("#gvEntity input[name='chkAll']");
        var chkItem = $("#gvEntity input[name='chkItem']");
        chkBox.click(
        function () {
            chkItem.prop('checked', chkBox.is(':checked'));
        });

        // To deselect CheckAll when a GridView CheckBox        
        // is unchecked        
        chkItem.click(
        function (e) {
            if (!$(this)[0].checked) {
                chkBox.prop("checked", false);
            }
        });
    });
</script>
<asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
    GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" ClientIDMode="Static"
    EnableViewState="False" DataSourceID="dsEntity" ShowFooter="True">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <input id="chkAll" name="chkAll" type="checkbox" />
            </HeaderTemplate>
            <ItemTemplate>
                <input id="chkItem" name="chkItem" type="checkbox" value='<%# ((CDS_Document)Container.DataItem).DocID  %>' />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="日期">
            <ItemTemplate>
                <uc2:InvoiceDate runat="server" ID="InvoiceDate" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="開立發票營業人">
            <ItemTemplate>
                <uc2:InvoiceSeller runat="server" ID="InvoiceSeller" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="統編">
            <ItemTemplate>
                <uc2:InvoiceSellerReceiptNo runat="server" ID="InvoiceSellerReceiptNo" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="發票">
            <ItemTemplate>
                <uc2:InvoiceNoPreview runat="server" ID="InvoiceNoPreview" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="金額">
            <ItemTemplate>
                <uc2:TotalAmount runat="server" ID="TotalAmount" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                共<%# _totalRecordCount %>筆
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="是否中獎" FooterText="總計金額：">
            <ItemTemplate>
                <uc2:WinningInvoice runat="server" ID="WinningInvoice" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="捐贈單位">
            <ItemTemplate>
                <uc2:InvoiceDonation runat="server" ID="InvoiceDonation" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                <%# String.Format("{0:##,###,###,##0}", _subtotal) %>
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
    <FooterStyle />
    <PagerStyle HorizontalAlign="Center" />
    <SelectedRowStyle />
    <HeaderStyle />
    <AlternatingRowStyle CssClass="OldLace" />
    <PagerTemplate>
        <uc2:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
    </PagerTemplate>
    <RowStyle />
    <EditRowStyle />
</asp:GridView>
<center>
    <span style="font-size:larger;color:red;"><%# _totalRecordCount==0 ? "查無資料":null %></span>
</center>
<cc1:DocumentDataSource ID="dsEntity" runat="server">
</cc1:DocumentDataSource>
