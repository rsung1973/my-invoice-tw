using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI.WebControls;

using DataAccessLayer.basis;
using Utility;
using Uxnet.Web.WebUI;
using Uxnet.Web.Module.DataModel;

namespace eIVOGo.Module.Base
{
    public abstract partial class EntityBase<T, TEntity> : EntityDataSource<T, TEntity>
        where T : DataContext, new()
        where TEntity : class, new()
    {

        protected TEntity _entity;

        public event EventHandler DataLoaded;

        protected virtual void loadEntity()
        {
            if (_entity == null)
            {
                _entity = Select().FirstOrDefault();
                if (_entity != null)
                {
                    OnDataLoaded(this, new EventArgs { });
                }
            }
        }

        protected virtual void OnDataLoaded(object sender, EventArgs eventArgs)
        {
            if (DataLoaded != null)
            {
                DataLoaded(sender, eventArgs);
            }
        }

        public TEntity DataItem
        {
            get
            {
                return _entity;
            }
        }

        public virtual void BindData()
        {
            if (_entity == null)
            {
                loadEntity();
            }
            this.DataBind();
        }

        public override void DataBind()
        {
            if (_entity != null)
            {
                base.DataBind();
            }
        }

        public void PrepareEntity()
        {
            loadEntity();
        }

    }
}