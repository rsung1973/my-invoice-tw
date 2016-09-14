<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationCategoryItem.ascx.cs"
    Inherits="eIVOGo.Module.SYS.Item.OrganizationCategoryItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
        <!--路徑名稱-->
        <!--交易畫面標題-->
        <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="公司類別套用維護" />
        <!--按鈕-->
        <div class="border_gray">
            <asp:DetailsView ID="dvEntity" runat="server" AutoGenerateRows="False" DataKeyNames="OrgaCateID"
                DataSourceID="dsEntity" DefaultMode="Insert" Height="50px" OnItemCommand="dvEntity_ItemCommand"
                CssClass="left_title" GridLines="None" OnItemInserted="dvEntity_ItemInserted"
                OnItemInserting="dvEntity_ItemInserting" OnItemUpdated="dvEntity_ItemUpdated"
                OnItemUpdating="dvEntity_ItemUpdating">
                <Fields>
                    <asp:BoundField DataField="OrgaCateID" HeaderText="OrgaCateID" ReadOnly="True" 
                        SortExpression="OrgaCateID" InsertVisible="False">
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="公司名稱" SortExpression="CompanyID">
                        <InsertItemTemplate>
                            <asp:DropDownList ID="CompanyID" runat="server" DataSourceID="dsOrg" SelectedValue='<%# Bind("CompanyID") %>' DataTextField="CompanyName" DataValueField="CompanyID" >
                            </asp:DropDownList>
                        </InsertItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="CompanyID" runat="server" DataSourceID="dsOrg" SelectedValue='<%# Bind("CompanyID") %>' DataTextField="CompanyName" DataValueField="CompanyID" >
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <HeaderStyle CssClass="th" />
                        <ItemStyle CssClass="tdleft" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="公司類別" SortExpression="CategoryID">
                        <InsertItemTemplate>
                            <asp:DropDownList ID="CategoryID" runat="server" DataSourceID="dsCategory" SelectedValue='<%# Bind("CategoryID") %>' DataTextField="Category" DataValueField="CategoryID" >
                            </asp:DropDownList>
                        </InsertItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="CategoryID" runat="server" DataSourceID="dsCategory" SelectedValue='<%# Bind("CategoryID") %>' DataTextField="Category" DataValueField="CategoryID" >
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <HeaderStyle CssClass="th" />
                        <ItemStyle CssClass="tdleft" />
                    </asp:TemplateField>
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
<cc1:OrganizationCategoryDataSource ID="dsEntity" runat="server" Isolated="true">
</cc1:OrganizationCategoryDataSource>
<cc1:OrganizationDataSource ID="dsOrg" runat="server" Isolated="true">
</cc1:OrganizationDataSource>
<cc1:CategoryDefinitionDataSource ID="dsCategory" runat="server" Isolated="true">
</cc1:CategoryDefinitionDataSource>
