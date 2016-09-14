using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;

namespace eIVOGo.Module.UI
{
    public partial class CALogForInvoiceItem : System.Web.UI.UserControl
    {
        public CALog DataItem { get; set; }
        public ListItemType ItemType { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(),"logID"))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "logID", @"
                    function createLogID(arg) {
                            var form = document.forms[0];

                            var element = form.all('logID');
                            if (element == null) {
                                element = document.createElement('input');
                                element.name = 'logID';
                                element.type = 'hidden';
                                element.style.display = 'none';
                                element.style.visibility = 'hidden';
                                form.appendChild(element);
                            }
                            element.value = arg;
                        }
                ",true);
            }
        }

        public override void DataBind()
        {
            if (DataItem != null)
            {
                base.DataBind();
                imgBtn.OnClientClick = String.Format("createLogID('{0}?logID={1}');", imgBtn.CommandArgument, DataItem.LogID);
            }
        }

        public void BindData(CALog item)
        {
            DataItem = item;
            this.DataBind();
        }
    }
}