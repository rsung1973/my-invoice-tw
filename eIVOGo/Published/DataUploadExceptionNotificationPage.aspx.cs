using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eIVOGo.Published
{
    public partial class DataUploadExceptionNotificationPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int companyID;
            int LogID = Convert.ToInt32(Request["LogID"]);
            if (!String.IsNullOrEmpty(Request["companyID"]) && int.TryParse(Request["companyID"], out companyID))
            {
                notification.QueryExpr = d => d.ExceptionReplication != null && d.CompanyID == companyID && d.LogID <= LogID;
            }
            else
            {
                notification.QueryExpr = d => d.ExceptionReplication != null && d.LogID <= LogID;
            }
        }
    }
}