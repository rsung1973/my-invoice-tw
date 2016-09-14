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
using Model.Locale;
using eIVOGo.Module.SCM.View;
using eIVOGo.Module.Base;

namespace eIVOGo.Module.SCM
{
    public partial class PreviewShipment : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;

        protected SCMEntityPreview<CDS_Document> shipment;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        public void PrepareDataFromDB(int shipmentSN)
        {
            shipment.PrepareDataFromDB(shipmentSN);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Server.Transfer(ToPreviewShipment.TransferTo);
        }

        protected  void btnConfirm_Click(object sender, EventArgs e)
        {
            doSave();
        }

        protected virtual void doSave()
        {
            try
            {
                var item = shipment.Item;
                using (InvoiceManager mgr = new InvoiceManager())
                {
                    var invItem = mgr.CreateInvoiceFromShipment(item, _userProfile.CurrentUserRole.OrganizationCategory.CompanyID);
                    item.BUYER_SHIPMENT.INVOICE_SN = invItem.InvoiceID;
                }

                BuyerShipmentManager shipMgr = new BuyerShipmentManager(dsUpdate.CreateDataManager());
                shipment.Item = shipMgr.Save(item, Settings.Default.ShipmentPrefix);
                Page.SetTransferMessage("出貨單已開立!!");
                Server.Transfer(ToInquireShipment.TransferTo);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert("作業無法完成,錯誤原因:" + ex.Message);
            }
        }
    }
}