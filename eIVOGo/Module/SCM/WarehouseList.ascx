<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WarehouseList.ascx.cs"
    Inherits="eIVOGo.Module.SCM.WarehouseList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/SupplierProductsItem.ascx" TagName="SupplierProductsItem"
    TagPrefix="uc2" %>
<%@ Register Src="../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="WAREHOUSE_SN" DataSourceID="dsEntity" EnableViewState="False" OnRowCommand="gvEntity_RowCommand">
        <Columns>
            <asp:BoundField DataField="WAREHOUSE_NAME" HeaderText="倉儲名稱" SortExpression="WAREHOUSE_NAME" />
            <asp:BoundField DataField="WAREHOUSE_ADDRESS" HeaderText="地址" SortExpression="WAREHOUSE_ADDRESS" />
            <asp:BoundField DataField="WAREHOUSE_PHONE" HeaderText="電話" SortExpression="WAREHOUSE_PHONE" />
            <asp:BoundField DataField="WAREHOUSE_CONTACT_NAME" HeaderText="聯絡人名稱" SortExpression="WAREHOUSE_CONTACT_NAME" />
            <asp:BoundField DataField="CONTACT_EMAIL" HeaderText="聯絡人電子郵件" SortExpression="CONTACT_EMAIL" />
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" OnClientClick='<%# doCreate.GetPostBackEventReference(null) %>' />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify"
                        Text="修改" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doEdit.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                    &nbsp;<asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="刪除" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確認刪除此筆資料?", String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <center><font color="red">查無資料!!</font></center>
        </EmptyDataTemplate>
        <EmptyDataRowStyle CssClass="noborder" />
        <PagerTemplate>
            <center>
                <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
            </center>
        </PagerTemplate>
    </asp:GridView>
</div>
<cc1:WarehouseDataSource ID="dsEntity" runat="server">
</cc1:WarehouseDataSource>
<uc3:ActionHandler ID="doDelete" runat="server" />
<uc3:ActionHandler ID="doEdit" runat="server" />
<uc3:ActionHandler ID="doCreate" runat="server" />
