<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoodsMaintain.ascx.cs"
    Inherits="eIVOGo.Module.SCM.GoodsMaintain" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc4" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc4" %>
<%@ Register Src="ProductsDataList.ascx" TagName="ProductsDataList" TagPrefix="uc7" %>
<%@ Register src="../Common/DataModelContainer.ascx" tagname="DataModelContainer" tagprefix="uc8" %>
<%@ Register src="../Common/ClearInputField.ascx" tagname="ClearInputField" tagprefix="uc9" %>



<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 料品資料維護" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="料品資料維護" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="top_table">
    <tr>
        <td>
            <asp:Button ID="btnAdd" runat="server" Text="新增料品" class="btn" OnClick="btnAdd_Click" />
        </td>
    </tr>
</table>
<div id="border_gray">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" id="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <tr>
            <th>
                BarCode
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtRno" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                料品名稱
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox><font color="blue">可輸入關鍵字查詢</font>
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" runat="server" Text="查詢" class="btn" OnClick="btnQuery_Click" />&nbsp;&nbsp;
            <uc9:ClearInputField ID="ClearInputField1" runat="server" />
        </td>
    </tr>
</table>
<uc6:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
<uc7:ProductsDataList ID="itemList" runat="server" Visible="false" />
<uc8:DataModelContainer ID="modelItem" runat="server" EnableViewState="false" />