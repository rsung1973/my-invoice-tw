<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PreviewShipmentFromExchange.ascx.cs"
    Inherits="eIVOGo.Module.SCM.PreviewShipmentFromExchange" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc6" %>
<%@ Register src="View/SingleShipmentPreviewFromExchange.ascx" tagname="SingleShipmentPreviewFromExchange" tagprefix="uc3" %>
<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 出貨單預覽" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="出貨單預覽" />
<uc3:SingleShipmentPreviewFromExchange ID="shipment" runat="server" />
<table border="0" cellspacing="0" cellpadding="0" width="100%">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnReturn" runat="server" Text="回上頁" OnClick="btnReturn_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="btnConfirm" runat="server" Text="開立出貨單&列印銷貨憑單" OnClick="btnConfirm_Click" />
            </td>
        </tr>
    </tbody>
</table>
<cc1:BuyerOrdersDataSource ID="dsEntity" runat="server">
</cc1:BuyerOrdersDataSource>
<cc1:BuyerShipmentDataSource ID="dsUpdate" runat="server" Isolated="true">
</cc1:BuyerShipmentDataSource>
<uc6:PageAnchor ID="ToPreviewShipment" runat="server" TransferTo="~/SCM/EditShipmentFromExchange.aspx" />
<uc6:PageAnchor ID="ToInquireShipment" runat="server" TransferTo="~/SCM/ShipmentManagement.aspx" />
