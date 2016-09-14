<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoodsReceiptList.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.GoodsReceiptList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Item/SupplierProductsItem.ascx" TagName="SupplierProductsItem"
    TagPrefix="uc2" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Register Src="../../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc4" %>
<%@ Register Src="ShowRefusalReason.ascx" TagName="ShowRefusalReason" TagPrefix="uc5" %>
<%@ Register Src="RefuseDocument.ascx" TagName="RefuseDocument" TagPrefix="uc6" %>
<%@ Register src="../Action/PrintEntityPreview.ascx" tagname="PrintEntityPreview" tagprefix="uc7" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="WAREHOUSE_WARRANT_SN" DataSourceID="dsEntity" EnableViewState="False"
        OnRowCommand="gvEntity_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="日期" SortExpression="">
                <ItemTemplate>
                    <%# ValueValidity.ConvertChineseDateString(((WAREHOUSE_WARRANT)Container.DataItem).WW_DATETIME) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="入庫單號碼" SortExpression="">
                <ItemTemplate>
                    <a onclick="<%# doPrintOrder.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>"
                        href="#">
                        <%# ((WAREHOUSE_WARRANT)Container.DataItem).WAREHOUSE_WARRANT_NUMBER %></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="轉入單據種類" SortExpression="">
                <ItemTemplate>
                    <%# ((WAREHOUSE_WARRANT)Container.DataItem).WAREHOUSE_WARRANT_DETAILS.Count(d=>d.PO_DETAILS_SN.HasValue)>0 ? "採購單":"退、換貨單" %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="入庫倉儲" SortExpression="">
                <ItemTemplate>
                    <%# ((WAREHOUSE_WARRANT)Container.DataItem).WAREHOUSE.WAREHOUSE_NAME %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="供應商" SortExpression="">
                <ItemTemplate>
                    <%# ((WAREHOUSE_WARRANT)Container.DataItem).SUPPLIER.SUPPLIER_NAME %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="金額" SortExpression="">
                <ItemTemplate>
                    <%# ((WAREHOUSE_WARRANT)Container.DataItem).WAREHOUSE_WARRANT_DETAILS.Count(d=>d.PO_DETAILS_SN.HasValue)>0 ? String.Format("{0:##,###,###,###,###}",((WAREHOUSE_WARRANT)Container.DataItem).WAREHOUSE_WARRANT_DETAILS.Sum(d=>d.PURCHASE_ORDER_DETAILS.PO_UNIT_PRICE)) 
                        : ((WAREHOUSE_WARRANT)Container.DataItem).WAREHOUSE_WARRANT_DETAILS.Count(d=>d.GR_DETAILS_SN.HasValue)>0 ? String.Format("{0:##,###,###,###}",((WAREHOUSE_WARRANT)Container.DataItem).WAREHOUSE_WARRANT_DETAILS.Sum(d=>d.GOODS_RETURNED_DETAILS.BUYER_ORDERS_DETAILS.BO_UNIT_PRICE))
                                            : ((WAREHOUSE_WARRANT)Container.DataItem).WAREHOUSE_WARRANT_DETAILS.Count(d=>d.EGI_DETAILS_SN.HasValue)>0 ? String.Format("{0:##,###,###,###}",((WAREHOUSE_WARRANT)Container.DataItem).WAREHOUSE_WARRANT_DETAILS.Sum(d=>d.EXCHANGE_GOODS_INBOUND_DETAILS.BO_UNIT_PRICE))
                                            : "N/A"
                    %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除狀態" SortExpression="EXCHANGE_GOODS_SN">
                <ItemTemplate>
                    <%# ((WAREHOUSE_WARRANT)Container.DataItem).CDS_Document.CurrentStep != (int)Model.Locale.Naming.DocumentStepDefinition.已刪除 ? "未刪除"
                        : String.Format("<a href=\"#\" onclick=\"{0}\">已刪除</a>",doShowRefusal.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0])))) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" OnClientClick='<%# doCreate.GetPostBackEventReference(null) %>' />
                </FooterTemplate>
                <ItemTemplate>
                    <%--<asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify"
                        Text="修改" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doEdit.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                    &nbsp;--%><asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Visible='<%# ((WAREHOUSE_WARRANT)Container.DataItem).CDS_Document.CurrentStep!=(int)Model.Locale.Naming.DocumentStepDefinition.已刪除 %>'
                        Text="刪除" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doDelete.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
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
<cc1:WarehouseWarrantDataSource ID="dsEntity" runat="server">
</cc1:WarehouseWarrantDataSource>
<uc3:ActionHandler ID="doDelete" runat="server" />
<uc3:ActionHandler ID="doEdit" runat="server" />
<uc3:ActionHandler ID="doCreate" runat="server" />
<uc3:ActionHandler ID="doShowRefusal" runat="server" />
<uc3:ActionHandler ID="doPrintOrder" runat="server" />
<uc5:ShowRefusalReason ID="showRefusal" runat="server" />
<uc6:RefuseDocument ID="refuseDoc" runat="server" />
<uc4:PageAnchor ID="ToShowInvoice" runat="server" TransferTo="~/Published/PrintInvoicePage.aspx" />
<uc7:PrintEntityPreview ID="printWarehouseWarrant"  runat="server" PreviewControlPath="~/Module/SCM/View/WarehouseWarrantPreview.ascx" />

