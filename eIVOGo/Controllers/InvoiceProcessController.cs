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
using eIVOGo.Properties;
using Model.DataEntity;
using Model.Helper;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;

namespace eIVOGo.Controllers
{
    public class InvoiceProcessController : SampleController<InvoiceItem>
    {
        protected UserProfileMember _userProfile;

        protected ModelSourceInquiry<InvoiceItem> createModelInquiry()
        {
            _userProfile = WebPageUtility.UserProfile;

            var inquireConsumption = new InquireInvoiceConsumption { ControllerName = "InquireInvoice", ActionName = "ByConsumption", CurrentController = this };
            inquireConsumption.Append(new InquireInvoiceConsumptionExtensionToPrint(inquireConsumption) { CurrentController = this });

            return (ModelSourceInquiry<InvoiceItem>)(new InquireEffectiveInvoice { CurrentController = this })
                .Append(new InquireInvoiceByRole(_userProfile) { CurrentController = this })
                .Append(inquireConsumption)
                .Append(new InquireInvoiceSeller { ControllerName = "InquireInvoice", ActionName = "BySeller", /*QueryRequired = true, AlertMessage = "請選擇公司名稱!!",*/ CurrentController = this })
                .Append(new InquireInvoiceBuyer { ControllerName = "InquireInvoice", ActionName = "ByBuyer", CurrentController = this })
                .Append(new InquireInvoiceBuyerByName { ControllerName = "InquireInvoice", ActionName = "ByBuyerName", CurrentController = this })
                .Append(new InquireInvoiceDate { ControllerName = "InquireInvoice", ActionName = "ByInvoiceDate", CurrentController = this })
                .Append(new InquireInvoiceAttachment { /*ControllerName = "InquireInvoice", ActionName = "ByAttachment",*/ CurrentController = this })
                .Append(new InquireInvoiceNo { CurrentController = this })
                .Append(new InquireInvoiceAgent { ControllerName = "InquireInvoice", ActionName = "ByAgent", CurrentController = this })
                .Append(new InquireWinningInvoice { CurrentController = this });
        }

