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
using Model.DataEntity;
using Model.SCMDataEntity;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;
using System.Linq.Expressions;
using Utility;
using eIVOGo.Properties;

namespace eIVOGo.Module.SCM
{
    public partial class PurchaseOrderVerify : System.Web.UI.UserControl, IPostBackEventHandler
    {
        UserProfileMember _userProfile;

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
            var mgr = this.dsPurchase.CreateDataManager();
            this.ddlWarehouse.Items.AddRange(mgr.GetTable<WAREHOUSE>().Select(wh => new ListItem(wh.WAREHOUSE_NAME, wh.WAREHOUSE_SN.ToString())).ToArray());
            this.ddlSupplier.Items.AddRange(mgr.GetTable<SUPPLIER>().Select(s => new ListItem(s.SUPPLIER_NAME, s.SUPPLIER_SN.ToString())).ToArray());
        }

        protected override void OnInit(EventArgs e)
        {
            this.PreRender+=new EventHandler(PurchaseOrderVerify_PreRender);
            this.dsPurchase.Select+=new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PURCHASE_ORDER>>(dsPurchase_Select);
        }

        void dsPurchase_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PURCHASE_ORDER> e)
        {
            if (!String.IsNullOrEmpty(this.btnQuery.CommandArgument))
            {
                var mgr = this.dsPurchase.CreateDataManager();

                IQueryable<PURCHASE_ORDER> po = mgr.EntityList.Where(p => p.PO_DELETE_STATUS == 0 & p.CDS_Document.CurrentStep==(int)Naming.DocumentStepDefinition.待審核);

                if (this.ddlWarehouse.SelectedIndex != 0)
                {
                    po = po.Where(p => p.WAREHOUSE.WAREHOUSE_SN == long.Parse(this.ddlWarehouse.SelectedValue));
                }

                if (this.ddlSupplier.SelectedIndex != 0)
                {
                    po = po.Where(p => p.SUPPLIER.SUPPLIER_SN == long.Parse(this.ddlSupplier.SelectedValue));
                }

                if (!string.IsNullOrEmpty(this.txtPurchaseNO.Text))
                {
                    po = po.Where(p => p.PURCHASE_ORDER_NUMBER.Equals(this.txtPurchaseNO.Text));
                }

                if (!string.IsNullOrEmpty(this.txtBarCode.Text))
                {
                    po = po.Where(p => p.PURCHASE_ORDER_DETAILS.Where(pod => pod.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_BARCODE.Equals(this.txtBarCode.Text)).Count() > 0);
                }

                if (!string.IsNullOrEmpty(this.txtProdName.Text))
                {
                    po = po.Where(p => p.PURCHASE_ORDER_DETAILS.Where(pod => pod.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_NAME.Contains(this.txtProdName.Text)).Count() > 0);
                }

                if (!string.IsNullOrEmpty(this.DateFrom.TextBox.Text) && !string.IsNullOrEmpty(this.DateTo.TextBox.Text))
                {
                    po = po.Where(p => p.PO_DATETIME.Value >= this.DateFrom.DateTimeValue & p.PO_DATETIME.Value <= this.DateTo.DateTimeValue.AddDays(1));
                }

                e.Query = po;
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
            this.gvEntity.DataBind();
            if (this.gvEntity.Rows.Count > 0)
            {
                this.countTable.Visible = true;
                this.btnVerify.Visible = true;
                this.lblError.Visible = false;
            }
            else
            {
                this.lblError.Text = "查無資料!!";
                this.lblError.Visible = true;
                this.countTable.Visible = false;
                this.btnVerify.Visible = false;
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            string[] ar = Request.Form.GetValues("chkItem");
            if (ar != null && ar.Count() > 0)
            {
                foreach (string item in ar)
                {
                    Page.Items["PONO"] = item;
                    Server.Transfer("previewPurchaseOrder.aspx");
                }
            }
            else
            {
                 this.AjaxAlert("請選擇審核資料!!");
            }            
        }
        #endregion

        #region "Gridview Event"
        protected void PageIndexChanged(object source, PageChangedEventArgs e)
        {
            gvEntity.PageIndex = e.NewPageIndex;
            gvEntity.DataBind();
        }

        void PurchaseOrderVerify_PreRender(object sender, EventArgs e)
        {
            if (gvEntity.BottomPagerRow != null)
            {
                PagingControl paging = (PagingControl)gvEntity.BottomPagerRow.Cells[0].FindControl("pagingList");
                paging.RecordCount = this.dsPurchase.CurrentView.LastSelectArguments.TotalRowCount;
                paging.CurrentPageIndex = gvEntity.PageIndex;
            }
        }
        #endregion

        #region IPostBackEventHandler Members
        public void RaisePostBackEvent(string eventArgument)
        { }
        #endregion
    }    
}
