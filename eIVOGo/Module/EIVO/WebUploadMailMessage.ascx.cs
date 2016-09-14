using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Utility;
using Model.Locale;
using Uxnet.Web.WebUI;
using Model.DataEntity;
using Model.InvoiceManagement;
using Uxnet.Web.Module.Common;
using System.ComponentModel;
using System.IO;
using System.Text;


namespace eIVOGo.Module.EIVO
{
    public partial class WebUploadMailMessage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void TextChanged(object sender, EventArgs e)
        {
            if (checkInput())
            {
                btnQuery.Visible = true;
            }
            else
            {
                divResult.Visible = false;
            }
        }

        protected List<InvoiceDeliveryTracking> DeliveryList
        {
            get
            {
                if (modelItem.DataItem == null)
                {
                    modelItem.DataItem = new List<InvoiceDeliveryTracking>();
                }
                return (List<InvoiceDeliveryTracking>)modelItem.DataItem;
            }
            set
            {
                modelItem.DataItem = value;
            }
        }

        [Bindable(true)]
        public Naming.InvoiceDeliveryStatus DeliveryStatus
        { get; set; }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                createPreview();
            }
            else
            {
                this.AjaxAlert("請填妥資料");
                divResult.Visible = false;
            }
        }

        private void createPreview()
        {
            var mgr = dsEntity.CreateDataManager();
            {
                #region 處理發票和郵件流水號配對

                //發票
                int invoiceCount = int.Parse(txtMail_Amount.Text);
                String startNo = txtInv_No_begin.Text.Substring(2, 8);
                String endNo = txtInv_No_end.Text.Substring(2, 8);
                String trackCode = txtInv_No_begin.Text.Substring(0, 2);

                //郵局流水號
                int Mail_No_1 = int.Parse(txtMail_No_1.Text);
                //int Mail_No_2 = int.Parse(txtMail_No_2.Text);

                var items = mgr.GetTable<InvoiceItem>().Where(i => i.TrackCode == trackCode
                    && String.Compare(i.No, startNo) >= 0
                    && String.Compare(i.No, endNo) <= 0
                    && i.InvoiceCancellation == null)
                    .Where(i => i.InvoiceBuyer.Address != null && i.InvoiceBuyer.ReceiptNo == "0000000000")
                    .OrderBy(i => i.TrackCode).ThenBy(i => i.No)
                    .Concat(mgr.GetTable<InvoiceItem>().Where(i => i.TrackCode == trackCode
                        && String.Compare(i.No, startNo) >= 0
                        && String.Compare(i.No, endNo) <= 0
                        && i.InvoiceCancellation == null)
                        .Where(i => i.InvoiceBuyer.Address != null && i.InvoiceBuyer.ReceiptNo != "0000000000")
                        .OrderBy(i => i.TrackCode).ThenBy(i => i.No)).ToArray();

                //紀錄筆數
                //int Inv_Count = 1;
                //DeliveryList = new List<InvoiceDeliveryTracking>();
                int q = 0;
                InvoiceItem prevItem = null;
                InvoiceDeliveryTracking indexItem = null;
                InvoiceDeliveryTracking deliveryItem;

                if (items != null && items.Length > 0)
                {
                    prevItem = items[0];
                    deliveryItem = new InvoiceDeliveryTracking()
                    {
                        InvoiceID = items[0].InvoiceID,
                        InvoiceItem = items[0],
                        TrackingNo1 = Mail_No_1.ToString(),
                        TrackingNo2 = txtMail_No_2.Text,
                        DeliveryDate = DeliveryDate.DateTimeValue,
                        DeliveryStatus = (int)DeliveryStatus,
                    };
                    DeliveryList.Add(deliveryItem);
                    indexItem = deliveryItem;

                    for (int i = 1; i < items.Length; i++)
                    {
                        var item = items[i];

                        if (prevItem.InvoiceBuyer.Address.Trim() != item.InvoiceBuyer.Address.Trim()
                            || prevItem.InvoiceBuyer.ContactName != item.InvoiceBuyer.ContactName)
                        {
                            indexItem = null;

                            q++;
                            if (q >= invoiceCount)
                                break;
                        }
                        else
                        {
                            indexItem.DuplicateCount = indexItem.DuplicateCount.HasValue ? indexItem.DuplicateCount + 1 : 2;
                            indexItem.MergedItem = true;
                        }

                        prevItem = item;

                        deliveryItem = new InvoiceDeliveryTracking()
                        {
                            InvoiceID = item.InvoiceID,
                            InvoiceItem = item,
                            TrackingNo1 = (Mail_No_1 + q).ToString(),
                            TrackingNo2 = txtMail_No_2.Text,
                            DeliveryDate = DeliveryDate.DateTimeValue,
                            DeliveryStatus = (int)DeliveryStatus
                        };

                        DeliveryList.Add(deliveryItem);

                        if (indexItem == null)
                            indexItem = deliveryItem;
                        else
                            deliveryItem.MergedItem = true;

                    }                    

                }
                else
                {
                    this.AjaxAlert("無發票資料");
                }


                #endregion 處理發票和郵件流水號配對

                //if (PagingIndex.HasValue)
                //{
                //    DeliveryList.Skip(PagingIndex.Value * gvEntity.PageSize);
                //}

                //if (DeliveryList != null && DeliveryList.Count() > 0)
                //{
                //    if (Action == "預覽")
                //    {
                //        gvEntity.DataSource = DeliveryList;
                //        gvEntity.DataBind();
                //        divResult.Visible = true;
                //        ResultTitle.Visible = true;
                //    }
                //    else if (Action == "確認")
                //    {
                //        mgr.SubmitChanges();
                //        gvEntity.DataSource = DeliveryList;
                //        gvEntity.DataBind();
                //        divResult.Visible = true;
                //        ResultTitle.Visible = true;
                //    }

                //    if (paging != null)
                //    {
                //        paging.RecordCount = DeliveryList.Count();
                //        paging.CurrentPageIndex = gvEntity.PageIndex;
                //        paging.PageSize = gvEntity.PageSize;
                //    }
                //}

                //return Invoice;
            }
        }

        protected bool checkInput()
        {
            //填入資料
            if (String.IsNullOrEmpty(txtInv_No_begin.Text) || String.IsNullOrEmpty(txtMail_Amount.Text) || String.IsNullOrEmpty(txtMail_No_1.Text) || String.IsNullOrEmpty(txtMail_No_2.Text) || String.IsNullOrEmpty(txtInv_No_end.Text))
            {
                return false;
            }

            //發票
            if (txtInv_No_begin.Text.Length != 10 || txtInv_No_end.Text.Length != 10)
            {
                this.AjaxAlert("發票號碼由2碼英文+8碼數字組成，共10碼。");
                return false;
            }

            if (!ValueValidity.ValidateString(txtInv_No_begin.Text.Substring(0, 2), 1) || !ValueValidity.ValidateString(txtInv_No_begin.Text.Substring(2, 8), 20))
            {
                this.AjaxAlert("發票起號由2碼英文+8碼數字組成，共10碼。");
                return false;
            }

            if (!ValueValidity.ValidateString(txtInv_No_end.Text.Substring(0, 2), 1) || !ValueValidity.ValidateString(txtInv_No_end.Text.Substring(2, 8), 20))
            {
                this.AjaxAlert("發票迄號由2碼英文+8碼數字組成，共10碼。");
                return false;
            }

            if (txtInv_No_end.Text.Substring(0, 2) != txtInv_No_begin.Text.Substring(0, 2))
            {
                this.AjaxAlert("發票起、迄號字軌不相同!!");
                return false;
            }

            //郵件張數            
            if (!ValueValidity.ValidateString(txtMail_Amount.Text, 20))
            {
                this.AjaxAlert("郵件張數，請輸入數字。");
                return false;
            }

            //郵件號碼
            if (!ValueValidity.ValidateString(txtMail_No_1.Text, 20) || !ValueValidity.ValidateString(txtMail_No_2.Text, 20))
            {
                this.AjaxAlert("郵件號碼1或郵件號碼2，請輸入數字。");
                return false;
            }

            return true;
        }

        //#region 分頁

        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    this.PreRender += WebUploadMailMessage_PreRender;
        //    gvEntity.Load += gvEntity_Load;
        //}

        //void WebUploadMailMessage_PreRender(object sender, EventArgs e)
        //{
        //    if (gvEntity.BottomPagerRow != null)
        //    {
        //        PagingControl paging = (PagingControl)gvEntity.BottomPagerRow.Cells[0].FindControl("pagingList");

        //        createPreview("", null, paging);
        //    }
        //}

        //void gvEntity_Load(object sender, EventArgs e)
        //{
        //    if (IsPostBack == false && checkInput())
        //    {
        //        createPreview("", null, null);
        //    }

        //    if (IsPostBack == true && checkInput())
        //    {
        //        createPreview("預覽", null, null);
        //    }
        //    gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(gvEntity, 0);
        //}

        protected void PageIndexChanged(object sender, PageChangedEventArgs e)
        {
            gvEntity.PageIndex = e.NewPageIndex;
        }

        //#endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var mgr = dsEntity.CreateDataManager();
            var trackingNo = Request.Form.GetValues("trackingNo");
            var deliveryDate = Request.Form.GetValues("deliveryDate");
            int idx = 0;
            mgr.GetTable<InvoiceDeliveryTracking>().InsertAllOnSubmit(DeliveryList.Select(d => new InvoiceDeliveryTracking
            {
                DeliveryDate = DateTime.ParseExact(deliveryDate[idx],"yyyy/MM/dd",System.Globalization.CultureInfo.CurrentCulture),
                DeliveryStatus = d.DeliveryStatus,
                InvoiceID = d.InvoiceID,
                TrackingNo1 = trackingNo[idx++],
                TrackingNo2 = d.TrackingNo2
            }));
            mgr.SubmitChanges();

            DeliveryList.Clear();
            modelItem.DataItem = null;

            this.AjaxAlert("資料已上傳");
            divResult.Visible = false;
        }
    }
}