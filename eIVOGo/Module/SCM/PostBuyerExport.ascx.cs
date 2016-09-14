using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Locale;
using Model.Security.MembershipManagement;
using Model.InvoiceManagement;
using Business.Helper;
using Model.SCMDataEntity;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Utility;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using System.IO;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SCM
{
    public partial class PostBuyerExport : System.Web.UI.UserControl, IPostBackEventHandler
    {
        private List<_QueryItem> _items;
        UserProfileMember _userProfile;

        public class _QueryItem
        {
            public String date;
            public String shipmentNO;
            public String buyer;
            public String buyerAddr;
            public String buyerPhone;
            public String buyerEmail;
            public String invoiceNO;
            public String marketResource;
            public String deliveryCompany;
            public String Type;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        protected override void OnInit(EventArgs e)
        {
            this.PreRender+=new EventHandler(PostBuyerExport_PreRender);
            this.dsEntity.Select+=new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<CDS_Document>>(dsEntity_Select);
            doPrintOrder.DoAction = arg =>
            {
                printShipment.Show(int.Parse(arg));
            };
            doPrintInvoice.DoAction = arg =>
            {
                printInvoice.Show(int.Parse(arg));
            };
        }

        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<CDS_Document> e)
        {
            if (!String.IsNullOrEmpty(this.btnQuery.CommandArgument))
            {
                var mgr = this.dsEntity.CreateDataManager();

                IQueryable<CDS_Document> bs = mgr.EntityList.Where(c => (c.DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder | c.DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods) && c.BUYER_SHIPMENT != null);

                if (!String.IsNullOrEmpty(this.orderNo.Text))
                {
                    bs = bs.Where(b => b.BUYER_SHIPMENT.BUYER_SHIPMENT_NUMBER == this.orderNo.Text);
                }

                if (!String.IsNullOrEmpty(delivery.SelectedValue))
                {
                    int sn = int.Parse(delivery.SelectedValue);
                    bs = bs.Where(b => b.BUYER_SHIPMENT.DELIVERY_COMPANY_SN == sn);
                }

                if (!String.IsNullOrEmpty(marketResource.SelectedValue))
                {
                    int marketSN = int.Parse(marketResource.SelectedValue);
                    bs = bs.Where(b => b.BUYER_ORDERS.MARKET_RESOURCE_SN == marketSN || b.GOODS_RETURNED.BUYER_ORDERS.MARKET_RESOURCE_SN == marketSN);
                }

                if (this.rdbExportStatus.SelectedIndex != 0)
                {
                    bs = bs.Where(b => b.BUYER_SHIPMENT.POST_PRINT_STATUS == (this.rdbExportStatus.SelectedIndex == 1 ? 0 : 1));
                }

                if (this.DateFrom.HasValue)
                {
                    bs = bs.Where(b => b.BUYER_SHIPMENT.SHIPMENT_DATETIME >= this.DateFrom.DateTimeValue);
                }
                if (this.DateTo.HasValue)
                {
                    bs = bs.Where(b => b.BUYER_SHIPMENT.SHIPMENT_DATETIME < this.DateTo.DateTimeValue.AddDays(1));
                }

                e.Query = bs;
                this.lblRowCount.Text = bs.Count().ToString();
            }
            else
            {
                e.QueryExpr = p => false;
            }
        }

        #region "Button Event"
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.divResult.Visible = true;
            this.btnQuery.CommandArgument = "Query";
            this.gvEntity.DataBind();
            if (this.gvEntity.Rows.Count > 0)
            {
                this.btnExport.Visible = true;
                this.countTable.Visible = true;
                this.lblError.Visible = false;
            }
            else
            {
                this.lblError.Text = "查無資料!!";
                this.lblError.Visible = true;
                this.countTable.Visible = false;
                this.btnExport.Visible = false;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string[] ids = Request.Form.GetValues("chkItem");
            if (ids != null && ids.Count() > 0)
            {
                var mgr = this.dsEntity.CreateDataManager();
                _items = new List<_QueryItem>();
                foreach (string id in ids)
                {
                    var data = mgr.EntityList.Where(c => c.DocID == int.Parse(id)).FirstOrDefault();                    
                    _items.Add(new _QueryItem
                    {
                        date = ValueValidity.ConvertChineseDateString(data.BUYER_SHIPMENT.SHIPMENT_DATETIME),
                        shipmentNO = data.BUYER_SHIPMENT.BUYER_SHIPMENT_NUMBER,
                        buyer = (data.DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? data.BUYER_ORDERS.BUYER_DATA.BUYER_NAME : data.DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? data.EXCHANGE_GOODS.BUYER_ORDERS.BUYER_DATA.BUYER_NAME : ""),
                        buyerAddr = (data.DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? data.BUYER_ORDERS.BUYER_DATA.BUYER_ADDRESS : data.DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? data.EXCHANGE_GOODS.BUYER_ORDERS.BUYER_DATA.BUYER_ADDRESS : ""),
                        buyerPhone = (data.DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? data.BUYER_ORDERS.BUYER_DATA.BUYER_PHONE : data.DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? data.EXCHANGE_GOODS.BUYER_ORDERS.BUYER_DATA.BUYER_PHONE : ""),
                        buyerEmail = (data.DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? data.BUYER_ORDERS.BUYER_DATA.BUYER_EMAIL : data.DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? data.EXCHANGE_GOODS.BUYER_ORDERS.BUYER_DATA.BUYER_EMAIL : ""),
                        invoiceNO = (data.BUYER_SHIPMENT.INVOICE_SN.HasValue ? data.BUYER_SHIPMENT.InvoiceItem.TrackCode + data.BUYER_SHIPMENT.InvoiceItem.No : ""),
                        marketResource = (data.DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? data.BUYER_ORDERS.MARKET_RESOURCE.MARKET_RESOURCE_NAME : data.DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? data.EXCHANGE_GOODS.BUYER_ORDERS.MARKET_RESOURCE.MARKET_RESOURCE_NAME : ""),
                        deliveryCompany = data.BUYER_SHIPMENT.DELIVERY_COMPANY.DELIVERY_COMPANY_NAME,
                        Type = (data.DocType == (int)Naming.DocumentTypeDefinition.BuyerOrder ? "訂單" : data.DocType == (int)Naming.DocumentTypeDefinition.OrderExchangeGoods ? "換貨單" : "")
                    });
                    data.BUYER_SHIPMENT.POST_PRINT_STATUS = 1;
                }
                mgr.SubmitChanges();
                OutExcelFile();
                this.gvEntity.DataBind();
            }
            else
            {
                this.AjaxAlert("請選擇匯出資料!!");
            }       
        }
        #endregion

        #region "Gridview Event"
        protected void PageIndexChanged(object source, PageChangedEventArgs e)
        {
            gvEntity.PageIndex = e.NewPageIndex;
            gvEntity.DataBind();
        }

        void PostBuyerExport_PreRender(object sender, EventArgs e)
        {
            if (gvEntity.BottomPagerRow != null)
            {
                PagingControl paging = (PagingControl)gvEntity.BottomPagerRow.Cells[0].FindControl("pagingList");
                paging.RecordCount = this.dsEntity.CurrentView.LastSelectArguments.TotalRowCount;
                paging.CurrentPageIndex = gvEntity.PageIndex;
                this.lblTotalSum.Text = paging.PageCount.ToString();
            }
        }
        #endregion

        #region IPostBackEventHandler Members
        public void RaisePostBackEvent(string eventArgument)
        {
             if (eventArgument.StartsWith("P:"))
            {

            }
        }
        #endregion

        #region "Create Excel Files"
        private void OutExcelFile()
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            try
            {
                // 新增試算表。
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("My Sheet");

                Font font1 = workbook.CreateFont();
                font1.Color = HSSFColor.WHITE.index;

                // 建立儲存格樣式。
                HSSFCellStyle headerStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                headerStyle.FillForegroundColor = HSSFColor.ORANGE.index;
                headerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
                headerStyle.SetFont(font1);
                headerStyle.BorderBottom = CellBorderType.THIN;
                headerStyle.BorderLeft = CellBorderType.THIN;
                headerStyle.BorderRight = CellBorderType.THIN;
                headerStyle.BorderTop = CellBorderType.THIN;
                headerStyle.Alignment = HorizontalAlignment.CENTER;

                HSSFCellStyle dataStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                dataStyle.BorderBottom = CellBorderType.THIN;
                dataStyle.BorderLeft = CellBorderType.THIN;
                dataStyle.BorderRight = CellBorderType.THIN;
                dataStyle.BorderTop = CellBorderType.THIN;
                dataStyle.Alignment = HorizontalAlignment.CENTER;

                int i =1;
                SetRow(sheet, headerStyle, 0, "日期", "出貨單號碼", "買受人名稱", "地址", "電話", "電子郵件", "發票號碼", "購物平台", "宅配公司", "轉入單據種類");
                foreach (var row in _items)
                {
                    SetRow(sheet, dataStyle, i, row.date, row.shipmentNO, row.buyer, row.buyerAddr, row.buyerPhone, row.buyerEmail, row.invoiceNO, row.marketResource, row.deliveryCompany, row.Type);
                    i++;
                }

                workbook.Write(ms);
                Page.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=PostBuyer.xls"));
                Page.Response.BinaryWrite(ms.ToArray());
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                workbook = null;
                ms.Close();
                ms.Dispose();
            }
        }

        /// 建立Excel欄位並塞入欄位值 
        /// </summary> 
        /// 
        static void SetRow(HSSFSheet sheet, HSSFCellStyle xlStyle, int intRow, params string[] values)
        {
            HSSFRow hsRow = (HSSFRow)sheet.CreateRow(intRow);
            for (int x = 0; x < values.Length; x++)
            {
                hsRow.CreateCell(x);
                hsRow.Cells[x].SetCellValue(values[x]);
                hsRow.Cells[x].CellStyle = xlStyle;
            }
        }
        #endregion
    }    
}
