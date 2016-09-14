<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExceptionLogList.ascx.cs"
    Inherits="eIVOGo.Module.SAM.ExceptionLogList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="Model" Namespace="Model.Locale" TagPrefix="cc2" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker" TagPrefix="uc2" %>
<%@ Register Src="../UI/InvoiceSellerSelector.ascx" TagName="InvoiceSellerSelector" TagPrefix="uc4" %>
<!--交易畫面標題-->
<uc1:FunctionTitleBar ID="titleBar" ItemName="異常記錄查詢" runat="server" />
<div id="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <tr>
            <th width="150">
                日期區間
            </th>
            <td class="tdleft">
                自&nbsp;&nbsp;<uc2:CalendarInputDatePicker ID="DateFrom" runat="server" />
                &nbsp;
                至&nbsp;&nbsp;<uc2:CalendarInputDatePicker ID="DateTo" runat="server" />
            </td>
        </tr>
        <tr>
            <th width="150">
                資料類型
            </th>
            <td class="tdleft">
                <asp:DropDownList ID="DocumentType" runat="server">
                    <asp:ListItem Text="全部" Value="" ></asp:ListItem>
                    <asp:ListItem Text="電子發票" Value="10"></asp:ListItem>
                    <asp:ListItem Text="電子發票折讓證明" Value="11"></asp:ListItem>
                    <asp:ListItem Text="作廢電子發票" Value="12"></asp:ListItem>
                    <asp:ListItem Text="作廢電子發票折讓證明" Value="14"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th width="150">
                營 業 人
            </th>
            <td class="tdleft">
                <uc4:InvoiceSellerSelector ID="SellerID" runat="server" SelectAll="True" />
            </td>
        </tr> 
    </table>
</div>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td class="Bargain_btn" colspan="4">
            <asp:Button ID="btnQuery" runat="server" CssClass="btn" OnClick="btnQuery_Click" Text=" 查詢" />
        </td>
    </tr>
    <!--按鈕-->
</table>
<div runat="server" id="result" visible="false">
<div id="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" Width="100%" BorderWidth="0px"
        CellPadding="0" GridLines="None" AutoGenerateColumns="False" CssClass="table01"
        EnableViewState="False" DataSourceID="dsLog">
        <Columns>
             <asp:TemplateField>
                <HeaderTemplate>
                    全選<input id="chkAll" name="chkAll" type="checkbox" onclick="$('input[id$=\'chkItem\']').attr('checked',$('input[id$=\'chkAll\']').is(':checked'));" />
                </HeaderTemplate>
                <ItemTemplate>
                    <input id="chkItem" name="chkItem" type="checkbox" 
                        value='<%# ((ExceptionLog)Container.DataItem).LogID  %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="營業人名稱">
                <ItemTemplate>
                    <%# ((ExceptionLog)Container.DataItem).Organization.CompanyName%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="資料類型">
                <ItemTemplate>
                    <%# ((ExceptionLog)Container.DataItem).DocumentType.TypeName%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="LogTime" HeaderText="時間" ReadOnly="True" />
            <asp:BoundField DataField="Message" HeaderText="內容" ReadOnly="True" />
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
<!--表格 結束-->
</div>
            <table border="0" cellspacing="0" cellpadding="0" width="100%" runat="server" visible="false"
        enableviewstate="false" id="tblAction">
        <tbody>
            <tr>
                <td class="Bargain_btn">
                    <asp:Button ID="btnShow" runat="server" Text="重送" OnClick="btnShow_Click" />&nbsp;&nbsp;
                </td>
            </tr>
        </tbody>
    </table>
</div>
<!--按鈕-->
<cc1:ExceptionLogDataSource ID="dsLog" runat="server">
</cc1:ExceptionLogDataSource>
