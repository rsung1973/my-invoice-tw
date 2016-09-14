using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using eIVOGo.Module.Base;
using Model.DataEntity;

namespace eIVOGo.Module.Inquiry.InquiryItem
{
    public abstract partial class InquireInvoice : UserControl, IInquireEntity<InvoiceItem>
    {
        public abstract Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr);

        [Bindable(true)]
        public bool QueryRequired
        { get; set; }

        public bool HasSet
        { get; set; }

        [Bindable(true)]
        public String AlertMessage
        { get; set; }

    }

    public abstract partial class InquireAllowance : UserControl, IInquireEntity<InvoiceAllowance>
    {
        public abstract Expression<Func<InvoiceAllowance, bool>> BuildQueryExpression(Expression<Func<InvoiceAllowance, bool>> queryExpr);

        [Bindable(true)]
        public bool QueryRequired
        { get; set; }

        public bool HasSet
        { get; set; }

        [Bindable(true)]
        public String AlertMessage
        { get; set; }
    }

}