using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Services;
using System.Xml;

using Model.InvoiceManagement;
using Model.Schema.EIVO;
using Model.Schema.TXN;
using eIVOGo.Properties;
using Utility;
using Uxnet.Com.Security.UseCrypto;
using System.Text;
using Model.DataEntity;
using System.Threading;
using Model.Helper;

namespace eIVOGo.Published
{
    /// <summary>
    /// Summary description for eInvoiceService
    /// </summary>
    [WebService(Namespace = "http://www.uxb2b.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class eInvoiceServiceTest : eInvoiceService
    {

        [WebMethod]
        public XmlDocument GetIncomingInvoicesBySeller(String receiptNo)
        {
            RootA0101 result = new RootA0101
            {
                UXB2B = "電子發票系統",
                Result = new RootResult
                {
                    timeStamp = DateTime.Now,
                    value = 0
                }
            }; 

            try
            {
                    using (InvoiceManager mgr = new InvoiceManager())
                    {
                        ///憑證資料檢查
                        ///
                        var seller = mgr.GetTable<Organization>().Where(t => t.ReceiptNo == receiptNo).FirstOrDefault();
                        if (seller != null)
                        {
                            buildIncomingInvoices(result, mgr, seller.CompanyID);
                        }
                        else
                        {
                            result.Result.message = "店家憑證資料驗證不符!!";
                        }
                    }
                

                //GovPlatformFactory.Notify();

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result.Result.message = ex.Message;
            }
            return result.ConvertToXml();
        }

        [WebMethod]
        public XmlDocument GetIncomingInvoiceCancellationsBySeller(String receiptNo)
        {
            RootA0201 result = new RootA0201
            {
                UXB2B = "電子發票系統",
                Result = new RootResult
                {
                    timeStamp = DateTime.Now,
                    value = 0
                }
            }; 

            try
            {
                using (InvoiceManager mgr = new InvoiceManager())
                {
                    ///憑證資料檢查
                    ///
                    var seller = mgr.GetTable<Organization>().Where(t => t.ReceiptNo == receiptNo).FirstOrDefault();
                    if (seller != null)
                    {
                        buildIncomingInvoiceCancellations(result, mgr, seller.CompanyID);
                    }
                    else
                    {
                        result.Result.message = "店家憑證資料驗證不符!!";
                    }
                }


                //GovPlatformFactory.Notify();

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result.Result.message = ex.Message;
            }
            return result.ConvertToXml();
        }


    }
}
