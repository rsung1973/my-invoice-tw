using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.DataEntity;
using Model.Helper;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.EIVO
{
    public partial class ExportInvoiceItemCA : ExportInvoiceItem
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            signContext.BeforeSign += new EventHandler(signContext_BeforeSign);
            signContext.Launcher = btnExport;
        }

        void signContext_BeforeSign(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder("您欲下載傳送大平台");
            sb.Append(rbType.SelectedItem.Text).Append("\r\n");
            sb.Append("日期區間:").Append(DateFrom.HasValue ? ValueValidity.ConvertChineseDateString(DateFrom.DateTimeValue) : "")
                .Append(" ~ ").Append(DateTo.HasValue ? ValueValidity.ConvertChineseDateString(DateTo.DateTimeValue) : "").Append("\r\n");

            var mgr = dsEntity.CreateDataManager();
            var table = mgr.GetTable<DocumentDispatch>();
            IQueryable<DocumentDispatch> items = null;

            switch (rbType.SelectedIndex)
            {
                case 0:
                    items = buildInvoiceQuery(table);
                    sb.Append("發票號碼如下:\r\n");
                    foreach (var i in items)
                    {
                        sb.Append(i.CDS_Document.InvoiceItem.TrackCode).Append(i.CDS_Document.InvoiceItem.No).Append("\r\n");
                    }
                    break;
                case 1:
                    items = buildAllowanceQuery(table);
                    sb.Append("折讓單號碼如下:\r\n");
                    foreach (var i in items)
                    {
                        sb.Append(i.CDS_Document.InvoiceAllowance.AllowanceNumber).Append("\r\n");
                    }
                    break;
                case 2:
                    items = buildInvoiceCancellationQuery(table);
                    sb.Append("作廢發票號碼如下:\r\n");
                    foreach (var i in items)
                    {
                        sb.Append(i.CDS_Document.InvoiceItem.InvoiceCancellation.CancellationNo).Append("\r\n");
                    }
                    break;
                case 3:
                    items = buildAllowanceCancellationQuery(table);
                    sb.Append("作廢折讓單號碼如下:\r\n");
                    foreach (var i in items)
                    {
                        sb.Append(i.CDS_Document.InvoiceAllowance.AllowanceNumber).Append("\r\n");
                    }
                    break;
            }

            if (items != null && items.Count() > 0)
            {
                signContext.DataToSign = sb.ToString();
            }
        }

        protected override void btnExport_Click(object sender, EventArgs e)
        {
            if (signContext.Verify())
            {
                switch (rbType.SelectedIndex)
                {
                    case 0:
                        downloadInvoices();
                        break;
                    case 1:
                        downloadAllowances();
                        break;
                    case 2:
                        downloadInvoiceCancellations();
                        break;
                    case 3:
                        downloadAllowanceCancellations();
                        break;
                }
            }
        }
        
    }
}