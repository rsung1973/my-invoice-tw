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


namespace eIVOGo.Module.Inquiry
{
    public partial class PrintCInvoice : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            btnSearch.Load += new EventHandler(btnSearch_Load);
        }

        void btnSearch_Load(object sender, EventArgs e)
        {
            if (Request.Form[btnSearch.UniqueID] == null)
            {
                initializeData();
            }
        }

        protected virtual void initializeData()
        {
            if (btnSearch.CommandArgument == "Query")
            {
                PrintCInvoiceItemList listView;
                listView = (PrintCInvoiceItemList)this.LoadControl("PrintCInvoiceItemList.ascx");
                listView.InitializeAsUserControl(this.Page);
                listView.BuildQuery = table =>
                {
                    Expression<Func<InvoiceItem, bool>> queryExpr = buildInvoiceItemQuery(i => i.InvoiceCancellation == null);
                    return table.Join(table.Context.GetTable<InvoiceItem>().Where(queryExpr),
                        d => d.DocID, i => i.InvoiceID, (d, i) => d);
                };
                plResult.Controls.Add(listView);                
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.divResult.Visible = true;
            btnSearch.CommandArgument = "Query";
            initializeData();
        }


        #region "Search Data"

        protected virtual Expression<Func<InvoiceItem, bool>> buildInvoiceItemQuery(Expression<Func<InvoiceItem, bool>> queryExpr)
        {
            if (this.DateFrom.HasValue)
            {
                queryExpr = queryExpr.And(i => i.InvoiceDate >= DateFrom.DateTimeValue);
            }

            if (DateTo.HasValue)
            {
                queryExpr = queryExpr.And(i => i.InvoiceDate < DateTo.DateTimeValue.AddDays(1));
            }

            if (!String.IsNullOrEmpty(this.ReceiptNo.Text.Trim()))
            {
                queryExpr = queryExpr.And(i => i.InvoiceBuyer.ReceiptNo.Equals(this.ReceiptNo.Text.Trim()));
            }

            if (!String.IsNullOrEmpty(invoiceNo.Text))
            {
                if (invoiceNo.Text.Trim().Length == 10)
                {
                    String trackCode = invoiceNo.Text.Substring(0, 2);
                    String no = invoiceNo.Text.Substring(2);
                    queryExpr = queryExpr.And(i => i.No == no && i.TrackCode == trackCode);
                }
                else
                {
                    queryExpr = queryExpr.And(i => i.No == invoiceNo.Text.Trim());
                }
            }
            return queryExpr;

        }
        #endregion

        protected void resetContent()
        {
            btnSearch.CommandArgument = "";
            plResult.Controls.Clear();
            divResult.Visible = false;
        }
    }    
}