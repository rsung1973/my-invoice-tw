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
    public partial class BuyerOrderQueryToReturn : InquireEntity<GOODS_RETURNED>
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
                Expression<Func<BUYER_ORDERS, bool>> queryExpr = p => p.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID && p.CDS_Document.BUYER_SHIPMENT != null && p.GOODS_RETURNED.Count==0;
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

            tblNext.Visible = true;

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
                var preparedItem = new GOODS_RETURNED
                {
                    BUYER_ORDERS_SN = item.BUYER_ORDERS_SN,
                    GOODS_RETURNED_DATETIME = DateTime.Now
                };

                preparedItem.GOODS_RETURNED_DETAILS.AddRange(item.BUYER_ORDERS_DETAILS.ToArray()
                    .Select(b =>
                        new GOODS_RETURNED_DETAILS
                        {
                            BO_DETAILS_SN = b.BO_DETAILS_SN,
                            GR_QUANTITY = (decimal)b.BO_QUANTITY.Value
                        }));
                modelItem.DataItem = preparedItem;
                Server.Transfer(ToReturnGoods.TransferTo);
            }
            else
            {
                this.AjaxAlert("請選擇訂單!!");
                tblNext.Visible = true;
            }
        }

    }
}