using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;
using System.Security.Permissions;
using System.Security;


namespace eIVOGo.Module.SAM
{
    public partial class CreateOrganizationCertificate : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (_userProfile.CurrentUserRole.OrganizationCategory.CategoryID != (int)Naming.CategoryID.COMP_SYS)
            {
                Response.Redirect("~/logout.aspx");
            }

            //signContext.AutoSign = true;
            signContext.Launcher = btnUpload;
            if (!this.IsPostBack)
            {
                btnUpload.CommandArgument = Request["companyID"];
                btnConfirm.CommandArgument = Request["companyID"];
            }
        }

       
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(CreateOrganizationCertificate_PreRender);
            signContext.BeforeSign += new EventHandler(signContext_BeforeSign);
        }

        void CreateOrganizationCertificate_PreRender(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                int companyID = int.Parse(Request["companyID"]);
                var mgr = dsOrg.CreateDataManager();
                var item = mgr.EntityList.Where(o => o.CompanyID == companyID).FirstOrDefault();
                if (item.OrganizationToken != null)
                    msg.Text = String.Format("原憑證金鑰:{0}", item.OrganizationToken.KeyID);
            }
        }

        void signContext_BeforeSign(object sender, EventArgs e)
        {
            signContext.DataToSign = String.Format("憑證資料建立時間:{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);
        }

        private void saveCertIdentification(int companyID)
        {
            if (!signContext.Verify())
            {
                this.AjaxAlert("驗簽失敗!!");
                return;
            }

            X509Certificate2 cert = signContext.SignerCertificate;
            
            var mgr = dsOrg.CreateDataManager();
            var item = mgr.EntityList.Where(o => o.CompanyID == companyID).FirstOrDefault();
            if (item == null)
            {
                this.AjaxAlert("店家資料不存在!!");
                return;
            }

            if (item.OrganizationToken == null)
            {
                item.OrganizationToken = new OrganizationToken { };
            }

            item.OrganizationToken.X509Certificate = Convert.ToBase64String(cert.RawData);
            item.OrganizationToken.Thumbprint = cert.Thumbprint;

            mgr.SubmitChanges();

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                saveCertIdentification(int.Parse(btnUpload.CommandArgument));
                this.AjaxAlert("憑證識別更新完成!!");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert("憑證識別更新失敗!!");
                msg.Text = ex.Message;
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (PfxFile.HasFile)
            {
                try
                {
                    uploadCertIdentification(int.Parse(btnConfirm.CommandArgument));
                    this.AjaxAlert("憑證識別更新完成!!");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    this.AjaxAlert("憑證識別更新失敗!!");
                    msg.Text = ex.Message;
                }
            }
            else
            {
                this.AjaxAlert("請選取PFX憑證檔!!");
            }
        }

        private void uploadCertIdentification(int companyID)
        {

            var mgr = dsOrg.CreateDataManager();
            var item = mgr.EntityList.Where(o => o.CompanyID == companyID).FirstOrDefault();
            if (item == null)
            {
                this.AjaxAlert("店家資料不存在!!");
                return;
            }

            try
            {

                KeyContainerPermission perm = new KeyContainerPermission(KeyContainerPermissionFlags.Open | KeyContainerPermissionFlags.Export);
                perm.Assert();

                byte[] buf = new byte[PfxFile.PostedFile.ContentLength];
                PfxFile.PostedFile.InputStream.Read(buf, 0, PfxFile.PostedFile.ContentLength);
                X509Certificate2 cert = new X509Certificate2(buf, PIN.Text,X509KeyStorageFlags.Exportable);

                if (item.OrganizationToken == null)
                {
                    item.OrganizationToken = new OrganizationToken { };
                }

                Guid keyID = Guid.NewGuid();
                item.OrganizationToken.X509Certificate = Convert.ToBase64String(cert.RawData);
                item.OrganizationToken.Thumbprint = cert.Thumbprint;
                item.OrganizationToken.KeyID = keyID;
                item.OrganizationToken.PKCS12 = Convert.ToBase64String(cert.Export(X509ContentType.Pkcs12, keyID.ToString().Substring(0, 8)));

                mgr.SubmitChanges();
                msg.Text = String.Format("更新憑證金鑰:{0}", keyID);
            }
            finally
            {
                CodeAccessPermission.RevertAll();
            }
            

        }


    }
}