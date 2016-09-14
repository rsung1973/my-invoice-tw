using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Helper;

using eIVOGo.Module.Base;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SAM
{
    public partial class InvoiceBuyerManger : InquireEntity
    {
        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (_userProfile.CurrentUserRole.RoleID != ((int)Naming.RoleID.ROLE_SYS))
            {
                if (_userProfile.CurrentUserRole.OrganizationCategory.CategoryID == (int)Naming.B2CCategoryID.Google台灣)
                {
                    this.itemList.HeaderName = "GoogleID";
                }
                else if (_userProfile.CurrentUserRole.OrganizationCategory.CategoryID == (int)Naming.B2CCategoryID.店家發票自動配號)
                {
                    this.itemList.HeaderName = "客戶ID";
                }
                this.SellerID.SelectedValue = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID.ToString().Trim();
                this.SellerID.Selector.Enabled = false;
            }
            else
            {
                if (!String.IsNullOrEmpty(this.SellerID.SelectedValue))
                {
                    this.itemList.setSellID = int.Parse(this.SellerID.SelectedValue.Trim());
                }
            }

            //this.ScriptManager1.RegisterAsyncPostBackControl(this.itemList);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(InvoiceBuyerManger_Load);
            this.SaveAsExcelButton1.BeforeClick += new EventHandler(SaveAsExcelButton1_BeforeClick);
        }

        void SaveAsExcelButton1_BeforeClick(object sender, EventArgs e)
        {
            this.itemList.gv.AllowPaging = false;
            this.itemList.gv.Columns[0].Visible = false;
            this.itemList.gv.Columns[8].Visible = false;
            buildQueryItem();
        }

        void InvoiceBuyerManger_Load(object sender, EventArgs e)
        {
            this.SaveAsExcelButton1.OutputFileName = "InvoiceBuyerData.xls";
            this.SaveAsExcelButton1.DownloadControls.Add(this.itemList.gv);
        }

        protected override UserControl _itemList
        {
            get { return itemList; }
        }

        protected override void buildQueryItem()
        {
            Expression<Func<InvoiceBuyer, bool>> queryExpr = i => i.ReceiptNo != null;

            if (String.IsNullOrEmpty(this.SellerID.SelectedValue))
            {
                if (_userProfile.CurrentUserRole.RoleID != ((int)Naming.RoleID.ROLE_SYS))
                {
                    queryExpr = queryExpr.And(i => i.InvoiceItem.SellerID.Equals(_userProfile.CurrentUserRole.OrganizationCategory.CompanyID));
                }
                else
                {
                    this.AjaxAlert("請選擇發票開立人!!");
                    itemList.QueryExpr = a => false;
                    ResetQuery();
                    this.SaveAsExcelButton1.Visible = false;
                    return;                    
                }
            }
            else
            {
                queryExpr = queryExpr.And(i => i.InvoiceItem.SellerID.Equals(this.SellerID.SelectedValue.Trim()));
            }

            if (this.rbInvoiceType.SelectedIndex == 0)
            {
                queryExpr = queryExpr.And(i => i.InvoiceItem.InvoiceBuyer.ReceiptNo != "0000000000");
            }
            else
            {
                queryExpr = queryExpr.And(i => i.InvoiceItem.InvoiceBuyer.ReceiptNo == "0000000000");
            }

            if (!String.IsNullOrEmpty(ReceiptNo.Text))
            {
                queryExpr = queryExpr.And(i => i.ReceiptNo == ReceiptNo.Text);
            }

            if (!String.IsNullOrEmpty(CustomerName.Text))
            {
                queryExpr = queryExpr.And(i => i.CustomerName.Equals(CustomerName.Text.Trim()));
            }

            if (!String.IsNullOrEmpty(this.invoiceNo.Text))
            {
                if (invoiceNo.Text.Trim().Length == 10)
                {
                    String trackCode = invoiceNo.Text.Substring(0, 2);
                    String no = invoiceNo.Text.Substring(2);
                    queryExpr = queryExpr.And(i => i.InvoiceItem.No == no && i.InvoiceItem.TrackCode == trackCode);

                }
                else
                {
                    queryExpr = queryExpr.And(i => i.InvoiceItem.No == invoiceNo.Text.Trim());
                }
            }
            else
            {
                if (!this.DateFrom.HasValue || !this.DateTo.HasValue)
                {
                    this.AjaxAlert("請指定日期區間!!");
                    itemList.QueryExpr = a => false;
                    ResetQuery();
                    this.SaveAsExcelButton1.Visible = false;
                    return;
                }

                if (this.DateFrom.DateTimeValue.AddMonths(3) < this.DateTo.DateTimeValue)
                {
                    this.AjaxAlert("日期區間多為3個月!!");
                    itemList.QueryExpr = a => false;
                    ResetQuery();
                    this.SaveAsExcelButton1.Visible = false;
                    return;
                }
            }

            if (this.DateFrom.HasValue)
            {
                queryExpr = queryExpr.And(i => i.InvoiceItem.InvoiceDate >= DateFrom.DateTimeValue);
            }
            if (DateTo.HasValue)
            {
                queryExpr = queryExpr.And(i => i.InvoiceItem.InvoiceDate < DateTo.DateTimeValue.AddDays(1));
            }

            itemList.BuildQuery = table =>
            {
                return table.Where(queryExpr);
            };

            if (itemList.Select().Count() > 0)
            {
                OnDone(null);
                if (_userProfile.CurrentUserRole.RoleID == ((int)Naming.RoleID.ROLE_SYS)) this.SaveAsExcelButton1.Visible = true;
            }
            else
            {
                this.SaveAsExcelButton1.Visible = false;
            }
        }

        protected void btnClean_Click(object sender, EventArgs e)
        {
            this.DateFrom.Reset();
            this.DateTo.Reset();
            itemList.QueryExpr = a => false;
            ResetQuery();
        }

        protected void rbInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemList.QueryExpr = a => false;
            ResetQuery();
            this.SaveAsExcelButton1.Visible = false;
        }
    }
}