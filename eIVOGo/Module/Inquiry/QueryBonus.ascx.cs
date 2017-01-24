using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Locale;
using Model.Security.MembershipManagement;
using Model.InvoiceManagement;
using Business.Helper;
using Model.DataEntity;
using Uxnet.Web.Module.Common;
using Utility;

namespace eIVOGo.Module.Inquiry
{
    public partial class QueryBonus : System.Web.UI.UserControl, IPostBackEventHandler
    {
        UserProfileMember _userProfile;

        public class dataType
        {
            public int id;
            public string range;
            public string TrackCode;
            public string No;
            public string CompanyName;
            public string Addr;
            public string CarrierType;
            public string Donation;
            public string DonateMark;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (!Page.IsPostBack)
            {
                if (_userProfile.CurrentUserRole.RoleID == ((int)Naming.RoleID.ROLE_GUEST))
                {
                    this.rdbType1.Visible = false;
                    this.rdbType2.Visible = false;
                    this.lblDevice.Visible = true;
                    this.ddlDevice.Visible = true;
                }
                LoadDevice();
                LoadRange();
                this.PrintingButton21.btnPrint.Text = "資料列印";
                this.PrintingButton21.btnPrint.CssClass = "btn";                
            }
            this.PrintingButton21.PrintControls.Add(this.gvEntity);
        }

        protected override void OnInit(EventArgs e)
        {
            gvEntity.PreRender += new EventHandler(gvEntity_PreRender);
            this.PrintingButton21.BeforeClick += new EventHandler(btnPrint_BeforeClick);
        }

        void btnPrint_BeforeClick(object sender, EventArgs e)
        {
            this.gvEntity.AllowPaging = false;
        }

        #region "Load Page Control Data"
        protected void LoadDevice()
        {
            this.ddlDevice.Items.Clear();
            this.ddlDevice.Items.Add(new ListItem("-請選擇-", "0"));
            this.ddlDevice.Items.Add(new ListItem("UXB2B條碼卡", "1"));
            this.ddlDevice.Items.Add(new ListItem("悠遊卡", "2"));
        }

        protected void LoadRange()
        {
            int year = 0;
            int month = 0;
            this.ddlRange1.Items.Clear();
            this.ddlRange2.Items.Clear();

            for (int m = 2; m < 26; m += 2)
            {
                DateTime date = DateTime.Now.AddMonths(-m);
                year = date.Year;
                month = date.Month;
                if (date.Month % 2 == 1)
                {
                    month += 1;
                }
                this.ddlRange1.Items.Add(new ListItem((year - 1911).ToString().PadLeft(3, '0') + "年 " + (month - 1).ToString("00") + "-" + month.ToString("00") + "月", year.ToString() + (month - 1).ToString("00") + month.ToString("00")));
                this.ddlRange2.Items.Add(new ListItem((year - 1911).ToString().PadLeft(3, '0') + "年 " + (month - 1).ToString("00") + "-" + month.ToString("00") + "月", year.ToString() + (month - 1).ToString("00") + month.ToString("00")));
            }
        }
        #endregion

