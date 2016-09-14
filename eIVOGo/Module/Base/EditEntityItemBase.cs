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
    public abstract partial class EditEntityItemBase<T, TEntity> : EntityBase<T, TEntity>
        where T : DataContext, new()
        where TEntity : class, new()
    {

        protected global::Uxnet.Web.Module.Common.ActionHandler doConfirm;

        public event EventHandler Done;
        public Action<Exception> ReportError;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            doConfirm.DoAction = arg =>
            {
                try
                {
                    if (saveEntity() && Done != null)
                    {
                        Done(this, new EventArgs());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    if (ReportError == null)
                    {
                        this.AjaxAlert(String.Format("作業失敗,原因:{0}", String.IsNullOrEmpty(_errorMsg) ? ex.Message : _errorMsg));
                    }
                    else
                    {
                        ReportError(ex);
                    }
                }
            };
        }

        protected virtual bool saveEntity()
        {
            return false;
        }
    }
}