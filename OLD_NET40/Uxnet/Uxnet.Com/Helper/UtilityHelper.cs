using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uxnet.ToolAdapter.Common;
using Uxnet.Com.Properties;
using Uxnet.Com.Helper.DefaultTools;

namespace Uxnet.Com.Helper
{
    public class UtilityHelper
    {
        private static UtilityHelper _instance;
        private IPdfUtility _pdfUtility;
        
        static UtilityHelper() 
        {
            _instance = new UtilityHelper();
        }

        private UtilityHelper() 
        {
            try
            {
                if (!String.IsNullOrEmpty(Settings.Default.IPdfUtilityImpl))
                {
                    Type type = Type.GetType(Settings.Default.IPdfUtilityImpl);
                    if (type.GetInterface("Uxnet.ToolAdapter.Common.IPdfUtility") != null)
                    {
                        _pdfUtility = (IPdfUtility)type.Assembly.CreateInstance(type.FullName);
                    }
                }
            }
            finally
            {
                if (_pdfUtility == null)
                {
                    _pdfUtility = new PdfUtility();
                }
            }
        }

        public static IPdfUtility GetPdfUtility()
        {
            return _instance._pdfUtility;
        }
    }
}
