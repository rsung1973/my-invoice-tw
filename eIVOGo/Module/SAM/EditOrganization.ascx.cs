using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using Business.Helper;
using eIVOGo.Helper;
using eIVOGo.Module.Base;
using eIVOGo.Module.Common;
using eIVOGo.Properties;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;
using DataAccessLayer.basis;
using eIVOGo.Module.UI;

namespace eIVOGo.Module.SAM
{  
   
    public partial class EditOrganization : EditEntityItemBase<EIVOEntityDataContext, Organization>
    {
        //public XmlDocument config = new XmlDocument();
       public  XmlDocument config
        {
        get;
        set;

        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
                
        }

        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(AddMember_PreRender);
            this.QueryExpr = m => m.CompanyID == (int?)modelItem.DataItem;
        }

        void AddMember_PreRender(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                String[] itemNames = Enum.GetNames(typeof(Naming.InvoiceTypeDefinition));
                Array itemValues = Enum.GetValues(typeof(Naming.InvoiceTypeDefinition));

                this.InvoiceType.Items.Insert(0, new ListItem("請選擇", ""));
                for (int i = 0; i < itemNames.Length; i++)
                {
                    this.InvoiceType.Items.Add(new ListItem(String.Format("{0:00}{1}", (int)itemValues.GetValue(i), itemNames[i]), ((int)itemValues.GetValue(i)).ToString()));
                }
            }

