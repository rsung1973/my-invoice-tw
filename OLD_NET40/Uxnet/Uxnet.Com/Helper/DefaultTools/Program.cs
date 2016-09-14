using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Utility;
using Uxnet.Com.Properties;

namespace Uxnet.Com.Helper.DefaultTools
{
    class Program
    {
        private WebBrowser _wb;
        protected FileSystemWatcher _watcher;
        protected bool _running = true;
        protected Queue<String> _queue;
        protected Thread _main;

        [STAThread()]
        public static void Main(string[] args)
        {

            if (!Win32.Winspool.SetDefaultPrinter(Settings.Default.PdfPrinterName))
            {
                Logger.Warn(String.Format("預設印表機設定失敗 => {0}", Settings.Default.PdfPrinterName));
                return;
            }

            if (args != null && args.Length > 0)
            {
                Program prog = new Program();
                Logger.Info(args[0]);
                prog.startUp(args[0]);
            }
            else
            {
                (new Program()).Run();
            }
        }

        public void Run()
        {
            Settings.Default.PDFWorkingQueue.CheckStoredPath();
            _watcher = new FileSystemWatcher(Settings.Default.PDFWorkingQueue);
            _watcher.Created += new FileSystemEventHandler(_watcher_Created);
            _watcher.Filter = "*.txt";
            _watcher.EnableRaisingEvents = true;

            _queue = new Queue<string>();
            _main = Thread.CurrentThread;

            Logger.Info("PDF列印服務啟動...");

            //ThreadPool.QueueUserWorkItem(arg =>
            //    {
            //        while (String.Compare(Console.ReadLine(), "Quit", true) != 0) ;
            //        _running = false;
            //        _main.Interrupt();
            //    });

            foreach (String fullPath in Directory.GetFiles(_watcher.Path, "*.txt"))
            {
                processFile(getFullPath(fullPath));
            }

            ThreadPool.QueueUserWorkItem(p =>
            {
                while (_running)
                {
                    _watcher.WaitForChanged(WatcherChangeTypes.Created);
                }
            });

            while (_running)
            {
                try
                {
                    Logger.Info(String.Format("[{0}] 佇列中的工作件數:[{1}]", DateTime.Now, _queue.Count));
                    while (_queue.Count > 0)
                    {
                        processFile(getFullPath(_queue.Dequeue()));
                        Logger.Info(String.Format("[{0}] 佇列中的工作件數:[{1}]", DateTime.Now, _queue.Count));
                    }

                    if (!Thread.Yield())
                    {
                        Logger.Info(String.Format("[{0}] 工作結束,等待...", DateTime.Now));
                        Thread.Sleep(Timeout.Infinite);
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }

        private String getFullPath(String pathFile)
        {
            String fullPath;
            using (StreamReader sr = new StreamReader(pathFile))
            {
                fullPath = sr.ReadLine();
                sr.Close();
            }
            File.Delete(pathFile);
            return fullPath;
        }

        private void processFile(String fullPath)
        {
            String outputFolder = Path.GetDirectoryName(fullPath);

            Logger.Info(String.Format("[{0}] 處理檔案 => {1}", DateTime.Now, fullPath));
            startUp(fullPath);
            Logger.Info(String.Format("[{0}] 處理完畢 => {1}", DateTime.Now, fullPath));

            if (File.Exists(Settings.Default.PdfOutput))
            {
                String fileName = Path.GetFileNameWithoutExtension(fullPath);
                String outputPath = Path.Combine(outputFolder, String.Format("{0}.pdf", fileName));
                String pdfDest = Path.Combine(outputFolder, Path.GetFileName(Settings.Default.PdfOutput));
                try
                {
                    if (File.Exists(outputPath))
                    {
                        File.Delete(outputPath);
                    }

                    Logger.Info(String.Format("[{0}] 搬移檔案 => {1} => {2}", DateTime.Now, Settings.Default.PdfOutput, outputPath));
                    File.Move(Settings.Default.PdfOutput, pdfDest);
                    File.Move(pdfDest, outputPath);
                    Logger.Info(String.Format("[{0}] 搬移完畢 => {1}", DateTime.Now, Settings.Default.PdfOutput, outputPath));
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }

        private void _watcher_Created(object sender, FileSystemEventArgs e)
        {
            Logger.Info(String.Format("[{0}] 發現檔案 => {1}", DateTime.Now, e.FullPath));
            _queue.Enqueue(e.FullPath);
            Logger.Info(String.Format("[{0}] 放入新工作,佇列中的工作件數:[{1}]", DateTime.Now, _queue.Count));
            if (_main.ThreadState == ThreadState.WaitSleepJoin)
            {
                _main.Interrupt();
                Logger.Info(String.Format("[{0}] 喚起主程序進行PDF列印...", DateTime.Now));
            }
        }


        void startUp(String url)
        {
            //foreach (var ptr in PrinterSettings.InstalledPrinters)
            //{
            //    Logger.Info("發現印表機:"+ptr);
            //}

            WebBrowser wb = new WebBrowser();
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);
            wb.Navigating += new WebBrowserNavigatingEventHandler(wb_Navigating);
            wb.Navigated += new WebBrowserNavigatedEventHandler(wb_Navigated);
            _wb = wb;
            _wb.Navigate(url);
            _wb.Show();
            System.Windows.Forms.Application.DoEvents();

            DateTime timeOut = DateTime.Now.AddMilliseconds(Settings.Default.MaxWaitingInterval);
            Logger.Info(String.Format("Timeout:{0}ms", Settings.Default.MaxWaitingInterval));
            Logger.Info(String.Format("Output:{0}", Settings.Default.PdfOutput));

            while (!File.Exists(Settings.Default.PdfOutput) && _running)
            {
                System.Windows.Forms.Application.DoEvents();
            }

        }

        void wb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Logger.Info(String.Format("Navigated:{0}", e.Url));
        }

        void wb_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            Logger.Info(String.Format("Navigating:{0}", e.Url));
        }

        void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Logger.Info(e.Url);
            if (_wb.ReadyState == WebBrowserReadyState.Complete)
            {
                //Logger.Info("Printer Name:" + (new PrintDocument()).PrinterSettings.PrinterName);
                _wb.Print();
                Logger.Info("Start printing...");
            }
        }

    }
}
