using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Net.Mail;
using System.Net;
using System.Text;
using Model.InvoiceManagement;
using eIVOGo.Properties;
using eIVOGo.Module.SAM;
using Utility;
using eIVOGo.Module.Common;
using Model.DataEntity;
using System.Diagnostics;
using Model.Locale;
using Uxnet.Com.Helper;


namespace eIVOGo.services
{
    public static class ServiceWorkItem
    {
        private static bool __IsNotifyingGovPlatform;
        private static bool __IsNotifyingClientAlert;
        private static DateTime __DailyCheck = DateTime.Today;
        private static DateTime __UnassignNOCheck;
        static int _AssertionDay = 10;
        static TimeSpan _AssertionTime = new TimeSpan(5, 0, 0);
        static TimeSpan _CheckTime = new TimeSpan(9, 0, 0);
        static ServiceWorkItem()
        {
            var jobList = JobScheduler.JobList;

            if (Settings.Default.EnableJobScheduler)
            {

                if (jobList == null || !jobList.Any(j => j.AssemblyQualifiedName == typeof(UnassignNOCheckSchedule).AssemblyQualifiedName))
                {
                    JobScheduler.AddJob(new JobItem
                    {
                        AssemblyQualifiedName = typeof(UnassignNOCheckSchedule).AssemblyQualifiedName,
                        Description = "計算上期空白發票",
                        Schedule = new DateTime(DateTime.Today.Year, DateTime.Today.Month, _AssertionDay).Add(_AssertionTime)
                    });
                }
                if (jobList == null || !jobList.Any(j => j.AssemblyQualifiedName == typeof(DailyCheckSchedule).AssemblyQualifiedName))
                {
                    JobScheduler.AddJob(new JobItem
                    {
                        AssemblyQualifiedName = typeof(DailyCheckSchedule).AssemblyQualifiedName,
                        Description = "簡訊儲值通知",
                        Schedule = new DateTime(DateTime.Today.Year, DateTime.Today.Month, _AssertionDay).Add(_AssertionTime)
                    });
                }
                if (jobList == null || !jobList.Any(j => j.AssemblyQualifiedName == typeof(TurnKeyCheckSchedule).AssemblyQualifiedName))
                {
                    JobScheduler.AddJob(new JobItem
                    {
                        AssemblyQualifiedName = typeof(TurnKeyCheckSchedule).AssemblyQualifiedName,
                        Description = "每日未上傳大平台統計",
                        Schedule = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day).Add(_AssertionTime)
                    });
                }
            }
            ///設定起始執行的時間
            ///最近的單月3日
            ///
            DateTime start = DateTime.Today.AddMonths(0 - ((DateTime.Today.Month + 1) % 2));
            __UnassignNOCheck = new DateTime(start.Year, start.Month, 3);
        }

        public static void NotifyGovPlatform()
        {
            if (!__IsNotifyingGovPlatform)
            {
                if (ThreadSafeCheckEnable(ref __IsNotifyingGovPlatform))
                {
                    ThreadPool.QueueUserWorkItem(info =>
                        {
                            //doUnassignNOCheck(null,null);
                            if (Settings.Default.EnableGovPlatform)
                                GovPlatformFactory.Notify();
                            if (Settings.Default.EnableEIVOPlatform)
                                EIVOPlatformFactory.Notify();
                           // doDailyCheck();                            
                            Thread.Sleep(Settings.Default.GovPlatformAutoTransferInterval);
                            SystemMonitorControl.BackgroundService.Interrupt();
                            __IsNotifyingGovPlatform = false;
                        });
                }

            }
        }

