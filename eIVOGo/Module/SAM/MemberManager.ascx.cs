using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;
using Model.Security.MembershipManagement;
using Model.Locale;
using Business.Helper;
using Uxnet.Web.Module.Common;

namespace eIVOGo.Module.SAM
{
    public partial class MemberManager : System.Web.UI.UserControl,IPostBackEventHandler
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                initializeData();
            }
        }

        private void initializeData()
        {
            var mgr = dsUserProfile.CreateDataManager();
            RoleID.Items.AddRange(mgr.GetTable<UserRoleDefinition>().Select(r => new ListItem(r.Role, r.RoleID.ToString())).ToArray());

            this.ddlMemStatus.Items.AddRange(mgr.GetTable<LevelExpression>().Where(le => le.LevelID > 1100 & le.LevelID < 1104).Select(le => new ListItem(le.Description, le.LevelID.ToString())).ToArray());
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(MemberManager_PreRender);
            dsUserProfile.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserProfile>>(dsUserProfile_Select);
        }

        void dsUserProfile_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserProfile> e)
        {
            if (!String.IsNullOrEmpty(btnQuery.CommandArgument))
            {
                UserProfileManager mgr = new UserProfileManager(dsUserProfile.CreateDataManager());

                IQueryable<UserProfile> items = mgr.EntityList.Where(u => u.UserProfileStatus != null);
                //IQueryable<UserProfile> items = mgr.EntityList.Where(u => u.UserProfileStatus.CurrentLevel != (int)Naming.MemberStatusDefinition.Mark_To_Delete);
                if (!String.IsNullOrEmpty(txtPID.Text))
                {
                    items = items.Where(u => u.PID.StartsWith(txtPID.Text));
                }

                if (!String.IsNullOrEmpty(txtUserName.Text))
                {
                    items = items.Where(u => u.UserName.StartsWith(txtUserName.Text));
                }

                if (RoleID.SelectedIndex>0)
                {
                    items = mgr.GetUserByUserRole(items, int.Parse(RoleID.SelectedValue));
                }

                if (this.ddlMemStatus.SelectedIndex > 0)
                {
                    items = items.Where(u => u.UserProfileStatus.CurrentLevel == int.Parse(this.ddlMemStatus.SelectedValue));
                }

                e.Query = items;
            }
            else
            {
                e.QueryExpr = u => false;
            }
        }

        protected void PageIndexChanged(object source, PageChangedEventArgs e)
        {
            gvEntity.PageIndex = e.NewPageIndex;
            gvEntity.DataBind();
        }

        void MemberManager_PreRender(object sender, EventArgs e)
        {
            if (gvEntity.BottomPagerRow != null)
            {
                PagingControl paging = (PagingControl)gvEntity.BottomPagerRow.Cells[0].FindControl("pagingList");
                paging.RecordCount = dsUserProfile.CurrentView.LastSelectArguments.TotalRowCount;
                paging.CurrentPageIndex = gvEntity.PageIndex;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateConsumerMember.aspx");
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            btnQuery.CommandArgument = "Query";
            gvEntity.DataBind();
        }


        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.StartsWith("D:"))
            {
                doDelete(int.Parse(eventArgument.Substring(2)));
            }
            else if (eventArgument.StartsWith("U:"))
            {
                doEdit(eventArgument.Substring(2));
            }
        }

        private void doEdit(String uid)
        {
            Page.Items["uid"] = uid;
            Server.Transfer("EditMember.aspx");
        }

        private void doDelete(int uid)
        {
            var mgr = dsUserProfile.CreateDataManager();
            mgr.GetTable<UserProfileStatus>().InsertOnSubmit(new UserProfileStatus
            {
                UID = uid,
                CurrentLevel = (int)Naming.MemberStatusDefinition.Mark_To_Delete
            });
            mgr.SubmitChanges();
            gvEntity.DataBind();
        }

        #endregion
    }
}