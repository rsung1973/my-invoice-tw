<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.DataModel.ItemSelector" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<div class="border_gray">
    <!--表格 開始-->
    <h2>
        倉儲資訊</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" id="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                選擇採購入庫倉儲
            </th>
        </tr>
        <tr>
            <th width="15%">
                採購入庫倉儲
            </th>
            <td class="tdleft">
                <asp:DropDownList ID="selector" runat="server" DataSourceID="dsEntity" DataTextField="WAREHOUSE_NAME"
                    DataValueField="WAREHOUSE_SN" EnableViewState="false" OnDataBound="selector_DataBound">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<cc1:WarehouseDataSource ID="dsEntity" runat="server">
</cc1:WarehouseDataSource>
<script runat="server">
    protected override void selector_DataBound(object sender, EventArgs e)
    {
        selector.Items.Insert(0, new ListItem("請選擇", ""));
        base.selector_DataBound(sender, e);
    }
</script>
