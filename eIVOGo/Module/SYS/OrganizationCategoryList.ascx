<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationCategoryList.ascx.cs"
    Inherits="eIVOGo.Module.SYS.OrganizationCategoryList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/OrganizationCategoryItem.ascx" TagName="OrganizationCategoryItem"
    TagPrefix="uc2" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="OrgaCateID" DataSourceID="dsEntity" EnableViewState="False" OnRowCommand="gvEntity_RowCommand"
        ShowFooter="True">
        <Columns>
            <asp:BoundField DataField="OrgaCateID" HeaderText="OrgaCateID" ReadOnly="True" SortExpression="OrgaCateID"
                InsertVisible="False" />
            <asp:TemplateField HeaderText="公司名稱" SortExpression="CompanyID">
                <ItemTemplate>
                    <%# ((OrganizationCategory)Container.DataItem).Organization.CompanyName %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="公司類別" SortExpression="CategoryID">
                <ItemTemplate>
                    <%# ((OrganizationCategory)Container.DataItem).CategoryDefinition.Category %>
                </ItemTemplate>
            </asp:TemplateField>
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
<cc1:OrganizationCategoryDataSource ID="dsEntity" runat="server">
</cc1:OrganizationCategoryDataSource>
<uc2:OrganizationCategoryItem ID="editItem" runat="server" />
