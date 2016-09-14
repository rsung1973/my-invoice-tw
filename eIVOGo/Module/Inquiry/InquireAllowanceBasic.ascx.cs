using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using eIVOGo.Module.Base;
using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.Inquiry
{
    public partial class InquireAllowanceBasic : CommonInquireEntity<CDS_Document>
    {
        protected Expression<Func<InvoiceAllowance, bool>> _queryExpr = f => true;

        public Expression<Func<InvoiceAllowance, bool>> QueryExpr
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

        public List<IInquireEntity<InvoiceAllowance>> InquiryItem
        { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (inquiryHolder != null && inquiryHolder.Controls.Count > 0)
            {
                InquiryItem = new List<IInquireEntity<InvoiceAllowance>>();
                foreach (var c in inquiryHolder.Controls)
                {
                    if (c is IInquireEntity<InvoiceAllowance>)
                    {
                        InquiryItem.Add((IInquireEntity<InvoiceAllowance>)c);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void buildQueryItem()
        {
            //一般使用者僅能查詢屬於自己卡號的發票資訊,系統管理者則可以查詢全部
            //if (_userProfile.CurrentUserRole.RoleID != ((int)Naming.RoleID.ROLE_SYS))
            //{
            //    if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_SELLER || _userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_GOOGLETW)
            //    {
            //        queryExpr = queryExpr.And(d => d.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID);
            //    }
            //    else
            //    {
            //        queryExpr = queryExpr.And(i => i.InvoiceByHousehold.InvoiceUserCarrier.UID == _userProfile.UID);
            //    }
            //}

            if (InquiryItem != null && InquiryItem.Count > 0)
            {
                foreach (var q in InquiryItem)
                {
                    _queryExpr = q.BuildQueryExpression(_queryExpr);
                    if (q.QueryRequired && !q.HasSet)
                    {
                        _queryExpr = f => false;
                        Page.AjaxAlert(q.AlertMessage);
                        return;
                    }
                }
            }

            itemList.BuildQuery = table =>
            {
                switch ((Naming.RoleID)_userProfile.CurrentUserRole.RoleID)
                {
                    case Naming.RoleID.ROLE_GUEST:
                    case Naming.RoleID.ROLE_BUYER:
                    case Naming.RoleID.ROLE_SYS:
                        return table.Join(table.Context.GetTable<InvoiceAllowance>()
                                .Where(_queryExpr),
                                d => d.DocID, a => a.AllowanceID, (d, a) => d)
                            .OrderByDescending(d => d.DocID);

                    default:
                        IQueryable<InvoiceAllowance> items = ((EIVOEntityDataContext)table.Context).GetAllowanceByAgent(_userProfile.CurrentUserRole.OrganizationCategory.CompanyID).Where(_queryExpr);
                        return table.Join(items,
                            d => d.DocID, a => a.AllowanceID, (d, a) => d)
                            .OrderByDescending(d => d.DocID);

                }
            };

            onDone(this, new EventArgs());
        }

        protected override UserControl _itemList
        {
            get { return itemList; }
        }

        //public EntityItemList<EIVOEntityDataContext, InvoiceAllowance> ItemList
        //{
        //    get
        //    {
        //        return itemList;
        //    }
        //    set
        //    {
        //        itemList = value;
        //    }
        //}

    }
}