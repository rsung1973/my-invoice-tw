<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addBuyer_Maintain.ascx.cs" Inherits="eIVOGo.Module.SCM.addBuyer_Maintain" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc5" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc6" %>
<%@ Register src="../Common/PrintingButton2.ascx" tagname="PrintingButton2" tagprefix="uc4" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc4" %>



<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 新增庫存警示" />
<!--交易畫面標題--><uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增庫存警示" />
<div id="border_gray">
    <h2>買受人基本資料</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" id="left_title">
        <tr>
            <th width="15%"><span style="color: red">*</span>買受人名稱：</th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="NAME" runat="server" MaxLength="100"></asp:TextBox>
            </td>
                <th width="15%" class="tdleft">統一編號：</th>
                <td class="tdleft">
                <asp:TextBox ID="BAN" runat="server" MaxLength="8"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="15%"><span style="color: red">*</span>地址：</th>
            <td colspan="3" class="tdleft">
    	        <asp:TextBox ID="ADDR" runat="server" MaxLength="200"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="15%">電話：</th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="PHONE" runat="server" MaxLength="20"></asp:TextBox>
        </td>
            <th width="15%" class="tdleft">行動電話：</th>
            <td class="tdleft">
                <asp:TextBox ID="MOBLIE" runat="server" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="15%" class="tdleft">電子郵件：</th>
            <td colspan="3" class="tdleft">
               <asp:TextBox ID="EMAIL" runat="server" MaxLength="50"></asp:TextBox>
        </td>
      </tr>
    </table>
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">    
            <asp:Button ID="btnOk" class="btn" runat="server" Text="確定" onclick="btnOk_Click" />&nbsp;&nbsp;
            <input id="Reset1" type="reset" value="重填" class="btn" />    
            <asp:HiddenField ID="HiddenField1" runat="server" />    
        </td>
    </tr>
</table>
<div>
    <center>
        <asp:Label ID="lblError"  ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
    </center> 
</div> 