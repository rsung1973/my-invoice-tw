<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileList.ascx.cs"
    Inherits="eIVOGo.Module.SYS.UserProfileList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/UserProfileItem.ascx" TagName="UserProfileItem" TagPrefix="uc2" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="UID" DataSourceID="dsEntity" EnableViewState="False" OnRowCommand="gvEntity_RowCommand"
        ShowFooter="False">
        <Columns>
            <asp:BoundField DataField="PID" HeaderText="PID" SortExpression="PID" />
            <asp:BoundField DataField="UserName" HeaderText="姓名" SortExpression="UserName" />
            <asp:BoundField DataField="EMail" HeaderText="EMail" SortExpression="EMail" />
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnUserRole" runat="server" CausesValidation="False" CommandName="EditRole"
                        Text="角色設定" CommandArgument='<%# Eval(gvEntity.DataKeyNames[0]) %>' />
                    <%--                &nbsp;
                <asp:Button ID="btnModify" runat="server" CausesValidation="False" 
                    CommandName="Modify" Text="修改" CommandArgument='<%# Eval(gvEntity.DataKeyNames[0]) %>' />
                &nbsp;<asp:Button ID="btnDelete" runat="server" CausesValidation="False" 
                    CommandName="Delete" Text="刪除" CommandArgument='<%# Eval(gvEntity.DataKeyNames[0]) %>' OnClientClick='return confirm("確認刪除此筆資料?");' />
                    --%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <font color="red">查無資料!!</font>
            <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                Text="新增" />
        </EmptyDataTemplate>
        <PagerTemplate>
            <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
    </asp:GridView>
</div>
<cc1:UserProfileDataSource ID="dsEntity" runat="server">
</cc1:UserProfileDataSource>
<uc2:UserProfileItem ID="editItem" runat="server" />
