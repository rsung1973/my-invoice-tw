using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.SCMDataEntity;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.Module.Common;

namespace eIVOGo.Module.SCM.Item
{
    public partial class ExchangeOutboundDetailsEditList : System.Web.UI.UserControl
    {
        protected PRODUCTS_DATA _currentItem;

        public IList<EXCHANGE_GOODS_OUTBOND_DETAILS> Items
        {
            get
            {
                return gvEntity.DataSource as IList<EXCHANGE_GOODS_OUTBOND_DETAILS>;
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
            this.PreRender += new EventHandler(BODetailsEditList_PreRender);
        }

        void BODetailsEditList_PreRender(object sender, EventArgs e)
        {
            if (Items != null)
            {
                BindData();
            }
        }

        protected PRODUCTS_DATA loadItem(EXCHANGE_GOODS_OUTBOND_DETAILS item)
        {
            var mgr = dsEntity.CreateDataManager();
            _currentItem = mgr.EntityList.Where(p => p.PRODUCTS_SN == item.PRODUCTS_SN).FirstOrDefault();
            item.PRODUCTS_DATA = _currentItem;
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
            for(int i=0;i<Items.Count;i++)
            {
                var item = Items[i];
                String priceText = Request[String.Format("BO_UNIT_PRICE{0}", i)];
                String quantityText = Request[String.Format("GR_BS_QUANTITY{0}", i)];
                if (!String.IsNullOrEmpty(priceText) && !String.IsNullOrEmpty(quantityText))
                {
                    item.GR_BS_QUANTITY = decimal.Parse(quantityText);
                    item.BO_UNIT_PRICE = decimal.Parse(priceText);
                }
            }
        }

        protected void extendAttributeField()
        {
            var mgr = dsEntity.CreateDataManager();
            var prodSN = Items.Select(b => b.PRODUCTS_SN).ToArray();
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