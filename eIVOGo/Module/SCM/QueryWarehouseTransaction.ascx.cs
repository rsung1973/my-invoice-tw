using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Locale;
using Model.Security.MembershipManagement;
using Model.InvoiceManagement;
using Business.Helper;
using Model.SCMDataEntity;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Utility;
using System.Data.Linq;

namespace eIVOGo.Module.SCM
{
    public partial class QueryWarehouseTransaction : System.Web.UI.UserControl
    {
        UserProfileMember _userProfile;

        public class _QueryItem
        {
            public String WAREHOUSE;
            public String PRODUCTSBARCODE;
            public String PRODUCTSNAME;
            public String INBOUNDQUANTITY;
            public String OUTBOUNDQUANTITY;
            public String DEFECTIVEQUANTITY;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (!Page.IsPostBack)
            {
                initializeData();
            }
        }

        private void initializeData()
        {
            var mgr = this.dsEntity.CreateDataManager();
            this.ddlWarehouse.Items.AddRange(mgr.GetTable<WAREHOUSE>().Select(wh => new ListItem(wh.WAREHOUSE_NAME, wh.WAREHOUSE_SN.ToString())).ToArray());
            this.DateFrom.DateTimeValue = DateTime.Now;
            this.DateTo.DateTimeValue = DateTime.Now;
        }

        protected override void OnInit(EventArgs e)
        {
            this.PreRender+=new EventHandler(PurchaseOrderMangement_PreRender);
            //this.dsEntity.Select+=new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<WAREHOUSE_PRODUCTS_TRANSACTION_LOG>>(dsEntity_Select);
        }

        protected void BindData()
        {
            if (!String.IsNullOrEmpty(this.btnQuery.CommandArgument))
            {
                var mgr = this.dsEntity.CreateDataManager();
                Expression<Func<WAREHOUSE_PRODUCTS_TRANSACTION_LOG, bool>> queryExpr = w => true;

                queryExpr = queryExpr.And(w => w.IN_OUTBOUND_DATETIME.Value >= this.DateFrom.DateTimeValue & w.IN_OUTBOUND_DATETIME.Value <= this.DateTo.DateTimeValue);
                
                if (this.ddlWarehouse.SelectedIndex != 0)
                {
                    queryExpr = queryExpr.And(w => w.WAREHOUSE_SN == long.Parse(this.ddlWarehouse.SelectedValue));
                }

                if (!string.IsNullOrEmpty(this.txtBarCode.Text))
                {
                    int prodSN = mgr.GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_BARCODE.Equals(this.txtBarCode.Text)).FirstOrDefault().PRODUCTS_SN;
                    queryExpr = queryExpr.And(w => w.PRODUCTS_SN == prodSN);
                }

                if (!string.IsNullOrEmpty(this.txtProdName.Text))
                {
                    var podSN = mgr.GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_NAME.Contains(this.txtProdName.Text)).Select(p => p.PRODUCTS_SN).ToArray();
                    queryExpr = queryExpr.And(w => podSN.Contains((int)w.PRODUCTS_SN));
                }

