using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Utility;
using Model.DataEntity;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Model.Security.MembershipManagement;
using Business.Helper;
using Uxnet.Web.WebUI;
using Model.InvoiceManagement;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Data.OleDb;
using eIVOGo.Module.Common;
using System.Text;

namespace eIVOGo.Module.Saler
{
    public partial class InvoiceCancellationImport : System.Web.UI.UserControl
    {
        protected static List<InvoiceCancellations> _queryItems;
        public decimal tax1 = (decimal)0.05;
        public int TotalNum = 0;
        public int YesNum = 0;
        public int NoNum = 0;
        public class InvoiceCancellations
        {

            public string OrderNo { get; set; }
            public string CustomerID { get; set; }
            public string BuyerReceiptNo { get; set; }
            public string BuyerName { get; set; }
            public decimal SalesAmount { get; set; }
            public decimal TaxAmount { get; set; }
            public decimal invAmount { get; set; }
            public string Remark { get; set; }
            public string Status { get; set; }
            public string No { get; set; }

        }

        public class InvoiceCancellationImports
        {
            public string date { get; set; }
            public string No { get; set; }
            public string Remark { get; set; }
            public string Status { get; set; }

        }


        protected internal Dictionary<String, SortDirection> _sortExpression
        {
            get
            {
                if (ViewState["sort"] == null)
                {
                    ViewState["sort"] = new Dictionary<String, SortDirection>();
                }
                return (Dictionary<String, SortDirection>)ViewState["sort"];
            }
            set
            {
                ViewState["sort"] = value;
            }
        }

        public int? RecordCount
        {
            get
            {
                if (_queryItems == null)
                    return 0;
                else
                    return _queryItems.Count();
            }
        }



        internal GridView DataList
        {
            get
            {
                return gvEntity;
            }
        }

        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            this.lblError.Text = "";

            this.PrintingButton21.btnPrint.Text = "資料列印";
            this.PrintingButton21.btnPrint.CssClass = "btn";

            this.PrintingButton21.PrintControls.Add(this.gvEntity);


        }

        protected override void OnInit(EventArgs e)
        {
            gvEntity.PreRender += new EventHandler(gvEntity_PreRender);
            this.PrintingButton21.BeforeClick += new EventHandler(btnPrint_BeforeClick);
            this.PreRender += new EventHandler(InvoiceFileImport_PreRender);
        }

        void InvoiceFileImport_PreRender(object sender, EventArgs e)
        {
            //btnOK.OnClientClick = String.Format("if (validateFileUpload(document.getElementById('{0}')))    {1};", FileUpload1.ClientID, Page.ClientScript.GetPostBackEventReference(btnOK, ""));
            btnOK.OnClientClick = String.Format("if (validateFileUpload(document.getElementById('{0}'))) {{ $('#btnPopup').trigger('click'); }} else return false; ", FileUpload1.ClientID);


        }

        void btnPrint_BeforeClick(object sender, EventArgs e)
        {
            this.gvEntity.AllowPaging = false;
        }

        #region "Gridview Event"
        void gvEntity_PreRender(object sender, EventArgs e)
        {
            bindData();
            //if(_queryItems .Count () == 0  )
            //    showGrid(this.ViewState ["FileName"].ToString ());
        }
        #endregion

        protected void gvInvoice_Sorting(object sender, GridViewSortEventArgs e)
        {
            _sortExpression.AddSortExpression(e, true);
            bindData();
        }

