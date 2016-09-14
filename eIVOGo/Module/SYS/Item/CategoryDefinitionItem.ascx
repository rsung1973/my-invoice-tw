<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryDefinitionItem.ascx.cs"
    Inherits="eIVOGo.Module.SYS.Item.CategoryDefinitionItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register src="~/Module/Common/EnumSelector.ascx" tagname="EnumSelector" tagprefix="uc2" %>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
        <!--路徑名稱-->
        <!--交易畫面標題-->
        <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="組織單位類別維護" />
        <!--按鈕-->
        <div class="border_gray">
            <asp:DetailsView ID="dvEntity" runat="server" AutoGenerateRows="False" DataKeyNames="CategoryID"
                DataSourceID="dsEntity" DefaultMode="Insert" Height="50px" OnItemCommand="dvEntity_ItemCommand"
                CssClass="left_title" GridLines="None" OnItemInserted="dvEntity_ItemInserted"
                OnItemInserting="dvEntity_ItemInserting" OnItemUpdated="dvEntity_ItemUpdated"
                OnItemUpdating="dvEntity_ItemUpdating">
                <Fields>
                    <asp:TemplateField HeaderText="CategoryID" SortExpression="CategoryID">
                        <EditItemTemplate>
                            <%# Eval("CategoryID") %>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <uc2:EnumSelector ID="CategoryID" runat="server" SelectedValue='<%# Bind("CategoryID") %>'  TypeName="Model.Locale.Naming+CategoryID, Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
                        </InsertItemTemplate>
                        <HeaderStyle CssClass="th" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Category" HeaderText="類別名稱" SortExpression="Category">
                    <HeaderStyle CssClass="th" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CharacterURL" HeaderText="Icon Url" SortExpression="CharacterURL">
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
<cc1:CategoryDefinitionDataSource ID="dsEntity" runat="server" Isolated="true">
</cc1:CategoryDefinitionDataSource>
