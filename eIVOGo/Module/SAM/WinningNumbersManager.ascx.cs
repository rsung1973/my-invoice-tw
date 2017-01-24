using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;
using Model.Security.MembershipManagement;
using Model.InvoiceManagement;
using Model.Locale;
using Business.Helper;
using Uxnet.Web.Module.Common;
using Utility;
using System.Text;
using Uxnet.Web.WebUI;
using System.Collections;
using eIVOGo.Module.Common;
using eIVOGo.Properties;
using com.uxb2b.util;
using System.Threading;


namespace eIVOGo.Module.SAM
{
    public partial class WinningNumbersManager : System.Web.UI.UserControl, IPostBackEventHandler
    {
        UserProfileMember _userProfile;

        public class UniInvoiceNO
        {
            public string ID;
            public int year;
            public string period;
            public string SpecialPrize;
            public string GrandPrize;
            public string FirstPrize;
            public string AdditionalSixthPrize;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (!this.IsPostBack)
            {
                initializeData();
            }
        }

        private void initializeData()
        {
            this.ddlYear.Items.Clear();
            this.ddlRange.Items.Clear();
            int year = DateTime.Now.Year;

            for (int y = -5; y < 6; y++)
            {
                this.ddlYear.Items.Add(new ListItem(((year - 1911) + y).ToString() + "年", (year + y).ToString()));
            }

            for (int m = 1; m < 7; m += 1)
            {
                this.ddlRange.Items.Add(new ListItem((2 * m-1).ToString("00") + "-" + (2 * m).ToString("00") + " 月", m.ToString()));
            }
            this.ddlYear.Items.Insert(0, (new ListItem("全部", "all")));
            this.ddlRange.Items.Insert(0, (new ListItem("全部", "all")));
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.gvEntity.PreRender += new EventHandler(gvEntity_PreRender);
        }

