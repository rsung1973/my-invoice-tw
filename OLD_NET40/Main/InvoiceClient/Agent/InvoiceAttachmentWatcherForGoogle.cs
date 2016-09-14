using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    class InvoiceAttachmentWatcherForGoogle : InvoiceWatcherForGoogle
    {
        public InvoiceAttachmentWatcherForGoogle(String fullPath)
            : base(fullPath)
        {

        }

        protected override void processFile(string invFile)
        {
            if (!File.Exists(invFile))
                return;

            String fileName = Path.GetFileName(invFile);
            String fullPath = Path.Combine(_inProgressPath, fileName);
            String contentPath;
            try
            {
                File.Move(invFile, fullPath);
                _isPGP = false;
                if (fullPath.EndsWith(".gpg", StringComparison.CurrentCultureIgnoreCase)
                    || fullPath.EndsWith(".pgp", StringComparison.CurrentCultureIgnoreCase))
                {
                    _isPGP = true;
                    contentPath = fullPath.Substring(0, fullPath.Length - 4);
                    var decrypt = PGPCrypto.Instance.CreateDecrypt(_inProgressPath, fullPath, contentPath);
                    if (decrypt == null)
                    {
                        throw new Exception("GnuPG command tools are not ready!!");
                    }
                    else
                    {
                        using (Process proc = new Process())
                        {
                            proc.StartInfo = decrypt;
                            proc.Start();
                            proc.WaitForExit(Settings.Default.PGPWaitingForExitInMilliSeconds);
                        }
                    }
                }
                else
                {
                    contentPath = fullPath;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return;
            }
            Root result = new Root();
            try
            {
                
                using (WebClientEx wc = new WebClientEx())
                {
                    wc.Timeout = 43200000;
                    //var signed = fullPath.SignFile(true);
                    //wc.Headers.Add("Signature", Convert.ToBase64String(signed.Encode()));
                    byte[] data = wc.UploadFile(Settings.Default.UploadAttachment, contentPath);
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(ms);
                        result = doc.ConvertTo<Root>();
                    }
                }

                if (contentPath != fullPath)
                {
                    File.Delete(contentPath);
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
            finally
            {
                if (result.Automation != null)
                {
                    Automation auto = new Automation { Item = result.Automation };
                    if (_isPGP)
                    {
                        String responseName = String.Format("{0}{1}", Path.Combine(Logger.LogDailyPath, Path.GetFileNameWithoutExtension(contentPath.Replace("request", "response").Replace("_OUT_", "_IN_"))), ".xml");
                        auto.ConvertToXml().Save(responseName);
                        PGPCrypto.Instance.EncryptFile(responseName);

                        String gpgName = responseName + ".gpg";
                        if (File.Exists(gpgName))
                        {
                            File.Copy(gpgName, Path.Combine(_ResponsedPath, Path.GetFileName(gpgName)), true);
                        }
                    }
                    else
                    {
                        auto.ConvertToXml().Save(String.Format("{0}{1}", Path.Combine(_ResponsedPath, Path.GetFileNameWithoutExtension(fileName.Replace("request", "response").Replace("_OUT_", "_IN_"))), ".xml"));
                    }
                }
            }
        }

        protected override void processError(string message, XmlDocument docInv, string fileName)
        {
            Logger.Warn(String.Format("Failed to Send an Attachment ({0}) When Uploading Files!!For the Following Reasons:\r\n{1}", fileName, message));
        }

        public override string ReportError()
        {
            int count = Directory.GetFiles(_failedTxnPath).Length;
            return count > 0 ? String.Format("{0} Attachment Zip File Transfer Failure!!\r\n", count) : null;
        }
    }
}
