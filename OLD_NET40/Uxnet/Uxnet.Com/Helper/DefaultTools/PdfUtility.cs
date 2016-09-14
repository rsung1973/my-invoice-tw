using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using Utility;
using Uxnet.Com.Properties;
using Uxnet.ToolAdapter.Common;

namespace Uxnet.Com.Helper.DefaultTools
{
    class PdfUtility : IPdfUtility
    {
        static PdfUtility()
        {
            if (Settings.Default.UsePDFPrinterService)
            {
                Thread thread = new Thread(() =>
                {
                    Program.Main(null);
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }

        public void ConvertHtmlToPDF(string htmlFile, string pdfFile, double timeOutInMinute)
        {
            String pdfTrigger = Path.Combine(Logger.LogDailyPath, String.Format("{0}.txt", Path.GetFileNameWithoutExtension(htmlFile)));

            using (StreamWriter sw = new StreamWriter(pdfTrigger))
            {
                sw.Write(htmlFile);
                sw.Flush();
                sw.Close();
            }

            File.Move(pdfTrigger, Path.Combine(Settings.Default.PDFWorkingQueue, Path.GetFileName(pdfTrigger)));
            DateTime deadline = DateTime.Now.AddMinutes(timeOutInMinute);

            while (!File.Exists(pdfFile) && DateTime.Now < deadline)
            {
                Thread.Yield();
            }
        }
    }
}
