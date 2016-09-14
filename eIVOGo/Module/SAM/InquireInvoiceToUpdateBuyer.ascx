<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquireInvoiceBasic" %>
<%@ Register Src="~/Module/Inquiry/InvoiceBuyerList.ascx" TagName="InvoiceBuyerList" TagPrefix="uc4" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/UrlRadioDirective.ascx" TagName="UrlRadioDirective" TagPrefix="uc8" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceSeller.ascx" TagPrefix="uc4" TagName="InquireInvoiceSeller" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceDate.ascx" TagPrefix="uc4" TagName="InquireInvoiceDate" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceNo.ascx" TagPrefix="uc4" TagName="InquireInvoiceNo" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireCustomerID.ascx" TagPrefix="uc4" TagName="InquireCustomerID" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireSMSNotification.ascx" TagPrefix="uc4" TagName="InquireSMSNotification" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagPrefix="uc4" TagName="DataModelCache" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceConsumption.ascx" TagPrefix="uc4" TagName="InquireInvoiceConsumption" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceBuyerNo.ascx" TagPrefix="uc4" TagName="InquireInvoiceBuyerNo" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceBuyerName.ascx" TagPrefix="uc4" TagName="InquireInvoiceBuyerName" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceType.ascx" TagPrefix="uc4" TagName="InquireInvoiceType" %>
<%@ Import Namespace="eIVOGo.Module.Base" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 發票買受人資料維護" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="functionTitle" runat="server" ItemName="發票買受人資料維護" />
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">查詢條件
            </th>
        </tr>
        <asp:PlaceHolder ID="inquiryHolder" runat="server">
            <uc4:InquireInvoiceType runat="server" id="InquireInvoiceType" />
            <uc4:InquireInvoiceSeller runat="server" ID="inquireSeller" QueryRequired="true" AlertMessage="請選擇發票開立人!!" />
            <uc4:InquireInvoiceConsumption runat="server" ID="inquireConsumption" />
            <uc4:InquireInvoiceDate runat="server" ID="inquireDate" QueryRequired="false" AlertMessage="請指定日期區間!!" />
            <uc4:InquireInvoiceNo runat="server" ID="inquireNo" />
            <uc4:InquireInvoiceBuyerNo runat="server" ID="inquireBuyerNo" />
            <uc4:InquireInvoiceBuyerName runat="server" ID="inquireBuyerName" />
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
            <uc4:InvoiceBuyerList ID="itemList" runat="server" EnableViewState="false" Visible="false" />
            <!--按鈕-->
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnQuery" />
    </Triggers>
</asp:UpdatePanel>
<uc4:DataModelCache runat="server" ID="modelItem" KeyName="defaultEntity" />
