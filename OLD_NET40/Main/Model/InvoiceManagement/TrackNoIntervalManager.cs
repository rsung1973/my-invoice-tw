using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Helper;
using Model.InvoiceManagement.Validator;
using Model.InvoiceManagement.zhTW;
using Model.Locale;
using Model.Schema.EIVO;
using Utility;

namespace Model.InvoiceManagement
{
    public partial class TrackNoIntervalManager : EIVOEntityManager<InvoiceNoInterval>
    {
        public TrackNoIntervalManager() : base() { }
        public TrackNoIntervalManager(GenericManager<EIVOEntityDataContext> mgr) : base(mgr) { }

        public Dictionary<int, Exception> SaveUploadBranchTrackInterval(BranchTrack item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();

            if (item != null && item.Main != null && item.Main.Length > 0)
            {
                //Organization donatory = owner.Organization.InvoiceWelfareAgencies.Select(w => w.WelfareAgency.Organization).FirstOrDefault();
                EventItems = new List<InvoiceNoInterval>();
                TrackNoIntervalValidator validator = new TrackNoIntervalValidator(this, owner);
                for (int idx = 0; idx < item.Main.Length; idx++)
                {
                    try
                    {
                        var invItem = item.Main[idx];

                        Exception ex;
                        if ((ex = validator.Validate(invItem)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        var newItem = validator.DataItem;

                        this.EntityList.InsertOnSubmit(newItem);
                        this.SubmitChanges();

                        EventItems.Add(newItem);

                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        result.Add(idx, ex);
                    }
                }

            }

            return result;
        }

        public void SettleUnassignedInvoiceNO(int sellerID, int trackID)
        {
            var trackCodeItem = this.GetTable<InvoiceTrackCodeAssignment>().Where(t => t.SellerID == sellerID
                && t.TrackID == trackID).FirstOrDefault().InvoiceTrackCode;

            if (trackCodeItem == null)
                return;

            var noIntervals = this.EntityList.Where(i => i.TrackID == trackID && i.SellerID == sellerID);

            this.DeleteAll<UnassignedInvoiceNo>(u => u.SellerID == sellerID && u.TrackID == trackID);

            foreach (var interval in noIntervals)
            {
                String startNo = String.Format("{0:00000000}",interval.StartNo),
                    endNo = String.Format("{0:00000000}",interval.EndNo);
                var items = this.GetTable<InvoiceItem>().Where(i => i.SellerID == sellerID && i.TrackCode == trackCodeItem.TrackCode
                    && String.Compare(i.No, startNo) >= 0 && String.Compare(i.No, endNo) <= 0);

                int recordCount = interval.EndNo - interval.StartNo + 1;

                if (items.Count() == recordCount)
                    continue;

                List<int> allInvoiceNo = Enumerable.Range(interval.StartNo, recordCount).ToList();
                foreach(var item in items)
                {
                    allInvoiceNo.Remove(int.Parse(item.No));
                }

                int startIndex = allInvoiceNo[0];
                int count = 1;
                for (int idx = 1; idx < allInvoiceNo.Count; idx++)
                {
                    if ((startIndex + count) == allInvoiceNo[idx])
                    {
                        count++;
                        continue;
                    }
                    else
                    {
                        this.GetTable<UnassignedInvoiceNo>().InsertOnSubmit(
                            new UnassignedInvoiceNo
                            {
                                InvoiceBeginNo = startIndex,
                                InvoiceEndNo = startIndex + count - 1,
                                SellerID = sellerID,
                                TrackID = trackID
                            });

                        count = 1;
                        startIndex = allInvoiceNo[idx];
                    }
                }

                if(count>0)
                {
                    this.GetTable<UnassignedInvoiceNo>().InsertOnSubmit(
                        new UnassignedInvoiceNo
                        {
                            InvoiceBeginNo = startIndex,
                            InvoiceEndNo = startIndex + count - 1,
                            SellerID = sellerID,
                            TrackID = trackID
                        });
                }

                this.SubmitChanges();
            }
        }
    }
}
