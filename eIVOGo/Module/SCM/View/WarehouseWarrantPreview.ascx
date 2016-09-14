<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.SCM.View.EditWarehouseWarrant" %>
<%@ Register Src="../Item/WarehouseWarrantDetailsPreviewList.ascx" TagName="WarehouseWarrantDetailsPreviewList"
    TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register src="../../Common/DataModelContainer.ascx" tagname="DataModelContainer" tagprefix="uc3" %>
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
    <uc1:WarehouseWarrantDetailsPreviewList ID="itemList" runat="server" />
</div>
<cc1:WarehouseWarrantDataSource ID="dsWW" runat="server">
</cc1:WarehouseWarrantDataSource>
<uc3:DataModelContainer ID="modelItem" runat="server" />


