using System;
using System.Linq.Expressions;
using System.Web.UI;
using System.Linq;

using eIVOGo.Module.Base;
using Model.SCMDataEntity;
using Utility;
using Model.Security.MembershipManagement;
using Business.Helper;
using Model.Locale;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SCM
{
    public partial class POReturnedQueryForShipment : InquireEntity<CDS_Document>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void buildQueryItem()
        {
            //設定能顯示的發票為登入者的公司作廢發票
            //過濾使用者所填入的條件
            if (_item != null)
            {
                itemList.QueryExpr = b => b.PURCHASE_ORDER_RETURNED_SN == _item.DocID;
                modelItem.DataItem = null;
            }
            else
            {
                Expression<Func<PURCHASE_ORDER_RETURNED, bool>> queryExpr = p => true;

                if (!String.IsNullOrEmpty(warehouse.SelectedValue))
                {
                    queryExpr = queryExpr.And(p => p.WAREHOUSE_SN == int.Parse(warehouse.SelectedValue));
                }

                if (!String.IsNullOrEmpty(supplier.SelectedValue))
                {
                    queryExpr = queryExpr.And(p => p.SUPPLIER_SN == int.Parse(supplier.SelectedValue));
                }

                if (!String.IsNullOrEmpty(orderNo.Text))
                {
                    queryExpr = queryExpr.And(b => b.PURCHASE_ORDER_RETURNED_NUMBER == orderNo.Text);
                }

                if (!String.IsNullOrEmpty(barcode.Text))
                {
                    queryExpr = queryExpr.And(b => b.PURCHASE_ORDER_RETURNED_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text) > 0);
                }
                if (!String.IsNullOrEmpty(txtName.Text))
                {
                    queryExpr = queryExpr.And(b => b.PURCHASE_ORDER_RETURNED_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)) > 0);
                }

                if (orderDateFrom.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.PO_RETURNED_DATETIME >= orderDateFrom.DateTimeValue);
                }
                if (orderDateTo.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.PO_RETURNED_DATETIME < orderDateTo.DateTimeValue.AddDays(1));
                }

                queryExpr = queryExpr.And(b => b.CDS_Document.CurrentStep != (int)Naming.DocumentStepDefinition.已刪除);

                itemList.QueryExpr = queryExpr;
            }

            tblNext.Visible = true;
        }

        protected void rbChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            Server.Transfer(rbChange.SelectedValue);
        }
      

        protected override UserControl _itemList
        {
            get { return itemList; }
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request["rbSN"]))
            {
                var item = dsEntity.CreateDataManager().EntityList.Where(b => b.PURCHASE_ORDER_RETURNED_SN == int.Parse(Request["rbSN"])).First().CDS_Document;
                item.BUYER_SHIPMENT = new BUYER_SHIPMENT
                {
                    BUYER_SHIPMENT_SN = item.DocID,
                    SHIPMENT_DATETIME = DateTime.Now
                };
                modelItem.DataItem = item;
                Server.Transfer(ToCreateShipment.TransferTo);
            }
            else
            {
                this.AjaxAlert("請選擇轉入單據!!");
            }
        }
    }
}