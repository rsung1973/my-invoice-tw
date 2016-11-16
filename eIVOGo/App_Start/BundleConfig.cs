using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace eIVOGo
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/App_Themes/Visitor").Include("~/App_Themes/Visitor/*.css"));
            bundles.Add(new StyleBundle("~/App_Themes/NewPrint").Include("~/App_Themes/NewPrint/*.css"));
        }
    }
}
