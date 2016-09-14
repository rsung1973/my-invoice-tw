<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderList.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.PurchaseOrderList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<%@ Import Namespace="Utility" %>
<script type="text/javascript">
    $(document).ready(
        function () {
            var chkBox = $("input[id$='chkAll']");
            chkBox.click(
            function () {
                $("#gvEntity INPUT[type='checkbox']")
                .attr('checked', chkBox
                .is(':checked'));
            });

            // To deselect CheckAll when a GridView CheckBox        
            // is unchecked        
            $("#gvEntity INPUT[type='checkbox']").click(
            function (e) {
                if (!$(this)[0].checked) {
                    chkBox.attr("checked", false);
                }
            });
        });
</script>
<div class="border_gray">
    <h2>
        入庫單清冊
    </h2>
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="PURCHASE_ORDER_SN" DataSourceID="dsEntity" EnableViewState="False"
        ClientIDMode="Static" OnRowCommand="gvEntity_RowCommand">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <input id="chkAll" name="chkAll" type="checkbox" />
                </HeaderTemplate>
                <ItemTemplate>
                    <input id="chkItem" name="chkItem" type="checkbox" value='<%#((PURCHASE_ORDER)Container.DataItem).PURCHASE_ORDER_SN %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="日期" SortExpression="PURCHASE_ORDER_SN">
                <ItemTemplate>
                    <%# ValueValidity.ConvertChineseDateString(((PURCHASE_ORDER)Container.DataItem).PO_DATETIME) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="單據號碼" SortExpression="PO_QUANTITY">
                <ItemTemplate>
                    <%#((PURCHASE_ORDER)Container.DataItem).PURCHASE_ORDER_NUMBER %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="供應商" SortExpression="PO_UNIT_PRICE">
                <ItemTemplate>
                    <%#((PURCHASE_ORDER)Container.DataItem).SUPPLIER.SUPPLIER_NAME %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="金額" SortExpression="PO_DETAIL_STATUS">
                <ItemTemplate>
                    <%# String.Format("{0:##,###,###,###,###}", ((PURCHASE_ORDER)Container.DataItem).PO_TOTAL_AMOUNT)%>
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
<cc1:PurchaseDataSource ID="dsEntity" runat="server">
</cc1:PurchaseDataSource>
<uc3:ActionHandler ID="doDelete" runat="server" />
<uc3:ActionHandler ID="doEdit" runat="server" />
<uc3:ActionHandler ID="doCreate" runat="server" />
<uc3:ActionHandler ID="doShowRefusal" runat="server" />
