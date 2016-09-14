using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Utility;
using Model.DataEntity;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Model.Security.MembershipManagement;
using Business.Helper;
using Uxnet.Web.WebUI;
using Model.InvoiceManagement;
using Model.Locale;

namespace eIVOGo.Module.SAM
{
    public partial class TurnKeyStatusList : System.Web.UI.UserControl
    {
        protected IQueryable<V_Logs> _queryItems;
        protected IQueryable<CDS_Document> _queryInvoiceItems;
        protected int? _totalRecordCount;
        protected int? _FalseCount;
        List<string> Result = new List<string>();
        protected internal Dictionary<String, SortDirection> _sortExpression
        {
            get
            {
                if (ViewState["sort"] == null)
                {
                    ViewState["sort"] = new Dictionary<String, SortDirection>();
                }
                return (Dictionary<String, SortDirection>)ViewState["sort"];
            }
            set
            {
                ViewState["sort"] = value;
            }
        }

        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
           
        }

        protected void bindData()
        {
            try
            {
                buildQueryItem();

 
                _FalseCount = Result.Count();
                pagingList.RecordCount = Result.Count();
                gvEntity.DataSource = Result.Skip(gvEntity.PageSize * pagingList.CurrentPageIndex).Take(gvEntity.PageSize).ToList();
                gvEntity.DataBind();
                if (_FalseCount > 0)
                    tblAction.Visible = true;
                else
                    tblAction.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected virtual void buildQueryItem()
        {

            Expression<Func<Model.DataEntity.V_Logs, bool>> queryExpr = o => o.STATUS.Equals("C");//成功筆數
            var mgr = dsInv.CreateDataManager();
            var querymgr = dsInvoice.CreateDataManager();
            _queryInvoiceItems = querymgr.GetTable<CDS_Document>();

            queryExpr = queryExpr.And(g => g.DocType.Equals(SearchItem.SelectedValue));


            if (DateFrom.HasValue)
            {
                queryExpr = queryExpr.And(g => g.InvoiceDate.Value >= DateFrom.DateTimeValue);
                
            }
            else
            {
                this.AjaxAlert("請輸入發票日期區間之起始日期!");
                return;
            }
            if (DateTo.HasValue)
            {
                queryExpr = queryExpr.And(g => g.InvoiceDate.Value < DateTo.DateTimeValue.AddDays(1));
                
            }
            if (MESSAGEDTSFrom.HasValue)
            {
                queryExpr = queryExpr.And(g => g.MESSAGE_DTS.Value >= MESSAGEDTSFrom.DateTimeValue);
            }

            if (MESSAGEDTSTo.HasValue)
            {
                queryExpr = queryExpr.And(g => g.MESSAGE_DTS.Value < MESSAGEDTSTo.DateTimeValue.AddDays(1));
            }



            _queryItems = mgr.GetTable<V_Logs>().Where(queryExpr);
            _totalRecordCount = _queryItems.Count();
            List<string> InvoiceNo = _queryItems.Select(i => i.TrackCode + i.No).ToList();


            switch (SearchItem.SelectedIndex)
            {
                case 0:
                    _queryInvoiceItems = _queryInvoiceItems.Where(i => i.DocType == (int)Naming.DocumentTypeDefinition.E_Invoice);
                    if (DateFrom.HasValue)
                        _queryInvoiceItems = _queryInvoiceItems.Where(g => g.InvoiceItem.InvoiceDate >= DateFrom.DateTimeValue);
                    if (DateTo.HasValue)
                        _queryInvoiceItems = _queryInvoiceItems.Where(g => g.InvoiceItem.InvoiceDate < DateTo.DateTimeValue.AddDays(1));

                    Result = _queryInvoiceItems.Join(querymgr.GetTable<InvoiceItem>(), c => c.DocID, i => i.InvoiceID, (c, i) => i.TrackCode + i.No).ToList().Except(InvoiceNo).ToList();
                    
                    break;
                case 1://InvoiceNo=折讓單號[AllowanceNumber]
                    _queryInvoiceItems = _queryInvoiceItems.Where(i => i.DocType == (int)Naming.DocumentTypeDefinition.E_Allowance);
                    if (DateFrom.HasValue)
                        _queryInvoiceItems = _queryInvoiceItems.Where(g => g.InvoiceAllowance.AllowanceDate >= DateFrom.DateTimeValue);
                    if (DateTo.HasValue)
                        _queryInvoiceItems = _queryInvoiceItems.Where(g => g.InvoiceAllowance.AllowanceDate < DateTo.DateTimeValue.AddDays(1));

                    Result = _queryInvoiceItems.Select(a=>a.InvoiceAllowance.AllowanceNumber)
                        .ToList().Except(InvoiceNo).ToList();

                    break;
                case 2:
                    _queryInvoiceItems = _queryInvoiceItems.Where(i => i.DocType == (int)Naming.DocumentTypeDefinition.E_Invoice)
                        .Join(querymgr.GetTable<DerivedDocument>() , c => c.DocID, d => d.SourceID, (c, d) => c);
                    if (DateFrom.HasValue)
                        _queryInvoiceItems = _queryInvoiceItems.Where(c => c.InvoiceItem.InvoiceDate >= DateFrom.DateTimeValue);
                    if (DateTo.HasValue)
                        _queryInvoiceItems = _queryInvoiceItems.Where(c =>c.InvoiceItem.InvoiceDate < DateTo.DateTimeValue.AddDays(1));

                    Result = _queryInvoiceItems.Select(c => c.InvoiceItem.TrackCode + c.InvoiceItem.No).ToList().Except(InvoiceNo).ToList();

                    break;
                case 3:
                    var item = querymgr.GetTable<InvoiceAllowanceCancellation>().AsQueryable();
                   // _queryInvoiceItems = _queryInvoiceItems.Where(i => i.DocType == (int)Naming.DocumentTypeDefinition.E_AllowanceCancellation);
                    if (DateFrom.HasValue)
                        item = item.Where(c => c.InvoiceAllowance.AllowanceDate >= DateFrom.DateTimeValue);
                        //_queryInvoiceItems = _queryInvoiceItems.Where(g => g.InvoiceAllowance.InvoiceAllowanceCancellation.CancelDate >= DateFrom.DateTimeValue);
                        if (DateTo.HasValue)
                            item = item.Where(c => c.InvoiceAllowance.AllowanceDate < DateTo.DateTimeValue.AddDays(1));
                        //_queryInvoiceItems = _queryInvoiceItems.Where(g => g.InvoiceAllowance.InvoiceAllowanceCancellation.CancelDate < DateTo.DateTimeValue.AddDays(1));

                        Result = item.Select(a=>a.InvoiceAllowance.AllowanceNumber).ToList().Except(InvoiceNo).ToList();


                    break;


            }
            var invoice = _queryInvoiceItems.Select(i => i.InvoiceItem.TrackCode + i.InvoiceItem.No);
            divResult.Visible = true;



        }

        public void BindData()
        {
          
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(TurnKeyStatusList_PreRender);
        }

        void TurnKeyStatusList_PreRender(object sender, EventArgs e)
        {
            if (this.IsPostBack)
            bindData();
        }
        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvEntity_Sorting(object sender, GridViewSortEventArgs e)
        {
            _sortExpression.AddSortExpression(e, true);
            
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            
        }
        protected void PageIndexChanged(object source, PageChangedEventArgs e)
        {

           
        }
        public String[] GetItemSelection()
        {
            return Request.Form.GetValues("chkItem");
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            String[] ar = GetItemSelection();
            var mgr = dsInvoice.CreateDataManager();
            
            if (ar != null && ar.Count() > 0)
            {
                foreach (var InvoiceId in ar)
                {
                    var item = mgr.GetTable<CDS_Document>().Where(c => c.DocID.Equals(int.Parse(InvoiceId))).FirstOrDefault();
                    if (!item.DocumentDispatches.Any(d => d.TypeID == item.DocType))
                    {
                        item.DocumentDispatches.Add(new DocumentDispatch
                        {
                            TypeID = (int)item.DocType
                        });
                    }
                }

                this.AjaxAlert("資料已重送!!");
            }
            else
            {
                this.AjaxAlert("請選擇重送資料!!");
            }

            mgr.SubmitChanges();
        }
    }
}