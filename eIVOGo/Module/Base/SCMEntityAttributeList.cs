using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.SCMDataEntity;
using DataAccessLayer.basis;
using System.Web.UI.WebControls;
using eIVOGo.Helper;
using eIVOGo.Module.SCM;

namespace eIVOGo.Module.Base
{
    public abstract partial class SCMEntityAttributeList<TEntity> : System.Web.UI.UserControl
        where TEntity : class,new()
    {
        protected PRODUCTS_DATA _currentItem;

        protected global::System.Web.UI.WebControls.GridView gvEntity;
        protected LinqToSqlDataSource<SCMEntityDataContext, TEntity> dsEntity;
        protected global::Uxnet.Web.Module.Common.ActionHandler doDelete;


        public IList<TEntity> Items
        {
            get
            {
                return gvEntity.DataSource as IList<TEntity>;
            }
            set
            {
                gvEntity.DataSource = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            doDelete.DoAction = arg =>
            {
                delete(int.Parse(arg));
            };
            this.PreRender += new EventHandler(PromoDetailsEditList_PreRender);
        }

        void PromoDetailsEditList_PreRender(object sender, EventArgs e)
        {
            if (Items != null)
            {
                BindData();
            }
        }

        protected abstract PRODUCTS_DATA loadItem(TEntity item);

        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Modify":
                    break;
                case "Delete":
                    delete(int.Parse((String)e.CommandArgument));
                    break;
                case "Create":
                    break;
            }
        }

        private void delete(int key)
        {
            Items.RemoveAt(key);
        }

        public void BindData()
        {
            extendAttributeField();
            gvEntity.DataBind();
        }

        public abstract void UpdateData();
        protected abstract IEnumerable<int> getProductSN();

        public IList<TEntity> GetSelectedItems(IList<TEntity> items)
        {
            String[] index = Request.GetItemSelection();
            List<TEntity> selectedItems = new List<TEntity>();
            if (index != null && index.Length > 0)
            {
                foreach (var idx in index.Select(s => int.Parse(s)).ToArray())
                {
                    selectedItems.Add(items[idx]);
                }
            }
            return selectedItems;
        }

        protected void extendAttributeField()
        {
            var mgr = dsEntity.CreateDataManager();
            var prodSN = getProductSN();
            var nameItems = mgr.GetTable<PRODUCTS_ATTRIBUTE_MAPPING>().Where(m => prodSN.Contains(m.PRODUCTS_SN)).Select(m => m.PRODUCTS_ATTRIBUTE_NAME).Distinct();
            int index = gvEntity.Columns.Count - 1;
            foreach (var item in nameItems)
            {
                TemplateField field = new TemplateField
                {
                    HeaderText = item.PRODUCTS_ATTR_NAME
                };

                field.ItemTemplate = new DataFieldViewLoader
                {
                    ControlLoader = this,
                    Field = field
                };

                gvEntity.Columns.Insert(index, field);
                index++;
            }
        }

        protected void gvEntity_DataBinding(object sender, EventArgs e)
        {
        }

    }
}