using System;
using System.Linq.Expressions;
using System.Web.UI;
using System.Linq;

using eIVOGo.Module.Base;
using Model.SCMDataEntity;
using Utility;
using Model.Security.MembershipManagement;
using Business.Helper;

namespace eIVOGo.Module.SCM
{
    public partial class ReturnedGoodsQuery : InquireEntity<GOODS_RETURNED>
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
                itemList.QueryExpr = b => b.GOODS_RETURNED_SN == _item.GOODS_RETURNED_SN;
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

                if (!String.IsNullOrEmpty(returnStatus.SelectedValue))
                {
                    queryExpr = queryExpr.And(b => b.GR_STATUS == int.Parse(returnStatus.SelectedValue));
                }

                itemList.QueryExpr = queryExpr;
            }
        }

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(ToCreateReturnedGoods.TransferTo);
        }

        protected override UserControl _itemList
        {
            get { return itemList; }
        }
    }
}