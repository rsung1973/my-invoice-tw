using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using Business.Helper;
using ClosedXML.Excel;
using eIVOGo.Helper;
using eIVOGo.Models;
using eIVOGo.Models.ViewModel;
using eIVOGo.Properties;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;

namespace eIVOGo.Controllers
{
    [Authorize]
    public class OrganizationController : SampleController<InvoiceItem>
    {
        // GET: Organization
        public ActionResult UpdateLogo(int? id)
        {
            var item = models.GetTable<Organization>().Where(c => c.CompanyID == id).FirstOrDefault();
            if(item==null)
            {
                ViewBag.Message = "店家資料錯誤!!";
                return View("~/Views/Shared/AlertMessage.ascx");
            }
            if (Request.Files.Count == 0)
            {
                ViewBag.Message = "請設定公司LOGO標幟!!";
                return View("~/Views/Shared/AlertMessage.ascx");
            }

            String path = Server.MapPath("~/LOGO");
            path.CheckStoredPath();

            var file = Request.Files[0];
            String fileName = item.ReceiptNo + "_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            String storePath = Path.Combine(path, fileName);
            file.SaveAs(storePath);

            item.LogoURL = "LOGO/" + fileName;
            models.SubmitChanges();

            return View("~/Views/Organization/LogoUpdated.ascx", item);

        }
    }
}