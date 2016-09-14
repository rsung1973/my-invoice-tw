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

namespace eIVOGo.template
{
    public partial class InquireEntityControl : InquireEntity
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
            Expression<Func<Organization, bool>> queryExpr = i => true;

            if (!String.IsNullOrEmpty(ReceiptNo.Text))
            {
                queryExpr = queryExpr.And(i => i.ReceiptNo == ReceiptNo.Text);
            }
            if (!String.IsNullOrEmpty(CompanyName.Text))
            {
                queryExpr = queryExpr.And(i => i.CompanyName == CompanyName.Text);
            }
            if (!String.IsNullOrEmpty(CompanyStatus.SelectedValue))
            {
                queryExpr = queryExpr.And(i => i.OrganizationStatus.CurrentLevel == int.Parse(CompanyStatus.SelectedValue));
            }

            itemList.BuildQuery = table =>
            {
                var org = table.Context.GetTable<Organization>();
                return table.Join(org.Where(queryExpr), b => b.CompanyID, o => o.CompanyID, (b, o) => b);
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