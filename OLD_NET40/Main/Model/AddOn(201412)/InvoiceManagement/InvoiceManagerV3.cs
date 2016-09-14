using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Helper;
using Model.InvoiceManagement.Validator;
using Model.InvoiceManagement.zhTW;
using Model.Locale;
using Model.Schema.EIVO;
using Utility;

namespace Model.InvoiceManagement
{
    public class InvoiceManagerV3 : InvoiceManagerV2
    {
        public InvoiceManagerV3() : base() { }
        public InvoiceManagerV3(GenericManager<EIVOEntityDataContext> mgr) : base(mgr) { }


        public override Dictionary<int, Exception> SaveUploadInvoice(InvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();

            if (item != null && item.Invoice != null && item.Invoice.Length > 0)
            {
                List<InvoiceItem> eventItems = new List<InvoiceItem>();
                InvoiceRootFormatValidator formatValidator = new InvoiceRootFormatValidator();
                InvoiceRootInvoiceValidator validator = new InvoiceRootInvoiceValidator(this, owner);

                //Organization donatory = owner.Organization.InvoiceWelfareAgencies.Select(w => w.WelfareAgency.Organization).FirstOrDefault();

                for (int idx = 0; idx < item.Invoice.Length; idx++)
                {
                    try
                    {
                        var invItem = item.Invoice[idx];
                        
                        Exception ex;
                        if ((ex = validator.Validate(invItem)) != null)
                        {
                            var errors = formatValidator.Validate(invItem);
                            if (errors.Count > 0)
                            {
                                result.Add(idx, new Exception(ex.Message + ";\r\n" + String.Join(";\r\n", errors.Where(x => x.Message != ex.Message)
                                    .Select(x => x.Message))));
                            }
                            else
                            {
                                result.Add(idx, ex);
                            } 
                            continue;
                        }

                        if (_checkUploadInvoice != null)
                        {
                            ex = _checkUploadInvoice();
                            if (ex != null)
                            {
                                result.Add(idx, ex);
                                continue;
                            }
                        }

                        InvoiceItem newItem = validator.InvoiceItem;

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

                if (eventItems.Count > 0)
                {
                    HasItem = true;
                    eventItems.Select(i => i.InvoiceID).SendIssuingNotification();
                }
                EventItems = eventItems;

            }
            return result;
        }

        public override Dictionary<int, Exception> SaveUploadInvoiceAutoTrackNo(InvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();

            if (item != null && item.Invoice != null && item.Invoice.Length > 0)
            {
                //Organization donatory = owner.Organization.InvoiceWelfareAgencies.Select(w => w.WelfareAgency.Organization).FirstOrDefault();
                EventItems = null;
                List<InvoiceItem> eventItems = new List<InvoiceItem>();

                InvoiceRootFormatValidator formatValidator = new InvoiceRootFormatValidator();
                InvoiceRootInvoiceValidator validator = new InvoiceRootInvoiceValidator(this, owner);
                validator.StartAutoTrackNo();
                for (int idx = 0; idx < item.Invoice.Length; idx++)
                {
                    try
                    {
                        var invItem = item.Invoice[idx];

                        Exception ex;
                        if ((ex = validator.Validate(invItem)) != null)
                        {
                            var errors = formatValidator.Validate(invItem);
                            if (errors.Count > 0)
                            {
                                result.Add(idx, new Exception(ex.Message + ";\r\n" + String.Join(";\r\n", errors.Where(x => x.Message != ex.Message)
                                    .Select(x => x.Message))));
                            }
                            else
                            {
                                result.Add(idx, ex);
                            }
                            continue;
                        }

                        InvoiceItem newItem = validator.InvoiceItem;

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
            
            if (item != null && item.Allowance != null && item.Allowance.Length > 0)
            {
                this.EventItems_Allowance = null;
                List<InvoiceAllowance> eventItems = new List<InvoiceAllowance>();

                AllowanceRootAllowanceValidator validator = new AllowanceRootAllowanceValidator(this, owner);
                var table = this.GetTable<InvoiceAllowance>();

                for (int idx = 0; idx < item.Allowance.Length; idx++)
                {
                    try
                    {
                        var allowanceItem = item.Allowance[idx];

                        Exception ex;
                        if((ex = validator.Validate(allowanceItem)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        InvoiceAllowance newItem = validator.Allowance;

                        table.InsertOnSubmit(newItem);
                        this.SubmitChanges();

                        eventItems.Add(newItem);

                    }
                    catch(Exception ex)
                    {
                        Logger.Error(ex);
                        result.Add(idx, ex);
                    }
                }
                EventItems_Allowance = eventItems;
            }

            return result;
        }
    }
}