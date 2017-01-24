using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Business.Helper;
using eIVOGo.Helper;
using eIVOGo.Models.ViewModel;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Newtonsoft.Json;
using Utility;

namespace eIVOGo.Controllers
{
    [Authorize]
    public class HandlingController : SampleController<InvoiceItem>
    {
        protected UserProfileMember _userProfile;

        public HandlingController() : base()
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Hello,World!!";
            return View();
        }

        // GET: Handling
        public ActionResult DisableCompany(int companyID)
        {
            updateCompanyStatus(companyID, Naming.MemberStatusDefinition.Mark_To_Delete);
            return View("Index");
        }

        private void updateCompanyStatus(int companyID, Naming.MemberStatusDefinition status)
        {
            if (!_userProfile.CheckSystemCompany())
            {
                ViewBag.Message = "使用者非系統管理公司!!";
                return;
            }

            ViewBag.Message = "資料不存在!!";

            using (ModelSource<Organization> models = new ModelSource<Organization>())
            {
                OrganizationStatus item = models.GetTable<Organization>().Where(o => o.CompanyID == companyID)
                    .Select(o => o.OrganizationStatus).FirstOrDefault();
                if (item != null)
                {
                    item.CurrentLevel = (int)status;
                    models.SubmitChanges();
                    ViewBag.Message = "資料已更新!!";
                }
            }
        }

        public ActionResult EnableCompany(int companyID)
        {
            updateCompanyStatus(companyID, Naming.MemberStatusDefinition.Checked);
            return View("Index");
        }

        public ActionResult ApplyRelationship(int companyID)
        {
            var item = models.GetTable<Organization>().Where(o => o.CompanyID == companyID).FirstOrDefault();
            if (item == null)
            {
                ViewBag.Message = "資料不存在!!";
            }
            else
            {
                if (!item.IsEnterpriseGroupMember())
                {
                    models.GetTable<EnterpriseGroupMember>().InsertOnSubmit(
                        new EnterpriseGroupMember
                        {
                            EnterpriseID = (int)Naming.EnterpriseGroup.網際優勢股份有限公司,
                            CompanyID = item.CompanyID
                        });
                    models.SubmitChanges();
                    ViewBag.Message = "設定完成!!";
                }
                else
                {
                    ViewBag.Message = "設開立人已是B2B營業人!!";
                }
            }

            return View("Index");
        }

        public ActionResult MailTracking()
        {
            return View("InvoiceMailTracking");
        }

        public ActionResult InquireToTrackMail(MailTrackingViewModel viewModel)
        {
            ViewBag.ViewModel = viewModel;
            viewModel.StartNo = viewModel.StartNo.GetEfficientString();
            viewModel.EndNo = viewModel.EndNo.GetEfficientString();
            if (viewModel.StartNo == null || viewModel.StartNo.Length != 10)
            {
                ModelState.AddModelError("StartNo", "發票起號由2碼英文+8碼數字組成，共10碼。");
            }
            else if (viewModel.EndNo == null || viewModel.EndNo.Length != 10)
            {
                ModelState.AddModelError("EndNo", "發票迄號由2碼英文+8碼數字組成，共10碼。");
            }
            else if(viewModel.StartNo.Substring(0, 2) != viewModel.EndNo.Substring(0, 2))
            {
                ModelState.AddModelError("StartNo", "發票起、迄號字軌不相同!!");
                ModelState.AddModelError("EndNo", "發票起、迄號字軌不相同!!");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ModelState = ModelState;
                return View("~/Views/Shared/ReportInputError.ascx");
            }

            String startNo = viewModel.StartNo.Substring(2, 8);
            String endNo = viewModel.EndNo.Substring(2, 8);
            String trackCode = viewModel.StartNo.Substring(0, 2);

            var items = models.GetTable<InvoiceItem>().Where(i => i.TrackCode == trackCode
                && String.Compare(i.No, startNo) >= 0
                && String.Compare(i.No, endNo) <= 0
                && i.InvoiceCancellation == null)
                .Where(i => i.InvoiceBuyer.Address != null && i.InvoiceBuyer.ReceiptNo == "0000000000")
                .OrderBy(i => i.TrackCode).ThenBy(i => i.No)
                .Concat(models.GetTable<InvoiceItem>().Where(i => i.TrackCode == trackCode
                    && String.Compare(i.No, startNo) >= 0
                    && String.Compare(i.No, endNo) <= 0
                    && i.InvoiceCancellation == null)
                    .Where(i => i.InvoiceBuyer.Address != null && i.InvoiceBuyer.ReceiptNo != "0000000000")
                    .OrderBy(i => i.TrackCode).ThenBy(i => i.No));

            return View("~/Views/Handling/MailTracking/QueryResult.ascx", items);
        }

