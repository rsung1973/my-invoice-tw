using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uxnet.Com.WS_DocumentService;
using System.IO;

namespace DocmentServer
{
    public partial class PdfDocument : IDisposable
    {
        protected bool _bDisposed = false;
        protected string _srcUrl;

        public string PdfFileName { get; protected set; }


        internal PdfDocument(string url)
        {
            _srcUrl = url;
        }

        internal virtual void CreateDocument()
        {
            using (DocumentCreator creator = new DocumentCreator())
            {
                PdfFileName = creator.CreatePdfFromUrl(_srcUrl);
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
                    if (!String.IsNullOrEmpty(PdfFileName) && File.Exists(PdfFileName))
                    {
                        File.Delete(PdfFileName);
                    }
                }

                _bDisposed = true;
            }
        }

        ~PdfDocument()
        {
            dispose(false);
        }


        #endregion
    }
}
