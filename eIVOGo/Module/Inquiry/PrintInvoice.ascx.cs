using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Locale;
using Model.Security.MembershipManagement;
using Model.InvoiceManagement;
using Business.Helper;
using Model.DataEntity;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Utility;
using System.Collections;
using Uxnet.Web.WebUI;
using eIVOGo.Module.EIVO;

namespace eIVOGo.Module.Inquiry
{
    public partial class PrintInvoice : System.Web.UI.UserControl
    {
        UserProfileMember _userProfile;

        public class dataType
        {
            public int InvoiceID;
            public DateTime? InvoiceDate;
            public string ChineseInvoiceDate;
            public string CompanyName;
            public string ReceiptNo;
            public string TrackCode;
            public string No;
            public Decimal? TotalAmount;
            public string check;
            public string Donation;
            public string DonationName;
            public string DonateMark;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (!Page.IsPostBack)
            {
                SearchItem();
                LoadCompanyReceiptNo();
            }
        }

        
        #region "Load Page Control Data"
        protected void SearchItem()
        {
            this.rdbInvoiceType.Items.Clear();
            this.rdbInvoiceType.Items.Add(new ListItem("電子發票", "1"));
            this.rdbInvoiceType.Items.Add(new ListItem("電子折讓單", "2"));
            this.rdbInvoiceType.Items.Add(new ListItem("作廢電子發票", "3"));
            this.rdbInvoiceType.Items.Add(new ListItem("作廢電子折讓單", "4"));
            this.rdbInvoiceType.SelectedIndex = 0;
            this.Item.Visible = true;

            this.rdbSearchItem.Items.Clear();
            this.rdbSearchItem.Items.Add(new ListItem("B2B", "1"));
            this.rdbSearchItem.Items.Add(new ListItem("B2C", "2"));
            this.rdbSearchItem.SelectedIndex = 0;

            this.rdbPriceType.Items.Clear();
            this.rdbPriceType.Items.Add(new ListItem("全部", "1"));
            this.rdbPriceType.Items.Add(new ListItem("中獎", "2"));
            this.rdbPriceType.SelectedIndex = 0;
        }

        protected void LoadCompanyReceiptNo()
        {
            using (InvoiceManager im = new InvoiceManager())
            {
                this.ddlOrganization.Items.AddRange(im.GetTable<Organization>().Where(o => o.OrganizationCategory.Any(c => c.CategoryID == (int)Naming.CategoryID.COMP_E_INVOICE_B2C_SELLER)).Select(o => new ListItem(o.ReceiptNo.Trim() + " " + o.CompanyName.Trim(), o.ReceiptNo.Trim())).ToArray());
            }
        }
        #endregion

