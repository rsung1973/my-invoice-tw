using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Helper;
using Model.InvoiceManagement.zhTW;
using Model.Locale;
using Model.Schema.EIVO;
using Utility;
using System.Globalization;
using Model.InvoiceManagement.Validator;

namespace Model.InvoiceManagement
{
    public class MIGInvoiceManager : InvoiceManagerV2
    {
        public MIGInvoiceManager() : base() { }
        public MIGInvoiceManager(GenericManager<EIVOEntityDataContext> mgr) : base(mgr) { }

        public virtual Exception SaveUploadInvoice(Model.Schema.MIG3_1.C0401.Invoice invItem, OrganizationToken owner)
        {

            if (invItem != null)
            {
                List<InvoiceItem> eventItem = new List<InvoiceItem>();
                C0401Validator validator = new C0401Validator(this, owner);

                try
                {

                    Exception ex;
                    if ((ex = validator.Validate(invItem)) != null)
                    {
                        return ex;
                    }

                    if (_checkUploadInvoice != null)
                    {
                        ex = _checkUploadInvoice();
                        if (ex != null)
                        {
                            return ex;
                        }
                    }

                    InvoiceItem newItem = validator.InvoiceItem;

                    this.EntityList.InsertOnSubmit(newItem);
                    this.SubmitChanges();

                    eventItem.Add(newItem);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }

                if (eventItem.Count > 0)
                {
                    HasItem = true;
                    eventItem.Select(i => i.InvoiceID).SendIssuingNotification();
                }

            }
            return null;
        }

    }
}
