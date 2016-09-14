using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;
using eIVOGo.Properties;
using com.uxb2b.util;

namespace eIVOGo.Published
{
    public partial class InvoiceMailPage : System.Web.UI.Page
    {
        public int? InvoiceID { get; set; }

        protected InvoiceItem _item;
        protected Organization _buyer;

        protected void Page_Load(object sender, EventArgs e)
        {
            int invoiceID;
            if (int.TryParse(Request["id"], out invoiceID))
            {
                InvoiceID = invoiceID;
            }
            else
            {
                String qs = Request.Params["QUERY_STRING"];
                if (!String.IsNullOrEmpty(qs))
                {
                    if (int.TryParse((new CipherDecipherSrv()).decipher(qs), out invoiceID))
                    {
                        InvoiceID = invoiceID;
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(InvoiceMailPage_PreRender);
        }

        void InvoiceMailPage_PreRender(object sender, EventArgs e)
        {
            if (InvoiceID.HasValue)
            {
                var mgr = dsEntity.CreateDataManager();
                _item = mgr.EntityList.Where(i => i.InvoiceID == InvoiceID).FirstOrDefault();
                if (_item != null)
                {
                    mailView.Item = _item;
                    //invRequest.HRef = String.Format("{0}{1}?{2}", Settings.Default.mailLinkAddress, VirtualPathUtility.ToAbsolute("~/Published/RequestInvoicePaper.aspx"), (new CipherDecipherSrv(16)).cipher(_item.InvoiceID.ToString()));
                    this.DataBind();
                }
            }
        }
    }
}