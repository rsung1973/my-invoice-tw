using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.DataEntity;
using Utility;
using Uxnet.Com.DataAccessLayer.Models;

namespace eIVOGo.Models
{
    public partial class InquireOrganizationReceiptNo : CommonInquiry<Organization>
    {
        public override void BuildQueryExpression(ModelSource<EIVOEntityDataContext, Organization> models)
        {
            if (!String.IsNullOrEmpty(CurrentController.Request["receiptNo"]))
            {
                models.Items = models.Items.Where(i => i.ReceiptNo.StartsWith(CurrentController.Request["receiptNo"].Trim()));
                this.HasSet = true;
            }

            base.BuildQueryExpression(models);
        }
    }

    public partial class InquireCompanyName : CommonInquiry<Organization>
    {

        public override void BuildQueryExpression(ModelSource<EIVOEntityDataContext, Organization> models)
        {
            if (!String.IsNullOrEmpty(CurrentController.Request["companyName"]))
            {
                models.Items = models.Items.Where(i => i.CompanyName.Contains(CurrentController.Request["companyName"].Trim()));
                this.HasSet = true;
            }

            base.BuildQueryExpression(models);
        }
    }

    public partial class InquireOrganizationStatus : CommonInquiry<Organization>
    {
        public override void BuildQueryExpression(ModelSource<EIVOEntityDataContext, Organization> models)
        {
            int orgStatus;
            if (!String.IsNullOrEmpty(CurrentController.Request["organizationStatus"]) && int.TryParse(CurrentController.Request["organizationStatus"],out orgStatus))
            {
                models.Items = models.Items.Where(i => i.OrganizationStatus.CurrentLevel == orgStatus);
                this.HasSet = true;
            }

            base.BuildQueryExpression(models);
        }
    }

}