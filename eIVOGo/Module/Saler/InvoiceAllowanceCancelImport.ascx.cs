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
using System.Text.RegularExpressions;
using System.Text;

namespace eIVOGo.Module.Saler
{
    public partial class InvoiceAllowanceCancelImport : System.Web.UI.UserControl
    {
        protected List<InvoiceAllowanceCancels> _queryItems;
        public decimal tax1 = (decimal)0.05;
        public int TotalNum = 0;
        public int YesNum = 0;
        public int NoNum = 0;

        public class InvoiceAllowanceCancels
        {

            public string OrderNo { get; set; }
            public string AllowanceDate { get; set; }
            public string CustomerID { get; set; }
            public string AllowanceNo { get; set; }
            public string InvDate { get; set; }
            public string InvNo { get; set; }
            public string BuyerReceiptNo { get; set; }
            public string BuyerCustomerName { get; set; }
            public string BuyerContactName { get; set; }
            public string BuyerAddress { get; set; }
            public string BuyerPhone { get; set; }
            public string BuyerEMail { get; set; }
            public string Piece { get; set; }
            public string UnitCost { get; set; }
            public string Amount { get; set; }
            public string TAmount { get; set; }
            public string Tax { get; set; }
            public string Remark { get; set; }
            public string Status { get; set; }
            public string Brief { get; set; }

        }

        //public class InvoiceAllowanceImports
        //{
        //    public string OrderNo { get; set; }
        //    public string AllowanceDate { get; set; }
        //    public string CustomerID { get; set; }
        //    public string AllowanceNo { get; set; }
        //    public string InvDate { get; set; }
        //    public string InvNo { get; set; }
        //    public string Piece { get; set; }
        //    public string PieceUnit { get; set; }
        //    public string Amount { get; set; }
        //    public string Tax { get; set; }
        //    public string BuyerReceiptNo { get; set; }
        //    public string BuyerName { get; set; }
        //    public string Remark { get; set; }

        //}


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
            if (!this.IsPostBack)
            {
                Session["InvoiceAllowanceCancels"] = null;
            }
            else
            {
                //InvoiceAllowanceCancels = Session["InvoiceAllowanceCancels"];
                _queryItems = (List<InvoiceAllowanceCancels>)Session["InvoiceAllowanceCancels"];
            }
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
                    Session["InvoiceAllowanceCancels"] = _queryItems;
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

            _queryItems = _queryItems.OrderBy(k => k.OrderNo).ToList();
        }

