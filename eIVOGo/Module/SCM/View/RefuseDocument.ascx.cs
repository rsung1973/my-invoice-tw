using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.WebUI;
using Model.SCMDataEntity;
using Model.Security.MembershipManagement;
using Business.Helper;
using Model.Locale;

namespace eIVOGo.Module.SCM.View
{
    public partial class RefuseDocument : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;

        public event EventHandler Done;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        protected CDS_Document _item;

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
            _item = dsEntity.CreateDataManager().EntityList.Where(d => d.DocID == DocID).FirstOrDefault();
            if (_item != null)
            {
                this.ModalPopupExtender.Show();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ModalPopupExtender.Hide();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Reason.Text))
            {
                var mgr = dsUpdate.CreateDataManager();
                _item = mgr.EntityList.Where(d => d.DocID == DocID).FirstOrDefault();
                _item.CurrentStep = (int)Naming.DocumentStepDefinition.已刪除;

                DateTime now = DateTime.Now;

                mgr.GetTable<DocumentProcessLog>().InsertOnSubmit(new DocumentProcessLog
                {
                    DocID = _item.DocID,
                    UID = _userProfile.UID,
                    FlowStep = (int)Naming.DocumentStepDefinition.已刪除,
                    StepDate = now
                });

                mgr.GetTable<DocumentReasonForRefusal>().InsertOnSubmit(new DocumentReasonForRefusal
                {
                    Reason = Reason.Text,
                    DocID = _item.DocID,
                    TimeToRefuse = now
                });

                mgr.SubmitChanges();

                if (Done != null)
                {
                    Done(this, new EventArgs());
                }
            }
            else
            {
                this.AjaxAlert("請填入刪除原因!!");
                return;
            }
        }
    }
}