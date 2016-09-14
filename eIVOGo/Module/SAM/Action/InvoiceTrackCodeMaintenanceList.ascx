<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceTrackCodeMaintenanceList.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Action.InvoiceTrackCodeMaintenanceList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<!--表格 開始-->
<asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
    GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" ClientIDMode="Static"
    EnableViewState="False" DataSourceID="dsEntity">
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
                <%# ((InvoiceTrackCode)Container.DataItem).TrackCode%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False" HeaderText="管理">
            <ItemTemplate>
                <asp:Button ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit"
                    Enabled='<%# ((InvoiceTrackCode)Container.DataItem).InvoiceTrackCodeAssignments.Count==0 %>'
                    Text="修改" OnClientClick='<%# Page.ClientScript.GetPostBackEventReference(this, String.Format("U:{0}",((InvoiceTrackCode)Container.DataItem).TrackID)) + "; return false;" %>' />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete"
                    Text='刪除' OnClientClick='<%# String.Format("if(confirm(\"確定要刪除該字軌資料?\")) {0}; return false; " , Page.ClientScript.GetPostBackEventReference(this, String.Format("D:{0}",((InvoiceTrackCode)Container.DataItem).TrackID))) %>'
                    Enabled='<%# ((InvoiceTrackCode)Container.DataItem).InvoiceTrackCodeAssignments.Count==0 %>' />
            </ItemTemplate>
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
    <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"
        Text="查無資料!!" EnableViewState="false"></asp:Label>
</center>
<cc1:InvoiceTrackCodeDataSource ID="dsEntity" runat="server">
</cc1:InvoiceTrackCodeDataSource>
