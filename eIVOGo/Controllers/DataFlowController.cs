using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DataEntity;
using Model.Locale;

namespace eIVOGo.Controllers
{
    public class DataFlowController : SampleController<InvoiceItem>
    {
        // GET: DataFlow
        public ActionResult SellerSelector(bool? selectAll,String selectorIndication,String selectorIndicationValue)
        {

            var userProfile = Business.Helper.WebPageUtility.UserProfile;
            this.ViewBag.SelectorIndication = selectorIndication;
            this.ViewBag.SelectorIndicationValue = selectorIndicationValue;
            if (selectAll==true)
            {
                this.ViewBag.SelectorIndication = "全部";
            }

            IQueryable<Organization> orgItems = models.GetTable<Organization>();
            switch ((Naming.CategoryID)userProfile.CurrentUserRole.OrganizationCategory.CategoryID)
            {
                case Naming.CategoryID.COMP_SYS:
                    orgItems = orgItems.Where(
                        o => o.OrganizationCategory.Any(
                            c => c.CategoryID == (int)Naming.CategoryID.COMP_E_INVOICE_B2C_SELLER
                                || c.CategoryID == (int)Naming.CategoryID.COMP_VIRTUAL_CHANNEL
                                || c.CategoryID == (int)Naming.CategoryID.COMP_E_INVOICE_GOOGLE_TW
                                || c.CategoryID == (int)Naming.CategoryID.COMP_INVOICE_AGENT))
                            .OrderBy(o => o.ReceiptNo);
                    break;
                case Naming.CategoryID.COMP_INVOICE_AGENT:
                    orgItems = models.GetQueryByAgent(userProfile.CurrentUserRole.OrganizationCategory.CompanyID)
                            .OrderBy(o => o.ReceiptNo);

                    break;

                case Naming.CategoryID.COMP_E_INVOICE_B2C_SELLER:
                case Naming.CategoryID.COMP_VIRTUAL_CHANNEL:
                case Naming.CategoryID.COMP_E_INVOICE_GOOGLE_TW:
                    orgItems = orgItems.Where(
                        o => o.CompanyID == userProfile.CurrentUserRole.OrganizationCategory.CompanyID);

                    break;

                default:
                    orgItems = orgItems.Where(o => false);
                    break;
            }

            return View(orgItems);

        }
    }
}