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
    public partial class TurnKeyLogList : System.Web.UI.UserControl
    {
        protected IQueryable<V_Logs> _queryItems;
        //protected int? _totalRecordCount;


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
            string TC=string.Empty;
            string no = string.Empty;
            if (String.IsNullOrEmpty(invoiceNo.Text))
            {
                this.AjaxAlert("請輸入發票／折讓單號碼號碼!");
                return;
            }
            
                TC = invoiceNo.Text.Substring(0, 2);
                no =invoiceNo.Text.Substring(2);
                
            divResult.Visible = true;
            Expression<Func<Model.DataEntity.V_Logs, bool>> queryExpr =g => g.TrackCode.Equals(TC);
            queryExpr=queryExpr.And(g=>g.No.Equals(no));
            if (SearchItem.SelectedIndex != 0)
                queryExpr = queryExpr.And(g => g.DocType.Equals(SearchItem.SelectedValue));
            if (DateFrom.HasValue)
            {
                queryExpr = queryExpr.And(g => g.InvoiceDate.Value >= DateFrom.DateTimeValue);
            }
            if (DateTo.HasValue)
            {
                queryExpr = queryExpr.And(g => g.InvoiceDate.Value < DateTo.DateTimeValue.AddDays(1));
            }
            
                if (MESSAGEDTSFrom.HasValue)
            {
                queryExpr = queryExpr.And(g =>g.MESSAGE_DTS >= MESSAGEDTSFrom.DateTimeValue);
            }
            if (MESSAGEDTSTo.HasValue)
            {
                queryExpr = queryExpr.And(g => g.MESSAGE_DTS <  MESSAGEDTSTo.DateTimeValue.AddDays(1));
            }
       

            var mgr = dsInv.CreateDataManager();
            _queryItems = mgr.GetTable<V_Logs>().Where(queryExpr).OrderByDescending(g=>g.MESSAGE_DTS);
           
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



        protected void btnQuery_Click(object sender, EventArgs e)
        {
            bindData();
        }

    }
}