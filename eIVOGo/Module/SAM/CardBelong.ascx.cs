using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Security.MembershipManagement;
using Model.DataEntity;
using Model.Locale;
using Business.Helper;

namespace eIVOGo.Module.SAM
{
    public partial class CardBelong : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
          
            if (!this.IsPostBack)
            {
                initCareType();
                btnQuery_Click( sender,  e);
            }


        }

        protected void initCareType()
        {
            using (UserManager mgr = new UserManager())
            {
                var cate = from d in mgr.GetTable<InvoiceUserCarrierType >().ToList()
                              select new
                              {
                                  cateid = d.TypeID,
                                  name = d.CarrierType
                              };

                if (cate.Count() > 0)
                {
                    this.ddlcardType.DataSource = cate;
                    this.ddlcardType.DataTextField = "name";
                    this.ddlcardType.DataValueField = "cateid";
                    this.ddlcardType.DataBind();
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            using (UserManager mgr = new UserManager())
            {
                //過濾訪客及系統管理員
                var UserCarrier = mgr.GetTable<InvoiceUserCarrier>().Where(w => w.UID == _userProfile.UID);
                if (UserCarrier.Count() > 0)
                {

                    var griddata = from u in UserCarrier
                                   select new
                                   {
                                       CareType = u.InvoiceUserCarrierType.CarrierType, 
                                       CareID = u.CarrierNo,
                                       CarrierID=u.CarrierID 


                                   };
                    this.gvEntity.DataSource = griddata.ToList();
                    this.gvEntity.DataBind();
                }


            }
        }

        protected void gvEntity_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvEntity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvEntity.PageIndex = e.NewPageIndex;
            btnQuery_Click(sender, e);
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            this.lblmsg.Text = "";    
            if (this.CardID.Text.Trim() != "")
            {
                using (UserManager mgr = new UserManager())
                {
                    
                    var UserCarrier = mgr.GetTable<InvoiceUserCarrier>().Where(w => w.CarrierNo == this.CardID.Text.Trim() && w.TypeID == int.Parse(this.ddlcardType.SelectedValue ) && w.DeleteFlag != true);
                    if (UserCarrier.Count() > 0)
                    {
                        foreach (var row in UserCarrier)
                        {
                            row.UID = _userProfile.UID;
                        }
                        mgr.SubmitChanges();
                        this.lblmsg.Text = "卡號歸戶完成";
                        btnQuery_Click(sender, e);
                    }
                    else
                    {
                        var newuc = new InvoiceUserCarrier();
                        newuc.UID = _userProfile.UID;
                        newuc.TypeID = int.Parse(this.ddlcardType.SelectedValue);
                        newuc.CarrierNo = this.CardID.Text.Trim();
                        newuc.CarrierNo2 = this.CardID.Text.Trim();
                        newuc.DeleteFlag = false;
                        mgr.GetTable<InvoiceUserCarrier>().InsertOnSubmit(newuc);

                        mgr.SubmitChanges();
                        this.lblmsg.Text = "卡號歸戶完成";
                        btnQuery_Click(sender, e);
                    }


                }
            }
            else
            {
                this.lblmsg.Text = "卡碼不得為空白";    
            }
        }

        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
            this.lblmsg.Text = "";
           
            int CareID = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "Delete")
            {
                using (UserManager mgr = new UserManager())
                {
                    var UserCarrier = mgr.GetTable<InvoiceUserCarrier >().Where(w => w.CarrierID == CareID).FirstOrDefault();
                    if (UserCarrier != null)
                    {
                        mgr.GetTable<InvoiceUserCarrier>().DeleteOnSubmit(UserCarrier);
                        mgr.SubmitChanges();
                        this.lblmsg .Text = "卡號刪除完成";
                        btnQuery_Click(sender, e);
                    }

                }
            }
        
        }

        protected void gvEntity_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}