using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Security.MembershipManagement;
using Model.DataEntity;
using Model.Locale;
using Business.Helper;
using Uxnet.Web.Module.Common;

namespace eIVOGo.Module.SAM
{
    public partial class SocialWelfareAgenciesManger : System.Web.UI.UserControl,IPostBackEventHandler
    {
        public class dataType
        {
            public int OrgID;
            public string OrgCode;
            public string OrgName;
            public string OrgAddr;
            public string OrgPhone;
            public int? OrgCurrentLevel;
            public string OrgStatus;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            this.gvEntity.PreRender += new EventHandler(this.gvEntity_PreRender);
        }

        private void getData()
        {
            using (UserManager um = new UserManager())
            {
                var data = from oc in um.GetTable<OrganizationCategory>()
                           where (oc.CategoryID == (int)Naming.CategoryID.COMP_WELFARE && oc.Organization.OrganizationStatus != null)
                           select new dataType
                           {
                               OrgID = oc.Organization.CompanyID,
                               OrgCode = oc.Organization.ReceiptNo,
                               OrgName = oc.Organization.CompanyName,
                               OrgAddr = oc.Organization.Addr,
                               OrgPhone = oc.Organization.Phone,
                               OrgCurrentLevel=oc.Organization.OrganizationStatus.CurrentLevel,
                               OrgStatus=oc.Organization.OrganizationStatus.LevelExpression.Description
                           };

                if (data.Count() > 0)
                {
                    int count = data.Count();
                    this.lblError.Visible = false;
                    this.gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(this.gvEntity, 0);
                    this.gvEntity.DataSource = data.ToList();
                    this.gvEntity.DataBind();
                    
                    PagingControl paging = (PagingControl)this.gvEntity.BottomPagerRow.Cells[0].FindControl("pagingIndex");
                    paging.RecordCount = count;
                    paging.CurrentPageIndex = this.gvEntity.PageIndex;
                }
                else
                {
                    this.lblError.Text = "尚未建立社福機構資料!!";
                    this.lblError.Visible = true;
                    this.gvEntity.DataSource = null;
                    this.gvEntity.DataBind();
                }
            }
        }

        #region "Gridview Event"

        void gvEntity_PreRender(object sender, EventArgs e)
        {
            getData();
        }

        #endregion        

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("modifySocialWelfareAgencies.aspx");
        }

        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            string OrgID = "";
            if (eventArgument.StartsWith("U:"))
            {
                OrgID = eventArgument.Substring(2).Trim();
                Response.Redirect("modifySocialWelfareAgencies.aspx?PID=" + OrgID);
            }
            else if (eventArgument.StartsWith("D:"))
            {
                OrgID = eventArgument.Substring(2).Trim();
                doEnableOrDisable(int.Parse(OrgID));
            }            
        }

        private void doEnableOrDisable(int id)
        {
            using (UserManager mgr = new UserManager())
            {
                int? clevel = mgr.GetTable<OrganizationStatus>().Where(os => os.CompanyID == id).FirstOrDefault().CurrentLevel;
                if (clevel == (int)Naming.MemberStatusDefinition.Checked)
                {
                    mgr.GetTable<OrganizationStatus>().Where(os => os.CompanyID == id).FirstOrDefault().CurrentLevel = (int)Naming.MemberStatusDefinition.Mark_To_Delete;
                }
                else if (clevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete)
                {
                    mgr.GetTable<OrganizationStatus>().Where(os => os.CompanyID == id).FirstOrDefault().CurrentLevel = (int)Naming.MemberStatusDefinition.Checked;
                }            
                mgr.SubmitChanges();
                getData();
            }
        }

        #endregion       
    }
}