<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UnusedInvoiceList.ascx.cs"
    Inherits="eIVOGo.Module.SYS.UnusedInvoiceList" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/CalendarInput.ascx" TagName="CalendarInput" TagPrefix="uc1" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc3" %>
<%@ Register Src="../UI/InvoiceSellerSelector.ascx" TagName="SellerSelector" TagPrefix="uc4" %>
<%@ Register Src="../UI/TwiceMonthlyPeriod.ascx" TagName="TwiceMonthlyPeriod" TagPrefix="uc5" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc6" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc7" %>
<%@ Register src="../UI/QuerySellerSelector.ascx" tagname="InvoiceSellerSelector" tagprefix="uc8" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Register src="UnusedInvoiceItemList.ascx" tagname="UnusedInvoiceItemList" tagprefix="uc9" %>
<!--隔畖嘿-->
<uc6:PageAction ID="PageAction1" runat="server" ItemName=" > 戳祇布フ腹絏琩高" />
<!--ユ礶夹肈-->
<uc7:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="戳祇布フ腹絏琩高" />
<!--秙-->
<div class="border_gray">
    <!-- 秨﹍-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                琩高兵ン
            </th>
        </tr>
        <tr>
            <th>祇布秨ミ</th>
            <td class="tdleft">
                <uc8:InvoiceSellerSelector ID="SellerID" runat="server"  Postback="true" />
            </td>
        </tr>
        <tr>
            <th width="150" nowrap="nowrap">
                祇布チ瓣
            </th>
            <td class="tdleft">
                <asp:DropDownList ID="SelectYear" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="SellerID_SelectedIndexChanged" >
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th width="150" nowrap="nowrap">
                祇布戳
            </th>
            <td class="tdleft">
                <asp:DropDownList ID="SelectPeriod" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SellerID_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th width="150" nowrap="nowrap">
                瓂
            </th>
            <td class="tdleft">
                <asp:DropDownList ID="SelectTrackCode" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
         <tr id="divUpdate" runat="server" visible="false">
            <th width="150" nowrap="nowrap">
                穝フ祇布参璸
            </th>
            <td class="tdleft">
              <asp:CheckBox ID="UpdateBlankInvoice" runat="server" />
            </td>
        </tr>
    </table>
    <!-- 挡-->
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" runat="server" CssClass="btn" OnClick="btnQuery_Click"
                Text="琩高" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnSettle" runat="server" CssClass="btn"                 Text="穝俱瞶フ祇布" OnClick="btnSettle_Click" />
        </td>
    </tr>
</table>
<uc7:FunctionTitleBar ID="resultTitle" runat="server" ItemName="琩高挡狦" Visible="false" />
<uc9:UnusedInvoiceItemList ID="itemList" runat="server" Visible="false" />
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnDownload" runat="server" CssClass="btn" 
                EnableViewState="false" Text="更フ祇布腹絏" onclick="btnDownload_Click" />&nbsp;&nbsp;
            <asp:Button ID="btnDownloadCSV" runat="server" CssClass="btn" 
                EnableViewState="false" Text="更CSV" onclick="btnDownloadCSV_Click" />
        </td>
    </tr>
</table>
<cc1:UnassignedInvoiceNoDataSource ID="dsEntity" runat="server">
</cc1:UnassignedInvoiceNoDataSource>