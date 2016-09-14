using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using Uxnet.Web.WebUI;
using Utility;
using System.ComponentModel;

namespace eIVOGo.Module.UI
{
    public partial class WaitEventModal : System.Web.UI.UserControl
    {
        private ThreadInfo _waitEvent;
        private WaitCallback _doWhat;

        public event EventHandler ActionComplete;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                initializeData();
            }
        }

        [Bindable(true)]
        public String CompleteMessage
        { get; set; }

        private void initializeData()
        {
            if (Session["waitEvent"] != null)
            {
                _waitEvent = (ThreadInfo)Session["waitEvent"];
                _waitEvent._EventHandle.WaitOne();
                _waitEvent._EventHandle.Set();
                Session.Remove("waitEvent");
                if (_waitEvent._Exception == null)
                {
                    if (!String.IsNullOrEmpty(CompleteMessage))
                    {
                        this.AjaxAlert(CompleteMessage);
                    }
                    if (ActionComplete != null)
                    {
                        ActionComplete(this, new EventArgs());
                    }
                }
                else
                {
                    this.AjaxAlert(_waitEvent._Exception.Message);
                }
            }
        }


        public void Do(WaitCallback doWhat)
        {
            if (doWhat != null)
            {
                _doWhat = doWhat;

                this.ModalPopupExtender.Show();
                _waitEvent = new ThreadInfo
                {
                    _EventHandle = new AutoResetEvent(false)
                };
                Session["waitEvent"] = _waitEvent;

                ThreadPool.QueueUserWorkItem(new WaitCallback(process), _waitEvent);
                if (ScriptManager.GetCurrent(Page) != null)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(WaitEventModal), "redirect", 
                        String.Format("window.location.href='{0}';", Request.RawUrl), true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(WaitEventModal), "redirect",
                        String.Format("window.location.href='{0}';", Request.RawUrl), true);
                }
            }
        }

        private void process(Object stateInfo)
        {
            ThreadInfo info = (ThreadInfo)stateInfo;
            try
            {
                _doWhat(info);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                info._Exception = ex;
            }
            finally
            {
                info._EventHandle.Set();
            }
        }


        class ThreadInfo
        {
            public AutoResetEvent _EventHandle { get; set; }
            public Exception _Exception { get; set; }
        }

    }
}