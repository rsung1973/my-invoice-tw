<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WarehouseMaintainAdd.ascx.cs"
    Inherits="eIVOGo.Module.SCM.WarehouseMaintainAdd" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc4" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc4" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc2" %>
<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 新增倉儲" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增倉儲" />
<div id="border_gray">
    <h2>
        倉儲基本資料</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" id="left_title">
        <tr>
            <th width="15%">
                <span style="color: red">*</span>倉儲名稱：
            </th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="WAREHOUSE_NAME" Text='<%# DataBinder.Eval(_item,"WAREHOUSE_NAME") %>'
                    runat="server" MaxLength="30"></asp:TextBox>
            </td>
            <th width="15%" class="tdleft">
                倉儲代號：
            </th>
            <td class="tdleft">
                <asp:TextBox ID="WAREHOUSE_NUMBER" Text='<%# DataBinder.Eval(_item,"WAREHOUSE_NUMBER") %>'
                    runat="server" MaxLength="30"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="15%">
                地址：
            </th>
            <td colspan="3" class="tdleft">
                <asp:TextBox ID="WAREHOUSE_ADDRESS" Text='<%# DataBinder.Eval(_item,"WAREHOUSE_ADDRESS") %>'
                    runat="server" Columns="80" MaxLength="200"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="15%">
                電話：
            </th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="WAREHOUSE_PHONE" Text='<%# DataBinder.Eval(_item,"WAREHOUSE_PHONE") %>'
                    runat="server" MaxLength="20"></asp:TextBox>
            </td>
            <th width="15%" class="tdleft">
                傳真：
            </th>
            <td class="tdleft">
                <asp:TextBox ID="WAREHOUSE_FAX" Text='<%# DataBinder.Eval(_item,"WAREHOUSE_FAX") %>'
                    runat="server" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="15%">
                聯絡人名稱：
            </th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="WAREHOUSE_CONTACT_NAME" Text='<%# DataBinder.Eval(_item,"WAREHOUSE_CONTACT_NAME") %>'
                    runat="server" MaxLength="64"></asp:TextBox>
            </td>
            <th width="15%" class="tdleft">
                聯絡人電子郵件：
            </th>
            <td class="tdleft">
                <asp:TextBox ID="CONTACT_EMAIL" Text='<%# DataBinder.Eval(_item,"CONTACT_EMAIL") %>'
                    runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <center>
                <asp:Button ID="btnOk" class="btn" runat="server" Text="確定" OnClick="btnOk_Click" />
                <input name="Reset" type="reset" class="btn" value="重填" />
            </center>
        </td>
    </tr>
</table>
<cc2:WarehouseDataSource ID="dsEntity" runat="server">
</cc2:WarehouseDataSource>
