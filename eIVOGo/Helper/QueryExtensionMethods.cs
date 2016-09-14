using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Model.DataEntity;

namespace eIVOGo.Helper
{
    public static class QueryExtensionMethods
    {
        public static DataSet GetDataSetResult<TEntity>(this ModelSource<TEntity> models)
            where TEntity : class,new()
        {
            using (SqlCommand sqlCmd = (SqlCommand)models.GetCommand(models.Items))
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

        public static ClosedXML.Excel.XLWorkbook GetExcelResult<TEntity>(this ModelSource<TEntity> models)
            where TEntity : class,new()
        {
            using (DataSet ds = models.GetDataSetResult())
            {
                ClosedXML.Excel.XLWorkbook xls = new ClosedXML.Excel.XLWorkbook();
                xls.Worksheets.Add(ds);
                return xls;
            }
        }

    }
}