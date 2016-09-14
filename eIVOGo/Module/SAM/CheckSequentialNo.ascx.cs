using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;

using Business.Helper;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;
using Utility;

namespace eIVOGo.Module.SAM
{
    public partial class CheckSequentialNo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gvEntity.DataSource = dataSource;
        }

        protected List<String> dataSource
        {
            get
            {
                return this.Cache[String.Format("{0}{1}",this.GetType().ToString(),Session.SessionID)] as List<String>;
            }
            set
            {
                Cache.Insert(String.Format("{0}{1}", this.GetType().ToString(), Session.SessionID), value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(MemberManager_PreRender);
        }


        protected void PageIndexChanged(object source, PageChangedEventArgs e)
        {
            gvEntity.PageIndex = e.NewPageIndex;
            gvEntity.DataBind();
        }

        void MemberManager_PreRender(object sender, EventArgs e)
        {
            if (gvEntity.DataSource != null)
            {
                gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(gvEntity, 0);
                gvEntity.DataBind();

                if (gvEntity.BottomPagerRow != null)
                {
                    PagingControl paging = (PagingControl)gvEntity.BottomPagerRow.Cells[0].FindControl("pagingList");
                    paging.RecordCount = dataSource.Count;
                    paging.CurrentPageIndex = gvEntity.PageIndex;
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            String startNo = StartNo.Text.Trim();
            String endNo = EndNo.Text.Trim();
            if (String.IsNullOrEmpty(startNo) || String.IsNullOrEmpty(endNo) || startNo.Length != 10 || endNo.Length != 10)
            {
                this.AjaxAlert("發票號碼格式不正確!!");
                return;
            }

            String trackCode = startNo.Substring(0, 2);
            if (!endNo.StartsWith(trackCode))
            {
                this.AjaxAlert("起始號碼與結尾號碼字軌不相同!!");
                return;
            }

            int start = int.Parse(startNo.Substring(2));
            int end = int.Parse(endNo.Substring(2));

            dataSource = new List<string>();
            var list = dataSource;

            var mgr = dsEntity.CreateDataManager();
            int s1 = Math.Min(start, end);
            int s2 = Math.Max(start, end);
            for (int idx = s1; idx <= s2; idx++)
            {
                String no = String.Format("{0:00000000}", idx);
                if (!mgr.EntityList.Any(i => i.TrackCode == trackCode && i.No == no))
                {
                    list.Add(trackCode + no);
                }
            }

            gvEntity.DataSource = dataSource;
        }

    }
}