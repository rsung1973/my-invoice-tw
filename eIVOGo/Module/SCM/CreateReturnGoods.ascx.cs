using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using Uxnet.Web.WebUI;
using eIVOGo.Module.SCM.Item;
using eIVOGo.Helper;

namespace eIVOGo.Module.SCM
{
    public partial class CreateReturnGoods : System.Web.UI.UserControl
    {
        protected GOODS_RETURNED _item;

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(CreateReturnGoods_PreRender);
            returnGoods.ItemType = typeof(GOODS_RETURNED);
            returnGoods.Load += new EventHandler(returnGoods_Load);
        }


        void returnGoods_Load(object sender, EventArgs e)
        {
            _item = (GOODS_RETURNED)returnGoods.DataItem;
            _item.BUYER_ORDERS = dsEntity.CreateDataManager().EntityList.Where(b => b.BUYER_ORDERS_SN == _item.BUYER_ORDERS_SN).First();
            returnDetails.Items = _item.GOODS_RETURNED_DETAILS;
        }

        void CreateReturnGoods_PreRender(object sender, EventArgs e)
        {
            if (_item != null)
            {
                returnDetails.UpdateData();
                this.DataBind();
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Server.Transfer(ToInquireReturn.TransferTo);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Reason.Text))
            {
                this.AjaxAlert("請填入退貨原因!!");
                return;
            }
            _item.GR_REASON = Reason.Text;

            if (String.IsNullOrEmpty(inboundWarehouse.SelectedValue))
            {
                this.AjaxAlert("未設定入庫倉儲!!");
                return;
            }

            _item.WAREHOUSE_SN = int.Parse(inboundWarehouse.SelectedValue);

            returnDetails.UpdateData(); 
            var returnedItems = returnDetails.GetSelectedItems(_item.GOODS_RETURNED_DETAILS);
            if (returnedItems.Count < 1)
            {
                this.AjaxAlert("尚未設定退貨項目!!");
                return;
            }

            _item.GOODS_RETURNED_DETAILS.Clear();
            _item.GOODS_RETURNED_DETAILS.AddRange(returnedItems);

            Server.Transfer(ToPreviewReturn.TransferTo);
        }
    }
}