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
    public partial class InvoiceAllowanceImport : System.Web.UI.UserControl
    {
        protected List<InvoiceAllowances> _queryItems;
        protected List<InvoiceAllowances> InvoAllowancesData = new List<InvoiceAllowances>();
        public decimal tax1 = (decimal)0.05;
        public int TotalNum = 0;
        public int YesNum = 0;
        public int NoNum = 0;

        public class InvoiceAllowances
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
                Session["InvoAllowancesData"] = null;
            }
            else
            {
                //InvoAllowancesData = Session["InvoAllowancesData"];
                _queryItems = (List<InvoiceAllowances>)Session["InvoAllowancesData"];
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
                    Session["InvoAllowancesData"] = null;
                    gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(gvEntity, 0);
                    //gvEntity.DataSource = _queryItems.Skip(gvEntity.PageSize * gvEntity.PageIndex).Take(gvEntity.PageSize);
                    gvEntity.DataSource = _queryItems;
                    gvEntity.DataBind();
                    Session["InvoAllowancesData"] = _queryItems;
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

        //public void BindData()
        //{
        //    bindData();
        //}

        //匯入CSV檔
        protected void btnOK_Click(object sender, EventArgs e)  
        {
            PrintingButton21.Visible = false;
            btnExcel.Visible = false;
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
                    _queryItems = new List<InvoiceAllowances>();
                    showGrid(Logger.LogDailyPath + "\\" + FileName);
                    //  this.BtnOk.Visible = true;
                }
            }
            else
            {
                this.lblError.Text = "請選擇要上傳的檔案!!";
            }

        }

        public List<InvoiceAllowances> ReadCsvToList(string FileName)
        {
            FileInfo fi = new FileInfo(FileName);
            if (fi.Exists)
            {
                List<InvoiceAllowances> result = new List<InvoiceAllowances>();
                using (StreamReader sr = new StreamReader(FileName, System.Text.Encoding.GetEncoding("GB2312")))
                {
                    if (sr.Peek() >= 1) sr.ReadLine();
                    while (sr.Peek() >= 1)
                    {
                        string line = sr.ReadLine();
                        line = Utility.ExtensionMethods.ToTraditional(line);
                        int i = -1;
                        InvoiceAllowances row = new InvoiceAllowances();
                        if (line.Split(',').Length >= 18)
                        {                            
                            row.OrderNo = line.Split(',')[i + 1].Trim();
                            row.AllowanceDate = line.Split(',')[i + 2].Trim();
                            row.CustomerID = line.Split(',')[i + 3].Trim();
                            row.AllowanceNo = line.Split(',')[i + 4].Trim();
                            row.InvDate = line.Split(',')[i + 5].Trim();
                            row.InvNo = line.Split(',')[i + 6].Trim();
                            row.Brief = line.Split(',')[i + 7].Trim();
                            row.UnitCost = line.Split(',')[i + 8].Trim();
                            row.Piece = line.Split(',')[i + 9].Trim();
                            row.Amount = line.Split(',')[i + 10].Trim();
                            if (line.Split(',')[i + 11].Trim() == "")
                                row.BuyerReceiptNo = "N/A";
                            else
                                row.BuyerReceiptNo = line.Split(',')[i + 11].Trim();

                            row.TAmount = line.Split(',')[i + 13].Trim();
                            if (row.BuyerReceiptNo == "N/A")   //B2C時計算金額與稅額
                            {
                                decimal sum;
                                sum = (decimal.Parse(row.TAmount) / decimal.Parse("1.05"));
                                sum = decimal.Round(sum, 0, MidpointRounding.AwayFromZero);
                                row.Amount = sum.ToString();
                                row.Tax = (int.Parse(row.TAmount) - sum).ToString();
                            }
                            else
                            {
                                row.Tax = line.Split(',')[i + 12].Trim();
                                row.TAmount = line.Split(',')[i + 13].Trim();
                            }
                            row.BuyerCustomerName = line.Split(',')[i + 14].Trim();
                            row.BuyerContactName = line.Split(',')[i + 15].Trim();
                            row.BuyerAddress = line.Split(',')[i + 16].Trim();
                            row.BuyerPhone = line.Split(',')[i + 17].Trim();
                            row.BuyerEMail = line.Split(',')[i + 18].Trim();
                            row.Remark = "";
                            row.Status = "";
                            result.Add(row);
                        }
                        else
                        {
                            if (line.Trim().Length > 0)
                            {
                                //this.lblError.Text = "上傳的檔案格式不合!!";
                                row.Tax = "0";
                                row.TAmount = "0";
                                row.Amount = "0";
                                row.Status = "資料少於18個欄位";
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
            List<InvoiceAllowances> MyFirstTable = ReadCsvToList(file);
            //_queryItems = MyFirstTable;
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
                            bool bolCheck = true; //如果googleid,發票日期,發票號碼,對方統編四個攔位都正確
                            InvoiceAllowances ic = new InvoiceAllowances();
                            ic = row;
                            if (row.Status == "")
                            {
                                #region 第一~六欄檢查
                                if (ic.OrderNo.Length < 1)
                                {
                                    ic.Status = "序號格式錯誤;";
                                }

                                if ((ic.AllowanceDate == "") || (ValidateString(ic.AllowanceDate, 30) == false))
                                {
                                    ic.Status = ic.Status + "折讓日期格式錯誤;";
                                }
                                if ((ic.CustomerID == "") || (ValidateString(ic.CustomerID, 20) == false))
                                {
                                    ic.Status = ic.Status + "GoogleID格式錯誤;";
                                    bolCheck = false;
                                }
                                if ((ic.AllowanceNo == "") || (ValidateString(ic.AllowanceNo, 14) == false))
                                {
                                    ic.Status = ic.Status + "折讓單號碼格式錯誤;";
                                }
                                if ((ic.InvNo == "") || (ValidateString(ic.InvNo, 14) == false))
                                {
                                    ic.Status = ic.Status + "發票號碼格式錯誤;";
                                    bolCheck = false;
                                }
                                if ((ic.InvDate == "") || (ValidateString(ic.InvDate, 30) == false))
                                {
                                    ic.Status = ic.Status + "發票日期格式錯誤;";
                                    bolCheck = false;
                                }
                                #endregion

                                #region 第7-13欄檢查
                                if (ic.Brief == "")
                                {
                                    ic.Status = ic.Status + "品項不得為空白;";
                                }
                                if ((ic.UnitCost == "") || (ValidateString(ic.UnitCost, 20) == false))
                                {
                                    ic.Status = ic.Status + "單價格式錯誤;";
                                }
                                if ((ic.Piece == "") || (ValidateString(ic.Piece, 20) == false))
                                {
                                    ic.Status = ic.Status + "數量格式錯誤;";
                                }

                                if (ic.Amount != "")
                                {
                                    if (ValidateString(ic.Amount, 20) == false)
                                        ic.Status = ic.Status + "金額格式錯誤;";
                                }
                                else
                                {
                                    if (row.BuyerReceiptNo != "N/A")
                                        ic.Status = ic.Status + "金額格式錯誤;";
                                }

                                if (ic.BuyerReceiptNo != "")
                                {
                                    if (ic.BuyerReceiptNo != "N/A")
                                        if (ValidateString(ic.BuyerReceiptNo, 20) == false)
                                        {
                                            ic.Status = ic.Status + "統編格式錯誤;";
                                            bolCheck = false;
                                        }
                                }
                                if (ic.Tax != "")
                                {
                                    if (ValidateString(ic.Tax, 20) == false)
                                        ic.Status = ic.Status + "稅額格式錯誤;";
                                }
                                if (ic.TAmount != "")
                                {
                                    if (ValidateString(ic.TAmount, 20) == false)
                                        ic.Status = ic.Status + "含稅金額格式錯誤;";
                                }
                                #endregion

                                #region 第14~18欄檢查

                                if (ic.BuyerCustomerName == "")
                                {
                                    ic.Status = ic.Status + "名稱不得為空白;";
                                }
                                if (ic.BuyerContactName == "")
                                {
                                    ic.Status = ic.Status + "聯絡人不得為空白;";
                                }
                                if (ic.BuyerAddress == "")
                                {
                                    ic.Status = ic.Status + "地址不得為空白;";
                                }
                                if (ic.BuyerPhone == "")
                                {
                                    ic.Status = ic.Status + "連絡電話不得為空白;";
                                }
                                if ((ic.BuyerEMail == "") || (ValidateString(ic.BuyerEMail, 16) == false))
                                {
                                    ic.Status = ic.Status + "Email格是不正確;";
                                }
                                #endregion

                                #region 判斷是否有重覆滙入的折讓單,同一個檔案內
                                var importinv = _queryItems.Where(w => w.AllowanceNo == row.AllowanceNo.Trim()).FirstOrDefault();
                                if (importinv != null)
                                {
                                    ic.Status = ic.Status + "折讓單號碼重複匯入;";
                                }
                                #endregion

                                #region 檢查折讓單編號不行重複(CheckSeriaKeyByDb)
                                if (CheckSeriaKeyByDb(ic.AllowanceNo))
                                {
                                    ic.Status = ic.Status + "該折讓單編號資料庫已存在;";
                                }
                                #endregion

                                #region 如果googleid,發票日期,發票號碼,對方統編四個攔位都正確
                                if (bolCheck)
                                {
                                    #region 檢查googleid,發票日期,發票號碼,對方統編是否正確 (CheckDataByDb)
                                    if (CheckDataByDb(ic.CustomerID, ic.InvDate, ic.InvNo, ic.BuyerReceiptNo))
                                    {
                                        ic.Status = ic.Status + "googleid,發票日期,發票號碼,對方統編不正確;";
                                    }
                                    #endregion
                                }
                                #endregion
                            }

                            if (ic.Status.Length == 0)
                            {
                                ic.Status = "等待滙入";
                            }
                            else
                            {
                                ic.Status = "第" + rowNum.ToString() + "筆失敗:" + ic.Status;
                                result = false;
                            }
                            _queryItems.Add(ic);
                        }
                        
                        if (result == true)
                        {                        
                            //this.btnOK.Visible = false;             //miyu20111104 del
                            //this.FileUpload1.Visible = false;
                            this.ImportButton.Visible = true;
                            this.CannelButton.Visible = true;
                            this.ImportButton.Visible = true;
                            this.CannelButton.Visible = true;
                            ImportButton.OnClientClick = String.Format("if ( confirm('確定匯入折讓單筆數共計 {0}筆'))   {{ $('#btnPopup').trigger('click'); }} else return false; ", _queryItems.Count().ToString());
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
                        bindData();
                        //this.PrintingButton21.Visible = true;
                    }
                }
                else
                {
                    this.lblError.Text = "資料格式有誤或無上傳資料";
                    this.PrintingButton21.Visible = false;
                    this.btnExcel.Visible = false;
                    this.divResult.Visible = false;
                    this.ImportButton.Visible = false;
                    this.CannelButton.Visible = true;
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        protected void CannelButton_Click(object sender, EventArgs e)
        {
            Logger.Info("匯入折讓單取消,檔案名稱" + this.ViewState["FileName"].ToString());
            _queryItems.Clear();
            this.PrintingButton21.Visible = false;
            this.divResult.Visible = false;
            this.ImportButton.Visible = false;
            this.CannelButton.Visible = false;
            this.FileUpload1.Visible = true;
            this.btnExcel.Visible = false;
            this.btnOK.Visible = true;

        }

        #region 寫入資料庫
        protected void ImportButton_Click(object sender, EventArgs e)
        {            
            #region 檢核成功寫入資料庫
            try
            {
                using (InvoiceManager im = new InvoiceManager())
                {
                    //string InvoiceIDs = "";

                    //InvoiceCancellationUpload invUp = new InvoiceCancellationUpload();
                    //invUp.FilePath = Logger.LogDailyPath;
                    //invUp.UID = _userProfile.UID;
                    //invUp.UploadDate = DateTime.Now;
                    List<InvoiceAllowance> items = new List<InvoiceAllowance>();
                    foreach (var row in _queryItems)
                    {
                        byte TheAllowanceType;
                        var inv = im.GetTable<InvoiceItem>().Where(w => w.TrackCode + w.No == row.InvNo.Trim() && w.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID).FirstOrDefault();

                        //判斷AllowanceType 1=買方開立折讓證明單 2=賣方折讓證明
                        //依據InvoiceBuyer之ReceiptNo若是"0000000000"為賣方折讓證明 = 2
                        //若否則為買方開立折讓證明單 = 1
                        if (inv.InvoiceBuyer.ReceiptNo == "0000000000")
                            TheAllowanceType = 2;  //2=賣方折讓證明
                        else
                            TheAllowanceType = 1;  //1=買方開立折讓證明單

                        CDS_Document doc = new CDS_Document();
                        doc.DocType = 11;
                        doc.DocDate = DateTime.Now;
                        im.GetTable<CDS_Document>().InsertOnSubmit(doc);
                        DocumentOwner docOwner = new DocumentOwner();
                        docOwner.CDS_Document = doc;
                        docOwner.OwnerID = _userProfile.CurrentUserRole.OrganizationCategory.Organization.CompanyID;
                        im.GetTable<DocumentOwner>().InsertOnSubmit(docOwner);
                        InvoiceAllowance invoiceallowance = new InvoiceAllowance();
                        items.Add(invoiceallowance);
                        invoiceallowance.CDS_Document = doc;                        
                        invoiceallowance.AllowanceDate = DateTime.Parse(row.AllowanceDate);
                        invoiceallowance.AllowanceNumber = row.AllowanceNo;
                        invoiceallowance.AllowanceType = TheAllowanceType;
                        invoiceallowance.SellerId = _userProfile.CurrentUserRole.OrganizationCategory.Organization.ReceiptNo;
                        invoiceallowance.BuyerId = row.BuyerReceiptNo;
                        invoiceallowance.InvoiceItem = inv;
                        //細項 
                        Int16 no = 0;
                        InvoiceProductItem invprditem = im.GetTable<InvoiceProductItem>().Where(w => w.InvoiceProduct.InvoiceDetails.Any(a => a.InvoiceID == inv.InvoiceID)).FirstOrDefault();
                        InvoiceAllowanceItem invAitem = new InvoiceAllowanceItem();                        
                        invAitem.InvoiceNo = inv.No;
                        invAitem.UnitCost = decimal.Parse(row.UnitCost);
                        invAitem.OriginalDescription = row.Brief;
                        //算這筆項目的總金額 
                        //invAitem.Amount = int.Parse(row.UnitCost) * int.Parse(row.Piece);
                        invAitem.Amount = int.Parse(row.Amount);
                        //invAitem.Piece = invprditem.Piece;
                        invAitem.Piece = int.Parse(row.Piece);
                        //invAitem.PieceUnit = invprditem.PieceUnit;
                        invAitem.PieceUnit = invprditem.PieceUnit;
                        invAitem.TaxType = invprditem.TaxType;
                        //invAitem.ItemNo = invprditem.ItemNo;
                        invAitem.InvoiceDate = DateTime.Parse(row.InvDate);
                        //invAitem.InvoiceDate = inv.InvoiceDate;
                        //算這筆單價的稅額
                        invAitem.Tax = int.Parse(row.Tax);
                        //
                        if (invoiceallowance.TotalAmount != null)
                            invoiceallowance.TotalAmount += invAitem.Amount;
                        else
                            invoiceallowance.TotalAmount = invAitem.Amount;

                        if (invoiceallowance.TaxAmount != null)
                            invoiceallowance.TaxAmount += invAitem.Tax;
                        else
                            invoiceallowance.TaxAmount = invAitem.Tax;

                        invAitem.No = (short)(no + Int16.Parse("1"));
                        invAitem.InvoiceProductItem = invprditem;
                        im.GetTable<InvoiceAllowanceItem>().InsertOnSubmit(invAitem);
                        InvoiceAllowanceDetail invoiceallowancedetail = new InvoiceAllowanceDetail();
                        invoiceallowancedetail.InvoiceAllowance = invoiceallowance;
                        invoiceallowancedetail.InvoiceAllowanceItem = invAitem;                        
                        im.GetTable<InvoiceAllowanceDetail>().InsertOnSubmit(invoiceallowancedetail);

                        InvoiceAllowanceSeller invAllowanceSeller = new InvoiceAllowanceSeller();
                        invAllowanceSeller.InvoiceAllowance = invoiceallowance;
                        invAllowanceSeller.SellerID = _userProfile.CurrentUserRole.OrganizationCategory.Organization.CompanyID;
                        im.GetTable<InvoiceAllowanceSeller>().InsertOnSubmit(invAllowanceSeller);
                        //  newics.Add(newic);
                        //InvoiceCancellationUploadList invuplist = new InvoiceCancellationUploadList();
                        //invuplist.InvoiceCancellation = newic;
                        //invuplist.InvoiceCancellationUpload = invUp;
                        //im.GetTable<InvoiceCancellationUploadList>().InsertOnSubmit(invuplist);
                        row.Status = "滙入成功";
                    }
                    im.SubmitChanges();
                    this.PrintingButton21.Visible = true;
                    this.lblError.Text = "匯入折讓單成功";
                    Logger.Info("匯入折讓單成功,檔案名稱" + this.ViewState["FileName"].ToString());
                    //寄送mail
                    //items.Where(r => r.InvoiceItem.InvoiceBuyer.IsB2C()).Select(a => a.AllowanceID).SendGoogleInvAllowanceMail();

                    TotalNum = _queryItems.Count();
                    YesNum = _queryItems.Where(r => r.Status == "滙入成功").Count();
                    NoNum = TotalNum - YesNum;
                    this.ImportButton.Visible = false;
                    this.CannelButton.Visible = false;
                    this.FileUpload1.Visible = true;
                    this.btnOK.Visible = true;
                    this.btnExcel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                foreach (var row in _queryItems)
                {
                    row.Status = "滙入失敗";
                }
                this.lblError.Text = "系統作業失敗:匯入折讓單存檔失敗,原因:" + ex.Message;
                Logger.Error("系統作業失敗,匯入折讓單存檔失敗原因:" + ex.Message);
                btnExcel.Visible = false;
                PrintingButton21.Visible = false;
            }
            #endregion
        }
        #endregion

        #region 資料庫中Key不能重複---CheckSeriaKeyByDb()
        //AllowanceNumber折讓單編號不能重複
        private bool CheckSeriaKeyByDb(string AllowanceNumberKey)
        {
            var mgr = dsEntity.CreateDataManager();
            Expression<Func<InvoiceAllowance, bool>> queryExpr = w => true;
            queryExpr = queryExpr.And(w => w.AllowanceNumber == AllowanceNumberKey);
            IQueryable<InvoiceAllowance> invData = mgr.GetTable<InvoiceAllowance>().Where(queryExpr);

            if (invData.Count() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region 資料庫中Key不能重複---CheckDataByDb(.....)
        //檢查googleid,發票日期,發票號碼,對方統編是否正確
        private bool CheckDataByDb
            (string strGoogleId, string strInvDate, string strInvNo, string strBuyerNo)
        {
            var mgr = dsEntity.CreateDataManager();
            Expression<Func<InvoiceItem, bool>> queryExpr = w => true;
            queryExpr = queryExpr.And(w => w.InvoiceBuyer.CustomerID == strGoogleId);
            queryExpr = queryExpr.And(w => w.InvoiceDate > DateTime.Parse(strInvDate));
            queryExpr = queryExpr.And(w => w.InvoiceDate < DateTime.Parse(strInvDate).AddDays(1));
            queryExpr = queryExpr.And(w => (w.TrackCode+w.No) == strInvNo);
            if (strBuyerNo == "" || strBuyerNo == "N/A")
                strBuyerNo = "0000000000";
            queryExpr = queryExpr.And(w => w.InvoiceBuyer.ReceiptNo == strBuyerNo);
            IQueryable<InvoiceItem> invData = mgr.GetTable<InvoiceItem>().Where(queryExpr);
            if (invData.Count() < 1)
                return true;
            else
                return false;
        }
        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            string[] StrCumName = { "序號", "GoogleID", "折讓號碼",  "買受人名稱", "買受人統編", "未稅金額", "稅額", "含稅金額", "匯入狀態" };
            ExportDataToCsv(_queryItems, "InvoiceAllowanceFile.csv", StrCumName, true);
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
        public void ExportDataToCsv(List<InvoiceAllowances> dt, string FileName, string[] ColumnName, bool HasColumnName)
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
                Page.Response.AddHeader("content-disposition", "attachment;filename=InvoiceAllowanceFile.csv");
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
               
                case 29:  //驗證特殊字元
                    RegularExpressions = "(?=.*[@#$%^&+=])";
                    break;
                case 30: //驗證日期格式YYYY/MM/DD

                    cv.ControlToValidate = _value;
                    cv.Operator=ValidationCompareOperator.DataTypeCheck;
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
