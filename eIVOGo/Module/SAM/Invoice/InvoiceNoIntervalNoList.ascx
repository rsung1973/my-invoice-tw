<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceNoIntervalNoList.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Invoice.InvoiceNoIntervalNoList" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc4" %>
<%@ Register Src="~/Module/Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc5" %>
<%@ Register src="EditInvoiceNoIntervalModal.ascx" tagname="EditInvoiceNoIntervalModal" tagprefix="uc3" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
        GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" EnableViewState="False"
        DataSourceID="dsEntity" ShowFooter="True" DataKeyNames="IntervalID">
        <Columns>
            <asp:TemplateField HeaderText="發票年度" SortExpression="IntervalID">
                <ItemTemplate>
                    <%# ((InvoiceNoInterval)Container.DataItem).InvoiceTrackCodeAssignment.InvoiceTrackCode.Year %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="發票期別" SortExpression="">
                <ItemTemplate>
                    <%# String.Format("{0:00}-{1:00}月",((InvoiceNoInterval)Container.DataItem).InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo*2-1,((InvoiceNoInterval)Container.DataItem).InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo*2) %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="字軌" SortExpression="TrackID">
                <ItemTemplate>
                    <%# ((InvoiceNoInterval)Container.DataItem).InvoiceTrackCodeAssignment.InvoiceTrackCode.TrackCode %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="StartNo" HeaderText="發票號碼起" ItemStyle-HorizontalAlign="Center"
                SortExpression="StartNo" DataFormatString="{0:00000000}">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="EndNo" HeaderText="發票號碼迄" SortExpression="EndNo" ItemStyle-HorizontalAlign="Center"
                DataFormatString="{0:00000000}">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField ShowHeader="False" HeaderText="操作">
                <ItemTemplate>
                    <asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify"
                        Text="編輯" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        Enabled='<%# ((InvoiceNoInterval)Container.DataItem).InvoiceNoAssignments.Count==0 %>'
                        OnClientClick='<%# doEdit.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="刪除" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        Enabled='<%# ((InvoiceNoInterval)Container.DataItem).InvoiceNoAssignments.Count==0 %>'
                        OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確認刪除此筆資料?", String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
        <FooterStyle CssClass="total-count" />
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
<cc1:InvoiceNoIntervalDataSource ID="dsEntity" runat="server">
</cc1:InvoiceNoIntervalDataSource>
<uc1:ActionHandler ID="doEdit" runat="server" />
<uc1:ActionHandler ID="doDelete" runat="server" />
<uc4:DataModelCache ID="modelItem" runat="server" KeyName="IntervalID" />
<uc5:PageAnchor ID="ToEdit" runat="server" TransferTo="~/SAM/EditInvoiceNoInterval.aspx" />
<uc3:EditInvoiceNoIntervalModal ID="editItemModal" runat="server" Visible="false" />

