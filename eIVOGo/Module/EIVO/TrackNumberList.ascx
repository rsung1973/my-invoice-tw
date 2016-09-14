<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackNumberList.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.TrackDataList" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/CalendarInput.ascx" TagName="CalendarInput" TagPrefix="uc1" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc3" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc4" %>
<%@ Register Src="../UI/TrackCodeYearSelector.ascx" TagName="TrackCodeYearSelector"
    TagPrefix="uc5" %>
<%@ Register Src="../SAM/Action/InvoiceTrackCodeMaintenanceList.ascx" TagName="InvoiceTrackCodeMaintenanceList"
    TagPrefix="uc6" %>
<!--路徑名稱-->
<uc3:PageAction ID="actionItem" runat="server" ItemName="首頁 > 電子發票字軌維護" />
<!--交易畫面標題-->
<uc4:FunctionTitleBar ID="titleBar" runat="server" ItemName="電子發票字軌維護" />
<%--<div id="DIV2" runat="server">--%>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="top_table">
    <tr>
        <td>
            <asp:Button ID="btnAdd" runat="server" Text="新增發票字軌" OnClick="btnAdd_Click" />
        </td>
    </tr>
</table>
<%--</div>--%>
<div id="DIV1" runat="server">
    <div class="border_gray" style="margin-top: 5px;">
        <!--表格 開始-->
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
            <tr>
                <th colspan="2" class="Head_style_a">
                    查詢條件
                </th>
            </tr>
            <tr>
                <th width="150" nowrap="nowrap">
                    <span class="red">*</span> 發票年度（民國年）
                </th>
                <td class="tdleft">
                    <uc5:TrackCodeYearSelector ID="TrackCodeYear" runat="server" />
                </td>
            </tr>
        </table>
        <!--表格 結束-->
    </div>
    <!--按鈕-->
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="Bargain_btn" align="center">
                <asp:Button ID="btnQuery" runat="server" CssClass="btn" OnClick="btnQuery_Click"
                    Text=" 查詢" />
            </td>
        </tr>
    </table>
</div>
<div id="showResult" runat="server" visible="false">
<uc4:FunctionTitleBar ID="resultBar" runat="server" ItemName="查詢結果" />
<div  class="border_gray">
    <uc6:InvoiceTrackCodeMaintenanceList ID="itemList" runat="server" />
</div>
</div>
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
