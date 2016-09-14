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
using System.Linq.Expressions;
using Utility;

namespace eIVOGo.Module.EIVO.Item
{
    public partial class InvoiceUploadList : System.Web.UI.UserControl, IPostBackEventHandler
    {

        protected int? _totalRecordCount;
        protected UserProfileMember _userProfile;
        protected IGoogleUploadManager _mgr;
        protected bool _allowPaging = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(gvEntity, 0);
            initializeData();
        }

        protected virtual void initializeData()
        {
            _mgr = _userProfile["importMgr"] as IGoogleUploadManager;
        }

        public bool AllowPaging
        {
            get
            {
                return _allowPaging;
            }
            set
            {
                _allowPaging = value;
            }
        }

        public GridView DataGridView
        {
            get
            {
                return gvEntity;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.PreRender += new EventHandler(InvoiceUploadList_PreRender);
        }

        void InvoiceUploadList_PreRender(object sender, EventArgs e)
        {
            if (_mgr != null)
                bindData();

            if (_totalRecordCount.HasValue && _totalRecordCount.Value == 0)
            {
                lblError.Visible = true;
            }
        }

        protected virtual void bindData()
        {
            if (_allowPaging)
            {
                if (_mgr.IsValid)
                {
                    gvEntity.DataSource = ((GoogleInvoiceUploadManager)_mgr).ItemList.Skip(pagingList.CurrentPageIndex * pagingList.PageSize).Take(pagingList.PageSize);
                }
                else
                {
                    gvEntity.DataSource = ((GoogleInvoiceUploadManager)_mgr).ErrorList.Skip(pagingList.CurrentPageIndex * pagingList.PageSize).Take(pagingList.PageSize);
                }
                pagingList.Visible = true;
            }
            else
            {
                if (_mgr.IsValid)
                {
                    gvEntity.DataSource = ((GoogleInvoiceUploadManager)_mgr).ItemList;
                }
                else
                {
                    gvEntity.DataSource = ((GoogleInvoiceUploadManager)_mgr).ErrorList;
                }
                pagingList.Visible = false;
            }
            gvEntity.DataBind();
            _totalRecordCount = _mgr.IsValid ? ((GoogleInvoiceUploadManager)_mgr).ItemList.Count : ((GoogleInvoiceUploadManager)_mgr).ErrorList.Count;
            pagingList.RecordCount = _totalRecordCount.Value;
        }

        #region IPostBackEventHandler Members

        public virtual void RaisePostBackEvent(string eventArgument)
        {

        }

        #endregion

        
    }    
}