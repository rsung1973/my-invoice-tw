using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using Utility;

using Uxnet.Com.Properties;
using Win32;

namespace DocmentServer
{
    public partial class DocumentFactory
    {
        public static XpsDocument BuildXpsDocument(string url)
        {
            lock (typeof(DocumentFactory))
            {
                XpsDocument xpsDoc = new XpsDocument(url);
                xpsDoc.CreateDocument();
                return xpsDoc;
            }
        }

        public static PdfDocument BuildPdfDocument(string url)
        {
            PdfDocument pdfDoc = new PdfDocument(url);
            pdfDoc.CreateDocument();
            return pdfDoc;
        }
    }

    public partial class XpsDocument : IDisposable
    {
        protected bool _bDisposed = false;
        protected string _srcUrl;
        protected Process _proc;

        public string XpsFileName { get; protected set; }


        internal XpsDocument(string url)
        {
            _srcUrl = url;
        }

        internal virtual void CreateDocument()
        {
            bool isFinished = false;

            XpsFileName = String.Format("{0}{1}.xps", Path.GetTempPath(), Guid.NewGuid());

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "rundll32.exe";
            info.Arguments = String.Format("mshtml.dll,PrintHTML \"{0}\" \"{1}\"", _srcUrl, Settings.Default.XpsPrinterName);
            info.WorkingDirectory = Directory.GetCurrentDirectory();

            _proc = new Process();
            _proc.EnableRaisingEvents = true;
            //proc.Exited += new EventHandler(proc_Exited);

            //if (null != _eventHandler)
            //{
            //    proc.Exited += new EventHandler(_eventHandler);
            //}

            _proc.StartInfo = info;
            _proc.Start();

            Thread.Sleep(5000);

            // Find Print Dialog box

            IntPtr hWnd;
            while ((hWnd = WinAPI.FindWindow("#32770", Settings.Default.WndPrnTitle)) == IntPtr.Zero)
                Thread.Sleep(5000);

            if (hWnd != IntPtr.Zero)
            {

                // Print button

                IntPtr hChild = WinAPI.FindWindowEx(hWnd, IntPtr.Zero, "Button", Settings.Default.BtnPrnTitle);
                if (hChild != IntPtr.Zero)
                {

                    StringBuilder command = new StringBuilder(4096);

                    WinAPI.GetWindowText(hChild, command, 4096);

                    //Logger.Info(command.ToString());


                    // Press Print button

                    WinAPI.SendMessage(hChild, (int)WM.IME_KEYDOWN, new IntPtr(80), IntPtr.Zero);

                    Thread.Sleep(5000);

                    // Find Save File Dialog Box

                    while ((hWnd = WinAPI.FindWindow("#32770", Settings.Default.WndSaveAsTitle)) == IntPtr.Zero)
                        Thread.Sleep(5000);

                    if (hWnd != IntPtr.Zero)
                    {
                        hChild = WinAPI.FindWindowEx(hWnd, IntPtr.Zero, "ComboBoxEx32", IntPtr.Zero);
                        if (hChild != IntPtr.Zero)
                        {
                            hChild = WinAPI.FindWindowEx(hChild, IntPtr.Zero, "ComboBox", IntPtr.Zero);
                            if (hChild != IntPtr.Zero)
                                hChild = WinAPI.FindWindowEx(hChild, IntPtr.Zero, "Edit", IntPtr.Zero);    // File name edit control
                        }
                        else
                        {
                            ///Vista Desktop
                            ///
                            hChild = WinAPI.FindWindowEx(hWnd, IntPtr.Zero, "DUIViewWndClassName", IntPtr.Zero);
                            if (hChild != IntPtr.Zero)
                                hChild = WinAPI.FindWindowEx(hChild, IntPtr.Zero, "DirectUIHWND", IntPtr.Zero);
                            if (hChild != IntPtr.Zero)
                                hChild = WinAPI.FindWindowEx(hChild, IntPtr.Zero, "FloatNotifySink", IntPtr.Zero);
                            if (hChild != IntPtr.Zero)
                                hChild = WinAPI.FindWindowEx(hChild, IntPtr.Zero, "ComboBox", IntPtr.Zero);
                            if (hChild != IntPtr.Zero)
                                hChild = WinAPI.FindWindowEx(hChild, IntPtr.Zero, "Edit", IntPtr.Zero);

                        }

                        if (hChild != IntPtr.Zero)
                        {

                            // Enter file name

                            WinAPI.SendMessage(hChild, (int)WM.SETTEXT, IntPtr.Zero, XpsFileName);

                            Thread.Sleep(1000);

                            // Find Save button

                            //hChild = WinAPI.FindWindowEx(hWnd, IntPtr.Zero, "Button", IntPtr.Zero);
                            hChild = WinAPI.FindWindowEx(hWnd, IntPtr.Zero, "Button", Settings.Default.BtnSaveTitle);

                            command.Remove(0, command.Length);
                            WinAPI.GetWindowText(hChild, command, 4096);
                            command.Append(" was sent to DocumentServer!!\r\n");
                            //Logger.Info(command.ToString());

                            // Press Save button

                            WinAPI.SendMessage(hChild, (int)WM.IME_KEYDOWN, new IntPtr(83), IntPtr.Zero);
                            isFinished = true;
                            //Logger.Info("Action is finished..");
                        }

                    }
                }
            }

            if (isFinished)
            {
                //Logger.Info("Start waiting ...");
                isFinished = _proc.WaitForExit(60000);
            }

            if (isFinished)
            {
                //Logger.Info("file OK! " + XpsFileName);
            }
            else
            {
                try
                {
                    _proc.Kill();
                    _proc.WaitForExit();
                    Logger.Warn("Wake up and kill the process , unexpected happens :-( ");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
                if (File.Exists(XpsFileName))
                {
                    File.Delete(XpsFileName);
                }
                XpsFileName = null;
            }
        }



        #region IDisposable 成員

        public void Dispose()
        {
            dispose(true);
        }

        private void dispose(bool disposing)
        {
            if (!_bDisposed)
            {
                if (disposing)
                {
                    if (!String.IsNullOrEmpty(XpsFileName) && File.Exists(XpsFileName))
                    {
                        File.Delete(XpsFileName);
                    }
                    if (_proc != null)
                    {
                        _proc.Dispose();
                        _proc = null;
                    }
                }

                _bDisposed = true;
            }
        }

        ~XpsDocument()
        {
            dispose(false);
        }


        #endregion
    }

}
