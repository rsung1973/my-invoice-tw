using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DataEntity;
using eIVOGo.Helper;
using eIVOGo.Models;
using Model.Security.MembershipManagement;
using Business.Helper;
using System.Text;
using Model.Locale;

namespace eIVOGo.Controllers
{
    public class WinningInvoiceController : Controller
    {
        protected ModelSourceInquiry<InvoiceItem> createModelInquiry()
        {
            UserProfileMember userProfile = WebPageUtility.UserProfile;

            return (new InquireEffectiveInvoice { CurrentController = this })
                .Append(new InquireWinningInvoice { CurrentController = this })
                .Append(new InquireInvoiceByRole(userProfile) { CurrentController = this })
                .Append(new InquireInvoiceSeller { ControllerName = "InquireInvoice", ActionName = "BySeller", CurrentController = this })
                .Append(new InquireInvoicePeriod { ControllerName = "InquireInvoice", ActionName = "ByPeriod", CurrentController = this, QueryRequired = true, AlertMessage = "請選擇期別!!" });
        }

        public ActionResult ReportIndex()
        {
            //ViewBag.HasQuery = false;
            //ViewBag.RequiredError = false;
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();

            return View(models.Inquiry);
        }

        public ActionResult InquireReport()
        {
            //ViewBag.HasQuery = true;
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            return View("ReportResult",models.Inquiry);
        }

        //public ActionResult GridPage(int index,int size)
        //{
        //    //ViewBag.HasQuery = true;
        //    ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
        //    TempData.SetModelSource(models);
        //    models.Inquiry = createModelInquiry();
        //    models.BuildQuery();

        //    if (index > 0)
        //        index--;
        //    else
        //        index = 0;

        //    return View(models.Items.OrderByDescending(d => d.InvoiceID)
        //        .Skip(index * size).Take(size)
        //        .ToArray());
        //}

        public ActionResult ReportGridPage(int index, int size)
        {
            //ViewBag.HasQuery = true;
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            if (index > 0)
                index--;
            else
                index = 0;

            return View(models.Items
                .GroupBy(i => i.SellerID)
                .OrderBy(g => g.Key)
                .Skip(index * size).Take(size)
                .Join(models.GetTable<Organization>(),
                    g => g.Key, o => o.CompanyID, (g, o) =>
                        new WinningInvoiceReportItem
                        {
                            Addr = o.Addr,
                            SellerName = o.CompanyName,
                            SellerReceiptNo = o.ReceiptNo,
                            WinningCount = g.Count(),
                            DonationCount = g.Where(i => i.InvoiceDonation != null).Count()
                        }
                ).ToArray());
        }


        public ActionResult DownloadCSV()
        {
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            Response.ContentEncoding = Encoding.GetEncoding(950);

            return View(models.Items
                .GroupBy(i => i.SellerID)
                .OrderBy(g => g.Key)
                .Join(models.GetTable<Organization>(),
                    g => g.Key, o => o.CompanyID, (g, o) =>
                        new WinningInvoiceReportItem
                        {
                            Addr = o.Addr,
                            SellerName = o.CompanyName,
                            SellerReceiptNo = o.ReceiptNo,
                            WinningCount = g.Count(),
                            DonationCount = g.Where(i => i.InvoiceDonation != null).Count()
                        }
                ).ToArray());
        }

        public ActionResult PrintResult()
        {
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();
            models.ResultModel = Naming.DataResultMode.Print;
            models.BuildQuery();

            return View(models.Inquiry);
        }

    }
}
