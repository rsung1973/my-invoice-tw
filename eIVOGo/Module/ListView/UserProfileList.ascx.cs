using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eIVOGo.Module.Base;
using Model.DataEntity;
using Model.Security.MembershipManagement;
using Business.Helper;
using Model.Locale;
using eIVOGo.Properties;
using eIVOGo.Module.Common;
using Utility;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.ListView
{
    public partial class UserProfileList : EntityItemList<EIVOEntityDataContext, UserProfile>
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            doActivate.DoAction = arg => 
            {
                var mgr = dsEntity.CreateDataManager();
                var item = mgr.EntityList.Where(m => m.UID == int.Parse(arg)).First();
                item.UserProfileStatus.CurrentLevel = (int)Naming.MemberStatusDefinition.Checked;
                mgr.SubmitChanges();
            };
            doCreate.DoAction = arg =>
            {
                Server.Transfer(ToEdit.TransferTo);
            };
            doEdit.DoAction = arg =>
            {
                modelItem.DataItem = int.Parse(arg);
                Server.Transfer(ToEdit.TransferTo);
            };
            doDelete.DoAction = arg =>
            {
                delete(arg);
            };
            doSendMail.DoAction = arg =>
                {
                    try
                    {
                        var mgr = dsEntity.CreateDataManager();
                        var item = mgr.EntityList.Where(u => u.UID == int.Parse(arg)).First();
                        String.Format("{0}{1}?id={2}", Settings.Default.mailLinkAddress, VirtualPathUtility.ToAbsolute(Settings.Default.NotifyActivation), item.UID)
                            .MailWebPage(item.EMail, "電子發票系統 會員啟用認證信");
                        this.AjaxAlert("認證信已寄出!!");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        this.AjaxAlert("［電子發票系統 會員啟用認證信］傳送失敗,原因 => " + ex.Message);
                    }
                };
        }


        protected void delete(string keyValue)
        {
            var mgr = dsEntity.CreateDataManager();
            var item = mgr.EntityList.Where(m => m.UID == int.Parse(keyValue)).First();
            item.UserProfileStatus.CurrentLevel = (int)Naming.MemberStatusDefinition.Mark_To_Delete;
            mgr.SubmitChanges();
        }

    }
}