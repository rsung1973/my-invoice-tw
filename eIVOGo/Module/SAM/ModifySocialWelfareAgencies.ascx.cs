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
using Utility;

namespace eIVOGo.Module.SAM
{
    public partial class ModifySocialWelfareAgencies : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["PID"] != null)
                {
                    this.lblTitle1.Text = "修改資料";
                    this.lblTitle2.Text = "修改資料";
                    int pid = int.Parse(Request["PID"].ToString().Trim());
                    getData(pid);
                }
                else
                {
                    this.lblTitle1.Text = "新增資料";
                    this.lblTitle2.Text = "新增資料";
                }
            }
        }

        #region "Button Event"

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string result = "";
            result = ColumnCheck();
            if (result.Equals("ok"))
            {
                if (Request["PID"] != null)
                {
                    int pid = int.Parse(Request["PID"].ToString().Trim());
                    result = InsertOrUpdate(pid);
                }
                else
                {
                    result = InsertOrUpdate(-1);
                }
            }

            if (result.Equals("ok"))
            {
                Response.Redirect("SocialWelfareAgenciesManger.aspx");
            }
            else
            {
                this.lblError.Text = result;
                this.lblError.Visible = true;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            foreach (Control tb in this.Controls)
            {
                if (tb is TextBox)
                {
                    ((TextBox)tb).Text = "";
                }
            }
        }

        #endregion

        #region "Select, Insert or Update"

        private void getData(int pid)
        {
            using (UserManager um = new UserManager())
            {
                var data = um.GetTable<Organization>().Where(o => o.CompanyID == pid).FirstOrDefault();
                this.txtOrgCode.Text = data.ReceiptNo;
                this.txtOrgName.Text = data.CompanyName;
                this.txtOrgAddr.Text = data.Addr;
                this.txtOrgPhone.Text = data.Phone;
                this.txtOrgEMail.Text = data.ContactEmail;
            }
        }

        private string InsertOrUpdate(int pid)
        {
            string result = "";
            try
            {
                using (UserManager um = new UserManager())
                {
                    if (pid != -1)
                    {
                        var data = um.GetTable<Organization>().Where(o => o.CompanyID == pid).FirstOrDefault();
                        data.ReceiptNo = this.txtOrgCode.Text;
                        data.CompanyName = this.txtOrgName.Text;
                        data.Addr = this.txtOrgAddr.Text;
                        data.Phone = this.txtOrgPhone.Text;
                        data.ContactEmail = this.txtOrgEMail.Text;
                        um.SubmitChanges();
                    }
                    else
                    {
                        Organization newOrg = new Organization
                        {
                            ReceiptNo = this.txtOrgCode.Text,
                            CompanyName = this.txtOrgName.Text,
                            Addr = this.txtOrgAddr.Text,
                            Phone = this.txtOrgPhone.Text,
                            ContactEmail = this.txtOrgEMail.Text,
                            WelfareAgency = new WelfareAgency { },
                            OrganizationStatus = new OrganizationStatus
                            {
                                CurrentLevel = (int)Naming.MemberStatusDefinition.Checked
                            }
                        };

                        //方法一: 關聯建立好的相關TABLE,塞值的方式
                        //um.GetTable<OrganizationCategory>().InsertOnSubmit(new OrganizationCategory
                        //{
                        //    Organization=newOrg,
                        //    CategoryID = (int)Naming.CategoryID.COMP_WELFARE
                        //});

                        //方法二: 關聯建立好的相關TABLE,塞值的方式
                        newOrg.OrganizationCategory.Add(new OrganizationCategory
                        {
                            CategoryID = (int)Naming.CategoryID.COMP_WELFARE
                        });

                        um.GetTable<Organization>().InsertOnSubmit(newOrg);
                        um.SubmitChanges();
                    }
                }
                result = "ok";
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result = "系統錯誤:" + ex.Message;
            }
            return result;
        }

        #endregion       

        #region "Column Check"

        private string ColumnCheck()
        {
            string result = "ok";
            if (string.IsNullOrEmpty(this.txtOrgCode.Text))
            {
                result = "請填入社福機構統編";
            }
            else if (string.IsNullOrEmpty(this.txtOrgName.Text))
            {
                result = "請填入社福機構名稱";
            }
            else if (string.IsNullOrEmpty(this.txtOrgAddr.Text))
            {
                result = "請填入住址";
            }
            else if (string.IsNullOrEmpty(this.txtOrgPhone.Text))
            {
                result = "請填入電話";
            }
            return result;
        }

        #endregion
    }
}