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
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SCM
{
    public partial class PODetailsEditList : System.Web.UI.UserControl
    {
        protected SUPPLIER_PRODUCTS_NUMBER _currentItem;

        public IList<PURCHASE_ORDER_DETAILS> Items
        {
            get
            {
                return gvEntity.DataSource as IList<PURCHASE_ORDER_DETAILS>;
            }
            set
            {
                gvEntity.DataSource = value;
            }
        }

        //public int? PURCHASE_ORDER_SN
        //{
        //    get;
        //    set;
        //}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender+=new EventHandler(PODetailsEditList_PreRender);
            doDelete.DoAction = arg =>
            {
                delete(int.Parse(arg));
            };
        }

        void PODetailsEditList_PreRender(object sender, EventArgs e)
        {
            if (Items != null)
            {
                BindData();
            }
        }

        protected SUPPLIER_PRODUCTS_NUMBER loadItem(PURCHASE_ORDER_DETAILS item)
        {
            var mgr = dsEntity.CreateDataManager();
            _currentItem = mgr.GetTable<SUPPLIER_PRODUCTS_NUMBER>().Where(sp => sp.PRODUCTS_SN == item.PRODUCTS_SN).FirstOrDefault();
            item.SUPPLIER_PRODUCTS_NUMBER = _currentItem;
            //item.SALES_PROMOTION_PRODUCTS = mgr.GetTable<SALES_PROMOTION_PRODUCTS>().Where(p => p.SALES_PROMOTION_SN == item.SP_PRODUCTS_SN).FirstOrDefault();
            return _currentItem;
        }

        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            { 
                case "Modify":
                    break;
                case "Delete":
                    delete(int.Parse((String)e.CommandArgument));
                    break;
                case "Create":
                    break;
            }
        }

        private void delete(int key)
        {
            Items.RemoveAt(key);
        }

        public void BindData()
        {
            extendAttributeField();
            gvEntity.DataBind();
        }

        public Boolean UpdateData()
        {
            var mgr = dsEntity.CreateDataManager();
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                String priceText = Request[String.Format("PO_UNIT_PRICE{0}", i)];
                String quantityText = Request[String.Format("PO_QUANTITY{0}", i)];
                if (int.Parse(quantityText) != 0)
                {
                    if (item.PURCHASE_ORDER_SN != 0)
                    {
                        if (priceText != null && quantityText != null)
                        {
                            var det = mgr.GetTable<PURCHASE_ORDER_DETAILS>().Where(pd => pd.PURCHASE_ORDER_SN == item.PURCHASE_ORDER_SN & pd.PO_DETAILS_SN == item.PO_DETAILS_SN).FirstOrDefault();
                            det.PO_QUANTITY = int.Parse(quantityText);
                            det.PO_UNIT_PRICE = decimal.Parse(priceText);
                            mgr.SubmitChanges();
                        }
                    }
                    else
                    {
                        if (priceText != null && quantityText != null)
                        {
                            item.PO_QUANTITY = int.Parse(quantityText);
                            item.PO_UNIT_PRICE = decimal.Parse(priceText);
                        }
                    }
                }
                else
                {
                    this.AjaxAlert("數量不得為零!!");
                    return false;                   
                }
            }
            return true;
        }

        protected void extendAttributeField()
        {
            var mgr = dsEntity.CreateDataManager();
            var prodSN = Items.Select(b => b.PRODUCTS_SN).ToArray();
            var nameItems = mgr.GetTable<PRODUCTS_ATTRIBUTE_MAPPING>().Where(m => prodSN.Contains(m.PRODUCTS_SN)).Select(m => m.PRODUCTS_ATTRIBUTE_NAME).Distinct();
            //var nameItems = mgr.GetTable<PRODUCTS_ATTRIBUTE_MAPPING>().Join(mgr.EntityList.Where(p => p.PURCHASE_ORDER_SN == PURCHASE_ORDER_SN).Select(p => p.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA), m => m.PRODUCTS_SN, p => p.PRODUCTS_SN, (m, p) => m.PRODUCTS_ATTRIBUTE_NAME).Distinct();
            int index = gvEntity.Columns.Count - 1;
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

        }
    }
}