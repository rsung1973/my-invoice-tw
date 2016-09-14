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
    public partial class previewPurchaseOrder : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;
        protected PURCHASE_ORDER _item;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        public void PrepareDataFromDB(int orderSN)
        {
            var mgr = this.dsPurchase.CreateDataManager();
            var item = mgr.EntityList.Where(p => p.PO_DELETE_STATUS == 0 && p.PURCHASE_ORDER_SN == orderSN).FirstOrDefault();
            item.DataFrom = Naming.DataItemSource.FromDB;
            DMContainer.DataItem = item;
        }

        private void initializeData()
        {
            var item = (PURCHASE_ORDER)DMContainer.DataItem;
            this.PODetais.Items = item.PURCHASE_ORDER_DETAILS;
            item.WAREHOUSE = this.dsPurchase.CreateDataManager().GetTable<WAREHOUSE>().Where(w => w.WAREHOUSE_SN == item.WAREHOUSE_SN).First();
            this.gvSupplier.DataSource = this.dsPurchase.CreateDataManager().GetTable<SUPPLIER>().Where(s => s.SUPPLIER_SN == item.SUPPLIER_SN).ToList();
            this.gvSupplier.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender+=new EventHandler(previewPurchaseOrder_PreRender);
            DMContainer.ItemType = typeof(PURCHASE_ORDER);
            DMContainer.Load += new EventHandler(DMContainer_Load);
        }

        void DMContainer_Load(object sender, EventArgs e)
        {
            initializeData();
        }

        void previewPurchaseOrder_PreRender(object sender, EventArgs e)
        {
            _item = (PURCHASE_ORDER)DMContainer.DataItem;
            if (!this.IsPostBack)
            {
                if (this._item.PURCHASE_ORDER_SN != 0)
                {
                    if (_item.CDS_Document.CurrentStep == (int)Naming.DocumentStepDefinition.待審核)
                    {
                        this.btnReturn.Visible = false;
                        this.btnCreatPO.Visible = false;
                        this.btnConfirm.Visible = true;
                        this.btnDeny.Visible = true;
                    }
                }
            }
            this.DataBind();
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            ((PURCHASE_ORDER)DMContainer.DataItem).DataFrom = Naming.DataItemSource.FromPreviousPage;
            Server.Transfer("PurchaseOrder.aspx");
        }

        protected void btnCreatPO_Click(object sender, EventArgs e)
        {
            var item = (PURCHASE_ORDER)DMContainer.DataItem;
            var mgr = this.dsUpdate.CreateDataManager();
            int currentCount = mgr.EntityList.Count(p => p.PO_DATETIME >= DateTime.Today & (p.PURCHASE_ORDER_NUMBER != "" && p.PURCHASE_ORDER_NUMBER != null));
            DateTime now = DateTime.Now;

            var dataItem = item.DataFrom == Naming.DataItemSource.FromDB ? mgr.EntityList.Where(p => p.PURCHASE_ORDER_SN == item.PURCHASE_ORDER_SN).FirstOrDefault() : null;
            if (dataItem != null)
            {
                dataItem.CDS_Document.CurrentStep = (int)Naming.DocumentStepDefinition.待審核;
                dataItem.PO_TOTAL_AMOUNT = dataItem.PURCHASE_ORDER_DETAILS.Sum(p => p.PO_QUANTITY * p.PO_UNIT_PRICE).Value;

                mgr.GetTable<DocumentProcessLog>().InsertOnSubmit(new DocumentProcessLog
                {
                    DocID = dataItem.CDS_Document.DocID,
                    UID = _userProfile.UID,
                    FlowStep = (int)Naming.DocumentStepDefinition.待審核,
                    StepDate = now
                });
            }
            else
            {
                Model.SCMDataEntity.CDS_Document cds = new Model.SCMDataEntity.CDS_Document 
                {
                    CurrentStep = (int)Naming.DocumentStepDefinition.待審核,
                    DocType = (int)Naming.DocumentTypeDefinition.PurchaseOrder,
                    DocDate = now
                };

                new DocumentProcessLog
                {
                    CDS_Document = cds,
                    UID = _userProfile.UID,
                    FlowStep = (int)Naming.DocumentStepDefinition.待審核,
                    StepDate = now
                };

                dataItem = new PURCHASE_ORDER();
                dataItem.CDS_Document = cds;
                dataItem.PURCHASE_ORDER_NUMBER = String.Format("{0}{1:yyyyMMdd}{2:0000}", Settings.Default.PurchaseOrderPrefix, now, currentCount + 1);
                dataItem.PO_CLOSED_MODE = 0;
                dataItem.PO_DATETIME = now;
                dataItem.PO_DELETE_STATUS = 0;
                dataItem.PO_STATUS = 0;
                dataItem.PO_TOTAL_AMOUNT = item.PURCHASE_ORDER_DETAILS.Sum(p => p.PO_QUANTITY * p.PO_UNIT_PRICE).Value;
                dataItem.SUPPLIER_SN = item.SUPPLIER_SN;
                dataItem.WAREHOUSE_SN = item.WAREHOUSE_SN;

                foreach (var det in item.PURCHASE_ORDER_DETAILS)
                {
                    new PURCHASE_ORDER_DETAILS
                    {
                        PURCHASE_ORDER = dataItem,
                        PO_QUANTITY = det.PO_QUANTITY,
                        PO_UNIT_PRICE = det.PO_UNIT_PRICE,
                        PRODUCTS_SN = det.PRODUCTS_SN,
                        SUPPLIER_SN = item.SUPPLIER_SN,
                    };
                }
                mgr.EntityList.InsertOnSubmit(dataItem);
            }
            mgr.SubmitChanges();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('採購單開立完成!!'); location.href='PurchaseOrderManagement.aspx';", true);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            var item = (PURCHASE_ORDER)DMContainer.DataItem;
            var mgr = this.dsUpdate.CreateDataManager();
            var dataItem = mgr.EntityList.Where(p => p.PURCHASE_ORDER_SN == item.PURCHASE_ORDER_SN).FirstOrDefault();
            dataItem.CDS_Document.CurrentStep = (int)Naming.DocumentStepDefinition.已開立;
            mgr.GetTable<DocumentProcessLog>().InsertOnSubmit(new DocumentProcessLog
            {
                DocID = dataItem.CDS_Document.DocID,
                UID = _userProfile.UID,
                FlowStep = (int)Naming.DocumentStepDefinition.已開立,
                StepDate = DateTime.Now
            });
            ///料品、倉儲對應自動關聯
            attachProdAndWarehouse(mgr.GetTable<PRODUCTS_DATA>().ToList(), dataItem);

            mgr.SubmitChanges();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('審核完成!!'); location.href='PurchaseOrderVerify.aspx';", true);
        }

        protected void btnDeny_Click(object sender, EventArgs e)
        {
            var item = (PURCHASE_ORDER)DMContainer.DataItem;
            var mgr = this.dsUpdate.CreateDataManager();
            var poData = mgr.EntityList.Where(d => d.PURCHASE_ORDER_SN == item.PURCHASE_ORDER_SN).FirstOrDefault();
            poData.CDS_Document.CurrentStep = (int)Naming.DocumentStepDefinition.已退回;
            mgr.GetTable<DocumentProcessLog>().InsertOnSubmit(new DocumentProcessLog
            {
                DocID = poData.CDS_Document.DocID,
                UID = _userProfile.UID,
                FlowStep = (int)Naming.DocumentStepDefinition.已退回,
                StepDate = DateTime.Now
            });
            mgr.SubmitChanges();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('審核完成!!'); location.href='PurchaseOrderVerify.aspx';", true);
        }

        private void attachProdAndWarehouse(IList<PRODUCTS_DATA> podData, PURCHASE_ORDER dataItem)
        {
            foreach (var it in dataItem.PURCHASE_ORDER_DETAILS)
            {
                if (podData.Where(p => p.PRODUCTS_SN == it.PRODUCTS_SN).FirstOrDefault().PRODUCTS_WAREHOUSE_MAPPING.Count(w => w.WAREHOUSE_SN == dataItem.WAREHOUSE_SN) <= 0)
                {
                    podData.Where(p => p.PRODUCTS_SN == it.PRODUCTS_SN).FirstOrDefault().PRODUCTS_WAREHOUSE_MAPPING.Add(new PRODUCTS_WAREHOUSE_MAPPING
                    {
                        WAREHOUSE_SN = dataItem.WAREHOUSE_SN,
                        PRODUCTS_DEFECTIVE_AMOUNT = 0,
                        PRODUCTS_PLAN_AMOUNT = 0,
                        PRODUCTS_SAFE_AMOUNT_PERCENTAGE = 0,
                        PRODUCTS_TOTAL_AMOUNT = 0
                    });
                }
            }
        }
    }    
}
