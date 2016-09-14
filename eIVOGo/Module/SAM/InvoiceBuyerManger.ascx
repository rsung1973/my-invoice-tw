<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceBuyerManger.ascx.cs" Inherits="eIVOGo.Module.SAM.InvoiceBuyerManger" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register src="../UI/SocialWelfareSetup.ascx" tagname="SocialWelfareSetup" tagprefix="uc3" %>
<%@ Register src="Business/BuyerList.ascx" tagname="BuyerList" tagprefix="uc4" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc5" %>
<%@ Register src="../UI/InvoiceQuerySellerSelector.ascx" tagname="InvoiceSellerSelector" tagprefix="uc6" %>
<%@ Register src="../Common/DataModelCache.ascx" tagname="DataModelCache" tagprefix="uc7" %>
<%@ Register src="../Common/CalendarInputDatePicker.ascx" tagname="ROCCalendarInput" tagprefix="uc8" %>
<%@ Register Src="~/Module/Common/SaveAsExcelButton.ascx" TagName="SaveAsExcelButton" TagPrefix="uc9" %>

<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="Model" Namespace="Model.Locale" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!--路徑名稱-->
<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 > 發票買受人資料維護" />
<!--交易畫面標題-->
<h1><img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />發票買受人資料維護</h1>
<div id="border_gray" style="margin-top:5px;">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <tr>
            <th>發票開立人</th>
            <td class="tdleft">
                <uc6:InvoiceSellerSelector ID="SellerID" runat="server" SelectorIndication="請選擇" />
            </td>
        </tr>
        <tr>
            <th>
                查詢項目
            </th>
            <td class="tdleft">
                <asp:RadioButtonList ID="rbInvoiceType" RepeatDirection="Horizontal" runat="server"
                    RepeatLayout="Flow" AutoPostBack="True" OnSelectedIndexChanged="rbInvoiceType_SelectedIndexChanged">
                    <asp:ListItem Value="1" Selected="True">B2B</asp:ListItem>
                    <asp:ListItem Value="2">B2C</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th nowrap="nowrap" width="120">
                買受人統編
            </th>
            <td class="tdleft">
                <asp:TextBox ID="ReceiptNo" class="textfield" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th nowrap="nowrap width="120">
                客戶名稱
            </th>
            <td class="tdleft">
                <asp:TextBox ID="CustomerName" class="textfield" runat="server"></asp:TextBox>
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
        <tr>
            <th width="20%">日期區間</th>
            <td class="tdleft">
            自&nbsp;<uc8:ROCCalendarInput ID="DateFrom" runat="server" />
                &nbsp;至&nbsp;<uc8:ROCCalendarInput ID="DateTo" runat="server" />
                <asp:Button ID="btnClean" Text="清除" CssClass="btn" runat="server" 
                    onclick="btnClean_Click" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn" align="center">
            <asp:Button ID="btnQuery" runat="server" class="btn" Text="查詢" OnClick="btnQuery_Click" />
                    
        </td>
    </tr>
</table>
<uc5:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc4:BuyerList ID="itemList" runat="server" Visible="false" />
    </ContentTemplate>
</asp:UpdatePanel>

        <center><uc9:SaveAsExcelButton ID="SaveAsExcelButton1" runat="server" Visible="false" /></center>
        <uc7:DataModelCache ID="modelItem" runat="server" KeyName="InvoiceID" />





