using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eIVOGo.Module.EIVO;
using Model.Security.MembershipManagement;
using Business.Helper;
using Model.InvoiceManagement;
using Model.DataEntity;
using Model.Locale;

namespace eIVOGo.SAM
{
    public partial class PrintInvoicePage : System.Web.UI.Page
    {
        private UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            initializeData();
        }

        protected virtual void initializeData()
        {
            IEnumerable<int> items;
            String invoicePrintView = null;
            using (InvoiceManager mgr = new InvoiceManager())
            {
                //items = mgr.GetTable<InvoicePrintQueue>().Where(i => i.UID == _userProfile.UID).Select(i => i.InvoiceID).ToList();
                items = mgr.GetTable<DocumentPrintQueue>().Where(i => i.UID == _userProfile.UID 
                    && i.CDS_Document.DocType == (int)Naming.DocumentTypeDefinition.E_Invoice)
                    .Select(i => i.DocID).ToList();
                //if (items != null && items.Count() > 0)
                //{
                //    invoicePrintView = mgr.GetTable<DocumentPrintQueue>().Where(i => i.UID == _userProfile.UID & i.CDS_Document.DocType == (int)Naming.DocumentTypeDefinition.E_Invoice).First().CDS_Document.InvoiceItem.Organization.OrganizationStatus.InvoicePrintView;
                //    if (String.IsNullOrEmpty(invoicePrintView))
                //    {
                //        invoicePrintView = "~/Module/EIVO/InvoicePrintView.ascx";
                //    }
                //}

                if (items != null && items.Count() > 0)
                {
                    InvoicePrintView finalView = null;
                    foreach (var item in items)
                    {
                        var invoice = mgr.EntityList.Where(i => i.InvoiceID == item).FirstOrDefault();
                        if (invoice == null)
                            continue;
                        invoicePrintView = invoice.Organization.OrganizationStatus.InvoicePrintView;

                        if (String.IsNullOrEmpty(invoicePrintView))
                        {
                            invoicePrintView = "~/Module/EIVO/InvoicePrintView.ascx";
                        }

                        InvoicePrintView view = (InvoicePrintView)this.LoadControl(invoicePrintView);
                        view.InitializeAsUserControl(this.Page);
                        view.InvoiceID = item;
                        theForm.Controls.Add(view);
                        finalView = view;
                    }
                    if (finalView != null)
                        finalView.IsFinal = true;
                }
            }
        }
    }
}