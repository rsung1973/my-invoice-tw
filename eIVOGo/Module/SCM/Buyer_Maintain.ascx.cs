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
    public partial class Buyer_Maintain : System.Web.UI.UserControl
    {

        protected List<BUYER_DATA> _queryItems;
        protected internal Dictionary<String, SortDirection> _sortExpression
        {
            get
            {
                if (ViewState["sort"] == null)
                {
                    ViewState["sort"] = new Dictionary<String, SortDirection>();
                }
                return (Dictionary<String, SortDirection>)ViewState["sort"];
            }
            set
            {
                ViewState["sort"] = value;
            }
        }

        public int? RecordCount
        {
            get
            {
                if (_queryItems == null)
                    return 0;
                else
                    return _queryItems.Count();
            }
        }



        internal GridView DataList
        {
            get
            {
                return gvEntity;
            }
        }

        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            this.lblError.Text = "";
            if (!this.IsPostBack)
                initdata();
        }

        protected void initdata()
        {
            //2011-11-14 Del 換頁顯示資料
            _userProfile["theName"] = null;
            //if (_userProfile["theName"] != null)
            //{
            //    txtName.Text = _userProfile["theName"].ToString();
            //    _userProfile["theName"] = null;
            //    BindData();
            //}
        }


        void btnPrint_BeforeClick(object sender, EventArgs e)
        {
            this.gvEntity.AllowPaging = false;
        }
        protected virtual void buildQueryItem()
        {
            //設定能顯示的發票為登入者的公司作廢發票

            //過濾使用者所填入的條件
            if (_queryItems != null)
                _queryItems = _queryItems.OrderBy(k => k.BUYER_NAME).ToList();
        }

        public void BindData()
        {
            bindData();
        }
        protected void bindData()
        {
            try
            {
                using (ProductManager scm = new ProductManager())
                {
                    //目前不考慮多家使用,所以沒過濾
                    _queryItems = scm.GetTable<BUYER_DATA>().Where(w => 1 == 1).ToList();
                    if (this.txtName .Text  != "")
                    {
                        _queryItems = _queryItems.Where(w => w.BUYER_NAME.Contains(this.txtName.Text.Trim ())).ToList();
                    }
                }

                buildQueryItem();

                if (this.ViewState["sort"] != null)
                {


                }
                if (_queryItems.Count() == 0)
                {
                    gvEntity.DataSource = _queryItems;
                    gvEntity.DataBind();
                    this.divResult.Visible = true;
                    this.lblError.Visible = true;
                    this.lblError.Text = "查無資料!!!";
                }
                else
                {
                    if (gvEntity.AllowPaging)
                    {
                        gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(gvEntity, 0);
                        gvEntity.DataSource = _queryItems.Skip(gvEntity.PageSize * gvEntity.PageIndex).Take(gvEntity.PageSize);
                        gvEntity.DataSource = _queryItems;
                        gvEntity.DataBind();
                        gvEntity.SetPageIndex("pagingList", _queryItems.Count());
                    }
                    else
                    {
                        gvEntity.DataSource = _queryItems;
                        gvEntity.DataBind();
                    }
                    this.lblError.Visible = false;
                    this.divResult.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                //this.lblError.Text = "系統錯誤:" + ex.Message;
                this.AjaxAlert("系統錯誤:" + ex.Message);
            }
            finally
            {

            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("addBuyer_Maintain.aspx", false);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            bindData();
        }

        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                Response.Redirect ("addBuyer_Maintain.aspx?id=" + e.CommandArgument.ToString(), false);
            }
            if (e.CommandName == "Delete")
            {
                try
                {
                    using (ProductManager scm = new ProductManager())
                    {
                        var data = scm.GetTable<BUYER_DATA>().Where(w => w.BUYER_SN == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
                        if (data != null)
                            scm.GetTable<BUYER_DATA>().DeleteOnSubmit(data);
                        scm.SubmitChanges();
                        bindData();
                        this.AjaxAlert("  資料刪除成功");
                        //this.lblError.Text = "資料刪除成功";
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    //this.lblError.Text = "系統發生錯誤,錯誤原因:" + ex.Message;
                    this.AjaxAlert("系統發生錯誤,錯誤原因:" + ex.Message);
                }
                finally
                {

                }
            }
        }

        protected void gvEntity_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
        }
    }
}