        public ActionResult InvoiceMailItems(bool? showTable,int[] id)
        {
            IQueryable<InvoiceItem> items;
            if (id != null && id.Length > 0)
            {
                items = models.GetTable<InvoiceItem>().Where(i => id.Contains(i.InvoiceID));
            }
            else
            {
                items = models.GetTable<InvoiceItem>().Where(i => false);
            }
            if(showTable==true)
            {
                return View("~/Views/Handling/MailTracking/TableList.ascx", items);
            }
            else
            {
                return View("~/Views/Handling/MailTracking/ItemList.ascx", items);
            }
        }

        public ActionResult PackInvoice(int[] id)
        {
            IQueryable<InvoiceItem> items;
            if (id != null && id.Length > 0)
            {
                items = models.GetTable<InvoiceItem>().Where(i => id.Contains(i.InvoiceID));
            }
            else
            {
                items = models.GetTable<InvoiceItem>().Where(i => false);
            }

            if (items.Count() == 0)
            {
                ViewBag.Message = "發票資料錯誤!!";
                return View("~/Views/Shared/AlertMessage.ascx");
            }
            else
            {
                return View("~/Views/Handling/MailTracking/PackedItemList.ascx", items);
            }
        }

        public ActionResult DownloadXlsx(String data)
        {
            var items = JsonConvert.DeserializeObject<MailTrackingCsvViewMode[]>(data);
            if (items == null || items.Length == 0)
            {
                ViewBag.Message = "請選擇郵寄項目!!";
                return View("~/Views/Shared/AlertMessage.ascx");
            }
            var invoiceID = items.Select(i => i.PackageID).ToArray();
            var resultItems = models.GetTable<InvoiceItem>().Where(i => invoiceID.Contains(i.InvoiceID))
                .Select(i => new {
                    掛號號碼 = "",
                    姓名 = i.InvoiceBuyer.ContactName,
                    寄件地名或地址 = i.InvoiceBuyer.Address,
                    Google_id = i.InvoiceBuyer.CustomerID,
                    發票號碼 = i.TrackCode+i.No,
                    發票日期 = i.InvoiceDate,
                    PackageID = i.InvoiceID
                });

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Cache-control", "max-age=1");
            Response.ContentType = "message/rfc822";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", HttpUtility.UrlEncode("掛號郵件號碼明細.xlsx")));


            using (DataSet ds = new DataSet())
            {
                DataTable table = new DataTable();
                ds.Tables.Add(table);
                table.Columns.Add("掛號號碼");
                table.Columns.Add("姓名");
                table.Columns.Add("寄件地名或地址");
                table.Columns.Add("是否回");
                table.Columns.Add("是否航空");
                table.Columns.Add("是否印刷物");
                table.Columns.Add("重量");
                table.Columns.Add("郵資");
                table.Columns.Add("備考");
                table.Columns.Add("遞件日期");
                table.Columns.Add("Google_id");
                table.Columns.Add("發票號碼");
                table.Columns.Add("發票日期");

                DataSource.GetDataSetResult(resultItems, table);
                foreach(var row in table.AsEnumerable())
                {
                    var p = items.First(m => m.PackageID == (int)row["PackageID"]);
                    row["掛號號碼"] = p.MailNo1;
                    row["遞件日期"] = p.DeliveryDate;
                    if (p.InvoiceID.Length > 1)
                        row["備考"] = p.InvoiceID.Length;
                }

                table.Columns.Remove("PackageID");

                using (var xls = ds.ConvertToExcel())
                {
                    xls.SaveAs(Response.OutputStream);
                }
            }

            return new EmptyResult();
        }

        // GET: Handling/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Handling/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Handling/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Handling/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
