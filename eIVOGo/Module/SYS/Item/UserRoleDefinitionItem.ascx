<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserRoleDefinitionItem.ascx.cs"
    Inherits="eIVOGo.Module.SYS.Item.UserRoleDefinitionItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register src="../../Common/EnumSelector.ascx" tagname="EnumSelector" tagprefix="uc2" %>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
        <!--路徑名稱-->
        <!--交易畫面標題-->
        <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="使用者角色維護" />
        <!--按鈕-->
        <div class="border_gray">
            <asp:DetailsView ID="dvEntity" runat="server" AutoGenerateRows="False" DataKeyNames="RoleID"
                DataSourceID="dsRole" DefaultMode="Insert" Height="50px" OnDataBound="dvEntity_DataBound"
                OnItemCommand="dvEntity_ItemCommand" CssClass="left_title" GridLines="None"
                OnItemInserted="dvEntity_ItemInserted" OnItemInserting="dvEntity_ItemInserting"
                OnItemUpdated="dvEntity_ItemUpdated" OnItemUpdating="dvEntity_ItemUpdating">
                <Fields>
                    <asp:TemplateField HeaderText="RoleID" SortExpression="RoleID">
                        <EditItemTemplate>
                            <%# Eval("RoleID") %>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <uc2:EnumSelector ID="RoleID" runat="server" SelectedValue='<%# Bind("RoleID") %>' TypeName="Model.Locale.Naming+RoleID, Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
                        </InsertItemTemplate>
                        <HeaderStyle CssClass="th" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Role" HeaderText="角色名稱" SortExpression="Role" >
                    <HeaderStyle CssClass="th" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SiteMenu" HeaderText="選單名稱" 
                        SortExpression="SiteMenu" >
                    <HeaderStyle CssClass="th" />
                    </asp:BoundField>
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:Button ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                Text="確定" />
                            &nbsp;<asp:Button ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="取消" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:Button ID="btnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                                Text="確定" />
                            &nbsp;<asp:Button ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="取消" />
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                Text="修改" />
                            &nbsp;<asp:Button ID="btnNew" runat="server" CausesValidation="False" CommandName="New"
                                Text="新增" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Fields>
            </asp:DetailsView>
        </div>
        <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </asp:Panel>
</asp:Panel>
<asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnPopup"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" DropShadow="true"
    PopupDragHandleControlID="Panel3" />
<cc1:UserRoleDefinitionDataSource ID="dsRole" runat="server" Isolated="true">
</cc1:UserRoleDefinitionDataSource>
