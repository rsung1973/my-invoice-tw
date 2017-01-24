using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Com.Security.UseCrypto;

namespace eIVOGo.Controllers
{
    public class TrackCodeController : SampleController<InvoiceItem>
    {
        // GET: TrackCode
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Inquire(TrackCodeQueryViewModel viewModel)
        {

            ViewBag.ViewModel = viewModel;
            IQueryable<InvoiceTrackCode> items = models.GetTable<InvoiceTrackCode>()
                .Where(t => t.Year == viewModel.Year).OrderBy(t => t.PeriodNo).ThenBy(t => t.TrackCode);

            ViewBag.PageSize = viewModel.PageSize.HasValue && viewModel.PageSize > 0 ? viewModel.PageSize.Value : Uxnet.Web.Properties.Settings.Default.PageSize;

            if (viewModel.PageIndex.HasValue)
            {
                if (viewModel.Sort != null && viewModel.Sort.Length > 0)
                    ViewBag.Sort = viewModel.Sort.Where(s => s.HasValue).Select(s => s.Value).ToArray();
                ViewBag.PageIndex = viewModel.PageIndex - 1;
                return View("~/Views/TrackCode/Module/ItemList.ascx", items);
            }
            else
            {
                ViewBag.PageIndex = 0;
                return View("~/Views/TrackCode/Module/QueryResult.ascx", items);
            }
        }

        public ActionResult EditItem(int? id)
        {
            var item = models.GetTable<InvoiceTrackCode>()
                .Where(d => d.TrackID == id)
                .FirstOrDefault();

            if (item == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "發票字軌資料錯誤!!");
            }

            return View("~/Views/TrackCode/Module/EditItem.ascx", item);

        }

        public ActionResult DeleteItem(int? id)
        {
            var item = models.DeleteAny<InvoiceTrackCode>(d => d.TrackID == id);

            if (item == null)
            {
                return Json(new { result = false, message = "發票字軌資料錯誤!!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DataItem(int? id)
        {
            var item = models.GetTable<InvoiceTrackCode>()
                .Where(d => d.TrackID == id)
                .FirstOrDefault();

            if (item == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "發票字軌資料錯誤!!");
            }
            
            return View("~/Views/TrackCode/Module/DataItem.ascx", item);

        }

        public ActionResult CommitItem(TrackCodeViewModel viewModel)
        {
            ViewBag.ViewModel = viewModel;

            viewModel.TrackCode = viewModel.TrackCode.GetEfficientString();
            if (viewModel.TrackCode==null || !Regex.IsMatch(viewModel.TrackCode,"^[A-Za-z]{2}$"))
            {
                ModelState.AddModelError("TrackCode", "字軌應為二位英文字母!!");
            }

            var model = models.GetTable<InvoiceTrackCode>().Where(t => t.TrackID == viewModel.TrackID).FirstOrDefault();
            if (model == null)
            {
                if (!viewModel.PeriodNo.HasValue || viewModel.PeriodNo > 6 || viewModel.PeriodNo < 1)
                {
                    ModelState.AddModelError("PeriodNo", "請選擇期別!!");
                }
                else if (!viewModel.Year.HasValue)
                {
                    ModelState.AddModelError("Year", "請選擇年份!!");
                }
                else
                {
                    var item = models.GetTable<InvoiceTrackCode>().Where(t => t.Year == viewModel.Year && t.TrackCode == viewModel.TrackCode
                    && t.PeriodNo == viewModel.PeriodNo).FirstOrDefault();

                    if (item != null && item.TrackID != viewModel.TrackID)
                    {
                        ModelState.AddModelError("TrackCode", "字軌重複!!");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                if (model != null)
                    ViewBag.DataRole = "edit";
                else
                    ViewBag.DataRole = "add";

                ViewBag.ModelState = ModelState;
                return View("~/Views/Shared/ReportInputError.ascx");
            }

            if (model == null)
            {
                model = new InvoiceTrackCode
                {
                    Year = viewModel.Year.Value,
                    PeriodNo = viewModel.PeriodNo.Value
                };
                models.GetTable<InvoiceTrackCode>().InsertOnSubmit(model);
            }

            model.TrackCode = viewModel.TrackCode;

            models.SubmitChanges();

            return View("~/Views/TrackCode/Module/DataItem.ascx", model);

        }


    }
}