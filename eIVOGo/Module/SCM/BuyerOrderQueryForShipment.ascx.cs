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
    public partial class BuyerOrderQueryForShipment : InquireEntity<CDS_Document>
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
                _item.BUYER_ORDERS = dsEntity.CreateDataManager().GetTable<BUYER_ORDERS>().Where(b => b.BUYER_ORDERS_SN == _item.DocID).First();
            }
        }

        protected override void buildQueryItem()
        {
            //設定能顯示的發票為登入者的公司作廢發票
            //過濾使用者所填入的條件
            if (_item != null)
            {
                itemList.QueryExpr = b => b.BUYER_ORDERS_SN == _item.DocID;
                modelItem.DataItem = null;
            }
            else
            {
                Expression<Func<BUYER_ORDERS, bool>> queryExpr = p => p.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID && p.CDS_Document.BUYER_SHIPMENT == null;
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
            if (!String.IsNullOrEmpty(Request["rbSN"]))
            {
                var item = dsEntity.CreateDataManager().EntityList.Where(b => b.BUYER_ORDERS_SN == int.Parse(Request["rbSN"])).First().CDS_Document;
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



        protected void rbChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            Server.Transfer(rbChange.SelectedValue);
        }
    }
}