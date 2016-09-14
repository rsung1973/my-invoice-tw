using System;
using System.Linq.Expressions;
using System.Web.UI;
using System.Linq;

using eIVOGo.Module.Base;
using Model.SCMDataEntity;
using Utility;
using Model.Security.MembershipManagement;
using Business.Helper;
using Uxnet.Web.WebUI;
using Model.Locale;

namespace eIVOGo.Module.SCM
{
    public partial class BuyerOrderQueryToExchangeGoods : InquireEntity<EXCHANGE_GOODS>
    {
        private UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        protected override void modelItem_Load(object sender, EventArgs e)
        {
            base.modelItem_Load(sender, e);
            if (_item != null)
            {
                _item.BUYER_ORDERS = dsEntity.CreateDataManager().EntityList.Where(b => b.BUYER_ORDERS_SN == _item.BUYER_ORDERS_SN).First();
            }
        }

        protected override void buildQueryItem()
        {
            //設定能顯示的發票為登入者的公司作廢發票
            //過濾使用者所填入的條件
            if (_item != null)
            {
                itemList.QueryExpr = b => b.BUYER_ORDERS_SN == _item.BUYER_ORDERS_SN;
                modelItem.DataItem = null;
            }
            else
            {
                Expression<Func<BUYER_ORDERS, bool>> queryExpr = p => p.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID && p.CDS_Document.BUYER_SHIPMENT != null;
                if (!String.IsNullOrEmpty(orderNo.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS_NUMBER == orderNo.Text);
                }
                if (!String.IsNullOrEmpty(barcode.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text) > 0);
                }
                if (!String.IsNullOrEmpty(txtName.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)) > 0);
                }
                if (!String.IsNullOrEmpty(buyerQuery.QueryArgument))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_SN == buyerQuery.Item.BUYER_SN);
                }
                if (orderDateFrom.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.BO_DATETIME >= orderDateFrom.DateTimeValue);
                }
                if (orderDateTo.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.BO_DATETIME < orderDateTo.DateTimeValue.AddDays(1));
                }

                if (!String.IsNullOrEmpty(marketResource.SelectedValue))
                {
                    int marketSN = int.Parse(marketResource.SelectedValue);
                    queryExpr = queryExpr.And(b => b.MARKET_RESOURCE_SN == marketSN);
                }

                queryExpr = queryExpr.And(b => b.CDS_Document.CurrentStep != (int)Naming.DocumentStepDefinition.已刪除);


                itemList.QueryExpr = queryExpr;
            }
        }

        protected override UserControl _itemList
        {
            get { return itemList; }
        }

        protected virtual void btnTransfer_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request["rbSN"]))
            {
                var item = dsEntity.CreateDataManager().EntityList.Where(b => b.BUYER_ORDERS_SN == int.Parse(Request["rbSN"])).First();
                var preparedItem = new EXCHANGE_GOODS
                {
                    BUYER_ORDERS_SN = item.BUYER_ORDERS_SN
                };

                preparedItem.EXCHANGE_GOODS_INBOUND_DETAILS.AddRange(item.BUYER_ORDERS_DETAILS.ToArray()
                    .Select(b =>
                        new EXCHANGE_GOODS_INBOUND_DETAILS
                        {
                            BO_UNIT_PRICE = b.BO_UNIT_PRICE,
                            GR_WW_QUANTITY = (decimal)b.BO_QUANTITY,
                            GR_WW_DEFECTIVE_AMOUNT = 0,
                            PRODUCTS_SN = b.PRODUCTS_SN
                        }));
                modelItem.DataItem = preparedItem;
                Server.Transfer(ToExchangeGoods.TransferTo);
            }
            else
            {
                this.AjaxAlert("請選擇訂單!!");
                tblNext.Visible = true;
            }
        }

        protected override void btnQuery_Click(object sender, EventArgs e)
        {
            base.btnQuery_Click(sender, e);
            tblNext.Visible = true;
        }

    }
}