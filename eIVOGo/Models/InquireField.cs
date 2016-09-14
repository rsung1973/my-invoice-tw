using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;
using eIVOGo.Helper;
using eIVOGo.Module.Base;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;

namespace eIVOGo.Models
{
    public partial class CommonInquiry<TEntity> : ModelSourceInquiry<TEntity>
        where TEntity : class,new()
    {
        public Controller CurrentController
        { get; set; }

        public void Render(HtmlHelper Html)
        {
            if (this.ControllerName != null && this.ActionName != null)
            {
                Html.RenderAction(this.ActionName, this.ControllerName);
            }
            if (_chainedInquiry != null)
            {
                foreach (var inquiry in _chainedInquiry)
                {
                    ((CommonInquiry<TEntity>)inquiry).Render(Html);
                }
            }
        }

        public void RenderAlert(HtmlHelper Html)
        {
            if (this.HasError)
                Html.RenderPartial("../Module/InquiryAlert", this);
            if (_chainedInquiry != null)
            {
                foreach (var inquiry in _chainedInquiry)
                {
                    ((CommonInquiry<TEntity>)inquiry).RenderAlert(Html);
                }
            }
        }

        public void RenderQueryMessage(HtmlHelper Html)
        {
            if (!String.IsNullOrEmpty(QueryMessage))
                Html.RenderPartial("../Module/QueryMessage", this);
            if (_chainedInquiry != null)
            {
                foreach (var inquiry in _chainedInquiry)
                {
                    ((CommonInquiry<TEntity>)inquiry).RenderQueryMessage(Html);
                }
            }
        }

    }


    public partial class InquireCustomerID : CommonInquiry<InvoiceItem>
    {
        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {
            if (!String.IsNullOrEmpty(CurrentController.Request["customerID"]))
            {
                HasSet = true;
                models.Items = models.Items.Where(i => i.InvoiceBuyer.CustomerID == CurrentController.Request["customerID"].Trim());
            }

            base.BuildQueryExpression(models);
        }
    }

    public partial class InquireInvoiceSeller : CommonInquiry<InvoiceItem>
    {
        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {
            if (!String.IsNullOrEmpty(CurrentController.Request["sellerID"]))
            {
                HasSet = true;
                models.Items = models.Items.Where(d => d.InvoiceSeller.SellerID == int.Parse(CurrentController.Request["sellerID"]));
            }

            base.BuildQueryExpression(models);
        }
    }

    public partial class InquireInvoiceDate : CommonInquiry<InvoiceItem>
    {
        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {
            DateTime? dateFrom;
            if (CurrentController.Request["invoiceDateFrom"].ParseDate(out dateFrom))
            {
                models.Items = models.Items.Where(i => i.InvoiceDate >= dateFrom.Value);
                HasSet = true;
            }

            DateTime? dateTo;
            if (CurrentController.Request["invoiceDateTo"].ParseDate(out dateTo))
            {
                models.Items = models.Items.Where(i => i.InvoiceDate < dateTo.Value.AddDays(1));
                HasSet = true;
            }
            base.BuildQueryExpression(models);
        }

    }

    public partial class InquireInvoiceConsumption : CommonInquiry<InvoiceItem>
    {
        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {
            if (CurrentController.Request["cc1"] == "B2B")
            {
                models.Items = models.Items.Where(i => i.InvoiceBuyer.ReceiptNo != "0000000000");
                HasSet = true;
            }
            else if (CurrentController.Request["cc1"] == "B2C")
            {
                models.Items = models.Items.Where(i => i.InvoiceBuyer.ReceiptNo == "0000000000");
                HasSet = true;
            }

            base.BuildQueryExpression(models);
        }

        public bool QueryForB2C
        {
            get
            {
                return CurrentController.Request["cc1"] == "B2C";
            }
        }
    }

    public partial class InquireInvoiceConsumptionExtensionToPrint : CommonInquiry<InvoiceItem>
    {
        private InquireInvoiceConsumption _inquiry;

