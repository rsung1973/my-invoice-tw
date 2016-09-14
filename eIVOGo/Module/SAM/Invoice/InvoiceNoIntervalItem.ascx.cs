using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;

using eIVOGo.Module.Base;
using Uxnet.Web.Module.DataModel;
using Model.DataEntity;
using Model.Locale;
using Uxnet.Web.WebUI;
using eIVOGo.Helper;
using Model.Security.MembershipManagement;
using Business.Helper;

namespace eIVOGo.Module.SAM.Invoice
{
    public partial class InvoiceNoIntervalItem : EditEntityItemBase<EIVOEntityDataContext, InvoiceNoInterval>
    {
        private UserProfileMember _userProfile;
        private int _startNo;
        private int _endNo;
        private int _trackID;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.QueryExpr = m => m.IntervalID == (int?)modelItem.DataItem;
        }

        protected override bool saveEntity()
        {
            var mgr = dsEntity.CreateDataManager();
            loadEntity();

            if (!checkInput())
                return false;

            if (_entity == null)
            {
                var table = mgr.GetTable<InvoiceTrackCodeAssignment>();
                var codeAssignment = table.Where(t => t.SellerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID && t.TrackID == _trackID).FirstOrDefault();
                if (codeAssignment == null)
                {
                    codeAssignment = new InvoiceTrackCodeAssignment
                    {
                        SellerID = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID,
                        TrackID = _trackID
                    };

                    table.InsertOnSubmit(codeAssignment);
                }

                _entity = new InvoiceNoInterval { };
                codeAssignment.InvoiceNoIntervals.Add(_entity);
            }
            else if (_entity.InvoiceNoAssignments.Count > 0)
            {
                this.AjaxAlert("該區間之號碼已經被使用,不可修改!!");
                return false;
            }

            _entity.StartNo = _startNo;
            _entity.EndNo = _endNo;

            mgr.SubmitChanges();
            modelItem.DataItem = _entity.IntervalID;

            return true;
        }

        private bool checkInput()
        {
            if (_entity == null)
            {
                if (String.IsNullOrEmpty(TrackCode.SelectedValue))
                {
                    this.AjaxAlert("未設定字軌!!");
                    return false;
                }
                _trackID = int.Parse(TrackCode.SelectedValue);
            }

            if (StartNo.Text.Trim().Length != 8 || EndNo.Text.Trim().Length != 8 || !int.TryParse(StartNo.Text, out _startNo) || !int.TryParse(EndNo.Text, out _endNo))
            {
                this.AjaxAlert("起、迄號碼非8位整數!!");
                return false;
            }

            if (_endNo <= _startNo || ((_endNo-_startNo+1)%50!=0))
            {
                this.AjaxAlert("不符號碼大小順序與差距為50之倍數原則!!");
                return false;
            }

            var mgr = dsEntity.CreateDataManager();
            if (_entity != null)
            {
                if (mgr.EntityList.Any(t => t.IntervalID != _entity.IntervalID && t.TrackID == _entity.TrackID && t.StartNo >= _endNo && t.InvoiceNoAssignments.Count > 0
                    && t.SellerID == _entity.SellerID))
                {
                    this.AjaxAlert("違反序時序號原則該區段無法修改!!");
                    return false;
                }

                if (mgr.EntityList.Any(t => t.IntervalID != _entity.IntervalID && t.TrackID == _entity.TrackID
                    && ((t.EndNo <= _endNo && t.EndNo >= _startNo) || (t.StartNo <= _endNo && t.StartNo >= _startNo) || (t.StartNo <= _startNo && t.EndNo >= _startNo) || (t.StartNo <= _endNo && t.EndNo >= _endNo))))
                {
                    this.AjaxAlert("系統中已存在重疊的區段!!");
                    return false;
                }

            }
            else
            {
                if (mgr.EntityList.Any(t => t.TrackID == _trackID && t.StartNo >= _endNo && t.InvoiceNoAssignments.Count > 0
                    && t.SellerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID))
                {
                    this.AjaxAlert("違反序時序號原則該區段無法新增!!");
                    return false;
                }

                if (mgr.EntityList.Any(t => t.TrackID == _trackID
                    && ((t.EndNo <= _endNo && t.EndNo >= _startNo) || (t.StartNo <= _endNo && t.StartNo >= _startNo) || (t.StartNo <= _startNo && t.EndNo >= _startNo) || (t.StartNo <= _endNo && t.EndNo >= _endNo))))
                {
                    this.AjaxAlert("系統中已存在重疊的區段!!");
                    return false;
                }

            }

            return true;
        }
    }
}