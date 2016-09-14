using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Model.DataEntity;
using Model.Locale;
using Utility;
using eIVOGo.Module.UI;
using eIVOGo.Module.Base;

namespace eIVOGo.Module.Inquiry
{
    public partial class QueryCALogList : InquireEntity
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override UserControl _itemList
        {
            get { return itemList; }
        }

        protected override void buildQueryItem()
        {
            Expression<Func<CALog, bool>> queryExpr = c => true;

            if (!String.IsNullOrEmpty(SellerID.SelectedValue))
            {
                int sellerID = int.Parse(SellerID.SelectedValue);
                queryExpr = queryExpr.And(w => (w.CDS_Document.InvoiceItem.SellerID == sellerID)
                    || (w.CDS_Document.DocumentOwner.OwnerID == sellerID)
                    || (w.CDS_Document.InvoiceAllowance.InvoiceAllowanceSeller.SellerID == sellerID));
            }

            if (DateFrom.HasValue)
                queryExpr = queryExpr.And(w => w.LogDate.HasValue && w.LogDate.Value >= DateFrom.DateTimeValue);

            if (DateTo.HasValue)
                queryExpr = queryExpr.And(w => w.LogDate.HasValue && w.LogDate.Value < DateTo.DateTimeValue.AddDays(1));
            if (!String.IsNullOrEmpty(CACatalog.SelectedValue))
            {
                queryExpr = queryExpr.And(w => w.Catalog == int.Parse(CACatalog.SelectedValue));
            }

            itemList.BuildQuery = table =>
            {
                return table.Where(queryExpr).OrderByDescending(c => c.LogID);
            };

        }
    }
}