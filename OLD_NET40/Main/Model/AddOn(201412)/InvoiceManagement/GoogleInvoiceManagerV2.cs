using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI.WebControls;
using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Helper;
using Model.InvoiceManagement.enUS;
using Model.InvoiceManagement.Validator;
using Model.Locale;
using Model.Properties;
using Model.Schema.EIVO;
using Utility;

namespace Model.InvoiceManagement
{
    public partial class GoogleInvoiceManagerV2 : GoogleInvoiceManager
    {

        public GoogleInvoiceManagerV2()
            : base()
        {

        }
        public GoogleInvoiceManagerV2(GenericManager<EIVOEntityDataContext> mgr)
            : base(mgr)
        {

        }

        public override Dictionary<int, Exception> SaveUploadInvoiceAutoTrackNo(InvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();

            if (item != null && item.Invoice != null && item.Invoice.Length > 0)
            {
                //Organization donatory = owner.Organization.InvoiceWelfareAgencies.Select(w => w.WelfareAgency.Organization).FirstOrDefault();
                EventItems = null;
                List<InvoiceItem> eventItems = new List<InvoiceItem>();

                GoogleInvoiceRootInvoiceValidator validator = new GoogleInvoiceRootInvoiceValidator(this, owner);
                validator.StartAutoTrackNo();
                for (int idx = 0; idx < item.Invoice.Length; idx++)
                {
                    try
                    {
                        var invItem = item.Invoice[idx];

                        Exception ex;
                        if ((ex = validator.Validate(invItem)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        InvoiceItem newItem = validator.InvoiceItem;
                        newItem.CDS_Document.DocumentOwner.ClientID = InvoiceClientID;

                        this.EntityList.InsertOnSubmit(newItem);
                        this.SubmitChanges();

                        eventItems.Add(newItem);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        result.Add(idx, ex);
                    }
                }
                validator.EndAutoTrackNo();

                if (eventItems.Count > 0)
                {
                    HasItem = true;
                    eventItems.Select(i => i.InvoiceID).SendIssuingNotification();
                }
                EventItems = eventItems;
            }

            return result;
        }

        public override Dictionary<int, Exception> SaveUploadAllowance(AllowanceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if(item!=null && item.Allowance!=null&&item.Allowance.Length>0)
            {
                AllowanceRootAllowanceValidator validator = new AllowanceRootAllowanceValidator(this, owner);
                var table = this.GetTable<InvoiceAllowance>();

                for(int idx = 0;idx < item.Allowance.Length;idx++)
                {
                    try
                    {
                        var allowanceItem = item.Allowance[idx];
                        Exception ex;
                        if ((ex = validator.Validate(allowanceItem)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        InvoiceAllowance newItem = validator.Allowance;
                        table.InsertOnSubmit(newItem);
                        this.SubmitChanges();
                    }
                    catch(Exception ex)
                    {
                        Logger.Error(ex);
                        result.Add(idx, ex);
                    }
                }
            }

            return result;
        }
    }
}
