using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DataEntity;
using eIVOGo.Helper;
using eIVOGo.Models;
using Model.Security.MembershipManagement;
using Business.Helper;
using System.Text;
using Model.Locale;
using System.Web.Script.Serialization;
using System.IO;
using System.IO.Compression;
using Utility;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using System.Data.Linq;

namespace eIVOGo.Controllers
{
    public class InvoiceQueryController : SampleController<InvoiceItem>
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
                .Append(new InquireInvoiceAttachment { ControllerName = "InquireInvoice", ActionName = "ByAttachment", CurrentController = this })
                .Append(new InquireInvoiceNo { CurrentController = this })
                .Append(new InquireInvoiceAgent { ControllerName = "InquireInvoice", ActionName = "ByAgent", CurrentController = this })
                .Append(new InquireWinningInvoice { CurrentController = this });
        }

        public ActionResult InvoiceReport()
        {
            //ViewBag.HasQuery = false;
            ViewBag.QueryAction = "Inquire";
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();

            return View(models.Inquiry);
        }

        public ActionResult InvoiceMediaReport()
        {
            ViewBag.QueryAction = "InquireInvoiceMedia";
            return View();
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
                ViewBag.TaxNo = taxNo.GetEfficientString();
                ViewBag.FileName = String.Format("{0:d4}{1:d2}({2}).txt", year, periodNo, items.First().Organization.ReceiptNo);
                var orgItem = models.GetTable<Organization>().Where(o => o.CompanyID == sellerID).First();
                if (orgItem.OrganizationExtension == null)
                    orgItem.OrganizationExtension = new OrganizationExtension { };
                if (orgItem.OrganizationExtension.TaxNo != (String)ViewBag.TaxNo)
                {
                    orgItem.OrganizationExtension.TaxNo = (String)ViewBag.TaxNo;
                    models.SubmitChanges();
                }
            }
            else
            {
                ViewBag.Message = "資料不存在!!";
                return View("InvoiceMediaReport");
            }

            return View(items);
        }



        public ActionResult Inquire()
        {
            //ViewBag.HasQuery = true;
            ViewBag.PrintAction = "PrintResult";
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            return View("InquiryResult",models.Inquiry);
        }

        public ActionResult InvoiceAttachment()
        {
            //ViewBag.HasQuery = false;
            ViewBag.QueryAction = "InquireAttachment";
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();

            return View("InvoiceReport", models.Inquiry);
        }

        public ActionResult InquireAttachment()
        {
            //ViewBag.HasQuery = true;
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            return View("AttachmentResult", models.Inquiry);
        }

        public ActionResult AttachmentGridPage(int index, int size)
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

            return View(models.Items.OrderByDescending(d => d.InvoiceID)
                .Skip(index * size).Take(size)
                .ToArray());
        }


        public ActionResult GridPage(int index,int size)
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

            return View(models.Items.OrderByDescending(d => d.InvoiceID)
                .Skip(index * size).Take(size)
                .ToArray());
        }

        public ActionResult DownloadCSV()
        {
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            Response.ContentEncoding = Encoding.GetEncoding(950);

            return View(models.Items);
        }

        public ActionResult CreateXlsx()
        {
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            _userProfile["modelSource"] = models;
            Server.Transfer("~/MvcHelper/CreateInvoiceReport.aspx");

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



        public ActionResult PrintResult()
        {
            ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();
            models.BuildQuery();
            models.ResultModel = Naming.DataResultMode.Print;

            return View(models.Inquiry);
        }

        public ActionResult DownloadAttachment()
        {
            String jsonData = Request["data"];
            if (!String.IsNullOrEmpty(jsonData))
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                int[] invoiceID = serializer.Deserialize<int[]>(jsonData);
                using (ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>())
                {
                    return zipAttachment(models.EntityList.Where(i => invoiceID.Contains(i.InvoiceID)));
                }
            }
            return Content(jsonData);
        }

        public ActionResult DownloadAll()
        {
            using (ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>())
            {
                TempData.SetModelSource(models);
                models.Inquiry = createModelInquiry();
                models.BuildQuery();

                return zipAttachment(models.Items);
            }
        }

        private ActionResult zipAttachment(IEnumerable<InvoiceItem> items)
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
                        if (item.CDS_Document.Attachment.Count > 0)
                        {
                            for (int i = 0; i < item.CDS_Document.Attachment.Count; i++)
                            {
                                var attach = item.CDS_Document.Attachment[i];
                                if (System.IO.File.Exists(attach.StoredPath))
                                {
                                    ZipArchiveEntry entry = zip.CreateEntry(i == 0 ? item.TrackCode + item.No + ".pdf" : item.TrackCode + item.No + "-" + i + ".pdf");
                                    using (Stream outStream = entry.Open())
                                    {
                                        using (var inStream = System.IO.File.Open(attach.StoredPath, FileMode.Open))
                                        {
                                            inStream.CopyTo(outStream);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var result = new FilePathResult(outFile, "message/rfc822");
            result.FileDownloadName = "發票附件.zip";
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
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            if (index > 0)
                index--;
            else
                index = 0;

            ViewBag.PageIndex = index;
            ViewBag.PageSize = size;

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



        // POST: InvoiceQuery/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: InvoiceQuery/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: InvoiceQuery/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: InvoiceQuery/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: InvoiceQuery/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
