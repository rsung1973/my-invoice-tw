using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;

using Utility;
using eIVOGo.Module.EIVO;
using eIVOGo.Module.UI;

namespace eIVOGo.Module.Inquiry
{
    public partial class InvoiceAndAllowanceReportList : System.Web.UI.UserControl
    {
        private InvoiceItemSellerGroupList itemList;

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (rbType.SelectedIndex)
            {
                case 0:
                    itemList = (InvoiceItemSellerGroupList)this.LoadControl("~/Module/EIVO/InvoiceItemSellerGroupList.ascx");
                    break;
                case 1:
                    itemList = (InvoiceItemSellerGroupList)this.LoadControl("~/Module/EIVO/InvoiceAllowanceSellerGroupList.ascx");
                    break;
                case 2:
                    itemList = (InvoiceCancellationSellerGroupList)this.LoadControl("~/Module/EIVO/InvoiceCancellationSellerGroupList.ascx");
                    break;
                case 3:
                    itemList = (InvoiceItemSellerGroupList)this.LoadControl("~/Module/EIVO/InvoiceAllowanceCancellationSellerGroupList.ascx");
                    break;

            }

            if (itemList != null)
            {
                border_gray.Controls.Add(itemList);
                itemList.InitializeAsUserControl(this.Page);
            }
           btnPrint.PrintControls.Add(border_gray);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            btnPrint.BeforeClick += new EventHandler(btnPrint_BeforeClick);
            this.PreRender += new EventHandler(InvoiceAndAllowanceReportList_PreRender);
        }

        void InvoiceAndAllowanceReportList_PreRender(object sender, EventArgs e)
        {
            if (btnQuery.CommandArgument == "Query")
            {
                itemList.BindData();
                //min-yu add 2011-05-26
                if (itemList.RecordCount > 0)
                {
                    this.itemList.Visible = true;
                    btnPrint.Visible = true;
                    NoData.Visible = false;
                }
                else
                {
                    this.itemList.Visible = false;
                    btnPrint.Visible = false;
                    NoData.Visible = true;
                }
            }
        }
        
        void btnPrint_BeforeClick(object sender, EventArgs e)
        {
            itemList.DataList.AllowPaging = false;
            itemList.BindData();            
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.border_gray.Visible = true;
            doQuery();                    
        }

        private void doQuery()
        {
            btnQuery.CommandArgument = "Query";
            border_gray.Visible = true;
            btnPrint.Visible = true;
            if (DateFrom.HasValue)
                itemList.DateFrom = DateFrom.DateTimeValue;
            if (DateTo.HasValue)
                itemList.DateTo = DateTo.DateTimeValue;
            if (!String.IsNullOrEmpty(SellerID.Selector.SelectedValue))
                itemList.SellerID = int.Parse(SellerID.Selector.SelectedValue);
            else //min-yu add 2011-05-26
                itemList.SellerID = null;
        }
       
    }
}