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

namespace eIVOGo.Module.EIVO
{
    public partial class InvoiceAllowanceCancell : System.Web.UI.UserControl
    {
        protected IQueryable<InvoiceAllowences> _queryItems;
        public decimal tax1 = (decimal)0.05;

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

        public DateTime? DateFrom
        {
            get
            {
                return (DateTime?)ViewState["from"];
            }
            set
            {
                ViewState["from"] = value;
            }
        }
        public DateTime? DateTo
        {
            get
            {
                return (DateTime?)ViewState["to"];
            }
            set
            {
                ViewState["to"] = value;
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
            //  this.SignContext1.Launcher = this.btnAllowance;



        }

        protected void gvInvoice_Sorting(object sender, GridViewSortEventArgs e)
        {
            _sortExpression.AddSortExpression(e, true);
            bindData();
        }

        protected void bindData()
        {
            try
            {
            buildQueryItem();

            if (this.ViewState["sort"] != null)
            {
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[0].SortExpression, b => b.invallowance.AllowanceDate);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[1].SortExpression, b => b.Seller.CompanyName);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[2].SortExpression, b => b.Seller.ReceiptNo);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[3].SortExpression, b => b.invoiceitem.CheckNo);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[4].SortExpression, b => b.invoiceitem.No);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[5].SortExpression, b => b.invallowance.AllowanceNumber);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[6].SortExpression, b => b.invallowance.TotalAmount);
                


            }
            
                if (gvEntity.AllowPaging)
                {
                    gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(gvEntity, 0);
                    gvEntity.DataSource = _queryItems.Skip(gvEntity.PageSize * gvEntity.PageIndex).Take(gvEntity.PageSize);
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
            if (_queryItems.Count() == 0)
            {
                this.lblError.Visible = true; this.lblError.Text = "查無資料!!";
            }
            
            }
            catch (Exception ex)
            {
                this.lblError.Visible = true; this.lblError.Text = "系統錯誤:" + ex.Message;
            }
            finally
            {

            }
        }

        protected virtual void buildQueryItem()
        {
            //設定能顯示的發票為登入者的公司發票折讓單
            //Expression<Func<InvoiceAllowance, bool>> queryExpr = o => o.SellerId  == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID.ToString() && o.InvoiceAllowanceCancellation == null    ;
            Expression<Func<InvoiceAllowance, bool>> queryExpr = o => o.CDS_Document.DocumentOwner.OwnerID  == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID && o.InvoiceAllowanceCancellation == null;
            
            
            //過濾使用者所填入的條件

            //折讓區間起
            if (this.CalendarInputDatePicker1.TextBox.Text.Trim().Length > 0)
                queryExpr = queryExpr.And(o => o.AllowanceDate >= this.CalendarInputDatePicker1.DateTimeValue);
            //折讓區間至
            if (this.CalendarInputDatePicker2.TextBox.Text.Trim().Length > 0)
                queryExpr = queryExpr.And(o => o.AllowanceDate <= this.CalendarInputDatePicker2.DateTimeValue);
            //訂單號碼
            if (this.CheckNo.Text.Trim().Length > 0)
                queryExpr = queryExpr.And(o => o.InvoiceItem.CheckNo == this.CheckNo.Text.Trim());
            //發票號碼
            if (this.InvoiceNo.Text.Trim().Length > 0)
                queryExpr = queryExpr.And(o => o.InvoiceItem.No == this.InvoiceNo.Text.Trim());
            //折讓單號碼
            if (this.AllowanceNo .Text.Trim().Length > 0)
                queryExpr = queryExpr.And(o => o.AllowanceNumber == this.AllowanceNo.Text.Trim());

            var mgr = dsEntity.CreateDataManager();
            _queryItems = mgr.GetTable<InvoiceAllowance>().Where(queryExpr).Select(o => new InvoiceAllowences
            {
                Seller = o.InvoiceItem.Organization,
                invallowance = o,
                invoiceitem = o.InvoiceItem 

            }).OrderByDescending(k => k.invallowance.AllowanceDate);
        }

        public void BindData()
        {
            bindData();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if ((this.CalendarInputDatePicker1.HasValue && this.CalendarInputDatePicker1.DateTimeValue.Year < DateTime.Now.Year) || (this.CalendarInputDatePicker2.HasValue && this.CalendarInputDatePicker2.DateTimeValue.Year < DateTime.Now.Year))
                this.AjaxAlert("作廢發票折讓日期不可以小於當年度");
            else
                bindData();
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            modalConfirm.Done += new EventHandler(modalConfirm_Done);
        }

        void modalConfirm_Done(object sender, EventArgs e)
        {
            bindData();
        }

        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                modalConfirm.AllowanceID = int.Parse((String)e.CommandArgument);
                modalConfirm.Show();
            }
        }
        public class InvoiceAllowences
        {
            public InvoiceAllowance invallowance { get; set; }
            public Organization  Seller { get; set; }
            public InvoiceItem invoiceitem { get; set; }

        }
    }
}