using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer.basis;
using Model.Locale;

namespace Model.DataEntity
{
    public static partial class ExtensionMethods
    {
        public static InvoiceUserCarrierType GetDefaultUserCarrierType<TEntity>(this  GenericManager<EIVOEntityDataContext, TEntity> mgr, String typeName)
            where TEntity : class,new()
        {
            return mgr.GetTable<InvoiceUserCarrierType>().Where(t => t.CarrierType == typeName).FirstOrDefault();
        }

        public static int? GetMemberCodeID<TEntity>(this  GenericManager<EIVOEntityDataContext, TEntity> mgr, String hashID)
            where TEntity : class,new()
        {
            var item = mgr.GetTable<MemberCode>().Where(m => m.HashID == hashID).FirstOrDefault();
            return item != null ? item.CodeID : (int?)null;
        }

        public static String GetMaskInvoiceNo(this InvoiceItem item)
        {
            return String.Format("{0}{1}", item.TrackCode, item.DonateMark == "0" ? item.No : item.No.Substring(0, 5) + "***");
        }

        public static string WinningTypeTransform(this String typeValue)
        {
            switch(typeValue)
            {
                case "0":
                    return "特獎";
                case "1":
                    return "頭獎";
                case "2":
                    return "二獎";
                case "3":
                    return "三獎";
                case "4":
                    return "四獎";
                case "5":
                    return "五獎";
                case "6":
                    return "六獎";
                default:
                    return typeValue;
            }
        }

        public static void ResetDocumentDispatch(this CDS_Document item, Naming.DocumentTypeDefinition docType)
        {
            if (!item.DocumentDispatches.Any(d => d.TypeID == (int)docType))
            {
                item.DocumentDispatches.Add(new DocumentDispatch
                {
                    TypeID = (int)docType
                });
            }
        }

        public static bool IsB2C(this InvoiceBuyer buyer)
        {
            return buyer.ReceiptNo == "0000000000";
        }

        public static bool IsB2C(this InvoiceAllowanceBuyer buyer)
        {
            return buyer.ReceiptNo == "0000000000";
        }


        public static bool MoveToNextStep(this CDS_Document item, GenericManager<EIVOEntityDataContext> mgr)
        {
            var flowStep = item.DocumentFlowStep;
            if (flowStep != null)
            {
                var currentStep = mgr.GetTable<DocumentFlowControl>().Where(f => f.StepID == flowStep.CurrentFlowStep).First();
                if (currentStep.NextStep.HasValue)
                {
                    var nextStep = currentStep.NextStepItem;
                    flowStep.CurrentFlowStep = nextStep.StepID;
                    item.CurrentStep = nextStep.LevelID;

                    mgr.GetTable<DocumentProcessLog>().InsertOnSubmit(new DocumentProcessLog
                    {
                        DocID = flowStep.DocID,
                        StepDate = DateTime.Now,
                        FlowStep = nextStep.LevelID
                    });

                    mgr.SubmitChanges();
                    return true;
                }
            }
            return false;
        }

        public static bool MoveToNextStep(this DocumentAccessaryFlow item, GenericManager<EIVOEntityDataContext> mgr)
        {

            var currentStep = mgr.GetTable<DocumentFlowControl>().Where(f => f.StepID == item.CurrentFlowStep).First();
            var table = mgr.GetTable<DocumentAccessaryFlow>();

            if (currentStep.NextStep.HasValue)
            {
                if (!table.Any(a => a.DocID == item.DocID && a.CurrentFlowStep == currentStep.NextStep))
                {
                    table.DeleteOnSubmit(item);

                    mgr.GetTable<DocumentAccessaryFlow>().InsertOnSubmit(new DocumentAccessaryFlow
                    {
                        DocID = item.DocID,
                        CurrentFlowStep = currentStep.NextStep.Value
                    });

                    mgr.SubmitChanges();
                    return true;
                }
            }
            else
            {
                table.DeleteOnSubmit(item);
                mgr.SubmitChanges();
                return true;
            }
            return false;
        }

