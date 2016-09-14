<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryWarehouseTransaction.ascx.cs" Inherits="eIVOGo.Module.SCM.QueryWarehouseTransaction" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register src="../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc2" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc5" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc6" %>
<%@ Register src="../Common/CalendarInputDatePicker.ascx" tagname="ROCCalendarInput" tagprefix="uc1" %>
<%@ Import Namespace="Utility" %>


<uc5:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 庫存異動查詢" />        
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="庫存異動查詢" />
<div id="border_gray">
<!--表格 開始-->
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
    <tr>
        <th colspan="2" class="Head_style_a">查詢條件</th>
    </tr>
    <tr>
        <th>倉儲</th>
        <td class="tdleft">
            <asp:DropDownList ID="ddlWarehouse" CssClass="textfield" runat="server">
                <asp:ListItem>全部</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>BarCode</th>
        <td class="tdleft">
            <asp:TextBox ID="txtBarCode" CssClass="textfield" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th>料品名稱</th>
        <td class="tdleft">
            <asp:TextBox ID="txtProdName" CssClass="textfield" runat="server"></asp:TextBox><font color="blue">可輸入關鍵字查詢</font>
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
        <asp:GridView ID="gvEntity" runat="server" CssClass="table01" AllowPaging="True"
        AutoGenerateColumns="False" EnableViewState="false">
        <Columns>
            <asp:TemplateField HeaderText="日期區間" InsertVisible="False">
                <ItemTemplate>
                    <%# ValueValidity.ConvertChineseDateString(this.DateFrom.DateTimeValue) %> ~ <%# ValueValidity.ConvertChineseDateString(this.DateTo.DateTimeValue)%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="倉儲" InsertVisible="False">
                <ItemTemplate>
                    <%# ((_QueryItem)Container.DataItem).WAREHOUSE%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="料品Barcode">
                <ItemTemplate>
                    <%# ((_QueryItem)Container.DataItem).PRODUCTSBARCODE%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="料品名稱">
                <ItemTemplate>
                    <%# ((_QueryItem)Container.DataItem).PRODUCTSNAME%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="入庫數量">
                <ItemTemplate>
                    <%# ((_QueryItem)Container.DataItem).INBOUNDQUANTITY %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="入庫瑕疵數量">
                <ItemTemplate>
                    <%# ((_QueryItem)Container.DataItem).DEFECTIVEQUANTITY %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="出庫數量">
                <ItemTemplate>
                    <%# ((_QueryItem)Container.DataItem).OUTBOUNDQUANTITY  %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
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
                <td class="total-count" align="right">總筆數： <asp:Label ID="lblRowCount" Text="0" runat="server"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;總頁數：<asp:Label ID="lblTotalSum" Text="0" runat="server"></asp:Label></td>
            </tr>
        </table>
        <!--表格 結束-->
        <center>
            <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
        </center>
    </div>            
    <!--按鈕-->
</div>
<cc1:WarehouseProductsTransactionLogDataSource ID="dsEntity" runat="server">
</cc1:WarehouseProductsTransactionLogDataSource>