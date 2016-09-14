using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Linq.Expressions;
using Model.DataEntity;

namespace eIVOGo.Module.SYS.Item
{
    public partial class OrgaCateSelector : System.Web.UI.UserControl
    {
        private bool _isBound;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(OrgaCateSelector_PreRender);
        }

        void OrgaCateSelector_PreRender(object sender, EventArgs e)
        {
            if (!_isBound)
            {
                BindData();
            }

        }

        public Expression<Func<OrganizationCategory, bool>> QueryExpr
        {
            get;
            set;
        }

        public void BindData()
        {
            OrgaCateSelect.Items.Clear();

            if (QueryExpr != null)
            {
                OrgaCateSelect.Items.AddRange(dsOrgaCate.CreateDataManager().EntityList.Where(QueryExpr)
                    .OrderBy(o => o.Organization.ReceiptNo).Select(o => new ListItem(
                        String.Format("{2} {0}－{1}", o.Organization.CompanyName, o.CategoryDefinition.Category, o.Organization.ReceiptNo), o.OrgaCateID.ToString())
                        ).ToArray());
            }
            else
            {
                OrgaCateSelect.Items.AddRange(dsOrgaCate.CreateDataManager().EntityList
                    .OrderBy(o => o.Organization.ReceiptNo).Select(o => new ListItem(
                        String.Format("{2} {0}－{1}", o.Organization.CompanyName, o.CategoryDefinition.Category, o.Organization.ReceiptNo), o.OrgaCateID.ToString())
                        ).ToArray());
            }

            if (Request[OrgaCateSelect.UniqueID] != null)
            {
                var item = OrgaCateSelect.Items.FindByValue(Request[OrgaCateSelect.UniqueID]);
                if (item != null)
                    item.Selected = true;
            }

            _isBound = true;
        }

        [Bindable(true)]
        public String OrgaCateID
        {
            get
            {
                return _isBound ? OrgaCateSelect.SelectedValue : Request[OrgaCateSelect.UniqueID];
            }
            set
            {
                OrgaCateSelect.SelectedValue = value;
            }
        }

        [Bindable(true)]
        public String SelectedValue
        {
            get
            {
                return _isBound ? OrgaCateSelect.SelectedValue : Request[OrgaCateSelect.UniqueID];
            }
            set
            {
                OrgaCateSelect.SelectedValue = value;
            }
        }

    }
}