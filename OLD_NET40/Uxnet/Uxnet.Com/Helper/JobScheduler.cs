using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Utility;
using System.Xml;

namespace Uxnet.Com.Helper
{
    public partial class JobScheduler : IDisposable
    {
        private static JobScheduler _instance;
        private static String _JobFileName;

        private Timer _timer;
        private List<JobItem> _jobItems;

        static JobScheduler()
        {
            _JobFileName = Path.Combine(Logger.LogPath, "JobScheduler.xml");
        }

        private JobScheduler(int period)
        {
            initialize();
            _timer = new Timer(run, null, period, period);
        }

        private void initialize()
        {
            if (File.Exists(_JobFileName))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_JobFileName);
                _jobItems = doc.ConvertTo<List<JobItem>>();
            }
            else
            {
                _jobItems = new List<JobItem>();
            }
        }

        private void run(Object state)
        {
            DateTime now = DateTime.Now;
            bool changed = false;
            foreach (var item in _jobItems)
            {
                if (item.Schedule <= now)
                {
                    try
                    {
                        doJob(item);
                        item.LastError = null;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        item.LastError = ex.ToString();
                    }
                    changed = true;
                }
            }
            if (changed)
            {
                saveJob();
            }
        }

        private void saveJob()
        {
            _jobItems.ConvertToXml().Save(_JobFileName);
        }

        private void doJob(JobItem item, bool nextSchedule = true)
        {
            var type = Type.GetType(item.AssemblyQualifiedName);
            IJob job = (IJob)type.Assembly.CreateInstance(type.FullName);
            job.DoJob();
            if (nextSchedule)
                item.Schedule = job.GetScheduleToNextTurn(item.Schedule);
            job.Dispose();
        }

        public static void StartUp(int period = 5*60000)
        {
            lock (typeof(JobScheduler))
            {
                if (_instance == null)
                {
                    _instance = new JobScheduler(period);
                }
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
            _jobItems.Clear();
        }

        public static void Reset()
        {
            lock (typeof(JobScheduler))
            {
                if (_instance != null)
                {
                    _instance.Dispose();
                    _instance = null;
                }
            }
        }

        public static bool AddJob(JobItem item)
        {
            if (_instance == null)
                StartUp();
            var type = Type.GetType(item.AssemblyQualifiedName);
            if (type.GetInterface("Uxnet.Com.Helper.IJob") != null)
            {
                _instance._jobItems.Add(item);
                _instance.saveJob();
                return true;
            }
            return false;
        }

        public static void RemoveJob(JobItem item)
        {
            if (_instance != null && _instance._jobItems.Remove(item))
            {
                _instance.saveJob();
            }
        }

        public static JobItem[] JobList
        {
            get
            {
                if (_instance != null)
                    return _instance._jobItems.ToArray();
                return null;
            }
        }

        public static void LaunchJob(JobItem item)
        {
            if (_instance != null)
                _instance.doJob(item, false);
        }

    }

    public class JobItem 
    {
        public DateTime Schedule { get; set; }
        public String AssemblyQualifiedName { get; set; }
        public String Description { get; set; }
        public String LastError { get; set; }
    }

    public interface IJob : IDisposable
    {
        DateTime GetScheduleToNextTurn(DateTime current);
        void DoJob();
    }
}
