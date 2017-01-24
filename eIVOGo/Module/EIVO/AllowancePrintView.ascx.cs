using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;
using Model.Security.MembershipManagement;
using Business.Helper;
using System.ComponentModel;
using Model.Locale;

namespace eIVOGo.Module.EIVO
{
    public partial class AllowancePrintView : System.Web.UI.UserControl
    {
        protected InvoiceAllowance _item;
        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        [Bindable(true)]
        public int? AllowanceID
        {
            get;
            set;
        }

        [Bindable(true)]
        public bool? IsFinal
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(AllowancePrintView_PreRender);
            this.Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            var mgr = dsEntity.CreateDataManager();
            if (!_item.CDS_Document.DocumentPrintLog.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Allowance))
            {                
                _item.CDS_Document.DocumentPrintLog.Add(new DocumentPrintLog
                {
                    PrintDate = DateTime.Now,
                    UID = _userProfile.UID,
                    TypeID = (int)Naming.DocumentTypeDefinition.E_Allowance
                });
            }

            mgr.DeleteAnyOnSubmit<DocumentPrintQueue>(d => d.DocID == _item.AllowanceID);
            mgr.SubmitChanges();
        }

        protected virtual void AllowancePrintView_PreRender(object sender, EventArgs e)
        {
            if (AllowanceID.HasValue)
            {
                var mgr = dsEntity.CreateDataManager();
                _item = mgr.GetTable<InvoiceAllowance>().Where(i => i.AllowanceID == AllowanceID).FirstOrDefault();
                balanceView.Item = _item;
                accountingView.Item = _item;
                this.DataBind();
            }
        }
    }
}