using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading;
using System.Xml;

using DataAccessLayer.basis;
using Model.DataEntity;
using Model.DocumentManagement;
using Model.Helper;
using Model.Locale;
using Model.Properties;
using Utility;
using Uxnet.Com.Security.UseCrypto;
using System.Data.Linq;

namespace Model.InvoiceManagement
{
    public class EIVOPlatformManager
    {
        public EIVOPlatformManager()
        {
        }

        public void TransmitInvoice()
        {
            using (InvoiceManager mgr = new InvoiceManager())
            {
                saveToPlatform(mgr, Naming.B2BInvoiceStepDefinition.待傳送, null);
            }
        }

        public void BatchTransmitInvoice(Func<Table<CDS_Document>, IQueryable<CDS_Document>> buildQuery)
        {
            using (InvoiceManager mgr = new InvoiceManager())
            {
                saveToPlatform(mgr, Naming.B2BInvoiceStepDefinition.待批次傳送,buildQuery);
            }
        }

        class _key
        {
            public int? a = (int?)null;
            public int? b = (int?)null;
        };

        public void NotifyToProcess()
        {
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var notify = mgr.GetTable<ReplicationNotification>();
                var items = notify.ToList();

                if (items.Count() > 0)
                {
                    notifyOwner(mgr, notify);
                    notifyCounterpart(mgr, notify);

                    foreach (var i in items)
                    {
                        mgr.DeleteAny<DocumentReplication>(d => d.DocID == i.DocID && d.TypeID == i.TypeID);
                    }
                }
            }
        }

        public void AlertForNotReceived()
        {
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var items = mgr.GetTable<CDS_Document>().Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待接收);

