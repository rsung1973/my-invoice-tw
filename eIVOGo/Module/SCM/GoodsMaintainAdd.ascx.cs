using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using eIVOGo.Module.Common;
using Model.InvoiceManagement;
using Model.SCM;
using Model.SCMDataEntity;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;
using Model.Locale;
using eIVOGo.Module.Base;

namespace eIVOGo.Module.SCM
{
    public partial class GoodsMaintainAdd : SCMEntityEdit<PRODUCTS_DATA>
    {

        protected override void prepareInitialData()
        {
            Item = new PRODUCTS_DATA
            {

            };

            productAttribute.Items = _item.PRODUCTS_ATTRIBUTE_MAPPING;

        }

        public override void PrepareDataFromDB(object keyValue)
        {
            int productSN = (int)keyValue;
            var scm = dsEntity.CreateDataManager();
            _item = scm.EntityList.Where(p => p.PRODUCTS_SN == productSN).FirstOrDefault();
            productAttribute.Items = _item.PRODUCTS_ATTRIBUTE_MAPPING;
            _item.DataFrom = Naming.DataItemSource.FromDB;
            modelItem.DataItem = _item;
        }

        protected override void prepareDataForViewState()
        {
            var table = dsEntity.CreateDataManager().GetTable<PRODUCTS_ATTRIBUTE_NAME>();
            foreach (var mappingItem in _item.PRODUCTS_ATTRIBUTE_MAPPING)
            {
                mappingItem.PRODUCTS_ATTRIBUTE_NAME = table.Where(a => a.PRODUCTS_ATTR_NAME_SN == mappingItem.PRODUCTS_ATTR_NAME_SN).FirstOrDefault();
            }
            productAttribute.Items = _item.PRODUCTS_ATTRIBUTE_MAPPING;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void SCMEntityPreview_PreRender(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.DataBind();
            }

            if (_item.SUPPLIER_PRODUCTS_NUMBER.Count > 0)
            {
                supplier.SelectedValue = _item.SUPPLIER_PRODUCTS_NUMBER[0].SUPPLIER_SN.ToString();
                supplierProductNumber.Text = _item.SUPPLIER_PRODUCTS_NUMBER[0].SUPPLIER_PRODUCTS_NUMBER1;
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.PRODUCTS_BARCODE.Text))
            {
                this.AjaxAlert("請輸入料品Barcode!!");
                return;
            }

            if (string.IsNullOrEmpty(this.PRODUCTS_NAME.Text))
            {
                this.AjaxAlert("請輸入料品名稱!!");
                return;
            }

            if (String.IsNullOrEmpty(supplierProductNumber.Text))
            {
                this.AjaxAlert("請輸入供應商貨號!!");
                return;
            }

            PRODUCTS_DATA item = (PRODUCTS_DATA)modelItem.DataItem;
            item.PRODUCTS_BARCODE = PRODUCTS_BARCODE.Text;
            item.PRODUCTS_NAME = PRODUCTS_NAME.Text;
            if (!String.IsNullOrEmpty(SELL_PRICE.Text))
                item.SELL_PRICE = decimal.Parse(SELL_PRICE.Text);
            if (!String.IsNullOrEmpty(BUY_PRICE.Text))
                item.BUY_PRICE = decimal.Parse(BUY_PRICE.Text);

            try
            {
                using (var mgr = new ProductManager())
                {
                    mgr.Save(item, int.Parse(supplier.SelectedValue), supplierProductNumber.Text);
                }

                //Server.Transfer("~/SCM_SYSTEM/Goods_Maintain.aspx");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('料品資料維護完成!!'); location.href='Goods_Maintain.aspx';", true);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert("系統發生錯誤,錯誤原因:" + ex.Message);
            }
        }
    }
}