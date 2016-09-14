<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.SCM.PreviewGoodsReceiptFromPO" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="Item/WarehouseWarrantDetailsPreviewList.ascx" TagName="WarehouseWarrantDetailsPreviewList"
    TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc2" %>
<%@ Register Src="../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc3" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 新增入庫單" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增入庫單" />
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        倉儲資訊</h2>
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th width="20%">
                    入庫倉儲
                </th>
                <td class="tdleft">
                    <%# _item.WAREHOUSE.WAREHOUSE_NAME %>
                </td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<div class="border_gray">
    <h2>
        料品資訊
    </h2>
    <table id="table01" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <td class="Head_style_a" colspan="12">
                    若入庫數量小於應入庫數量，採購單無法結案
                </td>
            </tr>
        </tbody>
    </table>
    <uc1:WarehouseWarrantDetailsPreviewList ID="itemList" runat="server" />
</div>
<table border="0" cellspacing="0" cellpadding="0" width="100%">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnReturn" runat="server" Text="回上頁" OnClick="btnReturn_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="btnConfirm" runat="server" Text="開立入庫單" OnClick="btnConfirm_Click" />
            </td>
        </tr>
    </tbody>
</table>
<cc1:WarehouseWarrantDataSource ID="dsWW" runat="server">
</cc1:WarehouseWarrantDataSource>
<cc1:WarehouseWarrantDataSource ID="dsUpdate" runat="server" Isolated="true">
</cc1:WarehouseWarrantDataSource>
<uc2:PageAnchor ID="NextAction" runat="server" TransferTo="~/SCM/WarehouseWarrantManagement.aspx" />
<uc2:PageAnchor ID="PreviousAction" runat="server" TransferTo="~/SCM/EditWarrantFromReturn.aspx" />
<uc3:DataModelContainer ID="warrantItem" runat="server" />
