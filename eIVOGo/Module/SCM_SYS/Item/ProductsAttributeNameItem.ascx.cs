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

namespace eIVOGo.Module.SCM_SYS.Item
{
    public partial class ProductsAttributeNameItem : System.Web.UI.UserControl
    {
        public event EventHandler Done;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.dsEntity.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PRODUCTS_ATTRIBUTE_NAME>>(dsEntity_Select);
        }

        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PRODUCTS_ATTRIBUTE_NAME> e)
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

        public Expression<Func<PRODUCTS_ATTRIBUTE_NAME, bool>> QueryExpr
        { get; set; }

        public void Show()
        {
            dvEntity.ChangeMode(DetailsViewMode.Insert);
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
                var mgr = dsEntity.CreateDataManager();
                PRODUCTS_ATTRIBUTE_NAME item = new PRODUCTS_ATTRIBUTE_NAME
                {
                    PRODUCTS_ATTR_NAME = (String)e.Values["PRODUCTS_ATTR_NAME"]
                };
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
                var item = mgr.EntityList.Where(r => r.PRODUCTS_ATTR_NAME_SN == (int)e.Keys[0]).First();
                item.PRODUCTS_ATTR_NAME = (String)e.NewValues["PRODUCTS_ATTR_NAME"];
                mgr.SubmitChanges();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert(String.Format("修改資料失敗,原因:{0}", ex.Message));
            }
            
        }
    }
}