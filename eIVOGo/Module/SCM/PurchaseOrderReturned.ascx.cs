using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using Utility;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;
using Model.Locale;

namespace eIVOGo.Module.SCM
{
    public partial class PurchaseOrderReturned : System.Web.UI.UserControl
    {
        protected PURCHASE_ORDER_RETURNED _item;

        protected void Page_Load(object sender, EventArgs e)
        { }

        public void PrepareDataFromDB(int orderSN)
        {
            //var mgr = this.dsPurchase.CreateDataManager();
            //var item = mgr.EntityList.Where(p => p.PO_DELETE_STATUS == 0 && p.PURCHASE_ORDER_SN == orderSN).FirstOrDefault();
            //item.DataFrom = Naming.DataItemSource.FromDB;
            //DMContainer.DataItem = item;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(PurchaseOrderReturned_PreRender);
            this.POReturnContainer.ItemType = typeof(PURCHASE_ORDER_RETURNED);
            this.POReturnContainer.Load += new EventHandler(POReturnContainer_Load);
        }

        void PurchaseOrderReturned_PreRender(object sender, EventArgs e)
        {
            _item = (PURCHASE_ORDER_RETURNED)this.POReturnContainer.DataItem;
            if (!this.IsPostBack && _item.DataFrom == Naming.DataItemSource.FromPreviousPage)
            {
                if (_item.SUPPLIER_SN != 0 & _item.WAREHOUSE_SN != 0)
                {
                    this.ddlSupplier.SelectedValue = _item.SUPPLIER_SN.ToString();
                    this.ddlWarehouse.SelectedValue = _item.WAREHOUSE_SN.ToString();
                }
                this.porDetail.Items = _item.PURCHASE_ORDER_RETURNED_DETAILS;
                this.porDetail.SUPPLIER_SN = int.Parse(this.ddlSupplier.SelectedValue);
                this.porDetail.WAREHOUSE_SN = int.Parse(this.ddlWarehouse.SelectedValue);
                this.divResult.Visible = true;
                this.btnCreatPOR.Visible = true;
                this.lblError.Visible = false;
                //this.DataBind();
            }
        }

        void POReturnContainer_Load(object sender, EventArgs e)
        {
            initializeData();
        }


        private void initializeData()
        {
            if (!this.IsPostBack)
            {
                var mgr = this.dsPOReturn.CreateDataManager();
                this.ddlWarehouse.Items.AddRange(mgr.GetTable<WAREHOUSE>().Select(wh => new ListItem(wh.WAREHOUSE_NAME, wh.WAREHOUSE_SN.ToString())).ToArray());
                this.ddlSupplier.Items.AddRange(mgr.GetTable<SUPPLIER>().Select(s => new ListItem(s.SUPPLIER_NAME, s.SUPPLIER_SN.ToString())).ToArray());
            }

            var item = (PURCHASE_ORDER_RETURNED)this.POReturnContainer.DataItem;
            if (item == null)
            {
                item = new PURCHASE_ORDER_RETURNED
                {
                    PURCHASE_ORDER_RETURNED_NUMBER = "",
                    PO_RETURNED_DATETIME = DateTime.Now,
                    PO_RETURNED_DELETE_STATUS = 0,
                    PO_RETURNED_STATUS = 0,
                    PO_RETURN_TOTAL_AMOUNT = 0
                };
                this.POReturnContainer.DataItem = item;
            }

            if (this.ddlSupplier.SelectedIndex != 0 & this.ddlWarehouse.SelectedIndex != 0)
            {
                this.porDetail.SUPPLIER_SN = int.Parse(this.ddlSupplier.SelectedValue);
                this.porDetail.WAREHOUSE_SN = int.Parse(this.ddlWarehouse.SelectedValue);
                this.porDetail.Items = item.PURCHASE_ORDER_RETURNED_DETAILS;
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.ddlSupplier.SelectedIndex == 0)
            {
                this.AjaxAlert("請選擇供應商!!");
                return;
            }

            if (this.ddlWarehouse.SelectedIndex == 0)
            {
                this.AjaxAlert("請選擇退貨倉庫!!");
                return;
            }

            this.divResult.Visible = true;
            this.btnQuery.CommandArgument = "Query";

            var prodItem = this.dsPOReturn.CreateDataManager().GetTable<PRODUCTS_DATA>().Where(p => p.SUPPLIER_PRODUCTS_NUMBER.Where(sp => sp.SUPPLIER_SN == int.Parse(this.ddlSupplier.SelectedValue)).Count() > 0 & p.PRODUCTS_WAREHOUSE_MAPPING.Where(pw => pw.WAREHOUSE_SN == int.Parse(this.ddlWarehouse.SelectedValue)).Count() > 0 & p.PRODUCTS_WAREHOUSE_MAPPING.Where(pwm => pwm.WAREHOUSE_SN == int.Parse(this.ddlWarehouse.SelectedValue)).FirstOrDefault().PRODUCTS_TOTAL_AMOUNT > 0);
            var itemData = (PURCHASE_ORDER_RETURNED)this.POReturnContainer.DataItem;
            itemData.PURCHASE_ORDER_RETURNED_DETAILS.Clear();
            if (prodItem != null)
            {
                foreach (var item in prodItem)
                {
                    itemData.PURCHASE_ORDER_RETURNED_DETAILS.Add(new PURCHASE_ORDER_RETURNED_DETAILS
                    {
                        POR_QUANTITY = 0,
                        POR_UNIT_PRICE = item.BUY_PRICE,
                        POR_DEFECTIVE_QUANTITY = 0,
                        PRODUCTS_SN = item.PRODUCTS_SN
                    });
                }
            }

            if (prodItem.Count() > 0)
            {
                this.btnCreatPOR.Visible = true;
                this.lblError.Visible = false;
                this.H2Title.Visible = true;
            }
            else
            {
                this.btnCreatPOR.Visible = false;
                this.lblError.Text = "查無資料!!";
                this.lblError.Visible = true;
                this.H2Title.Visible = false;
            }

            this.porDetail.Items = itemData.PURCHASE_ORDER_RETURNED_DETAILS;
        }

        protected void btnCreatPOR_Click(object sender, EventArgs e)
        {            
            var itemData = (PURCHASE_ORDER_RETURNED)this.POReturnContainer.DataItem;

            //if (this.porDetail.UpdateData())
            //{
            //    Boolean checkAll = false;
            //    do
            //    {
            //        foreach (var det in itemData.PURCHASE_ORDER_RETURNED_DETAILS)
            //        {
            //            if (det.POR_QUANTITY == 0)
            //            {
            //                itemData.PURCHASE_ORDER_RETURNED_DETAILS.Remove(det);
            //                POReturnContainer.DataItem = itemData;
            //                itemData = (PURCHASE_ORDER_RETURNED)this.POReturnContainer.DataItem;
            //                checkAll = false;
            //                break;
            //            }
            //            checkAll = true;
            //        }
            //    } while (checkAll == false);
            //}
            //else
            //{
            //    return;
            //}
            if (!this.porDetail.UpdateData())
            {
                return;
            }

            itemData.SUPPLIER_SN = int.Parse(this.ddlSupplier.SelectedValue);
            itemData.WAREHOUSE_SN = int.Parse(this.ddlWarehouse.SelectedValue);
            Server.Transfer("previewPurchaseOrderReturn.aspx");
        }
    }
}