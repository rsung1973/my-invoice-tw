using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using Model.Locale;
using Uxnet.Web.WebUI;
using Model.Security.MembershipManagement;
using Business.Helper;
using eIVOGo.Helper;
using Utility;
using Model.SCM;
using eIVOGo.Properties;

namespace eIVOGo.Module.SCM
{
    public partial class PreviewGoodsReceiptFromPO : EditGoodsReceiptFromPO
    {
        private UserProfileMember _userProfile;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _userProfile = WebPageUtility.UserProfile;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                WarehouseWarrantManager mgr = new WarehouseWarrantManager(dsUpdate.CreateDataManager());
                modelItem.DataItem = mgr.Save(_item, _userProfile.CurrentUserRole.OrganizationCategory.CompanyID, Settings.Default.WarehouseWarrantPrefix);
                Page.SetTransferMessage("入庫單已開立!!");
                Server.Transfer(NextAction.TransferTo);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert("作業無法完成,錯誤原因:" + ex.Message);
            }
        }
    }
}