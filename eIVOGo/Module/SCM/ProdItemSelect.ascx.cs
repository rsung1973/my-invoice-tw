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
using System.Linq.Expressions;
using Utility;
using Uxnet.Web.WebUI;
using DataAccessLayer.basis;

namespace eIVOGo.Module.SCM
{
    public partial class ProdItemSelect : System.Web.UI.UserControl
    {
        private PURCHASE_ORDER _currentPO;

        public event EventHandler ItemCreated;

        public int? PURCHASE_ORDER_SN
        {
            get
            {
                return (int?)ViewState["orderSN"];
            }
            set
            {
                ViewState["orderSN"] = value;
            }
        }

        public CreateItemFunc<PURCHASE_ORDER> CreatePurchaseOrder;

        protected void Page_Load(object sender, EventArgs e)
        {
            initializeData();
        }

        private void initializeData()
        {
            var mgr = this.dsPurchase.CreateDataManager();
            if (PURCHASE_ORDER_SN.HasValue)
            {
                _currentPO = mgr.EntityList.Where(p => p.PURCHASE_ORDER_SN == PURCHASE_ORDER_SN).FirstOrDefault();

            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.dsPurchase.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PURCHASE_ORDER>>(dsPurchase_Select);
            productQuery.Done += new EventHandler(productQuery_Done);
            productQuery.QueryExpr = p => p.PRODUCTS_NAME.Contains(productQuery.FieldValue);
        }

        void productQuery_Done(object sender, EventArgs e)
        {
            if (productQuery.Item != null)
            {
                txtBarcode.Text = productQuery.Item.PRODUCTS_BARCODE;
            }
            else
            {
                txtBarcode.Text = "";
            }
        }

        void dsPurchase_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PURCHASE_ORDER> e)
        {

        }

        protected void btnAddProd_Click(object sender, EventArgs e)
        {
            var mgr = dsPurchase.CreateDataManager();
            if (!string.IsNullOrEmpty(this.txtBarcode.Text))
            {
                var pod = mgr.GetTable<PRODUCTS_DATA>().Where(sp => sp.PRODUCTS_BARCODE == this.txtBarcode.Text).FirstOrDefault();
                if (pod != null)
                {
                    try
                    {
                        if (_currentPO == null && CreatePurchaseOrder != null)
                        {
                            _currentPO = CreatePurchaseOrder(mgr);
                            if (_currentPO != null)
                                PURCHASE_ORDER_SN = _currentPO.PURCHASE_ORDER_SN;
                        }

                        if (_currentPO != null)
                        {
                            var supNO = pod.SUPPLIER_PRODUCTS_NUMBER.Where(s => s.SUPPLIER_SN == _currentPO.SUPPLIER_SN).FirstOrDefault();

                            if (supNO != null)
                            {

                                PURCHASE_ORDER_DETAILS podt = new PURCHASE_ORDER_DETAILS
                                {
                                    PURCHASE_ORDER_SN = _currentPO.PURCHASE_ORDER_SN,
                                    SUPPLIER_PRODUCTS_NUMBER = supNO,
                                    PO_QUANTITY = 0,
                                    PO_UNIT_PRICE = pod.BUY_PRICE,
                                };

                                ///料品、倉儲對應自動關聯
                                if (pod.PRODUCTS_WAREHOUSE_MAPPING.Count(w => w.WAREHOUSE_SN == _currentPO.WAREHOUSE_SN) <= 0)
                                {
                                    pod.PRODUCTS_WAREHOUSE_MAPPING.Add(new PRODUCTS_WAREHOUSE_MAPPING
                                    {
                                        WAREHOUSE_SN = _currentPO.WAREHOUSE_SN,
                                        PRODUCTS_DEFECTIVE_AMOUNT = 0,
                                        PRODUCTS_PLAN_AMOUNT = 0,
                                        PRODUCTS_SAFE_AMOUNT_PERCENTAGE = 0,
                                        PRODUCTS_TOTAL_AMOUNT = 0
                                    });
                                }

                                mgr.GetTable<PURCHASE_ORDER_DETAILS>().InsertOnSubmit(podt);
                                mgr.SubmitChanges();

                                if (ItemCreated != null)
                                {
                                    ItemCreated(this, new EventArgs());
                                }
                            }
                            else
                            {
                                WebMessageBox.AjaxAlert(this, "供應商貨號尚未建立!!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
                else
                {
                    WebMessageBox.AjaxAlert(this, "查無此料品!!");
                }
            }
            else
            {
                WebMessageBox.AjaxAlert(this, "請輸入料品Barcode!!");
            }
        }
    }
}   

