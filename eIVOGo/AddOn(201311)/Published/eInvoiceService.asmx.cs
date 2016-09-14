using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Xml;

using eIVOGo.Module.Common;
using eIVOGo.Properties;
using Model.DataEntity;
using Model.Helper;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Schema.EIVO;
using Model.Schema.TurnKey;
using Model.Schema.TXN;
using Utility;
using Uxnet.Com.Security.UseCrypto;
using System.IO;
using System.Data;

namespace eIVOGo.Published
{

    public partial class eInvoiceService
    {

        static partial void AddOn()
        {
            EIVOPlatformFactory.DefaultUserCarrierType = Settings.Default.DefaultUserCarrierType;
            InvoiceEnterpriseManager.CreateMatchedDefaultUser = (item) =>
            {
                ThreadPool.QueueUserWorkItem(stateInfo =>
                {
                    using (EIVOEntityManager<Organization> mgr = new EIVOEntityManager<Organization>())
                    {
                        var orgItem = mgr.EntityList.Where(o => o.CompanyID == item.CompanyID).FirstOrDefault();
                        if (orgItem == null || orgItem.OrganizationCategory.Count == 0)
                            return;

                        var orgaCate = orgItem.OrganizationCategory.First();

                        var userProfile = new UserProfile
                        {
                            PID = orgItem.ReceiptNo,
                            Phone = orgItem.Phone,
                            EMail = orgItem.ContactEmail,
                            Address = orgItem.Addr,
                            UserProfileExtension = new UserProfileExtension
                            {
                                IDNo = orgItem.ReceiptNo
                            },
                            UserProfileStatus = new UserProfileStatus
                            {
                                CurrentLevel = (int)Naming.MemberStatusDefinition.Wait_For_Check
                            }
                        };

                        mgr.GetTable<UserRole>().InsertOnSubmit(new UserRole
                        {
                            RoleID = (int)Naming.RoleID.ROLE_SELLER,
                            UserProfile = userProfile,
                            OrganizationCategory = orgaCate
                        });

                        mgr.SubmitChanges();

                        try
                        {
                            //String.Format("{0}{1}?id={2}", eIVOGo.Properties.Settings.Default.mailLinkAddress, VirtualPathUtility.ToAbsolute(Settings.Default.NotifyActivation), userProfile.UID)
                            //    .MailWebPage(userProfile.EMail, "電子發票系統 會員啟用認證信");
                            String.Format("{0}{1}?id={2}", eIVOGo.Properties.Settings.Default.mailLinkAddress, VirtualPathUtility.ToAbsolute(Settings.Default.NotifyActivation), userProfile.UID)
                                .MailWebPage(Settings.Default.WebMaster, "電子發票系統 會員啟用認證信");
                        }
                        catch (Exception ex)
                        {
                            Logger.Warn("［電子發票系統 會員啟用認證信］傳送失敗,原因 => " + ex.Message);
                            Logger.Error(ex);
                        }
                    }

                });
            };

            InvoiceNotification.NotifyCancellingInvoice = (items) =>
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    using (InvoiceManagerV2 mgr = new InvoiceManagerV2())
                    {
                        var cipher = new com.uxb2b.util.CipherDecipherSrv(16);
                        String url = String.Format("{0}{1}", Settings.Default.mailLinkAddress, VirtualPathUtility.ToAbsolute("~/Published/B2CInvoiceCancelMailPage.aspx"));

                        foreach (var invoiceID in items)
                        {
                            try
                            {

                                var item = mgr.GetTable<InvoiceItem>().Where(i => i.InvoiceID == invoiceID
                                    && i.InvoiceCancellation != null).FirstOrDefault();

                                if (item == null || String.IsNullOrEmpty(item.InvoiceBuyer.EMail) || item.Organization.OrganizationStatus.DisableIssuingNotice == true)
                                {
                                    continue;
                                }

                                String.Format("{0}?{1}", url, cipher.cipher(invoiceID.ToString()))
                                    .MailWebPage(item.InvoiceBuyer.EMail, String.Format("{0}作廢電子發票開立郵件通知", item.Organization.CompanyName));

                            }
                            catch (Exception ex)
                            {
                                Logger.Warn(String.Format("作廢電子發票郵件通知客戶傳送失敗,ID:{0}", invoiceID));
                                Logger.Error(ex);
                            }
                        }
                    }
                });
            };

        }

        [WebMethod]
        public virtual XmlDocument UploadInvoiceAutoTrackNoByClient(XmlDocument uploadData,String clientID)
        {
            _clientID = clientID;
            return UploadInvoiceAutoTrackNoV2(uploadData);
        }

        [WebMethod]
        public virtual XmlDocument UploadInvoiceAutoTrackNoV2(XmlDocument uploadData)
        {
            Root result = createMessageToken();

            try
            {
                CryptoUtility crypto = new CryptoUtility();
                if (crypto.VerifyXmlSignature(uploadData))
                {
                    InvoiceRoot invoice = uploadData.TrimAll().ConvertTo<InvoiceRoot>();
                    using (InvoiceManagerV3 mgr = new InvoiceManagerV3 { InvoiceClientID = _clientID })
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
                                        ItemIndex = d.Key
                                    }).ToArray()
                                };

                                //失敗Response
                                automation.AddRange(items.Select(d => new AutomationItem
                                {
                                    Description = d.Value.Message,
                                    Status = 0,
                                    Invoice = new AutomationItemInvoice
                                    {
                                        DataNumber = invoice.Invoice[d.Key].DataNumber,
                                        SellerId = invoice.Invoice[d.Key].SellerId
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
                                        SellerId = i.InvoiceSeller.ReceiptNo,
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
                                SharedFunction.SendMailMessage(token.Organization.CompanyName + "電子發票已匯入,請執行發票列印作業!!", Settings.Default.WebMaster, token.Organization.CompanyName + "電子發票開立郵件通知");
                            }
                        }
                        else
                        {
                            result.Result.message = "營業人憑證資料驗證不符!!";
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
        public virtual XmlDocument UploadInvoiceCancellationV2(XmlDocument uploadData)
        {
            Root result = createMessageToken();

            try
            {
                CryptoUtility crypto = new CryptoUtility();
                if (crypto.VerifyXmlSignature(uploadData))
                {
                    CancelInvoiceRoot item = uploadData.TrimAll().ConvertTo<CancelInvoiceRoot>();
                    using (InvoiceManagerV3 mgr = new InvoiceManagerV3())
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
                                       SellerId = item.CancelInvoice[d.Key].SellerId
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
                                        SellerId = d.InvoiceSeller.ReceiptNo
                                    }
                                }));
                            }

                            result.Automation = automation.ToArray();
                        }
                        else
                        {
                            result.Result.message = "營業人憑證資料驗證不符!!";
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
        public virtual XmlDocument UploadAllowanceV2(XmlDocument uploadData)
        {
            Root result = createMessageToken();
            try
            {
                CryptoUtility crypto = new CryptoUtility();

                if (crypto.VerifyXmlSignature(uploadData))
                {
                    AllowanceRoot allowance = uploadData.TrimAll().ConvertTo<AllowanceRoot>();

                    using (InvoiceManagerV3 mgr = new InvoiceManagerV3())
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
                                         SellerId = allowance.Allowance[d.Key].SellerId,
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
                                        SellerId = d.InvoiceAllowanceSeller.ReceiptNo
                                    },
                                }));
                            }

                            result.Automation = automation.ToArray();
                        }
                        else
                        {
                            result.Result.message = "營業人憑證資料驗證不符!!";
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
        public virtual XmlDocument UploadAllowanceCancellationV2(XmlDocument uploadData)
        {
            Root result = createMessageToken();

            try
            {
                CryptoUtility crypto = new CryptoUtility();
                if (crypto.VerifyXmlSignature(uploadData))
                {
                    CancelAllowanceRoot item = uploadData.TrimAll().ConvertTo<CancelAllowanceRoot>();
                    using (InvoiceManagerV3 mgr = new InvoiceManagerV3())
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
                                        SellerId = item.CancelAllowance[d.Key].SellerId,
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
                                       SellerId = d.InvoiceAllowanceSeller.ReceiptNo
                                   },
                                }));
                            }

                            result.Automation = automation.ToArray();
                        }
                        else
                        {
                            result.Result.message = "營業人憑證資料驗證不符!!";
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
        public virtual XmlDocument UploadInvoiceV2(XmlDocument uploadData)
        {
            Root result = createMessageToken();
            try
            {
                CryptoUtility crypto = new CryptoUtility();
                if (crypto.VerifyXmlSignature(uploadData))
                {
                    InvoiceRoot invoice = uploadData.TrimAll().ConvertTo<InvoiceRoot>();

                    using (InvoiceManagerV3 mgr = new InvoiceManagerV3())
                    {
                        ///憑證資料檢查
                        ///
                        var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();
                        if (token != null)
                        {
                            List<AutomationItem> automation = new List<AutomationItem>();
                            var items = mgr.SaveUploadInvoice(invoice, token);

                            if (items.Count > 0)
                            {
                                result.Response = new RootResponse
                                {
                                    InvoiceNo =
                                    items.Select(d => new RootResponseInvoiceNo
                                    {
                                        Value = invoice.Invoice[d.Key].InvoiceNumber,
                                        Description = d.Value.Message,
                                        ItemIndexSpecified = true,
                                        ItemIndex = d.Key
                                    }).ToArray()
                                };

                                //失敗Response
                                automation.AddRange(items.Select(d => new AutomationItem
                                {
                                    Description = d.Value.Message,
                                    Status = 0,
                                    Invoice = new AutomationItemInvoice
                                    {
                                        InvoiceNumber = invoice.Invoice[d.Key].InvoiceNumber,
                                        SellerId = invoice.Invoice[d.Key].SellerId
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
                                        SellerId = i.InvoiceSeller.ReceiptNo,
                                        InvoiceNumber = i.TrackCode + i.No
                                    }
                                }));
                            }

                            result.Automation = automation.ToArray();

                            if (mgr.HasItem && token.Organization.OrganizationStatus.PrintAll == true)
                            {
                                SharedFunction.SendMailMessage(token.Organization.CompanyName + "電子發票已匯入,請執行發票列印作業!!", Settings.Default.WebMaster, token.Organization.CompanyName + "電子發票開立郵件通知");
                            }
                        }
                        else
                        {
                            result.Result.message = "營業人憑證資料驗證不符!!";
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
        public virtual XmlDocument UploadInvoiceCmsCSVAutoTrackNoV2(byte[] uploadData)
        {
            Root result = createMessageToken();

            try
            {
                CryptoUtility crypto = new CryptoUtility();
                byte[] dataToSign;
                if (crypto.VerifyEnvelopedPKCS7(uploadData, out dataToSign))
                {
                    String fileName = Path.Combine(Logger.LogDailyPath, String.Format("Invoice_{0}.csv", Guid.NewGuid()));
                    using (FileStream fs = new FileStream(fileName, FileMode.Create))
                    {
                        fs.Write(dataToSign, 0, dataToSign.Length);
                        fs.Flush();
                        fs.Close();
                    }

                    using (InvoiceManagerV3 mgr = new InvoiceManagerV3())
                    {
                        ///憑證資料檢查
                        ///
                        var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();
                        if (token != null)
                        {
                            CsvInvoiceUploadManagerV2 csvMgr = new CsvInvoiceUploadManagerV2(mgr, token.CompanyID);
                            csvMgr.ParseData(null, fileName, Encoding.GetEncoding(Settings.Default.CsvUploadEncoding));
                            if (!csvMgr.Save())
                            {
                                var items = csvMgr.ErrorList;
                                result.Response = new RootResponse
                                {
                                    InvoiceNo =
                                    items.Select(d => new RootResponseInvoiceNo
                                    {
                                        Value = d.DataContent,
                                        Description = d.Status,
                                        ItemIndexSpecified = true,
                                        ItemIndex = csvMgr.ItemList.IndexOf(d)
                                    }).ToArray()
                                };

                                ThreadPool.QueueUserWorkItem(ExceptionNotification.SendNotification,
                                    new ExceptionInfo
                                    {
                                        Token = token,
                                        InvoiceError = items
                                    });
                            }
                            else
                            {
                                result.Result.value = 1;
                                if (token.Organization.OrganizationStatus.PrintAll == true)
                                {
                                    SharedFunction.SendMailMessage(token.Organization.CompanyName + "電子發票已匯入,請執行發票列印作業!!", Settings.Default.WebMaster, token.Organization.CompanyName + "電子發票開立郵件通知");
                                }
                            }

                        }
                        else
                        {
                            result.Result.message = "營業人憑證資料驗證不符!!";
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

        #region B2B寄送發票(CSV)

        public DataTable TxtConvertToDataTable(string File, string TableName, string delimiter)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            StreamReader s = new StreamReader(File, System.Text.Encoding.Default);
            //string ss = s.ReadLine();//skip the first line
            string[] columns = s.ReadLine().Split(delimiter.ToCharArray());
            ds.Tables.Add(TableName);
            foreach (string col in columns)
            {
                bool added = false;
                string next = "";
                int i = 0;
                while (!added)
                {
                    string columnname = col + next;
                    columnname = columnname.Replace("#", "");
                    columnname = columnname.Replace("'", "");
                    columnname = columnname.Replace("&", "");

                    if (!ds.Tables[TableName].Columns.Contains(columnname))
                    {
                        ds.Tables[TableName].Columns.Add(columnname.ToUpper());
                        added = true;
                    }
                    else
                    {
                        i++;
                        next = "_" + i.ToString();
                    }
                }
            }

            string AllData = s.ReadToEnd();
            string[] rows = AllData.Split("\n".ToCharArray());

            foreach (string r in rows)
            {
                string[] items = r.Split(delimiter.ToCharArray());
                ds.Tables[TableName].Rows.Add(items);
            }

            s.Close();
            dt = ds.Tables[0];

            return dt;
        }

        #endregion B2B寄送發票(CSV)

        [WebMethod]
        public virtual XmlDocument UploadBranchTrack(XmlDocument uploadData)
        {
            Root result = createMessageToken();
            try
            {
                 CryptoUtility crypto = new CryptoUtility();
                 if (crypto.VerifyXmlSignature(uploadData))
                 {
                     BranchTrack interval = uploadData.TrimAll().ConvertTo<BranchTrack>();

                     using (TrackNoIntervalManager mgr = new TrackNoIntervalManager())
                     {
                         ///憑證資料檢查
                         ///
                         var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();
                         if (token != null)
                         {
                             List<AutomationItem> automation = new List<AutomationItem>();
                             var items = mgr.SaveUploadBranchTrackInterval(interval, token);

                             if (items.Count > 0)
                             {
                                 result.Response = new RootResponse
                                 {
                                     InvoiceNo =
                                     items.Select(d => new RootResponseInvoiceNo
                                     {
                                         Value = interval.Main[d.Key].SellerId,
                                         Description = d.Value.Message,
                                         ItemIndexSpecified = true,
                                         ItemIndex = d.Key
                                     }).ToArray()
                                 };

                                 //失敗Response
                                 automation.AddRange(items.Select(d => new AutomationItem
                                 {
                                     Description = d.Value.Message,
                                     Status = 0,
                                     BranchTrack = new AutomationItemBranchTrack
                                     {
                                         InvoiceBeginNo = interval.Main[d.Key].InvoiceBeginNo,
                                         InvoiceEndNo = interval.Main[d.Key].InvoiceEndNo,
                                         PeriodNo = interval.Main[d.Key].PeriodNo,
                                         TrackCode = interval.Main[d.Key].TrackCode,
                                         Year = interval.Main[d.Key].Year,
                                         SellerId = interval.Main[d.Key].SellerId
                                     }
                                 }));

                                 //ThreadPool.QueueUserWorkItem(ExceptionNotification.SendNotification,
                                 //    new ExceptionInfo
                                 //    {
                                 //        Token = token,
                                 //        ExceptionItems = items,
                                 //        InvoiceData = interval
                                 //    });
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
                                     BranchTrack = new AutomationItemBranchTrack
                                     {
                                         InvoiceBeginNo = String.Format("{0:00000000}",i.StartNo),
                                         InvoiceEndNo = String.Format("{0:00000000}",i.EndNo),
                                         PeriodNo = String.Format("{0:00}",i.InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo),
                                         TrackCode = i.InvoiceTrackCodeAssignment.InvoiceTrackCode.TrackCode,
                                         Year = i.InvoiceTrackCodeAssignment.InvoiceTrackCode.Year,
                                         SellerId = i.InvoiceTrackCodeAssignment.Organization.ReceiptNo
                                     }
                                 }));
                             }

                             result.Automation = automation.ToArray();

                         }
                         else
                         {
                             result.Result.message = "營業人憑證資料驗證不符!!";
                         }
                     }
                 }
                 else
                 {
                     result.Result.message = "資料簽章不符!!";
                 }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result.Result.message = ex.Message;
            }
            return result.ConvertToXml();
        }


        [WebMethod]
        public virtual XmlDocument UploadInvoiceV2_C0401(XmlDocument uploadData)
        {
            Root result = createMessageToken();
            try
            {
                CryptoUtility crypto = new CryptoUtility();
                if (crypto.VerifyXmlSignature(uploadData))
                {
                    Model.Schema.MIG3_1.C0401.Invoice invoice = uploadData.TrimAll().ConvertTo<Model.Schema.MIG3_1.C0401.Invoice>();

                    using (MIGInvoiceManager mgr = new MIGInvoiceManager())
                    {
                        ///憑證資料檢查
                        ///
                        var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();

                        if (token != null)
                        {
                            var ex = mgr.SaveUploadInvoice(invoice, token);

                            if (ex != null)
                            {

                                Dictionary<int, Exception> items = new Dictionary<int, Exception>();
                                items.Add(0, ex);

                                result.Response = new RootResponse
                                {
                                    InvoiceNo =
                                    items.Select(d => new RootResponseInvoiceNo
                                    {
                                        Value = invoice.Main.InvoiceNumber,
                                        Description = d.Value.Message,
                                        ItemIndexSpecified = true,
                                        ItemIndex = d.Key
                                    }).ToArray()
                                };

                                ThreadPool.QueueUserWorkItem(ExceptionNotification.SendNotification,
                                    new ExceptionInfo
                                    {
                                        Token = token,
                                        ExceptionItems = items,
                                        InvoiceData_C0401 = invoice
                                    });

                            }
                            else
                            {
                                result.Result.value = 1;
                            }

                            if (mgr.HasItem && token.Organization.OrganizationStatus.PrintAll == true)
                            {
                                SharedFunction.SendMailMessage(token.Organization.CompanyName + "電子發票已匯入,請執行發票列印作業!!", Settings.Default.WebMaster, token.Organization.CompanyName + "電子發票開立郵件通知");
                            }
                        }
                        else
                        {
                            result.Result.message = "營業人憑證資料驗證不符!!";
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
        public virtual XmlDocument UploadInvoiceCancellationV2_C0501(XmlDocument uploadData)
        {
            Root result = createMessageToken();

            try
            {
                CryptoUtility crypto = new CryptoUtility();
                if (crypto.VerifyXmlSignature(uploadData))
                {
                    Model.Schema.MIG3_1.C0501.CancelInvoice item = uploadData.TrimAll().ConvertTo<Model.Schema.MIG3_1.C0501.CancelInvoice>();

                    using (InvoiceManagerV3 mgr = new InvoiceManagerV3())
                    {
                        ///憑證資料檢查
                        ///
                        var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();
                        if (token != null)
                        {
                            var items = mgr.SaveUploadInvoiceCancellation_C0501(item, token);
                            if (items.Count > 0)
                            {
                                result.Response = new RootResponse
                                {
                                    InvoiceNo =
                                    items.Select(d => new RootResponseInvoiceNo
                                    {
                                        Value = item.CancelInvoiceNumber,
                                        Description = d.Value.Message,
                                        ItemIndexSpecified = true,
                                        ItemIndex = d.Key
                                    }).ToArray()
                                };

                                ThreadPool.QueueUserWorkItem(ExceptionNotification.SendNotification,
                                                                    new ExceptionInfo
                                                                    {
                                                                        Token = token,
                                                                        ExceptionItems = items,
                                                                        CancelInvoiceData_C0501 = item
                                                                    });
                            }
                            else
                            {
                                result.Result.value = 1;
                            }
                        }
                        else
                        {
                            result.Result.message = "營業人憑證資料驗證不符!!";
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
        public virtual XmlDocument UploadInvoiceEnterprise(XmlDocument uploadData)
        {
            Root result = createMessageToken();
            try
            {
                CryptoUtility crypto = new CryptoUtility();
                if (crypto.VerifyXmlSignature(uploadData))
                {
                    InvoiceEnterpriseRoot enterprise = uploadData.TrimAll().ConvertTo<InvoiceEnterpriseRoot>();

                    using (InvoiceEnterpriseManager mgr = new InvoiceEnterpriseManager())
                    {
                        ///憑證資料檢查
                        ///
                        var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();
                        if (token != null)
                        {
                            if (token.Organization.OrganizationCategory.Any(c => c.CategoryID == (int)Naming.CategoryID.COMP_INVOICE_AGENT))
                            {
                                List<AutomationItem> automation = new List<AutomationItem>();
                                var items = mgr.SaveInvoiceEnterprise(enterprise, token);

                                if (items.Count > 0)
                                {
                                    result.Response = new RootResponse
                                    {
                                        InvoiceNo =
                                        items.Select(d => new RootResponseInvoiceNo
                                        {
                                            Value = enterprise.InvoiceEnterprise[d.Key].SellerId,
                                            Description = d.Value.Message,
                                            ItemIndexSpecified = true,
                                            ItemIndex = d.Key
                                        }).ToArray()
                                    };

                                    //失敗Response
                                    automation.AddRange(items.Select(d => new AutomationItem
                                    {
                                        Description = d.Value.Message,
                                        Status = 0,
                                        InvoiceEnterprise = new AutomationItemInvoiceEnterprise
                                        {
                                            SellerId = enterprise.InvoiceEnterprise[d.Key].SellerId,
                                            SellerName = enterprise.InvoiceEnterprise[d.Key].SellerName,
                                            Address = enterprise.InvoiceEnterprise[d.Key].Address,
                                            ContactMobilePhone = enterprise.InvoiceEnterprise[d.Key].ContactMobilePhone,
                                            ContactName = enterprise.InvoiceEnterprise[d.Key].ContactName,
                                            ContactPhone = enterprise.InvoiceEnterprise[d.Key].ContactPhone,
                                            Email = enterprise.InvoiceEnterprise[d.Key].Email,
                                            InvoiceType = enterprise.InvoiceEnterprise[d.Key].InvoiceType,
                                            TEL = enterprise.InvoiceEnterprise[d.Key].TEL,
                                            UndertakerName = enterprise.InvoiceEnterprise[d.Key].UndertakerName
                                        }
                                    }));
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
                                        InvoiceEnterprise = new AutomationItemInvoiceEnterprise
                                        {
                                            SellerId = i.ReceiptNo,
                                            SellerName = i.CompanyName
                                        }
                                    }));
                                }

                                result.Automation = automation.ToArray();
                            }
                            else
                            {
                                result.Result.message = "營業人尚未核准代理傳送發票!!";
                            }
                        }
                        else
                        {
                            result.Result.message = "營業人憑證資料驗證不符!!";
                        }
                    }
                }
                else
                {
                    result.Result.message = "資料簽章不符!!";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result.Result.message = ex.Message;
            }
            return result.ConvertToXml();
        }

        [WebMethod]
        public virtual XmlDocument GetCurrentYearInvoiceTrackCode(XmlDocument sellerInfo)
        {
            RootInvoiceTrackCode result = new RootInvoiceTrackCode
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
                CryptoUtility crypto = new CryptoUtility();
                if (crypto.VerifyXmlSignature(sellerInfo))
                {
                    using (InvoiceManager mgr = new InvoiceManager())
                    {
                        ///憑證資料檢查
                        ///
                        var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();
                        if (token != null )
                        {
                            var items = mgr.GetTable<InvoiceTrackCode>().Where(t => t.Year == DateTime.Today.Year)
                                .OrderBy(t => t.PeriodNo);

                            if (items.Count() > 0)
                            {
                                result.Response = new RootResponseForInvoiceTrackCode
                                {
                                    InvoiceTrackCodeRoot = new InvoiceTrackCodeRoot
                                    {
                                        InvoiceTrackCode = items.Select(d => new InvoiceTrackCodeRootInvoiceTrackCode
                                        {
                                            Year = d.Year,
                                            PeriodNo = String.Format("{0:00}", d.PeriodNo),
                                            TrackCode = d.TrackCode
                                        }).ToArray()
                                    }
                                };

                                result.Result.value = 1;
                            }

                            Root root = sellerInfo.ConvertTo<Root>();
                            acknowledgeReport(mgr, token, root.Request.periodicalIntervalSpecified ? root.Request.periodicalInterval : (int?)null);
                        }
                        else
                        {
                            result.Result.message = "營業人憑證資料驗證不符!!";
                        }
                    }
                }
                else
                {
                    result.Result.message = "發票資料簽章不符!!";
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result.Result.message = ex.Message;
            }
            return result.ConvertToXml();
        }

        [WebMethod]
        public virtual String[] GetCustomerIdListByAgent(XmlDocument sellerInfo)
        {
            try
            {
                CryptoUtility crypto = new CryptoUtility();
                if (crypto.VerifyXmlSignature(sellerInfo))
                {
                    using (InvoiceManager mgr = new InvoiceManager())
                    {
                        ///憑證資料檢查
                        ///
                        var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();
                        if (token != null)
                        {
                            return mgr.GetQueryByAgent(token.CompanyID)
                                .Select(o=>o.ReceiptNo).ToArray();

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        [WebMethod]
        public virtual XmlDocument GetVacantInvoiceNo(XmlDocument sellerInfo,String receiptNo)
        {
            try
            {
                CryptoUtility crypto = new CryptoUtility();
                if (crypto.VerifyXmlSignature(sellerInfo))
                {
                    using (InvoiceManager mgr = new InvoiceManager())
                    {
                        ///憑證資料檢查
                        ///
                        var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();
                        if (token != null)
                        {
                            var item = mgr.GetQueryByAgent(token.CompanyID)
                                .Where(o => o.ReceiptNo == receiptNo).FirstOrDefault();
                            if (item != null)
                            {
                                var queryDate = DateTime.Today.AddMonths(-2);
                                var queryPeriod = queryDate.Month / 2 + queryDate.Month % 2;
                                var items = mgr.GetTable<InvoiceTrackCode>().Where(t => t.Year == queryDate.Year && t.PeriodNo == queryPeriod)
                                    .Join(mgr.GetTable<InvoiceTrackCodeAssignment>().Where(n => n.SellerID == item.CompanyID),
                                        t => t.TrackID, n => n.TrackID, (t, n) => n)
                                        .Where(n => n.UnassignedInvoiceNo.Count > 0)
                                        .GroupBy(n => n.TrackID);
                                if (items.Count() > 0)
                                {
                                    List<Model.Schema.MIG3_1.E0402.BranchTrackBlank> result = new List<Model.Schema.MIG3_1.E0402.BranchTrackBlank>();
                                    foreach (var g in items)
                                    {
                                        Model.Schema.MIG3_1.E0402.BranchTrackBlank vacantItem = new Model.Schema.MIG3_1.E0402.BranchTrackBlank
                                        {
                                            Main = new Model.Schema.MIG3_1.E0402.Main
                                            {
                                                HeadBan = item.ReceiptNo,
                                                BranchBan = item.ReceiptNo,
                                                InvoiceType = (Model.Schema.MIG3_1.E0402.InvoiceTypeEnum)item.OrganizationStatus.SettingInvoiceType.Value,
                                                YearMonth = String.Format("{0:000}{1:00}", queryDate.Year - 1911, queryPeriod * 2),
                                                InvoiceTrack = g.First().InvoiceTrackCode.TrackCode
                                            },
                                            Details = g.Join(mgr.GetTable<UnassignedInvoiceNo>(), n => new { n.TrackID, n.SellerID }, u => new { u.TrackID, u.SellerID }, (n, u) => u)
                                                .Select(u => new Model.Schema.MIG3_1.E0402.DetailsBranchTrackBlankItem
                                                {
                                                    InvoiceBeginNo = String.Format("{0:00000000}", u.InvoiceBeginNo),
                                                    InvoiceEndNo = String.Format("{0:00000000}", u.InvoiceEndNo)
                                                }).ToArray()
                                        };

                                        result.Add(vacantItem);
                                    }

                                    return result.ConvertToXml();

                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }


    }
}