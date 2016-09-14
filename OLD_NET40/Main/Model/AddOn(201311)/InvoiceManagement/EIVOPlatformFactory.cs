using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading;
using System.Xml;

using Model.DataEntity;
using Model.Properties;
using Utility;
using System.Security.Cryptography.X509Certificates;
using Uxnet.Com.Security.UseCrypto;
using Model.Helper;
using Model.Locale;

namespace Model.InvoiceManagement
{
    public static partial class EIVOPlatformFactory
    {
        public static String DefaultUserCarrierType
        {
            get;
            set;
        }
    }
}
