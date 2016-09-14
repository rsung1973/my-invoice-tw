using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;
using Model.Locale;
using eIVOGo.Module.Base;
using Utility;
using System.IO;
using System.Web.UI.HtmlControls;

namespace eIVOGo.Module.SAM.Business
{
    public partial class BuyerList : EntityItemList<EIVOEntityDataContext, InvoiceBuyer>
    {
        private String setHeadName = "";
        public String HeaderName
        {
            set { setHeadName = value; }
        }

        private int getSellerID = 0;
        public int setSellID
        {
            set { getSellerID = value; }
        }

        public GridView gv
        {
            get { return this.gvEntity; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(BuyerList_PreRender);
            this.gvEntity.PreRender += new EventHandler(gvEntity_PreRender);

            doEdit.DoAction = arg =>
            {
                edit(arg);
            };

            doUpdate.DoAction = arg =>
            {
                update(arg);
            };

            doCancel.DoAction = arg =>
            {
                cancel(arg);
            };
        }

        void gvEntity_PreRender(object sender, EventArgs e)
        {
            if (this.gvEntity.EditIndex != -1) this.gvEntity.Rows[this.gvEntity.EditIndex].FindControl("btnCancel").Focus();


            if (!String.IsNullOrEmpty(setHeadName))
            {
                this.gvEntity.Columns[1].Visible = true;
                this.gvEntity.Columns[1].HeaderText = setHeadName;
            }

            if (getSellerID != 0)
            {
                var mgr = dsEntity.CreateDataManager();
                if (mgr.GetTable<OrganizationCategory>().Where(o => o.CompanyID == getSellerID).FirstOrDefault().CategoryID == (int)Naming.B2CCategoryID.Google台灣)
                {
                    this.gvEntity.Columns[1].Visible = true;
                    this.gvEntity.Columns[1].HeaderText = "GoogleID";
                }
                else if (mgr.GetTable<OrganizationCategory>().Where(o => o.CompanyID == getSellerID).FirstOrDefault().CategoryID == (int)Naming.B2CCategoryID.店家發票自動配號)
                {
                    this.gvEntity.Columns[1].Visible = true;
                    this.gvEntity.Columns[1].HeaderText = "客戶ID";
                }
            }
        }

        void BuyerList_PreRender(object sender, EventArgs e)
        {
            if (this.gvEntity.Rows.Count <= 0)
            {
                this.lblError.Visible = true;
            }
            else
            {
                this.lblError.Visible = false;
            }
        }

        protected void edit(string keyValue)
        {
            this.gvEntity.EditIndex = int.Parse(keyValue);
            _rowIndex = int.Parse(keyValue);
        }

        protected int? _rowIndex;
        protected void update(string keyValue)
        {
            try
            {
                string[] a = keyValue.Split(',');
                int invoiceID = int.Parse(a[0]);
                int rowIndex = int.Parse(a[1]);

                String customerName = Request["txtCustomerName"].ToString();
                String contactName = Request["txtContactName"].ToString();
                String phone = Request["txtPhone"].ToString();
                String addr = Request["txtAddr"].ToString();

                var mgr = this.dsEntity.CreateDataManager();
                var invoiceBuyer = mgr.EntityList.Where(i => i.InvoiceID == invoiceID).FirstOrDefault();
                invoiceBuyer.CustomerName = customerName;
                invoiceBuyer.ContactName = contactName;
                invoiceBuyer.Phone = phone;
                invoiceBuyer.Address = addr;
                mgr.SubmitChanges();

                this.gvEntity.EditIndex = -1;
                _rowIndex = rowIndex;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        protected void cancel(string keyValue)
        {
            this.gvEntity.EditIndex = -1;
            _rowIndex = int.Parse(keyValue);
        }
    }
}