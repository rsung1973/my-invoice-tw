using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Resource;

namespace Model.InvoiceManagement.Validator
{
    public class GoogleInvoiceRootInvoiceValidator : InvoiceRootInvoiceValidator
    {
        public GoogleInvoiceRootInvoiceValidator(GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner)
            : base(mgr, owner)
        {

        }

        protected override Exception checkBusiness()
        {
            Exception ex = base.checkBusiness();
            if (ex != null)
            {
                return ex;
            }

            if (String.IsNullOrEmpty(_invItem.GoogleId))
            {
                return new Exception(MessageResources.AlertGoogleId);
            }
            else if (_invItem.GoogleId.Length > 64)
            {
                return new Exception(String.Format("GoogleId can not be over 64 characters，TAG:< GoogleId />"));
            }

            return null;
        }

    }
}
