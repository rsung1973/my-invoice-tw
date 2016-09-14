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

namespace eIVOGo.Module.EIVO
{
    public partial class AllowencePreview : System.Web.UI.UserControl
    {
        protected IQueryable<InvoiceAllowenceItems> _queryItems;
        public decimal tax1 = (decimal)0.05;

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



        internal GridView DataList
        {
            get
            {
                return gvEntity;
            }
        }

        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (this.Request["id"] == null)
            {
                this.AjaxAlert("傳入參數有誤");
            }
            else
            {
                bindData();
            }
            //  this.SignContext1.Launcher = this.btnAllowance;



        }

        protected void gvInvoice_Sorting(object sender, GridViewSortEventArgs e)
        {
            _sortExpression.AddSortExpression(e, true);
            bindData();
        }

        protected void bindData()
        {
            buildQueryItem();

            if (this.ViewState["sort"] != null)
            {
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[0].SortExpression, b => b.invAitem.No);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[1].SortExpression, b => b.Brief );
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[2].SortExpression, b => b.invAitem.Piece);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[3].SortExpression, b => b.UnitCost );
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[4].SortExpression, b => b.invAitem.Amount);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[5].SortExpression, b => b.invAitem.Tax);
                


            }

            if (gvEntity.AllowPaging)
            {
                gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(gvEntity, 0);
                gvEntity.DataSource = _queryItems.Skip(gvEntity.PageSize * gvEntity.PageIndex).Take(gvEntity.PageSize);
                gvEntity.DataBind();

                gvEntity.SetPageIndex("pagingList", _queryItems.Count());
            }
            else
            {
                gvEntity.DataSource = _queryItems;
                gvEntity.DataBind();
            }
        }

        protected virtual void buildQueryItem()
        {
            Expression<Func<InvoiceAllowanceItem , bool>> queryExpr ;
            //設定能顯示的發票為登入者的公司發票
            if(_userProfile.CurrentUserRole.RoleID == 1)
                queryExpr = o => o.InvoiceAllowanceDetail.Any(a => a.AllowanceID == int.Parse(this.Request["id"].ToString())) ;
            else
                queryExpr = o => o.InvoiceAllowanceDetail.Any(a => a.AllowanceID == int.Parse(this.Request["id"].ToString())) && o.InvoiceAllowanceDetail.Any(a => a.InvoiceAllowance.SellerId == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID.ToString());
            //過濾使用者所填入的條件

           

            var mgr = dsEntity.CreateDataManager();
            _queryItems = mgr.GetTable<InvoiceAllowanceItem>().Where(queryExpr).Select(o => new InvoiceAllowenceItems
            {
                invAitem = o,
                UnitCost = o.InvoiceProductItem.UnitCost.HasValue ?  o.InvoiceProductItem.UnitCost.Value : 0 ,
                Brief = o.InvoiceProductItem.InvoiceProduct .Brief 
             });
        }

        public void BindData()
        {
            bindData();
        }

        public class InvoiceAllowenceItems
        {
            public InvoiceAllowanceItem invAitem { get; set; }
            public decimal  UnitCost { get; set; }
            public string Brief { get; set; }
            
        }

    }
}