using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.SCM;
using Model.SCMDataEntity;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;
using eIVOGo.Module.Base;
using Model.Locale;
using eIVOGo.Helper;
using eIVOGo.Module.SCM.Item;

namespace eIVOGo.Module.SCM
{
    public partial class addProducts_Plan_Maintain : SCMEntityEdit<WAREHOUSE>
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //productQuery.QueryExpr = p => p.PRODUCTS_NAME.Contains(productQuery.FieldValue);
            //productQuery.Done += new EventHandler(productQuery_Done);
        }

        void productQuery_Done(object sender, EventArgs e)
        {
            //var items = ((ProductQuery)sender).Items;
            //if (items != null)
            //{
            //    _item.PRODUCTS_WAREHOUSE_MAPPING.AddRange(
            //        items.Select(p => new PRODUCTS_WAREHOUSE_MAPPING
            //        {
            //            PRODUCTS_PLAN_AMOUNT = 0,
            //            PRODUCTS_SAFE_AMOUNT_PERCENTAGE = 0,
            //            PRODUCTS_SN = p.PRODUCTS_SN,
            //            PRODUCTS_DATA = p
            //        }).ToArray()
            //        );
            //}
        }

        protected override void SCMEntityPreview_PreRender(object sender, EventArgs e)
        {
            //mappingDetails.UpdateData();
            //if (!this.IsPostBack && _item != null)
            //if (!this.IsPostBack && _item.DataFrom == Naming.DataItemSource.FromDB)
            //{
            //    warehouse.SelectedValue = _item.WAREHOUSE_SN.ToString();
            //}
            if (_item.DataFrom == Naming.DataItemSource.FromDB)
            {
                this.actionItem.ItemName = "首頁 > 編輯庫存警示維護資料";
                this.titleBar.ItemName = "編輯庫存警示維護資料";
            }
        }

        public override void PrepareDataFromDB(object keyValue)
        {
            //string[] temp = ((string)keyValue).Split(':');
            //int pwmsn = int.Parse(temp[0]);
            int PWMSN = (int)keyValue;
            //int warehouseSN = int.Parse(temp[1]);
            //_item = dsEntity.CreateDataManager().EntityList.Where(p => p.WAREHOUSE_SN == warehouseSN).First();
            //_item = dsEntity.CreateDataManager().EntityList.Where(p => p.PRODUCTS_WAREHOUSE_MAPPING.Any(pw => pw.PW_MAPPING_SN == PWMSN)).FirstOrDefault();
            _item = dsEntity.CreateDataManager().GetTable<PRODUCTS_WAREHOUSE_MAPPING>().Where(sn => sn.PW_MAPPING_SN == PWMSN).FirstOrDefault().WAREHOUSE;
            _item.DataFrom = Naming.DataItemSource.FromDB;
            
            mappingDetails.Items = _item.PRODUCTS_WAREHOUSE_MAPPING.Where(sn => sn.PW_MAPPING_SN == PWMSN).ToList();
            modelItem.DataItem = _item;
            this.warehouse.SelectedValue = _item.WAREHOUSE_SN.ToString();
            this.warehouse.Selector.Enabled = false;
            ViewState["PW_MAPPING_SN"] = PWMSN;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var prodItem = dsEntity.CreateDataManager().GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_BARCODE == Barcode.Text).FirstOrDefault();

            if (prodItem == null)
            {
                this.AjaxAlert("料品資料不存在!!");
                return;
            }

            _item.PRODUCTS_WAREHOUSE_MAPPING.Add(new PRODUCTS_WAREHOUSE_MAPPING
                    {
                        PRODUCTS_PLAN_AMOUNT = 0,
                        PRODUCTS_SAFE_AMOUNT_PERCENTAGE = 0,
                        PRODUCTS_SN = prodItem.PRODUCTS_SN,
                        PRODUCTS_DATA = prodItem
                    });

        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(this.warehouse.SelectedValue))
            //{
            //    this.AjaxAlert("請選擇料品倉儲!!");
            //    return;
            //}

            //if (mappingDetails.Items.Count == 0)
            //{
            //    this.AjaxAlert("請新增一筆料品庫存警示!!");
            //    return;
            //}

            //mappingDetails.UpdateData();
            //if (mappingDetails.Items.GroupBy(p => p.PRODUCTS_SN).Any(g => g.Count() > 1))
            //{
            //    this.AjaxAlert("庫存警示有重複的料品項目!!");
            //    return;
            //}

            try
            {
                //WarehouseManager mgr = new WarehouseManager(dsUpdate.CreateDataManager());
                //modelItem.DataItem = mgr.SaveProductsMapping(_item);
                mappingDetails.UpdateData();

                Page.SetTransferMessage("庫存警示資料維護完成!!");
                Server.Transfer(ToInquireWarehouseMapping.TransferTo);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                this.AjaxAlert("系統發生錯誤,錯誤原因:" + ex.Message);
            }
        }

        protected override void prepareInitialData()
        {
            //Item = dsEntity.CreateDataManager().EntityList.OrderBy(w => w.PRODUCTS_WAREHOUSE_MAPPING.Count).FirstOrDefault();
            Item = new WAREHOUSE { };
            //if (_item == null)
            ////if (Item == null)
            //{
            //    this.AjaxAlert("請先建立倉儲資料!!");
            //    return;
            //}
            mappingDetails.Items = _item.PRODUCTS_WAREHOUSE_MAPPING;
            //mappingDetails.Items = Item.PRODUCTS_WAREHOUSE_MAPPING;
        }

        protected override void prepareDataForViewState()
        {
            if (_item.DataFrom == Naming.DataItemSource.FromDB)
            {
                mappingDetails.Items = _item.PRODUCTS_WAREHOUSE_MAPPING.Where(sn => sn.PW_MAPPING_SN == (int)ViewState["PW_MAPPING_SN"]).ToList();
            }
        }
    }
}