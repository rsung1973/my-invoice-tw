<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceTrackCodeList.ascx.cs"
    Inherits="eIVOGo.Module.Base.InvoiceTrackCodeList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<!--表格 開始-->
<asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
    GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" ClientIDMode="Static"
    EnableViewState="False" DataSourceID="dsEntity" onrowdatabound="gvEntity_RowDataBound">
    <Columns>
        <asp:TemplateField HeaderText="發票年度">
            <ItemTemplate>
                <%# ((InvoiceTrackCode)Container.DataItem).Year-1911%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="發票期別">
            <ItemTemplate>
                <%# String.Format("{0:00}",((InvoiceTrackCode)Container.DataItem).PeriodNo*2-1)%>~<%# String.Format("{0:00}",((InvoiceTrackCode)Container.DataItem).PeriodNo*2)%>月</ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="字軌">
            <ItemTemplate>
                <%# ((InvoiceTrackCode)Container.DataItem).TrackCode%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
    </Columns>
    <FooterStyle />
    <PagerStyle HorizontalAlign="Center" />
    <SelectedRowStyle />
    <HeaderStyle />
    <AlternatingRowStyle CssClass="OldLace" />
    <PagerTemplate>
        <uc2:PagingControl ID="pagingList" runat="server" />
    </PagerTemplate>
    <RowStyle />
    <EditRowStyle />
</asp:GridView>
<center>
        <span style="font-size:larger;color:red;"><%# _totalRecordCount==0 ? "查無資料":null %></span>
</center>
<cc1:InvoiceTrackCodeDataSource ID="dsEntity" runat="server">
</cc1:InvoiceTrackCodeDataSource>
