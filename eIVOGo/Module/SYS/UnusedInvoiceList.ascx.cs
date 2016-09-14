using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using System.Linq.Expressions;
using Business.Helper;
using Uxnet.Web.Module.Common;
using eIVOGo.Module.Base;
using Model.Helper;
using System.Xml;
using Uxnet.Web.WebUI;
using System.Text;
using eIVOGo.services;
using Model.InvoiceManagement;
namespace eIVOGo.Module.SYS
{
    public partial class UnusedInvoiceList : InquireEntity
    {
        UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (_userProfile.CurrentUserRole.RoleID == ((int)Naming.RoleID.ROLE_SYS))
                this.divUpdate.Visible = true;
            else
                this.divUpdate.Visible = false;
            if (!this.IsPostBack)
            {
                initializeData();
            }

            if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_SYS)
            {
                SellerID.SelectorIndication = "請選擇...";
                SellerID.QueryExpr = o => o.OrganizationCategory.Any(c => c.CategoryID == (int)Naming.B2CCategoryID.Google台灣 
                    || c.CategoryID == (int)Naming.B2CCategoryID.店家 
                    || c.CategoryID == (int)Naming.B2CCategoryID.店家發票自動配號
                    || c.CategoryID == (int)Naming.B2CCategoryID.開立發票店家代理);
            }
            else
            {
                if (_userProfile.CurrentUserRole.OrganizationCategory.CategoryID != (int)Naming.B2CCategoryID.開立發票店家代理)
                {
                    //SellerID.SelectorIndication = "請選擇...";
                    SellerID.QueryExpr = o => o.CompanyID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID;

                }
                else
                    SellerID.QueryExpr =o =>true;
                if (!this.IsPostBack)
                        SellerID_SelectedIndexChanged(null, null);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(UnusedInvoiceList_PreRender);
            //this.Unload += new EventHandler(UnusedInvoiceList_Unload);

        }
        //void UnusedInvoiceList_Unload(object sender, EventArgs e)
        //{
        //    if (!_userProfile.CurrentUserRole.RoleID.Equals((int)Naming.RoleID.ROLE_SYS))
        //    {
        //        this.SellerID.Selector.SelectedValue = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID.ToString();
        //        this.SellerID.Selector.Enabled = false;
        //    }
        //}
    
        void UnusedInvoiceList_PreRender(object sender, EventArgs e)
        {
            this.btnDownload.Visible = this.itemList.Select().Count() > 0;
            this.btnDownloadCSV.Visible = this.itemList.Select().Count() > 0;

            this.SellerID.SelectedIndexChanged += new EventHandler(SellerID_SelectedIndexChanged);
        }

        protected void SellerID_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemList.QueryExpr = a => false;
            ResetQuery();
            this.btnDownload.Visible = false;
            this.btnDownloadCSV.Visible = false;
            this.SelectTrackCode.Items.Clear();
            var mgr = this.dsEntity.CreateDataManager();
            if (!String.IsNullOrEmpty(this.SellerID.SelectedValue))
            {
                
                this.SelectTrackCode.Items.AddRange(mgr.GetTable<InvoiceTrackCodeAssignment>().Where(t => t.SellerID.Equals(int.Parse(this.SellerID.SelectedValue)) & t.InvoiceTrackCode.Year.Equals(short.Parse(this.SelectYear.SelectedValue)) & t.InvoiceTrackCode.PeriodNo.Equals(short.Parse(this.SelectPeriod.SelectedValue))).Select(c => new ListItem(c.InvoiceTrackCode.TrackCode, c.TrackID.ToString())).ToArray());
            }
            else if(_userProfile.CurrentUserRole.RoleID != (int)Naming.RoleID.ROLE_SYS)
                this.SelectTrackCode.Items.AddRange(mgr.GetTable<InvoiceTrackCodeAssignment>().Where(t => t.SellerID.Equals(_userProfile.CurrentUserRole.OrganizationCategory.CompanyID) & t.InvoiceTrackCode.Year.Equals(short.Parse(this.SelectYear.SelectedValue)) & t.InvoiceTrackCode.PeriodNo.Equals(short.Parse(this.SelectPeriod.SelectedValue))).Select(c => new ListItem(c.InvoiceTrackCode.TrackCode, c.TrackID.ToString())).ToArray());
            
                

