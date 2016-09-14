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

namespace eIVOGo.Published
{
    /// <summary>
    ///UploadAttachment 的摘要描述
    /// </summary>
    public class UploadAttachment : IHttpHandler
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

            try
            {
                if (Request.Files.Count > 0)
                {
                    using (InvoiceManager mgr = new InvoiceManager())
                    {
                        var attachment = mgr.GetTable<Attachment>();
                        //for (int index = 0;index<Request.Files.Count;index++)
                        //{
                        //var file = Request.Files[index];
                        var file = Request.Files[0];
                        String fileName = Path.GetFileName(file.FileName);
                        String keyName = Path.GetFileNameWithoutExtension(fileName);
                        String fullPath = Path.Combine(Logger.LogDailyPath, fileName);
                        file.SaveAs(fullPath);

                        if (Request.Headers["Signature"] != null && !verifySignature(Request, file))
                        {
                            throw new Exception("資料簽章不符!!");
                        }

                        var item = attachment.Where(a => a.KeyName == keyName).FirstOrDefault();
                        if (item == null)
                        {
                            item = new Attachment
                            {
                                KeyName = keyName
                            };
                            attachment.InsertOnSubmit(item);
                        }
                        item.StoredPath = fullPath;
                        //}
                        mgr.SubmitChanges();
                    }
                    result.Result.value = 1;
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

        private bool verifySignature(HttpRequest Request, HttpPostedFile file)
        {
            CryptoUtility crypto = new CryptoUtility();
            PKCS7Log log = crypto.CA_Log.Table.DataSet as PKCS7Log;
            if (log != null)
            {
                log.Crypto = crypto;
                log.Catalog = Naming.CACatalogDefinition.UXGW上傳附件檔;
            }

            byte[] dataToSign = new byte[file.ContentLength];
            file.InputStream.Seek(0, SeekOrigin.Begin);
            file.InputStream.Read(dataToSign, 0, dataToSign.Length);

            return crypto.VerifyPKCS7(dataToSign, Convert.FromBase64String(Request.Headers["Signature"]));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}