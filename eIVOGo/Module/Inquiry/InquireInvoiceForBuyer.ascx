<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register Src="~/Module/Inquiry/InvoiceItemQueryList.ascx" TagPrefix="uc1" TagName="InvoiceItemQueryList" %>
<%@ Register Src="~/Module/Inquiry/QueryResultInfo.ascx" TagPrefix="uc1" TagName="QueryResultInfo" %>
<%@ Register Src="~/Module/Inquiry/InquireInvoiceBasic.ascx" TagPrefix="uc1" TagName="InquireInvoiceBasic" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/UrlRadioDirective.ascx" TagPrefix="uc1" TagName="UrlRadioDirective" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceDate.ascx" TagPrefix="uc4" TagName="InquireInvoiceDate" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceCarrierNo.ascx" TagPrefix="uc1" TagName="InquireInvoiceCarrierNo" %>


<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Utility" %>

<uc1:InquireInvoiceBasic runat="server" id="inquiry" />
<uc1:InquireInvoiceCarrierNo runat="server" id="inquireCarrierNo" QueryRequired="True" AlertMessage="請輸入載具號碼頭!!" />
<uc4:InquireInvoiceDate runat="server" id="inquireDate" />
<uc1:InvoiceItemQueryList runat="server" id="itemList" />
<uc1:QueryResultInfo runat="server" id="queryInfo" runat="server" enableviewstate="false" />

<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += module_inquiry_inquireinvoice_ascx_Load;
        this.PreRender += module_inquiry_inquireinvoice_ascx_PreRender;

        ((ASP.module_inquiry_inquiryitem_urlradiodirective_ascx)inquiry.urlGo).NamingDirection
            = new String[] { 
                "電子發票", "~/Visitor/InquireInvoiceForBuyer.aspx",
                "電子折讓單", "~/Visitor/InquireAllowanceForBuyer.aspx",
                "作廢電子發票", "~/Visitor/InquireInvoiceCancellationBuyer.aspx",
                "作廢電子折讓單", "~/Visitor/InquireAllowanceCancellationForBuyer.aspx",
                "中獎發票", "~/Visitor/InquireWinningInvoiceForBuyer.aspx"};

        inquiry.itemList = itemList;
        inquiry._inquiryItem = new List<eIVOGo.Module.Base.IInquireEntity<Model.DataEntity.InvoiceItem>>();
        inquiry._inquiryItem.Add(inquireCarrierNo);
        inquiry._inquiryItem.Add(inquireDate);

        switch (DefaultQuery)
        {
            case QueryType.電子發票:
                inquiry._queryExpr = i => i.InvoiceCancellation == null;
                break;
            case QueryType.作廢電子發票:
                inquiry._queryExpr = i => i.InvoiceCancellation != null;
                break;
            case QueryType.中獎發票:
                inquiry._queryExpr = i => i.InvoiceCancellation == null && i.InvoiceWinningNumber != null;
                break;
            default:
                inquiry._queryExpr = i => false;
                break;
        }
    }

    void module_inquiry_inquireinvoice_ascx_PreRender(object sender, EventArgs e)
    {
        inquiry.inquiryHolder.Controls.Add(inquireCarrierNo);
        inquiry.inquiryHolder.Controls.Add(inquireDate);

        inquiry.resultInfo.Controls.Add(queryInfo);
        inquiry.resultInfo.Controls.Add(itemList);
    }

    void module_inquiry_inquireinvoice_ascx_Load(object sender, EventArgs e)
    {
        ((ASP.module_inquiry_inquiryitem_urlradiodirective_ascx)inquiry.urlGo).DefaultName = this.DefaultQuery.ToString();
    }


    [System.ComponentModel.Bindable(true)]
    public QueryType DefaultQuery
    { get; set; }
    

</script>
