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
    public partial class NewPrintInvoicePage : System.Web.UI.Page
    {
        protected UserProfileMember _userProfile;
        protected String _defaultPrintView = "~/Module/EIVO/NewInvoicePrintView.ascx";

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
                    NewInvoicePrintView finalView = null;
                    foreach (var item in items)
                    {
                        var invoice = mgr.EntityList.Where(i => i.InvoiceID == item).FirstOrDefault();
                        if (invoice == null)
                            continue;
                        invoicePrintView = invoice.Organization.OrganizationStatus.InvoicePrintView;
                        if (String.IsNullOrEmpty(invoicePrintView))
                        {
                            if (invoice.Organization.AsInvoiceInsurer != null && invoice.Organization.AsInvoiceInsurer.Count > 0)
                            {
                                invoicePrintView = invoice.Organization.AsInvoiceInsurer
                                    .First().InvoiceAgent.OrganizationStatus.InvoicePrintView;
                            }
                        }
                        if (String.IsNullOrEmpty(invoicePrintView))
                        {
                            invoicePrintView = _defaultPrintView;
                        }

                        //NewInvoicePrintView view = (NewInvoicePrintView)this.LoadControl(invoicePrintView);
                        //view.InitializeAsUserControl(this.Page);
                        //view.InvoiceID = item;
                        //theForm.Controls.Add(view);
                        finalView = (NewInvoicePrintView)this.LoadControl(invoicePrintView);
                        finalView.IsFinal = false;
                        finalView.InitializeAsUserControl(this.Page);
                        finalView.InvoiceID = item;
                        theForm.Controls.Add(finalView);

                        if (item.Equals(items.Last()))
                            finalView.IsFinal = true;
                    }
                    //if (finalView != null)
                    //    finalView.IsFinal = true;
                }
            }
        }
    }
}