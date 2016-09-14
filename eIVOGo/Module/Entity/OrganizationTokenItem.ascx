<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationTokenItem.ascx.cs"
    Inherits="eIVOGo.Module.Entity.OrganizationTokenItem" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc5" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc2" %>
<div class="border_gray">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title"
        id="tblPrompt" runat="server" enableviewstate="false" visible="false">
        <tr>
            <th class="Head_style_a" align="left">
                使用者憑證:
            </th>
            <td>
                <asp:Button ID="btnUpload" runat="server" Text="建立憑證資訊" CommandName="Upload" OnClick="btnUpload_Click" />
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
                <asp:Button ID="btnConfirm" runat="server" Text="確定" OnClick="btnConfirm_Click" />
            &nbsp;
                <asp:Button ID="btnReset" runat="server" onclick="btnReset_Click" Text="重選憑證" />
            </td>
        </tr>
    </table>
    <!--表格 開始-->
    <!--表格 結束-->
</div>
<uc1:SignContext ID="signContext" runat="server" Catalog="設定簽章憑證" 
    UsePfxFile="False" />
<uc5:ActionHandler ID="doConfirm" runat="server" />
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<uc2:DataModelCache ID="modelItem" runat="server" KeyName="CompanyID" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (modelItem.DataItem != null)
            this.QueryExpr = o => o.CompanyID == (int?)modelItem.DataItem;
        else
            this.QueryExpr = o => o.CompanyID == Business.Helper.WebPageUtility.UserProfile.CurrentUserRole.OrganizationCategory.CompanyID;
    }

</script>
