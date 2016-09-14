using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using eIVOGo.Properties;
using Model.InvoiceManagement;
using System.Xml;
using Model.Schema.TXN;
using Uxnet.Com.Security.UseCrypto;
using Model.Schema.EIVO;
using Model.DataEntity;
using System.Threading;
using Model.Helper;
using eIVOGo.Module.Common;
using Utility;
using System.IO;


namespace eIVOGo.Published
{
    /// <summary>
    ///eInvoiceService_Google 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://www.uxb2b.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class eInvoiceService_Google : eInvoiceService
    {

        [WebMethod]
        public override XmlDocument UploadInvoiceAutoTrackNoV2(XmlDocument uploadData)
        {
            Root result = createMessageToken();
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            try
            {
                CryptoUtility crypto = new CryptoUtility();
                if (crypto.VerifyXmlSignature(uploadData))
                {
                    InvoiceRoot invoice = uploadData.TrimAll().ConvertTo<InvoiceRoot>();

                    using (GoogleInvoiceManagerV2 mgr = new GoogleInvoiceManagerV2 { InvoiceClientID = _clientID })
                    {
                        ///憑證資料檢查
                        ///
                        var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();
                        if (token != null)
                        {
                            List<AutomationItem> automation = new List<AutomationItem>();
                            var items = mgr.SaveUploadInvoiceAutoTrackNo(invoice, token);
                            if (items.Count > 0)
                            {
                                result.Response = new RootResponse
                                {
                                    InvoiceNo =
                                    items.Select(d => new RootResponseInvoiceNo
                                    {
                                        Value = invoice.Invoice[d.Key].DataNumber,
                                        Description = d.Value.Message,
                                        ItemIndexSpecified = true,
                                        ItemIndex = d.Key,                                        
                                    }).ToArray()                                    
                                };

                                //失敗Response
                                automation.AddRange(items.Select(d => new AutomationItem
                                {
                                    Description = d.Value.Message,
                                    Status = 0,
                                    Invoice = new AutomationItemInvoice
                                    {
                                        DataNumber = invoice.Invoice[d.Key].DataNumber
                                    }
                                }));

                                
                                ThreadPool.QueueUserWorkItem(ExceptionNotification.SendNotification,
                                    new ExceptionInfo
                                    {
                                        Token = token,
                                        ExceptionItems = items,
                                        InvoiceData = invoice
                                    });
                            }
                            else
                            {
                                result.Result.value = 1;
                            }

                            //成功Response
                            if (mgr.EventItems != null && mgr.EventItems.Count > 0)
                            {
                                automation.AddRange(mgr.EventItems.Select(i => new AutomationItem
                                {
                                    Description = "",
                                    Status = 1,
                                    Invoice = new AutomationItemInvoice
                                    {
                                        InvoiceNumber = i.TrackCode + i.No,
                                        DataNumber = i.InvoicePurchaseOrder.OrderNo,
                                        InvoiceDate = String.Format("{0:yyyy/MM/dd}", i.InvoiceDate),
                                        InvoiceTime = String.Format("{0:HH:mm:ss}", i.InvoiceDate),
                                    }
                                }));
                            }

                            result.Automation = automation.ToArray();

                            if (mgr.HasItem && token.Organization.OrganizationStatus.PrintAll == true)
                            {
                                SharedFunction.SendMailMessage("Google International LLC Taiwan Branch Electronic invoices have been imported, perform invoice print job!!", Settings.Default.WebMaster, token.Organization.CompanyName + "E-mail notification invoicing");
                            }
                        }
                        else
                        {
                            result.Result.message = "Merchant evidence does not match the validation!!";
                        }
                    }
                }
                else
                {
                    result.Result.message = "Signature does not match the invoice data!!";
                }
                GovPlatformFactory.Notify();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result.Result.message = ex.Message;
            }
            return result.ConvertToXml();
        }

        [WebMethod]
        public override XmlDocument UploadInvoiceCancellationV2(XmlDocument uploadData)
        {
            Root result = createMessageToken();
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            try
            {
                CryptoUtility crypto = new CryptoUtility();
                if (crypto.VerifyXmlSignature(uploadData))
                {
                    CancelInvoiceRoot item = uploadData.TrimAll().ConvertTo<CancelInvoiceRoot>();
                    using (GoogleInvoiceManagerV2 mgr = new GoogleInvoiceManagerV2())
                    {
                        ///憑證資料檢查
                        ///
                        var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();
                        if (token != null)
                        {
                            List<AutomationItem> automation = new List<AutomationItem>();
                            var items = mgr.SaveUploadInvoiceCancellation(item, token);
                            if (items.Count > 0)
                            {
                                result.Response = new RootResponse
                                {
                                    InvoiceNo =
                                    items.Select(d => new RootResponseInvoiceNo
                                    {
                                        Value = item.CancelInvoice[d.Key].CancelInvoiceNumber,
                                        Description = d.Value.Message,
                                        ItemIndexSpecified = true,
                                        ItemIndex = d.Key
                                    }).ToArray()
                                };

                                automation.AddRange(items.Select(d => new AutomationItem
                                {
                                    Description = d.Value.Message,
                                    Status = 0,
                                    CancelInvoice = new AutomationItemCancelInvoice
                                   {
                                       CancelInvoiceNumber = item.CancelInvoice[d.Key].CancelInvoiceNumber,
                                   }
                                }));

                                ThreadPool.QueueUserWorkItem(ExceptionNotification.SendNotification,
                                                                    new ExceptionInfo
                                                                    {
                                                                        Token = token,
                                                                        ExceptionItems = items,
                                                                        CancelInvoiceData = item
                                                                    });
                            }
                            else
                            {
                                result.Result.value = 1;
                            }

                            if (mgr.EventItems != null && mgr.EventItems.Count > 0)
                            {
                                automation.AddRange(mgr.EventItems.Select(d => new AutomationItem
                                {
                                    Description = "",
                                    Status = 1,
                                    CancelInvoice = new AutomationItemCancelInvoice
                                    {
                                        CancelInvoiceNumber = d.TrackCode + d.No,
                                    }
                                }));
                            }

                            result.Automation = automation.ToArray();
                        }
                        else
                        {
                            result.Result.message = "Merchant evidence does not match the validation!!";
                        }
                    }
                }
                else
                {
                    result.Result.message = "Signature does not match the invoice data!!";
                }
                GovPlatformFactory.Notify();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result.Result.message = ex.Message;
            }
            return result.ConvertToXml();
        }

        [WebMethod]
        public override XmlDocument UploadAllowanceV2(XmlDocument uploadData)
        {
            Root result = createMessageToken();
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            try
            {
                CryptoUtility crypto = new CryptoUtility();

                if (crypto.VerifyXmlSignature(uploadData))
                {
                    AllowanceRoot allowance = uploadData.TrimAll().ConvertTo<AllowanceRoot>();

                    using (GoogleInvoiceManagerV2 mgr = new GoogleInvoiceManagerV2())
                    {
                        ///憑證資料檢查
                        ///
                        var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();
                        if (token != null)
                        {
                            List<AutomationItem> automation = new List<AutomationItem>();
                            var items = mgr.SaveUploadAllowance(allowance, token);
                            if (items.Count > 0)
                            {
                                result.Response = new RootResponse
                                {
                                    InvoiceNo =
                                    items.Select(d => new RootResponseInvoiceNo
                                    {
                                        Value = allowance.Allowance[d.Key].AllowanceNumber,
                                        Description = d.Value.Message,
                                        ItemIndexSpecified = true,
                                        ItemIndex = d.Key
                                    }).ToArray()
                                };

                                automation.AddRange(items.Select(d => new AutomationItem 
                                {
                                     Description = d.Value.Message,
                                     Status = 0,
                                     Allowance = new AutomationItemAllowance 
                                     {
                                         AllowanceNumber = allowance.Allowance[d.Key].AllowanceNumber,
                                     },
                                }));

                                ThreadPool.QueueUserWorkItem(ExceptionNotification.SendNotification,
                                    new ExceptionInfo
                                    {
                                        Token = token,
                                        ExceptionItems = items,
                                        AllowanceData = allowance
                                    });
                            }
                            else
                            {
                                result.Result.value = 1;
                            }

                            if (mgr.EventItems_Allowance != null && mgr.EventItems_Allowance.Count() > 0)
                            {
                                //上傳後折讓
                                automation.AddRange(mgr.EventItems_Allowance.Select(d => new AutomationItem
                                {
                                    Description = "",
                                    Status = 1,
                                    Allowance = new AutomationItemAllowance
                                    {
                                        AllowanceNumber = d.AllowanceNumber,
                                    },
                                }));
                            }

                            result.Automation = automation.ToArray();
                        }
                        else
                        {
                            result.Result.message = "店家憑證資料驗證不符!!";
                        }
                    }
                }
                else
                {
                    result.Result.message = "發票資料簽章不符!!";
                }
                GovPlatformFactory.Notify();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result.Result.message = ex.Message;
            }
            return result.ConvertToXml();
        }

        [WebMethod]
        public override XmlDocument UploadAllowanceCancellationV2(XmlDocument uploadData)
        {
            Root result = createMessageToken();
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            try
            {
                CryptoUtility crypto = new CryptoUtility();
                if (crypto.VerifyXmlSignature(uploadData))
                {
                    CancelAllowanceRoot item = uploadData.TrimAll().ConvertTo<CancelAllowanceRoot>();
                    using (GoogleInvoiceManagerV2 mgr = new GoogleInvoiceManagerV2())
                    {
                        ///憑證資料檢查
                        ///
                        var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();
                        if (token != null)
                        {
                            List<AutomationItem> automation = new List<AutomationItem>();
                            var items = mgr.SaveUploadAllowanceCancellation(item, token);
                            if (items.Count > 0)
                            {
                                result.Response = new RootResponse
                                {
                                    InvoiceNo =
                                    items.Select(d => new RootResponseInvoiceNo
                                    {
                                        Value = item.CancelAllowance[d.Key].CancelAllowanceNumber,
                                        Description = d.Value.Message,
                                        ItemIndexSpecified = true,
                                        ItemIndex = d.Key
                                    }).ToArray()
                                };

                                automation.AddRange(items.Select(d => new AutomationItem
                                {
                                    Description = d.Value.Message,
                                    Status = 0,
                                    CancelAllowance = new AutomationItemCancelAllowance
                                    {
                                        CancelAllowanceNumber = item.CancelAllowance[d.Key].CancelAllowanceNumber,
                                    },
                                }));

                                ThreadPool.QueueUserWorkItem(ExceptionNotification.SendNotification,
                                                                    new ExceptionInfo
                                                                    {
                                                                        Token = token,
                                                                        ExceptionItems = items,
                                                                        CancelAllowanceData = item
                                                                    });
                            }
                            else
                            {
                                result.Result.value = 1;
                            }

                            if (mgr.EventItems_Allowance != null && mgr.EventItems_Allowance.Count() > 0)
                            {
                                //上傳後折讓
                                automation.AddRange(mgr.EventItems_Allowance.Select(d => new AutomationItem
                                {
                                    Description = "",
                                    Status = 1,
                                    CancelAllowance = new AutomationItemCancelAllowance
                                   {
                                       CancelAllowanceNumber = d.AllowanceNumber,
                                   },
                                }));
                            }

                            result.Automation = automation.ToArray();
                        }
                        else
                        {
                            result.Result.message = "Merchant evidence does not match the validation!!";
                        }
                    }
                }
                else
                {
                    result.Result.message = "Signature does not match the invoice data!!";
                }
                GovPlatformFactory.Notify();
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
