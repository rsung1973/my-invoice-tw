using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using InvoiceClient.Properties;
using Model.Schema.EIVO;

namespace InvoiceClient.Agent
{
    public static class InvoiceCenterTransferManager
    {
        private static A1401Watcher _InvoiceWatcher;
        private static B1401Watcher _AllowanceWatcher;
        private static A0501Watcher _CancellationWatcher;
        private static B0501Watcher _AllowanceCancellationWatcher;
        private static A0401Watcher _A0401Watcher;
        private static B0401Watcher _B0401Watcher;


        public static void EnableAll(String fullPath)
        {

            _InvoiceWatcher = new A1401Watcher(Path.Combine(fullPath, Settings.Default.UploadA1401));
            _InvoiceWatcher.StartUp();

            _A0401Watcher = new A0401Watcher(Path.Combine(fullPath, Settings.Default.UploadA0401));
            _A0401Watcher.StartUp();

            _AllowanceWatcher = new B1401Watcher(Path.Combine(fullPath, Settings.Default.UploadB1401));
            _AllowanceWatcher.StartUp();

            _B0401Watcher = new B0401Watcher(Path.Combine(fullPath, Settings.Default.UploadB0401));
            _B0401Watcher.StartUp();

            _CancellationWatcher = new A0501Watcher(Path.Combine(fullPath, Settings.Default.UploadA0501));
            _CancellationWatcher.StartUp();

            _AllowanceCancellationWatcher = new B0501Watcher(Path.Combine(fullPath, Settings.Default.UploadB0501));
            _AllowanceCancellationWatcher.StartUp();
        }

        public static void PauseAll()
        {
            if (_InvoiceWatcher != null)
            {
                _InvoiceWatcher.Dispose();
            }

            if (_A0401Watcher != null)
            {
                _A0401Watcher.Dispose();
            }

            if (_AllowanceWatcher != null)
            {
                _AllowanceWatcher.Dispose();
            }

            if (_B0401Watcher != null)
            {
                _B0401Watcher.Dispose();
            }

            if (_CancellationWatcher != null)
            {
                _CancellationWatcher.Dispose();
            }
            if (_AllowanceCancellationWatcher != null)
            {
                _AllowanceCancellationWatcher.Dispose();
            }
        }

        public static String ReportError()
        {
            StringBuilder sb = new StringBuilder();
            if (_InvoiceWatcher != null)
                sb.Append(_InvoiceWatcher.ReportError());
            if (_A0401Watcher != null)
                sb.Append(_A0401Watcher.ReportError());
            if (_CancellationWatcher != null)
                sb.Append(_CancellationWatcher.ReportError());
            if (_AllowanceWatcher != null)
                sb.Append(_AllowanceWatcher.ReportError());
            if (_B0401Watcher != null)
                sb.Append(_B0401Watcher.ReportError());
            if (_AllowanceCancellationWatcher != null)
                sb.Append(_AllowanceCancellationWatcher.ReportError());
            return sb.ToString();

        }

        public static void SetRetry()
        {
            _InvoiceWatcher.Retry();
            _A0401Watcher.Retry();
            _CancellationWatcher.Retry();
            _AllowanceWatcher.Retry();
            _B0401Watcher.Retry();
            _AllowanceCancellationWatcher.Retry();
        }

    }
}
