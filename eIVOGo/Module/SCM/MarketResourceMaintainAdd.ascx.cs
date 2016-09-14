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
    public partial class MarketResourceMaintainAdd : System.Web.UI.UserControl
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

                    this.titleBar.ItemName = "修改網購通路來源";
                    this.actionItem.ItemName = "首頁 > " + this.titleBar.ItemName;
                    this.MARKET_RESOURCE_SN = int.Parse(this.Request["id"]);
                    if (this.MARKET_RESOURCE_SN != null) initdata();
                }
                else
                {

                    this.titleBar.ItemName = "新增網購通路來源";
                    this.actionItem.ItemName = "首頁 > " + this.titleBar.ItemName;
                }
            }
        }

        public int? MARKET_RESOURCE_SN
        {
            get { return (int?)ViewState["_MARKET_RESOURCE_SN"]; }
            set { ViewState["_MARKET_RESOURCE_SN"] = value; }
        }

        protected void initdata()
        {
            try
            {
                this.MARKET_RESOURCE_SN = int.Parse(Request["id"].ToString());
                int id = this.MARKET_RESOURCE_SN.Value;

                using (ProductManager scm = new ProductManager())
                {
                    var data = scm.GetTable<MARKET_RESOURCE>().Where(w => w.MARKET_RESOURCE_SN == id).FirstOrDefault();
                    if (data != null)
                    {
                        this.Name.Text = data.MARKET_RESOURCE_NAME;
                        this.Remark.Text = data.REMARK;
                    }
                    else
                    {
                        this.btnOk.Enabled = false;
                        //this.lblError.Text = "找不到相關資料!!";
                        this.AjaxAlert("找不到相關資料!!");
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

        //protected bool ChecKFieldError()
        //{
        //    bool bolCheckFieldNo = false;
        //    string strErrMsg = "";
        //    if (Name.Text.Trim() == "")
        //    {
        //        bolCheckFieldNo = true;
        //        strErrMsg = strErrMsg + "網購通路來源名稱 ";
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
            if (string.IsNullOrEmpty(this.Name.Text))
            {
                this.AjaxAlert("請輸入網購通路來源名稱!!");
                return;
            }

            try
            {
                int id = -1;

                using (ProductManager scm = new ProductManager())
                {
                    MARKET_RESOURCE data = new MARKET_RESOURCE();
                    if (this.MARKET_RESOURCE_SN != null)
                    {
                        id = MARKET_RESOURCE_SN.Value;
                        data = scm.GetTable<MARKET_RESOURCE>().Where(w => w.MARKET_RESOURCE_SN == id).FirstOrDefault();
                        if (data != null)
                        {
                            data.MARKET_RESOURCE_NAME = this.Name.Text.Trim();
                            data.REMARK = this.Remark.Text.Trim();
                        }
                        else
                        {
                            //this.lblError.Text = "找不到相關資料!!";
                            this.AjaxAlert("找不到相關資料!!");
                        }
                    }
                    else
                    {
                        data.MARKET_RESOURCE_NAME = this.Name.Text.Trim();
                        data.REMARK = this.Remark.Text.Trim();
                        scm.GetTable<MARKET_RESOURCE>().InsertOnSubmit(data);
                    }
                    scm.SubmitChanges();
                    _userProfile["msg"] = "網購通路來源資料完成!!";
                    //this.lblError.Text = "作業成功!!";
                    MARKET_RESOURCE_SN = data.MARKET_RESOURCE_SN;
                    _userProfile["theName"] = Name.Text;
                    Response.Redirect("Market_Resource_Maintain.aspx", true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                //this.lblError.Text = "系統發生錯誤,錯誤原因:" + ex.Message;
                this.AjaxAlert("系統發生錯誤,錯誤原因:" + ex.Message);
            }
        }

        //protected void btnRest_Click(object sender, EventArgs e)
        //{
        //    this.Name.Text = "";
        //    this.Remark.Text = "";
        //}
    }
}