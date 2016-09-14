using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading;
using System.Xml;

using InvoiceClient.Helper;
using InvoiceClient.Properties;
using Model.Schema.EIVO;
using Model.Schema.EIVO.B2B;
using Model.Schema.TXN;
using Utility;

namespace InvoiceClient.Agent
{
    public class InvoiceAttachmentWatcher : InvoiceWatcher
    {

        public InvoiceAttachmentWatcher(String fullPath)
            : base(fullPath)
        {

        }

        protected override void processFile(string invFile)
        {
            if (!File.Exists(invFile))
                return;

            String fileName = Path.GetFileName(invFile);
            String fullPath = Path.Combine(_inProgressPath, fileName);
            try
            {
                File.Move(invFile, fullPath);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return;
            }

            try
            {
                Root result;
                using (WebClient wc = new WebClient())
                {
                    var signed = fullPath.SignFile(true);
                    wc.Headers.Add("Signature", Convert.ToBase64String(signed.Encode()));
                    byte[] data = wc.UploadFile(Settings.Default.UploadAttachment, fullPath);
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(ms);
                        result = doc.ConvertTo<Root>();
                    }
                }

                if (result.Result.value != 1)
                {
                    processError(result.Result.message, null, fileName);
                    storeFile(fullPath, Path.Combine(_failedTxnPath, fileName));
                }
                else
                {
                    storeFile(fullPath, Path.Combine(Logger.LogDailyPath, fileName));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                storeFile(fullPath, Path.Combine(_failedTxnPath, fileName));
            }
        }

        protected override void processError(string message, XmlDocument docInv, string fileName)
        {
            Logger.Warn(String.Format("在上傳發票附件檔({0})時,傳送失敗!!原因如下:\r\n{1}", fileName, message));
        }

    }
}
