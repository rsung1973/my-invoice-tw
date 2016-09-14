<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExportInvoiceItemCA.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.ExportInvoiceItemCA" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker" TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc4" %>
<!--路徑名稱-->
<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 > 大平台發票/折讓檔匯出" />
<!--交易畫面標題-->
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="大平台發票/折讓檔匯出" />
<div id="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                大平台發票/折讓檔匯出
            </th>
        </tr>
        <tr>
            <th width="120" nowrap="nowrap">
                日期區間
            </th>
            <td class="tdleft">
                自&nbsp;
                <uc3:CalendarInputDatePicker ID="DateFrom" runat="server" Required="true" />
                至&nbsp;
                <uc3:CalendarInputDatePicker ID="DateTo" runat="server" Required="true" />
            </td>
        </tr>
        <tr>
            <th width="120" nowrap="nowrap">
                資料種類
            </th>
            <td class="tdleft">
                <asp:RadioButtonList ID="rbType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Selected="True">電子發票 </asp:ListItem>
                    <asp:ListItem>電子折讓單 </asp:ListItem>
                    <asp:ListItem>作廢電子發票 </asp:ListItem>
                    <asp:ListItem>作廢電子折讓單 </asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td class="Bargain_btn" align="center">
            <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="匯出" />
            &nbsp;
        </td>
    </tr>
</table>
<!--表格 結束-->
<!--按鈕-->
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
<uc4:SignContext ID="signContext" runat="server" Catalog="下載大平台發票" 
    UsePfxFile="False" EmptyContentMessage="沒有資料可供下載!!" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        signContext.BeforeSign += new EventHandler(signContext_BeforeSign);
    }

    void signContext_BeforeSign(object sender, EventArgs e)
    {
        switch (rbType.SelectedIndex)
        {
            case 0:
                ((global::ASP.module_common_signcontext_ascx)signContext).Catalog = Model.Locale.Naming.CACatalogDefinition.下載大平台發票;
                break;
            case 1:
                ((global::ASP.module_common_signcontext_ascx)signContext).Catalog = Model.Locale.Naming.CACatalogDefinition.下載大平台折讓;
                break;
            case 2:
                ((global::ASP.module_common_signcontext_ascx)signContext).Catalog = Model.Locale.Naming.CACatalogDefinition.下載大平台作廢發票;
                break;
            case 3:
                ((global::ASP.module_common_signcontext_ascx)signContext).Catalog = Model.Locale.Naming.CACatalogDefinition.下載大平台作廢折讓;
                break;
         
        }
    }
    
</script>
