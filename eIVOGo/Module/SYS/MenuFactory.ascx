<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuFactory.ascx.cs"
    Inherits="eIVOGo.Module.SYS.MenuFactory" %>
<%@ Register Src="FunctionItem.ascx" TagName="FunctionItem" TagPrefix="uc1" %>
<table class="left_title" width="100%">
    <tr>
        <th>
            功能項目：<uc1:FunctionItem ID="rootItem" runat="server" Visible="false" Enabled="True"
                ReadOnly="True" IsDefault="True" />
        </th>
        <td class="tdleft">
            <uc1:FunctionItem ID="putOrderItem" runat="server" ItemName="採購作業" ActionUrl="~/SCM/PurchaseOrderManagement.aspx" />
            <br />
            <uc1:FunctionItem ID="approveOrderItem" runat="server" ItemName="採購審核作業" ActionUrl="~/SCM/PurchaseOrderVerify.aspx" />
            <br />
            <uc1:FunctionItem ID="returnOrderItem" runat="server" ItemName="採購退貨作業" ActionUrl="~/SCM/PurchaseOrderReturnedMangement.aspx" />
            <br />
            <uc1:FunctionItem ID="buyerOrderItem" runat="server" ItemName="訂單作業" ActionUrl="~/SCM/BuyerOrderManagement.aspx" />
            <br />
            <uc1:FunctionItem ID="storeAwayItem" runat="server" ItemName="入庫作業" ActionUrl="~/SCM/WarehouseWarrantManagement.aspx" />
            <br />
            <uc1:FunctionItem ID="shipItem" runat="server" ItemName="出貨作業" ActionUrl="~/SCM/ShipmentManagement.aspx" />
            <br />
            <uc1:FunctionItem ID="exchangeItem" runat="server" ItemName="換貨作業" ActionUrl="~/SCM/ExchangeGoodsManagement.aspx" />
            <br />
            <uc1:FunctionItem ID="returnItem" runat="server" ItemName="退貨作業" ActionUrl="~/SCM/ReturnedGoodsManagement.aspx" />
            <br />
            <uc1:FunctionItem ID="mailItem" runat="server" ItemName="買受人郵寄資料匯出" ActionUrl="~/SCM/PostBuyerExport.aspx" />
            <br />
            <uc1:FunctionItem ID="inquireStockItem" runat="server" ItemName="庫存存量查詢" ActionUrl="~/SCM/QueryWarehouseAmount.aspx" />
            <br />
            <uc1:FunctionItem ID="inquireDifferentialItem" runat="server" ItemName="庫存異動查詢" ActionUrl="~/SCM/QueryWarehouseTransaction.aspx" />
            <br />
        </td>
    </tr>
</table>
