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

namespace eIVOGo.Module.SCM
{
    public partial class PreviewShipmentFromExchange : PreviewShipment
    {

        protected override void doSave()
        {
            try
            {
                var item = shipment.Item;
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