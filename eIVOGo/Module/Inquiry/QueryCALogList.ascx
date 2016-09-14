<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryCALogList.ascx.cs"
    Inherits="eIVOGo.Module.Inquiry.QueryCALogList" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc3" %>
<%@ Register Src="../UI/CAUserSelector.ascx" TagName="CAUserSelector" TagPrefix="uc4" %>
<%@ Register Src="../UI/PupopModalFrame.ascx" TagName="PupopModalFrame" TagPrefix="uc5" %>
<%@ Register src="../Common/EnumSelector.ascx" tagname="EnumSelector" tagprefix="uc6" %>
<%@ Register src="../ListView/CALogList.ascx" tagname="CALogList" tagprefix="uc7" %>


<!--路徑名稱-->
<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 > 查詢憑證使用紀錄" />
<!--交易畫面標題-->
<uc3:FunctionTitleBar ID="titleBar" runat="server" ItemName="查詢憑證使用紀錄" />
<div id="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <tr>
            <th width="150">
                日期區間
            </th>
            <td class="tdleft">
                自&nbsp;&nbsp;<uc2:CalendarInputDatePicker ID="DateFrom" runat="server" />
                &nbsp;
                至&nbsp;&nbsp;<uc2:CalendarInputDatePicker ID="DateTo" runat="server" />
            </td>
        </tr>
        <tr>
            <th width="150">
                簽章明文類別
            </th>
            <td class="tdleft">
                <uc6:EnumSelector ID="CACatalog" runat="server" TypeName="Model.Locale.Naming+B2BCACatalogQueryDefinition, Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" SelectorIndication="全部" />
            </td>
        </tr>
        <tr>
            <th width="150">
                營 業 人
            </th>
            <td class="tdleft">
                <uc4:CAUserSelector ID="SellerID" runat="server" SelectAll="True" SelectorIndication="全部" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td class="Bargain_btn" align="center">
            <asp:Button ID="btnQuery" runat="server" CssClass="btn" OnClick="btnQuery_Click" Text=" 查詢" />
        </td>
    </tr>
</table>
<!--按鈕-->
<uc3:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
<uc7:CALogList ID="itemList" runat="server" Visible="false"/>

