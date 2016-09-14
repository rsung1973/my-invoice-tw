<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SocialWelfareSetup.ascx.cs"
    Inherits="eIVOGo.Module.UI.SocialWelfareSetup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register src="../Common/DataModelCache.ascx" tagname="DataModelCache" tagprefix="uc2" %>
<%@ Register src="../SAM/Business/SocialWelfareSelector.ascx" tagname="SocialWelfareSelector" tagprefix="uc3" %>
<%@ Register src="../Common/ActionHandler.ascx" tagname="ActionHandler" tagprefix="uc4" %>
<asp:UpdatePanel ID="Updatepanel1" runat="server">
    <ContentTemplate>
        <asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
        <asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
            border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
            <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
                border: solid 1px Gray; color: Black">
                <!--路徑名稱-->
                <!--交易畫面標題-->
                <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="社福機構設定" />
                    <uc3:SocialWelfareSelector ID="AgencyID" runat="server" />
                <!--按鈕-->
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="Bargain_btn">
                            <span class="table-title">
                                <asp:Button ID="OkButton" runat="server" Text="確定" />
                                <asp:Button ID="btnCancel" runat="server" Text="取消" />
                            </span>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
        <asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnPopup"
            CancelControlID="btnCancel" PopupControlID="Panel1" BackgroundCssClass="modalBackground"
            DropShadow="true" PopupDragHandleControlID="Panel3" />
    </ContentTemplate>
</asp:UpdatePanel>
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<uc2:DataModelCache ID="modelItem" runat="server" KeyName="CompanyID" />
<uc4:ActionHandler ID="doConfirm" runat="server" />
<uc4:ActionHandler ID="doCancel" runat="server" />
<script runat="server">
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            OkButton.OnClientClick = doConfirm.GetPostBackEventReference(null);
        }
</script>

