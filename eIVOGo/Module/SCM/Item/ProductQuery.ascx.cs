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
    public partial class ProductQuery : System.Web.UI.UserControl
    {
        public event EventHandler Done;
        public event EventHandler BeforeQuery;

        protected bool _doQuery = true;

        public bool DoQuery
        {
            get
            {
                return _doQuery;
            }
            set
            {
                _doQuery = value;
            }

        }

        public IEnumerable<PRODUCTS_DATA> Items
        { get; set; }

        public PRODUCTS_DATA Item
        { get; set; }

        public Expression<Func<PRODUCTS_DATA, bool>> QueryExpr
        {
            get;
            set;
        }

        public  Func<Table<PRODUCTS_DATA>,IQueryable<PRODUCTS_DATA>> BuildQuery
        {
            get;
            set;
        }

        protected virtual void btnQuery_Click(object sender, EventArgs e)
        {

            if (BeforeQuery != null)
            {
                BeforeQuery(this, new EventArgs());
            }

            if (_doQuery)
            {

                if (BuildQuery != null)
                {
                    productItem.Query = BuildQuery(dsProd.CreateDataManager().EntityList);
                }
                else if (QueryExpr != null)
                {
                    productItem.QueryExpr = QueryExpr;
                }

                productItem.BindData();
                ModalPopupExtender.Show();
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (productItem.SelectedValues != null && productItem.SelectedValues.Length > 0)
            {
                int[] productSN = productItem.SelectedValues.Select(s => int.Parse(s)).ToArray();
                Items = dsProd.CreateDataManager().EntityList.Where(p => productSN.Contains(p.PRODUCTS_SN));
                Item = Items.FirstOrDefault();
                if (Done != null)
                {
                    Done(this, new EventArgs());
                }
            }
        }
    }
}