<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuControlList.ascx.cs"
    Inherits="eIVOGo.Module.SYS.MenuControlList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/MenuControlItem.ascx" TagName="MenuControlItem" TagPrefix="uc2" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="MenuID" DataSourceID="dsEntity" EnableViewState="False" OnRowCommand="gvEntity_RowCommand"
        ShowFooter="True">
        <Columns>
            <asp:BoundField DataField="MenuID" HeaderText="MenuID" ReadOnly="True" SortExpression="MenuID"
                InsertVisible="False" />
            <asp:TemplateField HeaderText="選單名稱" SortExpression="SiteMenu">
                <ItemTemplate>
                    <asp:LinkButton ID="lbDownload" runat="server" Text='<%# Eval("SiteMenu") %>' CommandName="Download"
                        CommandArgument='<%# Eval("SiteMenu") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify"
                        Text="修改" CommandArgument='<%# Eval("MenuID") %>' />
                    &nbsp;<asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="刪除" CommandArgument='<%# Eval("MenuID") %>' OnClientClick='return confirm("確認刪除此筆資料?");' />
                    &nbsp;<asp:Button ID="btnOnline" runat="server" CausesValidation="False" CommandName="Online"
                        Text="編輯選單" CommandArgument='<%# Eval("SiteMenu") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <font color="red">查無資料!!</font>
        </EmptyDataTemplate>
        <PagerTemplate>
            <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
    </asp:GridView>
</div>
<cc1:MenuControlDataSource ID="dsEntity" runat="server">
</cc1:MenuControlDataSource>
<uc2:MenuControlItem ID="menuItem" runat="server" />
