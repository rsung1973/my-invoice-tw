using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.SCM;
using Model.SCMDataEntity;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;
using Model.Locale;
using eIVOGo.Module.Base;
using System.Web.UI;

namespace eIVOGo.Module.SCM
{
    public partial class PromotionalProjectMaintainAdd : SCMEntityEdit<SALES_PROMOTION>
    {
        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            productQueryByName.QueryExpr = p => p.PRODUCTS_NAME.Contains(productQueryByName.FieldValue);
            productQueryByName.Done += new EventHandler(productQueryByName_Done);
            this.PreRender += new EventHandler(PromotionalProjectMaintainAdd_PreRender);
        }

        void productQueryByName_Done(object sender, EventArgs e)
        {
            foreach (var prodItem in productQueryByName.Items)
            {
                addProductInPromotion(prodItem);
            }

        }

        void PromotionalProjectMaintainAdd_PreRender(object sender, EventArgs e)
        {
            //promoDetails.UpdateData();
            //_item.SALES_PROMOTION_NAME = this.txtPromotionalName.Text;
            //_item.SALES_PROMOTION_DISCOUNT = String.IsNullOrEmpty(txtPromotionalPercent.Text) ? (decimal?)null : decimal.Parse(this.txtPromotionalPercent.Text);
            //_item.SALES_PROMOTION_SYMBOL = this.txtPromotionalNo.Text;
            if (!this.IsPostBack && _item.DataFrom == Naming.DataItemSource.FromDB)
            {
                this.txtPromotionalName.Text = _item.SALES_PROMOTION_NAME;
                txtPromotionalPercent.Text = _item.SALES_PROMOTION_DISCOUNT.ToString();
                this.txtPromotionalNo.Text = _item.SALES_PROMOTION_SYMBOL;
                this.DataBind();
            }

            if (_item.DataFrom == Naming.DataItemSource.FromDB)
            {
                this.actionItem.ItemName = "首頁 > 修改促銷專案";
                this.titleBar.ItemName = "修改促銷專案";
            }
            //this.DataBind();
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtPromotionalName.Text))
            {
                this.AjaxAlert("請輸入促銷專案名稱!!");
                return;
            }

            if (promoDetails.Items.Count == 0)
            {
                this.AjaxAlert("請輸入料品資訊!!");
                return;
            }

            try
            {
                promoDetails.UpdateData();

                _item.SALES_PROMOTION_NAME = this.txtPromotionalName.Text;
                _item.SALES_PROMOTION_DISCOUNT = String.IsNullOrEmpty(txtPromotionalPercent.Text) ? (decimal?)null : decimal.Parse(this.txtPromotionalPercent.Text);
                _item.SALES_PROMOTION_SYMBOL = this.txtPromotionalNo.Text;
                

                SalesPromotionManager mgr = new SalesPromotionManager(dsUpdate.CreateDataManager());
                modelItem.DataItem= mgr.Save(_item);
                //Server.Transfer(ToAddSalesPromo.TransferTo);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('促銷專案資料維護完成!!'); location.href='Promotional_Project_Maintain.aspx';", true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                this.AjaxAlert("系統發生錯誤,錯誤原因:" + ex.Message);
            }
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var prodItem = dsEntity.CreateDataManager().GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_BARCODE == txtGoodsBarcode.Text).FirstOrDefault();
            if (prodItem == null)
            {
                this.AjaxAlert("料品資料不存在!!");
                return;
            }

            addProductInPromotion(prodItem);
            //Server.Transfer("Promotional_Project_Maintain.aspx", true);
            
        }

        private void addProductInPromotion(PRODUCTS_DATA prodItem)
        {
            _item.SALES_PROMOTION_PRODUCTS.Add(new SALES_PROMOTION_PRODUCTS
            {
                PRODUCTS_SN = prodItem.PRODUCTS_SN,
                PRODUCTS_DATA = prodItem
            });

        }

        protected override void prepareInitialData()
        {
            Item = new SALES_PROMOTION
            {

            };

            promoDetails.Items = Item.SALES_PROMOTION_PRODUCTS;
        }

        public override void PrepareDataFromDB(object keyValue)
        {
            int promotionSN = (int)keyValue;
            _item = dsEntity.CreateDataManager().EntityList.Where(p => p.SALES_PROMOTION_SN == promotionSN).FirstOrDefault();
            _item.DataFrom = Naming.DataItemSource.FromDB;
            modelItem.DataItem = _item;
            promoDetails.Items = _item.SALES_PROMOTION_PRODUCTS;
            //this.actionItem.ItemName = "首頁 > 修改促銷專案";
            //this.titleBar.ItemName = "修改促銷專案";
        }

        protected override void prepareDataForViewState()
        {
            promoDetails.Items = _item.SALES_PROMOTION_PRODUCTS;
        }
    }
}