<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderVerify.ascx.cs" Inherits="eIVOGo.Module.SCM.PurchaseOrderVerify" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register src="../Common/CalendarInputDatePicker.ascx" tagname="ROCCalendarInput" tagprefix="uc1" %>
<%@ Register src="../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc2" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc5" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc6" %>


<uc5:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 採購審核作業" />        
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="採購審核作業" />
<div id="border_gray">
<!--表格 開始-->
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
    <tr>
        <th colspan="2" class="Head_style_a">查詢條件</th>
    </tr>
    <tr>
        <th>入庫倉儲</th>
        <td class="tdleft">
            <asp:DropDownList ID="ddlWarehouse" CssClass="textfield" runat="server">
                <asp:ListItem>全部</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>供應商</th>
        <td class="tdleft">
            <asp:DropDownList ID="ddlSupplier" CssClass="textfield" runat="server" >
                <asp:ListItem>全部</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>採購單號碼</th>
        <td class="tdleft">
            <asp:TextBox ID="txtPurchaseNO" CssClass="textfield" runat="server"></asp:TextBox>
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
        </td>
    </tr>
</table>

<div id="divResult" visible="false" runat="server">
    <uc6:FunctionTitleBar ID="FunctionTitleBar2" runat="server" ItemName="查詢結果" />
    <!--表格 開始-->
    <div id="border_gray">
        <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" Width="100%" CellPadding="0"
        EnableViewState="false" GridLines="None" AutoGenerateColumns="False" CssClass="table01"
        DataSourceID="dsPurchase">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate><input id="chkItem" name="chkItem" type="radio" value='<%# ((PURCHASE_ORDER)Container.DataItem).PURCHASE_ORDER_SN  %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="日期">
                    <ItemTemplate><%# ((PURCHASE_ORDER)Container.DataItem).PO_DATETIME.Value %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="供應商">
                    <ItemTemplate><%# ((PURCHASE_ORDER)Container.DataItem).SUPPLIER.SUPPLIER_NAME %></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="採購單號碼" > 
                    <ItemTemplate><%# ((PURCHASE_ORDER)Container.DataItem).PURCHASE_ORDER_NUMBER %></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="金額">
                    <ItemTemplate><%# String.Format("{0:0,0.00}", ((PURCHASE_ORDER)Container.DataItem).PO_TOTAL_AMOUNT) %></ItemTemplate>
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
                <td class="total-count" align="right">共 <asp:Label ID="lblRowCount" Text="0" runat="server"></asp:Label> 筆&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;總計金額：<asp:Label ID="lblTotalSum" Text="0" runat="server"></asp:Label></td>
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
                <asp:Button ID="btnVerify" Text="審核" CssClass="btn" runat="server" onclick="btnVerify_Click" />
            </td>
        </tr>
    </table>
</div>
<cc1:PurchaseDataSource ID="dsPurchase" runat="server">
</cc1:PurchaseDataSource>
