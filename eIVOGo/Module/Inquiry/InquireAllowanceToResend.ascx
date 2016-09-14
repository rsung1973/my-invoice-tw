<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register Src="~/Module/Inquiry/InvoiceAllowanceCheckList.ascx" TagPrefix="uc1" TagName="InvoiceAllowanceCheckList" %>
<%@ Register Src="~/Module/Inquiry/QueryResultInfo.ascx" TagPrefix="uc1" TagName="QueryResultInfo" %>
<%@ Register Src="~/Module/Inquiry/InquireAllowanceBasic.ascx" TagPrefix="uc1" TagName="InquireAllowanceBasic" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/UrlRadioDirective.ascx" TagPrefix="uc1" TagName="UrlRadioDirective" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireAllowanceSeller.ascx" TagPrefix="uc4" TagName="InquireAllowanceSeller" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireAllowanceDate.ascx" TagPrefix="uc4" TagName="InquireAllowanceDate" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireAllowanceNo.ascx" TagPrefix="uc4" TagName="InquireAllowanceNo" %>

<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Import Namespace="Model.Helper" %>
<uc1:InquireAllowanceBasic runat="server" id="inquiry" />
<uc4:InquireAllowanceSeller runat="server" id="inquireSeller" />
<uc4:InquireAllowanceDate runat="server" id="inquireDate" />
<uc4:InquireAllowanceNo runat="server" id="inquireNo" />
<uc1:InvoiceAllowanceCheckList runat="server" id="itemList" />
<uc1:QueryResultInfo runat="server" id="queryInfo" runat="server" enableviewstate="false" />
<table id="tblAction" runat="server" visible="false" enableviewstate="false" width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnShow" runat="server" Text="重送郵件通知" OnClick="btnShow_Click" />&nbsp;&nbsp;
        </td>
    </tr>
</table>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += module_inquiry_InquireAllowance_ascx_Load;
        this.PreRender += module_inquiry_InquireAllowance_ascx_PreRender;

        ((ASP.module_inquiry_inquiryitem_urlradiodirective_ascx)inquiry.urlGo).NamingDirection
            = new String[] { 
                "電子發票", "~/Visitor/queryInvoiceAndAllowance.aspx",
                "電子折讓單", "~/Visitor/queryAllowance.aspx",
                "作廢電子發票", "~/Visitor/queryInvoiceCancellation.aspx",
                "作廢電子折讓單", "~/Visitor/queryAllowanceCancellation.aspx"};

        inquiry.itemList = itemList;
        inquiry._inquiryItem = new List<eIVOGo.Module.Base.IInquireEntity<Model.DataEntity.InvoiceAllowance>>();
        inquiry._inquiryItem.Add(inquireSeller);
        inquiry._inquiryItem.Add(InquireAllowanceNo);
        inquiry._inquiryItem.Add(inquireDate);

        switch (DefaultQuery)
        {
            case QueryType.電子發票:
                inquiry._queryExpr = i => i.InvoiceAllowanceCancellation == null;
                break;
            case QueryType.作廢電子發票:
                inquiry._queryExpr = i => i.InvoiceAllowanceCancellation != null;
                break;
            default:
                inquiry._queryExpr = i => false;
                break;
        }

        inquiry.actionItem.ItemName = "首頁 > 重送開立發票通知";
        inquiry.functionTitle.ItemName = "重送開立發票通知";
        
    }

    void module_inquiry_InquireAllowance_ascx_PreRender(object sender, EventArgs e)
    {
        inquiry.inquiryHolder.Controls.Add(inquireSeller);
        inquiry.inquiryHolder.Controls.Add(InquireAllowanceNo);
        inquiry.inquiryHolder.Controls.Add(inquireDate);

        inquiry.resultInfo.Controls.Add(queryInfo);
        inquiry.resultInfo.Controls.Add(itemList);
        tblAction.Visible = inquiry.ResultInfo.Visible;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        String[] ar = Request.GetItemSelection();
        if (ar != null && ar.Count() > 0)
        {
            ar.Select(a => int.Parse(a)).SendIssuingNotification();
            this.AjaxAlert("Email通知已重送!!");
        }
        else
        {
            this.AjaxAlert("請選擇重送資料!!");
        }
    }

    void module_inquiry_InquireAllowance_ascx_Load(object sender, EventArgs e)
    {
        ((ASP.module_inquiry_inquiryitem_urlradiodirective_ascx)inquiry.urlGo).DefaultName = this.DefaultQuery.ToString();

    }

    [System.ComponentModel.Bindable(true)]
    public QueryType DefaultQuery
    { get; set; }
    

</script>
