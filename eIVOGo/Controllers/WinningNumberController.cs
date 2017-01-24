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
using System.Xml;

using Business.Helper;
using ClosedXML.Excel;
using eIVOGo.Helper;
using eIVOGo.Models;
using eIVOGo.Models.ViewModel;
using eIVOGo.Module.Common;
using eIVOGo.Properties;
using Model.DataEntity;
using Model.Helper;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;

namespace eIVOGo.Controllers
{
    [Authorize]
    public class WinningNumberController : SampleController<InvoiceItem>
    {
        // GET: WinningNumber
        public ActionResult Index()
        {
            ViewBag.InquiryView = "~/Views/WinningNumber/WinningNoQuery.ascx";
            return View("~/Views/InvoiceProcess/Index.aspx");
        }

        public ActionResult Inquire(InquireNoIntervalViewModel viewModel)
        {
            ViewBag.ViewModel = viewModel;

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

            var items = models.GetTable<UniformInvoiceWinningNumber>().Where(w => w.Year == viewModel.Year && w.Period == viewModel.PeriodNo);

            return View("~/Views/WinningNumber/Module/QueryResult.ascx", items);
        }

        public ActionResult EditItem(int? id)
        {
            ViewResult result = (ViewResult)DataItem(id);
            UniformInvoiceWinningNumber model = result.Model as UniformInvoiceWinningNumber;
            if (model != null)
            {
                result.ViewName = "~/Views/WinningNumber/Module/EditItem.ascx";
            }
            return result;
        }

        public ActionResult DeleteItem(int? id)
        {
            var item = models.DeleteAny<UniformInvoiceWinningNumber>(d => d.WinningID == id);

            if (item == null)
            {
                return Json(new { result = false, message = "中獎號碼資料錯誤!!" }, JsonRequestBehavior.AllowGet);
            }

            if (item.Rank == (int)Naming.WinningPrizeType.頭獎)
            {
                models.DeleteAll<UniformInvoiceWinningNumber>(u => u.Period == item.Period && u.Year == item.Year && u.WinningNO == item.WinningNO.Substring(1));    //二獎
                models.DeleteAll<UniformInvoiceWinningNumber>(u => u.Period == item.Period && u.Year == item.Year && u.WinningNO == item.WinningNO.Substring(2));    //三獎
                models.DeleteAll<UniformInvoiceWinningNumber>(u => u.Period == item.Period && u.Year == item.Year && u.WinningNO == item.WinningNO.Substring(3));    //肆獎
                models.DeleteAll<UniformInvoiceWinningNumber>(u => u.Period == item.Period && u.Year == item.Year && u.WinningNO == item.WinningNO.Substring(4));    //伍獎
                models.DeleteAll<UniformInvoiceWinningNumber>(u => u.Period == item.Period && u.Year == item.Year && u.WinningNO == item.WinningNO.Substring(5));    //陸獎
            }

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DataItem(int? id)
        {
            var item = models.GetTable<UniformInvoiceWinningNumber>()
                .Where(d => d.WinningID == id)
                .FirstOrDefault();

            if (item == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "中獎號碼資料錯誤!!");
            }

            return View("~/Views/WinningNumber/Module/DataItem.ascx", item);

        }

