﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using DataAccessLayer.basis;

namespace Model.InvoiceManagement
{
    public class TrackNoManager : EIVOEntityManager<InvoiceItem>
    {
        private InvoiceNoInterval _currentInterval;
        private int _currentNo;
        private DateTime _uploadInvoiceDate;
        private int _sellerID;

        private static List<int> __lockManager = new List<int>();

        public TrackNoManager(int sellerID)
            : this(null, sellerID)
        {

        }
        public TrackNoManager(GenericManager<EIVOEntityDataContext> mgr, int sellerID)
            : base(mgr)
        {
            _sellerID = sellerID;
            initialize();
        }


        public InvoiceNoInterval InvoiceNoInterval
        {
            get
            {
                return _currentInterval;
            }
        }

        private void initialize()
        {
            lock (__lockManager)
            {
                if (__lockManager.Contains(_sellerID))
                {
                    throw new Exception("發票配號程序已被佔用!!");
                }
                else
                {
                    __lockManager.Add(_sellerID);
                }
            }

            _uploadInvoiceDate = DateTime.Now;
            _currentInterval = getCurrentInterval();

        }

        protected override void dispose(bool disposing)
        {
            base.dispose(disposing);
            lock (__lockManager)
            {
                __lockManager.Remove(_sellerID);
            }
        }

        public bool CheckInvoiceNo(InvoiceItem item)
        {
            if (_currentInterval == null)
            {
                return false;
            }
            _currentNo = _currentInterval.StartNo + _currentInterval.InvoiceNoAssignments.Count;
            item.InvoiceNoAssignment = new InvoiceNoAssignment
            {
                InvoiceNoInterval = _currentInterval,
                InvoiceNo = _currentNo
            };

            item.TrackCode = _currentInterval.InvoiceTrackCodeAssignment.InvoiceTrackCode.TrackCode;
            item.No = String.Format("{0:00000000}", _currentNo);

            _currentNo++;
            if (_currentNo > _currentInterval.EndNo)
            {
                _currentInterval = getNextInterval(_currentInterval.IntervalID);
            }
            return true;
        }

        private InvoiceNoInterval getCurrentInterval()
        {
            int currentYear = _uploadInvoiceDate.Year;
            int currentPeriodNo = (_uploadInvoiceDate.Month + 1) / 2;
            var intervalItems = this.GetTable<InvoiceNoInterval>().Where(n => n.InvoiceTrackCodeAssignment.SellerID == _sellerID
                && n.InvoiceTrackCodeAssignment.InvoiceTrackCode.Year == currentYear
                && n.InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo == currentPeriodNo);
            return intervalItems.Where(n => n.InvoiceNoAssignments.Count == 0 || n.StartNo + n.InvoiceNoAssignments.Count < n.EndNo).OrderBy(n => n.StartNo).FirstOrDefault();
        }

        private InvoiceNoInterval getNextInterval(int intervalID)
        {
            int currentYear = _uploadInvoiceDate.Year;
            int currentPeriodNo = (_uploadInvoiceDate.Month + 1) / 2;
            var intervalItems = this.GetTable<InvoiceNoInterval>().Where(n => n.InvoiceTrackCodeAssignment.SellerID == _sellerID
                && n.InvoiceTrackCodeAssignment.InvoiceTrackCode.Year == currentYear
                && n.InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo == currentPeriodNo);
            return intervalItems.Where(n => (n.InvoiceNoAssignments.Count == 0 || n.StartNo + n.InvoiceNoAssignments.Count < n.EndNo) && n.IntervalID > intervalID).OrderBy(n => n.StartNo).FirstOrDefault();
        }
    }
}
