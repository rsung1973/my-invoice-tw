using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Locale;
using Model.SCMDataEntity;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;
using eIVOGo.Properties;
using eIVOGo.Module.Base;

namespace eIVOGo.Module.SCM.View
{
    public partial class ReturnedPurchaseOrderPreview : SCMEntityPreview<PURCHASE_ORDER_RETURNED>
    {

        public override void PrepareDataFromDB(object keyValue)
        {
            int returnedSN = (int)keyValue;
            _item = dsEntity.CreateDataManager().EntityList.Where(s => s.PURCHASE_ORDER_RETURNED_SN == returnedSN).First();
            supplierView.QueryExpr = p => p.SUPPLIER_SN == _item.SUPPLIER_SN;
            POReturnDetais.Items = _item.PURCHASE_ORDER_RETURNED_DETAILS;
        }


        protected override void prepareDataForViewState()
        {
            _item.WAREHOUSE = dsEntity.CreateDataManager().GetTable<WAREHOUSE>().Where(b => b.WAREHOUSE_SN == _item.WAREHOUSE_SN).First();
            _item.SUPPLIER = dsEntity.CreateDataManager().GetTable<SUPPLIER>().Where(b => b.SUPPLIER_SN == _item.SUPPLIER_SN).First();
            POReturnDetais.Items = _item.PURCHASE_ORDER_RETURNED_DETAILS;
            supplierView.QueryExpr = p => p.SUPPLIER_SN == _item.SUPPLIER_SN;
        }

    }    
}
