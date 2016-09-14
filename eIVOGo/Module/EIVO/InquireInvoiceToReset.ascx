<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register Src="~/Module/Inquiry/QueryResultInfo.ascx" TagPrefix="uc1" TagName="QueryResultInfo" %>
<%@ Register Src="~/Module/Inquiry/InquireInvoiceBasic.ascx" TagPrefix="uc1" TagName="InquireInvoiceBasic" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/UrlRadioDirective.ascx" TagPrefix="uc1" TagName="UrlRadioDirective" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceSeller.ascx" TagPrefix="uc4" TagName="InquireInvoiceSeller" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceDate.ascx" TagPrefix="uc4" TagName="InquireInvoiceDate" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceNo.ascx" TagPrefix="uc4" TagName="InquireInvoiceNo" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireCustomerID.ascx" TagPrefix="uc4" TagName="InquireCustomerID" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireSMSNotification.ascx" TagPrefix="uc4" TagName="InquireSMSNotification" %>
<%@ Register Src="~/Module/Base/InvoiceItemList.ascx" TagName="InvoiceItemList" TagPrefix="uc4" %>
<%@ Register Src="InvoiceField/GrantDownloadDocument.ascx" TagName="GrantDownloadDocument" TagPrefix="uc7" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<uc1:InquireInvoiceBasic runat="server" id="inquiry" />
<uc4:InquireInvoiceSeller runat="server" id="inquireSeller" />
<uc4:InquireInvoiceDate runat="server" id="inquireDate" />
<uc4:InquireInvoiceNo runat="server" id="inquireNo" />
<uc4:InvoiceItemList ID="itemList" runat="server" EnableViewState="false" Visible="false" />
<uc1:QueryResultInfo runat="server" id="queryInfo" runat="server" enableviewstate="false" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += module_inquiry_inquireinvoice_ascx_Load;
        this.PreRender += module_inquiry_inquireinvoice_ascx_PreRender;
        this.inquiry.Done += inquiry_Done;

        ((ASP.module_inquiry_inquiryitem_urlradiodirective_ascx)inquiry.urlGo).NamingDirection
            = new String[] { 
                "電子發票", "~/SAM/InquireInvoiceToReset.aspx",
                "電子折讓單", "~/SAM/InquireAllowanceToReset.aspx",
                "作廢電子發票", "~/SAM/InquireInvoiceCancellationToReset.aspx",
                "作廢電子折讓單","~/SAM/InquireAllowanceCancellationToReset.aspx"};

        inquiry.ItemList = itemList;
        inquiry._inquiryItem = new List<eIVOGo.Module.Base.IInquireEntity<Model.DataEntity.InvoiceItem>>();
        inquiry._inquiryItem.Add(inquireSeller);
        inquiry._inquiryItem.Add(inquireInvoiceNo);
        inquiry._inquiryItem.Add(inquireDate);
        inquiry._inquiryItem.Add(inquireCustomerID);
        inquiry._inquiryItem.Add(inquireSMSNotification);

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

        inquiry.actionItem.ItemName = "首頁 > 下載重置";
        inquiry.functionTitle.ItemName = "查詢發票資料下載授權";

        var field = new TemplateField
        {
            HeaderText = "",
            ItemTemplate = new Uxnet.Web.WebUI.ItemTemplateCreator
            {
                BuildControl = () => new ASP.module_eivo_invoicefield_grantdownloaddocument_ascx()
            }
        };
        field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        itemList.Grid.Columns.Add(field);        
        
    }

    void module_inquiry_inquireinvoice_ascx_PreRender(object sender, EventArgs e)
    {
        inquiry.inquiryHolder.Controls.Add(inquireSeller);
        inquiry.inquiryHolder.Controls.Add(inquireInvoiceNo);
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

    void inquiry_Done(object sender, EventArgs e)
    {
        if (this.DefaultQuery == QueryType.電子發票)
        {
            inquiry.itemList.BuildQuery = table =>
            {
                var ctx = table.Context;
                return table.Where(inquiry._queryExpr)
                        .Except(table
                            .Join(ctx.GetTable<DocumentDispatch>()
                                .Select(d => d.CDS_Document)
                                .Where(c => c.DocType == (int)Naming.DocumentTypeDefinition.E_Invoice),
                            i => i.InvoiceID, c => c.DocID, (i, c) => i));
            };

        }
        else if (this.DefaultQuery == QueryType.作廢電子發票)
        {
            inquiry.itemList.BuildQuery = table =>
            {
                var ctx = table.Context;
                return table.Where(inquiry._queryExpr)
                    .Except(table.Join(ctx.GetTable<DocumentDispatch>()
                        .Select(d => d.CDS_Document).Where(c => c.DocType == (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation)
                        .Select(c => c.DerivedDocument.ParentDocument)
                         , i => i.InvoiceID, c => c.DocID, (i, c) => i));
            };
        }
    }
    

</script>