        public static bool MoveToFirstBranchStep(this CDS_Document item, GenericManager<EIVOEntityDataContext> mgr)
        {
            return item.MoveToBranchStep(mgr, 0);
        }

        public static bool MoveToSecondBranchStep(this CDS_Document item, GenericManager<EIVOEntityDataContext> mgr,bool isConcurrentFlow)
        {
            var flowStep = item.DocumentFlowStep;
            if (flowStep != null)
            {
                var currentStep = mgr.GetTable<DocumentFlowControl>().Where(f => f.StepID == flowStep.CurrentFlowStep).First();
                if (currentStep.BranchFlow.Count > 1)
                {
                    var branchStep = currentStep.BranchFlow.Select(b => b.BranchStepItem).OrderBy(b => b.FlowID).Skip(1).First();
                    if (isConcurrentFlow)
                    {
                        if (!item.DocumentAccessaryFlow.Any(a => a.CurrentFlowStep == branchStep.StepID))
                        {
                            item.DocumentAccessaryFlow.Add(new DocumentAccessaryFlow
                            {
                                CurrentFlowStep = branchStep.StepID
                            });
                        }
                    }
                    else
                    {
                        flowStep.CurrentFlowStep = branchStep.StepID;
                        item.CurrentStep = branchStep.LevelID;

                        mgr.GetTable<DocumentProcessLog>().InsertOnSubmit(new DocumentProcessLog
                        {
                            DocID = flowStep.DocID,
                            StepDate = DateTime.Now,
                            FlowStep = branchStep.LevelID
                        });
                    }

                    mgr.SubmitChanges();
                    return true;
                }
            }
            return false;
        }

        public static bool MoveToSecondBranchStep(this CDS_Document item, GenericManager<EIVOEntityDataContext> mgr)
        {
            return item.MoveToBranchStep(mgr, 1);
        }

        public static bool MoveToThirdBranchStep(this CDS_Document item, GenericManager<EIVOEntityDataContext> mgr, bool isConcurrentFlow)
        {
            var flowStep = item.DocumentFlowStep;
            if (flowStep != null)
            {
                var currentStep = mgr.GetTable<DocumentFlowControl>().Where(f => f.StepID == flowStep.CurrentFlowStep).First();
                if (currentStep.BranchFlow.Count > 2)
                {
                    var branchStep = currentStep.BranchFlow.Select(b => b.BranchStepItem).OrderBy(b => b.FlowID).Skip(2).First();
                    if (isConcurrentFlow)
                    {
                        if (!item.DocumentAccessaryFlow.Any(a => a.CurrentFlowStep == branchStep.StepID))
                        {
                            item.DocumentAccessaryFlow.Add(new DocumentAccessaryFlow
                            {
                                CurrentFlowStep = branchStep.StepID
                            });
                        }
                    }
                    else
                    {
                        flowStep.CurrentFlowStep = branchStep.StepID;
                        item.CurrentStep = branchStep.LevelID;

                        mgr.GetTable<DocumentProcessLog>().InsertOnSubmit(new DocumentProcessLog
                        {
                            DocID = flowStep.DocID,
                            StepDate = DateTime.Now,
                            FlowStep = branchStep.LevelID
                        });
                    }

                    mgr.SubmitChanges();
                    return true;
                }
            }
            return false;
        }

        public static bool MoveToThirdBranchStep(this CDS_Document item, GenericManager<EIVOEntityDataContext> mgr)
        {
            return item.MoveToBranchStep(mgr, 2);
        }

        public static bool MoveToForthBranchStep(this CDS_Document item, GenericManager<EIVOEntityDataContext> mgr)
        {
            return item.MoveToBranchStep(mgr, 3);
        }

        public static bool MoveToFifthBranchStep(this CDS_Document item, GenericManager<EIVOEntityDataContext> mgr)
        {
            return item.MoveToBranchStep(mgr, 4);
        }

