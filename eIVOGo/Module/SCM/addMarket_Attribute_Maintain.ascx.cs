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
    public partial class addMarket_Attribute_Maintain : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            this.lblError.Text = "";
            string appname = "網購通路平台屬性";
            if (!this.IsPostBack)
            {
                initMARKET();
                if (this.Request["id"] != null)
                {
                    this.titleBar.ItemName = "修改" + appname;
                    this.actionItem.ItemName = "首頁 > " + this.titleBar.ItemName;
                    if (this.HiddenField1.Value == "") initdata();
                }
                else
                {
                    this.titleBar.ItemName = "新增" + appname;
                    this.actionItem.ItemName = "首頁 > " + this.titleBar.ItemName;
                }
            }
        }

        protected void initMARKET()
        {
            this.ddlMarket.Items.Clear();
            //載入網購通路
            using (ProductManager scm = new ProductManager())
            {
                this.ddlMarket.Items.Add(new ListItem("請選擇", "-1"));
                //目前不考慮多家使用,所以沒過濾
                var data = scm.GetTable<MARKET_RESOURCE>().Where(w => 1 == 1).ToList();
                foreach (var row in data)
                {
                    this.ddlMarket.Items.Add(new ListItem(row.MARKET_RESOURCE_NAME, row.MARKET_RESOURCE_SN.ToString()));
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
                    var data = scm.GetTable<MARKET_ATTRIBUTE>().Where(w => w.MARKET_ATTR_SN == id).FirstOrDefault();
                    if (data != null)
                    {
                        this.Name.Text = data.MARKET_ATTR_NAME;
                        this.ddlMarket.SelectedValue = data.MARKET_RESOURCE_SN.ToString();
                    }
                    else
                    {
                        this.btnOk.Enabled = false;
                        //  this.btnRest.Enabled = false;
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
                this.lblError.Text = "";
                int id = -1;

                if (this.ddlMarket.SelectedValue == "-1")
                {
                    //this.lblError.Text = "請選擇網購通路來源!!";
                    this.AjaxAlert("請選擇網購通路來源!!");
                    return;
                }

                if (string.IsNullOrEmpty(this.Name.Text))
                {
                    this.AjaxAlert("請填入網購通路平台屬性名稱!!");
                    return;
                }

                using (ProductManager scm = new ProductManager())
                {
                    MARKET_ATTRIBUTE data = new MARKET_ATTRIBUTE();
                    if (this.HiddenField1.Value != "")
                    {
                        id = int.Parse(this.HiddenField1.Value);
                        data = scm.GetTable<MARKET_ATTRIBUTE>().Where(w => w.MARKET_ATTR_SN == id).FirstOrDefault();
                        if (data != null)
                        {
                            data.MARKET_ATTR_NAME = this.Name.Text.Trim();
                            data.MARKET_RESOURCE_SN = int.Parse(this.ddlMarket.SelectedValue);
                        }
                        else
                        {
                            //this.lblError.Text = "修改失敗,查無可修改的資料!!";
                            this.AjaxAlert("修改失敗,查無可修改的資料!!");
                        }
                    }
                    else
                    {
                        data = scm.GetTable<MARKET_ATTRIBUTE>().Where(w => w.MARKET_ATTR_NAME == this.Name.Text.Trim()).FirstOrDefault();
                        if (data == null)
                        {
                            data = new MARKET_ATTRIBUTE();
                            data.MARKET_ATTR_NAME = this.Name.Text.Trim();
                            data.MARKET_RESOURCE_SN = int.Parse(this.ddlMarket.SelectedValue);
                            scm.GetTable<MARKET_ATTRIBUTE>().InsertOnSubmit(data);
                        }
                        else
                        {
                            //this.lblError.Text = "新增失敗,網購通路平台屬性已存在!!";
                            this.AjaxAlert("新增失敗,網購通路平台屬性已存在!!");
                        }
                    }
                    if (this.lblError.Text == "")
                    {
                        scm.SubmitChanges();
                        this.HiddenField1.Value = data.MARKET_ATTR_SN.ToString();
                        _userProfile["msg"] = "網購通路平台屬性資料維護完成!!";
                        _userProfile["theName"] = ddlMarket.SelectedItem.Text;
                        _userProfile["theValue"] = ddlMarket.SelectedValue;
                        //Response.Redirect("Market_Attribute_Maintain.aspx", true);
                        Server.Transfer("Market_Attribute_Maintain.aspx", true);
                    }
                    //this.lblError.Text = "作業成功!!";
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
            this.Name.Text = "";
            this.lblError.Text = "";
        }
    }
}
