using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer.basis;
using Model.SCMDataEntity;

namespace eIVOGo.Module.SCM.Item
{
    public partial class DataContextSession : System.Web.UI.UserControl
    {
        private GenericManager<SCMEntityDataContext> _mgr;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Unload += new EventHandler(DataContextSession_Unload);
            if (Session["__dataContext"] != null)
            {
                _mgr = (GenericManager<SCMEntityDataContext>)Session["__dataContext"];
                Page.Items["__LinqDS"] = _mgr;
            }
        }

        void DataContextSession_Unload(object sender, EventArgs e)
        {
            if (Session["__dataContext"] == null && _mgr != null)
            {
                _mgr.Dispose();
            }
        }

        public void PutDataContext()
        {
            if (_mgr == null)
            {
                _mgr = new GenericManager<SCMEntityDataContext>();
                Session["__dataContext"] = _mgr;
            }
        }

        public void ReleaseDataContext()
        {
            Session["__dataContext"] = null;
        }

        public void SetObject(String key, Object value)
        {
            Session[key] = value;
        }

        public object GetObject(String key)
        {
            return Session[key];
        }

    }
}