        #region "Page Control Event"
        protected void rbdInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.rdbInvoiceType.SelectedValue.Equals("1"))
            {
                this.Item.Visible = true;
            }
            else
            {
                this.Item.Visible = false;
                this.Type.Visible = false;
                this.rdbSearchItem.SelectedIndex = 0;
            }
            this.divResult.Visible = false;
        }

        protected void rdbSearchItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.rdbSearchItem.SelectedValue.Equals("1"))
            {
                this.Type.Visible = false;
            }
            else
            {
                this.Type.Visible = true;
            }
            this.divResult.Visible = false;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.divResult.Visible = true;
            SearchData();
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            List<String> ar = buildItemSelection();
            //if (ar.Count > 0)
            //{
                
            //    this.AjaxAlert(string.Join(",", ar.ToArray()));

            //}
            //else
            //{
            //    this.AjaxAlert("請選擇列印資料!!");
            //}

            if (ar.Count>0)
            {
                Session["PrintDoc"] = ar;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "open",
                    String.Format("window.open('{0}','prnWin','toolbar=no,location=no,status=no,menubar=no,scrollbars=auto,resizable=yes,alwaysRaised,dependent,titlebar=no,width=64,height=48');", VirtualPathUtility.ToAbsolute("~/SAM/PrintInvoicePage.aspx"))
                    , true);
            }
            else
            {
                this.AjaxAlert("請選擇列印資料!!");
            }
        }

        private List<String> buildItemSelection()
        {
            List<String> ar = new List<string>();
            foreach (GridViewRow row in gvEntity.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("chkItem");
                Label lb = (Label)row.FindControl("lblID");
                if (cb != null && cb.Checked)
                {
                    ar.Add(lb.Text);
                }
            }
            return ar;
        }
        #endregion

        #region "Search Data"
        private void SearchData()
        {
            using (InvoiceManager im = new InvoiceManager())
            {                
                try
                {
                    IQueryable<dataType> data = null;
                    IQueryable<InvoiceItem> dataSrc = im.EntityList;
                    IQueryable<InvoiceAllowance> allowance = im.GetTable<InvoiceAllowance>();

                    if (this.rdbInvoiceType.SelectedValue.Equals("1"))
                    {
                        data = inqueryInvoiceItem(im, dataSrc, "1");
                    }
                    else if (this.rdbInvoiceType.SelectedValue.Equals("2"))
                    {
                        data = inqueryAllowanceInvoiceItem(im, allowance, "2");
                    }
                    else if (this.rdbInvoiceType.SelectedValue.Equals("3"))
                    {
                        data = inqueryInvoiceItem(im, dataSrc, "3");
                    }
                    else if (this.rdbInvoiceType.SelectedValue.Equals("4"))
                    {
                        data = inqueryAllowanceInvoiceItem(im, allowance, "4");
                    }
                    
                    if (!string.IsNullOrEmpty(this.DateFrom.TextBox.Text) & !string.IsNullOrEmpty(this.DateTo.TextBox.Text))
                    {
                        if (DateTime.Parse(this.DateFrom.TextBox.Text) > DateTime.Parse(this.DateTo.TextBox.Text))
                        {
                            DateTime tempDate;
                            tempDate = this.DateTo.DateTimeValue;
                            this.DateTo.DateTimeValue = this.DateFrom.DateTimeValue;
                            this.DateFrom.DateTimeValue = tempDate;
                        }
                        data = data.Where(d => d.InvoiceDate.Value.Date >= this.DateFrom.DateTimeValue.Date && d.InvoiceDate.Value.Date <= this.DateTo.DateTimeValue.Date);
                    }

                    if (data.Count() > 0)
                    {
                        int count = data.Count();
                        decimal total = data.Sum(d => d.TotalAmount) ?? 0;
                        this.lblError.Visible = false;
                        this.ResultTitle.Visible = true;
                        this.btnShow.Visible = true;
                        this.lblTotalSum.Text = total.ToString("0,0.00");
                        this.lblRowCount.Text = count.ToString();
                        this.gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(this.gvEntity, 0);
                        this.gvEntity.DataSource = data.OrderByDescending(d => d.InvoiceDate).ToList();
                        this.gvEntity.DataBind();

                        PagingControl paging = (PagingControl)this.gvEntity.BottomPagerRow.Cells[0].FindControl("pagingIndex");
                        paging.RecordCount = count;
                        paging.CurrentPageIndex = this.gvEntity.PageIndex;
                    }
                    else
                    {
                        this.lblError.Text = "查無資料!!";
                        this.lblError.Visible = true;
                        this.ResultTitle.Visible = false;
                        this.btnShow.Visible = false; 
                        this.lblTotalSum.Text = "0";
                        this.lblRowCount.Text = "0";
                        this.gvEntity.DataSource = null;
                        this.gvEntity.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    this.lblError.Text = "系統錯誤:" + ex.Message;
                }
            }
        }
        #endregion

        #region "Query Different Type Data"
        private IQueryable<dataType> inqueryInvoiceItem(InvoiceManager im, IQueryable<InvoiceItem> dataSrc, string type)
        {
            if (type.Equals("1"))
            {
                dataSrc = dataSrc.Where(i => i.InvoiceCancellation == null);
                if (this.rdbSearchItem.SelectedValue.Equals("1"))
                {
                    dataSrc = dataSrc.Where(i => i.InvoiceBuyer.ReceiptNo != null && i.InvoiceBuyer.ReceiptNo != "");
                }
                else if (this.rdbSearchItem.SelectedValue.Equals("2"))
                {
                    if (this.rdbPriceType.SelectedValue.Equals("1"))
                    {
                        dataSrc = dataSrc.Where(i => i.InvoiceBuyer.ReceiptNo == null || i.InvoiceBuyer.ReceiptNo == "");
                    }
                    else if (this.rdbPriceType.SelectedValue.Equals("2"))
                    {
                        dataSrc = dataSrc.Where(w => w.InvoiceWinningNumber != null);
                    }
                }
            }
            else if (type.Equals("3"))
                dataSrc = dataSrc.Where(i => i.InvoiceCancellation != null);
                

            return from a in dataSrc
                   select new dataType
                   {
                       InvoiceID = a.InvoiceID,
                       InvoiceDate = a.InvoiceDate,
                       ChineseInvoiceDate = Utility.ValueValidity.ConvertChineseDateString(a.InvoiceDate),
                       CompanyName = a.Organization.CompanyName,
                       ReceiptNo = a.Organization.ReceiptNo,
                       TrackCode = a.TrackCode,
                       No = a.No,
                       TotalAmount = a.InvoiceAmountType.TotalAmount ?? 0,
                       check = im.GetTable<InvoiceWinningNumber>().Where(i => i.InvoiceID == a.InvoiceID).FirstOrDefault().UniformInvoiceWinningNumber.PrizeType
                   };
        }

        private IQueryable<dataType> inqueryAllowanceInvoiceItem(InvoiceManager im, IQueryable<InvoiceAllowance> dataSrc, string type)
        {
            if (type.Equals("2"))
                dataSrc = dataSrc.Where(i => i.InvoiceAllowanceCancellation == null);
            else if (type.Equals("4"))
                dataSrc = dataSrc.Where(i => i.InvoiceAllowanceCancellation != null);

            return from a in dataSrc
                   select new dataType
                   {
                       InvoiceID = a.AllowanceID,
                       InvoiceDate = a.AllowanceDate,
                       ChineseInvoiceDate = Utility.ValueValidity.ConvertChineseDateString(a.AllowanceDate),
                       CompanyName = a.InvoiceAllowanceSeller.Organization.CompanyName,
                       ReceiptNo = a.InvoiceAllowanceSeller.Organization.ReceiptNo,
                       TrackCode = "",
                       No = a.AllowanceNumber,
                       TotalAmount = a.TotalAmount ?? 0,
                       check = "N/A",
                   };
        }
        #endregion
    }    
}