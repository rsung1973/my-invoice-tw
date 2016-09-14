<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditUserProfile.ascx.cs" Inherits="eIVOGo.Module.SAM.EditUserProfile" %>
<%@ Register src="../Common/CalendarInputDatePicker.ascx" tagname="CalendarInputDatePicker" tagprefix="uc1" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<div id="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th width="20%">
                <font color="red">*</font> 帳　　號
            </th>
            <td colspan="2" class="tdleft">
                <asp:TextBox ID="txtID" runat="server" Text="<%# DataItem.PID %>" ></asp:TextBox>                
                <asp:Label ID="lblIDWarning" runat="server" ForeColor="Red" Text="新增會員時，帳號由系統產生" 
                    Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <th width="20%">
                <font color="red">*</font> 姓　　名
            </th>
            <td colspan="2" class="tdleft">
                <asp:TextBox ID="txtName" runat="server" size="20" Text="<%# DataItem.UserName %>" />
            </td>
        </tr>
        <tr>
            <th width="20%">
                <font color="red">*</font> 密　　碼
            </th>
            <td width="20%" class="tdleft">
               <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" size="10" /></td>
            <td rowspan="2" class="tdleft">
                <asp:Label ID="lblupdpw" Text="長度最少需要 6 個字元，由英文、數字組成。" runat="server" ></asp:Label><br />
                <asp:Label ID="lblWorning" Text="密碼欄位若保持空白，更新後不修改原密碼。" runat="server" ></asp:Label></td>
        </tr>
        <tr>
            <th width="20%">
                <font color="red">*</font> 重新輸入密碼
            </th>
            <td class="tdleft">
                 <asp:TextBox ID="txtPassword2" TextMode="Password" runat="server" size="10" />
                 <asp:CompareValidator ID="CompareValidator1" runat="server" 
                     ControlToCompare="txtPassword" ControlToValidate="txtPassword2" 
                     ErrorMessage="CompareValidator" ForeColor="Red" Display="Dynamic">密碼輸入內容不同</asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <th width="20%">
                <font color="red">*</font> 常用電子郵件
            </th>
            <td colspan="2" class="tdleft">
                 <asp:TextBox ID="txtEmail" runat="server" size="60" Text="<%# DataItem.EMail %>" />
            </td>
        </tr>
        <tr>
            <th width="20%">
                <font color="red">*</font> 住　　址
            </th>
            <td colspan="2" class="tdleft">
                <asp:TextBox ID="txtAddr" runat="server" size="60" Text="<%# DataItem.Address %>" />
            </td>
        </tr>
        <tr>
            <th width="20%">
                電話（日）
            </th>
            <td colspan="2" class="tdleft">
                <asp:TextBox ID="txtPhone1" runat="server" size="16" Text="<%# DataItem.Phone %>" />
            </td>
        </tr>
        <tr>
            <th width="20%">
                電話（夜）
            </th>
            <td colspan="2" class="tdleft">
                <asp:TextBox ID="txtPhone2" runat="server" size="16" Text="<%# DataItem.UserProfileExtension!=null?DataItem.UserProfileExtension.NightPhone:null %>" />
            </td>
        </tr>
        <tr>
            <th width="20%">
                行動電話
            </th>
            <td colspan="2" class="tdleft">
                <asp:TextBox ID="txtMobilePhone" runat="server" size="16" Text="<%# DataItem.MobilePhone %>" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>

<cc1:InvoiceUserCarrierDataSource ID="dsCarrier" runat="server">
</cc1:InvoiceUserCarrierDataSource>






