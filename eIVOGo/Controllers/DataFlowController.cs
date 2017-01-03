using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DataEntity;
using Model.Locale;
using eIVOGo.Helper;

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

            IQueryable<Organization> orgItems = userProfile.InitializeOrganizationQuery(models);

            return View(orgItems);

        }
    }
}