        public InquireInvoiceConsumptionExtensionToPrint(InquireInvoiceConsumption inquiry)
        {
            _inquiry = inquiry;
        }

        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {
            if (_inquiry.QueryForB2C)
            {
                models.Items = models.Items.Where(i => i.DonateMark == "0"
                    && (i.PrintMark == "Y" || (i.PrintMark == "N" && i.InvoiceWinningNumber != null))
                    && !i.CDS_Document.DocumentPrintLogs.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice));
            }
        }
    }

    public partial class InquireInvoiceByRole : CommonInquiry<InvoiceItem>
    {
        protected UserProfileMember _userProfile;

        public InquireInvoiceByRole(UserProfileMember profile)
        {
            _userProfile = profile;
        }

        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {
            switch ((Naming.RoleID)_userProfile.CurrentUserRole.RoleID)
            {
                case Naming.RoleID.ROLE_GUEST:
                case Naming.RoleID.ROLE_BUYER:
                case Naming.RoleID.ROLE_SYS:
                    break;

                default:
                    models.Items = models.GetDataContext()
                                .GetInvoiceByAgent(models.Items, _userProfile.CurrentUserRole.OrganizationCategory.CompanyID);
                    break;
            }


            base.BuildQueryExpression(models);
        }
    }

    public partial class InquireEffectiveInvoice : CommonInquiry<InvoiceItem>
    {
        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {
            if (CurrentController.Request["cancelled"] == "1")
            {
                models.Items = models.Items.Where(i => i.InvoiceCancellation != null);
                this.HasSet = true;
            }
            else
            {
                models.Items = models.Items.Where(i => i.InvoiceCancellation == null);
            }
            base.BuildQueryExpression(models);
        }
    }

    public partial class InquireWinningInvoice : CommonInquiry<InvoiceItem>
    {
        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {
            if (CurrentController.Request["winning"] == null || CurrentController.Request["winning"] == "1")
            {
                models.Items = models.Items.Where(i => i.InvoiceWinningNumber != null);
                HasSet = true;
            }
            base.BuildQueryExpression(models);
        }
    }

    public partial class InquireInvoicePeriod : CommonInquiry<InvoiceItem>
    {
        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {
            if (CurrentController.Request["period"] != null)
            {
                String[] period = CurrentController.Request["period"].Split(',');
                if (period != null && period.Length == 2)
                {
                    int year, p;
                    if (int.TryParse(period[0], out year) && int.TryParse(period[1], out p) && p >= 1 && p <= 6)
                    {
                        DateTime dateFrom = new DateTime(year, p * 2 - 1, 1);
                        models.Items = models.Items.Where(i => i.InvoiceDate >= dateFrom && i.InvoiceDate < dateFrom.AddMonths(2));
                        HasSet = true;
                        QueryMessage = (dateFrom.Year - 1911) + "年 " + dateFrom.Month + "~" + (dateFrom.Month + 1) + "月";
                    }
                }
            }

            base.BuildQueryExpression(models);
        }

    }

    public partial class InquireDonatedInvoice : CommonInquiry<InvoiceItem>
    {
        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {
            models.Items = models.Items.Where(i => i.InvoiceCancellation == null
                && i.InvoiceDonation != null);

            if (CurrentController.Request["donation"] == "winning")
            {
                models.Items = models.Items.Where(i => i.InvoiceWinningNumber != null);
                HasSet = true;
            }

            base.BuildQueryExpression(models);
        }
    }

    public partial class InquireDonatory : CommonInquiry<InvoiceItem>
    {
        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {

            if (!String.IsNullOrEmpty(CurrentController.Request["donatory"]))
            {
                models.Items = models.Items.Where(i => i.InvoiceDonation.AgencyCode == CurrentController.Request["donatory"]);
                HasSet = true;
            }

            base.BuildQueryExpression(models);
        }
    }

    public partial class InquireInvoiceAttachment : CommonInquiry<InvoiceItem>
    {
        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {
            if (CurrentController.Request["attachment"] == "1")
            {
                models.Items = models.Items.Where(i => i.CDS_Document.Attachment.Count > 0);
                HasSet = true;
            }

            base.BuildQueryExpression(models);
        }
    }


    public partial class InquireInvoiceNo : CommonInquiry<InvoiceItem>
    {
        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {
            if (!String.IsNullOrEmpty(CurrentController.Request["invoiceNo"]))
            {
                String invoiceNo = CurrentController.Request["invoiceNo"].Trim();
                if (invoiceNo.Length == 10)
                {
                    String trackCode = invoiceNo.Substring(0, 2);
                    String no = invoiceNo.Substring(2);
                    models.Items = models.Items.Where(i => i.No == no && i.TrackCode == trackCode);

                }
                else
                {
                    models.Items = models.Items.Where(i => i.No == invoiceNo);
                }
                HasSet = true;
            }
            base.BuildQueryExpression(models);
        }

    }

    public partial class InquireInvoiceAgent : CommonInquiry<InvoiceItem>
    {
        public override void BuildQueryExpression(ModelSource<InvoiceItem> models)
        {
            if (!String.IsNullOrEmpty(CurrentController.Request["agentID"]))
            {
                HasSet = true;
                models.Items = models.Items.Where(d => d.CDS_Document.DocumentOwner.OwnerID == int.Parse(CurrentController.Request["agentID"]));
            }

            base.BuildQueryExpression(models);
        }
    }



}