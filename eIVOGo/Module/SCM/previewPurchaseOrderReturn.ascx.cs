using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.InvoiceManagement;
using Model.Locale;
using Model.SCMDataEntity;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;
using eIVOGo.Properties;

namespace eIVOGo.Module.SCM
{
    public partial class previewPurchaseOrderReturn : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;
        protected PURCHASE_ORDER_RETURNED _item;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        private void initializeData()
        {
            var item = (PURCHASE_ORDER_RETURNED)DMContainer.DataItem;
            this.POReturnDetais.Items = item.PURCHASE_ORDER_RETURNED_DETAILS.Where(pod => pod.DataStatus == Naming.DataItemStatus.Modified).ToList();
            this.POReturnDetais.SUPPLIER_SN = item.SUPPLIER_SN;
            this.POReturnDetais.WAREHOUSE_SN = item.WAREHOUSE_SN;
            item.WAREHOUSE = this.dsPurchaseReturn.CreateDataManager().GetTable<WAREHOUSE>().Where(w => w.WAREHOUSE_SN == item.WAREHOUSE_SN).First();
            this.gvSupplier.DataSource = this.dsPurchaseReturn.CreateDataManager().GetTable<SUPPLIER>().Where(s => s.SUPPLIER_SN == item.SUPPLIER_SN).ToList();
            this.gvSupplier.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender+=new EventHandler(previewPurchaseOrder_PreRender);
            DMContainer.ItemType = typeof(PURCHASE_ORDER_RETURNED);
            DMContainer.Load += new EventHandler(DMContainer_Load);
        }

        void DMContainer_Load(object sender, EventArgs e)
        {
            initializeData();
        }

        void previewPurchaseOrder_PreRender(object sender, EventArgs e)
        {
            _item = (PURCHASE_ORDER_RETURNED)DMContainer.DataItem;
            this.DataBind();
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            ((PURCHASE_ORDER_RETURNED)DMContainer.DataItem).DataFrom = Naming.DataItemSource.FromPreviousPage;
            Server.Transfer("PurchaseOrderReturned.aspx");
        }

        protected void btnCreatPOReturn_Click(object sender, EventArgs e)
        {
            var item = (PURCHASE_ORDER_RETURNED)DMContainer.DataItem;
            var mgr = this.dsUpdateReturn.CreateDataManager();
            int currentCount = mgr.EntityList.Count(p => p.PO_RETURNED_DATETIME >= DateTime.Today & (p.PURCHASE_ORDER_RETURNED_NUMBER != "" && p.PURCHASE_ORDER_RETURNED_NUMBER != null));
            DateTime now = DateTime.Now;

            Model.SCMDataEntity.CDS_Document cds = new Model.SCMDataEntity.CDS_Document
            {
                CurrentStep = (int)Naming.DocumentStepDefinition.已開立,
                DocType = (int)Naming.DocumentTypeDefinition.PurchaseOrderReturned,
                DocDate = now
            };

            mgr.GetTable<DocumentProcessLog>().InsertOnSubmit(new DocumentProcessLog
            {
                DocID = cds.DocID,
                UID = _userProfile.UID,
                FlowStep = (int)Naming.DocumentStepDefinition.已開立,
                StepDate = now
            });

            PURCHASE_ORDER_RETURNED por = new PURCHASE_ORDER_RETURNED();
            por.CDS_Document = cds;
            por.PURCHASE_ORDER_RETURNED_NUMBER = String.Format("{0}{1:yyyyMMdd}{2:0000}", Settings.Default.PurchaseOrderReturnedPrefix, now, currentCount + 1);
            por.PO_RETURNED_DATETIME = now;
            por.PO_RETURNED_DELETE_STATUS = 0;
            por.PO_RETURNED_STATUS = 0;
            por.PO_RETURN_TOTAL_AMOUNT = item.PURCHASE_ORDER_RETURNED_DETAILS.Sum(p => p.POR_QUANTITY * p.POR_UNIT_PRICE).Value;
            por.SUPPLIER_SN = item.SUPPLIER_SN;
            por.WAREHOUSE_SN = item.WAREHOUSE_SN;

            foreach (var det in item.PURCHASE_ORDER_RETURNED_DETAILS)
            {
                new PURCHASE_ORDER_RETURNED_DETAILS
                {
                    PURCHASE_ORDER_RETURNED = por,
                    POR_QUANTITY = det.POR_QUANTITY,
                    POR_UNIT_PRICE = det.POR_UNIT_PRICE,
                    POR_DEFECTIVE_QUANTITY = det.POR_DEFECTIVE_QUANTITY,
                    PRODUCTS_SN = det.PRODUCTS_SN,
                };
            }
            mgr.EntityList.InsertOnSubmit(por);
            mgr.SubmitChanges();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('採購退貨單開立完成!!'); location.href='PurchaseOrderReturnedMangement.aspx';", true);
        }
    }    
}
