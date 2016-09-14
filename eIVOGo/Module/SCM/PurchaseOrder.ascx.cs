using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Locale;
using Model.SCMDataEntity;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;
using eIVOGo.Module.SCM.Item;

namespace eIVOGo.Module.SCM
{
    public partial class PurchaseOrder : System.Web.UI.UserControl
    {       
        protected PURCHASE_ORDER _item;

        protected void Page_Load(object sender, EventArgs e)
        { }

        public void PrepareDataFromDB(int orderSN)
        {            
            var mgr = this.dsPurchase.CreateDataManager();
            var item = mgr.EntityList.Where(p => p.PO_DELETE_STATUS == 0 && p.PURCHASE_ORDER_SN == orderSN).FirstOrDefault();
            item.DataFrom = Naming.DataItemSource.FromDB;
            this.divPurchaseNo.Visible = true;
            DMContainer.DataItem = item;
        }

        private void initializeData()
        {
            var item = (PURCHASE_ORDER)DMContainer.DataItem;
            if (item == null)
            {
                item = new PURCHASE_ORDER
                {
                    PURCHASE_ORDER_NUMBER = "",
                    PO_CLOSED_MODE = 0,
                    PO_DATETIME = DateTime.Now,
                    PO_DELETE_STATUS = 0,
                    PO_STATUS = 0,
                    PO_TOTAL_AMOUNT = 0,
                };
                DMContainer.DataItem = item;
            }
            poDetails.Items = item.PURCHASE_ORDER_DETAILS;
        }
       
        protected override void OnInit(EventArgs e)
        {
            this.PreRender+=new EventHandler(PurchaseOrder_PreRender);
            this.DMContainer.ItemType = typeof(PURCHASE_ORDER);
            this.DMContainer.Load += new EventHandler(DMContainer_Load);
            this.supplierView.Done += new EventHandler(supplierView_Done);
            //this.warehouseView.Load += new EventHandler(warehouseView_Load);
            this.productQuery.BeforeQuery += new EventHandler(productQuery_BeforeQuery);
            this.productQuery.Done += new EventHandler(productQuery_Done);
        }

        void supplierView_Done(object sender, EventArgs e)
        {
            var item = (PURCHASE_ORDER)DMContainer.DataItem;
            if (item != null)
            {
                this.supplierView.SelectedValue = item.SUPPLIER_SN.ToString();
            }
        }

        //void warehouseView_Load(object sender, EventArgs e)
        //{
        //    var item = (PURCHASE_ORDER)DMContainer.DataItem;
        //    if (item != null)
        //    {
        //        this.warehouseView.SelectedValue = item.WAREHOUSE_SN.ToString();
        //    }
        //}

        void productQuery_BeforeQuery(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.supplierView.SelectedValue))
            {
                this.AjaxAlert("請選擇供應商!!");
                productQuery.DoQuery = false;
            }
            else
            {
                this.productQuery.QueryExpr = p => p.PRODUCTS_NAME.Contains(this.productQuery.FieldValue) & p.SUPPLIER_PRODUCTS_NUMBER.Where(s => s.SUPPLIER_SN == int.Parse(supplierView.SelectedValue)).Count() > 0;
            }
        }

        void DMContainer_Load(object sender, EventArgs e)
        {
            initializeData();
        }
        
        void PurchaseOrder_PreRender(object sender, EventArgs e)
        {
            _item = (PURCHASE_ORDER)DMContainer.DataItem;
            if (!this.IsPostBack)
            {
                this.DataBind();
                this.warehouseView.SelectedValue = _item.WAREHOUSE_SN.ToString();
            }
            //if (!this.IsPostBack && _item.DataFrom==Naming.DataItemSource.FromDB)
            //{
            //    this.DataBind();
            //}
        }

        void productQuery_Done(object sender, EventArgs e)
        {
            var items = ((ProductQuery)sender).Items;
            if (items != null)
            {
                var item = (PURCHASE_ORDER)DMContainer.DataItem;
                item.PURCHASE_ORDER_DETAILS.AddRange(
                    items.Select(p => new PURCHASE_ORDER_DETAILS
                    {
                        PO_QUANTITY = 0,
                        PO_UNIT_PRICE = p.BUY_PRICE,
                        PRODUCTS_SN = p.PRODUCTS_SN,
                        SUPPLIER_SN = int.Parse(supplierView.SelectedValue),
                    }).ToArray()
                    );
            }
        }

        protected void btnAddProd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(supplierView.SelectedValue))
            {
                this.AjaxAlert("請選擇供應商!!");
                return;
            }

            if (!string.IsNullOrEmpty(this.txtBarcode.Text))
            {
                var prodItem = this.dsPurchase.CreateDataManager().GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_BARCODE == this.txtBarcode.Text).FirstOrDefault();
                if (prodItem != null)
                {
                    var supNO = prodItem.SUPPLIER_PRODUCTS_NUMBER.Where(s => s.SUPPLIER_SN == int.Parse(supplierView.SelectedValue)).FirstOrDefault();
                    if (supNO != null)
                    {
                       ((PURCHASE_ORDER)DMContainer.DataItem).PURCHASE_ORDER_DETAILS.Add(new PURCHASE_ORDER_DETAILS
                        {
                            PO_QUANTITY = 0,
                            PO_UNIT_PRICE = prodItem.BUY_PRICE,
                            PRODUCTS_SN = prodItem.PRODUCTS_SN,
                            SUPPLIER_SN = int.Parse(supplierView.SelectedValue),
                        });
                    }
                    else
                    {
                        WebMessageBox.AjaxAlert(this, "供應商貨號尚未建立!!");
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

        protected void btnPOPreview_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(supplierView.SelectedValue))
            {
                this.AjaxAlert("請選擇供應商!!");
                return;
            }
            if (String.IsNullOrEmpty(warehouseView.SelectedValue))
            {
                this.AjaxAlert("請選擇倉庫!!");
                return;
            }
            var item = (PURCHASE_ORDER)DMContainer.DataItem;

            if (item.PURCHASE_ORDER_DETAILS.Count() == 0)
            {
                this.AjaxAlert("請新增料品!!");
                return;
            }

            if (!this.poDetails.UpdateData())
            {
                return;
            }

            if (item.PURCHASE_ORDER_SN != 0)
            {
                Page.Items["PONO"] = item.PURCHASE_ORDER_SN;
            }
            else
            {
                DateTime now = DateTime.Now;

                item.PURCHASE_ORDER_NUMBER = "";
                item.PO_CLOSED_MODE = 0;
                item.PO_DATETIME = now;
                item.PO_DELETE_STATUS = 0;
                item.PO_STATUS = 0;
                item.PO_TOTAL_AMOUNT = 0;
                item.SUPPLIER_SN = int.Parse(supplierView.SelectedValue);
                item.WAREHOUSE_SN = int.Parse(warehouseView.SelectedValue);
            }
            Server.Transfer("previewPurchaseOrder.aspx");
        }
    }    
}
