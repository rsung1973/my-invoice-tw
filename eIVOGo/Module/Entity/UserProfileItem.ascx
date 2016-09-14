<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileItem.ascx.cs"
    Inherits="eIVOGo.Module.Entity.UserProfileItem" %>
<%@ Register Src="../UI/CaptchaImg.ascx" TagName="CaptchaImg" TagPrefix="uc1" %>
<%@ Register Src="../UI/RegisterMessage.ascx" TagName="RegisterMessage" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<%@ Register Src="../Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc4" %>
<%@ Register Src="../ToolBox/AssignableUserRoleSelector.ascx" TagName="AssignableUserRoleSelector"
    TagPrefix="uc5" %>
<%@ Register Src="../ToolBox/AssignableCategorySelector.ascx" TagName="AssignableCategorySelector"
    TagPrefix="uc6" %>
<%@ Register Src="../ToolBox/OrganizationCategorySelector.ascx" TagName="OrganizationCategorySelector"
    TagPrefix="uc7" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc8" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc9" %>
<!--路徑名稱-->
<asp:UpdatePanel ID="Updatepanel1" runat="server">
    <ContentTemplate>
        <!--交易畫面標題-->
        <div class="border_gray">
            <!--表格 開始-->
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr id="trRole" runat="server">
                    <th width="20%" nowrap="nowrap">
                        <font color="red">*</font>角色別
                    </th>
                    <td class="tdleft">
                        <uc5:AssignableUserRoleSelector ID="RoleID" runat="server" SelectedValue="<%# _entity.UserRole.Count>0? _entity.UserRole[0].RoleID.ToString():null %>" />
                        <uc7:OrganizationCategorySelector ID="OrgaCateID" runat="server" SelectedValue="<%# _entity.UserRole.Count>0? _entity.UserRole[0].OrgaCateID.ToString():null %>" />
                    </td>
                </tr>
                <tr>
                    <th width="20%">
                        <font color="red">*</font> 帳 號
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="PID" runat="server" Text="<%# _entity.PID %>"></asp:TextBox>
                        <asp:Button ID="btnchkid" runat="server" Text="檢查可用的帳號" class="btn" OnClick="btnchkid_Click"                             />
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <th width="20%">
                        <font color="red">*</font> 姓 名
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="UserName" runat="server" size="20" Text="<%# _entity.UserName %>" />
                    </td>
                </tr>
                <tr>
                    <th width="20%">
                        <font color="red">*</font> 密 碼
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="Password" TextMode="Password" runat="server" size="10" />
                        長度最少需要 6 個字元，由英文、數字組成。
                        <asp:Label ID="lblupdpw" Text="(密碼為空白時，為不修改密碼)" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th width="20%">
                        <font color="red">*</font> 重新輸入密碼
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="Password1" TextMode="Password" runat="server" size="10" />
                    </td>
                </tr>
                <tr>
                    <th width="20%">
                        <font color="red">*</font> 常用電子郵件
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="EMail" runat="server" size="60" Text="<%# _entity.EMail %>" />
                    </td>
                </tr>
                <tr>
                    <th width="20%">
                        <font color="red">*</font> 住 址
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="Address" runat="server" size="60" Text="<%# _entity.Address %>" />
                    </td>
                </tr>
                <tr>
                    <th width="20%">
                        電話（日）
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="Phone" runat="server" size="16" Text="<%# _entity.Phone %>" />
                    </td>
                </tr>
                <tr>
                    <th width="20%">
                        電話（夜）
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="Phone2" runat="server" size="16" Text="<%# _entity.Phone2 %>" />
                    </td>
                </tr>
                <tr>
                    <th width="20%">
                        <font color="red">*</font> 行動電話
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="MobilePhone" runat="server" size="16" Text="<%# _entity.MobilePhone %>" />
                    </td>
                </tr>
                <tr id="trCaptchaImg" runat="server">
                    <th width="20%">
                        <font color="red">*</font> 驗 証 碼
                    </th>
                    <td class="tdleft">
                        <uc1:CaptchaImg ID="CaptchaImg1" runat="server" />
                    </td>
                </tr>
            </table>
            <!--表格 結束-->
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="Bargain_btn" align="center">
                    <asp:Button ID="btnOK" runat="server" class="btn" Text="確定" />
                    &nbsp;
                    <input name="Reset" type="reset" class="btn" value="重填" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<uc3:ActionHandler ID="doConfirm" runat="server" />
<uc4:DataModelCache ID="modelItem" runat="server" KeyName="UID" />
<cc1:UserProfileDataSource ID="dsEntity" runat="server">
</cc1:UserProfileDataSource>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        btnOK.OnClientClick = doConfirm.GetPostBackEventReference(null);
    }
</script>
