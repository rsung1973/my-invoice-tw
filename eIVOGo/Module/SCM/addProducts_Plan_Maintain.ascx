<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addProducts_Plan_Maintain.ascx.cs"
    Inherits="eIVOGo.Module.SCM.addProducts_Plan_Maintain" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc4" %>
<%@ Register Src="Item/ProductQuery.ascx" TagName="ProductQuery" TagPrefix="uc7" %>
<%@ Register Src="Item/ProductQueryByField.ascx" TagName="ProductQueryByField" TagPrefix="uc8" %>
<%@ Register Src="Item/StockAlertEditList.ascx" TagName="StockAlertEditList" TagPrefix="uc9" %>
<%@ Register Src="../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc10" %>
<%@ Register Src="Item/WarehouseSelector.ascx" TagName="WarehouseSelector" TagPrefix="uc11" %>
<%@ Register Src="../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc7" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 新增庫存警示維護資料" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增庫存警示維護資料" />
<div id="border_gray">
    <h2>
        倉儲設定</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                選擇料品倉儲
            </th>
        </tr>
        <tr>
            <th width="15%">
                料品倉儲
            </th>
            <td class="tdleft">
                <uc11:WarehouseSelector ID="warehouse" runat="server" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<!--表格 開始-->
<div class="border_gray" runat="server" visible="false">
    <h2>
        料品庫存警示資訊</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="4" class="Head_style_a">
                新增料品庫存警示
            </th>
        </tr>
        <tr>
            <th width="15%">
                料品Barcode
            </th>
            <td width="35%" class="tdleft">
                &nbsp;<asp:TextBox ID="Barcode" runat="server"></asp:TextBox>
                <asp:Button ID="btnAdd" class="btn" runat="server" Text="新增料品" OnClick="btnAdd_Click" />
            </td>
            <th width="15%" class="tdleft">
                料品名稱
            </th>
            <td class="tdleft">
                <uc8:ProductQueryByField ID="productQuery" runat="server" />
            </td>
        </tr>
    </table>
</div>
<uc9:StockAlertEditList ID="mappingDetails" runat="server" />
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnOk" class="btn" runat="server" Text="確定" OnClick="btnOk_Click" />
        </td>
    </tr>
</table>
<cc1:WarehouseDataSource ID="dsEntity" runat="server">
</cc1:WarehouseDataSource>
<cc1:WarehouseDataSource ID="dsUpdate" runat="server">
</cc1:WarehouseDataSource>
<uc10:DataModelContainer ID="modelItem" runat="server" />
<uc7:PageAnchor ID="ToInquireWarehouseMapping" runat="server" TransferTo="~/SCM_SYSTEM/Products_Plan_Maintain.aspx" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //warehouse.Selector.AutoPostBack = true;
        //warehouse.SelectedIndexChanged += new EventHandler(Selector_SelectedIndexChanged);
        this.warehouse.Selector.DataBound += new EventHandler(Selector_DataBound);
        
    }

    void Selector_DataBound(object sender, EventArgs e)
    {
        this.warehouse.Selector.Items.Insert(0, new ListItem("請選擇", ""));
        if (_item != null && _item.DataFrom != Model.Locale.Naming.DataItemSource.FromDB)
            this.warehouse.Selector.SelectedIndex = 0;
    }

    void Selector_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(warehouse.SelectedValue))
        {
            PrepareDataFromDB(int.Parse(warehouse.SelectedValue));
        }
    }

</script>
