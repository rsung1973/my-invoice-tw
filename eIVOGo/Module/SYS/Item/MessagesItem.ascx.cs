using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;
using eIVOGo.Module.Base;

namespace eIVOGo.Module.SYS.Item
{
    public partial class MessagesItem : EditEntityItemBase<EIVOEntityDataContext, SystemMessage>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.QueryExpr = m => m.MsgID == (int?)modelItem.DataItem;
            btnUpdate.OnClientClick = doConfirm.GetPostBackEventReference(null);
            this.PreRender += new EventHandler(MessagesItem_PreRender);
        }

        void MessagesItem_PreRender(object sender, EventArgs e)
        {
            if (_entity != null)
            {
                if (_entity.StartDate.HasValue)
                    DateFrom.DateTimeValue = _entity.StartDate.Value;
                else
                    DateFrom.Reset();
                if (_entity.EndDate.HasValue)
                    DateTo.DateTimeValue = _entity.EndDate.Value;
                else
                    DateTo.Reset();
            }
        }

        public void Show()
        {
            this.ModalPopupExtender.Show();
        }

        public void Clean()
        {
            modelItem.DataItem = null;
            this.txtMsg.Text = "";
            DateFrom.Reset();
            DateTo.Reset();
            this.chkShowForever.Checked = true;
        }

        protected override bool saveEntity()
        {
            var mgr = dsEntity.CreateDataManager();

            if (String.IsNullOrEmpty(this.txtMsg.Text.Trim()))
            {
                this.AjaxAlert("未填寫訊息內容!!");
                return false;
            }

            loadEntity();

            if (_entity == null)
            {
                _entity = new SystemMessage
                {
                    CreateTime = DateTime.Now
                };

                mgr.EntityList.InsertOnSubmit(_entity);
            }
            _entity.MessageContents = this.txtMsg.Text.Trim();
            _entity.AlwaysShow = this.chkShowForever.Checked;

            if (DateFrom.HasValue)
                _entity.StartDate = DateFrom.DateTimeValue;

            if (DateTo.HasValue)
                _entity.EndDate = DateTo.DateTimeValue;

            _entity.AlwaysShow = this.chkShowForever.Checked;
            _entity.UpdateTime = DateTime.Now;

            mgr.SubmitChanges();

            return true;
        }
    }
}