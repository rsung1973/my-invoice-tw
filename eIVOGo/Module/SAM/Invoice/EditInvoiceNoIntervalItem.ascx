<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.SAM.Invoice.InvoiceNoIntervalItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/Common/EnumSelector.ascx" TagName="EnumSelector" TagPrefix="uc4" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc5" %>
<%@ Register Src="TrackCodeYearSelector.ascx" TagName="TrackCodeYearSelector" TagPrefix="uc3" %>
<%@ Register Src="SpecifiedTrackCodePeriodSelector.ascx" TagName="SpecifiedTrackCodePeriodSelector"
    TagPrefix="uc6" %>
<%@ Register Src="TrackCodeSelector.ascx" TagName="TrackCodeSelector" TagPrefix="uc7" %>
<%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
<!--路徑名稱-->
<!--交易畫面標題-->
<!--按鈕-->
<div class="border_gray">
    <!--表格 開始-->
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th class="Head_style_a" colspan="2">
                    修改發票號碼
                </th>
            </tr>
            <tr>
                <th nowrap>
                    <span class="red">*</span> 發票年度（民國年）
                </th>
                <td class="tdleft">
                    <%# _entity.InvoiceTrackCodeAssignment.InvoiceTrackCode.Year%>
                </td>
            </tr>
            <tr>
                <th>
                    <span class="red">*</span> 發票期別
                </th>
                <td class="tdleft">
                    <%# String.Format("{0:00}-{1:00}月",_entity.InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo*2-1,_entity.InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo*2)%>
                </td>
            </tr>
            <tr>
                <th>
                    <span class="red">*</span> 發票字軌
                </th>
                <td class="tdleft">
                    <%# _entity.InvoiceTrackCodeAssignment.InvoiceTrackCode.TrackCode %>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    <span class="red">*</span> 發票號碼起
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="StartNo" runat="server" Columns="8" MaxLength="8" Text='<%# String.Format("{0:00000000}",_entity.StartNo) %>'></asp:TextBox>
                    &nbsp;(8位數)
                </td>
            </tr>
            <tr>
                <th>
                    <span class="red">*</span> 發票號碼迄
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="EndNo" runat="server" Columns="8" MaxLength="8" Text='<%# String.Format("{0:00000000}",_entity.EndNo) %>'></asp:TextBox>
                    &nbsp;(8位數，必需大於發票號碼起)
                </td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn" align="center">
            <asp:Button ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                Text="確定" />
            &nbsp;
            <asp:Button ID="btnCancel" runat="server" CausesValidation="True" CommandName="Update"
                Text="取消" />
            &nbsp;
            <input type="reset" value="重填" class="btn" />
        </td>
    </tr>
</table>
<cc1:InvoiceNoIntervalDataSource ID="dsEntity" runat="server">
</cc1:InvoiceNoIntervalDataSource>
<uc2:DataModelCache ID="modelItem" runat="server" KeyName="IntervalID" />
<uc5:ActionHandler ID="doConfirm" runat="server" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        btnUpdate.OnClientClick = doConfirm.GetPostBackEventReference(null);
    }
</script>
