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
using eIVOGo.Module.SCM.View;
using System.ComponentModel;
using eIVOGo.Module.Base;

namespace eIVOGo.Module.SCM.Action
{
    public partial class PrintEntityPreview : System.Web.UI.UserControl
    {
        private UserControl _preview;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!String.IsNullOrEmpty(PreviewControlPath))
            {
                btnPrint.PrintControlSource.Add(PreviewControlPath);
                _preview = (UserControl)this.LoadControl(PreviewControlPath);
                _preview.InitializeAsUserControl(Page);
                Panel3.Controls.AddAt(0, _preview);
            }
        }

        public void Show(int orderSN)
        {
            if (_preview is ISCMEntityPreview)
            {
                ((ISCMEntityPreview)_preview).PrepareDataFromDB(orderSN);
                this.ModalPopupExtender.Show();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ModalPopupExtender.Hide();
        }

        [Bindable(true)]
        public String PreviewControlPath
        {
            get;
            set;
        }

    }
}