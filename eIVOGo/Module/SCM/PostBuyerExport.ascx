<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostBuyerExport.ascx.cs" Inherits="eIVOGo.Module.SCM.PostBuyerExport" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register src="../Common/CalendarInputDatePicker.ascx" tagname="ROCCalendarInput" tagprefix="uc1" %>
<%@ Register src="../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc2" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc5" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc6" %>
<%@ Register Src="Item/MarketResourceSelector.ascx" TagName="MarketResourceSelector" TagPrefix="uc3" %>
<%@ Register src="Item/DeliveryCompanySelector.ascx" tagname="DeliveryCompanySelector" tagprefix="uc4" %>
<%@ Register Src="../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc7" %>
<%@ Register src="Action/PrintShipmentPreview.ascx" tagname="PrintShipmentPreview" tagprefix="uc8" %>
<%@ Register src="Action/PrintInvoicePreview.ascx" tagname="PrintInvoicePreview" tagprefix="uc9" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.Locale" %>

<script type="text/javascript">
    $(document).ready(
        function () {
            var chkBox = $("input[id$='chkAll']");
            chkBox.click(
            function () {
                $("#<%=gvEntity.ClientID%> INPUT[type='checkbox']")
                .attr('checked', chkBox
                .is(':checked'));
            });

            // To deselect CheckAll when a GridView CheckBox        
            // is unchecked        
            $("#<%=gvEntity.ClientID%> INPUT[type='checkbox']").click(
            function (e) {
                if (!$(this)[0].checked) {
                    chkBox.attr("checked", false);
                }
            });
        });
</script>


<uc5:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 買受人郵寄資料匯出" />        
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="買受人郵寄資料匯出" />
<div id="border_gray">
<!--表格 開始-->
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
    <tr>
        <th colspan="2" class="Head_style_a">查詢條件</th>
    </tr>
    <tr>
        <th width="20%">
            購物平台
        </th>
        <td class="tdleft">
            &nbsp;<uc3:MarketResourceSelector ID="marketResource" runat="server" />
        </td>
    </tr>
    <tr>
        <th width="20%">
            宅配公司
        </th>
        <td class="tdleft">
            <uc4:DeliveryCompanySelector ID="delivery" runat="server" />
        </td>
    </tr>
    <tr>
        <th>
            出貨單號
        </th>
        <td class="tdleft">
            <asp:TextBox ID="orderNo" runat="server"></asp:TextBox>
            &nbsp;
        </td>
    </tr>
    <tr>
        <th>匯出狀態</th>
        <td class="tdleft">
            <asp:RadioButtonList ID="rdbExportStatus" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                <asp:ListItem Selected="True">全部</asp:ListItem>
                <asp:ListItem>未匯出</asp:ListItem>
                <asp:ListItem>已匯出</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
        <tr>
        <th width="20%">日期區間</th>
        <td class="tdleft">
        自&nbsp;<uc1:ROCCalendarInput ID="DateFrom" runat="server" />&nbsp;
        至&nbsp;&nbsp;<uc1:ROCCalendarInput ID="DateTo" runat="server" />&nbsp;
        </td>
    </tr>
</table>
<!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" CssClass="btn" runat="server" Text="查詢" onclick="btnQuery_Click" />
            <asp:Button ID="btnReset" CssClass="btn" runat="server" Text="重填" />
        </td>
    </tr>
</table>

