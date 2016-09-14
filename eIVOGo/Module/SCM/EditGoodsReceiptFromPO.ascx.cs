using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using eIVOGo.Module.SCM.View;

namespace eIVOGo.Module.SCM
{
    public partial class EditGoodsReceiptFromPO : EditWarehouseWarrant
    {
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            itemList.UpdateData();
            Server.Transfer(NextAction.TransferTo);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Server.Transfer(PreviousAction.TransferTo);
        }
    }
}