using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Helper;
using eIVOGo.Module.Base;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.JsGrid
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    public partial class InquireInvoiceTemplate : InquireEntity
    {
        protected ITemplate _queryTemplate;
        protected Control _queryContainer;
        protected Expression<Func<InvoiceItem, bool>> _queryExpr = i => i.InvoiceCancellation == null;
        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Action DoQuery
        { get; set; }

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

        protected override UserControl _itemList
        {
            get { return ItemList; }
        }

        [Bindable(true)]
        public UserControl ItemList
        { get; set; }

        [Browsable(false)]
        [TemplateInstance(TemplateInstance.Single)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate QueryTemplate
        {
            get
            {
                return this._queryTemplate;
            }
            set
            {
                this._queryTemplate = value;
                if (this._queryTemplate == null)
                    return;
                this.createContents();
            }
        }

        private void createContents()
        {
            if (_queryContainer == null)
            {
                _queryContainer = new Control();
                if (this._queryTemplate != null)
                    this._queryTemplate.InstantiateIn(_queryContainer);
            }
            else
            {
                if (this._queryTemplate != null)
                    this._queryTemplate.InstantiateIn(_queryContainer);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _userProfile = WebPageUtility.UserProfile;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            if (_queryContainer != null)
            {
                InquiryItem = new List<IInquireEntity<InvoiceItem>>();

                foreach (Control c in _queryContainer.Controls.Cast<Control>().ToArray())
                {
                    this.inquiryHolder.Controls.Add(c);
                    if (c is IInquireEntity<InvoiceItem>)
                    {
                        InquiryItem.Add((IInquireEntity<InvoiceItem>)c);
                    }
                }
            }
        }

        protected override void buildQueryItem()
        {
            if(DoQuery!=null)
            {
                DoQuery();
            }
            base.buildQueryItem();
        }

        public IQueryable<InvoiceItem> BuildDefaultQuery(EIVOEntityDataContext context, IQueryable<InvoiceItem> items)
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

        public bool HasQuery
        {
            get
            {
                return btnQuery.CommandArgument == "Query";
            }
        }

    }
}