                if (items.Count() > 0)
                {
                    alertOwner(mgr, items);
                    alertCounterpart(mgr, items);
                    EIVOPlatformFactory.AlertSysAdminForNotReceivedItem(mgr, null);
                }
            }
        }

        public void AlertOwnerForNotReceivedThenTransmit()
        {
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var items = mgr.GetTable<CDS_Document>().Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待接收
                    && !d.DocumentAccessaryFlow.Any(f => f.DocumentFlowControl.LevelID == (int)Naming.B2BInvoiceStepDefinition.未接收資料待傳輸));

                if (items.Count() > 0)
                {
                    alertOwner(mgr, items);
                    EIVOPlatformFactory.AlertSysAdminForNotReceivedItem(mgr, null);
                    foreach (var item in items)
                    {
                        item.MoveToThirdBranchStep(mgr, true);
                    }
                }
            }
        }

        private void alertOwner(InvoiceManager mgr, IEnumerable<CDS_Document> items)
        {
            var org = mgr.GetTable<Organization>();

            var toNotify = items.Select(d => d.DocumentOwner.OwnerID)
                    .Distinct();

            foreach (var businessID in toNotify)
            {
                var item = org.Where(o => o.CompanyID == businessID).FirstOrDefault();
                if (item != null)
                {
                    EIVOPlatformFactory.AlertOwnerForNotReceivedItem(this, new EventArgs<Organization> { Argument = item });
                }
            }
        }

        private void alertCounterpart(InvoiceManager mgr, IEnumerable<CDS_Document> items)
        {
            var org = mgr.GetTable<Organization>();

            var toNotify = items.Where(t => t.DocType==(int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 &&  t.DocumentOwner.OwnerID == t.InvoiceItem.InvoiceSeller.SellerID).Select(t => t.InvoiceItem.InvoiceBuyer.BuyerID)
                    .Concat(items.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.SellerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.BuyerID))
                    .Concat(items.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 && t.DocumentOwner.OwnerID == t.InvoiceAllowance.InvoiceAllowanceSeller.SellerID).Select(t => t.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID))
                    .Concat(items.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.SellerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID))
                    .Concat(items.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 && t.DocumentOwner.OwnerID == t.InvoiceItem.InvoiceBuyer.BuyerID).Select(t => t.InvoiceItem.InvoiceSeller.SellerID))
                    .Concat(items.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.BuyerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.SellerID))
                    .Concat(items.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 && t.DocumentOwner.OwnerID == t.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID).Select(t => t.InvoiceAllowance.InvoiceAllowanceSeller.SellerID))
                    .Concat(items.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.SellerID))
                    .Distinct();

            foreach (var businessID in toNotify)
            {
                var item = org.Where(o => o.CompanyID == businessID).FirstOrDefault();
                if (item != null && (item.OrganizationStatus == null || item.OrganizationStatus.Entrusting != true))
                {
                    EIVOPlatformFactory.AlertCounterpartForNotReceivedItem(this, new EventArgs<Organization> { Argument = item });
                }
            }
        }


        private void notifyOwner(InvoiceManager mgr, System.Data.Linq.Table<ReplicationNotification> notify)
        {
            var org = mgr.GetTable<Organization>();

            var toIssue = notify.Select(r => r.DocumentReplication.CDS_Document).Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待開立);

            if (toIssue.Count() > 0)
            {
                var notifyToIssue = toIssue.Select(d => d.DocumentOwner.OwnerID)
                    .Distinct();

                foreach (var businessID in notifyToIssue)
                {
                    var item = org.Where(o => o.CompanyID == businessID).FirstOrDefault();
                    if (item != null && (item.OrganizationStatus == null || item.OrganizationStatus.Entrusting != true))
                    {
                        EIVOPlatformFactory.NotifyToIssueItem(this, new EventArgs<Organization> { Argument = item });
                    }
                }
            }

            //var toReceive = notify.Select(r => r.DocumentReplication.CDS_Document).Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待接收);
            //if (toReceive.Count() > 0)
            //{

            //    var notifyToReceive = toReceive.Select(d => d.DocumentOwner.OwnerID)
            //        .Distinct();

            //    foreach (var businessID in notifyToReceive)
            //    {
            //        var item = org.Where(o => o.CompanyID == businessID).FirstOrDefault();
            //        //if (item != null && (item.OrganizationStatus == null || item.OrganizationStatus.Entrusting != true))
            //        if (item != null)
            //        {
            //            EIVOPlatformFactory.NotifyToReceiveItem(this, new EventArgs<Organization> { Argument = item });
            //        }
            //    }
            //}

            var toDelete = notify.Select(r => r.DocumentReplication.CDS_Document).Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.申請退回);
            if (toDelete.Count() > 0)
            {

                var notifyToDelete = toDelete.Select(d => d.DocumentOwner.OwnerID)
                    .Distinct();

                foreach (var businessID in notifyToDelete)
                {
                    var item = org.Where(o => o.CompanyID == businessID).FirstOrDefault();
                    //if (item != null && (item.OrganizationStatus == null || item.OrganizationStatus.Entrusting != true))
                    if (item != null)
                    {
                        EIVOPlatformFactory.NotifyToRevokeItem(this, new EventArgs<Organization> { Argument = item });
                    }
                }
            }

            //var hasDeleted = notify.Select(r => r.DocumentReplication.CDS_Document).Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.已註銷);
            //if (hasDeleted.Count() > 0)
            //{

            //    var notifyDeleted = hasDeleted.Select(d => d.DocumentOwner.OwnerID)
            //        .Distinct();

            //    foreach (var businessID in notifyDeleted)
            //    {
            //        var item = org.Where(o => o.CompanyID == businessID).FirstOrDefault();
            //        if (item != null && (item.OrganizationStatus == null || item.OrganizationStatus.Entrusting != true))
            //        {
            //            EIVOPlatformFactory.AlertAboutRevocation(this, new EventArgs<Organization> { Argument = item });
            //        }
            //    }
            //}
        }

        private void notifyCounterpart(InvoiceManager mgr, System.Data.Linq.Table<ReplicationNotification> notify)
        {
            var org = mgr.GetTable<Organization>();

            //var toIssue = notify.Select(r => r.DocumentReplication.CDS_Document).Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待開立);

            //if (toIssue.Count() > 0)
            //{
            //    var notifyToIssue = toIssue.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 && t.DocumentOwner.OwnerID == t.InvoiceItem.InvoiceSeller.SellerID).Select(t => t.InvoiceItem.InvoiceBuyer.BuyerID)
            //        .Concat(toIssue.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.SellerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.BuyerID))
            //        .Concat(toIssue.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 && t.DocumentOwner.OwnerID == t.InvoiceAllowance.InvoiceAllowanceSeller.SellerID).Select(t => t.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID))
            //        .Concat(toIssue.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.SellerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID))
            //        .Concat(toIssue.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 && t.DocumentOwner.OwnerID == t.InvoiceItem.InvoiceBuyer.BuyerID).Select(t => t.InvoiceItem.InvoiceSeller.SellerID))
            //        .Concat(toIssue.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.BuyerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.SellerID))
            //        .Concat(toIssue.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 && t.DocumentOwner.OwnerID == t.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID).Select(t => t.InvoiceAllowance.InvoiceAllowanceSeller.SellerID))
            //        .Concat(toIssue.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.SellerID))
            //        .Distinct();

            //    foreach (var businessID in notifyToIssue)
            //    {
            //        var item = org.Where(o => o.CompanyID == businessID).FirstOrDefault();
            //        //if (item != null && (item.OrganizationStatus == null || item.OrganizationStatus.Entrusting != true))
            //        if (item != null)
            //        {
            //            EIVOPlatformFactory.NotifyToIssueItem(this, new EventArgs<Organization> { Argument = item });
            //        }
            //    }
            //}

            var toReceive = notify.Select(r => r.DocumentReplication.CDS_Document).Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待接收);
            if (toReceive.Count() > 0)
            {

                var notifyToReceive = toReceive.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 && t.DocumentOwner.OwnerID == t.InvoiceItem.InvoiceSeller.SellerID).Select(t => t.InvoiceItem.InvoiceBuyer.BuyerID)
                    .Concat(toReceive.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.SellerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.BuyerID))
                    .Concat(toReceive.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 && t.DocumentOwner.OwnerID == t.InvoiceAllowance.InvoiceAllowanceSeller.SellerID).Select(t => t.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID))
                    .Concat(toReceive.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.SellerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID))
                    .Concat(toReceive.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 && t.DocumentOwner.OwnerID == t.InvoiceItem.InvoiceBuyer.BuyerID).Select(t => t.InvoiceItem.InvoiceSeller.SellerID))
                    .Concat(toReceive.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.BuyerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.SellerID))
                    .Concat(toReceive.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 && t.DocumentOwner.OwnerID == t.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID).Select(t => t.InvoiceAllowance.InvoiceAllowanceSeller.SellerID))
                    .Concat(toReceive.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.SellerID))
                    .Distinct();

                foreach (var businessID in notifyToReceive)
                {
                    var item = org.Where(o => o.CompanyID == businessID).FirstOrDefault();
                    if (item != null && (item.OrganizationStatus == null || item.OrganizationStatus.Entrusting != true))
                    {
                        EIVOPlatformFactory.NotifyToReceiveItem(this, new EventArgs<Organization> { Argument = item });
                    }
                }
            }

            //var toDelete = notify.Select(r => r.DocumentReplication.CDS_Document).Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.申請退回);
            //if (toDelete.Count() > 0)
            //{

            //    var notifyToDelete = toDelete.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 && t.DocumentOwner.OwnerID == t.InvoiceItem.InvoiceSeller.SellerID).Select(t => t.InvoiceItem.InvoiceBuyer.BuyerID)
            //        .Concat(toDelete.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.SellerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.BuyerID))
            //        .Concat(toDelete.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 && t.DocumentOwner.OwnerID == t.InvoiceAllowance.InvoiceAllowanceSeller.SellerID).Select(t => t.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID))
            //        .Concat(toDelete.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.SellerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID))
            //        .Concat(toDelete.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 && t.DocumentOwner.OwnerID == t.InvoiceItem.InvoiceBuyer.BuyerID).Select(t => t.InvoiceItem.InvoiceSeller.SellerID))
            //        .Concat(toDelete.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.BuyerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.SellerID))
            //        .Concat(toDelete.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 && t.DocumentOwner.OwnerID == t.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID).Select(t => t.InvoiceAllowance.InvoiceAllowanceSeller.SellerID))
            //        .Concat(toDelete.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.SellerID))
            //        .Distinct();

            //    foreach (var businessID in notifyToDelete)
            //    {
            //        var item = org.Where(o => o.CompanyID == businessID).FirstOrDefault();
            //        //if (item != null && (item.OrganizationStatus == null || item.OrganizationStatus.Entrusting != true))
            //        if (item != null)
            //        {
            //            EIVOPlatformFactory.NotifyToRevokeItem(this, new EventArgs<Organization> { Argument = item });
            //        }
            //    }
            //}

            var hasDeleted = notify.Select(r => r.DocumentReplication.CDS_Document).Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.已註銷);
            if (hasDeleted.Count() > 0)
            {
                var notifyDeleted = hasDeleted.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 && t.DocumentOwner.OwnerID == t.InvoiceItem.InvoiceSeller.SellerID).Select(t => t.InvoiceItem.InvoiceBuyer.BuyerID)
                    .Concat(hasDeleted.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.SellerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.BuyerID))
                    .Concat(hasDeleted.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 && t.DocumentOwner.OwnerID == t.InvoiceAllowance.InvoiceAllowanceSeller.SellerID).Select(t => t.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID))
                    .Concat(hasDeleted.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.SellerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID))
                    .Concat(hasDeleted.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 && t.DocumentOwner.OwnerID == t.InvoiceItem.InvoiceBuyer.BuyerID).Select(t => t.InvoiceItem.InvoiceSeller.SellerID))
                    .Concat(hasDeleted.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.BuyerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.SellerID))
                    .Concat(hasDeleted.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 && t.DocumentOwner.OwnerID == t.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID).Select(t => t.InvoiceAllowance.InvoiceAllowanceSeller.SellerID))
                    .Concat(hasDeleted.Where(t => t.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && t.DocumentOwner.OwnerID == t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.BuyerID).Select(t => t.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.SellerID))
                    .Distinct();

                foreach (var businessID in notifyDeleted)
                {
                    var item = org.Where(o => o.CompanyID == businessID).FirstOrDefault();
                    if (item != null)
                    {
                        //EIVOPlatformFactory.NotifyToRevokeItem(this, new EventArgs<Organization> { Argument = item });
                        EIVOPlatformFactory.NotifyToCancelItem(this, new EventArgs<Organization> { Argument = item });
                    }
                }
            }
        }

        //public void NotifyCounterpartBusiness()
        //{
        //    using (InvoiceManager mgr = new InvoiceManager())
        //    {
        //        var notify = mgr.GetTable<ReplicationNotification>();
        //        var items = notify.ToList();

        //        var toIssue = mgr.GetTable<CDS_Document>().Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待開立);

        //        var notifyToIssue = toIssue
        //                .Join(mgr.EntityList, d => d.DocID, i => i.InvoiceID, (d, i) => i)
        //                .Join(notify, t => t.InvoiceID, s => s.DocID, (t, s) => new _key { a = t.InvoiceSeller.SellerID, b = t.InvoiceBuyer.BuyerID })
        //                .Join(mgr.GetTable<BusinessRelationship>(), k => k, r => new _key { a = r.RelativeID, b = r.MasterID }, (k, r) => k.a)
        //            .Concat(toIssue
        //                .Join(mgr.GetTable<DerivedDocument>(), t => t.DocID, d => d.DocID, (t, d) => d)
        //                .Join(mgr.EntityList, d => d.SourceID, i => i.InvoiceID, (d, i) => i)
        //                .Join(notify, t => t.InvoiceID, s => s.DocID, (t, s) => new _key { a = t.InvoiceSeller.SellerID, b = t.InvoiceBuyer.BuyerID })
        //                .Join(mgr.GetTable<BusinessRelationship>(), k => k, r => new _key { a = r.RelativeID, b = r.MasterID }, (k, r) => k.a))
        //            .Concat(toIssue
        //                .Join(mgr.GetTable<InvoiceAllowance>(), d => d.DocID, i => i.AllowanceID, (d, i) => i)
        //                .Join(notify, t => t.AllowanceID, s => s.DocID, (t, s) => new _key { a = t.InvoiceAllowanceSeller.SellerID, b = t.InvoiceAllowanceBuyer.BuyerID })
        //                .Join(mgr.GetTable<BusinessRelationship>(), k => k, r => new _key { a = r.MasterID, b = r.RelativeID }, (k, r) => k.b))
        //            .Concat(toIssue
        //                .Join(mgr.GetTable<DerivedDocument>(), t => t.DocID, d => d.DocID, (t, d) => d)
        //                .Join(mgr.GetTable<InvoiceAllowance>(), d => d.SourceID, i => i.AllowanceID, (d, i) => i)
        //                .Join(notify, t => t.AllowanceID, s => s.DocID, (t, s) => new _key { a = t.InvoiceAllowanceSeller.SellerID, b = t.InvoiceAllowanceBuyer.BuyerID })
        //                .Join(mgr.GetTable<BusinessRelationship>(), k => k, r => new _key { a = r.MasterID, b = r.RelativeID }, (k, r) => k.b))
        //            .Concat(toIssue
        //                .Join(mgr.GetTable<ReceiptItem>(), d => d.DocID, i => i.ReceiptID, (d, i) => i)
        //                .Join(notify, t => t.ReceiptID, s => s.DocID, (t, s) => new _key { a = t.SellerID, b = t.BuyerID })
        //                .Join(mgr.GetTable<BusinessRelationship>(), k => k, r => new _key { a = r.RelativeID, b = r.MasterID }, (k, r) => k.a))
        //            .Concat(toIssue
        //                .Join(mgr.GetTable<DerivedDocument>(), t => t.DocID, d => d.DocID, (t, d) => d)
        //                .Join(mgr.GetTable<ReceiptItem>(), d => d.SourceID, i => i.ReceiptID, (d, i) => i)
        //                .Join(notify, t => t.ReceiptID, s => s.DocID, (t, s) => new _key { a = t.SellerID, b = t.BuyerID })
        //                .Join(mgr.GetTable<BusinessRelationship>(), k => k, r => new _key { a = r.RelativeID, b = r.MasterID }, (k, r) => k.a))
        //            .Distinct();

        //        var toReceive = mgr.GetTable<CDS_Document>().Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待接收);

        //        var notifyToReceive = toReceive
        //                .Join(mgr.EntityList, d => d.DocID, i => i.InvoiceID, (d, i) => i)
        //                .Join(notify, t => t.InvoiceID, s => s.DocID, (t, s) => new _key { a = t.InvoiceSeller.SellerID, b = t.InvoiceBuyer.BuyerID })
        //                .Join(mgr.GetTable<BusinessRelationship>(), k => k, r => new _key { a = r.MasterID, b = r.RelativeID }, (k, r) => k.b)
        //            .Concat(toReceive
        //                .Join(mgr.GetTable<DerivedDocument>(), t => t.DocID, d => d.DocID, (t, d) => d)
        //                .Join(mgr.EntityList, d => d.SourceID, i => i.InvoiceID, (d, i) => i)
        //                .Join(notify, t => t.InvoiceID, s => s.DocID, (t, s) => new _key { a = t.InvoiceSeller.SellerID, b = t.InvoiceBuyer.BuyerID })
        //                .Join(mgr.GetTable<BusinessRelationship>(), k => k, r => new _key { a = r.MasterID, b = r.RelativeID }, (k, r) => k.b))
        //            .Concat(toReceive
        //                .Join(mgr.GetTable<InvoiceAllowance>(), d => d.DocID, i => i.AllowanceID, (d, i) => i)
        //                .Join(notify, t => t.AllowanceID, s => s.DocID, (t, s) => new _key { a = t.InvoiceAllowanceSeller.SellerID, b = t.InvoiceAllowanceBuyer.BuyerID })
        //                .Join(mgr.GetTable<BusinessRelationship>(), k => k, r => new _key { a = r.RelativeID, b = r.MasterID }, (k, r) => k.a))
        //            .Concat(toReceive
        //                .Join(mgr.GetTable<DerivedDocument>(), t => t.DocID, d => d.DocID, (t, d) => d)
        //                .Join(mgr.GetTable<InvoiceAllowance>(), d => d.SourceID, i => i.AllowanceID, (d, i) => i)
        //                .Join(notify, t => t.AllowanceID, s => s.DocID, (t, s) => new _key { a = t.InvoiceAllowanceSeller.SellerID, b = t.InvoiceAllowanceBuyer.BuyerID })
        //                .Join(mgr.GetTable<BusinessRelationship>(), k => k, r => new _key { a = r.RelativeID, b = r.MasterID }, (k, r) => k.a))
        //            .Concat(toReceive
        //                .Join(mgr.GetTable<ReceiptItem>(), d => d.DocID, i => i.ReceiptID, (d, i) => i)
        //                .Join(notify, t => t.ReceiptID, s => s.DocID, (t, s) => new _key { a = t.SellerID, b = t.BuyerID })
        //                .Join(mgr.GetTable<BusinessRelationship>(), k => k, r => new _key { a = r.MasterID, b = r.RelativeID }, (k, r) => k.b))
        //            .Concat(toReceive
        //                .Join(mgr.GetTable<DerivedDocument>(), t => t.DocID, d => d.DocID, (t, d) => d)
        //                .Join(mgr.GetTable<ReceiptItem>(), d => d.SourceID, i => i.ReceiptID, (d, i) => i)
        //                .Join(notify, t => t.ReceiptID, s => s.DocID, (t, s) => new _key { a = t.SellerID, b = t.BuyerID })
        //                .Join(mgr.GetTable<BusinessRelationship>(), k => k, r => new _key { a = r.MasterID, b = r.RelativeID }, (k, r) => k.b))
        //            .Distinct();


        //        var org = mgr.GetTable<Organization>();

        //        foreach (var businessID in notifyToIssue)
        //        {
        //            var item = org.Where(o => o.CompanyID == businessID).FirstOrDefault();
        //            if (item != null && (item.OrganizationStatus == null || item.OrganizationStatus.Entrusting != true))
        //            {
        //                EIVOPlatformFactory.NotifyToIssueItem(this, new EventArgs<Organization> { Argument = item });
        //            }
        //        }

        //        foreach (var businessID in notifyToReceive)
        //        {
        //            var item = org.Where(o => o.CompanyID == businessID).FirstOrDefault();
        //            if (item != null && (item.OrganizationStatus == null || item.OrganizationStatus.Entrusting != true))
        //            {
        //                EIVOPlatformFactory.NotifyToReceiveItem(this, new EventArgs<Organization> { Argument = item });
        //            }
        //        }

        //        mgr.ExecuteCommand("delete dbo.DocumentReplication");
        //        mgr.ExecuteCommand("delete dbo.DocumentDispatch");
        //        notify.DeleteAllOnSubmit(items);
        //        mgr.SubmitChanges();

        //    }

        //}

        private void saveToPlatform(InvoiceManager mgr,Naming.B2BInvoiceStepDefinition transmitWhenStatus,Func<Table<CDS_Document>, IQueryable<CDS_Document>> buildQuery)
        {
            Settings.Default.A1401Outbound.CheckStoredPath();
            Settings.Default.A0401Outbound.CheckStoredPath();
            int invoiceCounter = Directory.GetFiles(Settings.Default.A1401Outbound).Length;
            Settings.Default.B0401Outbound.CheckStoredPath();
            Settings.Default.B1401Outbound.CheckStoredPath();
            int allowanceCounter = Directory.GetFiles(Settings.Default.B1401Outbound).Length;
            Settings.Default.A0501Outbound.CheckStoredPath();
            int cancellationCounter = Directory.GetFiles(Settings.Default.A0501Outbound).Length;
            Settings.Default.B0501Outbound.CheckStoredPath();
            int allowanceCancellationCounter = Directory.GetFiles(Settings.Default.B0501Outbound).Length;

            var table = mgr.GetTable<CDS_Document>();
            var items = buildQuery != null
                ? buildQuery(table).Where(d => d.CurrentStep == (int)transmitWhenStatus)
                : table.Where(d => d.CurrentStep == (int)transmitWhenStatus);            

            if (items.Count() > 0)
            {
                String fileName;
                foreach (var item in items)
                {
                    try
                    {
                        switch ((Naming.DocumentTypeDefinition)item.DocType.Value)
                        {
                            case Naming.DocumentTypeDefinition.E_Invoice:
                                if (item.InvoiceItem.InvoiceSeller.Organization.OrganizationStatus != null && item.InvoiceItem.InvoiceSeller.Organization.OrganizationStatus.IronSteelIndustry == true)
                                {
                                    fileName = Path.Combine(Settings.Default.A1401Outbound, String.Format("A1401-{0:yyyyMMddHHmmssf}-{1:00000}.xml", DateTime.Now, invoiceCounter++));
                                    item.InvoiceItem.CreateA1401().ConvertToXml().Save(fileName);
                                }
                                else
                                {
                                    fileName = Path.Combine(Settings.Default.A0401Outbound, String.Format("A0401-{0:yyyyMMddHHmmssf}-{1:00000}.xml", DateTime.Now, invoiceCounter++));
                                    item.InvoiceItem.CreateA0401().ConvertToXml().Save(fileName);
                                }
                                break;
                            case Naming.DocumentTypeDefinition.E_Allowance:
                                if (item.InvoiceAllowance.InvoiceAllowanceSeller.Organization.OrganizationStatus != null && item.InvoiceAllowance.InvoiceAllowanceSeller.Organization.OrganizationStatus.IronSteelIndustry == true)
                                {
                                    fileName = Path.Combine(Settings.Default.B1401Outbound, String.Format("B1401-{0:yyyyMMddHHmmssf}-{1:00000}.xml", DateTime.Now, allowanceCounter++));
                                    item.InvoiceAllowance.CreateB1401().ConvertToXml().Save(fileName);
                                }
                                else
                                {
                                    fileName = Path.Combine(Settings.Default.B0401Outbound, String.Format("B0401-{0:yyyyMMddHHmmssf}-{1:00000}.xml", DateTime.Now, allowanceCounter++));
                                    item.InvoiceAllowance.CreateB0401().ConvertToXml().Save(fileName);
                                }
                                break;
                            case Naming.DocumentTypeDefinition.E_InvoiceCancellation:
                                fileName = Path.Combine(Settings.Default.A0501Outbound, String.Format("A0501-{0:yyyyMMddHHmmssf}-{1:00000}.xml", DateTime.Now, cancellationCounter++));
                                item.DerivedDocument.ParentDocument.InvoiceItem.CreateA0501().ConvertToXml().Save(fileName);
                                break;
                            case Naming.DocumentTypeDefinition.E_AllowanceCancellation:
                                fileName = Path.Combine(Settings.Default.B0501Outbound, String.Format("B0501-{0:yyyyMMddHHmmssf}-{1:00000}.xml", DateTime.Now, allowanceCancellationCounter++));
                                item.DerivedDocument.ParentDocument.InvoiceAllowance.CreateB0501().ConvertToXml().Save(fileName);
                                break;
                        }
                        transmit(mgr, item);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
            }
        }

        public void CommissionedToReceive()
        {
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var items = mgr.GetTable<CDS_Document>().Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待接收);

                if (items.Count() > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    Naming.InvoiceCenterBusinessType? itemType;

                    foreach (var item in items)
                    {
                        try
                        {
                            bool bSigned = false;
                            switch ((Naming.B2BInvoiceDocumentTypeDefinition)item.DocType.Value)
                            {
                                case Naming.B2BInvoiceDocumentTypeDefinition.電子發票:
                                    itemType = item.InvoiceItem.CheckBusinessType();
                                    if (itemType == Naming.InvoiceCenterBusinessType.銷項)
                                    {
                                        //賣方開立發票,買方接收
                                        if (item.InvoiceItem.InvoiceBuyer.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.InvoiceItem.InvoiceBuyer.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.InvoiceItem.InvoiceBuyer.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.InvoiceItem.SignAndCheckToReceiveInvoiceItem(cert, sb, itemType.Value);
                                                    if (bSigned)
                                                    {
                                                        item.MoveToSecondBranchStep(mgr,true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.InvoiceItem.SignAndCheckToReceiveInvoiceItem(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    else if (itemType == Naming.InvoiceCenterBusinessType.進項)
                                    {
                                        //買方開立發票,賣方接收
                                        if (item.InvoiceItem != null)
                                        {
                                            if (item.InvoiceItem.InvoiceSeller.Organization.OrganizationStatus.Entrusting == true)
                                            {
                                                sb.Clear();
                                                if (item.InvoiceItem.InvoiceSeller.Organization.IsEnterpriseGroupMember())
                                                {
                                                    var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.InvoiceItem.InvoiceSeller.Organization);
                                                    if (cert != null)
                                                    {
                                                        bSigned = item.InvoiceItem.SignAndCheckToReceiveInvoiceItem(cert, sb, itemType.Value);
                                                        if (bSigned)
                                                        {
                                                            item.MoveToSecondBranchStep(mgr,true);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    bSigned = item.InvoiceItem.SignAndCheckToReceiveInvoiceItem(null, sb, itemType.Value);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case Naming.B2BInvoiceDocumentTypeDefinition.發票折讓:
                                    itemType = item.InvoiceAllowance.CheckBusinessType();
                                    if (itemType == Naming.InvoiceCenterBusinessType.進項)
                                    {
                                        //買方開立發票,賣方接收
                                        if (item.InvoiceAllowance.InvoiceAllowanceSeller.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.InvoiceAllowance.InvoiceAllowanceSeller.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.InvoiceAllowance.InvoiceAllowanceSeller.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.InvoiceAllowance.SignAndCheckToReceiveInvoiceAllowance(cert, sb, itemType.Value);
                                                    if (bSigned)
                                                    {
                                                        item.MoveToSecondBranchStep(mgr,true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.InvoiceAllowance.SignAndCheckToReceiveInvoiceAllowance(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //賣方開立發票,買方接收
                                        if (item.InvoiceAllowance.InvoiceAllowanceBuyer.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.InvoiceAllowance.InvoiceAllowanceBuyer.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.InvoiceAllowance.InvoiceAllowanceBuyer.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.InvoiceAllowance.SignAndCheckToReceiveInvoiceAllowance(cert, sb, itemType.Value);
                                                    if (bSigned)
                                                    {
                                                        item.MoveToSecondBranchStep(mgr,true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.InvoiceAllowance.SignAndCheckToReceiveInvoiceAllowance(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    break;
                                case Naming.B2BInvoiceDocumentTypeDefinition.作廢發票:
                                    itemType = item.DerivedDocument.ParentDocument.InvoiceItem.CheckBusinessType();
                                    if (itemType == Naming.InvoiceCenterBusinessType.銷項)
                                    {
                                        if (item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.DerivedDocument.ParentDocument.InvoiceItem.SignAndCheckToReceiveInvoiceCancellation(cert, sb, item.DocID, itemType.Value);
                                                    if (bSigned)
                                                    {
                                                        item.MoveToSecondBranchStep(mgr, true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.DerivedDocument.ParentDocument.InvoiceItem.SignAndCheckToReceiveInvoiceCancellation(null, sb, item.DocID, itemType.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.DerivedDocument.ParentDocument.InvoiceItem.SignAndCheckToReceiveInvoiceCancellation(cert, sb, item.DocID, itemType.Value);
                                                    if (bSigned)
                                                    {
                                                        item.MoveToSecondBranchStep(mgr, true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.DerivedDocument.ParentDocument.InvoiceItem.SignAndCheckToReceiveInvoiceCancellation(null, sb, item.DocID, itemType.Value);
                                            }
                                        }
                                    }
                                    break;
                                case Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓:
                                    itemType = item.DerivedDocument.ParentDocument.InvoiceAllowance.CheckBusinessType();
                                    if (itemType == Naming.InvoiceCenterBusinessType.進項)
                                    {
                                        //買方開立發票,賣方接收

                                        if (item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.DerivedDocument.ParentDocument.InvoiceAllowance.SignAndCheckToReceiveAllowanceCancellation(cert, sb, item.DocID, itemType.Value);
                                                    if (bSigned)
                                                    {
                                                        item.MoveToSecondBranchStep(mgr, true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.DerivedDocument.ParentDocument.InvoiceAllowance.SignAndCheckToReceiveAllowanceCancellation(null, sb, item.DocID, itemType.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.DerivedDocument.ParentDocument.InvoiceAllowance.SignAndCheckToReceiveAllowanceCancellation(cert, sb, item.DocID, itemType.Value);
                                                    if (bSigned)
                                                    {
                                                        item.MoveToSecondBranchStep(mgr, true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.DerivedDocument.ParentDocument.InvoiceAllowance.SignAndCheckToReceiveAllowanceCancellation(null, sb, item.DocID, itemType.Value);
                                            }
                                        }
                                    }
                                    break;
                                case Naming.B2BInvoiceDocumentTypeDefinition.收據:
                                    if (item.ReceiptItem.Buyer.OrganizationStatus.Entrusting == true)
                                    {
                                        sb.Clear();
                                        if (item.ReceiptItem.Buyer.IsEnterpriseGroupMember())
                                        {
                                            var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.ReceiptItem.Buyer);
                                            if (cert != null)
                                            {
                                                bSigned = item.ReceiptItem.SignAndCheckToReceiveReceipt(cert, sb);
                                            }
                                        }
                                        else
                                        {
                                            bSigned = item.ReceiptItem.SignAndCheckToReceiveReceipt(null, sb);
                                        }
                                    }
                                    break;
                                case Naming.B2BInvoiceDocumentTypeDefinition.作廢收據:
                                    if (item.DerivedDocument.ParentDocument.ReceiptItem.Buyer.OrganizationStatus.Entrusting == true)
                                    {
                                        sb.Clear();
                                        if (item.DerivedDocument.ParentDocument.ReceiptItem.Buyer.IsEnterpriseGroupMember())
                                        {
                                            var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.ReceiptItem.Buyer);
                                            if (cert != null)
                                            {
                                                bSigned = item.DerivedDocument.ParentDocument.ReceiptItem.SignAndCheckToReceiveReceiptCancellation(cert, sb, item.DocID);
                                            }
                                        }
                                        else
                                        {
                                            bSigned = item.DerivedDocument.ParentDocument.ReceiptItem.SignAndCheckToReceiveReceiptCancellation(null, sb, item.DocID);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }

                            if (bSigned)
                            {
                                transmit(mgr, item);
                            }

                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }
                    }
                }
            }
        }

        public void CommissionedToIssue()
        {
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var items = mgr.GetTable<CDS_Document>().Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待開立
                    || d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.註銷申請待開立);

                if (items.Count() > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    Naming.InvoiceCenterBusinessType? itemType;

                    foreach (var item in items)
                    {
                        try
                        {
                            bool bSigned = false;
                            switch ((Naming.B2BInvoiceDocumentTypeDefinition)item.DocType.Value)
                            {
                                case Naming.B2BInvoiceDocumentTypeDefinition.電子發票:
                                    itemType = item.InvoiceItem.CheckBusinessType();
                                    if (itemType == Naming.InvoiceCenterBusinessType.銷項)
                                    {
                                        if (item.InvoiceItem.InvoiceSeller.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.InvoiceItem.InvoiceSeller.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.InvoiceItem.InvoiceSeller.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.InvoiceItem.SignAndCheckToIssueInvoiceItem(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.InvoiceItem.SignAndCheckToIssueInvoiceItem(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.InvoiceItem.InvoiceBuyer.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.InvoiceItem.InvoiceBuyer.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.InvoiceItem.InvoiceBuyer.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.InvoiceItem.SignAndCheckToIssueInvoiceItem(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.InvoiceItem.SignAndCheckToIssueInvoiceItem(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    break;
                                case Naming.B2BInvoiceDocumentTypeDefinition.發票折讓:
                                    itemType = item.InvoiceAllowance.CheckBusinessType();
                                    if (itemType == Naming.InvoiceCenterBusinessType.進項)
                                    {
                                        if (item.InvoiceAllowance.InvoiceAllowanceBuyer.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.InvoiceAllowance.InvoiceAllowanceBuyer.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.InvoiceAllowance.InvoiceAllowanceBuyer.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.InvoiceAllowance.SignAndCheckToIssueInvoiceAllowance(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.InvoiceAllowance.SignAndCheckToIssueInvoiceAllowance(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.InvoiceAllowance.InvoiceAllowanceSeller.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.InvoiceAllowance.InvoiceAllowanceSeller.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.InvoiceAllowance.InvoiceAllowanceSeller.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.InvoiceAllowance.SignAndCheckToIssueInvoiceAllowance(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.InvoiceAllowance.SignAndCheckToIssueInvoiceAllowance(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    break;
                                case Naming.B2BInvoiceDocumentTypeDefinition.作廢發票:
                                    itemType = item.DerivedDocument.ParentDocument.InvoiceItem.CheckBusinessType();
                                    if (itemType == Naming.InvoiceCenterBusinessType.銷項)
                                    {

                                        if (item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.SignAndCheckToIssueInvoiceCancellation(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.SignAndCheckToIssueInvoiceCancellation(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.SignAndCheckToIssueInvoiceCancellation(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.SignAndCheckToIssueInvoiceCancellation(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    break;
                                case Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓:
                                    itemType = item.DerivedDocument.ParentDocument.InvoiceAllowance.CheckBusinessType();
                                    if (itemType == Naming.InvoiceCenterBusinessType.進項)
                                    {

                                        if (item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.Organization);
                                                if (cert != null)
                                                {
                                                    //bSigned = item.DerivedDocument.ParentDocument.InvoiceAllowance.SignAndCheckToReceiveAllowanceCancellation(cert, sb, item.DocID);
                                                    bSigned = item.SignAndCheckToIssueAllowanceCancellation(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                //bSigned = item.DerivedDocument.ParentDocument.InvoiceAllowance.SignAndCheckToReceiveAllowanceCancellation(null, sb, item.DocID);
                                                bSigned = item.SignAndCheckToIssueAllowanceCancellation(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.Organization);
                                                if (cert != null)
                                                {
                                                    //bSigned = item.DerivedDocument.ParentDocument.InvoiceAllowance.SignAndCheckToReceiveAllowanceCancellation(cert, sb, item.DocID);
                                                    bSigned = item.SignAndCheckToIssueAllowanceCancellation(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                //bSigned = item.DerivedDocument.ParentDocument.InvoiceAllowance.SignAndCheckToReceiveAllowanceCancellation(null, sb, item.DocID);
                                                bSigned = item.SignAndCheckToIssueAllowanceCancellation(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    break;
                                case Naming.B2BInvoiceDocumentTypeDefinition.收據:
                                    if (item.ReceiptItem.Seller.OrganizationStatus.Entrusting == true)
                                    {
                                        sb.Clear();
                                        if (item.ReceiptItem.Seller.IsEnterpriseGroupMember())
                                        {
                                            var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.ReceiptItem.Seller);
                                            if (cert != null)
                                            {
                                                bSigned = item.ReceiptItem.SignAndCheckToIssueReceipt(cert, sb);
                                            }
                                        }
                                        else
                                        {
                                            bSigned = item.ReceiptItem.SignAndCheckToIssueReceipt(null, sb);
                                        }
                                    }
                                    break;
                                case Naming.B2BInvoiceDocumentTypeDefinition.作廢收據:
                                    if (item.DerivedDocument.ParentDocument.ReceiptItem.Seller.OrganizationStatus.Entrusting == true)
                                    {
                                        sb.Clear();
                                        if (item.DerivedDocument.ParentDocument.ReceiptItem.Seller.IsEnterpriseGroupMember())
                                        {
                                            var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.ReceiptItem.Seller);
                                            if (cert != null)
                                            {
                                                bSigned = item.DerivedDocument.ParentDocument.ReceiptItem.ReceiptCancellation.SignAndCheckToIssueReceiptCancellation(item.DerivedDocument.ParentDocument.ReceiptItem, cert, sb, item.DocID);
                                            }
                                        }
                                        else
                                        {
                                            bSigned = item.DerivedDocument.ParentDocument.ReceiptItem.ReceiptCancellation.SignAndCheckToIssueReceiptCancellation(item.DerivedDocument.ParentDocument.ReceiptItem, null, sb, item.DocID);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }

                            if (bSigned)
                            {
                                transmit(mgr, item);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }
                    }
                }
            }
        }

        public void CommissionedToIssueByCounterpart()
        {
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var items = mgr.GetTable<CDS_Document>().Where(d => d.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.退回申請待開立);

                if (items.Count() > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    Naming.InvoiceCenterBusinessType? itemType;

                    foreach (var item in items)
                    {
                        try
                        {
                            bool bSigned = false;
                            switch ((Naming.B2BInvoiceDocumentTypeDefinition)item.DocType.Value)
                            {
                                case Naming.B2BInvoiceDocumentTypeDefinition.電子發票:
                                    itemType = item.InvoiceItem.CheckBusinessType();
                                    if (itemType == Naming.InvoiceCenterBusinessType.銷項)
                                    {
                                        if (item.InvoiceItem.InvoiceBuyer.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.InvoiceItem.InvoiceBuyer.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.InvoiceItem.InvoiceBuyer.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.InvoiceItem.SignAndCheckInvoiceItemByCounterpart(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.InvoiceItem.SignAndCheckInvoiceItemByCounterpart(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.InvoiceItem.InvoiceSeller.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.InvoiceItem.InvoiceSeller.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.InvoiceItem.InvoiceSeller.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.InvoiceItem.SignAndCheckInvoiceItemByCounterpart(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.InvoiceItem.SignAndCheckInvoiceItemByCounterpart(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    break;
                                case Naming.B2BInvoiceDocumentTypeDefinition.發票折讓:
                                    itemType = item.InvoiceAllowance.CheckBusinessType();
                                    if (itemType == Naming.InvoiceCenterBusinessType.進項)
                                    {
                                        if (item.InvoiceAllowance.InvoiceAllowanceSeller.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.InvoiceAllowance.InvoiceAllowanceSeller.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.InvoiceAllowance.InvoiceAllowanceSeller.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.InvoiceAllowance.SignAndCheckInvoiceAllowanceByCounterpart(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.InvoiceAllowance.SignAndCheckInvoiceAllowanceByCounterpart(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.InvoiceAllowance.InvoiceAllowanceBuyer.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.InvoiceAllowance.InvoiceAllowanceBuyer.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.InvoiceAllowance.InvoiceAllowanceBuyer.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.InvoiceAllowance.SignAndCheckInvoiceAllowanceByCounterpart(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.InvoiceAllowance.SignAndCheckInvoiceAllowanceByCounterpart(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    break;
                                case Naming.B2BInvoiceDocumentTypeDefinition.作廢發票:
                                    itemType = item.DerivedDocument.ParentDocument.InvoiceItem.CheckBusinessType();
                                    if (itemType == Naming.InvoiceCenterBusinessType.銷項)
                                    {
                                        if (item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.SignAndCheckInvoiceCancellationByCounterpart(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.SignAndCheckInvoiceCancellationByCounterpart(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.SignAndCheckInvoiceCancellationByCounterpart(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.SignAndCheckInvoiceCancellationByCounterpart(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    break;
                                case Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓:
                                    itemType = item.DerivedDocument.ParentDocument.InvoiceAllowance.CheckBusinessType();
                                    if (itemType == Naming.InvoiceCenterBusinessType.進項)
                                    {
                                        if (item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.SignAndCheckAllowanceCancellationByCounterpart(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.SignAndCheckAllowanceCancellationByCounterpart(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.Organization.OrganizationStatus.Entrusting == true)
                                        {
                                            sb.Clear();
                                            if (item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.Organization.IsEnterpriseGroupMember())
                                            {
                                                var cert = (new B2BInvoiceManager(mgr)).PrepareSignerCertificate(item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.Organization);
                                                if (cert != null)
                                                {
                                                    bSigned = item.SignAndCheckAllowanceCancellationByCounterpart(cert, sb, itemType.Value);
                                                }
                                            }
                                            else
                                            {
                                                bSigned = item.SignAndCheckAllowanceCancellationByCounterpart(null, sb, itemType.Value);
                                            }
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }

                            if (bSigned)
                            {
                                transmit(mgr, item);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }
                    }
                }
            }
        }

        public void MatchDocumentAttachment()
        {
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var invoices = mgr.GetTable<InvoiceItem>();
                ((EIVOEntityDataContext)invoices.Context).MatchDocumentAttachment();
            }
        }

        private void transmit(GenericManager<EIVOEntityDataContext> mgr, CDS_Document item)
        {
            item.MoveToNextStep(mgr);
        }
    }
}
