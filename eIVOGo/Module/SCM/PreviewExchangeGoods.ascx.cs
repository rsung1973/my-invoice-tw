using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using Model.InvoiceManagement;
using Utility;
using Uxnet.Web.WebUI;
using Model.SCM;
using eIVOGo.Properties;
using Model.Security.MembershipManagement;
using Business.Helper;
using eIVOGo.Helper;

namespace eIVOGo.Module.SCM
{
    public partial class PreviewExchangeGoods : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        public void PrepareDataFromDB(int exchangeSN)
        {
            itemView.PrepareDataFromDB(exchangeSN);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Server.Transfer(ToCreateExchange.TransferTo);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                var item = itemView.Item;

                ExchangeGoodsManager mgr = new ExchangeGoodsManager(dsUpdate.CreateDataManager());
                itemView.Item = mgr.Save(item, Settings.Default.GoodsExchangedPrefix);
                Page.SetTransferMessage("換貨單已開立!!");
                Server.Transfer(ToInquireExchange.TransferTo);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert("作業無法完成,錯誤原因:" + ex.Message);
            }
        }
    }
}