        private void getData()
        {
            try
            {
                using (InvoiceManager im = new InvoiceManager())
                {
                    IQueryable<UniformInvoiceWinningNumber> items = im.GetTable<UniformInvoiceWinningNumber>();
                    if (this.ddlYear.SelectedValue != "all")
                    {
                        items = items.Where(d => d.Year == int.Parse(this.ddlYear.SelectedValue));
                    }

                    if (this.ddlRange.SelectedValue != "all")
                    {
                        items = items.Where(d => d.Period == int.Parse(this.ddlRange.SelectedValue));
                    }


                    var data = from d in items
                               group d by new { d.Year, d.Period } into g
                               select new UniInvoiceNO
                               {
                                   ID = g.Key.Year.ToString() + "-" + g.Key.Period.ToString(),
                                   year = g.Key.Year,
                                   period = ((g.Key.Period * 2) - 1).ToString() + "-" + (g.Key.Period * 2).ToString() + "月",
                                   SpecialPrize = string.Join(",", im.GetTable<UniformInvoiceWinningNumber>().Where(d => d.Year == g.Key.Year & d.Period == g.Key.Period & d.PrizeType == "特別獎").Select(d => d.WinningNO).ToArray()),
                                   GrandPrize = string.Join(",", im.GetTable<UniformInvoiceWinningNumber>().Where(d => d.Year == g.Key.Year & d.Period == g.Key.Period & d.PrizeType == "特獎").Select(d => d.WinningNO).ToArray()),
                                   FirstPrize = string.Join(",", im.GetTable<UniformInvoiceWinningNumber>().Where(d => d.Year == g.Key.Year & d.Period == g.Key.Period & d.PrizeType == "頭獎").Select(d => d.WinningNO).ToArray()),
                                   AdditionalSixthPrize = string.Join(",", im.GetTable<UniformInvoiceWinningNumber>().Where(d => d.Year == g.Key.Year & d.Period == g.Key.Period & d.PrizeType == "增開六獎").Select(d => d.WinningNO).ToArray())
                               };

                    if (data.Count() > 0)
                    {
                        int count = data.Count();
                        this.lblError.Visible = false;
                        this.gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(this.gvEntity, 0);
                        this.gvEntity.DataSource = data.ToList();
                        this.gvEntity.DataBind();

                        PagingControl paging = (PagingControl)this.gvEntity.BottomPagerRow.Cells[0].FindControl("pagingList");
                        paging.RecordCount = count;
                        paging.CurrentPageIndex = this.gvEntity.PageIndex;
                    }
                    else
                    {
                        this.lblError.Text = "查無資料!!";
                        this.lblError.Visible = true;
                        this.gvEntity.DataSource = null;
                        this.gvEntity.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        
        void gvEntity_PreRender(object sender, EventArgs e)
        {
            if (this.divResult.Visible == true)
            {
                getData();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditWinningNumber.aspx");
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.divResult.Visible = true;
            getData();
        }


        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.StartsWith("D:"))
            {
                doDelete(eventArgument.Substring(2));
            }
            else if (eventArgument.StartsWith("U:"))
            {
                doEdit(eventArgument.Substring(2));
            }
            else if (eventArgument.StartsWith("M:"))
            {
                string[] data = eventArgument.Substring(2).Split('-');
                int year = int.Parse(data[0].ToString());
                int period = int.Parse(data[1].ToString());

                int result = doMatchNumer(year,period);
                if (result > 0)
                {
                    SharedFunction. doSendMaild(new SharedFunction._MailQueryState { setYear = year, setPeriod = period });
                    SharedFunction.doSendSMSMessage(new SharedFunction._MailQueryState { setYear = year, setPeriod = period });
                }
                string month = period == 1 ? "1~2" : period == 2 ? "3~4" : period == 3 ? "5~6" : period == 4 ? "7~8" : period == 5 ? "9~10" : period == 6 ? "11~12" : "";
                this.AjaxAlert((year - 1911).ToString() + "年 " + month + "月 中獎發票共 " + result.ToString() + " 筆!!");
            }
        }

        private void doEdit(string id)
        {
            Page.Items["id"] = id;
            Server.Transfer("EditWinningNumber.aspx");
        }

        private void doDelete(string id)
        {
            try
            {
                using (InvoiceManager im = new InvoiceManager())
                {
                    string[] data = id.Split('-');
                    int year = int.Parse(data[0].ToString());
                    int period = int.Parse(data[1].ToString());
                    var oldNum = im.GetTable<UniformInvoiceWinningNumber>().Where(d => d.Year == year & d.Period == period);
                    im.GetTable<UniformInvoiceWinningNumber>().DeleteAllOnSubmit(oldNum);
                    im.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            getData();
        }

        private int doMatchNumer(int setYear, int setPeriod)
        {
            int year = setYear;
            int period = setPeriod;
            int smonth = (period * 2) - 1;
            int emonth = period * 2;
            int totalCount = 0;
            try
            {
                using (InvoiceManager mgr = new InvoiceManager())
                {
                    DateTime startDate = new DateTime(year, smonth, 1);
                    var allInvoiceNO = mgr.EntityList.Where(i => i.InvoiceDate >= startDate && i.InvoiceDate < startDate.AddMonths(2)
                        && i.InvoiceAmountType.TotalAmount > 0 && i.InvoiceCancellation == null && i.InvoiceBuyer.ReceiptNo == "0000000000");
                    var winNos = mgr.GetTable<UniformInvoiceWinningNumber>().Where(d => d.Year == year & d.Period == period).OrderBy(d => d.Rank);

                    var countWinNo = mgr.GetTable<InvoiceWinningNumber>().Where(n => n.UniformInvoiceWinningNumber.Year == year && n.UniformInvoiceWinningNumber.Period == period);
                    if (countWinNo.Count() > 0)
                    {
                        mgr.GetTable<InvoiceWinningNumber>().DeleteAllOnSubmit(countWinNo);
                    }

                    foreach (var ino in allInvoiceNO)
                    {
                        foreach (var d in winNos)
                        {
                            if (ino.No.EndsWith(d.WinningNO))
                            {
                                mgr.GetTable<InvoiceWinningNumber>().InsertOnSubmit(new InvoiceWinningNumber
                                {
                                    InvoiceID = ino.InvoiceID,
                                    WinningID=d.WinningID
                                });
                                totalCount += 1;
                                break;
                            }
                        }
                    }
                    mgr.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return totalCount;
        }        
        #endregion
    }
}