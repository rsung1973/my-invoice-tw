using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;

namespace eIVOGo.Helper
{
    public static class QueryExtensionMethods
    {
        public static DataSet GetDataSetResult<TEntity>(this ModelSource<TEntity> models)
            where TEntity : class,new()
        {
            return models.GetDataSetResult(models.Items);
        }

        public static ClosedXML.Excel.XLWorkbook GetExcelResult<TEntity>(this ModelSource<TEntity> models)
            where TEntity : class,new()
        {
            return models.GetExcelResult(models.Items);
        }

        public static DataSet GetDataSetResult<TEntity>(this ModelSource<TEntity> models,IQueryable items)
            where TEntity : class, new()
        {
            using (SqlCommand sqlCmd = (SqlCommand)models.GetCommand(items))
            {
                sqlCmd.Connection = (SqlConnection)models.GetDataContext().Connection;
                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd))
                {
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    return ds;
                }
            }
        }

        public static ClosedXML.Excel.XLWorkbook GetExcelResult<TEntity>(this ModelSource<TEntity> models,IQueryable items,String tableName = null)
            where TEntity : class, new()
        {
            using (DataSet ds = models.GetDataSetResult(items))
            {
                if (tableName != null)
                    ds.Tables[0].TableName = ds.DataSetName = tableName;
                ClosedXML.Excel.XLWorkbook xls = new ClosedXML.Excel.XLWorkbook();
                xls.Worksheets.Add(ds);
                return xls;
            }
        }

        public static IQueryable<Organization> InitializeOrganizationQuery(this UserProfileMember userProfile, GenericManager<EIVOEntityDataContext> mgr)
        {
            switch ((Naming.CategoryID)userProfile.CurrentUserRole.OrganizationCategory.CategoryID)
            {
                case Naming.CategoryID.COMP_SYS:
                    return mgr.GetTable<Organization>().Where(
                        o => o.OrganizationCategory.Any(
                            c => c.CategoryID == (int)Naming.CategoryID.COMP_E_INVOICE_B2C_SELLER
                                || c.CategoryID == (int)Naming.CategoryID.COMP_VIRTUAL_CHANNEL
                                || c.CategoryID == (int)Naming.CategoryID.COMP_E_INVOICE_GOOGLE_TW
                                || c.CategoryID == (int)Naming.CategoryID.COMP_INVOICE_AGENT));

                case Naming.CategoryID.COMP_INVOICE_AGENT:
                    return mgr.GetQueryByAgent(userProfile.CurrentUserRole.OrganizationCategory.CompanyID);

                case Naming.CategoryID.COMP_E_INVOICE_B2C_SELLER:
                case Naming.CategoryID.COMP_VIRTUAL_CHANNEL:
                case Naming.CategoryID.COMP_E_INVOICE_GOOGLE_TW:
                    return mgr.GetTable<Organization>().Where(
                        o => o.CompanyID == userProfile.CurrentUserRole.OrganizationCategory.CompanyID);
                default:
                    break;
            }

            return mgr.GetTable<Organization>().Where(o => false);
        }


    }
}