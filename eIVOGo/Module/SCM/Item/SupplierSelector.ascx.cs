using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Linq.Expressions;
using Model.SCMDataEntity;
using Uxnet.Web.Module.DataModel;

namespace eIVOGo.Module.SCM.Item
{
    public partial class SupplierSelector : ItemSelector
    {
        public event EventHandler Done;

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public Expression<Func<SUPPLIER, bool>> QueryExpr
        {
            get;
            set;
        }

        protected override void selector_DataBound(object sender, EventArgs e)
        {
            selector.Items.Clear();

            if (QueryExpr != null)
            {
                selector.Items.AddRange(((SupplierDataSource)dsEntity).CreateDataManager().EntityList.Where(QueryExpr)
                    .OrderBy(o => o.SUPPLIER_BAN).Select(o => new ListItem(
                        String.Format("{0} {1}", o.SUPPLIER_BAN, o.SUPPLIER_NAME), o.SUPPLIER_SN.ToString())
                        ).ToArray());
            }
            else
            {
                selector.Items.AddRange(((SupplierDataSource)dsEntity).CreateDataManager().EntityList
                    .OrderBy(o => o.SUPPLIER_BAN).Select(o => new ListItem(
                        String.Format("{0} {1}", o.SUPPLIER_BAN, o.SUPPLIER_NAME), o.SUPPLIER_SN.ToString())
                        ).ToArray());
            }

            if (Request[selector.UniqueID] != null)
            {
                var item = selector.Items.FindByValue(Request[selector.UniqueID]);
                if (item != null)
                    item.Selected = true;
            }

            _isBound = true;
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }
    }
}