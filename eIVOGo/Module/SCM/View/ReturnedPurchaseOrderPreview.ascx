<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReturnedPurchaseOrderPreview.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.ReturnedPurchaseOrderPreview" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Item/POReturnedDetailsEditList.ascx" TagName="POReturnedDetailsEditList"
    TagPrefix="uc4" %>
<%@ Register Src="../../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc1" %>
<%@ Register Src="SupplierDetails.ascx" TagName="SupplierDetails" TagPrefix="uc2" %>
<div class="border_gray">
    <h2>
        採購退貨資訊</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th width="20%">
                採購退貨倉儲
            </th>
            <td class="tdleft">
                <%# _item.WAREHOUSE.WAREHOUSE_NAME %>
            </td>
        </tr>
    </table>
</div>
<uc2:SupplierDetails ID="supplierView" runat="server" />
<uc4:POReturnedDetailsEditList ID="POReturnDetais" runat="server" />
<!--按鈕-->
<cc1:PurchaseOrderReturnDataSource ID="dsEntity" runat="server">
</cc1:PurchaseOrderReturnDataSource>
<uc1:DataModelContainer ID="modelItem" runat="server" />
