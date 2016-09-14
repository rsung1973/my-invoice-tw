<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InquireInvoiceToResend.ascx.cs"
    Inherits="eIVOGo.Module.Inquiry.InquireInvoiceToResend" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/UrlRadioDirective.ascx" TagName="UrlRadioDirective" TagPrefix="uc8" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceSeller.ascx" TagPrefix="uc4" TagName="InquireInvoiceSeller" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceDate.ascx" TagPrefix="uc4" TagName="InquireInvoiceDate" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceNo.ascx" TagPrefix="uc4" TagName="InquireInvoiceNo" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireCustomerID.ascx" TagPrefix="uc4" TagName="InquireCustomerID" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireSMSNotification.ascx" TagPrefix="uc4" TagName="InquireSMSNotification" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagPrefix="uc4" TagName="DataModelCache" %>
<%@ Register Src="~/Module/Inquiry/QueryResultInfo.ascx" TagPrefix="uc4" TagName="QueryResultInfo" %>
<%@ Register Src="~/Module/Base/InvoiceItemCheckList.ascx" TagPrefix="uc4" TagName="InvoiceItemCheckList" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/SMSNotification.ascx" TagPrefix="uc4" TagName="SMSNotification" %>


<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Module.Base" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 重送開立發票通知" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="functionTitle" runat="server" ItemName="重送開立發票通知" />
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <uc8:UrlRadioDirective ID="urlGo" runat="server" DefaultName="電子發票" />
        <asp:PlaceHolder ID="inquiryHolder" runat="server">
            <uc4:InquireInvoiceSeller runat="server" ID="inquireSeller" />
            <uc4:InquireCustomerID runat="server" ID="inquireCustomerID" />
            <uc4:InquireInvoiceDate runat="server" ID="inquireDate" />
            <uc4:InquireInvoiceNo runat="server" ID="inquireNo" />
        </asp:PlaceHolder>
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" CssClass="btn" runat="server" Text="查詢" OnClick="btnQuery_Click" />
        </td>
    </tr>
</table>
<uc6:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <!--表格 開始-->
        <div class="border_gray" id="resultInfo" visible="false" enableviewstate="false" runat="server">
            <uc4:QueryResultInfo runat="server" ID="queryInfo" />
            <uc4:InvoiceItemCheckList runat="server" ID="itemList" />
            <table id="tblAction" runat="server" enableviewstate="false" width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="Bargain_btn">
                        <asp:Button ID="btnShow" runat="server" Text="重送郵件通知" OnClick="btnShow_Click" />&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <!--按鈕-->
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnQuery" />
    </Triggers>
</asp:UpdatePanel>
<uc4:DataModelCache runat="server" ID="modelItem" KeyName="defaultEntity" />
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        ((ASP.module_inquiry_inquiryitem_urlradiodirective_ascx)urlGo).NamingDirection
            = new String[] { 
                "電子發票", "~/SAM/ResendInvoicesMail.aspx"};

        var field = new TemplateField
        {
            HeaderText = "簡訊通知",
            ItemTemplate = new Uxnet.Web.WebUI.ItemTemplateCreator
            {
                BuildControl = () => new ASP.module_eivo_invoicefield_smsnotification_ascx()
            }
        };
        field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        itemList.Grid.Columns.Add(field);

    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        String[] ar = Request.GetItemSelection();
        if (ar != null && ar.Count() > 0)
        {
            ar.Select(a => int.Parse(a)).SendIssuingNotification();
            UpdatePanel1.AjaxAlert("Email通知已重送!!");
        }
        else
        {
            UpdatePanel1.AjaxAlert("請選擇重送資料!!");
        }
    }
    
</script>
