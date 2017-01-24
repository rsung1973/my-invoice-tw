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
using Model.Schema.TurnKey.E0402;
using Model.Security.MembershipManagement;
using Utility;

namespace eIVOGo.Controllers
{
    [Authorize]
    public class InvoiceNoController : SampleController<InvoiceItem>
    {
        // GET: InvoiceNo
        public ActionResult MaintainInvoiceNoInterval()
        {
            return View();
        }

        public ActionResult InquireInterval(InquireNoIntervalViewModel viewModel)
        {
            var profile = HttpContext.GetUser();

            ViewBag.ViewModel = viewModel;

            IQueryable<InvoiceNoInterval> items = models.GetTable<InvoiceNoInterval>();
            if (!profile.CheckSystemCompany())
            {
                items = items.Where(t => t.InvoiceTrackCodeAssignment.SellerID == profile.CurrentUserRole.OrganizationCategory.CompanyID);
            }

            if (viewModel.Year.HasValue)
            {
                items = items.Where(i => i.InvoiceTrackCodeAssignment.InvoiceTrackCode.Year == viewModel.Year);
            }

            if (viewModel.PeriodNo.HasValue)
                items = items.Where(i => i.InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo == viewModel.PeriodNo);

            return View("~/Views/InvoiceNo/Module/QueryResult.ascx", items);
        }

        public ActionResult InquireVacantNo(InquireNoIntervalViewModel viewModel)
        {
            var profile = HttpContext.GetUser();
            
            ViewBag.ViewModel = viewModel;

            if (!viewModel.SellerID.HasValue)
            {
                ModelState.AddModelError("SellerID", "請選擇開立人!!");
            }

            if (!viewModel.Year.HasValue)
            {
                ModelState.AddModelError("Year", "請選擇年份!!");
            }

            if (!viewModel.PeriodNo.HasValue)
            {
                ModelState.AddModelError("PeriodNo", "請選擇期別!!");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ModelState = ModelState;
                return View("~/Views/Shared/ReportInputError.ascx");
            }

            IQueryable<Organization> orgItems;
            if (viewModel.SellerID.HasValue)
            {
                orgItems = models.GetTable<Organization>().Where(o => o.CompanyID == viewModel.SellerID);
            }
            else
            {
                orgItems = profile.InitializeOrganizationQuery(models);
            }

            List<InquireVacantNoResult> items = new List<InquireVacantNoResult>();
            foreach(var org in orgItems)
            {
                items.AddRange(((EIVOEntityDataContext)models.GetDataContext()).InquireVacantNo(org.CompanyID,viewModel.Year,viewModel.PeriodNo));
            }

            return View("~/Views/InvoiceNo/VacantNo/QueryResult.ascx", items);
        }

        public ActionResult DownloadVacantNo(InquireNoIntervalViewModel viewModel)
        {
            ViewResult result = (ViewResult)InquireVacantNo(viewModel);
            if (!ModelState.IsValid)
            {
                return result;
            }

            List<InquireVacantNoResult> items = (List<InquireVacantNoResult>)result.Model;

            if(items.Count<=0)
            {
                ViewBag.Message = "資料不存在!!";
                return View("~/Views/Shared/AlertMessage.ascx");
            }

            return zipVacantNo(items);

        }

