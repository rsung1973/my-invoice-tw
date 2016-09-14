using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Security.MembershipManagement;
using Model.DataEntity;

using Business.Helper;
using Model.Locale;
using Uxnet.Web.WebUI;
using System.Text.RegularExpressions;
using Utility;

namespace eIVOGo.Module.SAM
{
    public partial class EditMyself : EditMember
    {
        protected override void initializeData()
        {
            UserProfileMember user = WebPageUtility.UserProfile;

            var item = dsUserProfile.CreateDataManager().EntityList.Where(u => u.UID == user.UID).FirstOrDefault();
            if (item == null)
            {
                WebMessageBox.AjaxAlert(this, "使用者不存在!!");
            }

            if (item.UserProfileStatus.CurrentLevel == (int)Naming.MemberStatusDefinition.Wait_For_Check)
            {
                user.CurrentSiteMenu = "WaitForCheckMenu.xml";
            }
           
            EditItem.DataItem = item;
        }
    }
}