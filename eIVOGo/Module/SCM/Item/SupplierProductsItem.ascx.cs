using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.SCMDataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SCM.Item
{
    public partial class SupplierProductsItem : System.Web.UI.UserControl
    {
        public event EventHandler Done;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.dsEntity.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<SUPPLIER_PRODUCTS_NUMBER>>(dsEntity_Select);
            dsProd.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PRODUCTS_DATA>>(dsProd_Select);
        }

        void dsProd_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PRODUCTS_DATA> e)
        {
            e.QueryExpr = p => p.PRODUCTS_SN == PRODUCTS_SN;
        }

        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<SUPPLIER_PRODUCTS_NUMBER> e)
        {
            if (QueryExpr != null)
            {
                e.QueryExpr = QueryExpr;
            }
            else
            {
                e.QueryExpr = r => false;
            }
        }

        public int? PRODUCTS_SN
        {
            get;
            set;
        }

        public Expression<Func<SUPPLIER_PRODUCTS_NUMBER, bool>> QueryExpr
        { get; set; }

        public void Show()
        {
            dvEntity.ChangeMode(DetailsViewMode.Insert);
            dvEntity.DataBind();
            this.ModalPopupExtender.Show();
        }

        public void BindData()
        {
            dvEntity.ChangeMode(DetailsViewMode.Edit);
            dvEntity.DataBind();
            this.ModalPopupExtender.Show();

        }

        protected void dvEntity_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                this.ModalPopupExtender.Hide();
            }
        }

        protected void dvEntity_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            try
            {
                DropDownList supplier = (DropDownList)dvEntity.Rows[1].FindControl("SUPPLIER_SN");
                if (String.IsNullOrEmpty(supplier.SelectedValue))
                {
                    this.AjaxAlert("請先建立供應商資料!!");
                    return;
                }

                DropDownList product = (DropDownList)dvEntity.Rows[1].FindControl("PRODUCTS_SN");
                if (String.IsNullOrEmpty(product.SelectedValue))
                {
                    this.AjaxAlert("請先建立商品資料!!");
                    return;
                }

                var mgr = dsEntity.CreateDataManager();
                SUPPLIER_PRODUCTS_NUMBER item = new SUPPLIER_PRODUCTS_NUMBER
                {
                    SUPPLIER_SN = int.Parse(supplier.SelectedValue),
                    PRODUCTS_SN = int.Parse(product.SelectedValue),
                    SUPPLIER_PRODUCTS_NUMBER1 = (String)e.Values["SUPPLIER_PRODUCTS_NUMBER1"]
                };
                if (mgr.EntityList.Count(s => s.SUPPLIER_SN == item.SUPPLIER_SN && s.PRODUCTS_SN == item.PRODUCTS_SN) > 0)
                {
                    this.AjaxAlert("該料品已有相同的供應商!!");
                    return;
                }
                mgr.EntityList.InsertOnSubmit(item);
                mgr.SubmitChanges();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert(String.Format("新增資料失敗,原因:{0}", ex.Message));
            }
        }

        protected void dvEntity_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        protected void dvEntity_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        protected void dvEntity_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                var mgr = dsEntity.CreateDataManager();
                var item = mgr.EntityList.Where(r => r.SUPPLIER_SN == (int)e.Keys[0] && r.PRODUCTS_SN==(int)e.Keys[1]).First();
                item.SUPPLIER_PRODUCTS_NUMBER1 = (String)e.NewValues["SUPPLIER_PRODUCTS_NUMBER1"];
                mgr.SubmitChanges();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert(String.Format("修改資料失敗,原因:{0}", ex.Message));
            }
            
        }

        protected void RPODUCTS_SN_PreRender(object sender, EventArgs e)
        {
            if (PRODUCTS_SN.HasValue)
            {
                ((DropDownList)sender).SelectedValue = PRODUCTS_SN.ToString();
            }
        }

        protected void PRODUCTS_SN_DataBound(object sender, EventArgs e)
        {

        }
    }
}