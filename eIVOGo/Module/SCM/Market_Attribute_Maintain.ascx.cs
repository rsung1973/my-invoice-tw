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
    public partial class Market_Attribute_Maintain : System.Web.UI.UserControl
    {

        public class MARKET_ATTR
        {
            public string MARKET_ATTR_NAME;
            public string MARKET_RESOURCE_NAME;
            public string MARKET_ATTR_SN;
        }
        protected List<MARKET_ATTR> _queryItems;
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
        protected UserProfileMember _userProfile;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            this.lblError.Text = "";
            if(!this.IsPostBack )
                initdata();
        }

        protected void initdata()
        {            
            this.ddlMarket.Items.Clear();
            //載入網購通路
            using (ProductManager scm = new ProductManager())
            {
                this.ddlMarket.Items.Add(new ListItem("全部", "-1"));
                //目前不考慮多家使用,所以沒過濾
                var data = scm.GetTable<MARKET_RESOURCE>().Where(w => 1 == 1).ToList();
                foreach (var row in data)
                {
                    this.ddlMarket.Items.Add(new ListItem(row.MARKET_RESOURCE_NAME, row.MARKET_RESOURCE_SN.ToString()));
                }
            //2011-11-14 Del 換頁顯示資料
            //    if (_userProfile["theName"] != null)
            //    {                    
            //        ddlMarket.SelectedValue = _userProfile["theValue"].ToString() ;
            //        _userProfile["theName"] = null;
            //        _userProfile["theValue"] = null;
            //        BindData();
            //    }
            }
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
                _queryItems = _queryItems.OrderBy(k => k.MARKET_RESOURCE_NAME).ToList();
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
                    var data = from d in scm.GetTable<MARKET_ATTRIBUTE>().Where(w => 1 == 1)
                               select new
                         {
                             d.MARKET_ATTR_SN,
                             d.MARKET_ATTR_NAME,
                             d.MARKET_RESOURCE.MARKET_RESOURCE_NAME
                         };

                    _queryItems = new List<MARKET_ATTR>();
                    foreach (var row in data)
                    {
                        MARKET_ATTR d1 = new MARKET_ATTR();
                        d1.MARKET_ATTR_NAME = row.MARKET_ATTR_NAME;
                        d1.MARKET_ATTR_SN = row.MARKET_ATTR_SN.ToString ();
                        d1.MARKET_RESOURCE_NAME = row.MARKET_RESOURCE_NAME;
                        _queryItems.Add(d1);
                    }
                    if (this.ddlMarket .SelectedValue  != "-1")
                    {
                        _queryItems = _queryItems.Where(w => w.MARKET_RESOURCE_NAME.Trim() == this.ddlMarket.SelectedItem.Text.Trim()).ToList();
                    }
                }
                buildQueryItem();

                if (this.ViewState["sort"] != null)
                { }

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
                //this.lblError.Text = "系統錯誤:" + ex.Message;
                this.AjaxAlert("系統錯誤:" + ex.Message);
            }
            finally
            {

            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
//            Server.Transfer("addMarket_Attribute_Maintain.aspx", true);
            Response .Redirect ("addMarket_Attribute_Maintain.aspx", true);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            bindData();
        }

        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                Response.Redirect ("addMarket_Attribute_Maintain.aspx?id=" + e.CommandArgument.ToString(), false);
            }
            if (e.CommandName == "Delete")
            {
                try
                {
                    using (ProductManager scm = new ProductManager())
                    {
                        var data = scm.GetTable<MARKET_ATTRIBUTE >().Where(w => w.MARKET_ATTR_SN == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
                        if (data != null)
                            scm.GetTable<MARKET_ATTRIBUTE>().DeleteOnSubmit(data);
                        scm.SubmitChanges();
                        bindData();
                        this.AjaxAlert ( "  資料刪除成功");
                        //this.lblError.Text = "資料刪除成功";
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    //this.lblError.Visible = false;
                    //this.divResult.Visible = true;
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
    }
}