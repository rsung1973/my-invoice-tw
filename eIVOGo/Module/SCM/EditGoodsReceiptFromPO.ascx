<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditGoodsReceiptFromPO.ascx.cs"
    Inherits="eIVOGo.Module.SCM.EditGoodsReceiptFromPO" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="Item/WarehouseWarrantDetailsEditList.ascx" TagName="WarehouseWarrantDetailsEditList"
    TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register src="../Common/PageAnchor.ascx" tagname="PageAnchor" tagprefix="uc2" %>
<%@ Register src="../Common/DataModelContainer.ascx" tagname="DataModelContainer" tagprefix="uc3" %>


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
    <uc1:WarehouseWarrantDetailsEditList ID="itemList" runat="server" />
</div>
<table border="0" cellspacing="0" cellpadding="0" width="100%">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnReturn" runat="server" Text="回上頁" OnClick="btnReturn_Click" />
                &nbsp;&nbsp;
<asp:Button ID="btnPreview" runat="server" 
                    Text="入庫單預覽" onclick="btnPreview_Click" />
            </td>
        </tr>
    </tbody>
</table>
<cc1:WarehouseWarrantDataSource ID="dsWW" runat="server">
</cc1:WarehouseWarrantDataSource>
<uc2:PageAnchor ID="NextAction" runat="server" TransferTo="~/SCM/PreviewGoodsReceipt.aspx" />
<uc2:PageAnchor ID="PreviousAction" runat="server" TransferTo="~/SCM/WarehouseWarrantManagementFromPO.aspx" />
<uc3:DataModelContainer ID="modelItem" runat="server" />


