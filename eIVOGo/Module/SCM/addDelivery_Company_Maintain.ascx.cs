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
    public partial class addDelivery_Company_Maintain : System.Web.UI.UserControl
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
                    this.titleBar.ItemName = "修改宅配公司";
                    this.actionItem.ItemName = "首頁 > " + this.titleBar.ItemName;
                    if (this.HiddenField1.Value == "") initdata();
                }
                else
                {
                    this.titleBar.ItemName = "新增宅配公司";
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
                    var data = scm.GetTable<DELIVERY_COMPANY>().Where(w => w.DELIVERY_COMPANY_SN == id).FirstOrDefault();
                    if (data != null)
                    {
                        this.NAME.Text = data.DELIVERY_COMPANY_NAME;
                        this.ADDR.Text = data.DELIVERY_COMPANY_ADDRESS;
                        this.BAN.Text = data.DELIVERY_COMPANY_BAN;
                        this.CONTACT_EMAIL.Text = data.CONTACT_EMAIL;
                        this.CONTACT_NAME.Text = data.CONTACT_NAME;
                        this.FAX.Text = data.DELIVERY_COMPANY_FAX;
                        this.PHONE.Text = data.DELIVERY_COMPANY_PHONE;
                        if (data.PRINT_FLAG == 1)
                            this.RadioButton1.Checked = true;
                        else
                            this.RadioButton2.Checked = true; 
                    }
                    else
                    {
                        this.btnOk.Enabled = false;
                       // this.btnRest.Enabled = false;
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
                int id = -1;

                if (string.IsNullOrEmpty(this.NAME.Text))
                {
                    this.AjaxAlert("請輸入名稱!!");
                    return;
                }

                using (ProductManager scm = new ProductManager())
                {
                    DELIVERY_COMPANY data = new DELIVERY_COMPANY();
                    if (this.HiddenField1.Value != "")
                    {
                        id = int.Parse(this.HiddenField1.Value);
                        data = scm.GetTable<DELIVERY_COMPANY >().Where(w => w.DELIVERY_COMPANY_SN == id).FirstOrDefault();
                        if (data != null)
                        {
                            data.DELIVERY_COMPANY_NAME = this.NAME.Text;
                            data.DELIVERY_COMPANY_ADDRESS = this.ADDR.Text;
                            data.DELIVERY_COMPANY_BAN = this.BAN.Text;
                            data.CONTACT_EMAIL = this.CONTACT_EMAIL.Text;
                            data.CONTACT_NAME = this.CONTACT_NAME.Text;
                            data.DELIVERY_COMPANY_FAX = this.FAX.Text;
                            data.DELIVERY_COMPANY_PHONE = this.PHONE.Text;
                            if (this.RadioButton1.Checked == true)
                                data.PRINT_FLAG = 1;
                            else
                                data.PRINT_FLAG = 0;
                        }
                        else
                        {
                            //this.lblError.Text = "修改失敗,查無可修改的資料!!";
                            this.AjaxAlert("修改失敗,查無可修改的資料!!");
                        }
                    }
                    else
                    {
                        data = scm.GetTable<DELIVERY_COMPANY >().Where(w => w.DELIVERY_COMPANY_NAME == this.NAME.Text.Trim ()).FirstOrDefault();
                        if (data == null)
                        {
                            data = new DELIVERY_COMPANY();
                            data.DELIVERY_COMPANY_NAME = this.NAME.Text;
                            data.DELIVERY_COMPANY_ADDRESS = this.ADDR.Text;
                            data.DELIVERY_COMPANY_BAN = this.BAN.Text; // 統一編號
                            data.CONTACT_EMAIL = this.CONTACT_EMAIL.Text;
                            data.CONTACT_NAME = this.CONTACT_NAME.Text;
                            data.DELIVERY_COMPANY_FAX = this.FAX.Text;
                            data.DELIVERY_COMPANY_PHONE = this.PHONE.Text;
                            if (this.RadioButton1.Checked == true)
                                data.PRINT_FLAG = 1;
                            else
                                data.PRINT_FLAG = 0;
                            scm.GetTable<DELIVERY_COMPANY>().InsertOnSubmit(data);
                        }
                        else
                        {
                            //this.lblError.Text = "新增失敗,宅配公司資料已存在!!";
                            this.AjaxAlert("新增失敗,宅配公司資料已存在!!");
                        }
                    }
                    if (this.lblError.Text == "")
                    {
                        scm.SubmitChanges();
                        _userProfile["msg"] = "宅配公司資料維護完成!!";
                        this.HiddenField1.Value = data.DELIVERY_COMPANY_SN.ToString();
                        _userProfile["theName"] = NAME.Text;
                        Response.Redirect("Delivery_Company_Maintain.aspx", true);
                        //Server.Transfer("Delivery_Company_Maintain.aspx", true);
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
            this.ADDR.Text = ""; ;
            this.BAN.Text = ""; ;
            this.CONTACT_EMAIL.Text = ""; ;
            this.CONTACT_NAME.Text = ""; ;
            this.FAX.Text = "";
            this.PHONE.Text = ""; 
        }
    }
}