using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utility;

namespace Uxnet.Com.Helper
{
    public partial class LogTextWriter : TextWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        public override void Write(string value)
        {
            Logger.Info(value);
        }

        public override void WriteLine(string value)
        {
            Logger.Info(value);
        }

    }
}
