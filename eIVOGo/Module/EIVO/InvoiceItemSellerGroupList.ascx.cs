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

namespace eIVOGo.Module.EIVO
{
    public partial class InvoiceItemSellerGroupList : System.Web.UI.UserControl
    {
        protected IQueryable<SellerGroupList> _queryItems;

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

        public DateTime? DateFrom
        {
            get
            {
                return (DateTime?)ViewState["from"];
            }
            set
            {
                ViewState["from"] = value;
            }
        }
        public DateTime? DateTo
        {
            get
            {
                return (DateTime?)ViewState["to"];
            }
            set
            {
                ViewState["to"] = value;
            }
        }
        //min-yu add 2011-05-26
        public int? RecordCount
        {
            get
            {
                if (_queryItems == null)
                    return 0;
                else
                    return _queryItems.Count();
            }
        }

        public int? SellerID
        {
            get
            {
                return (int?)ViewState["seller"];
            }
            set
            {
                ViewState["seller"] = value;
            }
        }

        internal GridView DataList
        {
            get
            {
                return gvEntity;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gvInvoice_Sorting(object sender, GridViewSortEventArgs e)
        {
            _sortExpression.AddSortExpression(e, true);
        }

        protected void bindData()
        {
            buildQueryItem();

            if (this.ViewState["sort"] != null)
            {
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[0].SortExpression, b => b.Seller.ReceiptNo);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[1].SortExpression, b => b.Seller.CompanyName);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[2].SortExpression, b => b.Seller.Addr);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[3].SortExpression, b => b.TotalCount);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[4].SortExpression, b => b.Summary);
            }

            if (gvEntity.AllowPaging)
            {
                gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(gvEntity, 0);
                gvEntity.DataSource = _queryItems.Skip(gvEntity.PageSize * gvEntity.PageIndex).Take(gvEntity.PageSize);
                gvEntity.DataBind();

                gvEntity.SetPageIndex("pagingList",_queryItems.Count());
            }
            else
            {
                gvEntity.DataSource = _queryItems;
                gvEntity.DataBind();
            }
        }

        protected virtual void buildQueryItem()
        {
            Expression<Func<InvoiceItem, bool>> queryExpr = i => i.InvoiceCancellation == null;

            if (SellerID.HasValue)
            {
                queryExpr = queryExpr.And(o => o.SellerID == SellerID);
            }
            if (DateFrom.HasValue)
                queryExpr = queryExpr.And(i => i.InvoiceDate >= DateFrom);
            if (DateTo.HasValue)
                queryExpr = queryExpr.And(i => i.InvoiceDate < DateTo.Value.AddDays(1));
            //minyu-0701
            //queryExpr = queryExpr.And(o => o.InvoiceItems.Where(i => i.InvoiceCancellation == null).Count() > 0);

            var mgr = dsOrg.CreateDataManager();
            _queryItems = mgr.GetTable<InvoiceItem>().Where(queryExpr).GroupBy(i => i.SellerID).Select(g =>
                 new SellerGroupList
                    {
                        Seller = mgr.EntityList.Where(o => o.CompanyID == g.Key).First(),
                        TotalCount = g.Count(),
                        Summary = g.Sum(i => i.InvoiceAmountType.TotalAmount)
                    });
        }

        public void BindData()
        {
            bindData();
        }

    }

    public class SellerGroupList
    {
        public Organization Seller { get; set; }
        public int TotalCount { get; set; }
        public decimal? Summary { get; set; }
    }
}