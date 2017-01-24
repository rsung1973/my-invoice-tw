<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintCInvoiceItemList.ascx.cs" Inherits="eIVOGo.Module.Inquiry.PrintCInvoiceItemList" %>
<%@ Register src="../Common/DataModelCache.ascx" tagname="DataModelCache" tagprefix="uc1" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Src="~/Module/EIVO/PNewInvalidInvoicePreview.ascx" TagName="PNewInvalidInvoicePreview" TagPrefix="uc4" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<!--表格 開始-->
<asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
    GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" ClientIDMode="Static"
    EnableViewState="False" DataSourceID="dsInv" ShowFooter="True">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                全選<input id="chkAll" name="chkAll" type="checkbox" onclick="$('input[id$=\'chkItem\']').attr('checked',$('input[id$=\'chkAll\']').is(':checked'));" />
            </HeaderTemplate>
            <ItemTemplate>
                <input id="chkItem" name="chkItem" type="checkbox" value='<%# ((InvoiceItem)Container.DataItem).InvoiceID  %>' />
            </ItemTemplate>
            <FooterTemplate><input id="printBack" name="printBack" type="checkbox" value="True" <%# String.IsNullOrEmpty(Request["printBack"])?"":"checked=\"checked\""  %> /></FooterTemplate>
            <FooterStyle HorizontalAlign="Center" />
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="日期">
            <ItemTemplate>
                <%# ValueValidity.ConvertChineseDateString(((InvoiceItem)Container.DataItem).InvoiceDate)%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>列印發票背面</FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="開立發票營業人">
            <ItemTemplate>
                <%# ((InvoiceItem)Container.DataItem).CDS_Document.DocumentOwner.Organization.CompanyName%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="發票買受人">
            <ItemTemplate>
                <%# ((InvoiceItem)Container.DataItem).InvoiceBuyer.CustomerName %></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                共<%# _totalRecordCount %>筆
            </FooterTemplate>
            <FooterStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="發票號碼" FooterText="總計金額：">
            <ItemTemplate>
                <asp:LinkButton ID="lbtn" runat="server" Text='<%# ((InvoiceItem)Container.DataItem).GetMaskInvoiceNo() %>'
                    CausesValidation="false" CommandName="Edit" OnClientClick='<%# Page.ClientScript.GetPostBackEventReference(this, String.Format("S:{0}",((InvoiceItem)Container.DataItem).InvoiceID)) + "; return false;" %>' />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="金額">
            <ItemTemplate>
                <%#String.Format("{0:0,0.00}", ((InvoiceItem)Container.DataItem).InvoiceAmountType.TotalAmount)%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                <%# String.Format("{0:##,###,###,##0}", _subtotal) %></FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="列印狀態">
            <ItemTemplate>
                <%# ((InvoiceItem)Container.DataItem).CDS_Document.DocumentPrintLog.Count > 0 ? "已列印" : "未列印"%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
    </Columns>
    <FooterStyle />
    <EmptyDataTemplate>
        <%# doEmptyDataHandler() %>
    </EmptyDataTemplate>
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
    <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"
        Text="查無資料!!" EnableViewState="false"></asp:Label>
</center>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <br /><asp:Button ID="btnPrint" runat="server" Text="列印" onclick="btnPrint_Click" />
        </td>
    </tr>
</table>
<cc1:InvoiceDataSource ID="dsInv" runat="server">
</cc1:InvoiceDataSource>
<uc4:PNewInvalidInvoicePreview ID="PNewInvalidInvoicePreview1" runat="server" />



