using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;
using Model.SCMDataEntity;
using Model.InvoiceManagement;
using Uxnet.Com.Utility;
using Utility;
using Business.Helper;
using Model.Security.MembershipManagement;
using System.Linq.Expressions;

namespace eIVOGo.Module.EIVO
{
    public partial class PupopPORDetail : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;

        public AjaxControlToolkit.ModalPopupExtender Popup
        {
            get { return this.ModalPopupExtender; }
        }

        string id = "";

        public string setDetail
        {
            set
            {
                id = value;
                getPORData(id);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        private void getPORData(string id)
        {
            try
            {
                var mgr = this.dsPurchase.CreateDataManager();
                IQueryable<PURCHASE_ORDER_RETURNED> por = mgr.EntityList.Where(p => p.PO_RETURNED_DELETE_STATUS == 0 && p.PURCHASE_ORDER_RETURNED_SN == int.Parse(id));
                this.lblPORNO.Text = por.FirstOrDefault().PURCHASE_ORDER_RETURNED_NUMBER;
                this.lblWardhouse.Text = por.FirstOrDefault().WAREHOUSE.WAREHOUSE_NAME;
                this.gvSupplier.DataSource = this.dsSupplier.CreateDataManager().EntityList.Where(s => s.SUPPLIER_SN == por.FirstOrDefault().SUPPLIER_SN);
                this.gvSupplier.DataBind();
                this.POReturnDetails.Items = por.FirstOrDefault().PURCHASE_ORDER_RETURNED_DETAILS;
                this.POReturnDetails.SUPPLIER_SN = por.FirstOrDefault().SUPPLIER_SN;
                this.POReturnDetails.WAREHOUSE_SN = por.FirstOrDefault().WAREHOUSE_SN;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}