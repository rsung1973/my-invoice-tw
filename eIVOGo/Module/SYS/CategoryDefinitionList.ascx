<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryDefinitionList.ascx.cs"
    Inherits="eIVOGo.Module.SYS.CategoryDefinitionList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/CategoryDefinitionItem.ascx" TagName="CategoryDefinitionItem"
    TagPrefix="uc2" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="CategoryID" DataSourceID="dsEntity" EnableViewState="False" OnRowCommand="gvEntity_RowCommand"
        ShowFooter="True">
        <Columns>
            <asp:BoundField DataField="CategoryID" HeaderText="CategoryID" ReadOnly="True" SortExpression="CategoryID" />
            <asp:BoundField DataField="Category" HeaderText="類別名稱" SortExpression="Category" />
            <asp:BoundField DataField="CharacterURL" HeaderText="Icon Url" SortExpression="CharacterURL" />
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify"
                        Text="修改" CommandArgument='<%# Eval(gvEntity.DataKeyNames[0]) %>' />
                    &nbsp;<asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="刪除" CommandArgument='<%# Eval(gvEntity.DataKeyNames[0]) %>' OnClientClick='return confirm("確認刪除此筆資料?");' />
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
<cc1:CategoryDefinitionDataSource ID="dsEntity" runat="server">
</cc1:CategoryDefinitionDataSource>
<uc2:CategoryDefinitionItem ID="editItem" runat="server" />
