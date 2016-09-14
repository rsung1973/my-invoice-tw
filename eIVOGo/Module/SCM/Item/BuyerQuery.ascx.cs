using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;

namespace eIVOGo.Module.SCM.Item
{
    public partial class BuyerQuery : System.Web.UI.UserControl
    {
        public event EventHandler Done;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IEnumerable<BUYER_DATA> Items
        { get; set; }

        public BUYER_DATA Item
        {
            get
            {
                return queryItem.Item;
            }
        }

        public String QueryArgument
        {
            get
            {
                return btnQuery.CommandArgument;
            }
            set
            {
                btnQuery.CommandArgument = value;
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            //if (!String.IsNullOrEmpty(productName.Text))
            //{
            //    Item = dsProd.CreateDataManager().EntityList.Where(p => p.PRODUCTS_NAME.Contains(productName.Text)).FirstOrDefault();
            //    if (Item != null)
            //        productName.Text = Item.PRODUCTS_NAME;
            //    if (Done != null)
            //    {
            //        Done(this, new EventArgs());
            //    }
            //}

            if (!String.IsNullOrEmpty(queryField.Text))
            {
                queryItem.QueryExpr = p => p.BUYER_NAME.Contains(queryField.Text);
            }
            queryItem.BindData();
            ModalPopupExtender.Show();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (queryItem.SelectedValues != null && queryItem.SelectedValues.Length > 0)
            {
                int[] itemSN = queryItem.SelectedValues.Select(s => int.Parse(s)).ToArray();
                Items = dsEntity.CreateDataManager().EntityList.Where(p => itemSN.Contains(p.BUYER_SN));
                if (Done != null)
                {
                    Done(this, new EventArgs());
                }
                queryField.Text = Item.BUYER_NAME;
                btnQuery.CommandArgument = "Query";
            }
        }
    }
}