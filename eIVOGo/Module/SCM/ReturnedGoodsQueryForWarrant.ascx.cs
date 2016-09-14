using System;
using System.Linq.Expressions;
using System.Web.UI;
using System.Linq;

using eIVOGo.Module.Base;
using Model.SCMDataEntity;
using Utility;
using Model.Security.MembershipManagement;
using Business.Helper;
using eIVOGo.Helper;
using Uxnet.Web.WebUI;
using Model.Locale;

namespace eIVOGo.Module.SCM
{
    public partial class ReturnedGoodsQueryForWarrant : InquireEntity<WAREHOUSE_WARRANT>
    {
        private UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        protected override void buildQueryItem()
        {
            //設定能顯示的發票為登入者的公司作廢發票
            //過濾使用者所填入的條件
            if (_item != null)
            {
                int[] detailSN = _item.WAREHOUSE_WARRANT_DETAILS.Select(w => w.GR_DETAILS_SN.Value).ToArray();
                itemList.QueryExpr = p => p.GOODS_RETURNED_DETAILS.Any(d => detailSN.Contains(d.GOODS_RETURNED_SN));
                modelItem.DataItem = null;
            }
            else
            {
                Expression<Func<GOODS_RETURNED, bool>> queryExpr = p => p.BUYER_ORDERS.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID;

                if (!String.IsNullOrEmpty(orderNo.Text))
                {
                    queryExpr = queryExpr.And(b => b.GOODS_RETURNED_NUMBER == orderNo.Text);
                }
                if (!String.IsNullOrEmpty(buyerOrderNo.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_ORDERS_NUMBER == buyerOrderNo.Text);
                }

                if (!String.IsNullOrEmpty(barcode.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_ORDERS_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text) > 0);
                }
                if (!String.IsNullOrEmpty(txtName.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_ORDERS_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)) > 0);
                }

                if (!String.IsNullOrEmpty(buyerQuery.QueryArgument))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_SN == buyerQuery.Item.BUYER_SN );
                }
                if (orderDateFrom.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.GOODS_RETURNED_DATETIME >= orderDateFrom.DateTimeValue);
                }
                if (orderDateTo.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.GOODS_RETURNED_DATETIME < orderDateTo.DateTimeValue.AddDays(1));
                }

                if (!String.IsNullOrEmpty(marketResource.SelectedValue))
                {
                    int marketSN = int.Parse(marketResource.SelectedValue);
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.MARKET_RESOURCE_SN == marketSN);
                }

                queryExpr = queryExpr.And(b => !b.GR_STATUS.HasValue || b.GR_STATUS == 0);

                if (!String.IsNullOrEmpty(warehouse.SelectedValue))
                {
                    int warehouseSN = int.Parse(warehouse.SelectedValue);
                    queryExpr = queryExpr.And(p => p.WAREHOUSE_SN == warehouseSN);
                }

                if (!String.IsNullOrEmpty(SupplierSN.SelectedValue))
                {
                    int sn = int.Parse(SupplierSN.SelectedValue);
                    queryExpr = queryExpr.And(p => p.GOODS_RETURNED_DETAILS.Count(d => d.BUYER_ORDERS_DETAILS.PRODUCTS_DATA.SUPPLIER_PRODUCTS_NUMBER.Count(s => s.SUPPLIER_SN == sn) > 0) > 0);
                }

                queryExpr = queryExpr.And(b => b.CDS_Document.CurrentStep != (int)Naming.DocumentStepDefinition.已刪除);
                itemList.QueryExpr = queryExpr;
            }

            tblNext.Visible = true;
        }
             
        protected override UserControl _itemList
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
                Server.Transfer(ToCreateWarrant.TransferTo);
            }
            else
            {
                this.AjaxAlert("請選擇項目!!");
            }
        }
        private WAREHOUSE_WARRANT createWarehouseWarrant(int[] poSN)
        {
            var mgr = dsEntity.CreateDataManager();

            DateTime now = DateTime.Now;
            var item = new WAREHOUSE_WARRANT
            {
                WW_DATETIME = now,
                SUPPLIER_SN = int.Parse(SupplierSN.SelectedValue),
                WAREHOUSE_SN = int.Parse(warehouse.SelectedValue)
            };

            foreach (var p in mgr.GetTable<GOODS_RETURNED_DETAILS>().Where(p => poSN.Contains(p.GOODS_RETURNED_SN)))
            {
                var detailItem = new WAREHOUSE_WARRANT_DETAILS
                {
                    GR_DETAILS_SN = p.GR_DETAILS_SN,
                    RECEIPT_QUANTITY = 0,
                    WW_QUANTITY = p.GR_QUANTITY,
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
    }
}