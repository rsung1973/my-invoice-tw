<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExportInvoiceItem.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.ExportInvoiceItem" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker" TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<!--路徑名稱-->
<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 > 大平台發票/折讓檔匯出" />
<!--交易畫面標題-->
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="大平台發票/折讓檔匯出" />
<div id="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                大平台發票/折讓檔匯出
            </th>
        </tr>
        <tr>
            <th width="120" nowrap="nowrap">
                日期區間
            </th>
            <td class="tdleft">
                自&nbsp;
                <uc3:CalendarInputDatePicker ID="DateFrom" runat="server" />
                至&nbsp;
                <uc3:CalendarInputDatePicker ID="DateTo" runat="server" />
            </td>
        </tr>
        <tr>
            <th width="120" nowrap="nowrap">
                資料種類
            </th>
            <td class="tdleft">
                <asp:RadioButtonList ID="rbType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Selected="True">電子發票 </asp:ListItem>
                    <asp:ListItem>電子折讓單 </asp:ListItem>
                    <asp:ListItem>作廢電子發票 </asp:ListItem>
                    <asp:ListItem>作廢電子折讓單 </asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td class="Bargain_btn" align="center">
            <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="匯出" />
            &nbsp;
        </td>
    </tr>
</table>
<!--表格 結束-->
<!--按鈕-->
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
