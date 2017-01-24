using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using eIVOGo.Helper;
using Model.DataEntity;
using Model.Locale;
using Newtonsoft.Json;
using Utility;

namespace eIVOGo.Controllers
{
    public class DataEntityController : SampleController<InvoiceItem>
    {
        // GET: DataFlow
        public ActionResult Organization(int id)
        {
            var item = models.GetTable<Organization>().Where(o => o.CompanyID == id).FirstOrDefault();
            return Content(JsonConvert.SerializeObject(item), "application/json");
        }

        public ActionResult OrganizationExtension(int id)
        {
            var item = models.GetTable<OrganizationExtension>().Where(o => o.CompanyID == id).FirstOrDefault();
            return Content(JsonConvert.SerializeObject(item), "application/json");
        }

    }
}