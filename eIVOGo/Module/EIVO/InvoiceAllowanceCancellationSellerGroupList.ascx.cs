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

namespace eIVOGo.Module.EIVO
{
    public partial class InvoiceAllowanceCancellationSellerGroupList : InvoiceItemSellerGroupList
    {

        protected override void buildQueryItem()
        {
            Expression<Func<InvoiceAllowance, bool>> queryExpr = i => i.InvoiceAllowanceCancellation != null;

            if (SellerID.HasValue)
            {
                queryExpr = queryExpr.And(o => o.InvoiceAllowanceSeller.SellerID == SellerID);
            }
            if (DateFrom.HasValue)
                queryExpr = queryExpr.And(i => i.InvoiceAllowanceCancellation.CancelDate >= DateFrom);
            if (DateTo.HasValue)
                queryExpr = queryExpr.And(i => i.InvoiceAllowanceCancellation.CancelDate < DateTo.Value.AddDays(1));

            var mgr = dsOrg.CreateDataManager();
            _queryItems = mgr.GetTable<InvoiceAllowance>().Where(queryExpr).GroupBy(i => i.InvoiceAllowanceSeller.SellerID).Select(g =>
                 new SellerGroupList
                 {
                     Seller = mgr.EntityList.Where(o => o.CompanyID == g.Key).First(),
                     TotalCount = g.Count(),
                     Summary = g.Sum(i => i.TotalAmount)
                 });

        }
            

    }

}