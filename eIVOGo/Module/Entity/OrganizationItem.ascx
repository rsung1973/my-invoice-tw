<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationItem.ascx.cs"
    Inherits="eIVOGo.Module.Entity.OrganizationItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc5" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
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
        <div class="border_gray" id="holder" runat="server">
            <!--表格 開始-->
            <table id="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    <tr>
                        <th width="100">
                            營業人名稱
                        </th>
                        <td class="tdleft">
                            <%# _entity.CompanyName %>
                        </td>
                        <th width="100">
                            統一編號
                        </th>
                        <td class="tdleft">
                            <%# _entity.ReceiptNo %>
                        </td>
                    </tr>
                    <tr>
                        <th width="100">
                            <span class="tdleft">電話</span>
                        </th>
                        <td class="tdleft">
                            <%# _entity.Phone %>
                        </td>
                        <th width="100">
                            <span class="tdleft">聯絡人電子郵件</span>
                        </th>
                        <td class="tdleft">
                            <%# _entity.ContactEmail %>
                        </td>
                    </tr>
                    <tr>
                        <th width="100">
                            地址
                        </th>
                        <td class="tdleft" colspan="3">
                            <%# _entity.Addr %>
                        </td>
                    </tr>
                </tbody>
            </table>
            <!--表格 結束-->
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="Bargain_btn" align="center">
                    &nbsp;<asp:Button ID="btnCancel" runat="server" Text="關閉" />
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
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<uc5:ActionHandler ID="doCancel" runat="server" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        btnCancel.OnClientClick = doCancel.GetPostBackEventReference(null);
    }
</script>
