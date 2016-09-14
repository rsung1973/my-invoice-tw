<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintCInvoice.ascx.cs"
    Inherits="eIVOGo.Module.Inquiry.PrintCInvoice" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../UI/SellerSelector.ascx" TagName="SellerSelector" TagPrefix="uc7" %>

<uc5:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 列印電子計算機發票" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="列印電子計算機發票" />
<div class="border_gray">
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
                自&nbsp;<uc1:CalendarInputDatePicker ID="DateFrom" runat="server" />
                &nbsp;至&nbsp;<uc1:CalendarInputDatePicker ID="DateTo" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                買受人統編
            </th>
            <td class="tdleft">
                <asp:TextBox ID="ReceiptNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                發票號碼
            </th>
            <td class="tdleft">
                <asp:TextBox ID="invoiceNo" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnSearch" CssClass="btn" runat="server" Text="查詢" OnClick="btnSearch_Click" />
        </td>
    </tr>
</table>
<div id="divResult" visible="false" runat="server">
    <uc6:FunctionTitleBar ID="FunctionTitleBar2" runat="server" ItemName="查詢結果" />
    <!--表格 開始-->
    <div class="border_gray">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:PlaceHolder ID="plResult" runat="server"></asp:PlaceHolder>
                <center>
                    <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"
                        Text="查無資料!!" EnableViewState="false"></asp:Label></center>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!--表格 結束-->
    </div>
    <!--按鈕-->
</div>
