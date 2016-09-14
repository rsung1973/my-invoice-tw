using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Utility;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Model.Security.MembershipManagement;
using Business.Helper;
using Uxnet.Web.WebUI;
using Model.InvoiceManagement;
using System.IO;
using eIVOGo.Module.Common;
using System.Text.RegularExpressions;
using System.Text;
using Model.SCMDataEntity;
using Model.SCM;

namespace eIVOGo.Module.SCM
{
    public partial class SupplierMaintainAdd : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            this.lblError.Text = "";
            if (!this.IsPostBack)
            {
                if (this.Request["id"] != null)
                {

                    this.titleBar.ItemName = "修改供應商資料";
                    this.actionItem.ItemName = "首頁 > " + this.titleBar.ItemName;
                    if (this.HiddenField1.Value == "") initdata();
                }
                else
                {

                    this.titleBar.ItemName = "新增供應商資料";
                    this.actionItem.ItemName = "首頁 > " + this.titleBar.ItemName;
                }
            }

        }

        protected void initdata()
        {
            try
            {
                this.HiddenField1.Value = Request["id"].ToString();
                int id = int.Parse(this.HiddenField1.Value);

                using (ProductManager scm = new ProductManager())
                {
                    var data = scm.GetTable<SUPPLIER>().Where(w => w.SUPPLIER_SN == id).FirstOrDefault();
                    if (data != null)
                    {
                        this.NAME.Text = data.SUPPLIER_NAME;
                        this.ADDR.Text = data.SUPPLIER_ADDRESS;
                        this.BAN.Text = data.SUPPLIER_BAN;
                        this.CONTACT_EMAIL.Text = data.CONTACT_EMAIL;
                        this.CONTACT_NAME.Text = data.SUPPLIER_CONTACT_NAME;
                        this.FAX.Text = data.SUPPLIER_FAX;
                        this.PHONE.Text = data.SUPPLIER_PHONE;

                    }
                    else
                    {
                        this.btnOk.Enabled = false;
                        //this.btnRest.Enabled = false;
                        this.lblError.Text = "找不到相關資料!!";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                this.lblError.Text = "系統發生錯誤,錯誤原因:" + ex.Message;
            }
        }

        //protected bool ChecKFieldError()
        //{
        //    bool bolCheckFieldNo = false;
        //    string strErrMsg = "";
        //    if (BAN.Text.Trim() == "")
        //    {
        //        bolCheckFieldNo = true;
        //        strErrMsg = strErrMsg + "統一編號 ";
        //    }
        //    if (NAME.Text.Trim() == "")
        //    {
        //        bolCheckFieldNo = true;
        //        strErrMsg = strErrMsg + "名稱 ";
        //    }
        //    if (bolCheckFieldNo)
        //        this.lblError.Text = "無法新增原因:" + strErrMsg + "不能空白";   
        //    else
        //        this.lblError.Text = "";  
 
        //    return bolCheckFieldNo;
            
        //}

        protected void btnOk_Click(object sender, EventArgs e)
        {
            //if (ChecKFieldError()) return;
            try
            {
                int id = -1;
                Regex reg = new Regex("[0-9]{8}");

                if (string.IsNullOrEmpty(this.BAN.Text))
                {
                    this.AjaxAlert("請輸入統一編號!!");
                    return;
                }
                else if (this.BAN.Text.Trim().Length != 8)
                {
                    this.AjaxAlert("統一編號長度為8位數!!");
                    return;
                }
                else if (!reg.IsMatch(this.BAN.Text.Trim()))
                {
                    this.AjaxAlert("統一編號為數字!!");
                    return;
                }

                if (string.IsNullOrEmpty(this.NAME.Text))
                {
                    this.AjaxAlert("請輸入名稱!!");
                    return;
                }

                using (ProductManager scm = new ProductManager())
                {
                    SUPPLIER data = new SUPPLIER();
                    if (this.HiddenField1.Value != "")
                    {
                        id = int.Parse(this.HiddenField1.Value);
                        data = scm.GetTable<SUPPLIER>().Where(w => w.SUPPLIER_SN == id).FirstOrDefault();
                        if (data != null)
                        {
                            data.SUPPLIER_NAME = this.NAME.Text;
                            data.SUPPLIER_ADDRESS = this.ADDR.Text;
                            data.SUPPLIER_BAN = this.BAN.Text;
                            data.CONTACT_EMAIL = this.CONTACT_EMAIL.Text;
                            data.SUPPLIER_CONTACT_NAME = this.CONTACT_NAME.Text;
                            data.SUPPLIER_FAX = this.FAX.Text;
                            data.SUPPLIER_PHONE = this.PHONE.Text;

                        }
                        else
                        {
                            this.lblError.Text = "找不到相關資料!!";
                        }
                    }
                    else
                    {
                        data.SUPPLIER_NAME = this.NAME.Text;
                        data.SUPPLIER_ADDRESS = this.ADDR.Text;
                        data.SUPPLIER_BAN = this.BAN.Text;
                        data.CONTACT_EMAIL = this.CONTACT_EMAIL.Text;
                        data.SUPPLIER_CONTACT_NAME = this.CONTACT_NAME.Text;
                        data.SUPPLIER_FAX = this.FAX.Text;
                        data.SUPPLIER_PHONE = this.PHONE.Text;
                        scm.GetTable<SUPPLIER>().InsertOnSubmit(data);
                    }
                    scm.SubmitChanges();
                    this.HiddenField1.Value = data.SUPPLIER_SN.ToString();
                    _userProfile["msg"] = "供應商資料完成!!";
                    _userProfile["theRno"] = BAN.Text;
                    _userProfile["theName"] = NAME.Text;
                    Response.Redirect("Supplier_Maintain.aspx", true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                this.lblError.Text = "系統發生錯誤,錯誤原因:" + ex.Message;
            }
        }

        //protected void btnRest_Click(object sender, EventArgs e)
        //{
        //    //this.NAME.Text = "";
        //    //this.ADDR.Text = ""; 
        //    //this.BAN.Text = ""; 
        //    //this.CONTACT_EMAIL.Text = ""; 
        //    //this.CONTACT_NAME.Text = ""; 
        //    //this.FAX.Text = "";
        //    //this.PHONE.Text = "";
        //}
    }
}