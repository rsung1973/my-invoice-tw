<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TurnKeyLogList.ascx.cs"
    Inherits="eIVOGo.Module.SAM.TurnKeyLogList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker" TagPrefix="uc3" %>
<%@ Register Src="../UI/InvoiceSellerSelector.ascx" TagName="InvoiceSellerSelector" TagPrefix="uc4" %>
<%@ Import Namespace="Utility" %>
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="TurnKey傳送紀錄" />
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
                查詢類別
            </th>
            <td class="tdleft">
                <asp:DropDownList ID="SearchItem" runat="server">
                    <asp:ListItem Value="0">全部</asp:ListItem>
                    <asp:ListItem Value="C0401">電子發票</asp:ListItem>
                    <asp:ListItem Value="D0401">電子折讓單</asp:ListItem>
                    <asp:ListItem Value="C0501">作廢電子發票</asp:ListItem>
                    <asp:ListItem Value="D0501">作廢電子折讓單</asp:ListItem>
                </asp:DropDownList>
                </td>
        </tr>
        <tr>
            <th width="150">
                發票日期區間
            </th>
            <td class="tdleft">
                自&nbsp;&nbsp;<uc3:CalendarInputDatePicker ID="DateFrom" runat="server" />
                &nbsp;
                至&nbsp;&nbsp;<uc3:CalendarInputDatePicker ID="DateTo" runat="server" />
            </td>
        </tr>
        <tr>
            <th width="150">
                傳送日期區間
            </th>
            <td class="tdleft">
                自&nbsp;&nbsp;<uc3:CalendarInputDatePicker ID="MESSAGEDTSFrom" runat="server" />
                &nbsp;
                至&nbsp;&nbsp;<uc3:CalendarInputDatePicker ID="MESSAGEDTSTo" runat="server" />
            </td>
        </tr>
                <tr>
            <th>
                發票／折讓單號碼
            </th>
            <td class="tdleft">
                <asp:TextBox ID="invoiceNo" runat="server"></asp:TextBox>
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
<div id="divResult" visible="false" runat="server">
<uc6:FunctionTitleBar ID="FunctionTitleBar2" runat="server" ItemName="查詢結果" />
     <div id="border_gray">
        
    <asp:GridView ID="gvEntity" runat="server" EnableViewState="True" CssClass="table01"
        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" ShowFooter="False"
        Width="100%" BorderWidth="0px" CellPadding="0" GridLines="None" 
        OnRowCommand="gvEntity_RowCommand">
        <AlternatingRowStyle CssClass="OldLace" />
        <Columns>
            <asp:TemplateField HeaderText="發票號碼" >
                <ItemTemplate>
                    <%# ((V_Logs)Container.DataItem).TrackCode + ((V_Logs)Container.DataItem).No %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="類別" >
                <ItemTemplate>
                    <%# ((V_Logs)Container.DataItem).DocType %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="發票日期" >
                <ItemTemplate>
                    <%# ValueValidity.ConvertChineseDateString(((V_Logs)Container.DataItem).InvoiceDate)%></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="傳送日期" >
                <ItemTemplate>
                    <%#ValueValidity.ConvertChineseDateString(((V_Logs)Container.DataItem).MESSAGE_DTS)%></ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="狀態">
                <ItemTemplate>
                    <%# checkStatus(((V_Logs)Container.DataItem).STATUS) %>
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
</div></div>
<cc1:V_LogsDataSource ID="dsInv" runat="server">
</cc1:V_LogsDataSource>
<script runat="server">
    internal System.Drawing.Color _color = System.Drawing.Color.Green;
    internal String _status;
    internal String checkStatus(String status)
    {
        switch (status)
        {
            case "C":
                _status = "確認 (Server 已確認收到發票)";
                _color = System.Drawing.Color.Green;
                break;
            case "G":
                    _status = "處理完成 (發票已上傳完成)";
                    _color = System.Drawing.Color.Green;
                 break;
            case "P":
                _status = "處理中";
                    _color = System.Drawing.Color.Blue;
                break;
            case "E":
                _status = "錯誤";
                    _color = System.Drawing.Color.Red;
                    break;
            case "I":
                _status = "中斷";
                    _color = System.Drawing.Color.Red;
                    break;
            
            
        }
            
        
        return null;
    }
</script>