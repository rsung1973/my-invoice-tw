using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InvoiceClient.Properties;
using System.Xml;
using Utility;
using InvoiceClient.Helper;
using Model.Schema.EIVO;
using System.Threading;
using Model.Schema.TXN;
using System.Diagnostics;
using System.Globalization;


namespace InvoiceClient.Agent
{
    public class InvoiceWatcherForGoogle : InvoiceWatcher
    {
        protected bool _isPGP;

        public InvoiceWatcherForGoogle(String fullPath):base(fullPath)
        {
            _ResponsedPath = fullPath + "(Response)";
            _ResponsedPath.CheckStoredPath();
        }

        protected override void prepareStorePath(string fullPath)
        {
            _failedTxnPath = fullPath + "(Failure)";
            _failedTxnPath.CheckStoredPath();

            _inProgressPath = fullPath + "(Processing)";
            _inProgressPath.CheckStoredPath();
        }


        protected override void processComplete()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                Root token = this.CreateMessageToken("Data has been Sent to Complete－Notify Buyer");
                invSvc.NotifyCounterpartBusiness(token.ConvertToXml().Sign());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        protected override void processFile(String invFile)
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
            Root result=new Root();
            try
            {
                XmlDocument docInv = prepareInvoiceDocument(fullPath);

                docInv.Sign();
                result = processUpload(invSvc, docInv);

                if (result.Result.value != 1)
                {
                    if (result.Response != null && result.Response.InvoiceNo != null && result.Response.InvoiceNo.Length > 0)
                    {
                        processError(result.Response.InvoiceNo, docInv, fileName);
                        storeFile(fullPath, Path.Combine(Logger.LogDailyPath, fileName));
                    }
                    else
                    {
                        processError(result.Result.message, docInv, fileName);
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
            finally
            {
                if (result.Automation != null)
                {
                    Automation auto = new Automation { Item = result.Automation };
                    if (_isPGP)
                    {
                        String responseName = fileName.Replace("request", "response")
                            .Replace("_OUT_","_IN_");
                        int idx = responseName.IndexOf('.');
                        if (idx > 0)
                        {
                            responseName = responseName.Substring(0, idx);
                        }
                        responseName = Path.Combine(Logger.LogDailyPath, responseName + ".xml");
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
                        String responseName = Path.Combine(_ResponsedPath, fileName.Replace("request", "response")
                            .Replace("_OUT_","_IN_"));
                        auto.ConvertToXml().Save(responseName);
                    }
                }
            }
        }



        protected override void processError(string message, XmlDocument docInv, string fileName)
        {
            Logger.Warn(String.Format("Failed to Send an Invoice ({0}) When Uploading Files!!For the Following Reasons:\r\n{1}", fileName, message));
        }



        protected override void processError(IEnumerable<RootResponseInvoiceNo> rootInvoiceNo, XmlDocument docInv, string fileName)
        {
            if (rootInvoiceNo != null && rootInvoiceNo.Count() > 0)
            {
                IEnumerable<String> message = rootInvoiceNo.Select(i => String.Format("Invoice Number:{0}=>{1}", i.Value, i.Description));
                Logger.Warn(String.Format("Failed to Send an Invoice ({0}) When Uploading Files!!For the Following Reasons:\r\n{1}", fileName, String.Join("\r\n", message.ToArray())));

                InvoiceRoot invoice = docInv.ConvertTo<InvoiceRoot>();
                InvoiceRoot stored = docInv.ConvertTo<InvoiceRoot>();
                stored.Invoice = rootInvoiceNo.Where(i=>i.ItemIndexSpecified).Select(i=>invoice.Invoice[i.ItemIndex]).ToArray();

                if (_isPGP)
                {
                    String outputName = Path.GetFileName(fileName);
                    int idx = outputName.IndexOf('.');
                    if(idx>0)
                        outputName = outputName.Substring(0,idx);
                    String errOutFile = Path.Combine(Logger.LogDailyPath, outputName + ".xml");
                    stored.ConvertToXml().Save(errOutFile);

                    PGPCrypto.Instance.EncryptFile(errOutFile);

                    String gpgName = errOutFile + ".gpg";
                    if (File.Exists(gpgName))
                    {
                        File.Copy(gpgName, Path.Combine(_failedTxnPath, Path.GetFileName(gpgName)), true);
                    }


                }
                else
                {
                    String errOutFile = Path.Combine(_failedTxnPath, Path.GetFileName(fileName));
                    stored.ConvertToXml().Save(errOutFile);
                }
            }
        }


        public override String ReportError()
        {
            int count = Directory.GetFiles(_failedTxnPath).Length;
            return count > 0 ? String.Format("{0} Invoice Data Transfer Failure!!\r\n", count) : null;
        }

        protected override XmlDocument prepareInvoiceDocument(String invoiceFile)
        {
            XmlDocument docInv = new XmlDocument();
            _isPGP = false;

            if (invoiceFile.EndsWith(".gpg", StringComparison.CurrentCultureIgnoreCase)
                || invoiceFile.EndsWith(".pgp", StringComparison.CurrentCultureIgnoreCase))
            {
                _isPGP = true;
                var decrypt = PGPCrypto.Instance.CreateDecrypt(_inProgressPath, invoiceFile);
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
                        docInv.Load(proc.StandardOutput.BaseStream);
                    }
                }
            }
            else
            {
                docInv.Load(invoiceFile);
            }

            ///去除"N/A"資料
            ///
            var nodes = docInv.SelectNodes("//*[text()='N/A']");
            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes.Item(i);
                node.RemoveChild(node.SelectSingleNode("text()"));
            }
            ///
            return docInv;
        }



    }
}
