using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer.basis;

namespace Model.DocumentFlowManagement
{
    public static partial class ExtensionMethods
    {
    }

    public partial class FlowEntityManager<TEntity> : GenericManager<FlowEntityDataContext, TEntity>
        where TEntity : class,new()
    {
        public FlowEntityManager() : base() { }
        public FlowEntityManager(GenericManager<FlowEntityDataContext> manager) : base(manager) { }
    }

    public partial class DocumentFlowDataSource : LinqToSqlDataSource<FlowEntityDataContext, DocumentFlow> { }
    public partial class DocumentFlowControlDataSource : LinqToSqlDataSource<FlowEntityDataContext, DocumentFlowControl> { }
    public partial class DocumentTypeFlowDataSource : LinqToSqlDataSource<FlowEntityDataContext, DocumentTypeFlow> { }
    public partial class CommonDocumentTypeFlowDataSource : LinqToSqlDataSource<FlowEntityDataContext, CommonDocumentTypeFlow> { }
    public partial class DocumentFlowBranchDataSource : LinqToSqlDataSource<FlowEntityDataContext, DocumentFlowBranch> { }


}
