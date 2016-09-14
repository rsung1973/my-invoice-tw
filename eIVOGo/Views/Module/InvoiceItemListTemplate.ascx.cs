using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Helper;
using eIVOGo.Helper;
using eIVOGo.Models;
using eIVOGo.Module.Base;
using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.Module.Common;

namespace eIVOGo.Views.Module
{
    public partial class InvoiceItemListTemplate : DataListSource<InvoiceItem>
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            gridInit.GetRecordCount = () =>
            {
                return models.Items.Count();
            };
            gridInit.AllowPaging = true;
        }

        [Bindable(true)]
        public override bool AllowPaging
        {
            get
            {
                return gridInit.AllowPaging;
            }
            set
            {
                gridInit.AllowPaging = value;
            }
        }

        [Bindable(true)]
        public override bool PrintMode
        {
            get
            {
                return gridInit.PrintMode;
            }
            set
            {
                gridInit.PrintMode = value;
            }
        }

    }    
}