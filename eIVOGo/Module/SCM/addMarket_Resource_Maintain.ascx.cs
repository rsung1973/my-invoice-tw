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
    public partial class addMarket_Resource_Maintain : System.Web.UI.UserControl
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
                    if (this.HiddenField1.Value == "") initdata();
                }
                else
                {

                    this.titleBar.ItemName = "新增網購通路來源";
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
                    var data = scm.GetTable<MARKET_RESOURCE>().Where(w => w.MARKET_RESOURCE_SN == id).FirstOrDefault();
                    if (data != null)
                    {
                        this.Name.Text = data.MARKET_RESOURCE_NAME;
                        this.Remark.Text = data.REMARK;
                    }
                    else
                    {
                        this.btnOk.Enabled = false;
                        this.btnRest .Enabled = false;
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

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                int id = -1;
             
                using (ProductManager scm = new ProductManager())
                {
                    MARKET_RESOURCE data =  new MARKET_RESOURCE ();
                    if (this.HiddenField1.Value != "")
                    {
                        id = int.Parse(this.HiddenField1.Value);
                        data = scm.GetTable<MARKET_RESOURCE>().Where(w => w.MARKET_RESOURCE_SN == id).FirstOrDefault();
                        if (data != null)
                        {
                            data.MARKET_RESOURCE_NAME = this.Name.Text.Trim();
                            data.REMARK = this.Remark.Text.Trim();

                        }
                        else
                        {
                            this.lblError.Text = "找不到相關資料!!";
                        }
                    }
                    else
                    {
                        data.MARKET_RESOURCE_NAME = this.Name.Text.Trim();
                        data.REMARK = this.Remark.Text.Trim();
                        scm.GetTable<MARKET_RESOURCE>().InsertOnSubmit(data);
                        
                        
 
                    }
                    scm.SubmitChanges();
                    this.lblError.Text = "作業成功!!";
                    this.HiddenField1.Value = data.MARKET_RESOURCE_SN.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                this.lblError.Text = "系統發生錯誤,錯誤原因:" + ex.Message;
            }
        }

        protected void btnRest_Click(object sender, EventArgs e)
        {
            this.Name.Text = "";
            this.Remark.Text = "";
        }
    }
}