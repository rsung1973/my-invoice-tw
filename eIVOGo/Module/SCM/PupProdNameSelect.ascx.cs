using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Security.MembershipManagement;
using Model.DataEntity;
using Model.SCMDataEntity;
using Model.Locale;
using Business.Helper;
using Utility;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.UI
{
    public partial class PupProdNameSelect : System.Web.UI.UserControl
    {
        UserProfileMember _userProfile;

        public string setSupSN
        {
            get { return this.HiddenField1.Value; }
            set { this.HiddenField1.Value = value; }
        }

        public AjaxControlToolkit.ModalPopupExtender Popup
        {
            get { return this.ModalPopupExtender; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }        

        private void initializeData()
        {
            var mgr = this.PurchaseDataSource1.CreateDataManager();
            if (!string.IsNullOrEmpty(setSupSN))
            {
                this.chkProdList.Items.Clear();
                this.chkProdList.Items.AddRange(mgr.GetTable<SUPPLIER_PRODUCTS_NUMBER>().Where(sp => sp.SUPPLIER_SN == long.Parse(setSupSN) && sp.PRODUCTS_DATA.PRODUCTS_NAME.Contains(this.txtProdName.Text.Trim())).Select(sp => new ListItem(sp.PRODUCTS_DATA.PRODUCTS_NAME, sp.PRODUCTS_DATA.PRODUCTS_SN.ToString())).ToArray());
            }
            if (this.chkProdList.Items.Count <= 0)
            {
                this.lblError.Text = "無此名稱料品!!";
                this.lblError.Visible = true;
            }
            else
            {
                this.lblError.Visible = false;
            }
        }

        protected void btnQueryProd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtProdName.Text))
            {
                initializeData();
                this.ModalPopupExtender.Show();
            }
            else
            {
                WebMessageBox.AjaxAlert(this, "請輸入料品名稱!!");
            }
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.chkProdList.SelectedValue))
            {
                Uxnet.Web.WebUI.WebMessageBox.Alert(this.Page, "尚未選擇料品!!!");
            }
            else
            {
                var mgr = this.PurchaseDataSource1.CreateDataManager();
                foreach (ListItem li in this.chkProdList.Items)
                {
                    //if (li.Selected == true)
                    //{
                    //    var pod = mgr.GetTable<SUPPLIER_PRODUCTS_NUMBER>().Where(sp => sp.PRODUCTS_SN == long.Parse(li.Value));
                    //    try
                    //    {
                    //        PurchaseOrderDetailsTemp podt = new PurchaseOrderDetailsTemp();
                    //        podt.UID = _userProfile.UID;
                    //        podt.ItemDate = System.DateTime.Now;
                    //        podt.PO_QUANTITY = 0;
                    //        podt.PO_UNIT_PRICE = pod.FirstOrDefault().PRODUCTS_DATA.BUY_PRICE;
                    //        podt.SUPPLIER_PRODUCTS_NUMBER_SN = pod.FirstOrDefault().SUPPLIER_PRODUCTS_NUMBER_SN;
                    //        mgr.GetTable<PurchaseOrderDetailsTemp>().InsertOnSubmit(podt);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Logger.Error(ex);
                    //    }
                    //}
                }
                mgr.SubmitChanges();
            }
        }
    }
}