        #region "Page Control Event"
        protected void rdbType_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbType1.Checked == true)
            {
                this.ddlDevice.SelectedIndex = 0;
                this.ddlDevice.Visible = false;
                this.uxb2b.Visible = false;
            }
            else
            {
                this.ddlDevice.Visible = true;
            }
        }

        protected void ddlDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlDevice.SelectedValue == "1")
                this.uxb2b.Visible = true;
            else
                this.uxb2b.Visible = false;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.divResult.Visible = true;
            SearchData();
        }
        #endregion

        #region "Gridview Event"
        void gvEntity_PreRender(object sender, EventArgs e)
        {
            SearchData();
        }
        #endregion

        #region "Search Data"
        private void SearchData()
        {
            using (InvoiceManager im = new InvoiceManager())
            {
                try
                {

                    var data = from d in im.GetTable<InvoiceWinningNumber>()
                               select new
                               {
                                   d.InvoiceID,
                                   d.UniformInvoiceWinningNumber.Year,
                                   MonthFrom = d.UniformInvoiceWinningNumber.Period*2-1,
                                   MonthTo = d.UniformInvoiceWinningNumber.Period*2,
                                   d.InvoiceItem.TrackCode,
                                   d.InvoiceItem.No,
                                   d.InvoiceItem.InvoiceDate,
                                   d.InvoiceItem.Organization.CompanyName,
                                   d.InvoiceItem.Organization.Addr,
                                   d.InvoiceItem.InvoiceByHousehold.InvoiceUserCarrier.InvoiceUserCarrierType.CarrierType,
                                   Donation = im.GetTable<Organization>().Where(og => og.CompanyID == d.InvoiceItem.DonationID).FirstOrDefault().CompanyName,
                                   d.InvoiceItem.DonateMark,
                                   d.InvoiceItem.SellerID,
                                   d.InvoiceItem.InvoiceByHousehold.InvoiceUserCarrier.UID
                               };

                    //當登入者為賣方店家時,則只能查到自己的中獎發票資訊,若非店家而是一般使用者,則僅能查詢自己的中獎發票資訊
                    if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_SELLER || _userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_GOOGLETW)
                    {
                        data = data.Where(d => d.SellerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID);
                    }
                    else
                    {
                        data = data.Where(d => d.UID == _userProfile.UID);
                    }

                    int Year1 = int.Parse(this.ddlRange1.SelectedValue.Substring(0, 4));
                    int Year2 = int.Parse(this.ddlRange2.SelectedValue.Substring(0, 4));

                    DateTime sDate = new DateTime();
                    DateTime eDate = new DateTime();

                    if (Year1 > Year2)
                    {
                        int Month1 = int.Parse(this.ddlRange2.SelectedValue.Substring(4, 2));
                        int Month2 = int.Parse(this.ddlRange1.SelectedValue.Substring(6, 2));
                        sDate = new DateTime(Year2, Month1, 1);
                        eDate = Month2 == 12 ? new DateTime(Year1 + 1, 1, 1) : new DateTime(Year1, Month2 + 1, 1);
                    }
                    else if (Year1 < Year2)
                    {
                        int Month1 = int.Parse(this.ddlRange1.SelectedValue.Substring(4, 2));
                        int Month2 = int.Parse(this.ddlRange2.SelectedValue.Substring(6, 2));
                        sDate = new DateTime(Year1, Month1, 1);
                        eDate = Month2 == 12 ? new DateTime(Year2 + 1, 1, 1) : new DateTime(Year2, Month2 + 1, 1);
                    }
                    else if (Year1 == Year2)
                    {
                        if (int.Parse(this.ddlRange1.SelectedValue.Substring(4, 2)) > int.Parse(this.ddlRange2.SelectedValue.Substring(4, 2)))
                        {
                            int Month1 = int.Parse(this.ddlRange2.SelectedValue.Substring(4, 2));
                            int Month2 = int.Parse(this.ddlRange1.SelectedValue.Substring(6, 2));
                            sDate = new DateTime(Year1, Month1, 1);
                            eDate = Month2 == 12 ? new DateTime(Year1 + 1, 1, 1) : new DateTime(Year1, Month2 + 1, 1);
                        }
                        else if (int.Parse(this.ddlRange1.SelectedValue.Substring(4, 2)) < int.Parse(this.ddlRange2.SelectedValue.Substring(4, 2)))
                        {
                            int Month1 = int.Parse(this.ddlRange1.SelectedValue.Substring(4, 2));
                            int Month2 = int.Parse(this.ddlRange2.SelectedValue.Substring(6, 2));
                            sDate = new DateTime(Year1, Month1, 1);
                            eDate = Month2 == 12 ? new DateTime(Year1 + 1, 1, 1) : new DateTime(Year1, Month2 + 1, 1);
                        }
                        else if (int.Parse(this.ddlRange1.SelectedValue.Substring(4, 2)) == int.Parse(this.ddlRange2.SelectedValue.Substring(4, 2)))
                        {
                            int Month1 = int.Parse(this.ddlRange1.SelectedValue.Substring(4, 2));
                            int Month2 = int.Parse(this.ddlRange2.SelectedValue.Substring(6, 2));
                            sDate = new DateTime(Year1, Month1, 1);
                            eDate = Month2 == 12 ? new DateTime(Year1 + 1, 1, 1) : new DateTime(Year1, Month2 + 1, 1);
                        }
                    }

                    data = data.Where(d => d.InvoiceDate.Value.Date >= sDate.Date && d.InvoiceDate.Value.Date < eDate.Date);


                    if (this.rdbDonate.Checked == true)
                    {
                        data = data.Where(d => d.DonateMark.Equals("1"));
                    }

                    var getData = from da in data.OrderByDescending(d => d.InvoiceDate).ToList()
                                  select new dataType
                                  {
                                      id = da.InvoiceID,
                                      range = (da.Year - 1911) + "年 " + da.MonthFrom + "-" + da.MonthTo + "月",
                                      TrackCode = da.TrackCode,
                                      No = da.No,
                                      CompanyName = da.CompanyName,
                                      Addr = da.Addr,
                                      CarrierType = da.CarrierType,
                                      Donation = da.Donation,
                                      DonateMark=da.DonateMark
                                  };

                    if (getData.Count() > 0)
                    {
                        if (this.gvEntity.AllowPaging)
                        {
                            int count = getData.Count();
                            this.lblError.Visible = false;
                            this.gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(this.gvEntity, 0);
                            this.gvEntity.DataSource = getData.ToList();
                            this.gvEntity.DataBind();

                            PagingControl paging = (PagingControl)this.gvEntity.BottomPagerRow.Cells[0].FindControl("pagingIndex");
                            paging.RecordCount = count;
                            paging.CurrentPageIndex = this.gvEntity.PageIndex;
                        }
                        else
                        {
                            this.gvEntity.DataSource = getData.ToList();
                            this.gvEntity.DataBind();
                        }
                        this.PrintingButton21.Visible = true;
                    }
                    else
                    {
                        this.lblError.Text = "查無資料!!";
                        this.lblError.Visible = true;
                        this.PrintingButton21.Visible = false;
                        this.gvEntity.DataSource = null;
                        this.gvEntity.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    this.lblError.Text = "系統錯誤:" + ex.Message;
                }
            }
        }
        #endregion

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.StartsWith("S:"))
            {
                this.PNewInvalidInvoicePreview1.setDetail = eventArgument.Substring(2).Trim();
                this.PNewInvalidInvoicePreview1.Popup.Show();
            }
        }
    }
}