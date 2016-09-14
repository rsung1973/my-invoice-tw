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
    public partial class OrganizationEntrustingTokenItem : EditEntityItemBase<EIVOEntityDataContext, Organization>
    {
        protected UserProfileMember _userProfile;
        protected X509Certificate2 _certificate;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            signContext.Launcher = btnUpload;
            initializeData();
        }

        private void initializeData()
        {
            if (_tokenID.HasValue)
            {
                _certificate = new X509Certificate2(Convert.FromBase64String(
                    dsEntity.CreateDataManager().GetTable<UserToken>()
                    .Where(u => u.Token == _tokenID).First().X509Certificate));
            }
        }

       
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            signContext.BeforeSign += new EventHandler(signContext_BeforeSign);
            this.PreRender += new EventHandler(OrganizationEntrustingTokenItem_PreRender);
        }

        void OrganizationEntrustingTokenItem_PreRender(object sender, EventArgs e)
        {
            if (_certificate!=null)
            {
                tblAction.Visible = true;
                X509Certificate2 cert = new X509Certificate2(Convert.FromBase64String(
                    dsEntity.CreateDataManager().GetTable<UserToken>()
                    .Where(u=>u.Token==_tokenID).First().X509Certificate));
                certMsg.Text = cert.Subject;
                signContext.Thumbprint = cert.Thumbprint;
            }
        }

        void signContext_BeforeSign(object sender, EventArgs e)
        {
            loadEntity();

            if (_entity != null && _certificate != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("營業人名稱:").Append(_entity.CompanyName).Append("\r\n");
                sb.Append("營業人統編:").Append(_entity.ReceiptNo).Append("\r\n");
                sb.Append("使用者帳號:").Append(_userProfile.PID).Append("\r\n");
                sb.Append(String.Format("憑證資料建立時間:{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now)).Append("\r\n");
                sb.Append("您欲使用下述憑證資料營業人委由系統自動開立、接收發票相關資料之電子簽章數位憑證:\r\n")
                    .Append(_certificate.Subject).Append("\r\n")
                    .Append("姆指紋:").Append((new X509Certificate2(_certificate)).Thumbprint);
                signContext.DataToSign = sb.ToString();
            }
        }


        protected override bool saveEntity()
        {
            if (!signContext.Verify())
            {
                this.AjaxAlert("驗簽失敗!!");
                return false;
            }

            loadEntity();

            if (_entity == null)
            {
                this.AjaxAlert("會員資料不存在!!");
                return false;
            }

            if (_entity.OrganizationStatus == null)
            {
                _entity.OrganizationStatus = new OrganizationStatus
                {
                    CurrentLevel = (int)Naming.MemberStatusDefinition.Checked
                };
            }

            _entity.OrganizationStatus.TokenID = _tokenID;
            dsEntity.CreateDataManager().SubmitChanges();
            return true;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            doConfirm.DoAction(null);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (PfxFile.HasFile)
            {
                try
                {
                    uploadCertIdentification();
                    this.AjaxAlert("憑證提示完成!!");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    this.AjaxAlert("憑證提示失敗!!");
                }
            }
            else
            {
                this.AjaxAlert("請選取PFX憑證檔!!");
            }
        }

        protected Guid? _tokenID
        {
            get
            {
                return (Guid?)ViewState["cert"];
            }
            set
            {
                ViewState["cert"] = value;
            }
        }

        private void uploadCertIdentification()
        {
            loadEntity();
            if (_entity == null)
            {
                this.AjaxAlert("會員資料不存在!!");
                return;
            }

            try
            {
                var mgr = dsEntity.CreateDataManager();
                UserToken item = new UserToken
                {
                    LogonTime = DateTime.Now,
                    UID = _userProfile.UID,
                    Token = Guid.NewGuid()
                };

                KeyContainerPermission perm = new KeyContainerPermission(KeyContainerPermissionFlags.Open | KeyContainerPermissionFlags.Export);
                perm.Assert();

                byte[] buf = new byte[PfxFile.PostedFile.ContentLength];
                PfxFile.PostedFile.InputStream.Read(buf, 0, PfxFile.PostedFile.ContentLength);
                _certificate = new X509Certificate2(buf, PIN.Text,X509KeyStorageFlags.Exportable);

                item.X509Certificate = Convert.ToBase64String(_certificate.RawData);
                item.Thumbprint = _certificate.Thumbprint;
                item.PKCS12 = Convert.ToBase64String(_certificate.Export(X509ContentType.Pkcs12, item.Token.ToString().Substring(0, 8)));

                mgr.GetTable<UserToken>().InsertOnSubmit(item);
                mgr.SubmitChanges();

                _tokenID = item.Token;
            }
            finally
            {
                CodeAccessPermission.RevertAll();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            _tokenID = null;
            _certificate = null;
        }

    }
}