using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using Model.DataEntity;
using Model.Locale;
using Model.Schema.EIVO;
using Utility;

namespace eIVOGo.Module.UI
{
    public partial class UploadDataExceptionNotification : System.Web.UI.UserControl
    {
        protected InvoiceRootInvoice _invoiceItem;
        protected CancelInvoiceRootCancelInvoice _cancelItem;
        protected AllowanceRootAllowance _allowance;
        protected CancelAllowanceRootCancelAllowance _cancelAllowanceItem;

        protected Organization _seller;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Expression<Func<ExceptionLog, bool>> QueryExpr
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(GovPlatformNotification_PreRender);
        }

        protected String getInvoiceContent(String data)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);
                _invoiceItem = doc.ConvertTo<InvoiceRootInvoice>();
                 var mgr = dsEntity.CreateDataManager();
                 _seller = mgr.GetTable<Organization>().Where(o =>o.ReceiptNo == _invoiceItem.SellerId).FirstOrDefault();
                
                return _invoiceItem.InvoiceNumber;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        protected String getCancellationContent(String data)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);
                _cancelItem = doc.ConvertTo<CancelInvoiceRootCancelInvoice>();
                var mgr = dsEntity.CreateDataManager();
                _seller = mgr.GetTable<Organization>().Where(o => o.ReceiptNo == _cancelItem.SellerId).FirstOrDefault();
                
                return _cancelItem.CancelInvoiceNumber;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        protected String getAllowanceContent(String data)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);
                _allowance = doc.ConvertTo<AllowanceRootAllowance>();
                var mgr = dsEntity.CreateDataManager();
                _seller = mgr.GetTable<Organization>().Where(o => o.ReceiptNo == _allowance.SellerId).FirstOrDefault();
                
                return _allowance.AllowanceNumber;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        protected String getCancellationAllowanceContent(String data)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);
                _cancelAllowanceItem = doc.ConvertTo<CancelAllowanceRootCancelAllowance>();
                var mgr = dsEntity.CreateDataManager();
                _seller = mgr.GetTable<Organization>().Where(o => o.ReceiptNo == _cancelAllowanceItem.SellerId).FirstOrDefault();
                
                return _cancelAllowanceItem.CancelAllowanceNumber;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        void GovPlatformNotification_PreRender(object sender, EventArgs e)
        {
            var mgr = dsEntity.CreateDataManager();
            IQueryable<ExceptionLog> logItems = QueryExpr != null ? mgr.GetTable<ExceptionLog>().Where(QueryExpr) : mgr.GetTable<ExceptionLog>();
            var invItems = logItems.Where(r => r.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice);
            gvInvoice.DataSource = invItems;
            gvInvoice.DataBind();

            var cancelItems = logItems.Where(r => r.TypeID == (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation);
            gvCancel.DataSource = cancelItems;
            gvCancel.DataBind();

            var allowanceItems = logItems.Where(r => r.TypeID == (int)Naming.DocumentTypeDefinition.E_Allowance);
            gvAllowance.DataSource = allowanceItems;
            gvAllowance.DataBind();

            var cancelAllowanceItems = logItems.Where(r => r.TypeID == (int)Naming.DocumentTypeDefinition.E_AllowanceCancellation);
            gvCancelAllowance.DataSource = cancelAllowanceItems;
            gvCancelAllowance.DataBind();
        }
    }
}