using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class CsvInvoiceWatcher : InvoiceWatcher
    {

        public CsvInvoiceWatcher(String fullPath)
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

            WS_Invoice.eInvoiceService invSvc = CreateInvoiceService();

            try
            {
                var docInv = fullPath.SignFile();
                var result = processUpload(invSvc, docInv);

                if (result.Result.value != 1)
                {
                    if (result.Response != null && result.Response.InvoiceNo != null && result.Response.InvoiceNo.Length > 0)
                    {
                        processError(result.Response.InvoiceNo, fileName);
                        storeFile(fullPath, Path.Combine(_failedTxnPath, fileName));
                    }
                    else
                    {
                        processError(result.Result.message, null, fileName);
                        storeFile(fullPath, Path.Combine(_failedTxnPath, fileName));
                    }
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

        protected virtual Root processUpload(WS_Invoice.eInvoiceService invSvc, SignedCms docInv)
        {
            var result = invSvc.UploadInvoiceCmsCSVAutoTrackNo(docInv.Encode()).ConvertTo<Root>();
            return result;
        }

        protected virtual void processError(IEnumerable<RootResponseInvoiceNo> rootInvoiceNo, string fileName)
        {
            if (rootInvoiceNo != null && rootInvoiceNo.Count() > 0)
            {
                IEnumerable<String> message = rootInvoiceNo.Select(i => i.Description);
                Logger.Warn(String.Format("在上傳發票檔({0})時,傳送失敗!!原因如下:\r\n{1}", fileName, String.Join("\r\n", message.ToArray())));
            }
        }

    }
}