        private ActionResult zipVacantNo(List<InquireVacantNoResult> vacantNoItems)
        {
            String temp = Server.MapPath("~/temp");
            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
            }
            String outFile = Path.Combine(temp, Guid.NewGuid().ToString() + ".zip");
            using (var zipOut = System.IO.File.Create(outFile))
            {
                using (ZipArchive zip = new ZipArchive(zipOut, ZipArchiveMode.Create))
                {
                    var items = vacantNoItems.GroupBy(r => new { r.SellerID, r.TrackCode, r.Year, r.PeriodNo });

                    foreach (var item in items)
                    {
                        var orgItem = models.GetTable<Organization>().Where(o => o.CompanyID == item.Key.SellerID).First();

                        BranchTrackBlank blankItem = new BranchTrackBlank
                        {
                            Main = new Main
                            {
                                BranchBan = orgItem.ReceiptNo,
                                HeadBan = orgItem.ReceiptNo,
                                YearMonth = String.Format("{0}{1:00}", item.Key.Year - 1911, item.Key.PeriodNo * 2),
                                InvoiceType = InvoiceTypeEnum.Item07,
                                InvoiceTrack = item.Key.TrackCode
                            },
                        };

                        List<DetailsBranchTrackBlankItem> details = new List<DetailsBranchTrackBlankItem>();
                        var detailItems = item.ToList();
                        foreach(var blank in detailItems.Where(r => !r.CheckPrev.HasValue))
                        {
                            if(blank.CheckNext.HasValue)
                            {
                                var index = detailItems.IndexOf(blank);
                                details.Add(new DetailsBranchTrackBlankItem
                                {
                                    InvoiceBeginNo = String.Format("{0:00000000}", blank.SeqNo),
                                    InvoiceEndNo = String.Format("{0:00000000}", detailItems[index+1].SeqNo)
                                });
                            }
                            else
                            {
                                details.Add(new DetailsBranchTrackBlankItem
                                {
                                    InvoiceBeginNo = String.Format("{0:00000000}", blank.SeqNo),
                                    InvoiceEndNo = String.Format("{0:00000000}", blank.SeqNo)
                                });
                            }
                        }

                        blankItem.Details = details.ToArray();

                        ZipArchiveEntry entry = zip.CreateEntry(String.Format("{0}{1:00}({2}).xml",item.Key.Year,item.Key.PeriodNo,orgItem.ReceiptNo));
                        using (Stream outStream = entry.Open())
                        {
                            blankItem.ConvertToXml().Save(outStream);
                        }
                        
                    }
                }
            }

            var result = new FilePathResult(outFile, "message/rfc822");
            result.FileDownloadName = "空白發票字軌.zip";
            return result;
        }



