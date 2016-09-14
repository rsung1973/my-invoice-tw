using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eIVOGo.Module.Base
{
    public abstract partial class SCMEntityPreview<TEntity> : System.Web.UI.UserControl, eIVOGo.Module.Base.ISCMEntityPreview
         where TEntity : class,new()
    {

        protected TEntity _item;
        protected global::Uxnet.Web.Module.DataModel.DataModelContainer modelItem;
                       

        public abstract void PrepareDataFromDB(object keyValue);

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(SCMEntityPreview_PreRender);
            modelItem.ItemType = typeof(TEntity);
            modelItem.Load += new EventHandler(modelItem_Load);
        }

        protected virtual void modelItem_Load(object sender, EventArgs e)
        {
            if (_item == null)
            {
                _item = (TEntity)modelItem.DataItem;
                if (_item != null)
                {
                    prepareDataForViewState();
                }
            }
        }

        protected abstract void prepareDataForViewState();

        protected virtual void SCMEntityPreview_PreRender(object sender, EventArgs e)
        {
            this.DataBind();
        }

        public override void DataBind()
        {
            if (_item != null)
            {
                base.DataBind();
            }
        }

        public TEntity Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                modelItem.DataItem = value;
            }
        }
    }
}