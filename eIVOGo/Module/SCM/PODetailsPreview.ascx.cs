using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.Common;
using Model.Security.MembershipManagement;
using Business.Helper;
using Model.SCMDataEntity;

namespace eIVOGo.Module.SCM
{
    public partial class PODetailsPreview : System.Web.UI.UserControl
    {
        private bool _isSaved;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int? PURCHASE_ORDER_SN
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            dsEntity.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<Model.SCMDataEntity.PURCHASE_ORDER_DETAILS>>(dsEntity_Select);
        }

        protected virtual void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<Model.SCMDataEntity.PURCHASE_ORDER_DETAILS> e)
        {
            e.QueryExpr = p => p.PURCHASE_ORDER_SN == PURCHASE_ORDER_SN;
        }

        public void BindData()
        {
            extendAttributeField();
            gvEntity.DataBind();
        }

        protected virtual void saveAll()
        {
            var mgr = dsEntity.CreateDataManager();
            var items = mgr.EntityList.Where(p => p.PURCHASE_ORDER_SN == PURCHASE_ORDER_SN);
            if (items.Count() > 0)
            {
                foreach (var item in items)
                {
                    String priceText = Request[String.Format("PO_UNIT_PRICE{0}",item.PO_DETAILS_SN)];
                    String quantityText = Request[String.Format("PO_QUANTITY{0}", item.PO_DETAILS_SN)];
                    if (priceText != null && quantityText != null)
                    {
                        item.PO_QUANTITY = int.Parse(quantityText);
                        item.PO_UNIT_PRICE = decimal.Parse(priceText);
                    }
                }
                mgr.SubmitChanges();
            }
        }

        protected void extendAttributeField()
        {
            var mgr = dsEntity.CreateDataManager();
            var nameItems = mgr.GetTable<PRODUCTS_ATTRIBUTE_MAPPING>().Join(mgr.EntityList.Where(p => p.PURCHASE_ORDER_SN == PURCHASE_ORDER_SN).Select(p => p.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA), m => m.PRODUCTS_SN, p => p.PRODUCTS_SN, (m, p) => m.PRODUCTS_ATTRIBUTE_NAME).Distinct();
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

        protected void gvEntity_DataBinding(object sender, EventArgs e)
        {
            if (this.IsPostBack && !_isSaved)
            {
                saveAll();
                _isSaved = true;
            }
        }

    }
}