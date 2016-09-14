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
    public partial class BuyerOrderQuery : InquireEntity<BUYER_ORDERS>
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
                itemList.QueryExpr = b => b.BUYER_ORDERS_SN == _item.BUYER_ORDERS_SN;
            }
            else
            {
                Expression<Func<BUYER_ORDERS, bool>> queryExpr = p => p.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID;
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
                if (!String.IsNullOrEmpty(warehouse.SelectedValue))
                {
                    queryExpr = queryExpr.And(b => b.WAREHOUSE_SN == int.Parse(warehouse.SelectedValue));
                }
                if (!String.IsNullOrEmpty(rbShipment.SelectedValue))
                {
                    queryExpr = queryExpr.And(b => b.BO_SHIPMENT_STATUS == int.Parse(rbShipment.SelectedValue));
                }
                if (!String.IsNullOrEmpty(rbReturn.SelectedValue))
                {
                    queryExpr = queryExpr.And(b => b.BO_RETURNED_EXCHANGE_STATUS == int.Parse(rbReturn.SelectedValue));
                }
                if (!String.IsNullOrEmpty(marketResource.SelectedValue))
                {
                    int marketSN = int.Parse(marketResource.SelectedValue);
                    queryExpr = queryExpr.And(b => b.MARKET_RESOURCE_SN == marketSN);
                    foreach (String attr in Request.Form.AllKeys.Where(k => k.StartsWith("MARKET_ATTR_SN")))
                    {
                        int attrSN = int.Parse(attr.Split('$')[1]);
                        String attrValue = Request[attr];
                        if (!String.IsNullOrEmpty(attrValue))
                        {
                            queryExpr = queryExpr.And(b => b.ORDERS_MARKET_ATTRIBUTE_MAPPING.Count(m => m.MARKET_ATTR_NAME_SN == attrSN && m.MARKET_ATTR_VALUE == attrValue) > 0);
                        }
                    }
                }

                itemList.QueryExpr = queryExpr;
            }
        }

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(ToCreateBuyerOrder.RedirectTo);
        }

        protected override UserControl _itemList
        {
            get { return itemList; }
        }
    }
}