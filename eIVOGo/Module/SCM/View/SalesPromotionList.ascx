<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SalesPromotionList.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.SalesPromotionList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Item/SupplierProductsItem.ascx" TagName="SupplierProductsItem"
    TagPrefix="uc2" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<%@ Import Namespace="Utility" %>
<%@ Register src="../../Common/PageAnchor.ascx" tagname="PageAnchor" tagprefix="uc4" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="SALES_PROMOTION_SN" DataSourceID="dsEntity" EnableViewState="False"
        OnRowCommand="gvEntity_RowCommand">
        <Columns>
            <asp:BoundField DataField="SALES_PROMOTION_NAME" HeaderText="促銷專案名稱" 
                SortExpression="SALES_PROMOTION_NAME" />
            <asp:BoundField DataField="SALES_PROMOTION_SYMBOL" HeaderText="專案代號" 
                SortExpression="SALES_PROMOTION_SYMBOL" />
            <asp:BoundField DataField="SALES_PROMOTION_DISCOUNT" HeaderText="折扣（百分比）" 
                SortExpression="SALES_PROMOTION_DISCOUNT" />
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
            <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
    </asp:GridView>
</div>
<cc1:SalesPromotionDataSource ID="dsEntity" runat="server">
</cc1:SalesPromotionDataSource>
<uc3:ActionHandler ID="doDelete" runat="server" />
<uc3:ActionHandler ID="doEdit" runat="server" />
<uc3:ActionHandler ID="doCreate" runat="server" />
<uc4:PageAnchor ID="ToEditSalesPromotion" runat="server" 
    TransferTo="~/SCM_SYSTEM/Promotional_Project_Maintain_Add.aspx" />

