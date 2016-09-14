<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UnusedInvoiceItemList.ascx.cs"
    Inherits="eIVOGo.Module.SYS.UnusedInvoiceItemList" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/CalendarInput.ascx" TagName="CalendarInput" TagPrefix="uc1" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc3" %>
<%@ Register Src="../UI/InvoiceSellerSelector.ascx" TagName="SellerSelector" TagPrefix="uc4" %>
<%@ Register Src="../UI/TwiceMonthlyPeriod.ascx" TagName="TwiceMonthlyPeriod" TagPrefix="uc5" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc6" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc7" %>
<%@ Register src="../UI/QuerySellerSelector.ascx" tagname="InvoiceSellerSelector" tagprefix="uc8" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="Model.DataEntity" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
        GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" ClientIDMode="Static"
        DataSourceID="dsEntity" EnableViewState="False" DataKeyNames="UAID" ShowFooter="True">
        <Columns>
            <asp:TemplateField HeaderText="祇布">
                <ItemTemplate>
                    <%# ((UnassignedInvoiceNo)Container.DataItem).InvoiceTrackCodeAssignment.InvoiceTrackCode.Year - 1911%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="祇布戳"> 
                <ItemTemplate>
                    <%# ((UnassignedInvoiceNo)Container.DataItem).InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo%>
                </ItemTemplate>  
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="瓂">
                <ItemTemplate>
                    <%# ((UnassignedInvoiceNo)Container.DataItem).InvoiceTrackCodeAssignment.InvoiceTrackCode.TrackCode%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="祇布腹絏癬">
                <ItemTemplate>
                    <%# ((UnassignedInvoiceNo)Container.DataItem).InvoiceBeginNo.ToString("00000000")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="祇布腹絏ù">
                <ItemTemplate>
                    <%# ((UnassignedInvoiceNo)Container.DataItem).InvoiceEndNo.ToString("00000000")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
        <FooterStyle />
        <EmptyDataTemplate>
            <center><font color="red">琩礚戈!! </font></center>
        </EmptyDataTemplate>
        <EmptyDataRowStyle CssClass="noborder" />
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
</div>
<cc1:UnassignedInvoiceNoDataSource ID="dsEntity" runat="server">
</cc1:UnassignedInvoiceNoDataSource>