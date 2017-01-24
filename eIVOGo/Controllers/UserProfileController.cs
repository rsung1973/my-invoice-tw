using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using Business.Helper;
using ClosedXML.Excel;
using eIVOGo.Helper;
using eIVOGo.Models;
using eIVOGo.Models.ViewModel;
using eIVOGo.Properties;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;

namespace eIVOGo.Controllers
{
    [Authorize]
    public class UserProfileController : SampleController<InvoiceItem>
    {
        
        [HttpGet]
        public ActionResult EditMySelf(bool? forCheck)
        {
            var profile = HttpContext.GetUser();
            UserProfileViewModel viewModel = new UserProfileViewModel
            {
                WaitForCheck = forCheck
            };

            var item = models.GetTable<UserProfile>().Where(u => u.PID == profile.PID).FirstOrDefault();

            if (item != null)
            {
                viewModel.KeyID = Convert.ToBase64String(AppResource.Instance.EncryptSalted(BitConverter.GetBytes(item.UID)));
                viewModel.PID = item.PID;
                viewModel.UserName = item.UserName;
                viewModel.EMail = item.EMail;
                viewModel.Address = item.Address;
                viewModel.Phone = item.Phone;
                viewModel.MobilePhone = item.MobilePhone;
                viewModel.Phone2 = item.Phone2;
            }

            ViewBag.ViewModel = viewModel;
            return View(viewModel);
        }

        public ActionResult EditItem(UserProfileViewModel viewModel)
        {
            var item = models.GetTable<UserProfile>().Where(u => u.UID == viewModel.UID).FirstOrDefault();

            if (item != null)
            {
                viewModel.KeyID = Convert.ToBase64String(AppResource.Instance.EncryptSalted(BitConverter.GetBytes(item.UID)));
                viewModel.PID = item.PID;
                viewModel.UserName = item.UserName;
                viewModel.EMail = item.EMail;
                viewModel.Address = item.Address;
                viewModel.Phone = item.Phone;
                viewModel.MobilePhone = item.MobilePhone;
                viewModel.Phone2 = item.Phone2;
            }

            ViewBag.ViewModel = viewModel;
            return View("~/Views/UserProfile/EditUserProfile.ascx", viewModel);
        }

        public ActionResult Commit(UserProfileViewModel viewModel)
        {

            UserProfile item = null;
            int? uid = null;

            if (!String.IsNullOrEmpty(viewModel.KeyID))
            {
                uid = BitConverter.ToInt32(AppResource.Instance.DecryptSalted(Convert.FromBase64String(viewModel.KeyID)), 0);
                item = models.GetTable<UserProfile>().Where(u => u.UID == uid).FirstOrDefault();
            }


            viewModel.PID = viewModel.PID.GetEfficientString();
            if(String.IsNullOrEmpty(viewModel.PID))
            {
                ModelState.AddModelError("PID", "帳號不可為空白!!");
            }
            else if(models.GetTable<UserProfile>().Any(u=>u.UID!=uid && u.PID==viewModel.PID))
            {
                ModelState.AddModelError("PID", "這個帳號已被使用，請更換申請帳號!!");
            }

            Regex reg = new Regex("^(?=.*\\d)(?=.*[a-zA-Z])");

            if (!String.IsNullOrEmpty(viewModel.Password))
            {
                if (viewModel.Password.Length < 6)
                {
                    //檢查密碼
                    ModelState.AddModelError("PassWord", "密碼不可少於６個字碼!!");
                }
                else if (!reg.IsMatch(viewModel.Password))
                {
                    //檢查密碼
                    ModelState.AddModelError("PassWord", "密碼須由英文、數字組成!!");
                }
                else if (viewModel.Password != viewModel.Password1)
                {
                    //檢查密碼
                    ModelState.AddModelError("PassWord1", "二組密碼輸入不同!!");
                }
            }
            else if (item == null)
            {
                ///新增帳號
                ///
                if (String.IsNullOrEmpty(viewModel.Password))
                {
                    ModelState.AddModelError("PassWord", "密碼不可為空白!!");
                }
            }

            var profile = WebPageUtility.UserProfile;
            int? orgaCateID = null;

            if (viewModel.SellerID.HasValue)
            {
                orgaCateID = models.GetTable<OrganizationCategory>().Where(c => c.CompanyID == viewModel.SellerID)
                    .Select(c => c.OrgaCateID).FirstOrDefault();
            }

            if(!orgaCateID.HasValue)
            {
                if (profile.IsSystemAdmin())
                {
                    ModelState.AddModelError("SellerID", "請選擇所屬會員!!");
                }
                else
                {
                    orgaCateID = profile.CurrentUserRole.OrgaCateID;
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ModelState = this.ModelState;
                return View("~/Views/Shared/ReportInputError.ascx");
            }

            if (item == null)
            {
                item = new UserProfile
                {
                    UserProfileStatus = new UserProfileStatus
                    {
                        CurrentLevel = (int)Naming.MemberStatusDefinition.Wait_For_Check
                    }
                };
                item.UserRole.Add(new UserRole
                {
                    OrgaCateID = orgaCateID.Value,
                    RoleID = viewModel.DefaultRoleID ?? (int)Naming.EIVOUserRoleID.會員
                });
                models.GetTable<UserProfile>().InsertOnSubmit(item);
            }
            else
            {
                if (profile.IsSystemAdmin())
                {
                    if (!models.GetTable<UserRole>().Any(c => c.OrgaCateID == orgaCateID && c.UID == item.UID))
                    {
                        models.DeleteAllOnSubmit<UserRole>(r => r.UID == item.UID);
                        item.UserRole.Add(new UserRole
                        {
                            OrgaCateID = orgaCateID.Value,
                            RoleID = (int)Naming.EIVOUserRoleID.會員
                        });
                    }
                }
            }

            item.PID = viewModel.PID;
            item.UserName = viewModel.UserName;
            if (!String.IsNullOrEmpty(viewModel.Password))
            {
                item.Password2 = Utility.ValueValidity.MakePassword(viewModel.Password);
                if (viewModel.WaitForCheck == true)
                    item.UserProfileStatus.CurrentLevel = (int)Naming.MemberStatusDefinition.Checked;
            }
            item.EMail = viewModel.EMail;
            item.Address = viewModel.Address;
            item.Phone = viewModel.Phone;
            item.MobilePhone = viewModel.MobilePhone;
            item.Phone2 = viewModel.Phone2;

            models.SubmitChanges();

            if (viewModel.WaitForCheck == true)
            {
                return View("~/Views/UserProfile/AccountChecked.ascx");
            }
            else
            {
                return View("~/Views/SiteAction/Alert.ascx", model: "資料已修改!!");
            }
        }


    }
}