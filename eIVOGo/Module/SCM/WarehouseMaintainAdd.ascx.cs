using System;
using System.Linq;

using Model.SCMDataEntity;
using Utility;
using Uxnet.Web.WebUI;
using Model.Security.MembershipManagement;
using Business.Helper;

namespace eIVOGo.Module.SCM
{
    public partial class WarehouseMaintainAdd : System.Web.UI.UserControl
    {
        protected WAREHOUSE _item;
        private UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;

            if (WAREHOUSE_SN.HasValue)
            {
                initializeData();
            }
        }

        public int? WAREHOUSE_SN
        {
            get { return (int?)ViewState["_WAREHOUSE_SN"]; }
            set { ViewState["_WAREHOUSE_SN"] = value; }
        }

        protected void initializeData()
        {
            _item = dsEntity.CreateDataManager().EntityList.Where(e => e.WAREHOUSE_SN == WAREHOUSE_SN).FirstOrDefault();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(WarehouseMaintainAdd_PreRender);
        }

        void WarehouseMaintainAdd_PreRender(object sender, EventArgs e)
        {
            if (!this.IsPostBack && _item != null)
            {
                this.DataBind();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.WAREHOUSE_NAME.Text))
            {
                this.AjaxAlert("請輸入倉儲名稱!!");
                return;
            }
            saveItem();
        }

        private void saveItem()
        {
            try
            {
                var mgr = dsEntity.CreateDataManager();
                if (_item == null)
                {
                    _item = new WAREHOUSE
                    {
                    };
                    mgr.EntityList.InsertOnSubmit(_item);
                }

                _item.WAREHOUSE_NAME = WAREHOUSE_NAME.Text;
                _item.WAREHOUSE_NUMBER = WAREHOUSE_NUMBER.Text;
                _item.WAREHOUSE_ADDRESS = WAREHOUSE_ADDRESS.Text;
                _item.WAREHOUSE_PHONE = WAREHOUSE_PHONE.Text;
                _item.WAREHOUSE_FAX = WAREHOUSE_FAX.Text;
                _item.WAREHOUSE_CONTACT_NAME = WAREHOUSE_CONTACT_NAME.Text;
                _item.CONTACT_EMAIL = CONTACT_EMAIL.Text;

                mgr.SubmitChanges();

                _userProfile["msg"] = "倉儲資料完成!!";
                _userProfile["theName"] = WAREHOUSE_NAME.Text;
                Response.Redirect("Warehouse_Maintain.aspx", true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert("系統發生錯誤,錯誤原因:" + ex.Message);
            }
        }

        //protected void btnRest_Click(object sender, EventArgs e)
        //{
        //    resetFields();
        //}

        //private void resetFields()
        //{
        //    WAREHOUSE_NAME.Text = "";
        //    WAREHOUSE_NUMBER.Text = "";
        //    WAREHOUSE_ADDRESS.Text = "";
        //    WAREHOUSE_PHONE.Text = "";
        //    WAREHOUSE_FAX.Text = "";
        //    WAREHOUSE_CONTACT_NAME.Text = "";
        //    CONTACT_EMAIL.Text = "";
        //}

    }
}