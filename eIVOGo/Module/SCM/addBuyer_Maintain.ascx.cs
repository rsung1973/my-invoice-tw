using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Utility;
using Model.DataEntity;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Model.Security.MembershipManagement;
using Business.Helper;
using Uxnet.Web.WebUI;
using Model.InvoiceManagement;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Data.OleDb;
using eIVOGo.Module.Common;
using System.Text.RegularExpressions;
using System.Text;
using Model.SCMDataEntity;
using Model.SCM;

namespace eIVOGo.Module.SCM
{
    public partial class addBuyer_Maintain : System.Web.UI.UserControl
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

                    this.titleBar.ItemName = "修改買受人資料";
                    this.actionItem.ItemName = "首頁 > " + this.titleBar.ItemName;
                    if (this.HiddenField1.Value == "") initdata();
                }
                else
                {

                    this.titleBar.ItemName = "新增買受人資料";
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
                    var data = scm.GetTable<BUYER_DATA>().Where(w => w.BUYER_SN == id).FirstOrDefault();
                    if (data != null)
                    {
                        this.NAME.Text = data.BUYER_NAME;
                        this.ADDR.Text = data.BUYER_ADDRESS;
                        this.BAN.Text = data.BUYER_BAN;
                        this.EMAIL.Text = data.BUYER_EMAIL;
                        //this.CONTACT_NAME.Text = data.;
                        // this.FAX.Text = data.SUPPLIER_FAX;
                        this.PHONE.Text = data.BUYER_PHONE;
                    }
                    else
                    {
                        this.btnOk.Enabled = false;
                        //   this.btnRest.Enabled = false;
                        //this.lblError.Text = "查無資料!!";
                        this.AjaxAlert("查無資料!!");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                //this.lblError.Text = "系統發生錯誤,錯誤原因:" + ex.Message;
                this.AjaxAlert("系統發生錯誤,錯誤原因:" + ex.Message);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.NAME.Text))
                {
                    this.AjaxAlert("請輸入買受人名稱!!");
                    return;
                }

                if (string.IsNullOrEmpty(this.ADDR.Text))
                {
                    this.AjaxAlert("請輸入買受人地址!!");
                    return;
                }

                int id = -1;

                using (ProductManager scm = new ProductManager())
                {
                    BUYER_DATA data = new BUYER_DATA();
                    if (this.HiddenField1.Value != "")
                    {
                        id = int.Parse(this.HiddenField1.Value);
                        data = scm.GetTable<BUYER_DATA>().Where(w => w.BUYER_SN == id).FirstOrDefault();
                        if (data != null)
                        {
                            data.BUYER_NAME = this.NAME.Text;
                            data.BUYER_ADDRESS = this.ADDR.Text;
                            data.BUYER_BAN = this.BAN.Text;
                            data.BUYER_EMAIL = this.EMAIL.Text;
                            data.BUYER_PHONE = this.PHONE.Text;
                        }
                        else
                        {
                            //this.lblError.Text = "修改失敗,查無可修改的資料!!";
                            this.AjaxAlert("修改失敗,查無可修改的資料!!");
                        }
                    }
                    else
                    {
                        data = scm.GetTable<BUYER_DATA>().Where(w => w.BUYER_NAME == NAME.Text.Trim() && w.BUYER_ADDRESS == ADDR.Text.Trim()).FirstOrDefault();
                        if (data == null)
                        {
                            data = new BUYER_DATA();
                            data.BUYER_NAME = this.NAME.Text;
                            data.BUYER_ADDRESS = this.ADDR.Text;
                            data.BUYER_BAN = this.BAN.Text;
                            data.BUYER_EMAIL = this.EMAIL.Text;

                            data.BUYER_PHONE = this.PHONE.Text;
                            scm.GetTable<BUYER_DATA>().InsertOnSubmit(data);
                        }
                        else
                        {
                            //this.lblError.Text = "新增失敗,買受人資料已存在!!";
                            this.AjaxAlert("新增失敗,買受人資料已存在!!");
                        }
                    }
                    if (this.lblError.Text == "")
                    {
                        scm.SubmitChanges();
                        _userProfile["msg"] = "買受人資料維護完成!!";
                        this.HiddenField1.Value = data.BUYER_SN.ToString();
                        _userProfile["theName"] = NAME.Text;
                        Response.Redirect("Buyer_Maintain.aspx", true);
                        //WebMessageBox.Alert(this.Page, "  作業成功");
                        //this.lblError.Text = "作業成功!!";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                //this.lblError.Text = "系統發生錯誤,錯誤原因:" + ex.Message;
                this.AjaxAlert("系統發生錯誤,錯誤原因:" + ex.Message);
            }
        }

        protected void btnRest_Click(object sender, EventArgs e)
        {
            this.NAME.Text = "";
            this.ADDR.Text = "";
            this.BAN.Text = "";
            this.EMAIL.Text = "";
            this.PHONE.Text = "";
        }
    }
}