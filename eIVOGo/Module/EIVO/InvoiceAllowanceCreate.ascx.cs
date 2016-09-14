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
using System.Text;

namespace eIVOGo.Module.EIVO
{
    public partial class InvoiceAllowanceCreate : System.Web.UI.UserControl, IPostBackEventHandler
    {
        protected IQueryable<InvoiceItems> _queryItems;
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
            this.SignContext1.Launcher = this.btnAllowance;
            
           

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
                    _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[0].SortExpression, b => b.InvoiceItem.InvoiceDate);
                    _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[1].SortExpression, b => b.Seller.CompanyName);
                    _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[2].SortExpression, b => b.Seller.ReceiptNo);
                    _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[3].SortExpression, b => b.InvoiceItem.CheckNo);
                    _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[4].SortExpression, b => b.InvoiceItem.No);
                    _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[5].SortExpression, b => b.TotalAmount);
                    _queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[6].SortExpression, b => b.Donatory.CompanyName);


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
            //設定能顯示的發票為登入者的公司發票
            //Expression<Func<InvoiceItem, bool>> queryExpr = o => o.SellerID  == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID;
            //設定能顯示的發票為登入者的擁有權發票
            Expression<Func<InvoiceItem , bool>> queryExpr = o => o.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID ;
            //過濾使用者所填入的條件
            queryExpr = queryExpr.And(o => o.InvoiceDate.Value .Year  >= DateTime.Now .Year );
            //發票區間起
            if (this.CalendarInputDatePicker1.TextBox.Text.Trim().Length > 0)
                queryExpr = queryExpr.And(o => o.InvoiceDate >= this.CalendarInputDatePicker1.DateTimeValue );
            //發票區間至
            if (this.CalendarInputDatePicker2.TextBox.Text.Trim().Length > 0)
                queryExpr = queryExpr.And(o => o.InvoiceDate <= this.CalendarInputDatePicker2.DateTimeValue);
            //訂單號碼
            if (this.CheckNo.Text.Trim().Length > 0  )
                queryExpr = queryExpr.And(o => o.CheckNo == this.CheckNo.Text.Trim());
            //發票號碼
            if (this.InvoiceNo.Text.Trim().Length > 0)
                queryExpr = queryExpr.And(o => o.No == this.InvoiceNo.Text.Trim());

            var mgr = dsEntity.CreateDataManager() ;
            _queryItems = mgr.EntityList.Where(queryExpr).Select(o => new InvoiceItems
            {
                Seller = o.Organization,
                InvoiceItem = o,
                Donatory = o.Donatory,
                TotalAmount=o.InvoiceAmountType.TotalAmount.HasValue ?  o.InvoiceAmountType.TotalAmount.Value : 0
            }).OrderByDescending(k => k.InvoiceItem.InvoiceDate);
        }

        public void BindData()
        {
            bindData();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if ((this.CalendarInputDatePicker1.HasValue && this.CalendarInputDatePicker1.DateTimeValue.Year < DateTime.Now.Year ) || (this.CalendarInputDatePicker2.HasValue && this.CalendarInputDatePicker2.DateTimeValue.Year < DateTime.Now.Year))
                this.AjaxAlert("發票折讓日期不可以小於當年度");
            else
                bindData();
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SignContext1.BeforeSign += new EventHandler(signContext_BeforeSign);
        }

        void signContext_BeforeSign(object sender, EventArgs e)
        {
            this.SignData.Value = "";
            foreach (GridViewRow row in this.GridView1.Rows)
            {
                CheckBox chkbox = (CheckBox)row.FindControl("chk");
                HiddenField lb = (HiddenField)row.FindControl("Item");
                if (chkbox.Checked)
                {
                    this.SignData.Value += lb.Value + "\r\n <br/>";

                }
            }
            var item = dsEntity.CreateDataManager().GetTable<InvoiceItem >().Where(a => a.No  == this.InvNo.Value ).FirstOrDefault();
            if (item != null)
            {
                StringBuilder sb = new StringBuilder("您欲開立的折讓單證明資料如下:\r\n <br/>");
                sb.Append("作業時間:").Append(DateTime.Now.ToString()).Append("\r\n <br/>");
                sb.Append("發票開立日期:").Append(ValueValidity.ConvertChineseDateString(item.InvoiceDate)).Append("\r\n <br/>");
                sb.Append("發票號碼:").Append(item.TrackCode).Append(item.No).Append("\r\n <br/>");
                sb.Append("開立發票營業人名稱:").Append(item.Organization.CompanyName).Append("\r\n <br/>");
                sb.Append("統一編號:").Append(item.Organization.ReceiptNo).Append("\r\n <br/>");
                SignContext1.DataToSign += String.Format("發票折讓單內容:{0} \r\n <br/> ", sb.Append (this.SignData.Value));
            }
        }
        protected void btnAllowance_Click(object sender, EventArgs e)
        {
           
           

                if (SignContext1.Verify ())
                {
                   using(InvoiceManager mgr = new InvoiceManager() )
                   {
                       CDS_Document doc = new CDS_Document ();
                       doc.DocType = 11;
                       doc.DocDate = DateTime.Now ;
                       mgr.GetTable <CDS_Document >().InsertOnSubmit (doc);
                       InvoiceAllowance invoiceallowance = new InvoiceAllowance();
                       invoiceallowance.CDS_Document  = doc;
                       invoiceallowance.AllowanceDate = DateTime.Now;
                       invoiceallowance.AllowanceNumber = DateTime.Now.ToShortDateString ();
                       invoiceallowance.SellerId = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID.ToString ();
                       InvoiceItem invitem= mgr.GetTable<InvoiceItem>().Where(w => w.No == this.InvNo.Value.Trim()).FirstOrDefault();
                       invoiceallowance.InvoiceItem =invitem ;
                       Int16 no = 0;
                       foreach (GridViewRow row in this.GridView1.Rows)
                       { 
                           CheckBox chkbox = (CheckBox)row.FindControl("chk");
                           HiddenField lb = (HiddenField)row.FindControl("ItemID");
                           if (chkbox.Checked)
                           {
                               InvoiceProductItem invprditem = mgr.GetTable<InvoiceProductItem>().Where(w => w.ItemID == int.Parse ( lb.Value)).FirstOrDefault();
                               InvoiceAllowanceItem invAitem = new InvoiceAllowanceItem();
                               invAitem.InvoiceNo = this.InvNo.Value.Trim();
                               //算這筆項目的總金額
                               invAitem.Amount = invprditem.CostAmount * invprditem.Piece.Value;
                               invAitem.Piece = invprditem.Piece;
                               invAitem.PieceUnit = invprditem.PieceUnit;
                               invAitem.TaxType = invprditem.TaxType;
                               invAitem.ItemNo = invprditem.ItemNo;
                               invAitem.InvoiceDate = invitem.InvoiceDate;
                               //算這筆單價的稅額
                               invAitem.Tax = invAitem.Amount * tax1;
                               invoiceallowance.TotalAmount += invAitem.Amount;
                               invoiceallowance.TaxAmount += invAitem.Tax;
                               invAitem.No = (short)(no + Int16 .Parse ("1")) ;
                               invAitem.InvoiceProductItem = invprditem;
                               mgr.GetTable<InvoiceAllowanceItem>().InsertOnSubmit(invAitem);
                               InvoiceAllowanceDetail invoiceallowancedetail = new InvoiceAllowanceDetail();
                               invoiceallowancedetail.InvoiceAllowance = invoiceallowance;
                               invoiceallowancedetail.InvoiceAllowanceItem = invAitem;
                               mgr.GetTable<InvoiceAllowanceDetail>().InsertOnSubmit(invoiceallowancedetail);
                           }
                       }
                       mgr.SubmitChanges();
                     

                   }
                    this.AjaxAlert( "折讓單開立完成!!");
                }
                else
                {
                    this.AjaxAlert("簽章驗證失敗!!");
                }
           
        }
        protected void btnManager_Click(object sender, EventArgs e)
        {
          

        }

        protected void btnCancell_Click(object sender, EventArgs e)
        {
            this.divQueryList.Visible = true;
            this.divWorkForm.Visible = false;
            bindData();
        }

        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this.divQueryList.Visible = false;
            this.divWorkForm.Visible = true;
            this.InvNo.Value  = e.CommandArgument.ToString();
            var mgr = dsEntity.CreateDataManager();
            var AllowanceCancel = mgr.GetTable<InvoiceAllowanceCancellation>().Where(w => w.InvoiceAllowance.InvoiceItem.No == e.CommandArgument.ToString()).Select(s => s.AllowanceID).ToArray();
            var AllowanceDetail = mgr.GetTable<InvoiceAllowanceDetail>().Where(w => w.InvoiceAllowance.InvoiceItem.No == e.CommandArgument.ToString() && !AllowanceCancel.Contains(w.AllowanceID));
            var Allowance = from d in AllowanceDetail
                            select new
                            {
                                No = d.InvoiceAllowance.AllowanceNumber,
                                ID = d.InvoiceAllowance.AllowanceID
                            };
            var AllowanceItems = AllowanceDetail.Select(s => s.InvoiceAllowanceItem.ProductItemID).ToArray();
            
            var _ProductItems = from p in mgr.GetTable<InvoiceProductItem>().Where(w => w.InvoiceProduct.InvoiceDetails.Any(a => a.InvoiceItem.No == e.CommandArgument.ToString()) && !AllowanceItems.Contains(w.ItemID ) )
                                select new
                                {
                                    Brief=p.InvoiceProduct.Brief ,
                                    ItemID = p.ItemID.ToString () ,
                                    Item = "序號:" + p.ItemNo + " 品名:" + p.InvoiceProduct.Brief + " 數量:" + (p.Piece.HasValue ? p.Piece.Value.ToString() : "") + "單位:" + p.PieceUnit + "單價:" + (p.UnitCost.HasValue ? p.UnitCost.Value.ToString() : "") + "金額:" + (p.CostAmount.HasValue ? p.CostAmount.Value.ToString() : "") + "備註:" + p.Remark,
                                    p.No ,
                                    p.Piece ,
                                    p.PieceUnit ,
                                    p.UnitCost ,
                                    p.CostAmount ,
                                    p.Remark 
                                };
                
            if (this.GridView1 .AllowPaging)
            {
                GridView1.PageIndex = PagingControl.GetCurrentPageIndex(GridView1, 0);
                GridView1.DataSource = _ProductItems.Skip(GridView1.PageSize * GridView1.PageIndex).Take(GridView1.PageSize);
                GridView1.DataBind();
                

                GridView1.SetPageIndex("pagingList", _ProductItems.Count());
               
            }
            else
            {
                GridView1.DataSource = _ProductItems;
                GridView1.DataBind();
            }
           
            GridView2.DataSource = Allowance.ToList() ;
            GridView2.DataBind();
            if (_ProductItems.Count() == 0)
                this.btnAllowance.Enabled = false;
            else
                this.btnAllowance.Enabled = true;
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
             
             
        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)this.GridView1.HeaderRow.FindControl("chk");

            foreach (GridViewRow row in this.GridView1.Rows)
            {
                CheckBox chkbox2 = (CheckBox)row.FindControl("chk");
                chkbox2.Checked = chkbox.Checked;
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.StartsWith("S:"))
            {
                this.PNewInvalidInvoicePreview1.setDetail = eventArgument.Substring(2).Trim();
                this.PNewInvalidInvoicePreview1.Popup.Show();
            }
        }

    }

    public class InvoiceItems
    {
        public Organization Seller { get; set; }
        public InvoiceItem InvoiceItem { get; set; }
        public Organization Donatory { get; set; }
        public decimal TotalAmount { get; set; }
    }
   
}