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

namespace eIVOGo.Module.SAM
{
    public partial class UserProfileEntityManager : InquireEntity
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
            Expression<Func<UserProfile, bool>> queryExpr = i => true;

            if (!String.IsNullOrEmpty(PID.Text))
            {
                queryExpr = queryExpr.And(i => i.PID == PID.Text);
            }
            if (!String.IsNullOrEmpty(UserName.Text))
            {
                queryExpr = queryExpr.And(i => i.UserName == UserName.Text);
            }
            if (!String.IsNullOrEmpty(UserStatus.SelectedValue))
            {
                queryExpr = queryExpr.And(i => i.UserProfileStatus.CurrentLevel == int.Parse(UserStatus.SelectedValue));
            }
            if (!String.IsNullOrEmpty(RoleID.SelectedValue))
            {
                int roleID = int.Parse(RoleID.SelectedValue);
                queryExpr = queryExpr.And(i => i.UserRole.Count(r => r.RoleID == roleID) > 0);
            }

            itemList.QueryExpr = queryExpr;

        }

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            modelItem.DataItem = null;
            Server.Transfer(ToEdit.TransferTo);
        }
    }
}