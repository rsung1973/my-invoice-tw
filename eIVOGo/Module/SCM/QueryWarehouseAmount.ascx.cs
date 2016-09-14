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

namespace eIVOGo.Module.SCM
{
    public partial class QueryWarehouseAmount : System.Web.UI.UserControl
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
            var mgr = this.dsEntity.CreateDataManager();
            this.ddlWarehouse.Items.AddRange(mgr.GetTable<WAREHOUSE>().Select(wh => new ListItem(wh.WAREHOUSE_NAME, wh.WAREHOUSE_SN.ToString())).ToArray());
        }

        protected override void OnInit(EventArgs e)
        {
            this.PreRender+=new EventHandler(PurchaseOrderMangement_PreRender);
            this.dsEntity.Select+=new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PRODUCTS_WAREHOUSE_MAPPING>>(dsEntity_Select);
        }

        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PRODUCTS_WAREHOUSE_MAPPING> e)
        {
            if (!String.IsNullOrEmpty(this.btnQuery.CommandArgument))
            {
                var mgr = this.dsEntity.CreateDataManager();

                IQueryable<PRODUCTS_WAREHOUSE_MAPPING> pw = mgr.EntityList;

                if (this.ddlWarehouse.SelectedIndex != 0)
                {
                    pw = pw.Where(p => p.WAREHOUSE.WAREHOUSE_SN == long.Parse(this.ddlWarehouse.SelectedValue));
                }

                if (!string.IsNullOrEmpty(this.txtBarCode.Text))
                {
                    pw = pw.Where(p => p.PRODUCTS_DATA.PRODUCTS_BARCODE.Equals(this.txtBarCode.Text));
                }

                if (!string.IsNullOrEmpty(this.txtProdName.Text))
                {
                    pw = pw.Where(p => p.PRODUCTS_DATA.PRODUCTS_NAME.Contains(this.txtProdName.Text));
                }

                e.Query = pw;

                this.lblRowCount.Text = pw.Count().ToString();
            }
            else
            {
                e.QueryExpr = p => false;
            }
        }

        public void BindData()
        {
            gvEntity.DataBind();
            extendAttributeField();
        }

        #region "Button Event"
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.divResult.Visible = true;
            this.btnQuery.CommandArgument = "Query";
            BindData();
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

        protected void extendAttributeField()
        {
            var mgr = dsEntity.CreateDataManager();
            var prodSN = mgr.EntityList.Select(b => b.PRODUCTS_SN).ToArray();
            var nameItems = mgr.GetTable<PRODUCTS_ATTRIBUTE_MAPPING>().Where(m => prodSN.Contains(m.PRODUCTS_SN)).Select(m => m.PRODUCTS_ATTRIBUTE_NAME).Distinct();
            int index = gvEntity.Columns.Count;
            foreach (var item in nameItems)
            {
                TemplateField field = new TemplateField
                {
                    HeaderText = item.PRODUCTS_ATTR_NAME
                };

                field.ItemTemplate = new DataFieldViewLoader
                {
                    ControlLoader = this,
                    Field = field
                };

                gvEntity.Columns.Insert(index, field);
                index++;
            }
        }
        #endregion
    }    
}
