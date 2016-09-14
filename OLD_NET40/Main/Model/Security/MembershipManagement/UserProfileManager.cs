using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;

using Model.DataEntity ;
using DataAccessLayer.basis;
using Model.Properties;
using Model.Locale;

namespace Model.Security.MembershipManagement
{
    public partial class UserProfileManager : GenericManager<EIVOEntityDataContext ,UserProfile>
    {
        public UserProfileManager() : base() { }
        public UserProfileManager(GenericManager<EIVOEntityDataContext> mgr) : base(mgr) { }

        public UserProfile GetUserProfile(int uid)
        {
            return this.EntityList.Where(u => u.UID == uid).FirstOrDefault();
        }

        public bool UpdateUserProfileCert(int uid, X509Certificate2 cert)
        {
            UserProfile item = this.EntityList.Where(u => u.UID == uid).FirstOrDefault();
            if (item != null)
            {
                UserAuth auth = this.GetTable<UserAuth>().Where(a => a.Thumbprint == cert.Thumbprint).FirstOrDefault();
                if (auth == null)
                {
                    auth = new UserAuth
                    {
                        UserProfile = item ,
                        Thumbprint = cert.Thumbprint,
                        X509Certificate = Convert.ToBase64String(cert.RawData)
                    };

                    this.GetTable<UserAuth>().InsertOnSubmit(auth);
                    this.SubmitChanges();
                    return true; 
                }
            }
            return false;
        }

        public UserProfile GetUserProfile(X509Certificate2 cert)
        {
            UserAuth auth = this.GetTable<UserAuth>().Where(a => a.Thumbprint == cert.Thumbprint).FirstOrDefault();
            if (auth != null && auth.AuthID==auth.UserProfile.UserAuth.OrderByDescending(a=>a.AuthID).ElementAt(0).AuthID)
            {
                return auth.UserProfile;
            }
            return null;
        }

        public Guid? LogonUser(X509Certificate2 cert)
        {
            UserAuth auth = this.GetTable<UserAuth>().Where(a => a.Thumbprint == cert.Thumbprint).FirstOrDefault();
            if (auth != null)
            {
                return LogonUser(auth.UID);
            }

            return null;
        }

        public Guid? LogonUser(int uid)
        {
            UserToken item = new UserToken
            {
                LogonTime = DateTime.Now,
                UID = uid,
                Token = Guid.NewGuid()
            };

            this.GetTable<UserToken>().InsertOnSubmit(item);
            this.SubmitChanges();

            return item.Token;
        }

        public UserProfile GetUserProfile(Guid token, double intervalInMinutes)
        {
            UserToken item = this.GetTable<UserToken>().Where(t => t.Token == token).FirstOrDefault();

            if (item != null && item.LogonTime.AddMinutes(intervalInMinutes) >= DateTime.Now)
            {
                item.LogonTime = DateTime.Now;
                this.SubmitChanges();
                return item.UserProfile;
            }
            return null;
        }

        public void CreateConsumerProfile(UserProfile profile)
        {
            var orgaCate = this.GetTable<OrganizationCategory>().Where(w => w.CategoryID==(int)Naming.CategoryID.COMP_E_INVOICE_B2C_BUYER).FirstOrDefault();
            if(orgaCate!=null)
            {
                profile.UserRole.Add(new UserRole
                {
                    OrgaCateID = orgaCate.OrgaCateID,
                    RoleID = (int)Naming.RoleID.ROLE_BUYER
                });

                this.GetTable<UserProfileStatus>().InsertOnSubmit(new UserProfileStatus
                {
                    UserProfile = profile,
                    CurrentLevel = (int)Naming.MemberStatusDefinition.Wait_For_Check,
                });

                this.EntityList.InsertOnSubmit(profile);
                this.SubmitChanges();
            }
        }

        public IQueryable<UserProfile> GetAllSellerUser(IQueryable<UserProfile> items)
        {
            return items.Join(this.GetTable<UserRole>().Where(r => r.OrganizationCategory.CategoryID == (int)Naming.CategoryID.COMP_E_INVOICE_B2C_SELLER), u => u.UID, r => r.UID, (u, r) => u);
        }

        public IQueryable<UserProfile> GetAllBuyerUser(IQueryable<UserProfile> items)
        {
            return items.Join(this.GetTable<UserRole>().Where(r => r.OrganizationCategory.CategoryID == (int)Naming.CategoryID.COMP_E_INVOICE_B2C_BUYER), u => u.UID, r => r.UID, (u, r) => u);
        }

        public IQueryable<UserProfile> GetUserByUserRole(IQueryable<UserProfile> items,int roleID)
        {
            return items.Join(this.GetTable<UserRole>().Where(r => r.RoleID == roleID), u => u.UID, r => r.UID, (u, r) => u);
        }




    }
}
