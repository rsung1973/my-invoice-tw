<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderReturned.ascx.cs" Inherits="eIVOGo.Module.SCM.PurchaseOrderReturned" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register src="../Common/DataModelContainer.ascx" tagname="DataModelContainer" tagprefix="uc3" %>
<%@ Register src="POReturnDetailsEditList.ascx" tagname="POReturnDetailsEditList" tagprefix="uc4" %>

<uc1:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 新增採購退貨單" />
<uc2:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="新增採購退貨單" />
<div class="border_gray">
    <h2>供應商及倉儲</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">選擇供應商及倉儲</th>
        </tr>
        <tr>
            <th width="15%">供應商</th>
            <td class="tdleft">
                <asp:DropDownList ID="ddlSupplier" CssClass="textfield" runat="server">
                    <asp:ListItem>請選擇</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th width="15%">退貨倉儲</th>
            <td class="tdleft">
                <asp:DropDownList ID="ddlWarehouse" CssClass="textfield" runat="server">
                    <asp:ListItem>請選擇</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" CssClass="btn" runat="server" Text="查詢" 
                onclick="btnQuery_Click" />
        </td>
    </tr>
</table>
<div id="divResult" visible="false" runat="server">
    <div id="border_gray">
        <div id="H2Title" runat="server"><h2>退貨料品資訊</h2></div>
        <uc4:POReturnDetailsEditList ID="porDetail" runat="server" />
        &nbsp;
        <!--表格 結束-->
        <center>
            <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
        </center>
    </div>            
    <!--按鈕-->
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnCreatPOR" CssClass="btn" Text="採購退貨單預覽" runat="server" 
                    onclick="btnCreatPOR_Click" />
            </td>
        </tr>
    </table>
</div>
<cc1:PurchaseOrderReturnDataSource ID ="dsPOReturn" runat="server" >
</cc1:PurchaseOrderReturnDataSource>
<uc3:DataModelContainer ID="POReturnContainer" runat="server" />