        public static bool MoveToBranchStep(this CDS_Document item, GenericManager<EIVOEntityDataContext> mgr, int branchOrder)
        {
            var flowStep = item.DocumentFlowStep;
            if (flowStep != null)
            {
                var currentStep = mgr.GetTable<DocumentFlowControl>().Where(f => f.StepID == flowStep.CurrentFlowStep).First();
                if (currentStep.BranchFlow.Count > branchOrder)
                {
                    var branchStep = currentStep.BranchFlow.Select(b => b.BranchStepItem).OrderBy(b => b.FlowID).Skip(branchOrder).First();

                    flowStep.CurrentFlowStep = branchStep.StepID;
                    item.CurrentStep = branchStep.LevelID;

                    mgr.GetTable<DocumentProcessLog>().InsertOnSubmit(new DocumentProcessLog
                    {
                        DocID = flowStep.DocID,
                        StepDate = DateTime.Now,
                        FlowStep = branchStep.LevelID
                    });

                    mgr.SubmitChanges();
                    return true;
                }
            }
            return false;
        }


        public static Naming.InvoiceCenterBusinessType? CheckBusinessType(this InvoiceItem item)
        {
            return item.CDS_Document.DocumentOwner != null
                ? item.CDS_Document.DocumentOwner.OwnerID == item.SellerID
                    ? Naming.InvoiceCenterBusinessType.銷項
                    : item.CDS_Document.DocumentOwner.OwnerID == item.InvoiceBuyer.BuyerID
                    ? Naming.InvoiceCenterBusinessType.進項 : (Naming.InvoiceCenterBusinessType?)null
                : (Naming.InvoiceCenterBusinessType?)null;
        }

        public static Naming.InvoiceCenterBusinessType? CheckBusinessType(this InvoiceAllowance item)
        {
            return item.CDS_Document.DocumentOwner != null
                ? item.CDS_Document.DocumentOwner.OwnerID == item.InvoiceAllowanceSeller.SellerID
                    ? Naming.InvoiceCenterBusinessType.銷項
                    : item.CDS_Document.DocumentOwner.OwnerID == item.InvoiceAllowanceBuyer.BuyerID
                    ? Naming.InvoiceCenterBusinessType.進項 : (Naming.InvoiceCenterBusinessType?)null
                : (Naming.InvoiceCenterBusinessType?)null;
        }

        public static bool IsEnterpriseGroupMember(this Organization org)
        {
            return org.EnterpriseGroupMember.Count > 0;
        }

        public static Organization GetOrganizationByThumbprint(this GenericManager<EIVOEntityDataContext> mgr, String thumbprint)
        {
            Organization item = mgr.GetTable<Organization>().Where(t => t.OrganizationToken.Thumbprint == thumbprint).FirstOrDefault();
            if (item == null)
            {
                item = mgr.GetTable<Organization>().Where(t => t.OrganizationStatus.UserToken.Thumbprint == thumbprint).FirstOrDefault();
            }
            return item;
        }

        public static IQueryable<Organization> GetQueryByAgent(this GenericManager<EIVOEntityDataContext> mgr, int agentID)
        {
            return mgr.GetTable<Organization>().Where(o => o.CompanyID == agentID
                || mgr.GetTable<InvoiceIssuerAgent>()
                    .Where(a => a.AgentID == agentID)
                    .Select(a => a.IssuerID).Contains(o.CompanyID));
        }

        public static IQueryable<InvoiceItem> GetInvoiceByAgent(this EIVOEntityDataContext mgr, int agentID)
        {
            return mgr.GetInvoiceByAgent(mgr.GetTable<InvoiceItem>(),agentID);
        }

