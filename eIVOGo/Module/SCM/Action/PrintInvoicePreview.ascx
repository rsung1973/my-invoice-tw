<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintInvoicePreview.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Action.PrintInvoicePreview" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Utility" %>
<%@ Register Src="../../Common/PrintingButton3.ascx" TagName="PrintingButton3" TagPrefix="uc1" %>
<%@ Register Src="../View/BuyerOrderPreview.ascx" TagName="BuyerOrderPreview" TagPrefix="uc2" %>
<%@ Register Src="../View/SingleShipmentPreview.ascx" TagName="SingleShipmentPreview"
    TagPrefix="uc3" %>
<%@ Register Src="../../EIVO/InvoicePaperView.ascx" TagName="InvoicePaperView" TagPrefix="uc4" %>
<asp:button id="btnPopup" runat="server" text="Button" style="display: none" />
<asp:panel id="Panel1" runat="server" style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:panel id="Panel3" runat="server" style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <%--        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        --%>
        <!--路徑名稱-->
        <!--交易畫面標題-->
        <uc4:InvoicePaperView ID="invoiceView" runat="server" />
        <!--按鈕-->
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="Bargain_btn" align="center">
                    <span class="table-title">
                        <%--<uc1:PrintingButton3 ID="btnPrint" runat="server" />--%>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:button id="btnCancel" runat="server" text="關閉" onclick="btnCancel_Click" />
                    </span>
                </td>
            </tr>
        </table>
        <%--            </ContentTemplate>
        </asp:UpdatePanel>
        --%>
    </asp:panel>
</asp:panel>
<asp:modalpopupextender id="ModalPopupExtender" runat="server" targetcontrolid="btnPopup"
    cancelcontrolid="btnCancel" popupcontrolid="Panel1" backgroundcssclass="modalBackground"
    dropshadow="true" popupdraghandlecontrolid="Panel3" />
