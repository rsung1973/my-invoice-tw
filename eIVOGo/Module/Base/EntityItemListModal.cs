using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.Common;
using Uxnet.Web.Module.DataModel;

namespace eIVOGo.Module.Base
{
    public abstract partial class EntityItemListModal<T, TEntity> : EntityItemList<T, TEntity>
        where T : DataContext, new()
        where TEntity : class, new()
    {
        protected global::AjaxControlToolkit.ModalPopupExtender ModalPopupExtender;

        public event EventHandler Done;

        public virtual void Show()
        {
            this.Visible = true;
        }

        protected virtual void OnDone(object sender, EventArgs e)
        {
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        public virtual void Close()
        {
            this.Visible = false;
        }

        public override void BindData()
        {
            this.Visible = true;
            base.BindData();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(EntityItemListModal_PreRender);
        }

        void EntityItemListModal_PreRender(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.ModalPopupExtender.Show();
            }
            else
            {
                this.ModalPopupExtender.Hide();
            }
        }

    }
}