        public static IQueryable<InvoiceItem> GetInvoiceByAgent(this EIVOEntityDataContext mgr,IQueryable<InvoiceItem> items, int agentID)
        {
            return items.Where(i => i.SellerID == agentID
                    || mgr.GetTable<InvoiceIssuerAgent>()
                        .Where(a => a.AgentID == agentID)
                        .Select(a => a.IssuerID)
                        .Contains(i.SellerID.Value));
        }


        public static IQueryable<InvoiceAllowance> GetAllowanceByAgent(this EIVOEntityDataContext mgr, int agentID)
        {
            return mgr.GetAllowanceByAgent(mgr.GetTable<InvoiceAllowance>(),agentID);
        }

        public static IQueryable<InvoiceAllowance> GetAllowanceByAgent(this EIVOEntityDataContext mgr, IQueryable<InvoiceAllowance> items,int agentID)
        {
            return items.Where(i => i.InvoiceAllowanceSeller.SellerID == agentID
                    || mgr.GetTable<InvoiceIssuerAgent>()
                        .Where(a => a.AgentID == agentID)
                        .Select(a => a.IssuerID)
                        .Contains(i.InvoiceAllowanceSeller.SellerID.Value));
        }




    }

    public partial class EIVOEntityManager<TEntity> : GenericManager<EIVOEntityDataContext, TEntity>
        where TEntity : class,new()
    {
        public EIVOEntityManager() : base() { }
        public EIVOEntityManager(GenericManager<EIVOEntityDataContext> manager) : base(manager) { }

        protected virtual void applyProcessFlow(CDS_Document doc, int ownerID, Naming.B2BInvoiceDocumentTypeDefinition typeID, Naming.InvoiceCenterBusinessType businessID)
        {
            var flow = this.GetTable<DocumentTypeFlow>().Where(f => f.TypeID == (int)typeID
                && f.CompanyID == ownerID && f.BusinessID == (int)businessID).FirstOrDefault();

            if (flow != null && flow.DocumentFlow.InitialStep.HasValue)
            {
                var initialStep = flow.DocumentFlow.DocumentFlowControl;
                doc.CurrentStep = initialStep.LevelID;

                doc.DocumentFlowStep = new DocumentFlowStep
                {
                    CurrentFlowStep = initialStep.StepID
                };
            }
        }

        protected virtual void applyProcessFlow(CDS_Document doc, Naming.B2BInvoiceDocumentTypeDefinition typeID)
        {
            var flow = this.GetTable<CommonDocumentTypeFlow>().Where(f => f.TypeID == (int)typeID).FirstOrDefault();

            if (flow != null && flow.DocumentFlow.InitialStep.HasValue)
            {
                var initialStep = flow.DocumentFlow.DocumentFlowControl;
                doc.CurrentStep = initialStep.LevelID;

                doc.DocumentFlowStep = new DocumentFlowStep
                {
                    CurrentFlowStep = initialStep.StepID
                };
            }
        }

        public EIVOEntityDataContext Context
        {
            get
            {
                return (EIVOEntityDataContext)this._db;
            }
        }

        public List<TEntity> EventItems
        {
            get;
            protected set;
        }

    }

