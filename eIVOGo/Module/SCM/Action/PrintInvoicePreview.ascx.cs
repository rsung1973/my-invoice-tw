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

namespace eIVOGo.Module.SCM.Action
{
    public partial class PrintInvoicePreview : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
//            btnPrint.PrintControlSource.Add(invoiceView.AppRelativeVirtualPath);
        }

        public void Show(int invoiceID)
        {
            invoiceView.InvoiceID = invoiceID;
            this.ModalPopupExtender.Show();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ModalPopupExtender.Hide();
        }

    }
}