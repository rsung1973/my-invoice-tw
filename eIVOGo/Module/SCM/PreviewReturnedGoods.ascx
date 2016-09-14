<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PreviewReturnedGoods.ascx.cs"
    Inherits="eIVOGo.Module.SCM.PreviewReturnedGoods" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc6" %>
<%@ Register src="View/ReturnedGoodsPreview.ascx" tagname="ReturnedGoodsPreview" tagprefix="uc3" %>
<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 退貨單預覽" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="退貨單預覽" />
<uc3:ReturnedGoodsPreview ID="itemView" runat="server" />
<table border="0" cellspacing="0" cellpadding="0" width="100%">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnReturn" runat="server" Text="回上頁" OnClick="btnReturn_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="btnConfirm" runat="server" Text="開立退貨單" OnClick="btnConfirm_Click" />
            </td>
        </tr>
    </tbody>
</table>
<cc1:BuyerOrdersDataSource ID="dsEntity" runat="server">
</cc1:BuyerOrdersDataSource>
<cc1:BuyerShipmentDataSource ID="dsUpdate" runat="server" Isolated="true">
</cc1:BuyerShipmentDataSource>
<uc6:PageAnchor ID="ToCreateReturn" runat="server" TransferTo="~/SCM/EditReturnedGoods.aspx" />
<uc6:PageAnchor ID="ToInquireReturn" runat="server" TransferTo="~/SCM/ReturnedGoodsManagement.aspx" />
