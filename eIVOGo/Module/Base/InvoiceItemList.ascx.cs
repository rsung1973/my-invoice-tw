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

namespace eIVOGo.Module.Base
{
    public partial class InvoiceItemList : EntityItemList<EIVOEntityDataContext,CDS_Document> , IPostBackEventHandler
    {

        protected int? _totalRecordCount;
        protected decimal? _subtotal;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<CDS_Document> e)
        {
            base.dsEntity_Select(sender, e);

            _totalRecordCount = Select().Count();
            _subtotal = Select().Sum(i => i.InvoiceItem.InvoiceAmountType.TotalAmount);
        }


        #region IPostBackEventHandler Members

        public virtual void RaisePostBackEvent(string eventArgument)
        {

        }

        #endregion
    }    
}