using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Locale;
using Model.Resource;
using Model.Schema.EIVO;
using Utility;

namespace Model.InvoiceManagement.Validator
{
    public partial class TrackNoIntervalValidator
    {

        protected GenericManager<EIVOEntityDataContext> _mgr;
        protected OrganizationToken _owner;

        protected BranchTrackMain _branchMain;

        protected InvoiceNoInterval _newItem;
        protected Organization _seller;

        public TrackNoIntervalValidator(GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner)
        {
            _mgr = mgr;
            _owner = owner;
        }

        public InvoiceNoInterval DataItem
        {
            get
            {
                return _newItem;
            }
        }


        public virtual Exception Validate(BranchTrackMain dataItem)
        {
            _branchMain = dataItem;

            Exception ex;

            _seller = null;
            _newItem = null;

            if ((ex = checkBusiness()) != null)
            {
                return ex;
            }

            if ((ex = checkInterval()) != null)
            {
                return ex;
            }

            return null;
        }

        protected virtual Exception checkBusiness()
        {
            _seller = _mgr.GetTable<Organization>().Where(o => o.ReceiptNo == _branchMain.SellerId).FirstOrDefault();
            if (_seller == null)
            {
                return new Exception(String.Format(MessageResources.AlertInvalidSeller, _branchMain.SellerId));
            }

            if (_seller.CompanyID != _owner.CompanyID && !_mgr.GetTable<InvoiceIssuerAgent>().Any(a=>a.AgentID==_owner.CompanyID && a.IssuerID==_seller.CompanyID))
            {
                //return new Exception(String.Format(MessageResources.AlertSellerSignature, _invItem.SellerId));
                return new Exception(String.Format(MessageResources.InvalidSellerOrAgent, _branchMain.SellerId, _owner.Organization.ReceiptNo));
            }

            return null;
        }


        protected virtual Exception checkInterval()
        {
            int periodNo;

            if (!int.TryParse(_branchMain.PeriodNo, out periodNo) || periodNo > 6 || periodNo < 1)
            {
                return new Exception("期別錯誤!!");
            }

            var track = _mgr.GetTable<InvoiceTrackCode>().Where(t => t.PeriodNo == periodNo
                && t.Year == _branchMain.Year && t.TrackCode == _branchMain.TrackCode).FirstOrDefault();

            if (track == null)
            {
                return new Exception("未設定字軌!!");
            }

            String sStartNo = _branchMain.InvoiceBeginNo.GetEfficientString();
            String sEndNo = _branchMain.InvoiceEndNo.GetEfficientString();
            int startNo, endNo;

            if (sStartNo == null || sStartNo.Length != 8 || sEndNo == null || sEndNo.Length != 8
                || !int.TryParse(sStartNo, out startNo) || !int.TryParse(sEndNo, out endNo))
            {
                return new Exception("起、迄號碼非8位整數!!");
            }

            if (endNo <= startNo || ((endNo - startNo + 1) % 50 != 0))
            {
                return new Exception("不符號碼大小順序與差距為50之倍數原則!!");
            }

            if (_mgr.GetTable<InvoiceNoInterval>().Any(t => t.TrackID == track.TrackID && t.StartNo >= endNo && t.InvoiceNoAssignments.Count > 0
                && t.SellerID == _seller.CompanyID))
            {
                return new Exception("違反序時序號原則該區段無法新增!!");
            }

            if (_mgr.GetTable<InvoiceNoInterval>().Any(t => t.TrackID == track.TrackID
                && ((t.EndNo <= endNo && t.EndNo >= startNo) || (t.StartNo <= endNo && t.StartNo >= startNo) || (t.StartNo <= startNo && t.EndNo >= startNo) || (t.StartNo <= endNo && t.EndNo >= endNo))))
            {
                return new Exception("系統中已存在重疊的區段!!");
            }
            _newItem = new InvoiceNoInterval
            {
                StartNo = startNo,
                EndNo = endNo
            };

            var table = _mgr.GetTable<InvoiceTrackCodeAssignment>();
            var codeAssignment = table.Where(t => t.SellerID == _seller.CompanyID && t.TrackID == track.TrackID).FirstOrDefault();
            if (codeAssignment == null)
            {
                codeAssignment = new InvoiceTrackCodeAssignment
                {
                    SellerID = _seller.CompanyID,
                    TrackID = track.TrackID
                };

                _newItem.InvoiceTrackCodeAssignment = codeAssignment;
            }
            else
            {
                _newItem.TrackID = codeAssignment.TrackID;
                _newItem.SellerID = codeAssignment.SellerID;
            }

            return null;

        }

    }
}
