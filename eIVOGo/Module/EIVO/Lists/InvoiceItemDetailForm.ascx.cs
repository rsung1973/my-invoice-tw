using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using eIVOGo.Module.Base;
using Model.DataEntity;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.EIVO.Lists
{
    public partial class InvoiceItemDetailForm : EntityItemList<EIVOEntityDataContext, InvoiceProductItem>
    {
        List<String> ItemNo;
        List<String> ItemName;
        List<String> PieceUnit;
        List<String> UnitCost;
        List<String> Piece;
        List<String> Remark;

        protected String tempNo;
        protected String tempName;
        protected String tempUnit;
        protected String tempUnitePrice;

        public event EventHandler Done;

        public IList<InvoiceProductItem> ItemList
        {
            get
            {
                return (IList<InvoiceProductItem>)modelItem.DataItem;
            }
            set
            {
                modelItem.DataItem = value;
            }
        }

        protected virtual void OnDone(object sender, EventArgs e)
        {
            if (Done != null)
                Done(this, new EventArgs());
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            // 在這裡放置使用者程式碼以初始化網頁
            if (!this.IsPostBack)
            {
                ItemList = null;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(module_form_lcgoodsform_ascx_PreRender);
            //this.gvEntity.ShowFooter = false;
            doCreate.DoAction = arg =>
            {
                getInputValues();
                if (chkInput())
                {
                    saveAll();
                    tempNo = "";
                    tempName = "";
                    tempUnit = "";
                    tempUnitePrice = "";
                    OnDone(this, new EventArgs { });
                }
            };

            doShow.DoAction = arg =>
            {
                this.InvoiceItemTypeList1.Show();
            };

            doDelete.DoAction = arg =>
            {
                getInputValues();                
                int index = int.Parse(arg);
                ItemNo.RemoveAt(index);
                ItemName.RemoveAt(index);
                PieceUnit.RemoveAt(index);
                UnitCost.RemoveAt(index);
                Piece.RemoveAt(index);
                Remark.RemoveAt(index);
                saveAll();
                tempNo = "";
                tempName = "";
                tempUnit = "";
                tempUnitePrice = "";
                OnDone(this, new EventArgs { });
            };

            this.InvoiceItemTypeList1.Done += new EventHandler(InvoiceItemTypeList1_Done);
        }

        void InvoiceItemTypeList1_Done(object sender, EventArgs e)
        {
            attachItem();
        }

        void module_form_lcgoodsform_ascx_PreRender(object sender, EventArgs e)
        {
            this.BindData();
            this.DataBind();
        }

        void removeLast()
        {
            int index = ItemNo.Count - 1;
            if (index >= 0)
            {
                ItemNo.RemoveAt(index);
                ItemName.RemoveAt(index);
                PieceUnit.RemoveAt(index);
                UnitCost.RemoveAt(index);
                Piece.RemoveAt(index);
                //CostAmount.RemoveAt(index);
                Remark.RemoveAt(index);
            }
        }

        void saveAll()
        {
            int idx = 0;
            if (ItemList.Count < ItemNo.Count)
            {
                for (idx = ItemList.Count; idx < ItemNo.Count; idx++)
                {
                    ItemList.Add(new InvoiceProductItem
                    {
                        InvoiceProduct = new InvoiceProduct { Brief = ItemName[idx] },
                        No = Convert.ToInt16(idx + 1)
                    });
                }
            }
            else
            {
                foreach (var detail in ItemList.Where(g => g.No > ItemNo.Count).ToArray())
                {
                    ItemList.Remove(detail);
                }
            }

            for (idx = 0; idx < ItemNo.Count; idx++)
            {
                ItemList[idx].CostAmount = Decimal.Parse(Piece[idx]) * Decimal.Parse(UnitCost[idx]);
                ItemList[idx].ItemNo = ItemNo[idx];
                ItemList[idx].Piece = Decimal.Parse(Piece[idx]);
                ItemList[idx].PieceUnit = PieceUnit[idx];
                ItemList[idx].UnitCost = Decimal.Parse(UnitCost[idx]);
                ItemList[idx].Remark = Remark[idx];
            }
        }

        void getInputValues()
        {
            ItemNo = new List<string>(Request.Form.GetValues("ItemNo"));
            ItemName = new List<string>(Request.Form.GetValues("ItemName"));
            PieceUnit = new List<string>(Request.Form.GetValues("PieceUnit"));
            UnitCost = new List<string>(Request.Form.GetValues("UnitCost"));
            Piece = new List<string>(Request.Form.GetValues("Piece"));
            Remark = new List<string>(Request.Form.GetValues("Remark"));
            if (String.IsNullOrEmpty(ItemNo[ItemNo.Count-1]))
            {
                removeLast();
            }
        }

        void attachItem()
        {
            if (!String.IsNullOrEmpty(InvoiceItemTypeList1.SelectedValue))
            {
                var mgr = this.dsEntity.CreateDataManager().GetTable<ProductItemCategory>().Where(i => i.PICID.Equals(int.Parse(InvoiceItemTypeList1.SelectedValue)));
                tempNo = mgr.FirstOrDefault().ItemNo;
                tempName = mgr.FirstOrDefault().ItemName;
                tempUnit = mgr.FirstOrDefault().Unit;
                tempUnitePrice = mgr.FirstOrDefault().UnitePrice.ToString("#,0.##");
            }
        }

        public override void BindData()
        {
            if (ItemList == null)
            {
                ItemList = new List<InvoiceProductItem>();
            }

            gvEntity.DataSource = ItemList;
            base.BindData();
        }

        bool chkInput()
        {
            for (int i = 0; i < ItemNo.Count; i++)
            {
                if (String.IsNullOrEmpty(ItemNo[i]) & String.IsNullOrEmpty(Piece[i]))
                {
                    return false;
                }
                
                if (String.IsNullOrEmpty(ItemNo[i]) & !String.IsNullOrEmpty(Piece[i]))
                {
                    this.AjaxAlert("請選擇發票品項!!");
                    return false;
                }

                decimal p = 0;
                if (!String.IsNullOrEmpty(ItemNo[i]) & String.IsNullOrEmpty(Piece[i]))
                {
                    this.AjaxAlert("發票明細請輸入數量!!");
                    attachItem();
                    return false;
                }
                else
                {
                    if (!Decimal.TryParse(Piece[i], out p))
                    {
                        this.AjaxAlert("發票明細數量請輸入數字!!");
                        attachItem();
                        return false;
                    }
                    else if (p.Equals(0))
                    {
                        this.AjaxAlert("數量需大於0!!");
                        attachItem();
                        return false;
                    }
                }
            }
            return true;
        }
    }
}