        public ActionResult Index(InquireInvoiceViewModel viewModel)
        {
            ViewBag.ResultAction = "Common";
            DataLoadOptions ops = new DataLoadOptions();
            ops.LoadWith<InvoiceItem>(i => i.InvoiceBuyer);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceCancellation);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceAmountType);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceSeller);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceWinningNumber);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceCarrier);
            ops.LoadWith<InvoiceItem>(i => i.InvoicePurchaseOrder);
            models.GetDataContext().LoadOptions = ops;

            ViewBag.ViewModel = viewModel;

            models.Inquiry = createModelInquiry();

            var profile = HttpContext.GetUser();

            switch ((Naming.CategoryID)profile.CurrentUserRole.OrganizationCategory.CategoryID)
            {
                case Naming.CategoryID.COMP_E_INVOICE_B2C_SELLER:
                case Naming.CategoryID.COMP_INVOICE_AGENT:
                    ViewBag.InquiryView = "~/Views/InvoiceProcess/InvoiceQueryBySeller.ascx";
                    break;
                case Naming.CategoryID.COMP_E_INVOICE_B2C_BUYER:
                    ViewBag.InquiryView = "~/Views/InvoiceProcess/InvoiceQueryByBuyer.ascx";
                    break;
                default:
                    ViewBag.InquiryView = "~/Views/InvoiceProcess/InvoiceQuery.ascx";
                    break;
            }

            return View(models.Inquiry);

        }

        public ActionResult IssuingNotice(InquireInvoiceViewModel viewModel)
        {
            ViewResult result = (ViewResult)Index(viewModel);
            result.ViewName = "Index";
            ViewBag.ResultAction = "Notify";
            ViewBag.InquiryView = "~/Views/InvoiceProcess/InvoiceQueryForNotice.ascx";
            return result;
        }

        public ActionResult InquireToCancel(InquireInvoiceViewModel viewModel)
        {
            ViewResult result = (ViewResult)Index(viewModel);
            result.ViewName = "Index";
            ViewBag.ResultAction = "CancelInvoice";
            ViewBag.InquiryView = "~/Views/InvoiceProcess/InvoiceQueryForCancellation.ascx";
            return result;
        }

        public ActionResult InquireToMIG(InquireInvoiceViewModel viewModel)
        {
            ViewResult result = (ViewResult)Index(viewModel);
            result.ViewName = "Index";
            ViewBag.ResultAction = "CreateMIG";
            ViewBag.InquiryView = "~/Views/InvoiceProcess/InvoiceQueryForMIG.ascx";
            return result;
        }

        public ActionResult InquireInvoiceMedia(int sellerID,int year,int periodNo,String taxNo)
        {
            DateTime startDate = new DateTime(year, periodNo * 2 - 1, 1);
            DataLoadOptions ops = new DataLoadOptions();
            ops.LoadWith<InvoiceItem>(i => i.InvoiceBuyer);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceCancellation);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceAmountType);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceSeller);
            models.GetDataContext().LoadOptions = ops;

            var items = models.GetTable<InvoiceItem>().Where(i => i.SellerID == sellerID
                    && i.InvoiceDate >= startDate 
                    && i.InvoiceDate < startDate.AddMonths(2))
                .OrderBy(i => i.InvoiceID);

            if (items.Count() > 0)
            {
                ViewBag.TaxNo = taxNo;
                ViewBag.FileName = String.Format("{0:d4}{1:d2}({2}).txt", year, periodNo, items.First().Organization.ReceiptNo);
            }
            else
            {
                ViewBag.FileName = "test.txt";
            }

            return View(items);
        }

        public ActionResult InquireForIncoming(InquireInvoiceViewModel viewModel)
        {
            ViewResult result = (ViewResult)Index(viewModel);
            result.ViewName = "Index";
            ViewBag.ResultAction = "Incoming";
            ViewBag.InquiryView = "~/Views/InvoiceProcess/InvoiceQueryByBuyer.ascx";
            return result;
        }

        public ActionResult InquireToAuthorize(InquireInvoiceViewModel viewModel)
        {
            ViewResult result = (ViewResult)Index(viewModel);
            result.ViewName = "Index";
            ViewBag.ResultAction = "Authorize";
            ViewBag.InquiryView = "~/Views/InvoiceProcess/InvoiceQueryForNotice.ascx";

            return result;
        }

        public ActionResult Inquire(InquireInvoiceViewModel viewModel)
        {
            //ViewBag.HasQuery = true;
            DataLoadOptions ops = new DataLoadOptions();
            ops.LoadWith<InvoiceItem>(i => i.InvoiceBuyer);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceCancellation);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceAmountType);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceSeller);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceWinningNumber);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceCarrier);
            ops.LoadWith<InvoiceItem>(i => i.InvoicePurchaseOrder);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceDetails);
            ops.LoadWith<InvoiceDetail>(i => i.InvoiceProduct);
            //ops.LoadWith<InvoiceProduct>(i => i.InvoiceProductItem);

            models.GetDataContext().LoadOptions = ops;

            ViewBag.ViewModel = viewModel;

            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            ViewBag.PageSize = viewModel.PageSize.HasValue && viewModel.PageSize > 0 ? viewModel.PageSize.Value : Uxnet.Web.Properties.Settings.Default.PageSize;
            ViewBag.DataItemView = "~/Views/InvoiceProcess/Module/DataItem.ascx";

            if (viewModel.PageIndex.HasValue)
            {
                if (viewModel.Sort != null && viewModel.Sort.Length > 0)
                    ViewBag.Sort = viewModel.Sort.Where(s => s.HasValue).Select(s => s.Value).ToArray();
                ViewBag.PageIndex = viewModel.PageIndex - 1;
                return View("~/Views/InvoiceProcess/Module/ItemList.ascx", models.Items);
            }
            else
            {
                ViewBag.PageIndex = 0;
                ViewBag.ResultAction = checkQueryAction(viewModel.ResultAction);
                return View("~/Views/InvoiceProcess/Module/QueryResult.ascx", models.Items);
            }
        }

        private String checkQueryAction(String resultAction)
        {
            switch(resultAction)
            {
                case "Notify":
                    return "~/Views/InvoiceProcess/ResultAction/DoNotify.ascx";
                case "CancelInvoice":
                    return "~/Views/InvoiceProcess/ResultAction/DoCancel.ascx";
                case "CreateMIG":
                    return "~/Views/InvoiceProcess/ResultAction/DownloadMIG.ascx";
                case "Authorize":
                    ViewBag.DataItemView = "~/Views/InvoiceProcess/Module/AuthorizeDataItemToPrint.ascx";
                    return "~/Views/InvoiceProcess/ResultAction/DoAuthorize.ascx";
                case "Incoming":
                    ViewBag.DataItemView = "~/Views/InvoiceProcess/Buyer/DataItem.ascx";
                    return "~/Views/InvoiceProcess/ResultAction/MainQueryAction.ascx";
                default:
                    return "~/Views/InvoiceProcess/ResultAction/MainQueryAction.ascx";
            }
        }


        //public ActionResult InvoiceAttachment()
        //{
        //    //ViewBag.HasQuery = false;
        //    ViewBag.QueryAction = "InquireAttachment";
        //    ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
        //    TempData.SetModelSource(models);
        //    models.Inquiry = createModelInquiry();

        //    return View("InvoiceReport", models.Inquiry);
        //}

        //public ActionResult InquireAttachment()
        //{
        //    //ViewBag.HasQuery = true;
        //    ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
        //    TempData.SetModelSource(models);
        //    models.Inquiry = createModelInquiry();
        //    models.BuildQuery();

        //    return View("AttachmentResult", models.Inquiry);
        //}

        //public ActionResult AttachmentGridPage(int index, int size)
        //{
        //    //ViewBag.HasQuery = true;
        //    ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
        //    TempData.SetModelSource(models);
        //    models.Inquiry = createModelInquiry();
        //    models.BuildQuery();

        //    if (index > 0)
        //        index--;
        //    else
        //        index = 0;

        //    return View(models.Items.OrderByDescending(d => d.InvoiceID)
        //        .Skip(index * size).Take(size)
        //        .ToArray());
        //}


        //public ActionResult GridPage(int index,int size)
        //{
        //    //ViewBag.HasQuery = true;
        //    ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
        //    TempData.SetModelSource(models);
        //    models.Inquiry = createModelInquiry();
        //    models.BuildQuery();

        //    if (index > 0)
        //        index--;
        //    else
        //        index = 0;

        //    return View(models.Items.OrderByDescending(d => d.InvoiceID)
        //        .Skip(index * size).Take(size)
        //        .ToArray());
        //}

        public ActionResult CreateXlsx()
        {
            DataLoadOptions ops = new DataLoadOptions();
            ops.LoadWith<InvoiceItem>(i => i.InvoiceBuyer);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceCancellation);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceAmountType);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceSeller);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceWinningNumber);
            ops.LoadWith<InvoiceItem>(i => i.InvoiceCarrier);
            ops.LoadWith<InvoiceItem>(i => i.InvoicePurchaseOrder);
            models.GetDataContext().LoadOptions = ops;

            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            var items = models.Items.OrderBy(i => i.InvoiceID)
                .Select(i => new
                {
                    發票號碼 = i.TrackCode + i.No,
                    發票日期 = i.InvoiceDate,
                    附件檔名 = i.CDS_Document.Attachment.Count > 0 ? i.CDS_Document.Attachment.First().KeyName : null,
                    客戶ID = i.InvoiceBuyer.CustomerID,
                    序號 = i.InvoicePurchaseOrder != null ? i.InvoicePurchaseOrder.OrderNo : null,
                    發票開立人 = i.InvoiceSeller.CustomerName,
                    開立人統編 = i.InvoiceSeller.ReceiptNo,
                    未稅金額 = i.InvoiceAmountType.SalesAmount,
                    稅額 = i.InvoiceAmountType.TaxAmount,
                    含稅金額 = i.InvoiceAmountType.TotalAmount,
                    買受人名稱 = i.InvoiceBuyer.CustomerName,
                    買受人統編 = i.InvoiceBuyer.IsB2C() ? "" : i.InvoiceBuyer.ReceiptNo,
                    連絡人名稱 = i.InvoiceBuyer.ContactName,
                    連絡人地址 = i.InvoiceBuyer.Address,
                    買受人EMail = i.InvoiceBuyer.EMail,
                    愛心碼 = i.InvoiceDonation.AgencyCode,
                    是否中獎 = i.InvoiceWinningNumber.UniformInvoiceWinningNumber.PrizeType,
                    載具類別 = i.InvoiceCarrier.CarrierType,
                    載具號碼 = i.InvoiceCarrier.CarrierNo,
                    備註 = String.Join("", i.InvoiceDetails.Select(t => t.InvoiceProduct.InvoiceProductItem.FirstOrDefault())
                        .Select(p => p.Remark))
                });


            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Cache-control", "max-age=1");
            Response.ContentType = "message/rfc822";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", HttpUtility.UrlEncode("發票資料明細.xlsx")));

            using (var xls = DataSource.GetExcelResult(items, "發票資料明細"))
            {
                xls.SaveAs(Response.OutputStream);
            }

            return new EmptyResult();
        }

        public ActionResult AssignDownload()
        {
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            String resultFile = Path.Combine(Logger.LogDailyPath, Guid.NewGuid().ToString() + ".xlsx");
            _userProfile["assignDownload"] = resultFile;

            ThreadPool.QueueUserWorkItem(stateInfo => 
            {
                try
                {
                    SqlCommand sqlCmd = (SqlCommand)models.GetCommand(models.Items);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            adapter.Fill(ds);
                            using(XLWorkbook xls = new XLWorkbook())
                            {
                                xls.Worksheets.Add(ds);
                                xls.SaveAs(resultFile);
                            }
                        }
                    }
                    models.Dispose();
                }
                catch(Exception ex)
                {
                    Logger.Error(ex);
                }
            });

            return Content("下載資料請求已送出!!");
        }



        public ActionResult Print(int[] chkItem,String paperStyle)
        {

            var profile = HttpContext.GetUser();
            String printUrl = null;
            String keyCodeFile = Path.Combine(Logger.LogPath, "ORCodeKey.txt");
            if (System.IO.File.Exists(keyCodeFile))
            {
                if (!String.IsNullOrEmpty(System.IO.File.ReadAllText(keyCodeFile)))
                {
                    if (profile.EnqueueDocumentPrint(models, chkItem))
                    {
                        if (paperStyle == "A4")
                            printUrl = "~/SAM/NewPrintInvoiceAsPDF.aspx";
                        else if (paperStyle == "POS")
                            printUrl = "~/SAM/NewPrintInvoicePOSAsPDF.aspx?printBuyerAddr=" + Request["printBuyerAddr"];
                    }
                    else
                        ViewBag.Message = "資料已列印請重新選擇!!";
                }
                else
                {
                    ViewBag.Message = "QRCode金鑰檔無內容，無法列印!!";
                }
            }
            else
            {
                ViewBag.Message = "無QRCode金鑰檔，無法列印!!";
            }

            return View("~/Views/InvoiceProcess/Module/PrintResult.ascx", model: printUrl);
        }

        public ActionResult IssueInvoiceNotice(int[] chkItem)
        {
            if (chkItem != null && chkItem.Count() > 0)
            {
                chkItem.SendIssuingNotification();
                ViewBag.Message = "Email通知已重送!!";
                return View("~/Views/Shared/AlertMessage.ascx");
            }
            else
            {
                ViewBag.Message = "請選擇重送資料!!";
                return View("~/Views/Shared/AlertMessage.ascx");
            }

        }


        public ActionResult CancelInvoice(int[] chkItem)
        {
            if (chkItem != null && chkItem.Count() > 0)
            {
                InvoiceManager mgr = new InvoiceManager(models);
                mgr.CancelInvoice(chkItem);
                if (mgr.EventItems != null && mgr.EventItems.Count > 0)
                {
                    ViewBag.Message = "下列發票已作廢完成!!\r\n" + String.Join("\r\n", mgr.EventItems.Select(i => i.TrackCode + i.No));
                }
                return View("~/Views/Shared/AlertMessage.ascx");
            }
            else
            {
                ViewBag.Message = "請選擇作廢資料!!";
                return View("~/Views/Shared/AlertMessage.ascx");
            }

        }

        public ActionResult AuthorizeToPrint(int[] chkItem)
        {
            if (chkItem != null && chkItem.Count() > 0)
            {
                var items = models.GetTable<InvoiceItem>().Where(i => chkItem.Contains(i.InvoiceID))
                        .Where(i => i.CDS_Document.DocumentPrintLog.Any() && i.CDS_Document.DocumentAuthorization == null)
                        .Select(i=>i.InvoiceID).ToList()
                        .Select(i => new DocumentAuthorization
                            {
                                DocID = i
                            }).ToList();

                models.GetTable<DocumentAuthorization>().InsertAllOnSubmit(items);
                models.SubmitChanges();
            
                ViewBag.Message = "下列發票已核准重印!!\r\n" + String.Join("\r\n", items.Select(i => i.CDS_Document.InvoiceItem.TrackCode + i.CDS_Document.InvoiceItem.No));
                return View("~/Views/Shared/AlertMessage.ascx");
            }
            else
            {
                ViewBag.Message = "請選擇核准重印資料!!";
                return View("~/Views/Shared/AlertMessage.ascx");
            }

        }


        public ActionResult DownloadC0401(int[] chkItem)
        {
            if (chkItem != null && chkItem.Count() > 0)
            {
                var items = models.GetTable<InvoiceItem>().Where(i => chkItem.Contains(i.InvoiceID));
                return zipItems(items, i => i.CreateC0401().ConvertToXml(), "C0401");
            }
            else
            {
                ViewBag.Message = "請選擇下載資料!!";
                return View("~/Views/Shared/ShowMessage.aspx");
            }

        }

        public ActionResult DownloadC0701(int[] chkItem)
        {
            if (chkItem != null && chkItem.Count() > 0)
            {
                var items = models.GetTable<InvoiceItem>().Where(i => chkItem.Contains(i.InvoiceID));
                return zipItems(items, i => i.CreateC0701().ConvertToXml(), "C0701");
            }
            else
            {
                ViewBag.Message = "請選擇下載資料!!";
                return View("~/Views/Shared/ShowMessage.aspx");
            }
        }

        public ActionResult DownloadC0501(int[] chkItem)
        {
            if (chkItem != null && chkItem.Count() > 0)
            {
                var items = models.GetTable<InvoiceItem>().Where(i => chkItem.Contains(i.InvoiceID));
                return zipItems(items, i => i.CreateC0501().ConvertToXml(), "C0501");
            }
            else
            {
                ViewBag.Message = "請選擇下載資料!!";
                return View("~/Views/Shared/ShowMessage.aspx");
            }

        }

        private ActionResult zipItems(IEnumerable<InvoiceItem> items,Func<InvoiceItem,XmlDocument> convertTo,String docName)
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
                    foreach (var item in items)
                    {
                        var docItem = convertTo(item);
                        ZipArchiveEntry entry = zip.CreateEntry(String.Format("{0}_{1}{2}.xml", docName, item.TrackCode, item.No));
                        using (Stream outStream = entry.Open())
                        {
                            docItem.Save(outStream);
                        }
                    }
                }
            }

            var result = new FilePathResult(outFile, "message/rfc822");
            result.FileDownloadName = docName + ".zip";
            return result;
        }

        public ActionResult InvoiceSummary()
        {
            //ViewBag.HasQuery = false;
            ViewBag.QueryAction = "InquireSummary";
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();

            return View("InvoiceReport", models.Inquiry);
        }

        public ActionResult InquireSummary()
        {
            //ViewBag.HasQuery = true;
            ViewBag.PrintAction = "PrintInvoiceSummary";
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            return View("InvoiceSummaryResult", models.Inquiry);
        }

        public ActionResult InvoiceSummaryGridPage(int index, int size)
        {
            //ViewBag.HasQuery = true;
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            if (index > 0)
                index--;
            else
                index = 0;

            models.InquiryPageIndex = index;
            models.InquiryPageSize = size;

            return View(models.Items);
        }

        public ActionResult CreateInvoiceSummaryXlsx()
        {
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            _userProfile["modelSource"] = models;
            Server.Transfer("~/MvcHelper/CreateInvoiceSummaryReport.aspx");

            return new EmptyResult();
        }

        public ActionResult PrintInvoiceSummary()
        {
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();
            models.BuildQuery();
            models.ResultModel = Naming.DataResultMode.Print;

            return View(models.Inquiry);
        }

    }
}
