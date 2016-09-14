using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using DataAccessLayer.basis;
using Model.SCMDataEntity;
using Uxnet.Web.Module.Common;
using System.Web.UI.WebControls;

namespace eIVOGo.Module.Base
{
    public abstract partial class SCMEntityActionList<TEntity> : SCMEntityList<TEntity>
        where TEntity : class, new()
    {

        protected global::Uxnet.Web.Module.Common.ActionHandler doDelete;
        protected global::Uxnet.Web.Module.Common.ActionHandler doEdit;
        protected global::Uxnet.Web.Module.Common.ActionHandler doCreate;
        protected global::Uxnet.Web.Module.Common.ActionHandler doShowRefusal;
        protected global::eIVOGo.Module.SCM.View.ShowRefusalReason showRefusal;
        protected global::eIVOGo.Module.SCM.View.RefuseDocument refuseDoc;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            doDelete.DoAction = arg =>
            {
                delete(int.Parse(arg));
            };
            doEdit.DoAction = arg =>
                {
                    edit(arg);
                };
            doCreate.DoAction = arg =>
            {

            };
            doShowRefusal.DoAction = arg =>
            {
                showRefusal.Show(int.Parse(arg));
            };
        }


        protected override void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Modify":
                    edit((String)e.CommandArgument);
                    break;
                case "Delete":
                    delete(int.Parse((String)e.CommandArgument));
                    break;
                case "Create":
                    create();
                    break;
            }
        }

        protected virtual void create()
        {

        }

        protected virtual void edit(String keyValue)
        {
            Page.Items["id"] = int.Parse(keyValue);
        }

        protected virtual void delete(int keyValue)
        {
            refuseDoc.DocID = keyValue;
            refuseDoc.Show();
        }
    }
}