using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Schema.TXN;
using Utility;
using Uxnet.Com.Security.UseCrypto;
using Model.Helper;
using Model.Locale;
using Ionic.Zip;
using System.Threading;

namespace eIVOGo.Published
{
    /// <summary>
    ///UploadAttachment 的摘要描述
    /// </summary>
    public class UploadAttachmentForGoogle : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";

            var Request = context.Request;
            var Response = context.Response;

            Root result = new Root
            {
                UXB2B = "電子發票系統",
                Result = new RootResult
                {
                    timeStamp = DateTime.Now,
                    value = 0
                }
            };

            List<AutomationItem> automation = new List<AutomationItem>();
           
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    //if (Request.Headers["Signature"] != null && !verifySignature(Request, file))
                    //{
                    //    throw new Exception("資料簽章不符!!");
                    //}

                    //zip檔名
                    String fileName = Path.GetFileName(file.FileName);
                    //log 底下路徑 + zip檔名
                    String fullPath = Path.Combine(Logger.LogDailyPath, fileName);
                    file.SaveAs(fullPath);

                    using (InvoiceManager mgr = new InvoiceManager())
                    {
                        var table = mgr.GetTable<InvoicePurchaseOrder>();
                        var attachment = mgr.GetTable<Attachment>();

                        using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(fullPath))
                        {
                            foreach (var entry in zip.Entries)
                            {
                                AutomationItem auto = new AutomationItem();

                                if (!entry.IsDirectory)
                                {
                                    String entryName = Path.GetFileName(entry.FileName);
                                    auto.Attachment = new AutomationItemAttachment
                                    {
                                        FileName = entryName
                                    };

                                    try
                                    {
                                        //zip檔名(無副檔名)
                                        String keyName = Path.GetFileNameWithoutExtension(entryName);

                                        //比對資料
                                        var orderItem = table.Where(o => o.OrderNo == keyName).FirstOrDefault();
                                        if (orderItem == null)
                                        {
                                            ////建立暫存資料夾
                                            //string TempForPDF = Path.Combine(Path.Combine(Logger.LogPath, "TempForPDF"));// Server.MapPath("~/TempForReceivePDF");
                                            //if (!Directory.Exists(TempForPDF))
                                            //    Directory.CreateDirectory(TempForPDF);

                                            //將檔案解壓到暫存資料夾
                                            //string unZipPath = "";
                                            //string zipPath = "";
                                            //UnZipFiles(fullPath, out unZipPath, out zipPath);
                                            entry.Extract(GoogleInvoiceManager.AttachmentPoolPath, ExtractExistingFileAction.OverwriteSilently);
                                        }
                                        else
                                        {
                                            //確認是否有相同附件檔資料
                                            entry.Extract(Logger.LogDailyPath, ExtractExistingFileAction.OverwriteSilently);

                                            var attachedItem = mgr.GetTable<Attachment>().Where(a => a.KeyName == keyName).FirstOrDefault();
                                            if (attachedItem == null)
                                            {
                                                attachedItem = new Attachment
                                                {
                                                    DocID = orderItem.InvoiceID,
                                                    KeyName = keyName
                                                };

                                                mgr.GetTable<Attachment>().InsertOnSubmit(attachedItem);
                                            }

                                            attachedItem.StoredPath = Path.Combine(Logger.LogDailyPath, entryName);
                                        }

                                        auto.Description = "";
                                        auto.Status = 1;
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex);
                                        auto.Description = ex.Message;
                                        auto.Status = 0;

                                        ExceptionNotification.SendExceptionNotificationToSysAdmin(new Exception("PDF附件上傳：", ex));
                                    }

                                    automation.Add(auto);
                                }
                            }
                        } 

                        mgr.SubmitChanges();
                    }

                    result.Result.value = 1;
                    result.Automation = automation.ToArray();
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result.Result.message = ex.Message;

                ExceptionNotification.SendExceptionNotificationToSysAdmin(new Exception("PDF附件上傳：", ex));
            }

            result.ConvertToXml().Save(Response.OutputStream);

        }

        //private void UnZipFiles(string fullPath, out string unZipPath, out string zipPath)
        //{
        //    ZipFile unzip = ZipFile.Read(fullPath);

        //    unZipPath = Logger.LogDailyPath + "\\UnZip";

        //    foreach (ZipEntry e in unzip)
        //    {
        //        e.Extract(unZipPath, ExtractExistingFileAction.OverwriteSilently);
        //    }
        //    unzip.Dispose();
        //    zipPath = fullPath;
        //}

        //private bool verifySignature(HttpRequest Request, HttpPostedFile file)
        //{
        //    CryptoUtility crypto = new CryptoUtility();
        //    PKCS7Log log = crypto.CA_Log.Table.DataSet as PKCS7Log;
        //    if (log != null)
        //    {
        //        log.Crypto = crypto;
        //        log.Catalog = Naming.CACatalogDefinition.UXGW上傳附件檔;
        //    }

        //    byte[] dataToSign = new byte[file.ContentLength];
        //    file.InputStream.Seek(0, SeekOrigin.Begin);
        //    file.InputStream.Read(dataToSign, 0, dataToSign.Length);

        //    return crypto.VerifyPKCS7(dataToSign, Convert.FromBase64String(Request.Headers["Signature"]));
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}