    public partial class InvoiceDataSource : LinqToSqlDataSource<EIVOEntityDataContext, InvoiceItem> { }
    public partial class UserProfileDataSource : LinqToSqlDataSource<EIVOEntityDataContext, UserProfile> { }
    public partial class OrganizationDataSource : LinqToSqlDataSource<EIVOEntityDataContext, Organization> { }
    public partial class InvoiceUserCarrierDataSource : LinqToSqlDataSource<EIVOEntityDataContext, InvoiceUserCarrier> { }
    public partial class ExceptionLogDataSource : LinqToSqlDataSource<EIVOEntityDataContext, ExceptionLog> { }
    public partial class AllowanceDataSource : LinqToSqlDataSource<EIVOEntityDataContext, InvoiceAllowance> { }
    public partial class InvoiceTrackCodeDataSource : LinqToSqlDataSource<EIVOEntityDataContext, InvoiceTrackCode> { }
    public partial class UserRoleDefinitionDataSource : LinqToSqlDataSource<EIVOEntityDataContext, UserRoleDefinition> { }
    public partial class CategoryDefinitionDataSource : LinqToSqlDataSource<EIVOEntityDataContext, CategoryDefinition> { }
    public partial class MenuControlDataSource : LinqToSqlDataSource<EIVOEntityDataContext, MenuControl> { }
    public partial class UserMenuDataSource : LinqToSqlDataSource<EIVOEntityDataContext, UserMenu> { }
    public partial class OrganizationCategoryDataSource : LinqToSqlDataSource<EIVOEntityDataContext, OrganizationCategory> { }
    public partial class UserRoleDataSource : LinqToSqlDataSource<EIVOEntityDataContext, UserRole> { }
    public partial class OrganizationCategoryUserRoleDataSource : LinqToSqlDataSource<EIVOEntityDataContext, OrganizationCategoryUserRole> { }
    public partial class DocumentDataSource : LinqToSqlDataSource<EIVOEntityDataContext, CDS_Document> { }
    public partial class EnterpriseGroupMemberDataSource : LinqToSqlDataSource<EIVOEntityDataContext, EnterpriseGroupMember> { }
    public partial class BusinessRelationshipDataSource : LinqToSqlDataSource<EIVOEntityDataContext, BusinessRelationship> { }
    public partial class EnterpriseGroupDataSource : LinqToSqlDataSource<EIVOEntityDataContext, EnterpriseGroup> { }
    public partial class InvoiceNoIntervalDataSource : LinqToSqlDataSource<EIVOEntityDataContext, InvoiceNoInterval> { }
    public partial class InvoiceProductItemDataSource : LinqToSqlDataSource<EIVOEntityDataContext, InvoiceProductItem> { }
    public partial class AllowanceDetailDataSource : LinqToSqlDataSource<EIVOEntityDataContext, InvoiceAllowanceDetail> { }
    public partial class OrganizationTokenDataSource : LinqToSqlDataSource<EIVOEntityDataContext, OrganizationToken> { }
    public partial class CALogDataSource : LinqToSqlDataSource<EIVOEntityDataContext, CALog> { }
    public partial class ReceiptDataSource : LinqToSqlDataSource<EIVOEntityDataContext, ReceiptItem> { }
    public partial class ReceiptDetailDataSource : LinqToSqlDataSource<EIVOEntityDataContext, ReceiptDetail> { }
    public partial class UserTokenDataSource : LinqToSqlDataSource<EIVOEntityDataContext, UserToken> { }
    public partial class InvoiceBuyerDataSource : LinqToSqlDataSource<EIVOEntityDataContext, InvoiceBuyer> { }
    public partial class UnassignedInvoiceNoDataSource : LinqToSqlDataSource<EIVOEntityDataContext, UnassignedInvoiceNo> { }
    public partial class SystemMessagesDataSource : LinqToSqlDataSource<EIVOEntityDataContext, SystemMessage> { }
    public partial class ProductItemCategoryDataSource : LinqToSqlDataSource<EIVOEntityDataContext, ProductItemCategory> { }
    public partial class BlankInvoiceNoSummaryDataSource : LinqToSqlDataSource<EIVOEntityDataContext, BlankInvoiceNoSummary> { }

    public partial class V_LogsDataSource : LinqToSqlDataSource<TurnKey2DataContext, V_Logs> { }

    public class UnassignedInvoiceNoSummary
    {
        public string CompanyName { get; set; }
        public string ReceiptNo { get; set; }
        public InvoiceTrackCode TrackCode { get; set; }
        public int StartNo { get; set; }
        public int EndNo { get; set; }
        // public OrganizationDepartment Department { get; set; }
        public UserProfile VirtualUser { get; set; }
        public int CompanyID { get; set; }
        public int SegmentID { get; set; }
        public int TrackID { get; set; }
    }
}
