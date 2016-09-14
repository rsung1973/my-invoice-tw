using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Helper;

using eIVOGo.Module.Base;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.Module.Common;

namespace eIVOGo.Module.SAM
{
    public partial class CompanyManger : InquireEntity
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
            Expression<Func<Organization, bool>> queryExpr = i => i.OrganizationStatus != null;

            if (!String.IsNullOrEmpty(ReceiptNo.Text))
            {
                queryExpr = queryExpr.And(i => i.ReceiptNo == ReceiptNo.Text);
            }
            if (!String.IsNullOrEmpty(CompanyName.Text))
            {
                queryExpr = queryExpr.And(i => i.CompanyName.Contains(CompanyName.Text));
            }
            if (!String.IsNullOrEmpty(CompanyStatus.SelectedValue))
            {
                queryExpr = queryExpr.And(i => i.OrganizationStatus.CurrentLevel == int.Parse(CompanyStatus.SelectedValue));
            }

            itemList.BuildQuery = table =>
            {
                var org = table.Context.GetTable<Organization>();
                return table.Join(org.Where(queryExpr), b => b.CompanyID, o => o.CompanyID, (b, o) => b)
                    .OrderBy(b => b.ReceiptNo);
            };

            base.buildQueryItem();
        }
    }
}