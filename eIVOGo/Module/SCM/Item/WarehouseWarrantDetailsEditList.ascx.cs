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

namespace eIVOGo.Module.SCM.Item
{
    public partial class WarehouseWarrantDetailsEditList : System.Web.UI.UserControl
    {
        protected PRODUCTS_DATA _currentItem;
        protected SUPPLIER_PRODUCTS_NUMBER _supplierItem;

        private List<PRODUCTS_DATA> _productItems = new List<PRODUCTS_DATA>();

        public IList<WAREHOUSE_WARRANT_DETAILS> Items
        {
            get
            {
                return gvEntity.DataSource as IList<WAREHOUSE_WARRANT_DETAILS>;
            }
            set
            {
                gvEntity.DataSource = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            doDelete.DoAction = arg =>
            {
                delete(int.Parse(arg));
            };
            this.PreRender += new EventHandler(WarehouseWarrantDetailsEditList_PreRender);
        }

        void WarehouseWarrantDetailsEditList_PreRender(object sender, EventArgs e)
        {
            if (Items != null)
            {
                BindData();
            }
        }

        protected PRODUCTS_DATA loadItem(WAREHOUSE_WARRANT_DETAILS dataItem)
        {
            var mgr = dsEntity.CreateDataManager();
            _currentItem = null;
            if (dataItem.PO_DETAILS_SN.HasValue)
            {
                var item = mgr.GetTable<PURCHASE_ORDER_DETAILS>().Where(d => d.PO_DETAILS_SN == dataItem.PO_DETAILS_SN).First();
                _currentItem = item.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA;
                _supplierItem = _currentItem.SUPPLIER_PRODUCTS_NUMBER.Where(s=>s.SUPPLIER_SN==dataItem.WAREHOUSE_WARRANT.SUPPLIER_SN).First();
                dataItem.PURCHASE_ORDER_DETAILS = item;
            }
            else if (dataItem.EGI_DETAILS_SN.HasValue)
            {
                var item = mgr.GetTable<EXCHANGE_GOODS_INBOUND_DETAILS>().Where(e => e.EGI_DETAILS_SN == dataItem.EGI_DETAILS_SN).First();
                _currentItem = item.PRODUCTS_DATA;
                _supplierItem = _currentItem.SUPPLIER_PRODUCTS_NUMBER.Where(s => s.SUPPLIER_SN == dataItem.WAREHOUSE_WARRANT.SUPPLIER_SN).First();
                dataItem.EXCHANGE_GOODS_INBOUND_DETAILS = item;
           }
            else if (dataItem.GR_DETAILS_SN.HasValue)
            {
                var item =  mgr.GetTable<GOODS_RETURNED_DETAILS>().Where(r => r.GR_DETAILS_SN == dataItem.GR_DETAILS_SN).First();
                _currentItem = dataItem.GOODS_RETURNED_DETAILS.BUYER_ORDERS_DETAILS.PRODUCTS_DATA;
                _supplierItem = _currentItem.SUPPLIER_PRODUCTS_NUMBER.Where(s => s.SUPPLIER_SN == dataItem.WAREHOUSE_WARRANT.SUPPLIER_SN).First();
                dataItem.GOODS_RETURNED_DETAILS = item;
            }
            if (_currentItem != null)
            {
                _productItems.Add(_currentItem);
            }
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

        public void UpdateData()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                String defectiveText = Request[String.Format("WW_DEFECTIVE_QUANTITY{0}", i)];
                String quantityText = Request[String.Format("RECEIPT_QUANTITY{0}", i)];
                if (!String.IsNullOrEmpty(defectiveText) && !String.IsNullOrEmpty(quantityText))
                {
                    item.WW_DEFECTIVE_QUANTITY = decimal.Parse(defectiveText);
                    item.RECEIPT_QUANTITY = decimal.Parse(quantityText);
                }
            }
        }

        protected void extendAttributeField()
        {
            var mgr = dsEntity.CreateDataManager();
            var prodSN = _productItems.Select(p => p.PRODUCTS_SN).ToArray();
            var nameItems = mgr.GetTable<PRODUCTS_ATTRIBUTE_MAPPING>().Where(m => prodSN.Contains(m.PRODUCTS_SN)).Select(m => m.PRODUCTS_ATTRIBUTE_NAME).Distinct();
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