                IQueryable<WAREHOUSE_PRODUCTS_TRANSACTION_LOG> pw = mgr.EntityList.Where(queryExpr);
                _QueryItem detail = null;
                var data = pw.Select(d => new { d.PRODUCTS_SN, d.WAREHOUSE_SN }).Distinct();
                foreach (var d in data)
                {
                    detail = new _QueryItem
                                 {
                                     WAREHOUSE = mgr.GetTable<WAREHOUSE>().Where(w => w.WAREHOUSE_SN == d.WAREHOUSE_SN).FirstOrDefault().WAREHOUSE_NAME,
                                     PRODUCTSBARCODE = mgr.GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_SN == d.PRODUCTS_SN).FirstOrDefault().PRODUCTS_BARCODE,
                                     PRODUCTSNAME = mgr.GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_SN == d.PRODUCTS_SN).FirstOrDefault().PRODUCTS_NAME,
                                     INBOUNDQUANTITY = pw.Where(wp => wp.PRODUCTS_SN == d.PRODUCTS_SN & wp.WAREHOUSE_SN == d.WAREHOUSE_SN).Sum(wp => wp.INBOUND_QUANTITY).ToString(),
                                     OUTBOUNDQUANTITY = pw.Where(wp => wp.PRODUCTS_SN == d.PRODUCTS_SN & wp.WAREHOUSE_SN == d.WAREHOUSE_SN).Sum(wp => wp.OUTBOUND_QUANTITY).ToString(),
                                     DEFECTIVEQUANTITY = pw.Where(wp => wp.PRODUCTS_SN == d.PRODUCTS_SN & wp.WAREHOUSE_SN == d.WAREHOUSE_SN).Sum(wp => wp.DEFECTIVE_QUANTITY).ToString()
                                 };
                }

                this.gvEntity.DataSource = detail;
                this.gvEntity.DataBind();
                
                this.lblRowCount.Text = pw.Count().ToString();
            }
        }
        
        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<WAREHOUSE_PRODUCTS_TRANSACTION_LOG> e)
        {
            if (!String.IsNullOrEmpty(this.btnQuery.CommandArgument))
            {
                var mgr = this.dsEntity.CreateDataManager();

                IQueryable<WAREHOUSE_PRODUCTS_TRANSACTION_LOG> pw = mgr.EntityList.Where(p => p.IN_OUTBOUND_DATETIME.Value >= this.DateFrom.DateTimeValue & p.IN_OUTBOUND_DATETIME.Value <= this.DateTo.DateTimeValue);

                if (this.ddlWarehouse.SelectedIndex != 0)
                {
                    pw = pw.Where(p => p.WAREHOUSE_SN == long.Parse(this.ddlWarehouse.SelectedValue));
                }

                if (!string.IsNullOrEmpty(this.txtBarCode.Text))
                {
                    int prodSN = mgr.GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_BARCODE.Equals(this.txtBarCode.Text)).FirstOrDefault().PRODUCTS_SN;
                    pw = pw.Where(p => p.PRODUCTS_SN == prodSN);
                }

                if (!string.IsNullOrEmpty(this.txtProdName.Text))
                {
                    var podSN = mgr.GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_NAME.Contains(this.txtProdName.Text)).Select(p => p.PRODUCTS_SN).ToArray();
                    pw = pw.Where(p => podSN.Contains((int)p.PRODUCTS_SN));
                }

                e.Query = pw;

                this.lblRowCount.Text = pw.Count().ToString();
            }
            else
            {
                e.QueryExpr = p => false;
            }
        }

        #region "Button Event"
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.divResult.Visible = true;
            this.btnQuery.CommandArgument = "Query";
            BindData();
            //gvEntity.DataBind();
            if (this.gvEntity.Rows.Count > 0)
            {
                this.countTable.Visible = true;
                this.lblError.Visible = false;
            }
            else
            {
                this.lblError.Text = "查無資料!!";
                this.lblError.Visible = true;
                this.countTable.Visible = false;
            }
        }
        #endregion

        #region "Gridview Event"
        protected void PageIndexChanged(object source, PageChangedEventArgs e)
        {
            gvEntity.PageIndex = e.NewPageIndex;
            BindData();
            //gvEntity.DataBind();
        }

        void PurchaseOrderMangement_PreRender(object sender, EventArgs e)
        {
            if (gvEntity.BottomPagerRow != null)
            {
                PagingControl paging = (PagingControl)gvEntity.BottomPagerRow.Cells[0].FindControl("pagingList");
                paging.RecordCount = this.dsEntity.CurrentView.LastSelectArguments.TotalRowCount;
                paging.CurrentPageIndex = gvEntity.PageIndex;
                this.lblTotalSum.Text = paging.PageCount.ToString();
            }
        }
        #endregion
    }    
}
