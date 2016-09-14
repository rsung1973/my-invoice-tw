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
    public partial class InvoiceTrackCodeList : EntityItemList<EIVOEntityDataContext,InvoiceTrackCode>, IPostBackEventHandler
    {

        protected int? _totalRecordCount;

        protected override void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<InvoiceTrackCode> e)
        {
            base.dsEntity_Select(sender, e);
            _totalRecordCount = e.Query.Count();
        }

        #region IPostBackEventHandler Members

        public virtual void RaisePostBackEvent(string eventArgument)
        {

        }

        #endregion

    }    
}