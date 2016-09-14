using System;
using System.Collections;
using System.Threading;
using System.IO;
using System.Text;

using Uxnet.Com.Properties;

namespace Utility
{
    /// <summary>
    /// Logger 的摘要描述。
    /// </summary>
    public class Logger : IDisposable
    {
        private static Logger _instance = new Logger();

        private Queue _errQ;
        private Queue _infoQ;
        private Queue _dbgQ;
        private Queue _warnQ;

        private Hashtable _hashQ;

        private bool _disposed = false;
        private Thread _thread;
        private string _path;
        private ulong _fileID = 0;
        private Stream _stream;

        private Logger()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
            if (!String.IsNullOrEmpty(Settings.Default.LogPath))
                _path = Settings.Default.LogPath;
            else
                _path = Path.Combine(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory), "logs");

            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            _errQ = new Queue();
            _infoQ = new Queue();
            _dbgQ = new Queue();
            _warnQ = new Queue();
            _hashQ = new Hashtable();
            _hashQ.Add("err", _errQ);
            _hashQ.Add("nfo", _infoQ);
            _hashQ.Add("dbg", _dbgQ);
            _hashQ.Add("wrn", _warnQ);

            _thread = new Thread(new ThreadStart(this.run));
            _thread.IsBackground = true;
            _thread.Start();
        }

        public static void Shutdown()
        {
            Thread target = _instance._thread;
            _instance._thread = null;
            if (Thread.CurrentThread != target)
            {
                target.Interrupt();
                target.Join();
            }
        }

        public static void Error(object obj)
        {
            _instance._errQ.Enqueue(obj);
            _instance.interrupt();
        }

        public static void Info(object obj)
        {
            _instance._infoQ.Enqueue(obj);
            _instance.interrupt();
        }


        public static void Warn(object obj)
        {
            _instance._warnQ.Enqueue(obj);
            _instance.interrupt();
        }
        public static void Debug(object obj)
        {
            _instance._dbgQ.Enqueue(obj);
            _instance.interrupt();
        }

        public static string LogPath
        {
            get
            {
                return _instance._path;
            }
        }
        
        public static string LogDailyPath
        {
            get
            {
                string filePath = ValueValidity.GetDateStylePath(_instance._path);
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                return filePath;
            }
        }

        private void run()
        {

            while (_thread == Thread.CurrentThread)
            {
                try
                {
                    writeLog();

                    Thread.Sleep(Timeout.Infinite);

                }
                catch (ThreadInterruptedException ex)
                {

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public static void WriteLog()
        {
            _instance.writeLog();
        }

        public static void SetStream(Stream stream)
        {
            _instance._stream = stream;
        }

        private void writeLog()
        {
            foreach (string qName in _hashQ.Keys)
            {
                Queue _q = (Queue)_hashQ[qName];
                while (_q.Count > 0)
                {
                    object obj = _q.Dequeue();
                    string filePath = LogDailyPath;

                    StringBuilder sb = null;

                    if (obj is ILog)
                    {
                        if (obj is ILog2)
                        {
                            filePath = ((ILog2)obj).GetFileName(filePath, qName, _fileID++);
                            sb = new StringBuilder(obj.ToString());
                        }
                        else
                        {
                            filePath = String.Format("{0}\\{1:000000000000}_({3}).{2}", filePath, _fileID++, qName, ((ILog)obj).Subject);
                        }
                    }
                    else
                    {
                        filePath = String.Format("{0}\\SystemLog.{1}", filePath, qName);
                    }

                    using (StreamWriter sw = (_stream == null ? new StreamWriter(filePath, true) : new StreamWriter(_stream)))
                    {
                        if (sb == null)
                        {
                            sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            sw.WriteLine(obj.ToString());
                        }
                        else
                        {
                            sw.WriteLine(sb.ToString());
                        }
                        sw.Flush();
//                        sw.Close();
                    }
                }
            }
        }

        private void interrupt()
        {
            if(_thread!=null)
            {
                _thread.Interrupt();
            }
        }

        
        #region IDisposable 成員

        public void Dispose()
        {
            // TODO:  加入 Logger.Dispose 實作
            dispose(true);
        }

        #endregion

        protected void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine("Object is disposing now ...");
                }
                else
                {
                    Console.WriteLine("May destructor run ...");
                }
                _thread = null;
                writeLog();
            }
            _disposed = true;
        }

        ~Logger()
        {
            dispose(false);
        }

    }
}
