<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileList.ascx.cs"
    Inherits="eIVOGo.Module.ListView.UserProfileList" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc4" %>
<%@ Register Src="~/Module/Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc5" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
        GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" EnableViewState="False"
        DataSourceID="dsEntity" ShowFooter="True" DataKeyNames="UID">
        <Columns>
                    <asp:TemplateField HeaderText="公司名稱">
                        <ItemTemplate>
                            <%# ((UserProfile)Container.DataItem).UserRole.Count>0? ((UserProfile)Container.DataItem).UserRole[0].OrganizationCategory.Organization.CompanyName:null%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="角色">
                        <ItemTemplate>
                            <%# ((UserProfile)Container.DataItem).UserRole.Count>0?((UserProfile)Container.DataItem).UserRole[0].UserRoleDefinition.Role:null%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="會員姓名" DataField="UserName" ReadOnly="True" 
                        SortExpression="UserName" />
                    <asp:BoundField HeaderText="代號" DataField="PID" ReadOnly="True" 
                        SortExpression="PID" />
                    <asp:BoundField HeaderText="電子郵件" DataField="EMail" ReadOnly="True" 
                        SortExpression="EMail" />
                    <asp:TemplateField HeaderText="會員狀態">
                        <ItemTemplate>
                            <%# ((UserProfile)Container.DataItem).UserProfileStatus.LevelExpression.Description%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
            <asp:TemplateField ShowHeader="False" HeaderText="管理">
                <ItemTemplate>
                    <asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify"
                     Enabled="<%# ((UserProfile)Container.DataItem).UserProfileStatus.CurrentLevel != (int)Naming.MemberStatusDefinition.Mark_To_Delete%>"
                        Text="修改" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doEdit.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Visible="<%# ((UserProfile)Container.DataItem).UserProfileStatus.CurrentLevel != (int)Naming.MemberStatusDefinition.Mark_To_Delete%>"
                        Text="停用" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確認停用此筆資料?", String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                    <asp:Button ID="btnActivate" runat="server" CausesValidation="False" CommandName="Delete"
                        Visible="<%# ((UserProfile)Container.DataItem).UserProfileStatus.CurrentLevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete%>"
                        Text="啟用" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doActivate.GetConfirmedPostBackEventReference("確認啟用此筆資料?", String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSendMail" runat="server" Text="重送確認信" CausesValidation="False" CommandName="Modify"
                     Enabled="<%# ((UserProfile)Container.DataItem).UserProfileStatus.CurrentLevel != (int)Naming.MemberStatusDefinition.Mark_To_Delete%>"
                        CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doSendMail.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
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
<uc1:ActionHandler ID="doEdit" runat="server" />
<uc1:ActionHandler ID="doCreate" runat="server" />
<uc1:ActionHandler ID="doDelete" runat="server" />
<uc1:ActionHandler ID="doActivate" runat="server" />
<uc1:ActionHandler ID="doSendMail" runat="server" />
<uc4:DataModelCache ID="modelItem" runat="server" KeyName="UID" />
<uc5:PageAnchor ID="ToEdit" runat="server" TransferTo="~/SAM/EditUserProfile.aspx" />
<cc1:UserProfileDataSource ID="dsEntity" runat="server">
</cc1:UserProfileDataSource>