            if (_entity != null)
            {
                modelItem.DataItem = _entity.CompanyID;
                this.InvoiceType.SelectedValue = _entity.OrganizationStatus.SettingInvoiceType.Value.ToString();
            }
            else
            {
                this.SetToOutsourcingCS.Checked = true;
            }
        }

        protected override bool saveEntity()
        {
            var mgr = dsEntity.CreateDataManager();

            loadEntity();

            String receiptNo = ReceiptNo.Text.Trim();
            if (_entity == null || _entity.ReceiptNo != receiptNo)
            {
                if (mgr.GetTable<Organization>().Any(o => o.ReceiptNo == receiptNo))
                {
                    this.AjaxAlert("相同的企業統編已存在!!");
                    return false;
                }
            }

            if (String.IsNullOrEmpty(CategoryID.SelectedValue))
            {
                this.AjaxAlert("請設定公司類別!!");
                return false;
            }

            if (String.IsNullOrEmpty(InvoiceType.SelectedValue))
            {
                this.AjaxAlert("請設定發票類別!!");
                return false;
            }

            bool isNewItem = false;
            OrganizationCategory orgaCate = null;
            if (_entity == null)
            {
                _entity = new Organization
                    {
                        OrganizationStatus = new OrganizationStatus
                        {
                            CurrentLevel = (int)Naming.MemberStatusDefinition.Checked
                        }

                    };

                orgaCate = new OrganizationCategory
                {
                    Organization = _entity
                };

                mgr.EntityList.InsertOnSubmit(_entity);
                isNewItem = true;
            }
            else
            {
                orgaCate = _entity.OrganizationCategory.First();
            }

            orgaCate.CategoryID = int.Parse(CategoryID.SelectedValue);

            _entity.ReceiptNo = receiptNo;
            _entity.CompanyName = CompanyName.Text.Trim().InsteadOfNullOrEmpty(null);
            _entity.Addr = Addr.Text.Trim().InsteadOfNullOrEmpty(null);
            _entity.Phone = Phone.Text.Trim().InsteadOfNullOrEmpty(null);
            _entity.Fax = Fax.Text.Trim().InsteadOfNullOrEmpty(null);
            _entity.UndertakerName = UndertakerName.Text.Trim().InsteadOfNullOrEmpty(null);
            _entity.ContactName = ContactName.Text.Trim().InsteadOfNullOrEmpty(null);
            _entity.ContactTitle = ContactTitle.Text.Trim().InsteadOfNullOrEmpty(null);
            _entity.ContactPhone = ContactPhone.Text.Trim().InsteadOfNullOrEmpty(null);
            _entity.ContactMobilePhone = ContactMobilePhone.Text.Trim().InsteadOfNullOrEmpty(null);
            _entity.ContactEmail = ContactEmail.Text.Trim().InsteadOfNullOrEmpty(null);
            _entity.OrganizationStatus.SetToPrintInvoice = SetToPrintInvoice.Checked;
            _entity.OrganizationStatus.SetToOutsourcingCS = SetToOutsourcingCS.Checked;
            _entity.OrganizationStatus.InvoicePrintView = _entity.OrganizationStatus.SetToPrintInvoice.Value ? InvoicePrintView.Text : null;
            _entity.OrganizationStatus.AllowancePrintView = _entity.OrganizationStatus.SetToPrintInvoice.Value ? AllowancePrintView.Text : null;
            _entity.OrganizationStatus.AuthorizationNo = AuthorizationNo.Text.Trim().InsteadOfNullOrEmpty(null);
            _entity.OrganizationStatus.SetToNotifyCounterpartBySMS = SetToNotifyCounterpartBySMS.Checked;
            _entity.OrganizationStatus.DownloadDataNumber = DownloadDataNumber.Checked;
            _entity.OrganizationStatus.UploadBranchTrackBlank = UploadBranchTrackBlank.Checked;
            _entity.OrganizationStatus.PrintAll = PrintAll.Checked;
            _entity.OrganizationStatus.SettingInvoiceType = int.Parse(InvoiceType.SelectedValue);
            _entity.OrganizationStatus.SubscribeB2BInvoicePDF = SubscribeB2BInvoicePDF.Checked;
            _entity.OrganizationStatus.UseB2BStandalone = UseB2BStandalone.Checked;
            _entity.OrganizationStatus.DisableWinningNotice = !SetWinningNotice.Checked;
            if (!_entity.OrganizationValueCheck(this))
            {
                return false;
            }

            mgr.SubmitChanges();

            if (isNewItem)
            {
                createDefaultUser(mgr, orgaCate);
            }

            return true;
        }

        private void createDefaultUser(GenericManager<EIVOEntityDataContext, Organization> mgr, OrganizationCategory orgaCate)
        {
            var userProfile = new UserProfile
            {
                PID = _entity.ReceiptNo,
                Phone = _entity.Phone,
                EMail = _entity.ContactEmail,
                Address = _entity.Addr,
                UserProfileExtension = new UserProfileExtension
                {
                    IDNo = _entity.ReceiptNo
                },
                UserProfileStatus = new UserProfileStatus
                {
                    CurrentLevel = (int)Naming.MemberStatusDefinition.Wait_For_Check
                }
            };

            mgr.GetTable<UserRole>().InsertOnSubmit(new UserRole
            {
                RoleID = (int)Naming.RoleID.ROLE_SELLER,
                UserProfile = userProfile,
                OrganizationCategory = orgaCate
            });

            mgr.SubmitChanges();

            try
            {
                String.Format("{0}{1}?id={2}", eIVOGo.Properties.Settings.Default.mailLinkAddress, VirtualPathUtility.ToAbsolute(Settings.Default.NotifyActivation), userProfile.UID)
                    .MailWebPage(userProfile.EMail, "電子發票系統 會員啟用認證信");
            }
            catch (Exception ex)
            {
                Logger.Warn("［電子發票系統 會員啟用認證信］傳送失敗,原因 => " + ex.Message);
                Logger.Error(ex);
            }
        }

        //protected void Config_btn_Click(object sender, EventArgs e)
        //{
        //    CreateConfig();
        //    try
        //    {

        //        string _successfulMsg = "檔案下載中，請稍後...!!";
        //        PopupModal modal = (PopupModal)this.LoadControl("~/Module/UI/PopupModal.ascx");
        //        modal.InitializeAsUserControl(this.Page);
        //        modal.TitleName = _successfulMsg;
        //        _successfulMsg = null;
        //        this.Controls.Add(modal);
        //        LiteralControl lc = new LiteralControl(String.Format("<iframe src='{0}?printBack={1}' height='0' width='0'></iframe>",
        //        VirtualPathUtility.ToAbsolute("~/EIVO/DownloadAppconfig.aspx"), Request["printBack"]));
        //        modal.Show();
        //        modal.Controls.Add(lc);
               
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //}
        //public XmlDocument CreateConfig()
        //{

        //    string _path = Path.Combine(Logger.LogPath, "InvoiceClient.exe.config");
        //    config = new XmlDocument();
        //    config.Load(_path);
        //    var Properties = config["configuration"]["applicationSettings"]["InvoiceClient.Properties.Settings"];
        //    Properties.SelectSingleNode("setting[@name='ActivationKey']").FirstChild.InnerText= string.Empty;
            
        //    if (int.Parse(CategoryID.SelectedValue) == (int)Naming.B2CCategoryID.Google台灣)
        //    {
        //        //MainTabs
        //        Properties.SelectSingleNode("setting[@name='MainTabs']").FirstChild.InnerText = "InvoiceClient.MainContent.GoogleGUIConfigTab, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;";
        //        if (Request["svcType"] == "0")
        //        {//測試機
        //            //InvoiceClient_WS_Invoice_eInvoiceService
        //            Properties.SelectSingleNode("setting[@name='InvoiceClient_WS_Invoice_eInvoiceService']").FirstChild.InnerText =
        //                "https://eivo4all.uxifs.com/neweivo03/Published/eInvoiceService_Google.asmx";
        //            //UploadAttachment
        //            Properties.SelectSingleNode("setting[@name='UploadAttachment']").FirstChild.InnerText =
        //                "http://eivo4all.uxifs.com/neweivo03/Published/UploadAttachmentForGoogle.ashx";
        //        }
        //        else
        //        {//正式機
        //            //InvoiceClient_WS_Invoice_eInvoiceService
        //            Properties.SelectSingleNode("setting[@name='InvoiceClient_WS_Invoice_eInvoiceService']").FirstChild.InnerText =
        //                "https://eceivo.uxifs.com/Published/eInvoiceService_Google.asmx";
        //            //UploadAttachment
        //            Properties.SelectSingleNode("setting[@name='UploadAttachment']").FirstChild.InnerText =
        //                "http://eceivo.uxifs.com/Published/UploadAttachmentForGoogle.ashx";
                
        //        }
            
        //    }
        //    else
        //    {
        //        //MainTabs
        //        Properties.SelectSingleNode("setting[@name='MainTabs']").FirstChild.InnerText = "InvoiceClient.MainContent.SystemConfigTab, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;";
                
        //        //UploadAttachment
        //        if (Request["svcType"] == "0")
        //            Properties.SelectSingleNode("setting[@name='UploadAttachment']").FirstChild.InnerText =
        //                "http://eivo4all.uxifs.com/neweivo03/Published/UploadAttachment.ashx";
        //        else
        //            Properties.SelectSingleNode("setting[@name='UploadAttachment']").FirstChild.InnerText =
        //                "http://eceivo.uxifs.com/Published/UploadAttachment.ashx";

        //        //InvoiceClient_WS_Invoice_eInvoiceService
        //        if (int.Parse(CategoryID.SelectedValue) == (int)Naming.B2CCategoryID.開立發票店家代理)
        //        {
        //            if (Request["svcType"] == "0")
        //                Properties.SelectSingleNode("setting[@name='InvoiceClient_WS_Invoice_eInvoiceService']").FirstChild.InnerText =
        //                "https://eivo4all.uxifs.com/neweivo03/Published/eInvoiceService_Proxy.asmx";
        //            else
        //                Properties.SelectSingleNode("setting[@name='InvoiceClient_WS_Invoice_eInvoiceService']").FirstChild.InnerText =
        //                "https://eceivo.uxifs.com/Published/eInvoiceService_Proxy.asmx";

        //            Properties.SelectSingleNode("setting[@name='TransferManager']").FirstChild.InnerText =
        //                "InvoiceClient.TransferManagement.InvoiceTransferManagerV2ForProxy, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;";
                    
        //        }
        //        else
        //        {
        //            if (Request["svcType"] == "0")
        //                Properties.SelectSingleNode("setting[@name='InvoiceClient_WS_Invoice_eInvoiceService']").FirstChild.InnerText =
        //                "https://eivo4all.uxifs.com/neweivo03/Published/eInvoiceService.asmx";
        //            else
        //                Properties.SelectSingleNode("setting[@name='InvoiceClient_WS_Invoice_eInvoiceService']").FirstChild.InnerText =
        //               "https://eceivo.uxifs.com/Published/eInvoiceService.asmx";
                    
        //        }
        //    }
        //    //TransferManager
        //    if (int.Parse(CategoryID.SelectedValue) != (int)Naming.B2CCategoryID.開立發票店家代理)
        //    {
        //        if (Request["uploadType"]!="MIG")
        //            Properties.SelectSingleNode("setting[@name='TransferManager']").FirstChild.InnerText =
        //                    "InvoiceClient.TransferManagement.InvoiceTransferManagerV2, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;";
                        
        //        else
        //            Properties.SelectSingleNode("setting[@name='TransferManager']").FirstChild.InnerText =
        //                   "InvoiceClient.TransferManagement.MIGInvoiceTransferManager, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;";
                        

        //    }
        //    //if (InvoiceAttachment.Checked)
        //    //    Properties.SelectSingleNode("setting[@name='TransferManager']").FirstChild.InnerText +=
        //    //        "InvoiceClient.TransferManagement.InvoiceAttachmentTransferManager, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;";


        //    //ServerInspector
        //    Properties.SelectSingleNode("setting[@name='ServerInspector']").FirstChild.InnerText = string.Empty;
        //    if (DownloadDataNumber.Checked)
        //        Properties.SelectSingleNode("setting[@name='ServerInspector']").FirstChild.InnerText +=
        //            "InvoiceClient.Agent.InvoiceMappingInspector, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;";
        //    if (SubscribeB2BInvoicePDF.Checked)
        //        Properties.SelectSingleNode("setting[@name='ServerInspector']").FirstChild.InnerText +=
        //            "InvoiceClient.Agent.InvoicePDFInspector, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;";
        //    //if (MailTracking.Checked)
        //    //    Properties.SelectSingleNode("setting[@name='ServerInspector']").FirstChild.InnerText +=
        //    //        "InvoiceClient.Agent.InvoiceMailTrackingInspector, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;";
         
        //    modelItem2.DataItem = config;
        //    return config;
        //}
    }
}