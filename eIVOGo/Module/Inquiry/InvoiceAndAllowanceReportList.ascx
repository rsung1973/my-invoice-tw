<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceAndAllowanceReportList.ascx.cs"
    Inherits="eIVOGo.Module.Inquiry.InvoiceAndAllowanceReportList" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker" TagPrefix="uc2" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc1" %>
<%@ Register Src="../UI/InvoiceSellerSelector.ascx" TagName="SellerSelector" TagPrefix="uc3" %>
<%@ Register Assembly="Uxnet.Com.Net4" Namespace="DataAccessLayer.basis" TagPrefix="cc2" %>
<%@ Register src="../EIVO/InvoiceItemSellerGroupList.ascx" tagname="InvoiceItemSellerGroupList" tagprefix="uc4" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc5" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc6" %>
<!--路徑名稱-->




<uc6:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 發票/折讓統計表" />
<!--交易畫面標題--><uc5:FunctionTitleBar ID="titleBar" runat="server" ItemName="發票/折讓統計表" />
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <tr>
            <th>
                查詢項目
            </th>
            <td class="tdleft">
                <asp:RadioButtonList ID="rbType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Selected="True">電子發票  </asp:ListItem>
                    <asp:ListItem>電子折讓單  </asp:ListItem>
                    <asp:ListItem>作廢電子發票  </asp:ListItem>
                    <asp:ListItem>作廢電子折讓單  </asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th>
                日期區間
            </th>
            <td class="tdleft">
                自<uc2:CalendarInputDatePicker ID="DateFrom" runat="server" />
                &nbsp;&nbsp;至&nbsp;
                <uc2:CalendarInputDatePicker ID="DateTo" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                營 業 人
            </th>
            <td class="tdleft">
                <uc3:SellerSelector ID="SellerID" runat="server" SelectAll="True" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" runat="server" CssClass="btn" OnClick="btnQuery_Click"
                Text=" 查詢" />
        </td>
    </tr>
</table>
<uc5:FunctionTitleBar ID="resultTitleBar" runat="server" ItemName="查詢結果" Visible="false" />
<div id="border_gray" visible="false" runat="server" clientidmode="Static">
    <!--表格 開始-->
    <center>
        <span id="NoData" runat="server" style="color: Red; font-size: Larger;">查無資料!!</span>
    </center>
</div>
<!--表格 結束-->
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <uc1:PrintingButton2 ID="btnPrint" runat="server" Visible="false" />
        </td>
    </tr>
</table>



