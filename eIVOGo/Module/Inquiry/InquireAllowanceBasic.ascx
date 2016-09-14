<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InquireAllowanceBasic.ascx.cs"
    Inherits="eIVOGo.Module.Inquiry.InquireAllowanceBasic" %>
<%@ Register Src="InvoiceAllowanceQueryList.ascx" TagName="InvoiceAllowanceQueryList" TagPrefix="uc4" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/UrlRadioDirective.ascx" TagName="UrlRadioDirective" TagPrefix="uc8" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceSeller.ascx" TagPrefix="uc4" TagName="InquireInvoiceSeller" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceDate.ascx" TagPrefix="uc4" TagName="InquireInvoiceDate" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceNo.ascx" TagPrefix="uc4" TagName="InquireInvoiceNo" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireCustomerID.ascx" TagPrefix="uc4" TagName="InquireCustomerID" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireSMSNotification.ascx" TagPrefix="uc4" TagName="InquireSMSNotification" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagPrefix="uc4" TagName="DataModelCache" %>


<%@ Import Namespace="eIVOGo.Module.Base" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 查詢發票/折讓" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="functionTitle" runat="server" ItemName="查詢發票/折讓" />
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <uc8:UrlRadioDirective ID="urlGo" runat="server" DefaultName="電子發票" />
        <asp:PlaceHolder ID="inquiryHolder" runat="server"></asp:PlaceHolder>
        <%--        <uc4:InquireInvoiceSeller runat="server" id="inquireSeller" />
        <uc4:InquireCustomerID runat="server" id="inquireCustomerID" />
        <uc4:InquireInvoiceDate runat="server" id="inquireDate" />
        <uc4:InquireInvoiceNo runat="server" id="inquireNo" />
        <uc4:InquireSMSNotification runat="server" id="inquireSMSNotification" />--%>
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
            <%--<uc4:InvoiceAllowanceQueryList ID="itemList" runat="server" EnableViewState="false" Visible="false" />--%>
            <!--按鈕-->
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnQuery" />
    </Triggers>
</asp:UpdatePanel>
<uc4:DataModelCache runat="server" id="modelItem" KeyName="defaultEntity" />
