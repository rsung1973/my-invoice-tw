using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;


namespace eIVOGo.Module.SAM.Handler
{
    public partial class DownloadAppconfig : EditOrganization
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            config=  (XmlDocument)modelItem.DataItem;
            Response.Clear();
            Response.ClearHeaders();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "text/xml";
            Response.AddHeader("Content-Disposition", "attachment;filename=InvoiceClient.exe.config");
            //Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            Response.Write(config.InnerXml.Replace(">", ">" + Environment.NewLine).Replace(";", ";" + Environment.NewLine));
            Response.Flush();
            Response.End();
        }

      
    }
}