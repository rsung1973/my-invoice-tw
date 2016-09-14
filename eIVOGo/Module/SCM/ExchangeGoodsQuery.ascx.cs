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
    public partial class ExchangeGoodsQuery : InquireEntity<EXCHANGE_GOODS>
    {
        protected UserProfileMember _userProfile;

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
                itemList.QueryExpr = b => b.EXCHANGE_GOODS_SN == _item.EXCHANGE_GOODS_SN;
                modelItem.DataItem = null;
            }
            else
            {
                Expression<Func<EXCHANGE_GOODS, bool>> queryExpr = p => p.BUYER_ORDERS.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID;

                if (!String.IsNullOrEmpty(orderNo.Text))
                {
                    queryExpr = queryExpr.And(b => b.EXCHANGE_GOODS_NUMBER == orderNo.Text);
                }
                if (!String.IsNullOrEmpty(buyerOrderNo.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_ORDERS_NUMBER == buyerOrderNo.Text);
                }

                if (!String.IsNullOrEmpty(barcode.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_ORDERS_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text) > 0
                        || b.EXCHANGE_GOODS_INBOUND_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text) > 0
                        || b.EXCHANGE_GOODS_OUTBOND_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text) > 0);
                }
                if (!String.IsNullOrEmpty(txtName.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_ORDERS_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)) > 0
                        || b.EXCHANGE_GOODS_OUTBOND_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)) > 0
                        || b.EXCHANGE_GOODS_INBOUND_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)) > 0);
                }

                if (!String.IsNullOrEmpty(buyerQuery.QueryArgument))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_SN == buyerQuery.Item.BUYER_SN );
                }
                if (orderDateFrom.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.EXCHANGE_GOODS_DATETIME >= orderDateFrom.DateTimeValue);
                }
                if (orderDateTo.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.EXCHANGE_GOODS_DATETIME < orderDateTo.DateTimeValue.AddDays(1));
                }

                if (!String.IsNullOrEmpty(marketResource.SelectedValue))
                {
                    int marketSN = int.Parse(marketResource.SelectedValue);
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.MARKET_RESOURCE_SN == marketSN);
                }

                if (!String.IsNullOrEmpty(inboundStatus.SelectedValue))
                {
                    queryExpr = queryExpr.And(b => b.EG_WW_STATUS == int.Parse(inboundStatus.SelectedValue));
                }
                if (!String.IsNullOrEmpty(outboundStatus.SelectedValue))
                {
                    queryExpr = queryExpr.And(b => b.EG_BS_STATUS == int.Parse(outboundStatus.SelectedValue));
                }
                if (!String.IsNullOrEmpty(docStatus.SelectedValue))
                {
                    queryExpr = queryExpr.And(b => b.CDS_Document.CurrentStep == int.Parse(docStatus.SelectedValue));
                }

                itemList.QueryExpr = queryExpr;
            }
        }

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(ToCreateExchangeGoods.TransferTo);
        }

        protected override UserControl _itemList
        {
            get { return itemList; }
        }
    }
}