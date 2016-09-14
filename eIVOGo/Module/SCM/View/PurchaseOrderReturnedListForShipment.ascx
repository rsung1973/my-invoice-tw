<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderReturnedListForShipment.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.PurchaseOrderReturnedListForShipment" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Item/SupplierProductsItem.ascx" TagName="SupplierProductsItem"
    TagPrefix="uc2" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<%@ Import Namespace="Utility" %>
<%@ Register Src="../../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc4" %>
<%@ Register Src="ShowRefusalReason.ascx" TagName="ShowRefusalReason" TagPrefix="uc5" %>
<%@ Register Src="RefuseDocument.ascx" TagName="RefuseDocument" TagPrefix="uc6" %>
<%@ Register Src="../Action/PrintEntityPreview.ascx" TagName="PrintEntityPreview"
    TagPrefix="uc7" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="PURCHASE_ORDER_RETURNED_SN" DataSourceID="dsEntity" EnableViewState="False"
        OnRowCommand="gvEntity_RowCommand">
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <input type="radio" name="rbSN" value='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="日期" SortExpression="">
                <ItemTemplate>
                    <%# ValueValidity.ConvertChineseDateString(Eval("PO_RETURNED_DATETIME"))%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="供應商" SortExpression="">
                <ItemTemplate>
                    <%# ((PURCHASE_ORDER_RETURNED)Container.DataItem).SUPPLIER.SUPPLIER_NAME %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="退貨單號碼" SortExpression="">
                <ItemTemplate>
                    <a href="#" onclick="<%# doPrintPreview.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>">
                        <%# Eval("PURCHASE_ORDER_RETURNED_NUMBER")%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="金額" SortExpression="">
                <ItemTemplate>
                    <%# String.Format("{0:##,###,###,###,##0}", ((PURCHASE_ORDER_RETURNED)Container.DataItem).PO_RETURN_TOTAL_AMOUNT) %>
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
<cc1:PurchaseOrderReturnDataSource ID="dsEntity" runat="server">
</cc1:PurchaseOrderReturnDataSource>
<uc3:ActionHandler ID="doDelete" runat="server" />
<uc3:ActionHandler ID="doEdit" runat="server" />
<uc3:ActionHandler ID="doCreate" runat="server" />
<uc3:ActionHandler ID="doShowRefusal" runat="server" />
<uc3:ActionHandler ID="doPrintPreview" runat="server" />
<uc5:ShowRefusalReason ID="showRefusal" runat="server" />
<uc6:RefuseDocument ID="refuseDoc" runat="server" />
<uc7:PrintEntityPreview ID="printPreview" runat="server" PreviewControlPath="~/Module/SCM/View/ReturnedPurchaseOrderPreview.ascx" />
