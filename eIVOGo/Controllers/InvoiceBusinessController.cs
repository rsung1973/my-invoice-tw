using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using Business.Helper;
using ClosedXML.Excel;
using DataAccessLayer.basis;
using eIVOGo.Helper;
using eIVOGo.Models;
using eIVOGo.Models.ViewModel;
using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Resource;
using Model.Security.MembershipManagement;
using Utility;

namespace eIVOGo.Controllers
{
    public class InvoiceBusinessController : SampleController<InvoiceItem>
    {
        // GET: InvoiceBusiness
        public ActionResult ApplyPOSDevice(int? id)
        {
            var item = models.GetTable<Organization>().Where(o => o.CompanyID == id).FirstOrDefault();
            if (item == null)
                return Content("營業人資料錯誤!!");

            return View("POSDeviceList", item);
        }

        public ActionResult CommitPOS(int? id, int? deviceID, String POSNo)
        {
            var orgItem = models.GetTable<Organization>().Where(o => o.CompanyID == id).FirstOrDefault();
            if (orgItem == null)
                return View("~/Views/SiteAction/Alert.ascx", model: "營業人資料錯誤!!");

            POSNo = POSNo.GetEfficientString();
            if (POSNo == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "POS機編號錯誤!!");
            }

            if (orgItem.POSDevice.Any(p => p.POSNo == POSNo && p.DeviceID != deviceID))
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "已存在相同的POS機編號!!");
            }

            var item = models.GetTable<POSDevice>().Where(p => p.CompanyID == id && p.DeviceID == deviceID).FirstOrDefault();
            if (item == null)
            {
                item = new POSDevice
                {
                    CompanyID = orgItem.CompanyID
                };
                orgItem.POSDevice.Add(item);
            }
            item.POSNo = POSNo;

            models.SubmitChanges();

            return View("~/Views/InvoiceBusiness/POSDevice/DataItem.ascx", item);

        }

        public ActionResult DeletePOS(int? id, int deviceID)
        {
            var item = models.DeleteAny<POSDevice>(d => d.CompanyID == id && d.DeviceID == deviceID);

            if (item == null)
            {
                return Json(new { result = false, message = "POS機編號錯誤!!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult EditPOS(int? id, int deviceID)
        {
            var item = models.GetTable<POSDevice>().Where(d => d.CompanyID == id && d.DeviceID == deviceID).FirstOrDefault();

            if (item == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "POS機編號錯誤!!");
            }

            return View("~/Views/InvoiceBusiness/POSDevice/EditItem.ascx", item);

        }

        public ActionResult DataItem(int? id, int deviceID)
        {
            var item = models.GetTable<POSDevice>().Where(d => d.CompanyID == id && d.DeviceID == deviceID).FirstOrDefault();

            if (item == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "POS機編號錯誤!!");
            }

            return View("~/Views/InvoiceBusiness/POSDevice/DataItem.ascx", item);
        }

        public ActionResult GenerateGUID()
        {
            return Content(Guid.NewGuid().ToString());
        }

        public ActionResult CreateInvoice()
        {
            ViewBag.ViewModel = new InvoiceViewModel { };
            return View();
        }

        public ActionResult PreviewInvoice(InvoiceViewModel viewModel)
        {
            ViewBag.ViewModel = viewModel;

            var seller = models.GetTable<Organization>().Where(o => o.CompanyID == viewModel.SellerID).FirstOrDefault();
            if (seller == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "發票開立人錯誤!!");
            }

            viewModel.SellerName = seller.CompanyName;
            viewModel.SellerReceiptNo = seller.ReceiptNo;

            using (TrackNoManager mgr = new TrackNoManager(new GenericManager<EIVOEntityDataContext>(models.GetDataContext()), seller.CompanyID))
            {
                if (!mgr.ApplyInvoiceDate(viewModel.InvoiceDate.Value))
                {
                    return View("~/Views/SiteAction/Alert.ascx", model: String.Format(MessageResources.AlertNullTrackNoInterval, seller.ReceiptNo));
                }

                viewModel.TrackCode = mgr.InvoiceNoInterval.InvoiceTrackCodeAssignment.InvoiceTrackCode.TrackCode;
                viewModel.No = String.Format("{0:00000000}", mgr.PeekInvoiceNo());
            }

            return View("PrintInvoice");
        }

        public ActionResult CommitInvoice(InvoiceViewModel viewModel)
        {
            ViewBag.ViewModel = viewModel;

            var seller = models.GetTable<Organization>().Where(o => o.CompanyID == viewModel.SellerID).FirstOrDefault();
            if (seller == null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "發票開立人錯誤!!");
            }

            viewModel.SellerName = seller.CompanyName;
            viewModel.SellerReceiptNo = seller.ReceiptNo;

            InvoiceViewModelValidator<InvoiceItem> validator = new InvoiceViewModelValidator<InvoiceItem>(this.DataSource, seller);
            var exception = validator.Validate(viewModel);
            if (exception!=null)
            {
                return View("~/Views/SiteAction/Alert.ascx", model: exception.Message);
            }

            InvoiceItem newItem = validator.InvoiceItem;
            models.GetTable<InvoiceItem>().InsertOnSubmit(newItem);
            models.SubmitChanges();

            viewModel.TrackCode = newItem.TrackCode;
            viewModel.No = newItem.No;

            return View("PrintInvoice",newItem);

        }

        public ActionResult EncryptContent(String content,String key)
        {
            com.tradevan.qrutil.QREncrypter qrencrypter = new com.tradevan.qrutil.QREncrypter();
            return Content(qrencrypter.AESEncrypt(content, key));
        }

    }

}