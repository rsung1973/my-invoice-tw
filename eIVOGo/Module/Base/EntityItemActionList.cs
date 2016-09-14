using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI.WebControls;

using DataAccessLayer.basis;
using Uxnet.Web.Module.Common;

namespace eIVOGo.Module.Base
{
    public abstract partial class EntityItemActionList<T, TEntity> : EntityItemList<T, TEntity>
        where T : DataContext, new()
        where TEntity : class, new()
    {

        protected global::Uxnet.Web.Module.Common.ActionHandler doDelete;
        protected global::Uxnet.Web.Module.Common.ActionHandler doEdit;
        protected global::Uxnet.Web.Module.Common.ActionHandler doCreate;
        protected EditEntityItem<T,TEntity> editItem;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            doDelete.DoAction = arg =>
            {
                delete(arg);
            };
            doEdit.DoAction = arg =>
            {
                edit(arg);
            };
            doCreate.DoAction = arg =>
            {
                create();
            };
            editItem.Done += new EventHandler(editItem_Done);
        }

        void editItem_Done(object sender, EventArgs e)
        {
            gvEntity.DataBind();
        }

        protected virtual void create()
        {

        }

        protected virtual void edit(String keyValue)
        {

        }

        protected virtual void delete(String keyValue)
        {

        }

    }
}