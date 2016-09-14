<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderReturnedMangement.ascx.cs" Inherits="eIVOGo.Module.SCM.PurchaseOrderReturnedMangement" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register src="../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc2" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc5" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc6" %>
<%@ Register src="../Common/CalendarInputDatePicker.ascx" tagname="ROCCalendarInput" tagprefix="uc1" %>
<%@ Register src="Item/PupopPORDetail.ascx" tagname="PupopPORDetail" tagprefix="uc3" %>
<%@ Register src="View/RefuseDocument.ascx" tagname="RefuseDocument" tagprefix="uc4" %>
<%@ Register src="Item/PupopDeleteReason.ascx" tagname="PupopDeleteReason" tagprefix="uc7" %>
<%@ Import Namespace="Model.Locale" %>


<uc5:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 採購退貨作業" />        
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="採購退貨作業" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="top_table">
    <tr>
        <td>
            <asp:Button ID="btnNewPurchaseRtn" Text="新增採購退貨單" CssClass="btn" runat="server" onclick="btnNewPurchaseRtn_Click" />
        </td>
    </tr>
</table>
<div id="border_gray">
<!--表格 開始-->
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
    <tr>
        <th colspan="2" class="Head_style_a">查詢條件</th>
    </tr>
    <tr>
        <th>退貨倉儲</th>
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
        <th>採購單退貨號碼</th>
        <td class="tdleft">
            <asp:TextBox ID="txtPurchaseRetNO" CssClass="textfield" runat="server"></asp:TextBox>
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
    <tr>
        <th>結案狀態</th>
        <td class="tdleft">
            <asp:RadioButtonList ID="rdbCloseStatus" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                <asp:ListItem Selected="True">全部</asp:ListItem>
                <asp:ListItem>已結案</asp:ListItem>
                <asp:ListItem>未結案</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th>單據刪除狀態</th>
        <td class="tdleft">
            <asp:RadioButtonList ID="rdbDeleteStatus" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                <asp:ListItem Selected="True">全部</asp:ListItem>
                <asp:ListItem>已刪除</asp:ListItem>
                <asp:ListItem>未刪除</asp:ListItem>
            </asp:RadioButtonList>
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
        DataSourceID="dsPOReturn">
            <Columns>
                <asp:TemplateField HeaderText="日期">
                    <ItemTemplate><%# Utility.ValueValidity.ConvertChineseDateString(((PURCHASE_ORDER_RETURNED)Container.DataItem).PO_RETURNED_DATETIME.Value)%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="供應商">
                    <ItemTemplate><%# ((PURCHASE_ORDER_RETURNED)Container.DataItem).SUPPLIER.SUPPLIER_NAME %></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="採購單退貨號碼" > <ItemTemplate>                    
                    <asp:LinkButton ID="lbtn" runat="server" Text='<%# ((PURCHASE_ORDER_RETURNED)Container.DataItem).PURCHASE_ORDER_RETURNED_NUMBER %>' 
                     CausesValidation="false" CommandName="Edit" OnClientClick='<%# Page.ClientScript.GetPostBackEventReference(this, String.Format("P:{0}",((PURCHASE_ORDER_RETURNED)Container.DataItem).PURCHASE_ORDER_RETURNED_SN)) + "; return false;" %>' />
                    </ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="金額">
                    <ItemTemplate><%# String.Format("{0:0,0.00}", ((PURCHASE_ORDER_RETURNED)Container.DataItem).PO_RETURN_TOTAL_AMOUNT)%></ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="結案">
                    <ItemTemplate><%# ((PURCHASE_ORDER_RETURNED)Container.DataItem).PO_RETURNED_STATUS==1?"是":"否" %></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="刪除狀態"><ItemTemplate>
                    <asp:Label ID="lblDel" runat="server" Text='<%# ((PURCHASE_ORDER_RETURNED)Container.DataItem).CDS_Document.CurrentStep==(int)Naming.DocumentStepDefinition.已刪除 ? "已刪除" : "未刪除" %>' Visible='<%# ((PURCHASE_ORDER_RETURNED)Container.DataItem).CDS_Document.CurrentStep==(int)Naming.DocumentStepDefinition.已刪除 ? false : true %>'></asp:Label>
                    <asp:LinkButton ID="lbtn" runat="server" Text='<%# ((PURCHASE_ORDER_RETURNED)Container.DataItem).CDS_Document.CurrentStep==(int)Naming.DocumentStepDefinition.已刪除 ? "已刪除" : "未刪除" %>' Visible='<%# ((PURCHASE_ORDER_RETURNED)Container.DataItem).CDS_Document.CurrentStep==(int)Naming.DocumentStepDefinition.已刪除 ? true : false %>'
                     CausesValidation="false" CommandName="Edit" OnClientClick='<%# Page.ClientScript.GetPostBackEventReference(this, String.Format("S:{0}",((PURCHASE_ORDER_RETURNED)Container.DataItem).CDS_Document.DocID)) + "; return false;" %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" HeaderText="管理">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete" Enabled='<%# ((PURCHASE_ORDER_RETURNED)Container.DataItem).PO_RETURNED_STATUS == 1 ? false : true %>'
                            Text="刪除" CssClass="btn" OnClientClick='<%# String.Format("if(confirm(\"確認刪除此筆資料?\")) {0} ", Page.ClientScript.GetPostBackEventReference(this, String.Format("D:{0}",((PURCHASE_ORDER_RETURNED)Container.DataItem).PURCHASE_ORDER_RETURNED_SN))) + "; return false;" %>' />
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
                <td class="total-count" align="right">共 <asp:Label ID="lblRowCount" Text="0" runat="server"></asp:Label> 筆&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;總計金額：<asp:Label ID="lblTotalSum" Text="0" runat="server"></asp:Label></td>
            </tr>
        </table>
        <!--表格 結束-->
        <center>
            <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
        </center>
    </div>            
    <!--按鈕-->
</div>
<cc1:PurchaseOrderReturnDataSource ID="dsPOReturn" runat="server">
</cc1:PurchaseOrderReturnDataSource>
<uc3:PupopPORDetail ID="PupopPORDetail1" runat="server" />
<uc4:RefuseDocument ID="RefuseDocument" runat="server" />
<uc7:PupopDeleteReason ID="PupopDeleteReason1" runat="server" />


