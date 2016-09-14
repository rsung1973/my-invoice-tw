<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationEntrustingTokenItem.ascx.cs"
    Inherits="eIVOGo.Module.Entity.OrganizationEntrustingTokenItem" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc5" %>
<div class="border_gray">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th class="Head_style_a" align="left">
                PKCS12(PFX)憑證檔:<br />
                <asp:FileUpload ID="PfxFile" runat="server" />
            </th>
            <td>
                PIN Code:<asp:TextBox ID="PIN" runat="server" TextMode="Password"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnConfirm" runat="server" Text="上載"  OnClick="btnConfirm_Click" />
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title"
        id="tblAction" runat="server" visible="false" enableviewstate="false">
        <tr>
            <th class="Head_style_a">
                憑證明細:
            </th>
            <td>
                <asp:Literal ID="certMsg" runat="server" EnableViewState="false"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th class="Head_style_a">
                設定電子簽章憑證:
            </th>
            <td>
                <asp:Button ID="btnUpload" runat="server" Text="建立憑證資訊" CommandName="Upload" 
                    onclick="btnUpload_Click"  />
                &nbsp;
                <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="重選憑證" />
            </td>
        </tr>
    </table>
    <!--表格 開始-->
    <!--表格 結束-->
</div>
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<uc1:SignContext ID="signContext" runat="server" />
<uc5:ActionHandler ID="doConfirm" runat="server" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _userProfile = Business.Helper.WebPageUtility.UserProfile;
        this.QueryExpr = o => o.CompanyID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID;
    }
</script>
