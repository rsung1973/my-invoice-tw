using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Linq.Expressions;
using Model.DataEntity;
using Model.InvoiceManagement;
using Utility;
using Model.Locale;
namespace eIVOGo.Published
{
    public partial class AlertAdmTurnKeyInfo : System.Web.UI.Page
    {
        protected IQueryable<V_Logs> _queryItems;
        protected IQueryable<CDS_Document> _queryInvoiceItems;
     //protected int? _totalRecordCount;
        protected int? _C0401FalseCount;
        protected int? _C0501FalseCount;
       // protected int? _C0701FalseCount;
        protected int? _D0401FalseCount;
        protected int? _D0501FalseCount;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            totalRecordCount();
        }



        public  int? totalRecordCount()
        {

            Expression<Func<Model.DataEntity.V_Logs, bool>> queryExpr = o => o.STATUS.Equals("C");//成功筆數
            var mgr = new V_LogsDataSource().CreateDataManager();
            var querymgr =  new InvoiceManager();
            IQueryable<CDS_Document> _queryInvoiceItems = querymgr.GetTable<CDS_Document>();
            
            //queryExpr = queryExpr.And(g => g.InvoiceDate.Value == DateTime.Today);

            //_queryInvoiceItems = _queryInvoiceItems.Where(i => i.DocDate == DateTime.Today);

            _C0401FalseCount = querymgr.GetTable<InvoiceItem>().Where(i => i.CDS_Document.DocType == (int)Naming.DocumentTypeDefinition.E_Invoice)
                .Where(d=> d.InvoiceDate >= DateTime.Today.AddMonths(-2))
                .Select(d => d.TrackCode + d.No).ToList()
                //_queryInvoiceItems.Where(d => d.DocType == (int)Naming.DocumentTypeDefinition.E_Invoice)
                //.Where(d=>d.DocDate>=DateTime.Today.AddMonths(-2) ||d.InvoiceItem.InvoiceDate>=DateTime.Today.AddMonths(-2))
                //.Select(d => d.InvoiceItem.TrackCode + d.InvoiceItem.No).ToList()
                .Except(mgr.GetTable<V_Logs>().
                Where(queryExpr.And(v => v.DocType.Equals("C0401"))
                .And(v => v.MESSAGE_DTS >= DateTime.Today.AddMonths(-2) || v.InvoiceDate >= DateTime.Today.AddMonths(-2)))
                .Select(v => v.TrackCode + v.No).ToList()).Count();

            _C0501FalseCount = 
                _queryInvoiceItems.Where(d => d.DocType == (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation)
                 .Join(querymgr.GetTable<InvoiceItem>().Where(d =>  d.InvoiceDate >= DateTime.Today.AddMonths(-2))
                 , c => c.DerivedDocument.SourceID, i => i.InvoiceID, (c, i) => i)
               
                 .Select(d => d.TrackCode + d.No).ToList()
                .Except(mgr.GetTable<V_Logs>().
                Where(queryExpr.And(v => v.DocType.Equals("C0501"))
                    .And(v => v.MESSAGE_DTS >= DateTime.Today.AddMonths(-2)))
                .Select(v => v.TrackCode + v.No).ToList()).Count();

            _D0401FalseCount = _queryInvoiceItems.Where(d => d.DocType == (int)Naming.DocumentTypeDefinition.E_Allowance)
                .Where(d => d.InvoiceAllowance.AllowanceDate >= DateTime.Today.AddMonths(-2))
                .Select(d => d.InvoiceAllowance.AllowanceNumber).ToList()
                .Except(mgr.GetTable<V_Logs>().
                Where(queryExpr.And(v => v.DocType.Equals("D0401"))
                 .And(v => v.MESSAGE_DTS >= DateTime.Today.AddMonths(-2)))
                .Select(v => v.TrackCode + v.No).ToList()).Count();

            _D0501FalseCount=querymgr.GetTable<InvoiceAllowanceCancellation>()
                .Where(d => d.InvoiceAllowance.AllowanceDate >= DateTime.Today.AddMonths(-2) || d.CancelDate >= DateTime.Today.AddMonths(-2))
                .Select(d => d.InvoiceAllowance.AllowanceNumber).ToList()
                .Except(mgr.GetTable<V_Logs>().
                Where(queryExpr.And(v => v.DocType.Equals("D0501"))
                 .And(v => v.MESSAGE_DTS >= DateTime.Today.AddMonths(-2)))
                .Select(v => v.TrackCode + v.No).ToList()).Count();
           return _C0401FalseCount+_C0501FalseCount+_D0401FalseCount+_D0501FalseCount;

        }

    }
}