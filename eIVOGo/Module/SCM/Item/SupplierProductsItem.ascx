<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierProductsItem.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Item.SupplierProductsItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
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
            <asp:DetailsView ID="dvEntity" runat="server" AutoGenerateRows="False" DataKeyNames="SUPPLIER_SN,PRODUCTS_SN"
                DataSourceID="dsEntity" DefaultMode="Insert" Height="50px" OnItemCommand="dvEntity_ItemCommand"
                CssClass="left_title" GridLines="None" OnItemInserted="dvEntity_ItemInserted"
                OnItemInserting="dvEntity_ItemInserting" OnItemUpdated="dvEntity_ItemUpdated"
                OnItemUpdating="dvEntity_ItemUpdating">
                <Fields>
                    <asp:TemplateField HeaderText="料品" SortExpression="PRODUCTS_SN">
                        <EditItemTemplate>
                            <asp:DropDownList ID="PRODUCTS_SN" runat="server" DataSourceID="dsProd" DataTextField="PRODUCTS_NAME"
                                DataValueField="PRODUCTS_SN" SelectedValue='<%# Eval("PRODUCTS_SN") %>'>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DropDownList ID="PRODUCTS_SN" runat="server" DataSourceID="dsProd" DataTextField="PRODUCTS_NAME"
                                DataValueField="PRODUCTS_SN" onprerender="RPODUCTS_SN_PreRender" 
                                ondatabound="PRODUCTS_SN_DataBound">
                            </asp:DropDownList>
                        </InsertItemTemplate>
                        <HeaderStyle CssClass="th" />
                        <ItemStyle CssClass="tdleft" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="供應商" SortExpression="SUPPLIER_SN">
                        <EditItemTemplate>
                            <asp:DropDownList ID="SUPPLIER_SN" runat="server" DataSourceID="dsSupplier" DataTextField="SUPPLIER_NAME"
                                DataValueField="SUPPLIER_SN">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DropDownList ID="SUPPLIER_SN" runat="server" DataSourceID="dsSupplier" DataTextField="SUPPLIER_NAME"
                                DataValueField="SUPPLIER_SN">
                            </asp:DropDownList>
                        </InsertItemTemplate>
                        <HeaderStyle CssClass="th" />
                        <ItemStyle CssClass="tdleft" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="SUPPLIER_PRODUCTS_NUMBER1" HeaderText="貨號" SortExpression="SUPPLIER_PRODUCTS_NUMBER">
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
<cc1:SupplierProductsNumberDataSource ID="dsEntity" runat="server" Isolated="true">
</cc1:SupplierProductsNumberDataSource>
<cc1:ProductsDataSource ID="dsProd" runat="server">
</cc1:ProductsDataSource>
<cc1:SupplierDataSource ID="dsSupplier" runat="server">
</cc1:SupplierDataSource>