            if (this.SelectTrackCode.Items.Count.Equals(0))
            {
                this.SelectTrackCode.Items.Add(new ListItem("無", ""));
            }
        }

        protected override UserControl _itemList
        {
            get { return itemList; }
        }

        private void initializeData()
        {
            for (int i = 0; i < 6; i++)
            {
                SelectYear.Items.Add(new ListItem(String.Format("{0:000} 年", DateTime.Now.AddYears(-i).Year - 1911), DateTime.Now.AddYears(-i).Year.ToString()));
            }

            for (int j = 1; j <= 6; j++)
            {
                SelectPeriod.Items.Add(new ListItem(String.Format("{0:00} ~ {1:00} 月", 2 * j - 1, 2 * j), j.ToString()));
            }
            SelectPeriod.SelectedValue = ((DateTime.Now.Month % 2).Equals(0) ? DateTime.Now.AddMonths(-2).Month / 2 : DateTime.Now.AddMonths(-1).Month / 2).ToString();

            this.SelectTrackCode.Items.Add(new ListItem("無"));
        }

        protected override void buildQueryItem()
        {
            Expression<Func<UnassignedInvoiceNo, bool>> queryExpr = i => i.InvoiceTrackCodeAssignment.InvoiceTrackCode.Year.Equals(short.Parse(this.SelectYear.SelectedValue)) & i.InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo.Equals(short.Parse(this.SelectPeriod.SelectedValue));
            if (!String.IsNullOrEmpty(this.SellerID.SelectedValue))
            {
                queryExpr = queryExpr.And(u => u.SellerID.Equals(int.Parse(this.SellerID.SelectedValue)));
            }
            else
            {
                this.AjaxAlert("請選擇發票開立人!!");
                itemList.QueryExpr = a => false;
                ResetQuery();
                return;
            }

            if (!String.IsNullOrEmpty(this.SelectTrackCode.SelectedValue))
            {
                queryExpr = queryExpr.And(u => u.TrackID.Equals(int.Parse(this.SelectTrackCode.SelectedValue)));
            }
            else
            {
                this.AjaxAlert("發票開立人未設定字軌!!");
                itemList.QueryExpr = a => false;
                ResetQuery();
                return;
            }
            if (UpdateBlankInvoice.Checked)
                ServiceWorkItem.doUnassignNOCheck(this.SellerID.SelectedValue,this.SelectTrackCode.SelectedValue);
            itemList.BuildQuery = table =>
            {
                return table.Where(queryExpr);
            };

            base.buildQueryItem();
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            buildQueryItem();
            Response.Clear();
            Response.ContentType = "text/xml";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + String.Format("{0}_PreInvoice.xml", DateTime.Now.ToString("yyyyMMddHHmmssf")));

            var mgr = dsEntity.CreateDataManager();
            InvoiceTrackCodeAssignment item = this.itemList.Select().FirstOrDefault().InvoiceTrackCodeAssignment;
            XmlDocument data = item.BuildE0402().ConvertToXml();

            Response.Write(data.InnerXml);
            Response.Flush();
            Response.End();
        }

        protected void btnDownloadCSV_Click(object sender, EventArgs e)
        {
            CreateCSV();
        }

        private void CreateCSV()
        {
            buildQueryItem();
            StringBuilder sb = new StringBuilder();            
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + String.Format("{0}_PreInvoice.csv", DateTime.Now.ToString("yyyyMMddHHmmssf")));
            Response.ContentEncoding = Encoding.GetEncoding("UTF-8");

            var mgr = dsEntity.CreateDataManager();
            InvoiceTrackCodeAssignment item = this.itemList.Select().FirstOrDefault().InvoiceTrackCodeAssignment;
            int i = 1;
            foreach (var detail in item.UnassignedInvoiceNo)
            {
                sb.Append(String.Format("{0:00000}", i));
                sb.Append(",");
                sb.Append(item.Organization.ReceiptNo);
                sb.Append(",");
                sb.Append(String.Format("{0:000}{1:00}", item.InvoiceTrackCode.Year - 1911, item.InvoiceTrackCode.PeriodNo * 2));
                sb.Append(",");
                sb.Append(item.InvoiceTrackCode.TrackCode);
                sb.Append(",");
                sb.Append(String.Format("{0:00000000}", detail.InvoiceBeginNo));
                sb.Append(",");
                sb.Append(String.Format("{0:00000000}", detail.InvoiceEndNo));
                sb.Append(",");
                sb.Append(string.Format("{0:0#}", dsEntity.CreateDataManager().GetTable<Organization>().Where(o => o.CompanyID == item.Organization.CompanyID).FirstOrDefault().OrganizationStatus.SettingInvoiceType));
                //sb.Append(String.Format("{0:00}", 5));
                sb.Append("\n");
                i++;
            }

            Response.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        protected void btnSettle_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(SelectYear.SelectedValue))
            {
                Page.AjaxAlert("請選擇年度!!");
                return;
            }
            if (String.IsNullOrEmpty(SelectPeriod.SelectedValue))
            {
                Page.AjaxAlert("請選擇期別!!");
                return;
            }

            TrackNoIntervalManager mgr = new TrackNoIntervalManager(dsEntity.CreateDataManager());
            var items = mgr.GetTable<InvoiceTrackCode>().Where(t => t.Year == short.Parse(SelectYear.SelectedValue)
                && t.PeriodNo == short.Parse(SelectPeriod.SelectedValue))
                .Join(mgr.GetTable<InvoiceTrackCodeAssignment>(), t => t.TrackID, a => a.TrackID, (t, a) => a);
            foreach(var item in items)
            {
                mgr.SettleUnassignedInvoiceNO(item.SellerID, item.TrackID);
            }
            Page.AjaxAlert("空白發票整理完成!!");
        }
    }
}