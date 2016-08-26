using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.Helper;
using ClosedXML.Excel;
using Model.Locale;
using Model.Security.MembershipManagement;
using ModelExtension.DataExchange;
using Utility;

namespace eIVOGo.Controllers
{
    public class DataExchangeController : Controller
    {
        // GET: DataExchange
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateBuyer()
        {
            UserProfileMember profile = WebPageUtility.UserProfile;

            try
            {
                var xlFile = Request.Files["InvoiceBuyer"];
                if (xlFile != null)
                {
                    using(XLWorkbook xlwb = new XLWorkbook(xlFile.InputStream))
                    {
                        InvoiceBuyerExchange exchange = new InvoiceBuyerExchange();
                        switch ((Naming.RoleID)profile.CurrentUserRole.RoleID)
                        {
                            case Naming.RoleID.ROLE_SYS:
                                exchange.ExchangeData(xlwb);
                                break;
                            case Naming.RoleID.ROLE_SELLER:
                            case Naming.RoleID.ROLE_NETWORKSELLER:
                                exchange.ExchangeData(xlwb, item =>
                                {
                                    return item.SellerID == profile.CurrentUserRole.OrganizationCategory.CompanyID
                                        || item.CDS_Document.DocumentOwner.OwnerID == profile.CurrentUserRole.OrganizationCategory.CompanyID;
                                });
                                break;
                            default:
                                break;
                        }

                        String result = Path.Combine(Logger.LogDailyPath, Guid.NewGuid().ToString() + ".xslx");
                        xlwb.SaveAs(result);
                        return File(result, "message/rfc822", "修改買受人資料(回應).xlsx");
                    }
                }
                ViewBag.AlertMessage = "檔案錯誤!!";
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                ViewBag.AlertMessage = ex.ToString();
            }
            return View("Index");
        }

        public ActionResult UpdateTrackCode()
        {
            UserProfileMember profile = WebPageUtility.UserProfile;

            try
            {
                var xlFile = Request.Files["TrackCode"];
                if (xlFile != null)
                {
                    using (XLWorkbook xlwb = new XLWorkbook(xlFile.InputStream))
                    {
                        TrackCodeExchange exchange = new TrackCodeExchange();
                        switch ((Naming.RoleID)profile.CurrentUserRole.RoleID)
                        {
                            case Naming.RoleID.ROLE_SYS:
                                exchange.ExchangeData(xlwb);
                                break;
                            default:
                                break;
                        }

                        String result = Path.Combine(Logger.LogDailyPath, Guid.NewGuid().ToString() + ".xslx");
                        xlwb.SaveAs(result);
                        return File(result, "message/rfc822", "修改發票字軌(回應).xlsx");
                    }
                }
                ViewBag.AlertMessage = "檔案錯誤!!";
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                ViewBag.AlertMessage = ex.ToString();
            }
            return View("Index");
        }



    }
}