        private static void doDailyCheck()
        {
            if (__DailyCheck < DateTime.Now)
            {
                __DailyCheck = DateTime.Today.AddDays(1);
                try
                {
                    double credit;
                    if (ModelExtension.MessageManagement.SMSManager.AlertToLowerCredit(out credit))
                    {
                        String.Format("簡訊儲值點數即將用盡!!剩餘點數:{0}", credit).SendMailMessage(Settings.Default.WebMaster, "簡訊儲值點數不足");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }

        public static void doUnassignNOCheck(string SellerID, string TrackID)
        {
            //if (__UnassignNOCheck < DateTime.Now || !string.IsNullOrEmpty(TrackID))
            //{
                DateTime prophase;
                int year, period;
                using (InvoiceManager mgr = new InvoiceManager())
                {
                    IQueryable<InvoiceTrackCode> trackcodes;
                    if (string.IsNullOrEmpty( TrackID ))
                    {
                        prophase = __UnassignNOCheck.AddMonths(-1);
                        year = prophase.Year;
                        period = prophase.Month / 2;
                        ///累進到下次執行時間
                        ///
                        __UnassignNOCheck = __UnassignNOCheck.AddMonths(2);
                        //DateTime prophase = (DateTime.Now.Month % 2).Equals(0) ? DateTime.Now.AddMonths(-2) : DateTime.Now.AddMonths(-1);
                         trackcodes = mgr.GetTable<InvoiceTrackCode>().Where(t => t.Year.Equals(year) & t.PeriodNo.Equals(period));
                   
                    }
                    else
                    {
                        trackcodes = mgr.GetTable<InvoiceTrackCode>().Where(t => t.TrackID.Equals(TrackID));
                   
                    }




                     foreach (var tcode in trackcodes)
                    {
                        var nointervals = mgr.GetTable<InvoiceNoInterval>().Where(i => i.TrackID.Equals(tcode.TrackID));
                        if (!string.IsNullOrEmpty(SellerID))
                            nointervals = nointervals.Where(n => n.SellerID.Equals(SellerID));
                        foreach (var nointerval in nointervals)
                        {
                            var record = mgr.GetTable<UnassignedInvoiceNo>().Where(b => b.TrackID == nointerval.TrackID && b.SellerID == nointerval.SellerID);
                            if (record.Count() > 0)
                            {
                                mgr.GetTable<UnassignedInvoiceNo>().DeleteAllOnSubmit(record);

                            }
                            List<int> allInvoiceNo = Enumerable.Range(nointerval.StartNo, (nointerval.EndNo - nointerval.StartNo + 1)).ToList();
                            List<int> usedInvoiceNo;
                            if (mgr.GetTable<Organization>().Where(o => o.CompanyID == nointerval.SellerID).FirstOrDefault().OrganizationCategory.FirstOrDefault().CategoryID == (int)Naming.B2CCategoryID.店家)
                                usedInvoiceNo = mgr.GetTable<InvoiceItem>()
                            .Where(a => a.SellerID.Equals(nointerval.SellerID))
                            .Where(a => a.TrackCode.Equals(nointerval.InvoiceTrackCodeAssignment.InvoiceTrackCode.TrackCode))
                            .Select(a => Convert.ToInt32(a.No)).ToList();
                            else
                                usedInvoiceNo = mgr.GetTable<InvoiceNoAssignment>().Where(a => a.IntervalID.Equals(nointerval.IntervalID)).Select(a => a.InvoiceNo.Value).ToList();
                            List<int> result = allInvoiceNo.Except(usedInvoiceNo).ToList();
                            processUnassignNo(nointerval.TrackID, nointerval.SellerID, result, mgr);

                        }
                    }
                    mgr.SubmitChanges();
                }
            //}
        }

        private static void processUnassignNo(int trackid, int sellerid, List<int> Numbers, InvoiceManager mgr)
        {
            //using (InvoiceManager mgr = new InvoiceManager())
            //{
                int startIndex = 0;
                int endIndex = 0;
                for (int i = 0; i < Numbers.Count; i++)
                {
                    if (i.Equals(Numbers.Count - 1))
                    {
                        UnassignedInvoiceNo item = new UnassignedInvoiceNo
                        {
                            TrackID = trackid,
                            SellerID = sellerid,
                            InvoiceBeginNo = Numbers[startIndex],
                            InvoiceEndNo = Numbers[i]
                        };
                        mgr.GetTable<UnassignedInvoiceNo>().InsertOnSubmit(item);
                    }
                    else if (Numbers[i + 1] - Numbers[i] > 1)
                    {
                        endIndex = i;
                        UnassignedInvoiceNo item = new UnassignedInvoiceNo
                        {
                            TrackID = trackid,
                            SellerID = sellerid,
                            InvoiceBeginNo = Numbers[startIndex],
                            InvoiceEndNo = Numbers[endIndex]
                        };
                        mgr.GetTable<UnassignedInvoiceNo>().InsertOnSubmit(item);
                        startIndex = i + 1;
                    }
                }
               // mgr.SubmitChanges();
           // }
        }
        private static void doDailyTurnKeyCheck()
        {
            if (__DailyCheck < DateTime.Now)
            {
                __DailyCheck = DateTime.Today.AddDays(1);
                try
                {
                    eIVOGo.Published.AlertAdmTurnKeyInfo Info= new eIVOGo.Published.AlertAdmTurnKeyInfo();
                    
                    if (Info.totalRecordCount() > 0)
                    {
                        try
                        {
                            MailMessage message = new MailMessage();
                            message.ReplyToList.Add(Settings.Default.ReplyTo);
                            message.From = new MailAddress(Settings.Default.WebMaster);

                            message.To.Add(Settings.Default.WebMaster);
                            message.Subject = "電子發票系統 未上傳至TurnKey通知";
                            message.IsBodyHtml = true;
                            using (WebClient wc = new WebClient())
                            {
                                wc.Encoding = Encoding.UTF8;
                                message.Body = wc.DownloadString(String.Format("{0}{1}",
                                    Settings.Default.mailLinkAddress,
                                    VirtualPathUtility.ToAbsolute("~/Published/AlertAdmTurnKeyInfo.aspx")));
                            }

                            SmtpClient smtpclient = new SmtpClient(Settings.Default.MailServer);
                            smtpclient.Credentials = CredentialCache.DefaultNetworkCredentials;
                            smtpclient.Send(message);

                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }    
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }
        public static void Reset()
        {
            __IsNotifyingGovPlatform = false;
            __IsNotifyingClientAlert = false;
        }

        public static bool ThreadSafeCheckEnable(ref bool token)
        {
            var bRun = false;
            lock (typeof(ServiceWorkItem))
            {
                if (!token)
                {
                    bRun = true;
                    token = true;
                }
            }
            return bRun;
        }

        public static void NotifyClientResponseTimeoutAlert()
        {
            if (!__IsNotifyingClientAlert)
            {
                if (ThreadSafeCheckEnable(ref __IsNotifyingClientAlert))
                {
                    ThreadPool.QueueUserWorkItem(info =>
                    {

                        Thread.Sleep(Settings.Default.ClientResponseTimeoutAlertInterval);
                        SystemMonitorControl.BackgroundService.Interrupt();
                        __IsNotifyingClientAlert = false;
                    });
                }
            }
        }
        public class UnassignNOCheckSchedule : IJob
        {

            public DateTime GetScheduleToNextTurn(DateTime current)
            {
                return current.AddMonths(1);
            }

            public void DoJob()
            {
                EIVOPlatformManager mgr = new EIVOPlatformManager();
                doUnassignNOCheck(null, null);
            }

            public void Dispose()
            {

            }
        }
        public class DailyCheckSchedule : IJob
        {

            public DateTime GetScheduleToNextTurn(DateTime current)
            {
                return current.AddMonths(1);
            }

            public void DoJob()
            {
                EIVOPlatformManager mgr = new EIVOPlatformManager();
                doDailyCheck();
            }

            public void Dispose()
            {

            }
        }
        public class TurnKeyCheckSchedule : IJob
        {

            public DateTime GetScheduleToNextTurn(DateTime current)
            {
                return current.AddDays(1);
            }

            public void DoJob()
            {
                EIVOPlatformManager mgr = new EIVOPlatformManager();
                doDailyTurnKeyCheck();
            }

            public void Dispose()
            {

            }
        }
        
        
    }
}