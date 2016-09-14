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
using Utility;
using System.Text;
using System.Net.Mail;
using eIVOGo.Properties;
using System.Net;
using Uxnet.Web.WebUI;
using eIVOGo.Module.Common;

namespace eIVOGo.Module.SAM
{
    public partial class MemberManager1 : System.Web.UI.UserControl, IPostBackEventHandler
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
            this.gvEntity.PreRender += new EventHandler(gvEntity_PreRender);
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

                if (RoleID.SelectedIndex > 0)
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

        private void getData()
        {
            try
            {
                using (UserProfileManager mgr = new UserProfileManager())
                {
                    IQueryable<UserProfile> items = mgr.EntityList.Where(u => u.UserProfileStatus != null);
                    
                    if (!String.IsNullOrEmpty(txtPID.Text))
                    {
                        items = items.Where(u => u.PID.StartsWith(txtPID.Text));
                    }

                    if (!String.IsNullOrEmpty(txtUserName.Text))
                    {
                        items = items.Where(u => u.UserName.StartsWith(txtUserName.Text));
                    }

                    if (RoleID.SelectedIndex > 0)
                    {
                        items = mgr.GetUserByUserRole(items, int.Parse(RoleID.SelectedValue));
                    }

                    if (this.ddlMemStatus.SelectedIndex > 0)
                    {
                        items = items.Where(u => u.UserProfileStatus.CurrentLevel == int.Parse(this.ddlMemStatus.SelectedValue));
                    }

                    if (items.Count() > 0)
                    {
                        int count = items.Count();
                        this.lblError.Visible = false;
                        this.gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(this.gvEntity, 0);
                        this.gvEntity.DataSource = items.ToList();
                        this.gvEntity.DataBind();

                        PagingControl paging = (PagingControl)this.gvEntity.BottomPagerRow.Cells[0].FindControl("pagingList");
                        paging.RecordCount = count;
                        paging.CurrentPageIndex = this.gvEntity.PageIndex;
                    }
                    else
                    {
                        this.lblError.Text = "查無資料!!";
                        this.lblError.Visible = true;
                        this.gvEntity.DataSource = null;
                        this.gvEntity.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        void MemberManager_PreRender(object sender, EventArgs e)
        {           
            //if (gvEntity.BottomPagerRow != null)
            //{
            //    this.gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(this.gvEntity, 0);
            //    gvEntity.DataBind();

            //    PagingControl paging = (PagingControl)gvEntity.BottomPagerRow.Cells[0].FindControl("pagingList");
            //    paging.RecordCount = dsUserProfile.CurrentView.LastSelectArguments.TotalRowCount;
            //    paging.CurrentPageIndex = gvEntity.PageIndex;
            //}
        }
        
        void gvEntity_PreRender(object sender, EventArgs e)
        {
            if (this.divResult.Visible == true)
            {
                getData();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateConsumerMember.aspx");
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.divResult.Visible = true;
            getData();
        }


        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.StartsWith("D:"))
            {
                doEnableOrDisable(int.Parse(eventArgument.Substring(2)));
            }
            else if (eventArgument.StartsWith("U:"))
            {
                doEdit(eventArgument.Substring(2));
            }
            else if (eventArgument.StartsWith("M:"))
            {
                if (SharedFunction.CreatePWDSendMail(int.Parse(eventArgument.Substring(2))))
                {
                    Uxnet.Web.WebUI.WebMessageBox.Alert(this.Page, "確認信已送出!!");
                }
                else
                {
                    Uxnet.Web.WebUI.WebMessageBox.Alert(this.Page, "確認信重送失敗!!");
                }
            }
        }

        private void doEdit(String uid)
        {
            Page.Items["uid"] = uid;
            Server.Transfer("EditMember.aspx");
        }

        private void doEnableOrDisable(int uid)
        {
            using (UserProfileManager mgr = new UserProfileManager())
            {
                int? clevel = mgr.GetTable<UserProfileStatus>().Where(u => u.UID == uid).FirstOrDefault().CurrentLevel;
                if (clevel == (int)Naming.MemberStatusDefinition.Checked)
                {
                    mgr.GetTable<UserProfileStatus>().Where(u => u.UID == uid).FirstOrDefault().CurrentLevel = (int)Naming.MemberStatusDefinition.Mark_To_Delete;
                }
                else if (clevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete)
                {
                    mgr.GetTable<UserProfileStatus>().Where(u => u.UID == uid).FirstOrDefault().CurrentLevel = (int)Naming.MemberStatusDefinition.Wait_For_Check;
                    if (SharedFunction.CreatePWDSendMail(uid))
                    {
                        Uxnet.Web.WebUI.WebMessageBox.Alert(this.Page, "確認信已送出!!");
                    }
                    else
                    {
                        Uxnet.Web.WebUI.WebMessageBox.Alert(this.Page, "確認信重送失敗!!");
                    }
                }
                mgr.SubmitChanges();
            }
            getData();
        }

        //private Boolean doSendMail(int uid)
        //{
        //    Boolean result = false;
        //    try
        //    {
        //        using (UserProfileManager mgr = new UserProfileManager())
        //        {
        //            IQueryable<UserProfile> items = mgr.EntityList.Where(u => u.UID == uid);
        //            string _tempPassword = Utility.ExtensionMethods.CreateRandomPassword(6);
        //            items.FirstOrDefault().Password2 = Utility.ValueValidity.MakePassword(_tempPassword);

        //            StringBuilder body = new StringBuilder();
        //            MailMessage message = new MailMessage();
        //            message.From = new MailAddress(Settings.Default.WebMaster);
        //            message.To.Add(items.FirstOrDefault().EMail);
        //            message.Subject = "網際優勢電子發票獨立第三方平台 會員啟用認證信";
        //            message.IsBodyHtml = true;

        //            body.Append("本信件由 網際優勢電子發票獨立第三方平台 寄出，為本站之會員啟用認證信。<br/><br/>");
        //            body.Append("(本信件為系統自動發出，請勿回覆本信件。)<br/>");
        //            body.Append("-------------------------------------------------<br/>");
        //            body.Append("會員帳號：").Append(items.FirstOrDefault().PID).Append("<br/>");
        //            body.Append("會員密碼：").Append(_tempPassword).Append("<br/>");
        //            body.Append("-------------------------------------------------<br/>");
        //            body.Append("請立即透過下方帳號啟用連結 登入網際優勢電子發票獨立第三方平台 變更密碼 。<br/><br/>");
        //            body.Append("帳號啟用連結： ");
        //            body.Append("<a href=").Append(Settings.Default.mailLinkAddress).Append(VirtualPathUtility.ToAbsolute("~/SAM/EditMyself.aspx")).Append("?active=aEfs45WE>會員帳號啟用</a>");
        //            body.Append("<br/><br/>電子發票獨立第三方平台 感謝您的加入");

        //            message.Body = body.ToString();

        //            SmtpClient smtpclient = new SmtpClient(Settings.Default.MailServer);
        //            smtpclient.Credentials = CredentialCache.DefaultNetworkCredentials;
        //            smtpclient.Send(message);

        //            mgr.SubmitChanges();
        //        }
        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //    return result;
        //}

        #endregion
    }
}