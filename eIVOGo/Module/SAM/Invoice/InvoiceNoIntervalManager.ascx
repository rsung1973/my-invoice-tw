<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceNoIntervalManager.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Invoice.InvoiceNoIntervalManager" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc3" %>
<%@ Register Src="~/Module/SAM/Invoice/InvoiceNoIntervalNoList.ascx" TagName="InvoiceNoIntervalNoList"
    TagPrefix="uc4" %>
<%@ Register Src="~/Module/Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc5" %>
<%@ Register src="TrackCodeYearSelector.ascx" tagname="TrackCodeYearSelector" tagprefix="uc6" %>
<%@ Register src="TrackCodePeriodSelector.ascx" tagname="TrackCodePeriodSelector" tagprefix="uc7" %>
<%@ Register src="../../Common/DataModelCache.ascx" tagname="DataModelCache" tagprefix="uc8" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<%@ Register src="../../Common/CrossPageMessage.ascx" tagname="CrossPageMessage" tagprefix="uc9" %>


<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 電子發票號碼維護" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="電子發票號碼維護" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="top_table">
    <tr>
        <td>
            <asp:Button ID="btnAdd" runat="server" class="btn" Text="新增發票號碼" OnClick="btnAdd_Click" />
        </td>
    </tr>
</table>
<div class="border_gray">
    <!--表格 開始-->
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th class="Head_style_a" colspan="2">
                    查詢條件
                </th>
            </tr>
            <tr>
                <th width="20%" nowrap>
                    發票年度（民國年）
                </th>
                <td class="tdleft">
                    <uc6:TrackCodeYearSelector ID="year" runat="server" SelectorIndication="全部" />
                </td>
            </tr>
            <tr>
                <th>
                    發票期別
                </th>
                <td class="tdleft">
                    <uc7:TrackCodePeriodSelector ID="periodNo" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<table border="0" cellspacing="0" cellpadding="0" width="100%">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnQuery" runat="server" Text="查詢" class="btn" OnClick="btnQuery_Click" />
            </td>
        </tr>
    </tbody>
</table>
<uc2:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
<uc4:InvoiceNoIntervalNoList ID="itemList" runat="server" Visible="false" />
<uc5:PageAnchor ID="ToEdit" runat="server" TransferTo="~/SAM/CreateInvoiceNoInterval.aspx" />
<uc8:DataModelCache ID="modelItem" runat="server" KeyName="IntervalID" />
<cc1:InvoiceNoIntervalDataSource ID="dsEntity" 
    runat="server">
</cc1:InvoiceNoIntervalDataSource>
<uc9:CrossPageMessage ID="pageMsg" runat="server" />

<script runat="server">
    protected override void OnPreRender(EventArgs e)
    {
        int? intervalID = (int?)modelItem.DataItem;
        if(intervalID.HasValue)
        {
            var item = dsEntity.CreateDataManager().EntityList.Where(i => i.IntervalID == intervalID.Value).FirstOrDefault();
            if (item != null)
            {
                year.SelectedValue = item.InvoiceTrackCodeAssignment.InvoiceTrackCode.Year.ToString();
                periodNo.SelectedValue = item.InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo.ToString();
                btnQuery_Click(this, null);
            }
        }

        base.OnPreRender(e);
    }
</script>


