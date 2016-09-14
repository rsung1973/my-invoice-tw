<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductsAttributeMappingList.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Item.ProductsAttributeMappingList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc2" %>
<asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
    DataKeyNames="PRODUCTS_ATTR_NAME_SN,PRODUCTS_SN" EnableViewState="False"
    OnRowCommand="gvEntity_RowCommand" ShowFooter="False">
    <Columns>
        <asp:TemplateField HeaderText="料品屬性名稱" SortExpression="PRODUCTS_ATTR_NAME_SN">
            <ItemTemplate>
                <%#((PRODUCTS_ATTRIBUTE_MAPPING)Container.DataItem).PRODUCTS_ATTRIBUTE_NAME.PRODUCTS_ATTR_NAME %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="PRODUCTS_ATTR_VALUE" HeaderText="料品屬性內容" SortExpression="PRODUCTS_ATTR_VALUE" />
        <asp:TemplateField ShowHeader="False">
            <FooterTemplate>
                <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                    Text="新增" />
            </FooterTemplate>
            <ItemTemplate>
                &nbsp;<asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                    Text="刪除" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確認刪除此筆資料?",Container.DataItemIndex.ToString()) %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <PagerTemplate>
        <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
    </PagerTemplate>
</asp:GridView>
<uc2:ActionHandler ID="doDelete" runat="server" />


