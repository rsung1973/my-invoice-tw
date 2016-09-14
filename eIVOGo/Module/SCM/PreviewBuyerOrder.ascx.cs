using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using Utility;
using Uxnet.Web.WebUI;
using eIVOGo.Module.SCM.Item;
using Model.Locale;
using Model.SCM;
using eIVOGo.Properties;
using Model.Security.MembershipManagement;
using Business.Helper;

namespace eIVOGo.Module.SCM
{
    public partial class PreviewBuyerOrder : System.Web.UI.UserControl
    {
        protected BUYER_ORDERS _item;
        private UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        private void initializeData()
        {
            BUYER_ORDERS item = (BUYER_ORDERS)buyerOrder.DataItem;
            boDetails.Items = item.BUYER_ORDERS_DETAILS;
            resourcePreview.MappingItems = item.ORDERS_MARKET_ATTRIBUTE_MAPPING;
            item.WAREHOUSE = dsEntity.CreateDataManager().GetTable<WAREHOUSE>().Where(w => w.WAREHOUSE_SN == item.WAREHOUSE_SN).First();
            item.MARKET_RESOURCE = dsEntity.CreateDataManager().GetTable<MARKET_RESOURCE>().Where(m => m.MARKET_RESOURCE_SN == item.MARKET_RESOURCE_SN).First();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(PreviewBuyerOrder_PreRender);
            buyerOrder.ItemType = typeof(BUYER_ORDERS);
            buyerOrder.Load += new EventHandler(buyerOrder_Load);
        }

        void buyerOrder_Load(object sender, EventArgs e)
        {
            initializeData();
        }

        void PreviewBuyerOrder_PreRender(object sender, EventArgs e)
        {
            _item = (BUYER_ORDERS)buyerOrder.DataItem;
            this.DataBind();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                BUYER_ORDERS item = (BUYER_ORDERS)buyerOrder.DataItem;
                BuyerOrdersManager boMgr = new BuyerOrdersManager(dsUpdate.CreateDataManager());
                buyerOrder.DataItem = boMgr.Save(item, buyerPreview.Items, Settings.Default.BuyerOrderPrefix, _userProfile.CurrentUserRole.OrganizationCategory.CompanyID);
                Server.Transfer(ToBuyerOrderQuery.TransferTo);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert(String.Format("無法完成作業,原因:{0}", ex.Message));
            }
        }


        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Server.Transfer(ReturnToEditBuyerOrder.TransferTo);
        }
    }
}