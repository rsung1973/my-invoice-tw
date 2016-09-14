using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;
using Model.SCMDataEntity;
using Model.InvoiceManagement;
using Uxnet.Com.Utility;
using Utility;
using Business.Helper;
using Model.Security.MembershipManagement;
using System.Linq.Expressions;
using Uxnet.Web.WebUI;
using Model.Locale;

namespace eIVOGo.Module.EIVO
{
    public partial class PupopDeleteReason : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        protected DocumentReasonForRefusal _item;

        public int? DocID
        {
            get
            {
                return (int?)ViewState["docID"];
            }
            set
            {
                ViewState["docID"] = value;
            }
        }

        public void Show()
        {
            _item = dsEntity.CreateDataManager().GetTable<DocumentReasonForRefusal>().Where(d => d.DocID == DocID).FirstOrDefault();
            if (_item != null)
            {
                this.ModalPopupExtender.Show();
                this.DataBind();
            }
        }
    }
}