<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceCancelList.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.InvoiceCancelList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker" TagPrefix="uc3" %>
<%@ Register src="ConfirmInvoiceCancelList.ascx" tagname="ConfirmInvoiceCancelList" tagprefix="uc4" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc5" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc6" %>
<%@ Register src="PNewInvalidInvoicePreview.ascx" tagname="PNewInvalidInvoicePreview" tagprefix="uc7" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" ItemName="首頁 >作廢電子發票開立" runat="server" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" ItemName="作廢電子發票開立" runat="server" />
<!--交易畫面標題-->
<div id="DIV1" runat="server" class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <tr>
            <th width="20%">
                日期區間
            </th>
            <td class="tdleft">
                自&nbsp;
                <uc3:CalendarInputDatePicker ID="CalendarInputDatePicker1" runat="server" />
                至&nbsp; &nbsp;<uc3:CalendarInputDatePicker ID="CalendarInputDatePicker2" runat="server" />
                &nbsp;<span class="red"><asp:Label ID="Label1" runat="server" ForeColor="#FF3300"
                    Text="當期已開立電子發票方可執行作廢電子發票"></asp:Label>
                </span>
            </td>
        </tr>
        <tr>
            <th>
                訂單號碼
            </th>
            <td class="tdleft">
                &nbsp;<asp:TextBox ID="CheckNo" type="text" class="textfield" size="20" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                發票號碼
            </th>
            <td class="tdleft">
                &nbsp;<asp:TextBox ID="InvoiceNo" type="text" class="textfield" size="20" runat="server" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<div id="DIV2" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td id="QTB" runat="server" class="Bargain_btn">
                <asp:Button ID="btnQuery" runat="server" CssClass="btn" OnClick="btnQuery_Click"
                    Text=" 查詢" />
            </td>
        </tr>
    </table>
</div>
<uc6:FunctionTitleBar ID="H1" ItemName="查詢結果" runat="server" />
<div id="DIV3" runat="server" class="border_gray">
<asp:GridView ID="gvEntity" runat="server" EnableViewState="True" CssClass="table01"
    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" ShowFooter="False"
    Width="100%" OnRowCommand="gvEntity_RowCommand" OnSelectedIndexChanged="gvEntity_SelectedIndexChanged" CellPadding="0">
    <AlternatingRowStyle CssClass="OldLace" />
    <Columns>
        <asp:TemplateField HeaderText="日期">
            <ItemTemplate>
                <%# Utility.ValueValidity.ConvertChineseDate(Eval("invoiceitem.InvoiceDate"))%>
            </ItemTemplate>
            <FooterStyle HorizontalAlign="Right" CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="開立發票營業人名稱">
            <ItemTemplate>
                <%# Eval("Seller.CompanyName") %></ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="統編">
            <ItemTemplate>
                <%# Eval("Seller.ReceiptNo") %>
            </ItemTemplate>
            <FooterStyle HorizontalAlign="Right" CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="訂單號碼">
            <ItemTemplate>
                <%# Eval("InvoiceItem.CheckNo")%>
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
            <FooterStyle HorizontalAlign="Right" CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="發票號碼">
            <ItemTemplate>
                <asp:LinkButton ID="viewInvoice" runat="server" CommandName="Show" CommandArgument='<%# Eval("InvoiceItem.InvoiceID")%>' Text='<%# String.Format("{0}{1}",Eval("InvoiceItem.TrackCode"),Eval("InvoiceItem.No")) %>' ></asp:LinkButton>
            </ItemTemplate>
            <FooterTemplate>
                &nbsp;&nbsp; 金額：
            </FooterTemplate>
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="金額">
            <ItemTemplate>
                <%# String.Format("{0:##,###,###,##0.00}", Eval("invoiceitem.InvoiceAmountType.TotalAmount"))%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="捐贈單位">
            <ItemTemplate>
                <%# Eval("WName")%>
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
            <FooterStyle HorizontalAlign="Right" CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="btnManager" runat="server" Text="作廢發票" CommandArgument='<%#  Eval("InvoiceItem.InvoiceID")%>'
                    CommandName="CancelInv" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <FooterTemplate>
            </FooterTemplate>
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <div align="center">
            <font color="red">查無資料!!</font></div>
    </EmptyDataTemplate>
    <PagerTemplate>
        <uc2:PagingControl ID="pagingList" runat="server" />
    </PagerTemplate>
</asp:GridView>
</div>
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
<uc4:ConfirmInvoiceCancelList ID="modalConfirm" runat="server" />
<uc7:PNewInvalidInvoicePreview ID="modalInvoice" runat="server" />
