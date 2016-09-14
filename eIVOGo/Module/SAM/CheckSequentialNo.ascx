<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckSequentialNo.ascx.cs"
    Inherits="eIVOGo.Module.SAM.CheckSequentialNo" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="Model" Namespace="Model.Locale" TagPrefix="cc2" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker" TagPrefix="uc2" %>
<%@ Register Src="../UI/InvoiceSellerSelector.ascx" TagName="SellerSelector" TagPrefix="uc4" %>
<!--交易畫面標題-->
<uc1:FunctionTitleBar ID="titleBar" ItemName="發票號碼連號檢查" runat="server" />
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <tr>
            <th width="150">
                起始發票號碼
            </th>
            <td class="tdleft">
                <asp:TextBox ID="StartNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="150">
                結尾發票號碼
            </th>
            <td class="tdleft">
                <asp:TextBox ID="EndNo" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    
    <!--表格 結束-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td class="Bargain_btn" colspan="4">
            <asp:Button ID="btnQuery" runat="server" CssClass="btn" OnClick="btnQuery_Click" Text=" 查詢" />
        </td>
    </tr>
    <!--按鈕-->
</table>
</div>
<asp:GridView ID="gvEntity" runat="server" AllowPaging="True" Width="100%" BorderWidth="0px"
        CellPadding="0" GridLines="None" AutoGenerateColumns="False" CssClass="table01"
        EnableViewState="False" >
        <Columns>
            <asp:TemplateField HeaderText="下列發票號碼不存在於系統中">
                <ItemTemplate>
                    <%# Container.DataItem %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <AlternatingRowStyle CssClass="OldLace" />
        <EmptyDataRowStyle HorizontalAlign="Center" CssClass="noborderline" />
        <EmptyDataTemplate>
            <asp:Label ID="empty" runat="server" EnableViewState="false" ForeColor="Red" Visible='<%# !String.IsNullOrEmpty(btnQuery.CommandArgument) %>' Text="查無資料!!"></asp:Label>
        </EmptyDataTemplate>
        <PagerTemplate>
            <uc2:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
    </asp:GridView>
<!--按鈕-->
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
