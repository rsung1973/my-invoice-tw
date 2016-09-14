using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Model.DataEntity;
using Utility;

namespace eIVOGo.Published
{
    public partial class NotifyMemberActivation : System.Web.UI.Page
    {
        protected UserProfile _item;
        protected String _tempPWD;

        protected void Page_Load(object sender, EventArgs e)
        {
            int uid;
            if (!String.IsNullOrEmpty(Request["id"]) && int.TryParse(Request["id"], out uid))
            {
                var mgr = dsEntity.CreateDataManager();
                _item = mgr.EntityList.Where(u => u.UID == uid).FirstOrDefault();
                if (_item != null)
                {
                    _tempPWD = 6.CreateRandomPassword();
                    _item.Password2 = ValueValidity.MakePassword(_tempPWD);
                    mgr.SubmitChanges();
                    this.DataBind();
                }
            }
        }
    }
}