        public ActionResult CommitItem(WinningNumberViewModel viewModel)
        {
            ViewBag.ViewModel = viewModel;

            viewModel.WinningNo = viewModel.WinningNo.GetEfficientString();
            if (viewModel.Rank == (int)Naming.EditableWinningPrizeType.特別獎
                || viewModel.Rank == (int)Naming.EditableWinningPrizeType.特獎
                || viewModel.Rank == (int)Naming.EditableWinningPrizeType.頭獎)
            {
                if (viewModel.WinningNo == null || !Regex.IsMatch(viewModel.WinningNo, "^[0-9]{8}$"))
                {
                    ModelState.AddModelError("WinningNo", "中獎號碼為8碼數字!!");
                }
            }
            else if (viewModel.Rank == (int)Naming.EditableWinningPrizeType.增開六獎)
            {
                if (viewModel.WinningNo == null || !Regex.IsMatch(viewModel.WinningNo, "^[0-9]{3}$"))
                {
                    ModelState.AddModelError("WinningNo", "中獎號碼為3碼數字!!");
                }
            }
            else
            {
                ModelState.AddModelError("Rank", "獎別錯誤!!");
            }

            var table = models.GetTable<UniformInvoiceWinningNumber>();
            var model = table.Where(t => t.WinningID == viewModel.WinningID).FirstOrDefault();
            if (model == null)
            {
                if (!viewModel.Period.HasValue || viewModel.Period > 6 || viewModel.Period < 1)
                {
                    ViewBag.Message = "請選擇期別!!";
                    return View("~/Views/Shared/AlertMessage.ascx");
                }
                else if (!viewModel.Year.HasValue)
                {
                    ViewBag.Message = "請選擇年份!!";
                    return View("~/Views/Shared/AlertMessage.ascx");
                }
                else
                {
                    if (table.Any(t => t.Year == viewModel.Year && t.WinningNO == viewModel.WinningNo && t.Period == viewModel.Period))
                    {
                        ModelState.AddModelError("WinningNo", "中獎號碼重複!!");
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
                model = new UniformInvoiceWinningNumber
                {
                    Year = viewModel.Year.Value,
                    Period = viewModel.Period.Value
                };
                table.InsertOnSubmit(model);
            }
            else
            {
                if (model.Rank == (int)Naming.WinningPrizeType.頭獎)
                {
                    models.DeleteAllOnSubmit<UniformInvoiceWinningNumber>(u => u.Period == model.Period && u.Year == model.Year && u.WinningNO == model.WinningNO.Substring(1));    //二獎
                    models.DeleteAllOnSubmit<UniformInvoiceWinningNumber>(u => u.Period == model.Period && u.Year == model.Year && u.WinningNO == model.WinningNO.Substring(2));    //三獎
                    models.DeleteAllOnSubmit<UniformInvoiceWinningNumber>(u => u.Period == model.Period && u.Year == model.Year && u.WinningNO == model.WinningNO.Substring(3));    //肆獎
                    models.DeleteAllOnSubmit<UniformInvoiceWinningNumber>(u => u.Period == model.Period && u.Year == model.Year && u.WinningNO == model.WinningNO.Substring(4));    //伍獎
                    models.DeleteAllOnSubmit<UniformInvoiceWinningNumber>(u => u.Period == model.Period && u.Year == model.Year && u.WinningNO == model.WinningNO.Substring(5));    //陸獎
                }
            }

            model.WinningNO = viewModel.WinningNo;
            model.Year = viewModel.Year.Value;
            model.Period = viewModel.Period.Value;
            model.Rank = viewModel.Rank.Value;
            model.PrizeType = ((Naming.WinningPrizeType)viewModel.Rank.Value).ToString();
            model.Bonus = Naming.WinningBonus[viewModel.Rank.Value];

            if (model.Rank == (int)Naming.WinningPrizeType.頭獎)
            {
                createWinningNo(table, model.Year, model.Period, viewModel.WinningNo.Substring(1), Naming.WinningPrizeType.二獎);
                createWinningNo(table, model.Year, model.Period, viewModel.WinningNo.Substring(2), Naming.WinningPrizeType.三獎);
                createWinningNo(table, model.Year, model.Period, viewModel.WinningNo.Substring(3), Naming.WinningPrizeType.四獎);
                createWinningNo(table, model.Year, model.Period, viewModel.WinningNo.Substring(4), Naming.WinningPrizeType.五獎);
                createWinningNo(table, model.Year, model.Period, viewModel.WinningNo.Substring(5), Naming.WinningPrizeType.六獎);
                models.SubmitChanges();
                return View("~/Views/WinningNumber/Module/QueryRequired.ascx", model);
            }
            else
            {
                models.SubmitChanges();
                return View("~/Views/WinningNumber/Module/DataItem.ascx", model);
            }

        }

        private void createWinningNo(Table<UniformInvoiceWinningNumber> table, int year, int period, string winningNo, Naming.WinningPrizeType prizeType)
        {
            table.InsertOnSubmit(new UniformInvoiceWinningNumber
            {
                Year = year,
                Period = period,
                WinningNO = winningNo,
                PrizeType = prizeType.ToString(),
                Rank = (int)prizeType,
                Bonus = Naming.WinningBonus[(int)prizeType]
            });
        }

        public ActionResult MatchWinningInvoiceNo(InquireNoIntervalViewModel viewModel)
        {
            ViewResult result = (ViewResult)Inquire(viewModel);
            IQueryable<UniformInvoiceWinningNumber> items = result.Model as IQueryable<UniformInvoiceWinningNumber>;
            if (items != null && items.Count() > 0)
            {
                models.GetDataContext().MatchWinningInvoiceNo(viewModel.Year, viewModel.PeriodNo);

                SharedFunction.doSendMaild(new SharedFunction._MailQueryState { setYear = viewModel.Year.Value, setPeriod = viewModel.PeriodNo.Value });
                SharedFunction.doSendSMSMessage(new SharedFunction._MailQueryState { setYear = viewModel.Year.Value, setPeriod = viewModel.PeriodNo.Value });

                ViewBag.Message = "對獎作業完成!!";
                return View("~/Views/Shared/AlertMessage.ascx");
            }
            else
            {
                return result;
            }
        }

        public ActionResult ClearWinningInvoiceNo(InquireNoIntervalViewModel viewModel)
        {
            ViewResult result = (ViewResult)Inquire(viewModel);
            IQueryable<UniformInvoiceWinningNumber> items = result.Model as IQueryable<UniformInvoiceWinningNumber>;
            if (items != null && items.Count() > 0)
            {
                models.ExecuteCommand(@"	
                    DELETE FROM InvoiceWinningNumber
	                FROM     UniformInvoiceWinningNumber INNER JOIN
					                InvoiceWinningNumber ON UniformInvoiceWinningNumber.WinningID = InvoiceWinningNumber.WinningID
	                WHERE   (UniformInvoiceWinningNumber.Period = {0}) AND (UniformInvoiceWinningNumber.Year = {1})", viewModel.PeriodNo, viewModel.Year);
                ViewBag.Message = "中獎發票已清除完成!!";
                return View("~/Views/Shared/AlertMessage.ascx");
            }
            else
            {
                return result;
            }
        }

    }
}