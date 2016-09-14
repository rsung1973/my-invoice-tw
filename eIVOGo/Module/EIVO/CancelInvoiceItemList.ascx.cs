using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using eIVOGo.Module.Base;
using eIVOGo.Helper;
using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;
using System.Text;
using Business.Helper;

namespace eIVOGo.Module.EIVO
{
    public partial class CancelInvoiceItemList : EntityItemList<EIVOEntityDataContext, InvoiceItem>
    {
        private UserProfileMember _userProfile;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //this.PreRender += new EventHandler(CancelInvoiceItemList_PreRender);
            this.Load += new EventHandler(CancelInvoiceItemList_Load);
        }

        void CancelInvoiceItemList_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        //void CancelInvoiceItemList_PreRender(object sender, EventArgs e)
        //{
        //    this.btnPreview.Visible = dsInv.CurrentView.LastSelectArguments.TotalRowCount > 0;
        //}

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            getValue();
            if (this.rejectItem.DataItem != null)
            {
                Dictionary<int, String> inputValues;
                inputValues = (Dictionary<int, String>)rejectInput.DataItem;

                foreach (int appID in (int[])rejectItem.DataItem)
                {
                    if (String.IsNullOrEmpty(inputValues[appID]))
                    {
                        this.AjaxAlert("勾選作廢項目時，請輸入作廢原因!!");
                        this.rejectItem.DataItem = null;
                        this.rejectInput.DataItem = null;
                        return;
                    }
                }
                //this.AjaxAlert("進入預覽!!");
                Response.Redirect(NextAction.RedirectTo);
            }
            else
            {
                this.AjaxAlert("請選擇要作廢發票!!");
            }
        }

        protected void getValue()
        {
            this.rejectItem.DataItem = null;
            this.rejectInput.DataItem = null;

            String[] ar = Request.GetItemSelection();
            if (ar != null && ar.Count() > 0)
            {
                this.rejectItem.DataItem = ar.Select(s => int.Parse(s)).ToArray();
            }

            if (Request.Form.AllKeys.Any(s => s.StartsWith("i_")))
            {
                Dictionary<int, String> values = new Dictionary<int, String>();
                foreach (var item in Request.Form.AllKeys.Where(s => s.StartsWith("i_")))
                {
                    values[int.Parse(item.Substring(2))] = Request[item];
                }
                rejectInput.DataItem = values;
            }
        }
    }    
}