        protected void bindData()
        {
            try
            {
                buildQueryItem();

                if (this.ViewState["sort"] != null)
                {
                    //_queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[0].SortExpression, b => b.invallowance.AllowanceDate);
                    //_queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[1].SortExpression, b => b.Seller.CompanyName);
                    //_queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[2].SortExpression, b => b.Seller.ReceiptNo);
                    //_queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[3].SortExpression, b => b.invoiceitem.CheckNo);
                    //_queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[4].SortExpression, b => b.invoiceitem.No);
                    //_queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[5].SortExpression, b => b.invallowance.AllowanceNumber);
                    //_queryItems = _sortExpression.QueryOrderBy(_queryItems, gvEntity.Columns[6].SortExpression, b => b.invallowance.TotalAmount);



                }

                if (gvEntity.AllowPaging)
                {
                    gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(gvEntity, 0);
                    //gvEntity.DataSource = _queryItems.Skip(gvEntity.PageSize * gvEntity.PageIndex).Take(gvEntity.PageSize);
                    gvEntity.DataSource = _queryItems;
                    gvEntity.DataBind();

                    gvEntity.SetPageIndex("pagingList", _queryItems.Count());
                }
                else
                {
                    gvEntity.DataSource = _queryItems;
                    gvEntity.DataBind();
                }
                //  this.lblError.Visible = false;
                this.divResult.Visible = true;
                if (_queryItems.Count() == 0)
                {
                    this.lblError.Text = "資料滙入失敗!!";
                }

            }
            catch (Exception ex)
            {
                this.lblError.Text = "系統錯誤:" + ex.Message;
            }
            finally
            {

            }
        }

        protected virtual void buildQueryItem()
        {
            //設定能顯示的發票為登入者的公司作廢發票

            //過濾使用者所填入的條件

            _queryItems = _queryItems.OrderByDescending(k => k.OrderNo).ToList();
        }

        public void BindData()
        {
            bindData();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {

                if (FileUpload1.FileName.Substring(FileUpload1.FileName.Length - 3) != "csv")
                {
                    this.lblError.Text = "上傳檔案格式限csv";
                }
                //else if (FileUpload1.PostedFile.ContentLength > 1024000)
                //{
                //    this.lblError.Text = "上傳檔案太大,請小於1M";
                //}
                else
                {
                    string FileName = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_InvoiceCancellation_" + _userProfile.PID + ".csv";
                    FileUpload1.PostedFile.SaveAs(Logger.LogDailyPath + "\\" + FileName);
                    _queryItems = new List<InvoiceCancellations>();
                    showGrid(Logger.LogDailyPath + "\\" + FileName);
                    //  this.BtnOk.Visible = true;

                }
            }
            else
            {
                this.lblError.Text = "請選擇要上傳的檔案!!";
            }

        }

        public List<InvoiceCancellationImports> ReadCsvToList(string FileName)
        {
            FileInfo fi = new FileInfo(FileName);
            if (fi.Exists)
            {
                List<InvoiceCancellationImports> result = new List<InvoiceCancellationImports>();
                using (StreamReader sr = new StreamReader(FileName, System.Text.Encoding.GetEncoding("GB2312")))
                {
                    if (sr.Peek() >= 1) sr.ReadLine();
                    while (sr.Peek() >= 1)
                    {
                        string line = sr.ReadLine();
                        InvoiceCancellationImports row = new InvoiceCancellationImports();
                        if (line.Trim().Length > 0)
                        {
                            if (line.Split(',').Length >= 3)
                            {
                                row.date = "";
                                row.No = line.Split(',')[1];
                                row.Remark = line.Split(',')[2];
                                row.Status = "";
                            }
                            else
                            {
                                row.date = "";
                                row.No = "";
                                row.Remark = "";
                                row.Status = "資料少於3個欄位";
                            }
                            result.Add(row);
                        }
                    }
                }

                return result;

            }

            else return null;

        }

