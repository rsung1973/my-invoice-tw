<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditMemberMenuItem.ascx.cs"
    Inherits="eIVOGo.Module.SYS.EditMemberMenuItem" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Src="MenuFactory.ascx" TagName="MenuFactory" TagPrefix="uc3" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>


<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 系統維護" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="使用者角色維護" />
<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
--%>        <div class="border_gray">
            <uc3:MenuFactory ID="menuFactory" runat="server" />
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="Bargain_btn" align="center">
                        <asp:Button ID="btnConfirm" runat="server" CausesValidation="false" CssClass="btn"
                             Text="確定" onclick="btnConfirm_Click" />
                        &nbsp;
                        <asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"
                             Text="重設" onclick="btnReset_Click" />
                        &nbsp; &nbsp;
                    </td>
                </tr>
            </table>
        </div>
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>
<cc1:OrganizationCategoryUserRoleDataSource ID="dsRole" runat="server">
</cc1:OrganizationCategoryUserRoleDataSource>

