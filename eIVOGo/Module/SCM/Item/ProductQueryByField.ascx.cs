using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Model.SCMDataEntity;
using Utility;
using System.Data.Linq;

namespace eIVOGo.Module.SCM.Item
{
    public partial class ProductQueryByField : ProductQuery
    {
        public String FieldValue
        {
            get
            {
                return queryField.Text;
            }
            set
            {
                queryField.Text = value;
            }
        }
    }
}