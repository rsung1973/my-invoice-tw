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


namespace InvoiceClient.Agent
{
    public class InvoiceWatcher : IDisposable
    {
        protected FileSystemWatcher _watcher;
        protected String _failedTxnPath;
        protected String _inProgressPath;
        protected String _ResponsedPath;

        public InvoiceWatcher(String fullPath)
        {
            fullPath.CheckStoredPath();

            _watcher = new FileSystemWatcher(fullPath);
            _watcher.Created += new FileSystemEventHandler(_watcher_Created);
            _watcher.EnableRaisingEvents = true;

            prepareStorePath(fullPath);

            ThreadPool.QueueUserWorkItem(p =>
            {
                while (_watcher != null)
                {
                    _watcher.WaitForChanged(WatcherChangeTypes.Created);
                }
            });
        }

        protected virtual void prepareStorePath(String fullPath)
        {
            _failedTxnPath = fullPath + "(傳送失敗)";
            _failedTxnPath.CheckStoredPath();

            _inProgressPath = fullPath + "(正在處理)";
            _inProgressPath.CheckStoredPath();

            if(Settings.Default.ResponseUpload)
            {
                _ResponsedPath = fullPath + "(Response)";
                _ResponsedPath.CheckStoredPath();
            }
        }

        private void _watcher_Created(object sender, FileSystemEventArgs e)
        {
            String[] files;
            bool done = false;
            do
            {
                Thread.Sleep(5000);
                files = Directory.GetFiles(_watcher.Path);
                if (files != null && files.Count() > 0)
                {
                    done = true;
                    foreach (String fullPath in files)
                    {
                        processFile(fullPath);
                    }
                }
            } while (files != null && files.Count() > 0);

            if (done)
            {
                processComplete();
            }

        }

        protected virtual void processComplete()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                Root token = this.CreateMessageToken("資料已傳送完成－通知相對營業人");
                invSvc.NotifyCounterpartBusiness(token.ConvertToXml().Sign());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        protected virtual void processFile(String invFile)
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
            Root result = new Root();
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
                if (Settings.Default.ResponseUpload && result.Automation != null)
                {
                    Automation auto = new Automation { Item = result.Automation };
                    auto.ConvertToXml().Save(Path.Combine(_ResponsedPath, fileName));
                }
            }

        }

        protected virtual XmlDocument prepareInvoiceDocument(String invoiceFile)
        {
            XmlDocument docInv = new XmlDocument();
            docInv.Load(invoiceFile);
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

        protected virtual void processError(string message, XmlDocument docInv, string fileName)
        {
            Logger.Warn(String.Format("在上傳發票檔({0})時,傳送失敗!!原因如下:\r\n{1}", fileName, message));
        }

        public static WS_Invoice.eInvoiceService CreateInvoiceService()
        {
            WS_Invoice.eInvoiceService invSvc = new WS_Invoice.eInvoiceService();
            invSvc.Url = Settings.Default.InvoiceClient_WS_Invoice_eInvoiceService;
            return invSvc;
        }

        protected virtual Root processUpload(WS_Invoice.eInvoiceService invSvc, XmlDocument docInv)
        {
            var result = invSvc.UploadInvoice(docInv).ConvertTo<Root>();
            return result;
        }

        protected void storeFile(String srcName,String destName)
        {
            try
            {
                if (File.Exists(destName))
                {
                    File.Delete(destName);
                }
                File.Move(srcName, destName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected virtual void processError(IEnumerable<RootResponseInvoiceNo> rootInvoiceNo, XmlDocument docInv, string fileName)
        {
            if (rootInvoiceNo != null && rootInvoiceNo.Count() > 0)
            {
                IEnumerable<String> message = rootInvoiceNo.Select(i => String.Format("發票號碼:{0}=>{1}", i.Value, i.Description));
                Logger.Warn(String.Format("在上傳發票檔({0})時,傳送失敗!!原因如下:\r\n{1}", fileName, String.Join("\r\n", message.ToArray())));

                InvoiceRoot invoice = docInv.ConvertTo<InvoiceRoot>();
                InvoiceRoot stored = docInv.ConvertTo<InvoiceRoot>();
                stored.Invoice = rootInvoiceNo.Where(i=>i.ItemIndexSpecified).Select(i=>invoice.Invoice[i.ItemIndex]).ToArray();

                stored.ConvertToXml().Save(Path.Combine(_failedTxnPath, Path.GetFileName(fileName)));
            }
        }

        public virtual String ReportError()
        {
            int count = Directory.GetFiles(_failedTxnPath).Length;
            return count > 0 ? String.Format("{0}筆發票資料傳送失敗!!\r\n", count) : null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_watcher != null)
            {
                _watcher.Dispose();
                _watcher = null;
            }
        }

        #endregion

        public void Retry()
        {
            foreach (String fileName in Directory.GetFiles(_failedTxnPath))
            {
                File.Move(fileName, Path.Combine(_watcher.Path, Path.GetFileName(fileName)));
            }
        }

        public void StartUp()
        {
            foreach (String filePath in Directory.GetFiles(_watcher.Path))
            {
                processFile(filePath);
            }
        }
    }
}
