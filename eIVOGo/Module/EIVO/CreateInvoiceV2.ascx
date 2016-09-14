<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateInvoiceV2.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.CreateInvoiceV2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Src="~/Module/EIVO/Lists/EnterpriseGroupMemberList.ascx" TagName="EnterpriseGroupMemberList"
    TagPrefix="uc4" %>
<%@ Register src="~/Module/Common/PageAnchor.ascx" tagname="PageAnchor" tagprefix="uc5" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<%@ Register src="~/Module/Common/DataModelCache.ascx" tagname="DataModelCache" tagprefix="uc6" %>
<%@ Register src="Lists/InvoiceItemDetailForm.ascx" tagname="InvoiceItemDetailForm" tagprefix="uc7" %>
<%@ Register Src="~/Module/Inquiry/BuyerDataList.ascx" TagName="BuyerDataList" TagPrefix="uc8" %>

<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 電子發票開立" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="電子發票開立" />
<div class="border_gray" id="BuyerChosies" runat="server" visible="false">
    <!--表格 開始-->
    <h2>買受人</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">選擇買受人
            </th>
        </tr>
        <tr>
            <th nowrap="nowrap" width="120">
                <font color="red">*</font>買受人
            </th>
            <td class="tdleft">方法1：下拉選單
                <asp:DropDownList ID="BuyerCompany" runat="server" AutoPostBack="True" Visible="true"
                    OnSelectedIndexChanged="BuyerCompany_SelectedIndexChanged">
                    <asp:ListItem Value="">請選擇</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <panel id="NonCSC" runat="server">
                方法2：叫出視窗搜尋
                <asp:Button ID="btnBuyerSelect" runat="server" Text="選取買受人" Visible="true" CausesValidation="false" OnClick="btnBuyerSelect_Click" />
                <br />
                <br />
                方法3：直接輸入搜尋比對
                <asp:TextBox ID="txtBuyerName" runat="server" Enabled="true" Visible="true" BackColor="White" ForeColor="#3333FF" Width="205px"
                    OnTextChanged="txtBuyerName_TextChanged" AutoPostBack="true" />
                <asp:Label ID="lalBuyerMessage" runat="server" Visible="false"  ForeColor="Red" Text="無此買受人資料"/>
                <br />
                <font color="red">*</font>以此顯示的買受人資料為主
                </panel>
            </td>
        </tr>
    </table>
    <uc4:EnterpriseGroupMemberList ID="itemList" runat="server" Visible="false" />
    <!--表格 結束-->
</div>
<div class="border_gray">
    <!--表格 開始-->
    <h2>發票</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="4" class="Head_style_a">
                新增發票主檔
            </th>
        </tr>
        <tr>
            <th nowrap="nowrap" width="150">
                課稅別
            </th>
            <td class="tdleft">
                <asp:RadioButtonList ID="rbType" runat="server" RepeatDirection="Horizontal" 
                    RepeatLayout="Flow" AutoPostBack="True" 
                    onselectedindexchanged="rbType_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="1">應稅 </asp:ListItem>
                    <asp:ListItem Value="2">零稅率 </asp:ListItem>
                    <asp:ListItem Value="3">免稅 </asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <th nowrap="nowrap" width="150">
                買受人簽署適用零稅率註記
            </th>
            <td class="tdleft">
                <asp:DropDownList ID="ddlBondedAreaConfirm" runat="server" Enabled="false">
                    <asp:ListItem Value="0">無</asp:ListItem>
                    <asp:ListItem Value="1">買受人為園區事業</asp:ListItem>
                    <asp:ListItem Value="2">買受人為遠洋漁業</asp:ListItem>
                    <asp:ListItem Value="3">買受人為保稅區(自由貿易港區)</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th nowrap="nowrap" width="150">
                稅率
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtTaxRate" class="textfield" runat="server" ReadOnly></asp:TextBox>%
            </td>
            <th nowrap="nowrap" width="150">
                銷售額合計(新台幣)
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtSellTotal" class="textfield" runat="server" Text="0" ReadOnly></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th nowrap="nowrap" width="150">
                營業稅額
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtTax" class="textfield" runat="server" Text="0" ReadOnly></asp:TextBox>
            </td>
            <th nowrap="nowrap" width="150">
                總計(含稅)
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtTotal" class="textfield" runat="server" Text="0" ReadOnly></asp:TextBox>
            </td>
        </tr>
        <tr id='extra1' runat="server" visible="false">
            <th nowrap="nowrap" width="150">
                部門
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtRepDept" class="textfield" runat="server" Text="F12"></asp:TextBox>
            </td>
            <th nowrap="nowrap" width="150">
                姓名
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtRepName" class="textfield" runat="server" Text="許巧珊"></asp:TextBox>
            </td>
        </tr>
        <tr id='extra2' runat="server" visible="false">
            <th nowrap="nowrap" width="150">
                職編
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtRepEmployeeId" class="textfield" runat="server" Text="007843"></asp:TextBox>
            </td>
            <th nowrap="nowrap" width="150">
                溝通編號
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtRepNumber" class="textfield" runat="server" Text="007843"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>
<div class="border_gray">
    <!--表格 開始-->
    <h2>發票明細</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="4" class="Head_style_a">
                新增發票明細
            </th>
        </tr>
    </table>
    <uc7:InvoiceItemDetailForm ID="InvoiceItemDetailForm1" runat="server" />
</div>
<table border="0" cellspacing="0" cellpadding="0" width="100%">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnPreview" runat="server" Text="發票預覽" class="btn" 
                    onclick="btnPreview_Click" />
            </td>
        </tr>
    </tbody>
</table>
<uc6:DataModelCache ID="ContentItem" runat="server" KeyName="content" />
<uc6:DataModelCache ID="ListItem" runat="server" KeyName="list" />
<uc3:ActionHandler ID="doConfirm" runat="server" />
<uc3:ActionHandler ID="doShow" runat="server" />

<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
<uc5:PageAnchor ID="NextAction" runat="server" RedirectTo="CreateInvoicePreview.aspx" />
<uc8:BuyerDataList runat="server" id="dsBuyerDataList" Visible="false"/>