        public void BindData()
        {
            bindData();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            btnExcel.Visible = false;
            PrintingButton21.Visible = false;
            lblError.Visible = false;
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
                    string FileName = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_InvoiceAllowances_" + _userProfile.PID + ".csv";
                    FileUpload1.PostedFile.SaveAs(Logger.LogDailyPath + "\\" + FileName);
                    _queryItems = new List<InvoiceAllowanceCancels>();
                    showGrid(Logger.LogDailyPath + "\\" + FileName);
                    //  this.BtnOk.Visible = true;

                }
            }
            else
            {
                this.lblError.Text = "請選擇要上傳的檔案!!";
            }

        }

        public List<InvoiceAllowanceCancels> ReadCsvToList(string FileName)
        {
            FileInfo fi = new FileInfo(FileName);
            if (fi.Exists)
            {
                List<InvoiceAllowanceCancels> result = new List<InvoiceAllowanceCancels>();
                using (StreamReader sr = new StreamReader(FileName, System.Text.Encoding.GetEncoding("GB2312")))
                {
                    if (sr.Peek() >= 1) sr.ReadLine();
                    while (sr.Peek() >= 1)
                    {
                        string line = sr.ReadLine();
                        line = Utility.ExtensionMethods.ToTraditional(line);
                        int i = -1;
                        InvoiceAllowanceCancels row = new InvoiceAllowanceCancels();
                        if (line.Split(',').Length == 4)
                        {                           
                            row.AllowanceDate = line.Split(',')[i + 1].Trim();
                            row.AllowanceNo = line.Split(',')[i + 2].Trim();
                            row.BuyerReceiptNo = line.Split(',')[i + 3].Trim();
                            row.Status = "";
                            row.Remark = line.Split(',')[i + 4].Trim();
                            result.Add(row);
                        }
                        else
                        {
                            if (line.Trim().Length > 0)
                            {                                
                                row.Status = "資料少於4個欄位";
                                result.Add(row);
                            }
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
            List<InvoiceAllowanceCancels> MyFirstTable = ReadCsvToList(file);
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
                            InvoiceAllowanceCancels ic = new InvoiceAllowanceCancels();
                            ic = row;

                            ic.Amount = "0";
                            ic.Tax = "0";
                            ic.TAmount = "0";
                            string strRegNo = "";
                            if (ic.Status.Length == 0) //若資料欄位長度正確
                            {
                                #region 欄位內容檢查
                                if ((ic.AllowanceDate == "") || (ValidateString(ic.AllowanceDate, 30) == false))
                                {
                                    ic.Status = ic.Status + "折讓日期格式錯誤;";
                                }

                                if ((ic.AllowanceNo == "") || (ValidateString(ic.AllowanceNo, 14) == false))
                                {
                                    ic.Status = ic.Status + "折讓單號碼格式錯誤;";
                                }
                                if (ic.BuyerReceiptNo != "")
                                {
                                    if (ic.BuyerReceiptNo != "N/A")
                                    {
                                        if (ValidateString(ic.BuyerReceiptNo, 20) == false)
                                            ic.Status = ic.Status + "統編格式錯誤;";
                                        strRegNo = ic.BuyerReceiptNo;
                                    }
                                    else
                                        strRegNo = "0000000000";
                                }
                                else
                                    strRegNo = "0000000000";

                                if (ic.Remark.Trim() == "")
                                {
                                    ic.Status = ic.Status + "作廢原因不得為空白;";
                                }
                                #endregion

                                if (ic.Status.Length == 0) //若資料內容正確
                                {
                                    #region 資料完整性檢查

                                    //判斷折讓單發票是否存在 
                                    var inv = im.GetTable<InvoiceItem>().Where(w => w.InvoiceAllowances.Any(a => a.AllowanceNumber == ic.AllowanceNo.Trim())
                                        && w.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID
                                        && w.InvoiceBuyer.ReceiptNo == strRegNo).FirstOrDefault();
                                    if (inv != null)
                                    {
                                        ic.InvDate = inv.InvoiceDate.Value.ToShortDateString();
                                        ic.InvNo = inv.TrackCode + inv.No;
                                        //判斷是否有重覆滙入的作廢折讓單,同一個檔案內
                                        var importinv = _queryItems.Where(w => w.AllowanceNo == row.AllowanceNo.Trim()).FirstOrDefault();
                                        if (importinv == null)
                                        {
                                            //判斷是否已曾經滙入折讓單
                                            var invallowance = inv.InvoiceAllowances.Where(w => w.AllowanceNumber == row.AllowanceNo.Trim()).FirstOrDefault();
                                            if (invallowance.InvoiceAllowanceCancellation == null)
                                            {
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
                                                    ic.BuyerCustomerName = inv.InvoiceBuyer.CustomerName;
                                                    if (inv.InvoiceBuyer.ReceiptNo != "0000000000")
                                                        ic.BuyerReceiptNo = inv.InvoiceBuyer.ReceiptNo;
                                                    else
                                                        ic.BuyerReceiptNo = "N/A";
                                                }
                                                else
                                                {
                                                    ic.CustomerID = "N/A"; ic.BuyerCustomerName = "N/A"; ic.BuyerReceiptNo = "N/A";
                                                }

                                                var invAllowances = im.GetTable<InvoiceAllowance>().Where(w => w.AllowanceNumber == row.AllowanceNo.Trim()).FirstOrDefault();
                                                ic.Amount = invAllowances.InvoiceAllowanceDetails.FirstOrDefault().InvoiceAllowanceItem.Amount.ToString();
                                                ic.Tax = invAllowances.InvoiceAllowanceDetails.FirstOrDefault().InvoiceAllowanceItem.Tax.ToString();
                                                ic.TAmount = (decimal.Parse(ic.Amount) + decimal.Parse(ic.Tax)).ToString();
                                                ic.Status = "等待滙入";
                                                _queryItems.Add(ic);
                                            }
                                            else
                                            {
                                                ic.Status = "失敗:第" + rowNum.ToString() + "筆作廢折讓單,已經作廢";
                                                _queryItems.Add(ic);
                                                result = false;
                                            }
                                        }
                                        else
                                        {
                                            ic.Status = "失敗:第" + rowNum.ToString() + "筆作廢折讓單,重覆滙入";
                                            _queryItems.Add(ic);
                                            result = false;
                                        }
                                    }
                                    else
                                    {
                                        ic.Status = "失敗:第" + rowNum.ToString() + "筆作廢折讓單不存在或對方統編錯誤";
                                        _queryItems.Add(ic);
                                        result = false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    ic.Status = "失敗:第" + rowNum.ToString() + "筆" + ic.Status;
                                    _queryItems.Add(ic);
                                    result = false;
                                }
                            }
                            else
                            {
                                ic.Status = "失敗:第" + rowNum.ToString() + "筆" + ic.Status;
                                _queryItems.Add(ic);
                                result = false;
                            }

                        }                        
                        if (result == true)
                        {
                            //this.btnOK.Visible = false;
                            //this.FileUpload1.Visible = false;
                            this.ImportButton.Visible = true;
                            this.CannelButton.Visible = true;
                            PrintingButton21.Visible = false;
                            btnExcel.Visible = false;
                            ImportButton.OnClientClick = String.Format("if ( confirm('確定匯入作廢折讓單筆數共計 {0}筆'))   {{ $('#btnPopup').trigger('click'); }} else return false; ", _queryItems.Count().ToString());
                            //ImportButton.Attributes .Add("onClick", "return confirm('本次發票預計作廢"+_queryItems.Count().ToString() +"筆，是否作廢?');");
                        }
                        else
                        {
                            _queryItems.RemoveAll(r => r.Status == "等待滙入");
                            this.btnOK.Visible = true;
                            this.FileUpload1.Visible = true;
                            this.ImportButton.Visible = false;
                            this.CannelButton.Visible = true;
                        }

                        this.divResult.Visible = true;
                        TotalNum = _queryItems.Count();
                        YesNum = _queryItems.Where(r => r.Status == "等待滙入").Count();
                        NoNum = TotalNum - YesNum;
                        BindData();
                        //this.PrintingButton21.Visible = true;
                    }
                }
                else
                {
                    this.lblError.Text = "資料格式有誤或無上傳資料";
                    this.PrintingButton21.Visible = false;
                    this.btnExcel.Visible = false;
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
            Logger.Info("匯入作廢折讓單取消,檔案名稱" + this.ViewState["FileName"].ToString());
            _queryItems.Clear();
            this.PrintingButton21.Visible = false;
            this.divResult.Visible = false;
            this.ImportButton.Visible = false;
            this.CannelButton.Visible = false;
            this.FileUpload1.Visible = true;
            this.btnExcel.Visible = false;
            this.btnOK.Visible = true;
        }

        protected void ImportButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (InvoiceManager im = new InvoiceManager())
                {
                    List<InvoiceAllowance> items = new List<InvoiceAllowance>();
                    foreach (var row in _queryItems)
                    {
                        var inv = im.GetTable<InvoiceAllowance>().Where(w => w.AllowanceNumber == row.AllowanceNo.Trim() && w.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID).FirstOrDefault();

                        //判斷是否為b2c才寄mail
                        //         if (inv.InvoiceBuyer.ReceiptNo == "0000000000")
                        //            InvoiceIDs += inv.InvoiceID.ToString() + ",";
                        if (inv != null)
                        {
                            im.GetTable<InvoiceAllowanceCancellation>().InsertOnSubmit(new InvoiceAllowanceCancellation
                            {
                                AllowanceID = inv.AllowanceID,
                                CancelDate = DateTime.Now,
                                Remark = row.Remark
                            });
                            row.Status = "滙入成功";
                        }
                        InvoiceAllowance invoiceallowance = new InvoiceAllowance();
                    }
                    im.SubmitChanges();
                    this.PrintingButton21.Visible = true;
                    this.lblError.Text = "匯入作廢折讓單成功";
                    TotalNum = _queryItems.Count();
                    YesNum = _queryItems.Where(r => r.Status == "滙入成功").Count();
                    NoNum = TotalNum - YesNum;
                    Logger.Info("匯入作廢折讓單成功,檔案名稱" + this.ViewState["FileName"].ToString());
                    //寄送mail
                    //items.Where(r => r.InvoiceItem.InvoiceBuyer.IsB2C()).Select(a => a.AllowanceID).SendGoogleInvAllowanceCancelMail();
                    this.ImportButton.Visible = false;
                    this.CannelButton.Visible = false;
                    this.FileUpload1.Visible = true;
                    this.btnOK.Visible = true;
                    this.btnExcel.Visible = true;
                    //   this.divResult.Visible = false;
                }
            }
            catch (Exception ex)
            {
                foreach (var row in _queryItems)
                {
                    row.Status = "滙入失敗";
                }
                this.lblError.Text = "系統作業失敗:匯入作廢折讓單存檔失敗,原因:" + ex.Message;
                Logger.Error("系統作業失敗,匯入作廢折讓單存檔失敗原因:" + ex.Message);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string[] StrCumName = { "序號", "GoogleID", "折讓號碼", "作廢原因", "買受人名稱", "買受人統編", "未稅金額", "稅額", "含稅金額", "匯入狀態" };
            ExportDataToCsv(_queryItems, "InvoiceAllowanceCancelFile.csv", StrCumName, true);
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
        public void ExportDataToCsv(List<InvoiceAllowanceCancels> dt, string FileName, string[] ColumnName, bool HasColumnName)
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
                strValue = strValue + dt[i].AllowanceNo + ",";
                strValue = strValue + dt[i].Remark + ",";
                strValue = strValue + dt[i].BuyerCustomerName + ",";
                strValue = strValue + dt[i].BuyerReceiptNo + ",";
                strValue = strValue + dt[i].Amount + ",";
                strValue = strValue + dt[i].Tax + ",";
                strValue = strValue + dt[i].TAmount + ",";
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
                Page.Response.AddHeader("content-disposition", "attachment;filename=InvoiceAllowanceCancelFile.csv");
                Page.Response.Charset = "";
                Page.Response.ContentType = "application/vnd.ms-excel";
                //using (StreamWriter sw = new StreamWriter(Response.OutputStream, Encoding.UTF8))
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

        #region "驗証輸入的字串"
        /// 
        /// 判斷輸入的字串類型。　
        /// 
        /// 輸入的字串。(string) 
        /// 要驗証的類型，可選擇之類型如下列表。(int)         
        /// 驗証通過則傳回 True，反之則為 False。
        public bool ValidateString(String _value, int _kind)
        {
            string RegularExpressions = null;
            CompareValidator cv = new CompareValidator();
            switch (_kind)
            {
                case 1:
                    //由26個英文字母組成的字串
                    RegularExpressions = "^[A-Za-z]+$";
                    break;
                case 2:
                    //正整數 
                    RegularExpressions = "^[0-9]*[1-9][0-9]*$";
                    break;
                case 3:
                    //非負整數（正整數 + 0)
                    RegularExpressions = "^\\d+$";
                    break;
                case 4:
                    //非正整數（負整數 + 0）
                    RegularExpressions = @"^((-\\d+)|(0+))$";
                    break;
                case 5:
                    //負整數 
                    RegularExpressions = @"^-[0-9]*[1-9][0-9]*$";
                    break;
                case 6:
                    //整數
                    RegularExpressions = @"^-?\\d+$";
                    break;
                case 7:
                    //非負浮點數（正浮點數 + 0）
                    RegularExpressions = @"^\\d+(\\.\\d+)?$";
                    break;
                case 8:
                    //正浮點數
                    RegularExpressions = @"^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
                    break;
                case 9:
                    //非正浮點數（負浮點數 + 0）
                    RegularExpressions = @"^((-\\d+(\\.\\d+)?)|(0+(\\.0+)?))$";
                    break;
                case 10:
                    //負浮點數
                    RegularExpressions = @"^(-(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*)))$";
                    break;
                case 11:
                    //浮點數
                    RegularExpressions = @"^(-?\\d+)(\\.\\d+)?$";
                    break;
                case 12:
                    //由26個英文字母的大寫組成的字串
                    RegularExpressions = "^[A-Z]+$";
                    break;
                case 13:
                    //由26個英文字母的小寫組成的字串
                    RegularExpressions = "^[a-z]+$";
                    break;
                case 14:
                    //由數位和26個英文字母組成的字串
                    RegularExpressions = "^[A-Za-z0-9]+$";
                    break;
                case 15:
                    //由數位、26個英文字母或者下劃線組成的字串 
                    RegularExpressions = "^[0-9a-zA-Z_]+$";
                    break;
                case 16:
                    //email地址
                    RegularExpressions = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                    break;
                case 17:
                    //url
                    RegularExpressions = "^[a-zA-z]+://(\\w+(-\\w+)*)(\\.(\\w+(-\\w+)*))*(\\?\\S*)?$";
                    break;
                case 18:
                    //只能輸入中文
                    RegularExpressions = "^[^\u4E00-\u9FA5]";
                    break;
                case 19:
                    //只能輸入0和非0打頭的數字
                    RegularExpressions = "^(0|[1-9][0-9]*)$";
                    break;
                case 20:
                    //只能輸入數字
                    RegularExpressions = "^[0-9]*$";
                    break;
                case 21:
                    //只能輸入數字加2位小數
                    RegularExpressions = "^[0-9]+(.[0-9]{1,2})?$";
                    break;
                case 22:
                    //只能輸入0和非0打頭的數字加2位小數
                    RegularExpressions = "^(0|[1-9]+)(.[0-9]{1,2})?$";
                    break;
                case 23:
                    //只能輸入0和非0打頭的數字加2位小數，但不匹配0.00
                    RegularExpressions = "^(0(.(0[1-9]|[1-9][0-9]))?|[1-9]+(.[0-9]{1,2})?)$";
                    break;
                //case 24:
                //    //驗證日期格式 YYYYMMDD, 範圍19000101~20991231
                //    RegularExpressions = "(19|20)\\d\\d+(0[1-9]|1[012])+(0[1-9]|[12][0-9]|3[01])$";
                //    break;
                //case 25:
                //    //驗證日期格式 MMDDYYYY
                //    RegularExpressions = "(0[1-9]|1[012])+(0[1-9]|[12][0-9]|3[01])+(19|20)\\d\\d$";
                //    break;
                //case 26:
                //    //驗證日期格式 YYYYMM
                //    RegularExpressions = "(19|20)\\d\\d+(0[1-9]|1[012])$";
                //    break;
                //case 27:
                //    //驗證日期格式 YYYYMMDD, 範圍00010101~99991231
                //    RegularExpressions = "(^0000|0001|9999|[0-9]{4})+(0[1-9]|1[0-2])+(0[1-9]|[12][0-9]|3[01])$";
                //    break; 
                case 28: //驗證日期格式YYYY/MM/DD
                    RegularExpressions = "^([2][0]\\d{2}\\/([0]\\d|[1][0-2])\\/([0-2]\\d|[3][0-1]))$|^([2][0]\\d{2}\\/([0]\\d|[1][0-2])\\/([0-2]\\d|[3][0-1]))$";
                    break;
                case 29:  //驗證特殊字元
                    RegularExpressions = "(?=.*[@#$%^&+=])";
                    break;
                case 30: //驗證日期格式YYYY/MM/DD

                    cv.ControlToValidate = _value;
                    cv.Operator = ValidationCompareOperator.DataTypeCheck;
                    cv.Type = ValidationDataType.Date;
                    // RegularExpressions = "^((19|20)[0-9]{2}[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01]))$";
                    break;
                default:
                    break;
            }
            if (_kind < 30)
            {
                Match m = Regex.Match(_value, RegularExpressions);
                if (m.Success)
                    return true;
                else
                    return false;
            }
            else
            {

                return cv.IsValid;
            }
        }
        #endregion

    }
}