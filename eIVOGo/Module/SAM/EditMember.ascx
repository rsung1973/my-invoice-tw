<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditMember.ascx.cs"
    Inherits="eIVOGo.Module.SAM.EditMember" %>
<%@ Register Src="../UI/RegisterMessage.ascx" TagName="RegisterMessage" TagPrefix="uc2" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc3" %>
<%@ Register Src="EditUserProfile.ascx" TagName="EditUserProfile" TagPrefix="uc4" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<!--路徑名稱-->


<asp:UpdatePanel ID="Updatepanel1" runat="server">
    <ContentTemplate>
        <div id="mainpage" runat="server">
            <uc3:PageAction ID="actionItem" runat="server" ItemName="會員管理-修改帳號" />
            <!--交易畫面標題-->
            <h1>
                <img id="img4" runat="server" enableviewstate="false" src="~/images/icon_search.gif"
                    width="29" height="28" border="0" align="absmiddle" />會員管理-修改帳號</h1>
            <uc4:EditUserProfile ID="EditItem" runat="server" />
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="Bargain_btn" align="center">
                        <asp:Button ID="btnOK" runat="server" class="btn" Text="確定" OnClick="btnOK_Click" />
                        &nbsp;
                        <input name="Reset" type="reset" class="btn" value="重填" />
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<cc1:UserProfileDataSource ID="dsUserProfile" runat="server">
</cc1:UserProfileDataSource>

