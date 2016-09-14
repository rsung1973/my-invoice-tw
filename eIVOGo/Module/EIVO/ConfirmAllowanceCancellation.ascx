<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfirmAllowanceCancellation.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.ConfirmAllowanceCancellation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
       <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
                <!--路徑名稱-->
                <!--交易畫面標題-->
                <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="開立作廢折讓" />
                <pre>
<asp:Literal ID="dataToSign" runat="server"></asp:Literal></pre>
                作廢原因:<br />
                <asp:TextBox ID="reason" runat="server" Rows="5" TextMode="MultiLine" Columns="60"></asp:TextBox>
                <!--按鈕-->
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="Bargain_btn" align="center">
                            <span class="table-title">
                                <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="關閉" 
                                onclick="btnCancel_Click" />
                            </span>
                        </td>
                    </tr>
                </table>
           <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </asp:Panel>
</asp:Panel>
<asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnPopup"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" CancelControlID="btnCancel"
    DropShadow="true" PopupDragHandleControlID="Panel3" />
<uc2:SignContext ID="signContext" runat="server" UsePfxFile="False" Catalog="開立作廢折讓單" />
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
