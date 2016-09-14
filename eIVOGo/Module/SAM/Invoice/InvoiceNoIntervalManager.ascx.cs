using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using eIVOGo.Module.Base;
using Model.DataEntity;
using Utility;
using Model.Security.MembershipManagement;
using Business.Helper;

namespace eIVOGo.Module.SAM.Invoice
{
    public partial class InvoiceNoIntervalManager : InquireEntity
    {
        private UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override UserControl _itemList
        {
            get { return itemList; }
        }

        protected override void buildQueryItem()
        {
            Expression<Func<InvoiceTrackCodeAssignment, bool>> queryExpr = i => i.SellerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID;

            if (!String.IsNullOrEmpty(year.SelectedValue))
            {
                queryExpr = queryExpr.And(i => i.InvoiceTrackCode.Year == short.Parse(year.SelectedValue));
            }
            else
            {
                queryExpr = queryExpr.And(i => i.InvoiceTrackCode.Year >= TrackCodeYearSelector.MinValue && i.InvoiceTrackCode.Year <= TrackCodeYearSelector.MaxValue);
            }

            if (!String.IsNullOrEmpty(periodNo.SelectedValue))
            {
                queryExpr = queryExpr.And(i => i.InvoiceTrackCode.PeriodNo == short.Parse(periodNo.SelectedValue));
            }

            itemList.BuildQuery = table =>
            {
                var assignment = table.Context.GetTable<InvoiceTrackCodeAssignment>();
                return table.Join(assignment.Where(queryExpr), b => b.InvoiceTrackCodeAssignment, o => o, (b, o) => b);
            };

            base.buildQueryItem();
        }

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            modelItem.DataItem = null;
            Server.Transfer(ToEdit.TransferTo);
        }
    }
}