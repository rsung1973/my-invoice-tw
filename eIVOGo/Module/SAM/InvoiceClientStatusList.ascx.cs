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

namespace eIVOGo.Module.SAM
{
    public partial class InvoiceClientStatusList : System.Web.UI.UserControl
    {
        protected IQueryable<OrganizationStatus> _queryItems;

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

                if (this.ViewState["sort"] != null)
                {
                    _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[0].SortExpression, b => b.Organization.CompanyName);
                    _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[1].SortExpression, b => b.LastTimeToAcknowledge);
                    _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[2].SortExpression, b => b.RequestPeriodicalInterval);
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
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected virtual void buildQueryItem()
        {
            Expression<Func<OrganizationStatus, bool>> queryExpr = o => o.LastTimeToAcknowledge.HasValue;

            if (DateFrom.HasValue)
            {
                queryExpr = queryExpr.And(g => g.LastTimeToAcknowledge.Value >= DateFrom.DateTimeValue);
            }
            if (DateTo.HasValue)
            {
                queryExpr = queryExpr.And(g => g.LastTimeToAcknowledge.Value < DateTo.DateTimeValue.AddDays(1));
            }
            if (!String.IsNullOrEmpty(SellerID.SelectedValue))
            {
                queryExpr = queryExpr.And(g => g.CompanyID == int.Parse(SellerID.SelectedValue));
            }

            var mgr = dsEntity.CreateDataManager();
            _queryItems = mgr.GetTable<OrganizationStatus>().Where(queryExpr);
        }

        public void BindData()
        {
            bindData();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        
        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvEntity_Sorting(object sender, GridViewSortEventArgs e)
        {
            _sortExpression.AddSortExpression(e, true);
            bindData();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            bindData();
        }

    }
}