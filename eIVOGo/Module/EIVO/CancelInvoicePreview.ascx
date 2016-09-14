<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CancelInvoicePreview.ascx.cs"
    Inherits="EIVO07Tools.Module.EIVO.CancelInvoicePreview" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register src="../Common/DataModelCache.ascx" tagname="DataModelCache" tagprefix="uc1" %>
<%@ Import Namespace="Utility" %>

<uc5:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 開立作廢電子發票" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="開立作廢電子發票" />
<div class="border_gray">
    <!--表格 開始-->
    <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
        GridLines="None" CellPadding="0" CssClass="table01" ClientIDMode="Static"
        EnableViewState="False" DataSourceID="dsInv" ShowFooter="True">
        <Columns>
            <asp:TemplateField HeaderText="作廢原因">
                <ItemTemplate>
                    <%# inputValues != null && inputValues.ContainsKey(((InvoiceItem)Container.DataItem).InvoiceID) ? inputValues[((InvoiceItem)Container.DataItem).InvoiceID] : null%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="日期">
                <ItemTemplate>
                    <%# ValueValidity.ConvertChineseDateString(((InvoiceItem)Container.DataItem).InvoiceDate)%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
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
            </asp:TemplateField>
            <asp:TemplateField HeaderText="發票號碼">
                <ItemTemplate>
                    <%# ((InvoiceItem)Container.DataItem).GetMaskInvoiceNo() %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="金額">
                <ItemTemplate>
                    <%#String.Format("{0:0,0.00}", ((InvoiceItem)Container.DataItem).InvoiceAmountType.TotalAmount)%></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
        <FooterStyle />
        <PagerStyle HorizontalAlign="Center" />
        <SelectedRowStyle />
        <HeaderStyle />
        <AlternatingRowStyle CssClass="OldLace" />
        <RowStyle />
        <EditRowStyle />
    </asp:GridView>
    <!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnBack" runat="server" CssClass="btn" Text="回上頁" />&nbsp;&nbsp;
            <asp:Button ID="btnCancelCreat" CssClass="btn" runat="server" Text="開立作廢發票" 
                onclick="btnCancelCreat_Click" />
        </td>
    </tr>
</table>
<cc1:InvoiceDataSource ID="dsInv" runat="server">
</cc1:InvoiceDataSource>
<uc1:DataModelCache ID="rejectInput" runat="server" KeyName="rejectInput" />
<uc1:DataModelCache ID="rejectItem" runat="server" KeyName="rejectItem" />