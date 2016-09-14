using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;
using Model.SCMDataEntity;
using Model.InvoiceManagement;
using Uxnet.Com.Utility;
using Utility;
using Business.Helper;
using Model.Security.MembershipManagement;
using System.Linq.Expressions;
using Model.Locale;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using System.IO;

namespace eIVOGo.Module.EIVO
{
    public partial class PupopPurchaseOrderDetail : System.Web.UI.UserControl
    {
        private List<_QueryItem> _items;
        protected UserProfileMember _userProfile;

        public class _QueryItem
        {
            public String SUPPLIER_PRODUCTS_NUMBER;
            public String PRODUCTS_BARCODE;
            public String PRODUCTS_NAME;
            public String PO_QUANTITY;
        }

        public AjaxControlToolkit.ModalPopupExtender Popup
        {
            get { return this.ModalPopupExtender; }
        }

        string id = "";

        public string setDetail
        {
            set
            {
                id = value;
                getPOData(id);
                ViewState["id"] = id;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (!this.Page.IsPostBack)
            {
                this.PrintingButton21.btnPrint.Text = "列印供應商採購單";
                this.PrintingButton21.btnPrint.CssClass = "btn";
            }
            this.PrintingButton21.PrintControls.Add(this.printArea);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PrintingButton21.BeforeClick += new EventHandler(PrintingButton21_BeforeClick);
        }

        void PrintingButton21_BeforeClick(object sender, EventArgs e)
        {
            getPOData(ViewState["id"].ToString());
        }

        private void getPOData(string id)
        {
            try
            {
                var mgr = this.dsPurchase.CreateDataManager();
                IQueryable<PURCHASE_ORDER> po = mgr.EntityList.Where(p => p.PO_DELETE_STATUS == 0 && p.PURCHASE_ORDER_SN == int.Parse(id));
                this.lblPONO.Text = po.FirstOrDefault().PURCHASE_ORDER_NUMBER;
                this.lblWardhouse.Text = po.FirstOrDefault().WAREHOUSE.WAREHOUSE_NAME;
                this.lblCloseType.Text = po.FirstOrDefault().PO_CLOSED_MODE == 0 ? "正常足量入庫結案" : "特許未足量入庫結案";
                this.gvSupplier.DataSource = this.dsSupplier.CreateDataManager().EntityList.Where(s => s.SUPPLIER_SN == po.FirstOrDefault().SUPPLIER_SN);
                this.gvSupplier.DataBind();
                this.PODetailsPreview.PURCHASE_ORDER_SN = po.FirstOrDefault().PURCHASE_ORDER_SN;
                this.PODetailsPreview.BindData();

                if (po.FirstOrDefault().CDS_Document.CurrentStep == (int)Naming.DocumentStepDefinition.已刪除)
                {
                    this.PrintingButton21.Visible = false;
                    this.btnExport.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var pod = this.dsPurchase.CreateDataManager().GetTable<PURCHASE_ORDER_DETAILS>().Where(p => p.PURCHASE_ORDER_SN == int.Parse(ViewState["id"].ToString())).FirstOrDefault();
            _items = new List<_QueryItem>();
            _items.Add(new _QueryItem
            {
                SUPPLIER_PRODUCTS_NUMBER = pod.SUPPLIER_PRODUCTS_NUMBER.SUPPLIER_PRODUCTS_NUMBER1,
                PRODUCTS_BARCODE = pod.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_BARCODE,
                PRODUCTS_NAME = pod.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_NAME,
                PO_QUANTITY = pod.PO_QUANTITY.ToString()
            });
            OutExcelFile();
        }

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

                int i = 1;
                SetRow(sheet, headerStyle, 0, "供應商貨號", "料品Barcode", "料品名稱", "數量");
                foreach (var row in _items)
                {
                    SetRow(sheet, dataStyle, i, row.SUPPLIER_PRODUCTS_NUMBER, row.PRODUCTS_BARCODE, row.PRODUCTS_NAME, row.PO_QUANTITY);
                    i++;
                }

                workbook.Write(ms);
                Page.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=PurchaseOrderList.xls"));
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