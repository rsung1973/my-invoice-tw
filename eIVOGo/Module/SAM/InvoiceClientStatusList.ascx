<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceClientStatusList.ascx.cs"
    Inherits="eIVOGo.Module.SAM.InvoiceClientStatusList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker" TagPrefix="uc3" %>
<%@ Register Src="../UI/InvoiceSellerSelector.ascx" TagName="InvoiceSellerSelector" TagPrefix="uc4" %>
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="用戶端連線狀態" />
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
                自&nbsp;&nbsp;<uc3:CalendarInputDatePicker ID="DateFrom" runat="server" />
                &nbsp;
                至&nbsp;&nbsp;<uc3:CalendarInputDatePicker ID="DateTo" runat="server" />
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
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" EnableViewState="True" CssClass="table01"
        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" ShowFooter="False"
        Width="100%" BorderWidth="0px" CellPadding="0" GridLines="None" 
        OnRowCommand="gvEntity_RowCommand" onsorting="gvEntity_Sorting">
        <AlternatingRowStyle CssClass="OldLace" />
        <Columns>
            <asp:TemplateField HeaderText="客戶名稱" SortExpression="CompanyName">
                <ItemTemplate>
                    <%# ((OrganizationStatus)Container.DataItem).Organization.CompanyName%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="上回連線時間" SortExpression="LastTimeToAcknowledge">
                <ItemTemplate>
                    <%# ((OrganizationStatus)Container.DataItem).LastTimeToAcknowledge %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="連線間隔(秒)" SortExpression="RequestPeriodicalInterval">
                <ItemTemplate>
                    <%# ((OrganizationStatus)Container.DataItem).RequestPeriodicalInterval%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="狀態">
                <ItemTemplate>
                    <%# checkStatus((OrganizationStatus)Container.DataItem) %>
                    <asp:Label ID="lblStatus" runat="server" ForeColor='<%# _color %>' Text='<%# _status %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataRowStyle HorizontalAlign="Center" CssClass="noborderline" />
        <EmptyDataTemplate>
            <span style="color: Red;">查無資料!!</span>
        </EmptyDataTemplate>
        <PagerTemplate>
            <uc2:PagingControl ID="pagingList" runat="server" />
        </PagerTemplate>
    </asp:GridView>
</div>
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
<script runat="server">
    internal System.Drawing.Color _color = System.Drawing.Color.Red;
    internal String _status;
    internal String checkStatus(OrganizationStatus status)
    {
        if (status.RequestPeriodicalInterval.HasValue)
        {
            int seconds = (int)(DateTime.Now - status.LastTimeToAcknowledge.Value).TotalSeconds;
            if (seconds < status.RequestPeriodicalInterval.Value)
            {
                _color = System.Drawing.Color.Green;
                _status = "連線正常";
            }
            else if (seconds < status.RequestPeriodicalInterval.Value * 2)
            {
                _color = System.Drawing.Color.Yellow;
                _status = "等待回應";
            }
            else
            {
                _color = System.Drawing.Color.Red;
                _status = "無回應";
            }
        }
        else
        {
            _color = System.Drawing.Color.Yellow;
            _status = "未設定連線週期";
        }
        return null;
    }
</script>