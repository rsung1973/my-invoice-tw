using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Utility
{
    public class UtilityConfig
    {
        private static UtilityConfig _instance = new UtilityConfig();

        private string _logPath;

        private UtilityConfig()
        {
            _logPath = Path.Combine(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory), "logs");
            if (!Directory.Exists(_logPath))
                Directory.CreateDirectory(_logPath);
        }

        public static string LogPath
        {
            get
            {
                return _instance._logPath;
            }
        }

        public static String CurrentStorePath
        {
            get
            { 
                String path = ValueValidity.GetDateStylePath(_instance._logPath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

    }
}
