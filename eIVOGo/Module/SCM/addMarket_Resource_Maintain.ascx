<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addMarket_Resource_Maintain.ascx.cs" Inherits="eIVOGo.Module.SCM.addMarket_Resource_Maintain" %>
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
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 新增網購通路來源" />
<!--交易畫面標題--><uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增網購通路來源" />


<div id="border_gray">
<h2>網購通路來源基本資料</h2>

<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">

  <tr>

    <th width="15%"><span style="color: red">*</span>網購通路來源名稱：</th>

    <td width="35%" class="tdleft">

    	

        <asp:TextBox ID="Name" runat="server"></asp:TextBox>

    	

        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="Name" ErrorMessage="RequiredFieldValidator" ForeColor="Red">請輸入</asp:RequiredFieldValidator>

    	

    </td>

  </tr>

  <tr>

    <th width="15%">備註：</th>

    <td class="tdleft">

    	<asp:TextBox ID="Remark" runat="server" width="400"></asp:TextBox>

    </td>

  </tr>

</table>


<!--按鈕-->

<table width="100%" border="0" cellspacing="0" cellpadding="0">

  <tr>

    <td class="Bargain_btn">
    
        <asp:Button ID="btnOk" class="btn" runat="server" Text="確定" 
            onclick="btnOk_Click" />
&nbsp;<asp:Button ID="btnRest" class="btn" runat="server" Text="重填" 
            onclick="btnRest_Click" />
    
        <asp:HiddenField ID="HiddenField1" runat="server" />
    
    </td>

  </tr>

</table>
<center>
            <asp:Label ID="lblError"  ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
            </center> 
</div> 