using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Utility;
using System.IO;
using Model.Properties;

namespace Model.InvoiceManagement
{
    public class GovPlatformFactoryForB2C
    {
        private static bool _IsActive = false;
        private static Queue<DateTime?> _EventQ = new Queue<DateTime?>();

        public static EventHandler SendNotification
        {
            get;
            set;
        }

        public static void Notify()
        {
            Settings.Default.GOVPlatformOutbound.CheckStoredPath();
            Settings.Default.GOVPlatformResponse.CheckStoredPath();

            _EventQ.Enqueue(DateTime.Now);
            if (!_IsActive)
            {
                ThreadPool.QueueUserWorkItem(notifyToProcess);
            }
        }

        private static void notifyToProcess(object stateInfo)
        {
            bool bRun = false;
            lock (typeof(GovPlatformFactoryForB2C))
            {
                if (!_IsActive)
                {
                    bRun = true;
                    _IsActive = true;
                }
            }

            if (!bRun)
                return;

            try
            {
                processEventQueue();
                Logger.Info("event message處理完成!");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            _IsActive = false;
        }

        private static void processEventQueue()
        {
            while (_EventQ.Count > 0)
            {
                DateTime? ev = (DateTime?)_EventQ.Dequeue();
                GovPlatformManagerForB2C mgr = new GovPlatformManagerForB2C();
                mgr.TransmitInvoice();
                mgr.CheckResponse();
            }
        }
    }
}
