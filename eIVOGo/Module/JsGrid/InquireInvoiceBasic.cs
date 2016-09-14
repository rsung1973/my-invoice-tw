using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Helper;
using eIVOGo.Helper;
using eIVOGo.Module.Base;
using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.JsGrid
{
    public abstract partial class InquireInvoiceBasic : InquireEntity
    {
        protected global::System.Web.UI.WebControls.PlaceHolder inquiryHolder;
        protected Expression<Func<InvoiceItem, bool>> _queryExpr = i => i.InvoiceCancellation == null;

        protected UserProfileMember _userProfile;

        public Expression<Func<InvoiceItem, bool>> QueryExpr
        {
            get
            {
                return _queryExpr;
            }
            set
            {
                _queryExpr = value;
            }
        }

        public List<IInquireEntity<InvoiceItem>> InquiryItem
        { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _userProfile = WebPageUtility.UserProfile;

            if (inquiryHolder != null && inquiryHolder.Controls.Count > 0)
            {
                InquiryItem = new List<IInquireEntity<InvoiceItem>>();
                foreach (var c in inquiryHolder.Controls)
                {
                    if (c is IInquireEntity<InvoiceItem>)
                    {
                        InquiryItem.Add((IInquireEntity<InvoiceItem>)c);
                    }
                }
            }
        }

        protected IQueryable<InvoiceItem> buildDefaultQuery(EIVOEntityDataContext context, IQueryable<InvoiceItem> items)
        {

            if (InquiryItem != null && InquiryItem.Count > 0)
            {
                foreach (var q in InquiryItem)
                {
                    _queryExpr = q.BuildQueryExpression(_queryExpr);
                    if (q.QueryRequired && !q.HasSet)
                    {
                        _queryExpr = f => false;
                        Page.AjaxAlert(q.AlertMessage);
                        return items.Where(_queryExpr);
                    }
                }
            }

            switch ((Naming.RoleID)_userProfile.CurrentUserRole.RoleID)
            {
                case Naming.RoleID.ROLE_GUEST:
                case Naming.RoleID.ROLE_BUYER:
                case Naming.RoleID.ROLE_SYS:
                    return items.Where(_queryExpr);

                default:
                    return context.GetInvoiceByAgent(items, _userProfile.CurrentUserRole.OrganizationCategory.CompanyID).Where(_queryExpr);
            }
        }

    }
}