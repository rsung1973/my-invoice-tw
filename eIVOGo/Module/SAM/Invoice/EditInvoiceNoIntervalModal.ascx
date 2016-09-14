<%@ Control Language="C#" AutoEventWireup="true"  Inherits="eIVOGo.Module.UI.PopupModal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register src="EditInvoiceNoIntervalItem.ascx" tagname="EditInvoiceNoIntervalItem" tagprefix="uc2" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
<%--        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
--%>                <!--路徑名稱-->
                <!--交易畫面標題-->
                <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="修改發票號碼" />
                <!--按鈕-->
                <div class="border_gray" id="holder" runat="server">
                    <uc2:EditInvoiceNoIntervalItem ID="intervalItem" runat="server" />
                </div>
<%--            </ContentTemplate>
        </asp:UpdatePanel>
--%>    </asp:Panel>
</asp:Panel>
<asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnPopup"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" 
    DropShadow="true" PopupDragHandleControlID="Panel3" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        intervalItem.Done += new EventHandler(intervalItem_Done);
    }

    public override void Show()
    {
        base.Show();
        intervalItem.BindData();
    }
    
    void intervalItem_Done(object sender, EventArgs e)
    {
        this.AjaxAlert("發票號碼區段設定完成!!");
        this.Close();
    }
</script>