using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.DataEntity;
using Model.Security.MembershipManagement;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SAM.Action
{
    public partial class ActivateOrganizationToken : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;
        private OrganizationToken _orgToken;
        private X509Certificate2 _cert;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            signContext.Launcher = btnConfirm;
            _orgToken = dsEntity.CreateDataManager().GetTable<OrganizationToken>().Where(o => o.CompanyID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID).FirstOrDefault();
            if (_orgToken != null && !String.IsNullOrEmpty(_orgToken.X509Certificate))
            {
                _cert = new X509Certificate2(Convert.FromBase64String(_orgToken.X509Certificate));
                buildCertInfo();
            }
            else
            {
                UpdatePanel1.AjaxAlert("用戶端傳輸工具數位憑證尚未設定!!");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(ActivateOrganizationToken_PreRender);
            signContext.BeforeSign += new EventHandler(signContext_BeforeSign);
        }

        void signContext_BeforeSign(object sender, EventArgs e)
        {
            if (_cert != null)
            {
                signContext.DataToSign = agreement.InnerText + certInfo.Text;
            }
        }

        void ActivateOrganizationToken_PreRender(object sender, EventArgs e)
        {
            btnConfirm.Visible = _cert != null;
        }

        private void buildCertInfo()
        {
            certInfo.Text = String.Format("憑證資訊:\r\n{0}\r\n姆指紋:{1}", _cert.Subject, _cert.Thumbprint);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (signContext.Verify())
            {
                var mgr = dsEntity.CreateDataManager();
                _orgToken.IsActivated = true;
                mgr.SubmitChanges();

                UpdatePanel1.AjaxAlert("用戶端數位憑證授權完成!!");
            }
            else
            {
                UpdatePanel1.AjaxAlert("驗簽失敗!!");
            }
        }
    }
}