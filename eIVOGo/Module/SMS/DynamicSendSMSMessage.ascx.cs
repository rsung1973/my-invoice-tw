using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using eIVOGo.Module.Common;
using eIVOGo.Module.UI;
using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.Inquiry
{
    public partial class DynamicSendSMSMessage : System.Web.UI.UserControl
    {
        UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;

            if (!Page.IsPostBack)
            {
                var mgr = dsEntity.CreateDataManager();
                if (_userProfile.CurrentUserRole.RoleID != ((int)Naming.RoleID.ROLE_SYS))
                {
                    if (mgr.GetTable<OrganizationStatus>().Where(o => o.CompanyID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID).FirstOrDefault().SetToNotifyCounterpartBySMS != true)
                    {
                        this.typeTable.Visible = false;
                        this.btnTable.Visible = false;
                        this.lblMsg.Visible = true;
                    }
                    else
                    {
                        this.ddlCompany.SelectedValue = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID.ToString();
                        this.ddlCompany.Enabled = false;
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.IsPostBack)
            {
                loadCompany();
            }
        }

        #region "Function or Methor"
        protected void loadCompany()
        {
            var mgr = dsEntity.CreateDataManager();
            this.ddlCompany.Items.AddRange(mgr.GetTable<Organization>().Where(o => o.OrganizationStatus.SetToNotifyCounterpartBySMS==true &  o.OrganizationCategory.Any(c => c.CategoryID == (int)Naming.B2CCategoryID.Google台灣 || c.CategoryID == (int)Naming.B2CCategoryID.店家 || c.CategoryID == (int)Naming.B2CCategoryID.店家發票自動配號)).Select(o => new ListItem(o.ReceiptNo.Trim() + " " + o.CompanyName.Trim(), o.CompanyID.ToString())).ToArray());
        }

        //private Boolean sendSMS(object stateInfo)
        //{
        //    Boolean isSending = false;
        //    var mgr = dsEntity.CreateDataManager();
        //    var logs = mgr.GetTable<SMSNotificationLog>();

        //    ModelExtension.MessageManagement.SMSHelper sms = new ModelExtension.MessageManagement.SMSHelper();
        //    if (sms.Start())
        //    {
        //        string subject = "客服訊息";
        //        string content = this.txtMsgContent.Text.Trim();
        //        string mobile = this.rdbFreeType.Checked ? this.txtMobilNo.Text.Trim() : this.rdbUpload.Checked ? this.txtMobilePreview.Text.Trim() : "";
        //        int owner = !String.IsNullOrEmpty(this.ddlCompany.SelectedValue.Trim()) ? int.Parse(this.ddlCompany.SelectedValue) : _userProfile.CurrentUserRole.OrganizationCategory.CompanyID;

        //        if (!VarifyPhoneNo(mobile))
        //        {
        //            WebMessageBox.AjaxAlert(this, "手機號碼格式錯誤!!");
        //        }
        //        else
        //        {
        //            if (sms.SendSMS(subject, content, mobile))
        //            {
        //                logs.InsertOnSubmit(new SMSNotificationLog
        //                {
        //                    MessageID = (int)Naming.MessageTypeDefinition.客服訊息通知,
        //                    SubmitDate = DateTime.Now,
        //                    OwnerID = owner,
        //                    SendingMobil = mobile,
        //                    SendingContent = content
        //                });
        //                mgr.SubmitChanges();
        //                isSending = true;
        //            }
        //        }
        //        sms.Close();
        //    }
        //    return isSending;
        //}

        private Boolean sendSMS(object stateInfo)
        {
            Boolean isSending = false;
            String content = this.txtMsgContent.Text.Trim();
            ModelExtension.MessageManagement.SMSManager smsMgr = new ModelExtension.MessageManagement.SMSManager();
            smsMgr.BuildMessageContent = id =>
                {
                    return new Model.Helper.NotificationMesssage
                    {
                        Content = content,
                        Subject = "客服訊息"
                    };
                };
            smsMgr.ExceptionHandler = SharedFunction.AlertSMSError;
            string mobile = this.rdbFreeType.Checked ? this.txtMobilNo.Text.Trim() : this.rdbUpload.Checked ? this.txtMobilePreview.Text.Trim() : "";
            smsMgr.OwnerID = !String.IsNullOrEmpty(this.ddlCompany.SelectedValue.Trim()) ? int.Parse(this.ddlCompany.SelectedValue) : _userProfile.CurrentUserRole.OrganizationCategory.CompanyID;

            if (!VarifyPhoneNo(mobile))
            {
                WebMessageBox.AjaxAlert(this, "手機號碼格式錯誤!!");
            }
            else
            {
                smsMgr.MobileNo = mobile;
                smsMgr.ProcessMessage();
                isSending = true;
            }

            return isSending;
        }


        protected Boolean VarifyPhoneNo(string NOs)
        {
            Boolean isOK = true;
            Regex reg = new Regex("[0-9]");
            string[] pNo = NOs.Split(',');
            foreach (var d in pNo)
            {
                if (!reg.IsMatch(d))
                {
                    isOK = false;
                    break;
                }
            }
            return isOK;
        }

        protected void CleanAllContent()
        {
            this.ddlCompany.SelectedIndex = 0;
            this.txtMobilNo.Text = "";
            this.txtMobilePreview.Text = "";
            this.txtMsgContent.Text = "";
        }
        #endregion

        #region "Countrol Event"
        protected void rdbFreeType_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbFreeType.Checked)
            {
                this.txtMobilNo.Enabled = true;
                this.txtMobilePreview.Text = "";
                this.Upload.Enabled = false;
                this.btnUploadPreview.Enabled = false;
            }
            else if (this.rdbUpload.Checked)
            {
                this.txtMobilNo.Enabled = false;
                this.txtMobilNo.Text = "";
                this.Upload.Enabled = true;
                this.btnUploadPreview.Enabled = true;
            }
        }

        protected void btnUploadPreview_Click(object sender, EventArgs e)
        {
            if (this.Upload.HasFile)
            {
                using (StreamReader sr = new StreamReader(this.Upload.FileContent))
                {
                    string textLine = "";
                    while (sr.Peek() >= 0)
                    {
                        textLine += sr.ReadLine() + ",";
                    }
                    sr.Close();
                    this.txtMobilePreview.Text = textLine.Substring(0, textLine.Length - 1);
                }
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (sendSMS(null))
            {
                CleanAllContent();
                WebMessageBox.AjaxAlert(this, "簡訊已送出!!");
            }
            else
            {
                WebMessageBox.AjaxAlert(this, "系統錯誤，簡訊未送出!!");
            }
        }
        #endregion

    }    
}