using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Uxnet.Web.Module.Common;
using Utility;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SAM
{
    public partial class ExceptionLogList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                initializeData();
            }
            else
                tblAction.Visible = true;
        }

        private void initializeData()
        {
            
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(MemberManager_PreRender);
            dsLog.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<ExceptionLog>>(dsLog_Select);
        }

        void dsLog_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<ExceptionLog> e)
        {
            if (!String.IsNullOrEmpty(btnQuery.CommandArgument))
            {
                Expression<Func<ExceptionLog, bool>> queryExpr = g => true;

                if (DateFrom.HasValue)
                {
                    queryExpr = queryExpr.And(g => g.LogTime >= DateFrom.DateTimeValue);
                }
                if (DateTo.HasValue)
                {
                    queryExpr = queryExpr.And(g => g.LogTime < DateTo.DateTimeValue.AddDays(1));
                }
                if (DocumentType.SelectedIndex > 0)
                {
                    queryExpr = queryExpr.And(g => g.TypeID == int.Parse(DocumentType.SelectedValue));

                }
                if (!String.IsNullOrEmpty(SellerID.SelectedValue))
                {
                    queryExpr = queryExpr.And(g => g.CompanyID == int.Parse(SellerID.SelectedValue));
                }

                e.Query = dsLog.CreateDataManager().EntityList.Where(queryExpr).OrderByDescending(g => g.LogID);

            }
            else
            {
                e.QueryExpr = u => false;
            }
        }

        protected void PageIndexChanged(object source, PageChangedEventArgs e)
        {
            gvEntity.PageIndex = e.NewPageIndex;
            gvEntity.DataBind();
        }

        void MemberManager_PreRender(object sender, EventArgs e)
        {
            if (gvEntity.BottomPagerRow != null)
            {
                PagingControl paging = (PagingControl)gvEntity.BottomPagerRow.Cells[0].FindControl("pagingList");
                paging.RecordCount = dsLog.CurrentView.LastSelectArguments.TotalRowCount;
                paging.CurrentPageIndex = gvEntity.PageIndex;
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            btnQuery.CommandArgument = "Query";
            gvEntity.DataBind();
            
                tblAction.Visible = true;
            
            result.Visible = true;
        }

        public String[] GetItemSelection()
        {
            return Request.Form.GetValues("chkItem");
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            String[] ar = GetItemSelection();
            var mgr = dsLog.CreateDataManager();

            if (ar != null && ar.Count() > 0)
            {
                foreach (var InvoiceId in ar)
                {
                    //var table = mgr.GetTable<ExceptionLog>();
                    var table = mgr.GetTable<ExceptionReplication>();

                    table.InsertOnSubmit(new ExceptionReplication { LogID = int.Parse(InvoiceId) });
                }

                this.AjaxAlert("資料已重送!!");
            }
            else
            {
                this.AjaxAlert("請選擇重送資料!!");
            }

            mgr.SubmitChanges();

            Model.Helper.ExceptionNotification.ProcessNotification();
        }

    }
}