        protected void showGrid(string file)
        {
            this.ViewState["FileName"] = file;
            var MyFirstTable = ReadCsvToList(file);
            try
            {
                if (MyFirstTable.Count() > 0)
                {
                    bool result = true;
                    TotalNum = MyFirstTable.Count();
                    int rowNum = 0;
                    //List<InvoiceCancellation> newics = new List<InvoiceCancellation>();
                    using (InvoiceManager im = new InvoiceManager())
                    {
                        foreach (var row in MyFirstTable)
                        {
                            rowNum += 1;
                            InvoiceCancellations ic = new InvoiceCancellations();
                            if (row.Status.Length == 0)
                            {
                                //判斷發票是否存在 
                                var inv = im.GetTable<InvoiceItem>().Where(w => w.TrackCode + w.No == row.No.Trim() && w.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID).FirstOrDefault();
                                if (inv != null)
                                {
                                    //判斷是否有重覆滙入的發票,同一個檔案內
                                    var importinv = _queryItems.Where(w => w.No == row.No.Trim()).FirstOrDefault();
                                    if (importinv == null)
                                    {
                                        #region 檢察資料的完整性
                                        //判斷是否已作廢過
                                        if (inv.InvoiceCancellation == null)
                                        {
                                            if (row.Remark.Trim() != "")
                                            {
                                                //InvoiceCancellations ic = new InvoiceCancellations();
                                                ic.No = row.No;
                                                if (inv.InvoicePurchaseOrder != null)
                                                {
                                                    ic.OrderNo = inv.InvoicePurchaseOrder.OrderNo.Substring(0, 11);
                                                }
                                                else
                                                {
                                                    ic.OrderNo = "N/A";
                                                }
                                                if (inv.InvoiceBuyer != null)
                                                {
                                                    ic.CustomerID = inv.InvoiceBuyer.CustomerID;
                                                    ic.BuyerName = inv.InvoiceBuyer.CustomerName;
                                                    if (inv.InvoiceBuyer.ReceiptNo != "0000000000")
                                                        ic.BuyerReceiptNo = inv.InvoiceBuyer.ReceiptNo;
                                                    else
                                                        ic.BuyerReceiptNo = "N/A";
                                                }
                                                else
                                                {
                                                    ic.CustomerID = "N/A"; ic.BuyerName = "N/A"; ic.BuyerReceiptNo = "N/A";
                                                }

                                                if (inv.InvoiceAmountType != null)
                                                {
                                                    ic.invAmount = inv.InvoiceAmountType.TotalAmount.HasValue ? inv.InvoiceAmountType.TotalAmount.Value : 0;
                                                    ic.SalesAmount = inv.InvoiceAmountType.SalesAmount.HasValue ? inv.InvoiceAmountType.SalesAmount.Value : 0;
                                                    ic.TaxAmount = inv.InvoiceAmountType.TaxAmount.HasValue ? inv.InvoiceAmountType.TaxAmount.Value : 0;

                                                }
                                                else
                                                {
                                                    ic.invAmount = 0;
                                                }

                                                ic.Remark = row.Remark.Trim();
                                                ic.Status = "等待滙入";
                                                _queryItems.Add(ic);

                                            }

                                            else
                                            {
                                                //InvoiceCancellations ic = new InvoiceCancellations();
                                                ic.OrderNo = "N/A"; ic.CustomerID = "N/A"; ic.invAmount = 0; ic.BuyerName = "N/A"; ic.BuyerReceiptNo = "N/A";
                                                ic.SalesAmount = 0; ic.TaxAmount = 0;
                                                ic.No = row.No;
                                                ic.Remark = row.Remark.Trim(); ;
                                                ic.Status = "失敗:第" + rowNum.ToString() + "筆發票,作廢原因空白";
                                                _queryItems.Add(ic);
                                                result = false;
                                            }

                                        }

                                        else
                                        {
                                            //InvoiceCancellations ic = new InvoiceCancellations();
                                            ic.OrderNo = "N/A"; ic.CustomerID = "N/A"; ic.invAmount = 0; ic.BuyerName = "N/A"; ic.BuyerReceiptNo = "N/A";
                                            ic.SalesAmount = 0; ic.TaxAmount = 0;
                                            ic.No = row.No;
                                            ic.Remark = row.Remark.Trim();
                                            ic.Status = "失敗:第" + rowNum.ToString() + "筆發票,已重覆作廢";
                                            _queryItems.Add(ic);
                                            result = false;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //InvoiceCancellations ic = new InvoiceCancellations();
                                        ic.OrderNo = "N/A"; ic.CustomerID = "N/A"; ic.invAmount = 0; ic.BuyerName = "N/A"; ic.BuyerReceiptNo = "N/A";
                                        ic.SalesAmount = 0; ic.TaxAmount = 0;
                                        ic.No = row.No;
                                        ic.Remark = row.Remark.Trim();
                                        ic.Status = "失敗:第" + rowNum.ToString() + "筆發票,重覆滙入";
                                        _queryItems.Add(ic);
                                        result = false;
                                    }
                                }
                                else
                                {
                                    //InvoiceCancellations ic = new InvoiceCancellations();
                                    ic.OrderNo = "N/A"; ic.CustomerID = "N/A"; ic.invAmount = 0; ic.BuyerName = "N/A"; ic.BuyerReceiptNo = "N/A";
                                    ic.SalesAmount = 0; ic.TaxAmount = 0;
                                    ic.Remark = row.Remark.Trim();
                                    ic.No = row.No.Trim();
                                    ic.Status = "失敗:第" + rowNum.ToString() + "筆發票,不存在";
                                    _queryItems.Add(ic);
                                    result = false;
                                }
                            }
                            else
                            {
                                //InvoiceCancellations ic = new InvoiceCancellations();
                                ic.OrderNo = "N/A"; ic.CustomerID = "N/A"; ic.invAmount = 0; ic.BuyerName = "N/A"; ic.BuyerReceiptNo = "N/A";
                                ic.SalesAmount = 0; ic.TaxAmount = 0;
                                ic.Remark = row.Remark.Trim();
                                ic.No = row.No.Trim();
                                ic.Status = "失敗:第" + rowNum.ToString() + "筆" + row.Status;
                                _queryItems.Add(ic);
                                result = false;
                            }
                        }
                        if (result == true)
                        {
                            this.btnOK.Visible = true;
                            this.FileUpload1.Visible = true;
                            this.ImportButton.Visible = true;
                            this.CannelButton.Visible = true;
                            this.btnExcel.Visible = false;
                            ImportButton.OnClientClick = String.Format("if ( confirm('確定作廢發票筆數共計 {0}筆'))   {{ $('#btnPopup').trigger('click'); }} else return false; ", _queryItems.Count().ToString());
                        }
                        else
                        {
                            _queryItems.RemoveAll(r => r.Status == "等待滙入");
                            this.btnOK.Visible = true;
                            this.FileUpload1.Visible = true;
                            this.ImportButton.Visible = false;
                            this.CannelButton.Visible = true;
                            this.btnExcel.Visible = false;
                        }
                        this.divResult.Visible = true;
                        TotalNum = _queryItems.Count();
                        YesNum = _queryItems.Where(r => r.Status == "等待滙入").Count();
                        NoNum = TotalNum - YesNum;
                        BindData();
                    }
                }
                else
                {
                    this.lblError.Text = "資料格式有誤或無上傳資料";
                    this.PrintingButton21.Visible = false;
                    this.divResult.Visible = false;
                }

            }
            catch (Exception ex)
            {
                this.lblError.Text = ex.Message;
            }
        }

        protected void CannelButton_Click(object sender, EventArgs e)
        {
            Logger.Info("作廢發票上傳取消,檔案名稱" + this.ViewState["FileName"].ToString());
            _queryItems.Clear();
            this.PrintingButton21.Visible = false;
            this.divResult.Visible = false;
            this.ImportButton.Visible = false;
            this.CannelButton.Visible = false;
            this.FileUpload1.Visible = true;
            this.btnOK.Visible = true;
            this.btnExcel.Visible = false;
        }

        protected void ImportButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (InvoiceManager im = new InvoiceManager())
                {
                    string InvoiceIDs = "";
                    //   List<InvoiceCancellation> newics = new List<InvoiceCancellation>();
                    InvoiceCancellationUpload invUp = new InvoiceCancellationUpload();
                    invUp.FilePath = Logger.LogDailyPath;
                    invUp.UID = _userProfile.UID;
                    invUp.UploadDate = DateTime.Now;
                    foreach (var row in _queryItems)
                    {
                        var inv = im.GetTable<InvoiceItem>().Where(w => w.TrackCode + w.No == row.No.Trim() && w.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID).FirstOrDefault();
                        InvoiceCancellation newic = new InvoiceCancellation();
                        //判斷是否為b2c才寄mail
                        if (inv.InvoiceBuyer.ReceiptNo == "0000000000")
                            InvoiceIDs += inv.InvoiceID.ToString() + ",";
                        newic.InvoiceItem = inv;
                        newic.CancellationNo = row.No;
                        newic.ReturnTaxDocumentNo = "";
                        newic.CancelDate = DateTime.Now;
                        newic.Remark = row.Remark.Trim();
                        im.GetTable<InvoiceCancellation>().InsertOnSubmit(newic);
                        //  newics.Add(newic);
                        InvoiceCancellationUploadList invuplist = new InvoiceCancellationUploadList();
                        invuplist.InvoiceCancellation = newic;
                        invuplist.InvoiceCancellationUpload = invUp;
                        im.GetTable<InvoiceCancellationUploadList>().InsertOnSubmit(invuplist);
                        row.Status = "滙入成功";
                    }
                    im.SubmitChanges();
                    this.PrintingButton21.Visible = true;
                    this.lblError.Text = "作廢發票滙入成功";
                    Logger.Info("作廢發票滙入成功,檔案名稱" + this.ViewState["FileName"].ToString());
                    //寄送mail
                    if (InvoiceIDs.Length > 0)
                        SharedFunction.doSendMaild(new SharedFunction._MailQueryState { allInvoiceID = InvoiceIDs.Substring(0, InvoiceIDs.Length - 1) });
                    TotalNum = _queryItems.Count();
                    YesNum = _queryItems.Where(r => r.Status == "滙入成功").Count();
                    NoNum = TotalNum - YesNum;
                    this.ImportButton.Visible = false;
                    this.CannelButton.Visible = false;
                    this.FileUpload1.Visible = true;
                    this.btnExcel.Visible = true;
                    this.btnOK.Visible = true;
                    //   this.divResult.Visible = false;
                }
            }
            catch (Exception ex)
            {
                foreach (var row in _queryItems)
                {
                    row.Status = "滙入失敗";
                }
                this.lblError.Text = "系統作業失敗:發票作廢存檔失敗,原因:" + ex.Message;
                Logger.Error("系統作業失敗,發票作廢存檔失敗原因:" + ex.Message);
            }


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string[] StrCumName = { "序號", "GoogleID", "發票號碼", "作廢原因", "買受人名稱", "買受人統編", "未稅金額", "稅額", "含稅金額", "匯入狀態" };
            ExportDataToCsv(_queryItems, "InvoiceCancelFile.csv", StrCumName, true);
            //Page.SaveControlAsDownload(DIV2, "InvoiceFile.xls");
        }

        #region 匯出csv檔案
        /// <summary>
        /// Exports the data table to CSV.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="ColumnName">Name of the column.</param>
        /// <param name="HasColumnName">if set to <c>true</c> [has column name].</param>
        public void ExportDataToCsv(List<InvoiceCancellations> dt, string FileName, string[] ColumnName, bool HasColumnName)
        {
            string strValue = string.Empty;
            //CSV 匯出的標題 要先塞一樣的格式字串 充當標題
            if (HasColumnName == true)
                strValue = string.Join(",", ColumnName);
            for (int i = 0; i < dt.Count; i++)
            {
                strValue = strValue + Environment.NewLine;
                strValue = strValue + dt[i].OrderNo + ",";
                strValue = strValue + dt[i].CustomerID + ",";
                strValue = strValue + dt[i].No + ",";
                strValue = strValue + dt[i].Remark + ",";
                strValue = strValue + dt[i].BuyerName + ",";
                strValue = strValue + dt[i].BuyerReceiptNo + ",";
                strValue = strValue + dt[i].SalesAmount + ",";
                strValue = strValue + dt[i].TaxAmount + ",";
                strValue = strValue + dt[i].invAmount + ",";
                strValue = strValue + dt[i].Status;
            }
            //存成檔案
            string strFile = FileName;
            if (!string.IsNullOrEmpty(strValue))
            {
                strValue = strValue.ToSimplified();
                //File.WriteAllText(strFile, strValue, Encoding.Default);
                Page.Response.Clear();
                Page.Response.Buffer = true;
                Page.Response.AddHeader("content-disposition", "attachment;filename=InvoiceCancelFile.csv");
                Page.Response.Charset = "";
                Page.Response.ContentType = "application/vnd.ms-excel";
                using (StreamWriter sw = new StreamWriter(Response.OutputStream, Encoding.GetEncoding("GB2312")))
                {
                    sw.Write(strValue);
                    sw.Flush();
                }
                //System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                //rpList.RenderControl(htmlWrite);

                //Page.Response.Write(strValue);                
                //Page.Response.Write(stringWrite.ToString());
                //Page.Response.Write(stringWrite.ToString());
                Page.Response.End();
            }
        }
        #endregion


    }
}