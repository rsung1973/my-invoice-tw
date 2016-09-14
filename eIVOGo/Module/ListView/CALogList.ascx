<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CALogList.ascx.cs" Inherits="eIVOGo.Module.ListView.CALogList" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Register src="../Entity/CALogItem.ascx" tagname="CALogItem" tagprefix="uc3" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
        GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" EnableViewState="False"
        DataSourceID="dsEntity" ShowFooter="True" DataKeyNames="LogID">
        <Columns>
            <asp:TemplateField HeaderText="營業人統編 " SortExpression="DocID">
                <ItemTemplate>
                    <%# ((CALog)Container.DataItem).CDS_Document.DocumentOwner.Organization.ReceiptNo %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="營業人名稱 " SortExpression="DocID">
                <ItemTemplate>
                    <%# ((CALog)Container.DataItem).CDS_Document.DocumentOwner.Organization.CompanyName %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="憑證作業時間" SortExpression="LogDate">
                <ItemTemplate>
                    <%# ValueValidity.ConvertChineseDateTimeString(((CALog)Container.DataItem).LogDate) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="憑證內容" SortExpression="ContentPath">
                <ItemTemplate>
                    <asp:ImageButton ID="imgBtn" runat="server" ImageUrl="~/images/icon_ca.gif" OnClientClick='<%# doDisplayContent.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
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
<uc1:ActionHandler ID="doDisplayContent" runat="server" />
<cc1:CALogDataSource ID="dsEntity" runat="server">
</cc1:CALogDataSource>
<uc3:CALogItem ID="logView" runat="server" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        doDisplayContent.DoAction = arg =>
        {
            logView.QueryExpr = o => o.LogID == int.Parse(arg);
            logView.BindData();
        };
        
    }
</script>
