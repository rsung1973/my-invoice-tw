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
    public class AllowanceProcessController : SampleController<InvoiceItem>
    {
        protected UserProfileMember _userProfile;

        protected ModelSourceInquiry<InvoiceAllowance> createModelInquiry()
        {
            _userProfile = WebPageUtility.UserProfile;

            var inquireConsumption = new InquireAllowanceConsumption { CurrentController = this };
            
            return (ModelSourceInquiry<InvoiceAllowance>)(new InquireEffectiveAllowance { CurrentController = this })
                .Append(new InquireAllowanceByRole(_userProfile) { CurrentController = this })
                .Append(inquireConsumption)
                .Append(new InquireAllowanceSeller { CurrentController = this })
                .Append(new InquireAllowanceBuyer { CurrentController = this })
                .Append(new InquireAllowanceBuyerByName { CurrentController = this })
                .Append(new InquireAllowanceDate { CurrentController = this })
                .Append(new InquireAllowanceNo { CurrentController = this })
                .Append(new InquireAllowanceAgent { CurrentController = this });
        }

        public ActionResult Index(InquireInvoiceViewModel viewModel)
        {
            ViewBag.ViewModel = viewModel;
            return View();
        }

        public ActionResult Inquire(int? pageIndex,int?[] sort,int? pageSize, InquireInvoiceViewModel viewModel)
        {

            ViewBag.ViewModel = viewModel;

            DataLoadOptions ops = new DataLoadOptions();
            ops.LoadWith<InvoiceAllowance>(i => i.InvoiceAllowanceBuyer);
            ops.LoadWith<InvoiceAllowance>(i => i.InvoiceAllowanceSeller);
            models.GetDataContext().LoadOptions = ops;

            var modelSource = new ModelSource<InvoiceAllowance>(models);

            modelSource.Inquiry = createModelInquiry();
            modelSource.BuildQuery();

            ViewBag.PageSize = pageSize.HasValue && pageSize > 0 ? pageSize.Value : Uxnet.Web.Properties.Settings.Default.PageSize;

            if (pageIndex.HasValue)
            {
                if (sort != null && sort.Length > 0)
                    ViewBag.Sort = sort.Where(s => s.HasValue).Select(s => s.Value).ToArray();
                ViewBag.PageIndex = pageIndex - 1;
                return View("~/Views/AllowanceProcess/Module/ItemList.ascx", modelSource.Items);
            }
            else
            {
                ViewBag.PageIndex = 0;
                return View("~/Views/AllowanceProcess/Module/QueryResult.ascx", modelSource.Items);
            }
        }


        public ActionResult CreateXlsx()
        {
            DataLoadOptions ops = new DataLoadOptions();
            ops.LoadWith<InvoiceAllowance>(i => i.InvoiceAllowanceBuyer);
            ops.LoadWith<InvoiceAllowance>(i => i.InvoiceAllowanceSeller);
            models.GetDataContext().LoadOptions = ops;

            ModelSource<InvoiceAllowance> modelSource = new ModelSource<InvoiceAllowance>(models);

            modelSource.Inquiry = createModelInquiry();
            modelSource.BuildQuery();

            var items = modelSource.Items.OrderBy(i => i.AllowanceID)
                .Select(i => new
                {
                    折讓單號碼 = i.AllowanceNumber,
                    折讓日期 = i.AllowanceDate,
                    客戶ID = i.InvoiceAllowanceBuyer.CustomerID,
                    發票開立人 = i.InvoiceAllowanceSeller.CustomerName,
                    開立人統編 = i.InvoiceAllowanceSeller.ReceiptNo,
                    未稅金額 = i.TotalAmount-i.TaxAmount,
                    稅額 = i.TaxAmount,
                    含稅金額 = i.TotalAmount,
                    買受人名稱 = i.InvoiceAllowanceBuyer.CustomerName,
                    買受人統編 = i.InvoiceAllowanceBuyer.ReceiptNo,
                    連絡人名稱 = i.InvoiceAllowanceBuyer.ContactName,
                    連絡人地址 = i.InvoiceAllowanceBuyer.Address,
                    買受人EMail = i.InvoiceAllowanceBuyer.EMail,
                    備註 = String.Join("", i.InvoiceAllowanceDetails.Select(t => t.InvoiceAllowanceItem)
                        .Select(p => p.Remark))
                });


            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Cache-control", "max-age=1");
            Response.ContentType = "message/rfc822";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", HttpUtility.UrlEncode("折讓資料明細.xlsx")));

            using (var xls = DataSource.GetExcelResult(items, "折讓資料明細"))
            {
                xls.SaveAs(Response.OutputStream);
            }

            return new EmptyResult();
        }



        public ActionResult Print(int[] chkItem, String paperStyle)
        {

            var profile = HttpContext.GetUser();
            String printUrl = null;
            if (profile.EnqueueDocumentPrint(models, chkItem))
            {
                printUrl = "~/SAM/PrintAllowanceAsPDF.aspx";
            }
            else
                ViewBag.Message = "資料已列印請重新選擇!!";


            return View("~/Views/InvoiceProcess/Module/PrintResult.ascx", model: printUrl);
        }


    }
}
