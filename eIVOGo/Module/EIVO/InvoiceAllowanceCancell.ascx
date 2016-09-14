<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceAllowanceCancell.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.InvoiceAllowanceCancell" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker" TagPrefix="uc3" %>
<%@ Register Src="ConfirmAllowanceCancellation.ascx" TagName="ConfirmAllowanceCancellation"
    TagPrefix="uc4" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc5" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc6" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 >作廢電子折讓單" />
<!--交易畫面標題--><uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="作廢電子折讓單" />
<div id="border_gray">
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
                &nbsp;&nbsp; 至&nbsp;<uc3:CalendarInputDatePicker ID="CalendarInputDatePicker2" runat="server" />
                &nbsp;&nbsp;
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
                折讓單號碼
            </th>
            <td class="tdleft">
                <asp:TextBox ID="AllowanceNo" type="text" class="textfield" size="20" runat="server" />
                &nbsp;
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button class="btn" Text="查詢" ID="btnQuery" runat="server" OnClick="btnQuery_Click" />
        </td>
    </tr>
</table>
    <div id="divResult" visible="false" runat="server">
<h1>
    <img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif"
        width="29" height="28" border="0" align="absmiddle" />查詢結果</h1>
        <div id="border_gray">
<asp:GridView ID="gvEntity" runat="server" EnableViewState="True" CssClass="table01"
    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" ShowFooter="False"
    Width="100%" OnRowCommand="gvEntity_RowCommand">
    <AlternatingRowStyle CssClass="OldLace" />
    <Columns>
        <asp:TemplateField HeaderText="日期">
            <ItemTemplate>
                <%# Utility .ValueValidity .ConvertChineseDate( Eval("invallowance.AllowanceDate"))%>
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
                <a href="NewInvalidInvoicePreview.aspx?id=<%#  Eval("InvoiceItem.InvoiceID")%>" target="_blank">
                    <%#  Eval("InvoiceItem.No")%></a>
            </ItemTemplate>
            <FooterTemplate>
                &nbsp;&nbsp; 金額：
            </FooterTemplate>
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="折讓號碼">
            <ItemTemplate>
                <a href="AllowencePreview.aspx?id=<%#  Eval("invallowance.AllowanceID")%>" target="_blank">
                    <%#  Eval("invallowance.AllowanceNumber")%></a>
            </ItemTemplate>
            <FooterTemplate>
                &nbsp;&nbsp; 金額：
            </FooterTemplate>
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="金額">
            <ItemTemplate>
                <%# String.Format("{0:##,###,###,##0.00}", Eval("invallowance.TotalAmount"))%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="btnManager" runat="server" Text="作廢折讓" CommandArgument='<%#  Eval("invallowance.AllowanceID")%>'
                    CommandName="Select" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <FooterTemplate>
            </FooterTemplate>
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
    </Columns>
    
    <PagerTemplate>
        <uc2:PagingControl ID="pagingList" runat="server" />
    </PagerTemplate>
</asp:GridView>
<center>
            <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
            </center>
</div>
</div>
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
<uc4:ConfirmAllowanceCancellation ID="modalConfirm" runat="server" />
