using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Linq.Expressions;

using eIVOGo.Module.Base;
using eIVOGo.Helper;
using Model.SCM;
using Model.SCMDataEntity;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;
using Utility;
using Model.Locale;
using eIVOGo.Properties;

namespace eIVOGo.Module.SCM
{
    public partial class MaintainGoodsReceiptFromPO : InquireEntity<WAREHOUSE_WARRANT>
    {


        protected override void btnAdd_Click(object sender, EventArgs e)
        {

        }

        protected override void buildQueryItem()
        {
            if (_item != null)
            {
                int[] detailSN = _item.WAREHOUSE_WARRANT_DETAILS.Select(w => w.PO_DETAILS_SN.Value).ToArray();
                itemList.QueryExpr = p => p.PURCHASE_ORDER_DETAILS.Any(d => detailSN.Contains(d.PO_DETAILS_SN));
                modelItem.DataItem = null;
            }
            else
            {
                Expression<Func<PURCHASE_ORDER, bool>> queryExpr = w => w.CDS_Document.CurrentStep == (int)Naming.DocumentStepDefinition.已開立;
                if (!String.IsNullOrEmpty(PONo.Text))
                {
                    queryExpr = queryExpr.And(p => p.PURCHASE_ORDER_NUMBER == PONo.Text);
                }
                if (!String.IsNullOrEmpty(warehouse.SelectedValue))
                {
                    queryExpr = queryExpr.And(p => p.WAREHOUSE_SN == int.Parse(warehouse.SelectedValue));
                }
                else
                {
                    queryExpr = queryExpr.And(p => false);
                }
                if (!String.IsNullOrEmpty(SupplierSN.SelectedValue))
                {
                    queryExpr = queryExpr.And(p => p.SUPPLIER_SN == int.Parse(SupplierSN.SelectedValue));
                }
                else
                {
                    queryExpr = queryExpr.And(p => false);
                }
                if (!String.IsNullOrEmpty(ProductNo.Text))
                {
                    queryExpr = queryExpr.And(p => p.PURCHASE_ORDER_DETAILS.Count(d => d.SUPPLIER_PRODUCTS_NUMBER.SUPPLIER_PRODUCTS_NUMBER1 == ProductNo.Text) > 0);
                }
                if (!String.IsNullOrEmpty(Barcode.Text))
                {
                    queryExpr = queryExpr.And(p => p.PURCHASE_ORDER_DETAILS.Count(d => d.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_BARCODE == Barcode.Text) > 0);
                }
                if (!String.IsNullOrEmpty(ProductName.Text))
                {
                    queryExpr = queryExpr.And(p => p.PURCHASE_ORDER_DETAILS.Count(d => d.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_NAME == ProductName.Text) > 0);
                }
                if (PODateFrom.HasValue)
                {
                    queryExpr = queryExpr.And(p => p.PO_DATETIME >= PODateFrom.DateTimeValue);
                }
                if (PODateTo.HasValue)
                {
                    queryExpr = queryExpr.And(p => p.PO_DATETIME < PODateTo.DateTimeValue.AddDays(1));
                }

                queryExpr = queryExpr.And(b => b.CDS_Document.CurrentStep != (int)Naming.DocumentStepDefinition.已刪除);
                itemList.QueryExpr = queryExpr;
            }

            tblNext.Visible = true;

        }


        protected override System.Web.UI.UserControl _itemList
        {
            get { return itemList; }
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            var items = Request.GetItemSelection();
            if (items != null && items.Length > 0)
            {
                var item = createWarehouseWarrant(items.Select(s => int.Parse(s)).ToArray());
                modelItem.DataItem = item;
                Server.Transfer(ToCreateReceipt.TransferTo);
            }
            else
            {
                this.AjaxAlert("請選擇項目!!");
            }
        }
        private WAREHOUSE_WARRANT createWarehouseWarrant(int[] poSN)
        {
            var mgr = dsWW.CreateDataManager();

            DateTime now = DateTime.Now;
            var item = new WAREHOUSE_WARRANT
            {
                WW_DATETIME = now,
                SUPPLIER_SN = int.Parse(SupplierSN.SelectedValue),
                WAREHOUSE_SN = int.Parse(warehouse.SelectedValue)
            };

            foreach (var p in mgr.GetTable<PURCHASE_ORDER_DETAILS>().Where(p => poSN.Contains(p.PURCHASE_ORDER_SN)))
            {
                var detailItem = new WAREHOUSE_WARRANT_DETAILS
                {
                    PO_DETAILS_SN = p.PO_DETAILS_SN,
                    RECEIPT_QUANTITY = 0,
                    WW_QUANTITY = p.PO_QUANTITY,
                    WW_DEFECTIVE_QUANTITY = 0
                };
                item.WAREHOUSE_WARRANT_DETAILS.Add(detailItem);
            }

            return item;
        }

        protected void rbChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            Server.Transfer(rbChange.SelectedValue);
        }


        //private WAREHOUSE_WARRANT createWarehouseWarrant(int[] poSN)
        //{
        //    var mgr = dsWW.CreateDataManager();
        //    int currentCount = mgr.EntityList.Count(w => w.WW_DATETIME >= DateTime.Today);

        //    DateTime now = DateTime.Now;
        //    var item = new WAREHOUSE_WARRANT
        //    {
        //        CDS_Document = new Model.SCMDataEntity.CDS_Document
        //        {
        //            CurrentStep = (int)Naming.DocumentStepDefinition.預覽,
        //            DocType = (int)Naming.DocumentTypeDefinition.WarehouseWarrant,
        //            DocDate = now
        //        },
        //        WW_DATETIME = now,
        //        WAREHOUSE_WARRANT_NUMBER = String.Format("{0}{1:yyyyMMdd}{2:0000}", Settings.Default.WarehouseWarrantPrefix, now, currentCount + 1)
        //    };

        //    mgr.EntityList.InsertOnSubmit(item);
        //    foreach(var p in mgr.GetTable<PURCHASE_ORDER_DETAILS>().Where(p => poSN.Contains(p.PURCHASE_ORDER_SN)))
        //    {
        //        var detailItem = new WAREHOUSE_WARRANT_DETAILS
        //        {
        //            PO_DETAILS_SN = p.PO_DETAILS_SN,
        //            RECEIPT_QUANTITY = 0,
        //            WW_QUANTITY = p.PO_QUANTITY,
        //            WW_DEFECTIVE_QUANTITY = 0
        //        };
        //        item.WAREHOUSE_WARRANT_DETAILS.Add(detailItem);
        //    }

        //    mgr.SubmitChanges();
        //    return item;
        //}
    }
}