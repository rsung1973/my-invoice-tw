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

namespace eIVOGo.Module.EIVO
{
    public partial class WebUploadReturnMailMessage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void TextChanged(object sender, EventArgs e)
        {
            if (chkData())
            {
                btnQuery.Visible = true;
            }
            else
            {
                divResult.Visible = false;
            }
        }

        protected InvoiceManager mgr;
        protected List<InvoiceDeliveryTracking> Inv_List;

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (chkData())
            {
                CREATE_Inv_Data("預覽", null, null);
            }
            else
            {
                this.AjaxAlert("請填妥資料");
                divResult.Visible = false;
            }
        }

        private InvoiceItem CREATE_Inv_Data(String Action, int? PagingIndex, PagingControl paging)
        {
            using (mgr = new InvoiceManager())
            {
                #region 處理發票和郵件流水號配對

                //發票
                int Inv_Q = int.Parse(txtMail_Amount.Text);
                int Inv_No = int.Parse(txtInv_No_begin.Text.Substring(2, 8));
                String Inv_no = "";
                InvoiceItem Invoice = null;

                //郵局流水號
                int Mail_No_1 = int.Parse(txtMail_No_1.Text);
                int Mail_No_2 = int.Parse(txtMail_No_2.Text);

                //紀錄筆數
                int Inv_Count = 1;
                Inv_List = new List<InvoiceDeliveryTracking>();
                for (int q = 0; q < Inv_Q; q++)
                {
                    Inv_no = String.Format("{0:0000000#}", Inv_No + q);
                    Invoice = mgr.GetTable<InvoiceItem>().Where(i => i.TrackCode == txtInv_No_begin.Text.Substring(0, 2) && i.No == Inv_no && i.InvoiceCancellation == null).FirstOrDefault();

                    if (Invoice != null)
                    {
                        InvoiceDeliveryTracking Inv_DT = new InvoiceDeliveryTracking()
                        {
                            InvoiceID = Invoice.InvoiceID,
                            TrackingNo1 = Mail_No_1.ToString(),
                            TrackingNo2 = Mail_No_2.ToString(),
                            DeliveryDate = DateTime.Now,
                            DeliveryStatus = (int)Naming.InvoiceDeliveryStatus.申請退回,
                        };
                        Invoice.InvoiceDeliveryTracking.Add(Inv_DT);

                        Mail_No_1 = Mail_No_1 + Inv_Count;
                        Inv_List.Add(Inv_DT);
                    }
                }

                #endregion 處理發票和郵件流水號配對

                if (PagingIndex.HasValue)
                {
                    Inv_List.Skip(PagingIndex.Value * gvEntity.PageSize);
                }

                if (Inv_List != null && Inv_List.Count() > 0)
                {
                    if (Action == "預覽")
                    {
                        gvEntity.DataSource = Inv_List;
                        gvEntity.DataBind();
                        divResult.Visible = true;
                        ResultTitle.Visible = true;
                    }
                    else if (Action == "確認")
                    {
                        mgr.SubmitChanges();
                        gvEntity.DataSource = Inv_List;
                        gvEntity.DataBind();
                        divResult.Visible = true;
                        ResultTitle.Visible = true;
                    }

                    if (paging != null)
                    {
                        paging.RecordCount = Inv_List.Count();
                        paging.CurrentPageIndex = gvEntity.PageIndex;
                        paging.PageSize = gvEntity.PageSize;
                    }
                }

                return Invoice;
            }
        }

        protected bool chkData()
        {
            //填入資料
            if (String.IsNullOrEmpty(txtInv_No_begin.Text) || String.IsNullOrEmpty(txtMail_Amount.Text) || String.IsNullOrEmpty(txtMail_No_1.Text) || String.IsNullOrEmpty(txtMail_No_2.Text))
            {
                return false;
            }

            //發票
            if (txtInv_No_begin.Text.Length != 10)
            {
                this.AjaxAlert("發票號碼由2碼英文+8碼數字組成，共10碼。");
                return false;
            }

            if (!ValueValidity.ValidateString(txtInv_No_begin.Text.Substring(0, 2), 1) || !ValueValidity.ValidateString(txtInv_No_begin.Text.Substring(2, 8), 20))
            {
                this.AjaxAlert("發票號碼由2碼英文+8碼數字組成，共10碼。");
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

        #region 分頁

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += WebUploadMailMessage_PreRender;
            gvEntity.Load += gvEntity_Load;
        }

        void WebUploadMailMessage_PreRender(object sender, EventArgs e)
        {
            if (gvEntity.BottomPagerRow != null)
            {
                PagingControl paging = (PagingControl)gvEntity.BottomPagerRow.Cells[0].FindControl("pagingList");

                CREATE_Inv_Data("", null, paging);
            }
        }

        void gvEntity_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false && chkData())
            {
                CREATE_Inv_Data("", null, null);
            }

            if (IsPostBack == true && chkData())
            {
                CREATE_Inv_Data("預覽", null, null);
            }
            gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(gvEntity, 0);
        }

        protected void PageIndexChanged(object sender, PageChangedEventArgs e)
        {
            gvEntity.PageIndex = e.NewPageIndex;
            CREATE_Inv_Data("預覽", e.NewPageIndex, null);
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (chkData())
            {
                CREATE_Inv_Data("確認", null, null);
                this.AjaxAlert("資料已上傳");
                divResult.Visible = false;
            }
            else
            {
                this.AjaxAlert("請填妥資料");
                divResult.Visible = false;
            }
        }
    }
}