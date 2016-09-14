<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivateOrganizationToken.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Action.ActivateOrganizationToken" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc3" %>


<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 授權用戶傳輸" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="用戶端傳輸憑證設定" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="border_gray">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="left">
                        <pre id="agreement" runat="server" enableviewstate="false">
    本人授權電子發票用戶端傳輸工具以下述數位憑證，對電子發票、發票折讓、作廢發票、作廢票折讓、
收據、作廢收據等資料上傳至發票集團加值中心時進行電子簽章，用以表示上述資料係由本用戶端所送出，
以致資料之不可否任性及完整性。</pre>
                        <pre>
    <asp:Literal ID="certInfo" runat="server" EnableViewState="false"></asp:Literal></pre>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnConfirm" runat="server" Text="同意" 
                            onclick="btnConfirm_Click" />
                    </td>
                </tr>
            </table>
            <!--表格 開始-->
            <!--表格 結束-->
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<uc3:SignContext ID="signContext" runat="server" Catalog="設定簽章憑證" 
    UsePfxFile="False" />
