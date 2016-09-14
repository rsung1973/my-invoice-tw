using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Security.MembershipManagement;
using Model.DataEntity;
using Model.Locale;
using eIVOGo.Module.Base;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.UI
{
    public partial class SocialWelfareSetup : EditEntityItemModal<EIVOEntityDataContext,Organization>
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.QueryExpr = o => o.CompanyID == (int?)modelItem.DataItem;
        }

        protected override bool saveEntity()
        {
            loadEntity();

            if (AgencyID.SelectedValues.Length==0)
            {
                WebMessageBox.AjaxAlert(this.Page, "尚未設定社福機構!!");
                return false;
            }
            else if(_entity==null)
            {
                WebMessageBox.AjaxAlert(this.Page, "未指定發票開立人!!");
                return false;
            }
            else
            {
                int agencyID = int.Parse(AgencyID.SelectedValues[0]);
                var mgr = dsEntity.CreateDataManager();
                {
                    var item = mgr.GetTable<InvoiceWelfareAgency>().Where(w=>w.SellerID==_entity.CompanyID).FirstOrDefault();
                    if(item==null)
                    {
                        item = new InvoiceWelfareAgency
                        {
                            SellerID = _entity.CompanyID
                        };
                        mgr.GetTable<InvoiceWelfareAgency>().InsertOnSubmit(item);
                    }
                    item.CreateTime = DateTime.Now;
                    item.AgencyID = agencyID;
                    mgr.SubmitChanges();
                }

               WebMessageBox.AjaxAlert(this.Page, "社福機構設定完成");
               return true;
            }
        }
    }
}