        public ActionResult CommitItem(InvoiceNoIntervalViewModel viewModel)
        {
            ViewBag.ViewModel = viewModel;
            var profile = HttpContext.GetUser();

            var table = models.GetTable<InvoiceNoInterval>();
            var model = table.Where(i => i.IntervalID == viewModel.IntervalID).FirstOrDefault();

            if (model == null)
            {
                if (!viewModel.TrackID.HasValue)
                {
                    ModelState.AddModelError("TrackID", "未設定字軌!!");
                }
            }

            if (!viewModel.StartNo.HasValue || !(viewModel.StartNo >= 0 && viewModel.StartNo < 100000000))
            {
                ModelState.AddModelError("StartNo", "起號非8位整數!!");
            }
            else if (!viewModel.EndNo.HasValue || !(viewModel.EndNo >= 0 && viewModel.EndNo < 100000000))
            {
                ModelState.AddModelError("EndNo", "迄號非8位整數!!");
            }
            else if (viewModel.EndNo <= viewModel.StartNo || ((viewModel.EndNo - viewModel.StartNo + 1) % 50 != 0))
            {
                ModelState.AddModelError("StartNo", "不符號碼大小順序與差距為50之倍數原則!!");
            }
            else
            {
                if (model != null)
                {
                    if (model.InvoiceNoAssignments.Count > 0)
                    {
                        ModelState.AddModelError("IntervalID", "該區間之號碼已經被使用,不可修改!!!!");
                    }
                    else if (table.Any(t => t.IntervalID != model.IntervalID && t.TrackID == model.TrackID && t.StartNo >= viewModel.EndNo && t.InvoiceNoAssignments.Count > 0
                        && t.SellerID == model.SellerID))
                    {
                        ModelState.AddModelError("StartNo", "違反序時序號原則該區段無法修改!!");
                    }
                    else if (table.Any(t => t.IntervalID != model.IntervalID && t.TrackID == model.TrackID
                        && ((t.EndNo <= viewModel.EndNo && t.EndNo >= viewModel.StartNo) || (t.StartNo <= viewModel.EndNo && t.StartNo >= viewModel.StartNo) || (t.StartNo <= viewModel.StartNo && t.EndNo >= viewModel.StartNo) || (t.StartNo <= viewModel.EndNo && t.EndNo >= viewModel.EndNo))))
                    {
                        ModelState.AddModelError("StartNo", "系統中已存在重疊的區段!!");
                    }
                }
                else
                {
                    if (table.Any(t => t.TrackID == viewModel.TrackID && t.StartNo >= viewModel.EndNo && t.InvoiceNoAssignments.Count > 0
                        && t.SellerID == profile.CurrentUserRole.OrganizationCategory.CompanyID))
                    {
                        ModelState.AddModelError("StartNo", "違反序時序號原則該區段無法新增!!");
                    }
                    else if (table.Any(t => t.TrackID == viewModel.TrackID
                        && ((t.EndNo <= viewModel.EndNo && t.EndNo >= viewModel.StartNo) || (t.StartNo <= viewModel.EndNo && t.StartNo >= viewModel.StartNo) || (t.StartNo <= viewModel.StartNo && t.EndNo >= viewModel.StartNo) || (t.StartNo <= viewModel.EndNo && t.EndNo >= viewModel.EndNo))))
                    {
                        ModelState.AddModelError("StartNo", "系統中已存在重疊的區段!!");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ModelState = ModelState;
                if (model != null)
                    ViewBag.DataRole = "edit";
                else
                    ViewBag.DataRole = "add";
                return View("~/Views/Shared/ReportInputError.ascx");
            }

            if (model == null)
            {
                var codeAssignment = models.GetTable<InvoiceTrackCodeAssignment>().Where(t => t.SellerID == profile.CurrentUserRole.OrganizationCategory.CompanyID && t.TrackID == viewModel.TrackID).FirstOrDefault();
                if (codeAssignment == null)
                {
                    codeAssignment = new InvoiceTrackCodeAssignment
                    {
                        SellerID = profile.CurrentUserRole.OrganizationCategory.CompanyID,
                        TrackID = viewModel.TrackID.Value
                    };

                    models.GetTable<InvoiceTrackCodeAssignment>().InsertOnSubmit(codeAssignment);
                }

                model = new InvoiceNoInterval { };
                codeAssignment.InvoiceNoIntervals.Add(model);
            }

            model.StartNo = viewModel.StartNo.Value;
            model.EndNo = viewModel.EndNo.Value;

            models.SubmitChanges();

            return View("~/Views/InvoiceNo/Module/DataItem.ascx", model);

        }

        public ActionResult EditNoInterval(int? id)
        {
            var profile = HttpContext.GetUser();
            var item = models.GetTable<InvoiceNoInterval>()
                .Where(d => d.IntervalID == id)
                .Where(d => d.InvoiceTrackCodeAssignment.SellerID == profile.CurrentUserRole.OrganizationCategory.CompanyID)
                .FirstOrDefault();

            if (item == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "配號區間資料錯誤!!");
            }

            return View("~/Views/InvoiceNo/Module/EditItem.ascx", item);

        }

        public ActionResult DeleteNoInterval(int? id)
        {
            var profile = HttpContext.GetUser();
            var item = models.DeleteAny<InvoiceNoInterval>(d => d.IntervalID == id 
                && d.InvoiceTrackCodeAssignment.SellerID == profile.CurrentUserRole.OrganizationCategory.CompanyID);

            if (item == null)
            {
                return Json(new { result = false, message = "配號區間資料錯誤!!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult IntervalItem(int? id)
        {
            var profile = HttpContext.GetUser();
            var item = models.GetTable<InvoiceNoInterval>()
                .Where(d => d.IntervalID == id)
                .Where(d => d.InvoiceTrackCodeAssignment.SellerID == profile.CurrentUserRole.OrganizationCategory.CompanyID)
                .FirstOrDefault();

            if (item == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "配號區間資料錯誤!!");
            }

            return View("~/Views/InvoiceNo/Module/DataItem.ascx", item);

        }

        public ActionResult TrackCodeSelector(InquireNoIntervalViewModel viewModel)
        {
            ViewBag.ViewModel = viewModel;
            return View("~/Views/InvoiceNo/Module/TrackCodeSelector.ascx");
        }

        public ActionResult VacantNoIndex()
        {
            return View();
        }

        public ActionResult ProcessVacantNo(InquireNoIntervalViewModel viewModel)
        {
            var profile = HttpContext.GetUser();

            ViewBag.ViewModel = viewModel;

            if (!viewModel.SellerID.HasValue)
            {
                ModelState.AddModelError("SellerID", "請選擇開立人!!");
            }

            if (!viewModel.Year.HasValue)
            {
                ModelState.AddModelError("Year", "請選擇年份!!");
            }

            if (!viewModel.PeriodNo.HasValue)
            {
                ModelState.AddModelError("PeriodNo", "請選擇期別!!");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ModelState = ModelState;
                return View("~/Views/Shared/ReportInputError.ascx");
            }

            models.GetDataContext().ProcessInvoiceNo(viewModel.SellerID, viewModel.Year, viewModel.PeriodNo);

            ViewBag.Message = "整理完成!!";
            return View("~/Views/Shared/AlertMessage.ascx");


        }

    }
}