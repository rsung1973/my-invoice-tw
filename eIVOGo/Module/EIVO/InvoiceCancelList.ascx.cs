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
    public partial class InvoiceCancelList : System.Web.UI.UserControl
    {
        protected IQueryable<InvoiceItemData> _queryItems;
        protected UserProfileMember _userProfile;
        public decimal tax1 = (decimal)0.05;

        public class InvoiceItemData
        {
            public string  WName { get; set; }
            public Organization Seller { get; set; }
            public InvoiceItem invoiceitem { get; set; }
        }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (!this.IsPostBack)
            {
                //initializeData();
                //ShowResult(false);
                H1.Visible = false;
            }
            //PagingControl1.PageSize = 10;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.modalConfirm.Done += new EventHandler(modalConfirm_Done);
        }

        void modalConfirm_Done(object sender, EventArgs e)
        {
            this.bindData();
        }

        protected void gvInvoice_Sorting(object sender, GridViewSortEventArgs e)
        {
            _sortExpression.AddSortExpression(e, true);
            bindData();
        }

        protected void ShowResult(bool bolShow)
        {
            H1.Visible = true;
        }

        public void BindData()
        {
            bindData();
            H1.Visible = true;
        }

        protected void bindData()
        {
            buildQueryItem();

            if (this.ViewState["sort"] != null)
            {
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[0].SortExpression, b => b.invoiceitem.InvoiceDate);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[1].SortExpression, b => b.Seller.CompanyName);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[2].SortExpression, b => b.Seller.ReceiptNo);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[3].SortExpression, b => b.invoiceitem.CheckNo);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[4].SortExpression, b => b.invoiceitem.No);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[5].SortExpression, b => b.invoiceitem.InvoiceAmountType.TotalAmount);
                _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[6].SortExpression, b => b.WName);
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
        }

        protected virtual void buildQueryItem()
        {
            //設定能顯示登入者公司的發票
            Expression<Func<InvoiceItem, bool>> queryExpr = o => 
                o.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID &&
                o.InvoiceCancellation == null;

           
             #region 過濾使用者所填入的條件

            //發票起日
            if (this.CalendarInputDatePicker1.TextBox.Text.Trim().Length > 0)
                queryExpr = queryExpr.And(o => o.InvoiceDate >= this.CalendarInputDatePicker1.DateTimeValue);
            //發票迄日
            if (this.CalendarInputDatePicker2.TextBox.Text.Trim().Length > 0)
                queryExpr = queryExpr.And(o => o.InvoiceDate <= this.CalendarInputDatePicker2.DateTimeValue);
            //訂單號碼
            if (this.CheckNo.Text.Trim().Length > 0)
                queryExpr = queryExpr.And(o => o.CheckNo == this.CheckNo.Text.Trim());
            //發票號碼
            if (this.InvoiceNo.Text.Trim().Length > 0)
                queryExpr = queryExpr.And(o => o.No == this.InvoiceNo.Text.Trim());
             #endregion

            var mgr = dsEntity.CreateDataManager();
            _queryItems = mgr.GetTable<InvoiceItem>().Where(queryExpr).Select(o => new InvoiceItemData
            {
                Seller = o.Organization,
                WName = o.Donatory.CompanyName,
                invoiceitem = o

            }).OrderByDescending(k => k.invoiceitem.InvoiceDate);
        }
        
        //void signContext_BeforeSign(object sender, EventArgs e)
        //{
        //    SignContext1.DataToSign = String.Format("憑證資料建立時間:{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);
        //    SignContext1.DataToSign += String.Format("憑證資料使用人:{0}", _userProfile.PID + " " + _userProfile.UserName);
        //    SignContext1.DataToSign += String.Format("憑證資料使用功能:{0}", "作廢發票折讓單開立");
        //    SignContext1.DataToSign += String.Format("發票折讓單內容:{0}", this.SignData.Value);
        //}

        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CancelInv":
                    modalConfirm.InvoiceID = int.Parse((String)e.CommandArgument);
                    modalConfirm.Show();
                    break;
                case "Show":
                    modalInvoice.setDetail = (String)e.CommandArgument;
                    modalInvoice.Popup.Show();
                    break;
            }


        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if ((this.CalendarInputDatePicker1.HasValue && this.CalendarInputDatePicker1.DateTimeValue.Year < DateTime.Now.Year) || (this.CalendarInputDatePicker2.HasValue && this.CalendarInputDatePicker2.DateTimeValue.Year < DateTime.Now.Year))
                this.AjaxAlert("作廢電子發票開立日期不可以小於當年度");
            else
            {
                H1.Visible = true;
                bindData();
            }
        }

        protected void gvEntity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}