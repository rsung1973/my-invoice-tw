using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

using eIVOGo.Module.Common;
using Model.DataEntity;
using Model.Locale;
using Model.UploadManagement;
using Utility;
using eIVOGo.Properties;
using DataAccessLayer.basis;

namespace eIVOGo.Helper
{
    public class BusinessCounterpartUploadManager : CsvUploadManager<EIVOEntityDataContext, Organization, ItemUpload<Organization>>
    {
        public BusinessCounterpartUploadManager() : base() {

        }
        public BusinessCounterpartUploadManager(GenericManager<EIVOEntityDataContext> manager)
            : base(manager)
        {
        }

        public Naming.InvoiceCenterBusinessType BusinessType { get; set; }

        private int? _masterID;
        private List<UserProfile> _userList;

        public int? MasterID
        {
            get
            {
                return _masterID;
            }
            set
            {
                _masterID = value;
            }
        }

        public override void ParseData(Model.Security.MembershipManagement.UserProfileMember userProfile, string fileName, System.Text.Encoding encoding)
        {
            _userProfile = userProfile;
            if (!_masterID.HasValue)
                _masterID = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID;
            base.ParseData(userProfile, fileName, encoding);
        }

        protected override void initialize()
        {
            __COLUMN_COUNT = 6;
            _userList = new List<UserProfile>();
        }

        protected override void doSave()
        {
            this.SubmitChanges();

            var enterprise = this.GetTable<Organization>().Where(o => o.CompanyID == _masterID)
                .FirstOrDefault().EnterpriseGroupMember.FirstOrDefault();

            String subject = (enterprise != null ? enterprise.EnterpriseGroup.EnterpriseName : "") + " 會員啟用認證信";

            ThreadPool.QueueUserWorkItem(p =>
            {
                foreach (var u in _userList)
                {
                    try
                    {
                        String.Format("{0}{1}?id={2}", Uxnet.Web.Properties.Settings.Default.HostUrl, VirtualPathUtility.ToAbsolute(Settings.Default.NotifyActivation), u.UID)
                        .MailWebPage(u.EMail, subject);

                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("［" + subject + "］傳送失敗,原因 => " + ex.Message);
                        Logger.Error(ex);
                    }
                }
            });
        }

        protected override bool validate(ItemUpload<Organization> item)
        {
            String[] column = item.Columns;

            if (string.IsNullOrEmpty(column[0]))
            {
                item.Status = String.Join("、", item.Status, "營業人名稱格式錯誤");
                _bResult = false;
            }

            if (column[1].Length != 8 || !ValueValidity.ValidateString(column[1], 20))
            {
                item.Status = String.Join("、", item.Status, "統編格式錯誤");
                _bResult = false;
            }

            if (string.IsNullOrEmpty(column[2]) || !ValueValidity.ValidateString(column[2], 16))
            {
                item.Status = String.Join("、", item.Status, "聯絡人電子郵件格式錯誤");
                _bResult = false;
            }

            if (string.IsNullOrEmpty(column[3]))
            {
                item.Status = String.Join("、", item.Status, "地址格式錯誤");
                _bResult = false;
            }

            if (string.IsNullOrEmpty(column[4]))
            {
                item.Status = String.Join("、", item.Status, "電話格式錯誤");
                _bResult = false;
            }

            item.Entity = this.EntityList.Where(o => o.ReceiptNo == column[1]).FirstOrDefault();

            if (item.Entity == null)
            {
                item.Entity = new Organization
                {
                    OrganizationStatus = new OrganizationStatus
                    {
                        CurrentLevel = (int)Naming.MemberStatusDefinition.Checked
                    },
                    OrganizationExtension = new OrganizationExtension { }
                };

                new BusinessRelationship
                {
                    Counterpart = item.Entity,
                    BusinessID = (int)BusinessType,
                    MasterID = _masterID.Value,
                    CurrentLevel = (int)Naming.MemberStatusDefinition.Checked
                };

                var orgaCate = new OrganizationCategory
                {
                    Organization = item.Entity,
                    CategoryID = (int)Naming.CategoryID.COMP_E_INVOICE_B2C_BUYER
                };

                this.EntityList.InsertOnSubmit(item.Entity);

                checkUser(column, orgaCate, column[5].GetEfficientString() ?? column[1]);
            }
            else
            {
                if (!this.GetTable<BusinessRelationship>().Any(r => r.MasterID == _masterID && r.BusinessID == (int)BusinessType && r.RelativeID == item.Entity.CompanyID))
                {
                    new BusinessRelationship
                    {
                        Counterpart = item.Entity,
                        BusinessID = (int)BusinessType,
                        MasterID = _masterID.Value
                    };
                }

                var orgaCate = this.GetTable<OrganizationCategory>().Where(c => c.CategoryID == (int)Naming.CategoryID.COMP_E_INVOICE_B2C_BUYER
                        && c.CompanyID == item.Entity.CompanyID).FirstOrDefault();
                if (orgaCate == null)
                {
                    orgaCate = new OrganizationCategory
                    {
                        CategoryID = (int)Naming.CategoryID.COMP_E_INVOICE_B2C_BUYER,
                        CompanyID = item.Entity.CompanyID
                    };
                    this.GetTable<OrganizationCategory>().InsertOnSubmit(orgaCate);
                }

                var currentUser = checkUser(column, orgaCate, column[5].GetEfficientString() ?? column[1]);
                                currentUser.Phone = column[4];
                currentUser.EMail = column[2];
                currentUser.Address = column[3];
            }

            item.Entity.CompanyName = column[0];
            item.Entity.ReceiptNo = column[1];
            item.Entity.ContactEmail = column[2];
            item.Entity.Addr = column[3];
            item.Entity.Phone = column[4];
            item.Entity.OrganizationExtension.CustomerNo = column[5];

            return _bResult;
        }

        private UserProfile checkUser(string[] column, OrganizationCategory orgaCate,String pid)
        {
            var userProfile = this.GetTable<UserProfile>().Where(u => u.PID == pid).FirstOrDefault();
            if (userProfile == null)
            {
                userProfile = new UserProfile
                {
                    PID = pid,
                    Phone = column[4],
                    EMail = column[2],
                    Address = column[3],
                    Password2 = ValueValidity.MakePassword(pid),
                    UserProfileExtension = new UserProfileExtension
                    {
                        IDNo = column[1]
                    },
                    UserProfileStatus = new UserProfileStatus
                    {
                        CurrentLevel = (int)Naming.MemberStatusDefinition.Wait_For_Check
                    }
                };

                _userList.Add(userProfile);

                this.GetTable<UserRole>().InsertOnSubmit(new UserRole
                {
                    RoleID = (int)Naming.RoleID.ROLE_BUYER,
                    UserProfile = userProfile,
                    OrganizationCategory = orgaCate
                });
            }

            return userProfile;
        }
    }
}