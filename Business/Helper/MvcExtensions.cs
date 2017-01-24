using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Business.Helper
{
    public static class MvcExtensions
    {
        public static String ErrorMessage(this ModelStateDictionary modelState)
        {
            return String.Join("、", modelState.Keys.Where(k => modelState[k].Errors.Count > 0)
                    .Select(k => /*k + " : " +*/ String.Join("/", modelState[k].Errors.Select(r => r.ErrorMessage))));
        }

        public static String HtmlBreakLine(this String strVal)
        {
            if (strVal != null)
            {
                return strVal.Replace("\r\n", "<br/>");
            }
            return null;
        }
    }
}
