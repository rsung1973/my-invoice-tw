<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileItem.ascx.cs"
    Inherits="eIVOGo.Module.SYS.Item.UserProfileItem" %>
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
        <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="員工帳號管理維護" />
        <!--按鈕-->
        <div class="border_gray">
            <asp:DetailsView ID="dvEntity" runat="server" AutoGenerateRows="False" DataKeyNames="UID"
                DataSourceID="dsEntity" DefaultMode="Insert" Height="50px" OnItemCommand="dvEntity_ItemCommand"
                CssClass="left_title" GridLines="None" OnItemInserted="dvEntity_ItemInserted"
                OnItemInserting="dvEntity_ItemInserting" OnItemUpdated="dvEntity_ItemUpdated"
                OnItemUpdating="dvEntity_ItemUpdating">
                <Fields>
                    <asp:BoundField DataField="PID" HeaderText="PID" SortExpression="PID">
                    <HeaderStyle CssClass="th" />
                    <ItemStyle CssClass="tdleft" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UserName" HeaderText="姓名" SortExpression="UserName">
                    <HeaderStyle CssClass="th" />
                    <ItemStyle CssClass="tdleft" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="密碼" SortExpression="Password">
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Password") %>' 
                                TextMode="Password"></asp:TextBox>
                        </InsertItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Password") %>' 
                                TextMode="Password"></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle CssClass="th" />
                        <ItemStyle CssClass="tdleft" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="ContactTitle" HeaderText="稱謂" SortExpression="ContactTitle">
                        <HeaderStyle CssClass="th" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Address" HeaderText="地址" SortExpression="Address">
                    <HeaderStyle CssClass="th" />
                    <ItemStyle CssClass="tdleft" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Phone" HeaderText="電話" SortExpression="Phone" >
                        <HeaderStyle CssClass="th" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MobilePhone" HeaderText="行動電話" 
                        SortExpression="MobilePhone" >
                    <HeaderStyle CssClass="th" />
                    <ItemStyle CssClass="tdleft" />
                    </asp:BoundField>
                    <asp:BoundField DataField="EMail" HeaderText="EMail" SortExpression="EMail" >
                    <HeaderStyle CssClass="th" />
                    <ItemStyle CssClass="tdleft" />
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
<cc1:UserProfileDataSource ID="dsEntity" runat="server" Isolated="true">
</cc1:UserProfileDataSource>
