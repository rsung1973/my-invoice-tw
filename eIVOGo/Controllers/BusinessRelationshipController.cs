using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Business.Helper;
using eIVOGo.Helper;
using eIVOGo.Models.ViewModel;
using eIVOGo.Module.Common;
using eIVOGo.Properties;
using Model.DataEntity;
using Model.DocumentManagement;
using Model.Helper;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Schema.EIVO.B2B;
using Model.Schema.MIG3_1;
using Model.Schema.TurnKey;
using Model.Schema.TXN;
using Utility;
using Uxnet.Com.Security.UseCrypto;

namespace eIVOGo.Controllers
{
    public class BusinessRelationshipController : SampleController<InvoiceItem>
    {
        public ActionResult MaintainRelationship(String message)
        {
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ImportCounterpartBusiness(String message)
        {
            var userProfile = HttpContext.GetUser();
            var uploadMgr = (BusinessCounterpartUploadManager)userProfile["UploadManager"];
            if (uploadMgr != null)
                uploadMgr.Dispose();
            userProfile["UploadManager"] = null;

            ViewBag.Message = message;
            return View();
        }

        public ActionResult InquireBusinessRelationship(int? pageIndex, int? companyID, String ReceiptNo, String CompanyName, int? CompanyStatus, bool? EntrustToPrint, bool? Entrusting, int? BusinessType)
        {
            var profile = HttpContext.GetUser();
            if(!profile.IsSystemAdmin())
            {
                companyID = profile.CurrentUserRole.OrganizationCategory.CompanyID;
            }

            Expression<Func<Organization, bool>> queryExpr = i => true;

            if (!String.IsNullOrEmpty(ReceiptNo))
            {
                queryExpr = queryExpr.And(i => i.ReceiptNo == ReceiptNo);
            }
            if (!String.IsNullOrEmpty(CompanyName))
            {
                queryExpr = queryExpr.And(i => i.CompanyName == CompanyName);
            }
            if (CompanyStatus.HasValue)
            {
                queryExpr = queryExpr.And(i => i.OrganizationStatus.CurrentLevel == CompanyStatus);
            }

            //主動列印
            if (EntrustToPrint.HasValue)
            {
                if (EntrustToPrint == true)
                {
                    queryExpr = queryExpr.And(i => i.OrganizationStatus.EntrustToPrint == true);
                }
                else
                {
                    queryExpr = queryExpr.And(i => i.OrganizationStatus.EntrustToPrint == false || !i.OrganizationStatus.EntrustToPrint.HasValue);
                }
            }
            //自動接收
            if (Entrusting.HasValue)
            {
                if (Entrusting == true)
                {
                    queryExpr = queryExpr.And(i => i.OrganizationStatus.Entrusting == true);
                }
                else
                {
                    queryExpr = queryExpr.And(i => i.OrganizationStatus.Entrusting == false || !i.OrganizationStatus.Entrusting.HasValue);
                }
            }


            var org = models.GetTable<Organization>();
            IQueryable<BusinessRelationship> items;

            if (BusinessType.HasValue)
            {
                items = companyID.HasValue
                    ? models.GetTable<BusinessRelationship>().Where(b => b.MasterID == companyID.Value && b.BusinessID == BusinessType)
                        .Join(org.Where(queryExpr), b => b.RelativeID, o => o.CompanyID, (b, o) => b)
                    : models.GetTable<BusinessRelationship>().Where(b => b.BusinessID == BusinessType)
                        .Join(org.Where(queryExpr), b => b.RelativeID, o => o.CompanyID, (b, o) => b);
            }
            else
            {
                items = companyID.HasValue
                    ? models.GetTable<BusinessRelationship>().Where(b => b.MasterID == companyID.Value)
                        .Join(org.Where(queryExpr), b => b.RelativeID, o => o.CompanyID, (b, o) => b)
                    : models.GetTable<BusinessRelationship>()
                        .Join(org.Where(queryExpr), b => b.RelativeID, o => o.CompanyID, (b, o) => b);
            }

            if (pageIndex.HasValue)
            {
                ViewBag.PageIndex = pageIndex - 1;
                return View("~/Views/BusinessRelationship/Module/ItemList.ascx", items);
            }
            else
            {
                ViewBag.PageIndex = 0;
                return View("~/Views/BusinessRelationship/Module/QueryResult.ascx", items);
            }

        }

        public ActionResult DeleteItem(int businessID, int masterID, int relativeID)
        {
            var item = models.DeleteAny<BusinessRelationship>(m => m.MasterID == masterID && m.RelativeID == relativeID && m.BusinessID == businessID);
            if (item == null)
            {
                return Json(new { result = false, message = "營業人資料錯誤!!" });
            }

            return Json(new { result = true });
        }

        public ActionResult Deactivate(int businessID, int masterID, int relativeID)
        {
            var item = models.GetTable<BusinessRelationship>().Where(m => m.MasterID == masterID && m.RelativeID == relativeID && m.BusinessID == businessID).FirstOrDefault();
            if (item == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "營業人資料錯誤!!");
            }

            item.CurrentLevel = (int)Naming.MemberStatusDefinition.Mark_To_Delete;
            models.SubmitChanges();

            return View("~/Views/BusinessRelationship/Module/DataItem.ascx", item);
        }

