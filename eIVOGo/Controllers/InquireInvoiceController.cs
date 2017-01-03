using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eIVOGo.Helper;
using Model.DataEntity;
using Model.Locale;

namespace eIVOGo.Controllers
{
    public class InquireInvoiceController : SampleController<InvoiceItem>
    {
        public ActionResult BySeller(String fieldName,String title)
        {
            ViewBag.FieldName = fieldName ?? "sellerID";
            ViewBag.Title = title ?? "統編";
            var userProfile = Business.Helper.WebPageUtility.UserProfile;
            var orgItems = userProfile.InitializeOrganizationQuery(models);
            return View(orgItems);
        }

        public ActionResult ByBuyer()
        {
            return View();
        }
        public ActionResult ByBuyerName()
        {
            return View();
        }

        public ActionResult ByInvoiceDate()
        {
            return View();
        }

        public ActionResult ByConsumption()
        {
            return View();
        }

        public ActionResult ByPeriod(String dateFrom, String dateTo)
        {
            DateTime endDate;
            if (dateTo == null || !DateTime.TryParse(dateTo, out endDate))
            {
                endDate = DateTime.Today;
            }
            DateTime startDate;
            if (dateFrom == null || !DateTime.TryParse(dateFrom, out startDate))
            {
                //startDate = endDate.AddYears(-2);
                using (ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>())
                {
                    var item = models.EntityList.OrderBy(i => i.InvoiceID).FirstOrDefault();
                    startDate = item == null ? endDate.AddYears(-2) : item.InvoiceDate.Value;
                }
            }

            startDate = new DateTime(startDate.Year, (startDate.Month + 1) / 2 * 2 - 1, 1);
            endDate = new DateTime(endDate.Year, (endDate.Month + 1) / 2 * 2 - 1, 1);

            List<SelectListItem> items = null;

            if (endDate >= startDate)
            {
                items = new List<SelectListItem>();
                for (DateTime d = endDate; d >= startDate; d = d.AddMonths(-2))
                {
                    items.Add(new SelectListItem
                    {
                        Text = String.Format("{0:000}年 {1:00}月-{2:00}月", d.Year - 1911, d.Month, d.Month + 1),
                        Value = String.Format("{0},{1}", d.Year, (d.Month + 1) / 2)
                    });
                }
            }
            else
            {
                items = new List<SelectListItem>();
                for (DateTime d = startDate; d >= endDate; d = d.AddMonths(-2))
                {
                    items.Add(new SelectListItem
                    {
                        Text = String.Format("{0:000}年 {1:00}月-{2:00}月", d.Year - 1911, d.Month, d.Month + 1),
                        Value = String.Format("{0},{1}", d.Year, (d.Month + 1) / 2)
                    });
                }
            }

            return View(items);
        }

        public ActionResult ByDonation()
        {
            return View();
        }

        public ActionResult ByDonatory()
        {
            return View();
        }

        public ActionResult ByAttachment()
        {
            return View();
        }

        public ActionResult ByAgent()
        {
            var userProfile = Business.Helper.WebPageUtility.UserProfile;
            var mgr = TempData.InvokeModelSource<Organization>();
            IEnumerable<SelectListItem> items = null;
            IQueryable<Organization> orgItems =
                mgr.GetTable<Organization>().Where(o => o.InvoiceInsurerAgent.Count > 0);

            switch ((Naming.CategoryID)userProfile.CurrentUserRole.OrganizationCategory.CategoryID)
            {
                case Naming.CategoryID.COMP_SYS:
                    items = orgItems
                            .OrderBy(o => o.ReceiptNo)
                            .Select(o => new SelectListItem
                            {
                                Text = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName),
                                Value = o.CompanyID.ToString()
                            });
                    break;
                case Naming.CategoryID.COMP_INVOICE_AGENT:
                    items = orgItems.Where(o => o.CompanyID == userProfile.CurrentUserRole.OrganizationCategory.CompanyID)
                            .Select(o => new SelectListItem
                            {
                                Text = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName),
                                Value = o.CompanyID.ToString()
                            });
                    break;

                default:
                    break;
            }
            return View(items);
        }



    }
}
