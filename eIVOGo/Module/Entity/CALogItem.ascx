<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CALogItem.ascx.cs"
    Inherits="eIVOGo.Module.Entity.CALogItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/EnumSelector.ascx" TagName="EnumSelector" TagPrefix="uc4" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc2" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc5" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<%@ Register src="~/Module/SAM/CAContentDetail.ascx" tagname="CAContentDetail" tagprefix="uc3" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Register src="../SAM/CAContentDetailPKCS7.ascx" tagname="CAContentDetailPKCS7" tagprefix="uc6" %>
<%@ Register src="../SAM/CAContentDetailXmlSig.ascx" tagname="CAContentDetailXmlSig" tagprefix="uc7" %>
<%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
<!--路徑名稱-->
<!--交易畫面標題-->
<!--按鈕-->
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <%--        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        --%>
        <!--路徑名稱-->
        <!--交易畫面標題-->
        <!--按鈕-->
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="Bargain_btn" align="center">
                    <asp:Button ID="btnCancel" runat="server" Text="關閉" />
                </td>
            </tr>
        </table>
        <%--            </ContentTemplate>
        </asp:UpdatePanel>
        --%>
    </asp:Panel>
</asp:Panel>
<asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnPopup"
    CancelControlID="btnCancel" PopupControlID="Panel1" BackgroundCssClass="modalBackground"
    DropShadow="true" PopupDragHandleControlID="Panel3" />
<uc5:ActionHandler ID="doCancel" runat="server" />
<uc5:ActionHandler ID="doConfirm" runat="server" />
<cc1:CALogDataSource ID="dsEntity" runat="server">
</cc1:CALogDataSource>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        btnCancel.OnClientClick = doCancel.GetPostBackEventReference(null);
    }

    public override void BindData()
    {
        loadEntity();

        UserControl uc;
        switch ((Naming.CACatalogDefinition)_entity.Catalog.Value)
        { 
            case Naming.CACatalogDefinition.UXGW上傳資料:
                uc = (UserControl)this.LoadControl("~/Module/SAM/CAContentDetail.ascx");
                ((ASP.module_sam_cacontentdetail_ascx)uc).DataItem = _entity;
                break;
            case Naming.CACatalogDefinition.UXGW自動接收:
                uc = (UserControl)this.LoadControl("~/Module/SAM/CAContentDetailXmlSig.ascx");
                ((ASP.module_sam_cacontentdetailxmlsig_ascx)uc).DataItem = _entity;
                break;
            default:
                uc = (UserControl)this.LoadControl("~/Module/SAM/CAContentDetailPKCS7.ascx");
                ((ASP.module_sam_cacontentdetailpkcs7_ascx)uc).DataItem = _entity;
                break;
        }
        
        uc.InitializeAsUserControl(this.Page);
        Panel3.Controls.AddAt(0, uc);

        this.DataBind();
        this.Visible = true;
        this.ModalPopupExtender.Show();
    }
</script>
