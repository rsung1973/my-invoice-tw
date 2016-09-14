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
    public partial class PreviewReturnedGoods : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        public void PrepareDataFromDB(int returnSN)
        {
            itemView.PrepareDataFromDB(returnSN);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Server.Transfer(ToCreateReturn.TransferTo);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {

                var item = itemView.Item;
                BUYER_SHIPMENT shipment = dsEntity.CreateDataManager().GetTable<BUYER_SHIPMENT>().Where(b => b.BUYER_SHIPMENT_SN == item.BUYER_ORDERS_SN).First();

                using (InvoiceManager mgr = new InvoiceManager())
                {
                    var cancelItem = mgr.CreateInvoiceCancellationFromReturnedGoods(shipment.INVOICE_SN.Value, item.GR_REASON);
                    item.CANCEL_INVOICE_SN = cancelItem.InvoiceID;
                }

                ReturnedGoodsManager returnMgr = new ReturnedGoodsManager(dsUpdate.CreateDataManager());
                itemView.Item = returnMgr.Save(item, Settings.Default.GoodsReturnedPrefix);
                Page.SetTransferMessage("退貨單已開立!!");
                Server.Transfer(ToInquireReturn.TransferTo);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert("作業無法完成,錯誤原因:" + ex.Message);
            }
        }
    }
}