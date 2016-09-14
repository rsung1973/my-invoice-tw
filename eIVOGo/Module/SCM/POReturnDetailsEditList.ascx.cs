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
using Model.Locale;

namespace eIVOGo.Module.SCM
{
    public partial class POReturnDetailsEditList : System.Web.UI.UserControl
    {
        protected SUPPLIER_PRODUCTS_NUMBER _currentItem1;
        protected PRODUCTS_WAREHOUSE_MAPPING _currentItem2;

        public IList<PURCHASE_ORDER_RETURNED_DETAILS> Items
        {
            get
            {
                return gvEntity.DataSource as IList<PURCHASE_ORDER_RETURNED_DETAILS>;
            }
            set
            {
                gvEntity.DataSource = value;
            }
        }

        public int WAREHOUSE_SN
        {
            get;
            set;
        }

        public int SUPPLIER_SN
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(POReturnDetailsEditList_PreRender);
        }

        void POReturnDetailsEditList_PreRender(object sender, EventArgs e)
        {
            if (Items != null)
            {
                BindData();
            }
        }

        protected SUPPLIER_PRODUCTS_NUMBER loadItem1(PURCHASE_ORDER_RETURNED_DETAILS item)
        {
            var mgr = dsEntity.CreateDataManager();
            _currentItem1 = mgr.GetTable<SUPPLIER_PRODUCTS_NUMBER>().Where(sp => sp.PRODUCTS_SN == item.PRODUCTS_SN & sp.SUPPLIER_SN == SUPPLIER_SN).FirstOrDefault();
            //item.PRODUCTS_DATA = _currentItem1.PRODUCTS_DATA;
            item.PRODUCTS_DATA = mgr.GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_SN == item.PRODUCTS_SN).First();
            return _currentItem1;
        }

        protected PRODUCTS_WAREHOUSE_MAPPING loadItem2(PURCHASE_ORDER_RETURNED_DETAILS item)
        {
            var mgr = dsEntity.CreateDataManager();
            _currentItem2 = mgr.GetTable<PRODUCTS_WAREHOUSE_MAPPING>().Where(pw => pw.PRODUCTS_SN == item.PRODUCTS_SN & pw.WAREHOUSE_SN == WAREHOUSE_SN).FirstOrDefault();
            return _currentItem2;
        }

        public void BindData()
        {
            extendAttributeField();
            gvEntity.DataBind();
        }

        public Boolean UpdateData()
        {
            String[] chk = Request.Form.GetValues("chkItem");
            if (chk != null && chk.Count() > 0)
            {
                foreach (var index in chk)
                {
                    int i = int.Parse(index);
                    var item = Items[i];
                    String priceText = Request[String.Format("POR_UNIT_PRICE{0}", i)];
                    String quantityText = Request[String.Format("POR_QUANTITY{0}", i)];
                    String defectiveQuantityText = Request[String.Format("POR_DEFECTIVE_QUANTITY{0}", i)];
                    Decimal? TotalAmount = loadItem2(item).PRODUCTS_TOTAL_AMOUNT;
                    if (TotalAmount > 0 & TotalAmount >= decimal.Parse(quantityText))
                    {
                        if (int.Parse(quantityText) != 0)
                        {
                            if (priceText != null && quantityText != null)
                            {
                                item.POR_QUANTITY = int.Parse(quantityText);
                                item.POR_UNIT_PRICE = decimal.Parse(priceText);
                                item.POR_DEFECTIVE_QUANTITY = int.Parse(defectiveQuantityText);
                                item.DataStatus = Naming.DataItemStatus.Modified;
                            }
                        }
                        else
                        {
                            this.AjaxAlert("已選擇退貨數量不得為零!!");
                            return false;
                        }
                    }
                    else
                    {
                        this.AjaxAlert("無庫存數量或庫存數量不足!!");
                        return false;
                    }
                }
            }
            else
            {
                this.AjaxAlert("請選擇一筆退貨料品資訊!!");
                return false;
            }
            return true;
        }

        protected void extendAttributeField()
        {
            var mgr = dsEntity.CreateDataManager();
            var prodSN = Items.Select(b => b.PRODUCTS_SN).ToArray();
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

        protected void gvEntity_DataBinding(object sender, EventArgs e)
        {

        }
    }
}