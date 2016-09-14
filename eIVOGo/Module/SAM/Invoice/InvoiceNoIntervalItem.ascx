<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceNoIntervalItem.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Invoice.InvoiceNoIntervalItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
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
                    新增發票號碼
                </th>
            </tr>
            <tr>
                <th nowrap>
                    <span class="red">*</span> 發票年度（民國年）
                </th>
                <td class="tdleft">
                    <uc3:TrackCodeYearSelector ID="Year" runat="server" SelectedValue="<%# _entity.InvoiceTrackCodeAssignment.InvoiceTrackCode.Year%>" />
                </td>
            </tr>
            <tr>
                <th>
                    <span class="red">*</span> 發票期別
                </th>
                <td class="tdleft">
                    <uc6:SpecifiedTrackCodePeriodSelector ID="PeriodNo" SelectedValue="<%# _entity.InvoiceTrackCodeAssignment.InvoiceTrackCode.TrackID%>"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    <span class="red">*</span> 發票字軌
                </th>
                <td class="tdleft">
                    <uc7:TrackCodeSelector ID="TrackCode" SelectedValue="<%# _entity.InvoiceTrackCodeAssignment.InvoiceTrackCode.TrackID%>"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <th width="20%">
                    <span class="red">*</span> 發票號碼起
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="StartNo" runat="server" Columns="8" MaxLength="8"></asp:TextBox>
                    &nbsp;(8位數)
                </td>
            </tr>
            <tr>
                <th>
                    <span class="red">*</span> 發票號碼迄
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="EndNo" runat="server" Columns="8" MaxLength="8"></asp:TextBox>
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
        Year.Selector.AutoPostBack = true;
        PeriodNo.Selector.AutoPostBack = true;
        PeriodNo.Load += new EventHandler(PeriodNo_Load);
        PeriodNo.Selector.DataBound += new EventHandler(Selector_DataBound);
    }

    void Selector_DataBound(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Year.SelectedValue) && !String.IsNullOrEmpty(PeriodNo.SelectedValue))
        {
            short year = short.Parse(Year.SelectedValue);
            short periodNo = short.Parse(PeriodNo.SelectedValue);
            TrackCode.QueryExpr = t => t.Year == year && t.PeriodNo == periodNo ;
        }
    }

    void PeriodNo_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Year.SelectedValue))
        {
            PeriodNo.QueryExpr = t => t.Year == short.Parse(Year.SelectedValue);
        }
    }
</script>
