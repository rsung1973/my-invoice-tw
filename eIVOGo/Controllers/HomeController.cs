using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Model.DataEntity;
using eIVOGo.Helper;
using Utility;
using Model.Locale;
using Model.Security.MembershipManagement;
using Business.Helper;

namespace eIVOGo.Controllers
{
    public class HomeController : SampleController<InvoiceItem>
    {
        // GET: Home
        public ActionResult MainPage()
        {
            return View();
        }
    }
}