        public ActionResult Activate(int businessID, int masterID, int relativeID)
        {
            var item = models.GetTable<BusinessRelationship>().Where(m => m.MasterID == masterID && m.RelativeID == relativeID && m.BusinessID == businessID).FirstOrDefault();
            if (item == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "營業人資料錯誤!!");
            }

            item.CurrentLevel = (int)Naming.MemberStatusDefinition.Checked;
            models.SubmitChanges();

            return View("~/Views/BusinessRelationship/Module/DataItem.ascx", item);
        }

        public ActionResult SetEntrusting(int businessID, int masterID, int relativeID, bool status)
        {
            var item = models.GetTable<BusinessRelationship>().Where(m => m.MasterID == masterID && m.RelativeID == relativeID && m.BusinessID == businessID).FirstOrDefault();
            if (item == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "營業人資料錯誤!!");
            }

            item.Counterpart.OrganizationStatus.Entrusting = status;
            models.SubmitChanges();

            return View("~/Views/BusinessRelationship/Module/DataItem.ascx", item);
        }

        public ActionResult SetEntrustToPrint(int businessID, int masterID, int relativeID, bool status)
        {
            var item = models.GetTable<BusinessRelationship>().Where(m => m.MasterID == masterID && m.RelativeID == relativeID && m.BusinessID == businessID).FirstOrDefault();
            if (item == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "營業人資料錯誤!!");
            }

            item.Counterpart.OrganizationStatus.EntrustToPrint = status;
            models.SubmitChanges();

            return View("~/Views/BusinessRelationship/Module/DataItem.ascx", item);
        }

        public ActionResult CommitItem(BusinessRelationshipViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.CompanyName))
            {
                ModelState.AddModelError("CompanyName", "營業人名稱格式錯誤");
            }

            if (viewModel.ReceiptNo == null || !Regex.IsMatch(viewModel.ReceiptNo, "\\d{8}"))
            {
                ModelState.AddModelError("ReceiptNo", "統編格式錯誤");
            }

            if (string.IsNullOrEmpty(viewModel.ContactEmail) || !viewModel.ContactEmail.Contains('@'))
            {
                ModelState.AddModelError("ContactEmail", "聯絡人電子郵件格式錯誤");
            }

            if (string.IsNullOrEmpty(viewModel.Addr))
            {
                ModelState.AddModelError("Addr", "地址格式錯誤");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ModelState = ModelState;
                return View("~/Views/SiteAction/Alert.ascx", model: null);
            }


            BusinessRelationship model = null;
            var item = models.GetTable<Organization>().Where(o => o.ReceiptNo == viewModel.ReceiptNo).FirstOrDefault();
            UserProfile userProfile = null;

            if (item == null)
            {
                item = new Organization
                {
                    OrganizationStatus = new OrganizationStatus
                    {
                        CurrentLevel = (int)Naming.MemberStatusDefinition.Checked
                    },
                    OrganizationExtension = new OrganizationExtension { }
                };

                model = new BusinessRelationship
                {
                    Counterpart = item,
                    BusinessID = viewModel.BusinessType.Value,
                    MasterID = viewModel.CompanyID.Value,
                    CurrentLevel = viewModel.CompanyStatus
                };

                var orgaCate = new OrganizationCategory
                {
                    Organization = item,
                    CategoryID = (int)Naming.CategoryID.COMP_E_INVOICE_B2C_BUYER
                };

                models.GetTable<Organization>().InsertOnSubmit(item);

                userProfile = new UserProfile
                {
                    PID = !String.IsNullOrEmpty(viewModel.CustomerNo) ? viewModel.CustomerNo : viewModel.ReceiptNo,
                    Phone = viewModel.Phone,
                    EMail = viewModel.ContactEmail,
                    Address = viewModel.Addr,
                    UserProfileExtension = new UserProfileExtension
                    {
                        IDNo = viewModel.ReceiptNo
                    },
                    UserProfileStatus = new UserProfileStatus
                    {
                        CurrentLevel = (int)Naming.MemberStatusDefinition.Wait_For_Check
                    }
                };

                models.GetTable<UserProfile>().InsertOnSubmit(userProfile);

                models.GetTable<UserRole>().InsertOnSubmit(new UserRole
                {
                    RoleID = (int)Naming.RoleID.ROLE_BUYER,
                    UserProfile = userProfile,
                    OrganizationCategory = orgaCate
                });
            }
            else
            {
                model = models.GetTable<BusinessRelationship>().Where(r => r.MasterID == viewModel.CompanyID && r.BusinessID == viewModel.BusinessType && r.RelativeID == item.CompanyID).FirstOrDefault();
                if (model == null)
                {
                    model = new BusinessRelationship
                    {
                        Counterpart = item,
                        BusinessID = viewModel.BusinessType.Value,
                        MasterID = viewModel.CompanyID.Value
                    };
                }

                //var currentUser = models.GetTable<UserProfile>().Where(u => u.PID == viewModel.ReceiptNo).FirstOrDefault();
                //if (currentUser != null)
                //{
                //    currentUser.Phone = viewModel.Phone;
                //    currentUser.EMail = viewModel.ContactEmail;
                //    currentUser.Address = viewModel.Addr;
                //}
            }

            item.CompanyName = viewModel.CompanyName.Trim();
            item.ReceiptNo = viewModel.ReceiptNo;
            item.ContactEmail = viewModel.ContactEmail;
            item.Addr = viewModel.Addr;
            item.Phone = viewModel.Phone;
            item.OrganizationExtension.CustomerNo = viewModel.CustomerNo;

            models.SubmitChanges();

            if(userProfile!=null)
            {
                ThreadPool.QueueUserWorkItem(stateInfo => 
                {
                    String.Format("{0}{1}?id={2}", Settings.Default.mailLinkAddress, VirtualPathUtility.ToAbsolute(Settings.Default.NotifyActivation), userProfile.UID)
                        .MailWebPage(userProfile.EMail, "電子發票系統 會員啟用認證信");
                });
            }

            return View("~/Views/BusinessRelationship/Module/DataItem.ascx", model);

        }

        public ActionResult ImportCsv(BusinessRelationshipViewModel viewModel)
        {
            var userProfile = HttpContext.GetUser();

            if (!viewModel.CompanyID.HasValue)
            {
                ModelState.AddModelError("CompanyID", "請先建立集團成員!!");
            }
            else if (!viewModel.BusinessType.HasValue)
            {
                ModelState.AddModelError("BusinessType", "請選擇相對營業人類別!!");
            }
            else if (Request.Files.Count == 0)
            {
                ModelState.AddModelError("csvFile", "請匯入檔案!!");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ModelState = ModelState;
                ViewBag.Message = ModelState.ErrorMessage();
                return View("~/Views/BusinessRelationship/ImportCounterpartBusiness.aspx");
            }

            var file = Request.Files[0];
            String fileName = Path.Combine(Logger.LogDailyPath, DateTime.Now.Ticks + "_" + Path.GetFileName(file.FileName));
            file.SaveAs(fileName);

            var mgr = new BusinessCounterpartUploadManager();
            mgr.BusinessType = (Naming.InvoiceCenterBusinessType)viewModel.BusinessType;
            mgr.MasterID = viewModel.CompanyID;
            mgr.ParseData(userProfile, fileName, Encoding.GetEncoding(950));

            userProfile["UploadManager"] = mgr;

            return View("~/Views/BusinessRelationship/ImportCounterpartBusiness.aspx");

        }

        public ActionResult ImportCounterpartBusinessList(int? pageIndex)
        {

            if (pageIndex.HasValue)
            {
                ViewBag.PageIndex = pageIndex - 1;
            }
            else
            {
                ViewBag.PageIndex = 0;
            }

            var profile = HttpContext.GetUser();
            var mgr = (BusinessCounterpartUploadManager)profile["UploadManager"];

            if (mgr == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "連線逾時，請重新匯入資料!!");
            }

            return View("~/Views/BusinessRelationship/Module/ImportCounterpartBusinessList.ascx");

        }

        public ActionResult CommitImport()
        {
            var userProfile = HttpContext.GetUser();
            var uploadMgr = (BusinessCounterpartUploadManager)userProfile["UploadManager"];
            if (uploadMgr.IsValid)
            {
                uploadMgr.Save();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = true, message = "資料檔有錯,無法匯入!!" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult CancelImport()
        {
            var userProfile = HttpContext.GetUser();
            var uploadMgr = (BusinessCounterpartUploadManager)userProfile["UploadManager"];
            uploadMgr.Dispose();
            userProfile["UploadManager"] = null;

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);

        }


    }
}