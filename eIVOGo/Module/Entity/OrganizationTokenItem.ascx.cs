using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using eIVOGo.Module.Base;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;


namespace eIVOGo.Module.Entity
{
    public partial class OrganizationTokenItem : EditEntityItemBase<EIVOEntityDataContext, Organization>
    {
        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;

            if (_certificate == null)
            {
                signContext.Launcher = btnUpload;
            }
            else
            {
                signContext.Launcher = btnConfirm;
            }
        }

       
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            signContext.BeforeSign += new EventHandler(signContext_BeforeSign);
            this.PreRender += new EventHandler(OrganizationTokenItem_PreRender);
        }

        void OrganizationTokenItem_PreRender(object sender, EventArgs e)
        {
            if (_certificate == null)
            {
                tblPrompt.Visible = true;
            }
            else
            {
                tblAction.Visible = true;
                certMsg.Text = _certificate.Subject;
                signContext.Thumbprint = (new X509Certificate2(_certificate)).Thumbprint;
            }

        }

        protected X509Certificate _certificate
        {
            get
            {
                return ViewState["cert"] as X509Certificate;
            }
            set
            {
                ViewState["cert"] = value;
            }
        }

        void signContext_BeforeSign(object sender, EventArgs e)
        {
            loadEntity();

            if (_entity != null)
            {
                StringBuilder sb = new StringBuilder();
                if (_certificate == null)
                {
                    sb.Append("營業人名稱:").Append(_entity.CompanyName).Append("\r\n");
                    sb.Append("營業人統編:").Append(_entity.ReceiptNo).Append("\r\n");
                    sb.Append("使用者帳號:").Append(_userProfile.PID).Append("\r\n");
                    sb.Append(String.Format("提示會員憑證時間:{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now)).Append("\r\n");
                }
                else
                {
                    sb.Append("營業人名稱:").Append(_entity.CompanyName).Append("\r\n");
                    sb.Append("營業人統編:").Append(_entity.ReceiptNo).Append("\r\n");
                    sb.Append("使用者帳號:").Append(_userProfile.PID).Append("\r\n");
                    sb.Append("您欲使用下述憑證資料建立或修改為營業人於本系統之電子簽章數位憑證:\r\n")
                        .Append(_certificate.Subject).Append("\r\n")
                        .Append("姆指紋:").Append((new X509Certificate2(_certificate)).Thumbprint);
                }
                signContext.DataToSign = sb.ToString();
            }
        }

        private void saveCertIdentification()
        {
            _certificate = null;

            if (!signContext.Verify())
            {
                this.AjaxAlert("驗簽失敗!!");
                return;
            }

            _certificate = new X509Certificate(signContext.SignerCertificate);
            signContext.Launcher = btnConfirm;

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                saveCertIdentification();
                this.AjaxAlert("憑證提示完成!!");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert("憑證提示失敗!!");
            }
        }

        protected override bool saveEntity()
        {
            if (!signContext.Verify())
            {
                this.AjaxAlert("驗簽失敗!!");
                return false;
            }

            _certificate = new X509Certificate(signContext.SignerCertificate);

            loadEntity();

            if (_entity == null)
            {
                this.AjaxAlert("會員資料不存在!!");
                return false;
            }

            X509Certificate2 cert = signContext.SignerCertificate;

            if (_entity.OrganizationToken == null)
            {
                _entity.OrganizationToken = new OrganizationToken
                {
                };
            }

            _errorMsg = "無法建立憑證資訊,憑證資料重複!!";

            _entity.OrganizationToken.X509Certificate = Convert.ToBase64String(cert.RawData);
            _entity.OrganizationToken.Thumbprint = cert.Thumbprint;

            dsEntity.CreateDataManager().SubmitChanges();
            return true;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            doConfirm.DoAction(null);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            _certificate = null;
            signContext.Launcher = btnUpload;
        }
    }
}