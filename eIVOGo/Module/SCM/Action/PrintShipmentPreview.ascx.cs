using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.WebUI;
using Model.SCMDataEntity;
using Model.Security.MembershipManagement;
using Business.Helper;
using Model.Locale;
using eIVOGo.Module.SCM.View;

namespace eIVOGo.Module.SCM.Action
{
    public partial class PrintShipmentPreview : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Show(int orderSN)
        {
            var item = dsEntity.CreateDataManager().EntityList.Where(d => d.BUYER_SHIPMENT_SN == orderSN).First();
            if (item.CDS_Document.DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder)
            {
                String path = "~/Module/SCM/View/SingleShipmentPreview.ascx";
                btnPrint.PrintControlSource.Add(path);
                SingleShipmentPreview shipmentPreview = (SingleShipmentPreview)this.LoadControl(path);
                shipmentPreview.InitializeAsUserControl(Page);
                Panel3.Controls.AddAt(0, shipmentPreview);
                shipmentPreview.PrepareDataFromDB(orderSN);
                this.ModalPopupExtender.Show();
            }
            else if (item.CDS_Document.DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods)
            {
                String path = "~/Module/SCM/View/SingleShipmentPreviewFromExchange.ascx";
                btnPrint.PrintControlSource.Add(path);
                SingleShipmentPreviewFromExchange shipmentPreview = (SingleShipmentPreviewFromExchange)this.LoadControl(path);
                shipmentPreview.InitializeAsUserControl(Page);
                Panel3.Controls.AddAt(0, shipmentPreview);
                shipmentPreview.PrepareDataFromDB(orderSN);
                this.ModalPopupExtender.Show();
            }
            else if (item.CDS_Document.DocType == (int)Naming.DocumentTypeDefinition.PurchaseOrderReturned)
            {
            }
            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ModalPopupExtender.Hide();
        }

    }
}