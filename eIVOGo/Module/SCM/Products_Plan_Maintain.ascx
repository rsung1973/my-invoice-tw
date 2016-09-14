<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Products_Plan_Maintain.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Products_Plan_Maintain" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<%@ Register Src="../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc8" %>
<%@ Register Src="../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc7" %>
<%@ Register Src="View/ProductWarehouseList.ascx" TagName="ProductWarehouseList"
    TagPrefix="uc9" %>
<%@ Register Src="Item/WarehouseSelector.ascx" TagName="WarehouseSelector" TagPrefix="uc10" %>
<%@ Register src="../Common/ClearInputField.ascx" tagname="ClearInputField" tagprefix="uc1" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 庫存警示維護作業" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="庫存警示維護作業" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="top_table">
    <tr>
        <td>
            <asp:Button ID="btnAdd" runat="server" Text="新增庫存警示" class="btn" OnClick="btnAdd_Click" Visible="false" />
        </td>
    </tr>
</table>
<div class="border_gray">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" id="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <tr>
            <th>
                倉儲
            </th>
            <td class="tdleft">
                <uc10:WarehouseSelector ID="warehouse" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                BarCode
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtBarCode" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                料品名稱
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox><font color="blue">可輸入關鍵字查詢</font>
            </td>
        </tr>
        <tr>
            <th>
                設定狀態
            </th>
            <td class="tdleft">
                <asp:RadioButtonList ID="rdbStatus" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                    <asp:ListItem Selected="True">全部</asp:ListItem>
                    <asp:ListItem>已設定</asp:ListItem>
                    <asp:ListItem>未設定</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" runat="server" Text="查詢" class="btn" OnClick="btnQuery_Click" />
            &nbsp;&nbsp;
            <uc1:ClearInputField ID="ClearInputField1" runat="server" />
        </td>
    </tr>
</table>
<uc6:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
<uc9:ProductWarehouseList ID="itemList" runat="server" Visible="false" />
<uc8:DataModelContainer ID="modelItem" runat="server" EnableViewState="false" />
<uc7:PageAnchor ID="ToAddSalesPromo" runat="server" 
    TransferTo="~/SCM_SYSTEM/addProducts_Plan_Maintain.aspx" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        warehouse.Selector.DataBound += new EventHandler(Selector_DataBound);
    }

    void Selector_DataBound(object sender, EventArgs e)
    {
        warehouse.Selector.Items.Insert(0, new ListItem("全部", ""));
        warehouse.Selector.SelectedIndex = 0;
    }
</script>