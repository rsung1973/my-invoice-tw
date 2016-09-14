using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

using Utility;

namespace eIVOGo.Module.EIVO.Export
{
    public partial class GoogleInvoiceCSVList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public object DataSource
        {
            get
            {
                return rpExcel.DataSource;
            }
            set
            {
                rpExcel.DataSource = value;
            }
        }

        public void Export(String exportPrefix)
        {
            Response.Clear();
//            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "message/rfc822";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}({1:yyyy-MM-dd}).csv",exportPrefix, DateTime.Today));
            Encoding enc = Encoding.GetEncoding("GB2312");
            Response.HeaderEncoding = enc;

            StringBuilder sb = new StringBuilder();
            rpExcel.DataBind();
            sb.Append(this.GetContent());

            using (StreamWriter sw = new StreamWriter(Response.OutputStream, enc))
            {
                sw.WriteLine(sb.ToString().ToSimplified());
            }

            Response.End();
        }

    }
}