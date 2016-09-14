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
    public abstract partial class EntityModal<T, TEntity> : EntityBase<T, TEntity>
        where T : DataContext, new()
        where TEntity : class, new()
    {

        protected global::AjaxControlToolkit.ModalPopupExtender ModalPopupExtender;
        protected global::Uxnet.Web.Module.Common.ActionHandler doCancel;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            doCancel.DoAction = arg =>
            {
                this.ModalPopupExtender.Hide();
                this.Visible = false;
            };
        }

        public virtual void Show()
        {
            this.Visible = true;
            this.ModalPopupExtender.Show();
        }

        public virtual void Close()
        {
            ModalPopupExtender.Hide();
            this.Visible = false;
        }

        public override void BindData()
        {
            base.BindData();
            this.Visible = true;
            this.ModalPopupExtender.Show();
        }
    }
}