<div id="divResult" visible="false" runat="server">
    <uc6:FunctionTitleBar ID="FunctionTitleBar2" runat="server" ItemName="查詢結果" />
    <!--表格 開始-->
    <div id="border_gray">
        <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" Width="100%" CellPadding="0"
        EnableViewState="false" GridLines="None" AutoGenerateColumns="False" CssClass="table01"
        DataKeyNames="DocID" DataSourceID="dsEntity">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <input id="chkAll" name="chkAll" type="checkbox" /></HeaderTemplate>
                    <ItemTemplate>
                        <input id="chkItem" name="chkItem" type="checkbox" value='<%#((CDS_Document)Container.DataItem).DocID %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="日期"><ItemTemplate>
                    <%# ValueValidity.ConvertChineseDateString(((CDS_Document)Container.DataItem).BUYER_SHIPMENT.SHIPMENT_DATETIME)%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="買受人"><ItemTemplate>
                    <%# ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? ((CDS_Document)Container.DataItem).BUYER_ORDERS.BUYER_DATA.BUYER_NAME
                        : ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? ((CDS_Document)Container.DataItem).EXCHANGE_GOODS.BUYER_ORDERS.BUYER_DATA.BUYER_NAME
                        : "" %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="出貨單號" > <ItemTemplate>                    
                    <a onclick="<%# doPrintOrder.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>" href="#"><%# ((CDS_Document)Container.DataItem).BUYER_SHIPMENT.BUYER_SHIPMENT_NUMBER%></a>
                    </ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="發票號碼"><ItemTemplate>
                    <a onclick="<%# doPrintInvoice.GetPostBackEventReference(String.Format("{0}",((CDS_Document)Container.DataItem).BUYER_SHIPMENT.INVOICE_SN)) %>" href="#"><%# ((CDS_Document)Container.DataItem).BUYER_SHIPMENT.INVOICE_SN.HasValue ? ((CDS_Document)Container.DataItem).BUYER_SHIPMENT.InvoiceItem.TrackCode + ((CDS_Document)Container.DataItem).BUYER_SHIPMENT.InvoiceItem.No : ""%></a>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="購物平台"><ItemTemplate>
                    <%# ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? ((CDS_Document)Container.DataItem).BUYER_ORDERS.MARKET_RESOURCE.MARKET_RESOURCE_NAME
                        : ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? ((CDS_Document)Container.DataItem).EXCHANGE_GOODS.BUYER_ORDERS.MARKET_RESOURCE.MARKET_RESOURCE_NAME
                        : "" %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="宅配公司">
                    <ItemTemplate><%# ((CDS_Document)Container.DataItem).BUYER_SHIPMENT.DELIVERY_COMPANY.DELIVERY_COMPANY_NAME%></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="轉入單據種類"><ItemTemplate>
                    <%# ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? "訂單"
                        : ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? "換貨單"
                        : "" %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="匯出狀態"><ItemTemplate>
                    <%# ((CDS_Document)Container.DataItem).BUYER_SHIPMENT.POST_PRINT_STATUS == 1 ? "已匯出" : "未匯出"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="金額"><ItemTemplate>
                    <%# ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? String.Format("{0:0,0.00}", ((CDS_Document)Container.DataItem).BUYER_ORDERS.ORDERS_AMOUNT)
                        : ((CDS_Document)Container.DataItem).DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? String.Format("{0:0,0.00}", ((CDS_Document)Container.DataItem).EXCHANGE_GOODS.EXCHANGE_GOODS_OUTBOND_DETAILS.Sum(o => o.GR_BS_QUANTITY * o.BO_UNIT_PRICE))
                        : ""
                    %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
            </Columns>
            <AlternatingRowStyle CssClass="OldLace" />
            <PagerStyle BackColor="PaleGoldenrod" HorizontalAlign="Left" BorderStyle="None" CssClass="noborder" />
            <PagerTemplate>
                <uc2:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
            </PagerTemplate>
        </asp:GridView>
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table01" id="countTable" runat="server">
            <tr>
                <td class="total-count" align="right">總比數： <asp:Label ID="lblRowCount" Text="0" runat="server"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;總頁數：<asp:Label ID="lblTotalSum" Text="0" runat="server"></asp:Label></td>
            </tr>
        </table>
        <!--表格 結束-->
        <center>
            <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
        </center>
    </div>            
    <!--按鈕-->
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnExport" Text="匯出" CssClass="btn" runat="server" onclick="btnExport_Click" />
            </td>
        </tr>
    </table>
</div>
<cc1:DocumentDataSource ID="dsEntity" runat="server">
</cc1:DocumentDataSource>
<uc7:ActionHandler ID="doPrintOrder" runat="server" />
<uc7:ActionHandler ID="doPrintInvoice" runat="server" />
<uc8:PrintShipmentPreview ID="printShipment" runat="server" />
<uc9:PrintInvoicePreview ID="printInvoice" runat="server" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        marketResource.Selector.DataBound += new EventHandler(market_DataBound);
        delivery.Selector.DataBound += new EventHandler(Selector_DataBound);
    }

    void Selector_DataBound(object sender, EventArgs e)
    {
        delivery.Selector.Items.Insert(0, new ListItem("全部", ""));
    }

    void market_DataBound(object sender, EventArgs e)
    {
        marketResource.Selector.Items.Insert(0, new ListItem("全部", ""));
    }
</script>
