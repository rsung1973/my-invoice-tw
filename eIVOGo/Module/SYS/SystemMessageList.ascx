<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemMessageList.ascx.cs"
    Inherits="eIVOGo.Module.SYS.SystemMessageList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="Item/MessagesItem.ascx" TagName="MessagesItem" TagPrefix="uc2" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>

<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="MsgID" DataSourceID="dsEntity" EnableViewState="False" OnRowCommand="gvEntity_RowCommand"
        ShowFooter="True">
        <Columns>
            <asp:BoundField DataField="MessageContents" HeaderText="訊息內容" SortExpression="MessageContents" ItemStyle-Width="50%" />
            <asp:BoundField DataField="StartDate" HeaderText="訊息起始日" SortExpression="StartDate" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="EndDate" HeaderText="訊息迄止日" SortExpression="EndDate" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Center" />
            <asp:TemplateField HeaderText="永久顯示">
                <ItemTemplate>
                    <%# ((SystemMessage)Container.DataItem).AlwaysShow ? "是" : "否" %></ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify" Text="修改" CommandArgument='<%# Eval("MsgID") %>' />
                    &nbsp;<asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="刪除" CommandArgument='<%# Eval("MsgID") %>' OnClientClick='return confirm("確認刪除此筆資料?");' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <center><asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create" Text="新增" /></center>
        </EmptyDataTemplate>
        <PagerTemplate>
            <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
    </asp:GridView>
</div>
<cc1:SystemMessagesDataSource ID="dsEntity" runat="server">
</cc1:SystemMessagesDataSource>
<uc2:MessagesItem ID="MessagesItem" runat="server" />
<uc3:DataModelCache ID="modelItem" runat="server" KeyName="MsgID" />