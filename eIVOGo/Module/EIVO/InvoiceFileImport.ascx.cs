using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using eIVOGo.Module.Common;
using Model.DataEntity;
using Model.Helper;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;

namespace eIVOGo.Module.EIVO
{
    public partial class InvoiceFileImport : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;
        protected List<ToReadData> ReadData = new List<ToReadData>();
        protected ToReadData TheReadData = new ToReadData();
        protected int intYear = int.Parse(DateTime.Now.Year.ToString());
        protected int intMonth = int.Parse(DateTime.Now.Month.ToString());
        protected int intPeriod;

        internal const int LOCALE_SYSTEM_DEFAULT = 0x0800;
        internal const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        internal const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc, 
            [Out] string lpDestStr, int cchDest); 


        public class ToReadData
        {
            #region  自訂類別
            // Class members:
            // Property.
            public string SerialNumber { get; set; }
            public string InvoiceDate { get; set; }
            public string GoogleID { get; set; }
            public string GoodsItem { get; set; }
            public string GoodsPrice { get; set; }
            public string GoodsCount { get; set; }
            public string BuyAmount { get; set; }
            public string BuyerUniformNumber { get; set; }
            public string BuyTax { get; set; }
            public string BuyAmountTax { get; set; }
            public string BuyerName { get; set; }
            public string BuyerContact { get; set; }
            public string BuyerAddr { get; set; }
            public string BuyerPhone { get; set; }
            public string BuyerEmail { get; set; }
            public string InvoiceNo { get; set; }
            public string InvoiceTrackCode { get; set; }
            public string InvoiceType { get; set; }
            public string DataMessage { get; set; }
            public int IntervalID { get; set; }
            public int InvoiceID { get; set; }
        }

        public class InvoiceNoAssignmented
        {
            public InvoiceNoInterval invNoInterval { get; set; }
            public int NumberCount { get; set; }
            public int NumberAssignmentedCount { get; set; }
            public int NumberCanUseCount { get; set; }
        }

        public class InvoiceNoIntervalToExhibition
        {
            public int IntervalID  { get; set; }
            public int TrackID { get; set; }
            public int InvoiceNo { get; set; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            
            _userProfile = WebPageUtility.UserProfile;
            //FileUpload.Attributes.Add
            //    ("onchange", "return validateFileUpload(this);");
            if (!this.IsPostBack)
            {
                Session["ReadData"]=null;
                FormDisplay(false);
            }
            PagingControl1.PageSize = 10;
            btnPrint.PrintControls.Add(DIV2);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PagingControl1.PageIndexChanged += new Uxnet.Web.Module.Common.PageChangedEventHandler(PagingControl1_PageIndexChanged);
            btnPrint.BeforeClick += new EventHandler(btnPrint_BeforeClick);
            this.PreRender += new EventHandler(InvoiceFileImport_PreRender);
        }

        void InvoiceFileImport_PreRender(object sender, EventArgs e)
        {
            btnConfirm.OnClientClick = String.Format("return validateFileUpload(document.getElementById('{0}'));", FileUpload.ClientID);
            btnAddCode.OnClientClick = String.Format("if (confirm('確定匯入發票筆數共計 {0}筆')) {{ $('#btnPopup').trigger('click'); }} else return false; ", ReadData.Count);
        }

        void btnPrint_BeforeClick(object sender, EventArgs e)
        {
            ToBindData(false);
        }

        void PagingControl1_PageIndexChanged(object source, Uxnet.Web.Module.Common.PageChangedEventArgs e)
        {
            bool btnPrintShow = btnPrint.Visible;
            bool btnAddCodeShow = btnAddCode.Visible;
            bool btnResetShow = btnReset.Visible;            
            ToBindData(true);
            btnPrint.Visible = btnPrintShow;
            btnExcell.Visible = btnPrint.Visible;
            btnAddCode.Visible = btnAddCodeShow;
            btnReset.Visible = btnResetShow;
        }

        protected void FormDisplay(bool showYes)
        {
            DIV2.Visible = showYes;
            PagingControl1.Visible = showYes;
            btnPrint.Visible = showYes;
            btnExcell.Visible = showYes;
            lblEx.Visible = showYes;
            btnAddCode.Visible = showYes;
            btnReset.Visible = showYes;
        }

        public static string ToTraditional(string pSource)
        {
            String tTarget = new String(' ', pSource.Length);
            int tReturn = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_TRADITIONAL_CHINESE, pSource, pSource.Length, tTarget, pSource.Length);
            return tTarget;
        } 

        #region  0.讀CSV檔-------parseCSV()
        private bool parseCSV()
        {
            try
            {
                ReadData.Clear();
                using (StreamReader readFile = new StreamReader(FileUpload.PostedFile.InputStream,Encoding.GetEncoding("GB2312")))
                {
                    string line;                    
                    string[] row;
                    //Encoding encBig5 = Encoding.GetEncoding("big5");
                    //Encoding encGBK = Encoding.GetEncoding("GBK");
                    bool YesData = true;
                    int intReadCount = 0;
                    while ((line = readFile.ReadLine()) != null)
                    {
                        if (String.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        if (intReadCount > 0)
                        {                            
                            line = ToTraditional(line);
                            row = line.Split(',');

                            //if (row.Length != 15)
                            //{
                            //    YesData = false;
                            //    return YesData;
                            //}                            
                            row[0] = row[0].Trim();
                            if (row.Length == 15)
                            {
                                int x = 0;
                                TheReadData = new ToReadData();
                                TheReadData.SerialNumber = row[x++];
                                TheReadData.InvoiceDate = row[x++];
                                TheReadData.GoogleID = row[x++];
                                TheReadData.GoodsItem = row[x++];
                                TheReadData.GoodsPrice = row[x++];
                                TheReadData.GoodsCount = row[x++];
                                TheReadData.BuyAmount = row[x++];
                                TheReadData.BuyerUniformNumber = row[x++];
                                TheReadData.BuyTax = row[x++];
                                TheReadData.BuyAmountTax = row[x++];
                                TheReadData.BuyerName = row[x++];
                                TheReadData.BuyerContact = row[x++];
                                TheReadData.BuyerAddr = row[x++];
                                TheReadData.BuyerPhone = row[x++];
                                TheReadData.BuyerEmail = row[x++];
                                if (TheReadData.BuyerUniformNumber == "" && TheReadData.BuyAmountTax != "")   //B2C時計算金額與稅額
                                {
                                    decimal sum;
                                    sum = (decimal.Parse(TheReadData.BuyAmountTax) / decimal.Parse("1.05"));
                                    sum = decimal.Round(sum, 0, MidpointRounding.AwayFromZero);
                                    TheReadData.BuyAmount = sum.ToString();
                                    TheReadData.BuyTax = (int.Parse(TheReadData.BuyAmountTax) - int.Parse(TheReadData.BuyAmount)).ToString();
                                    TheReadData.BuyerUniformNumber = "N/A";
                                }
                                TheReadData.InvoiceNo = "";
                                TheReadData.InvoiceTrackCode = "";
                                TheReadData.DataMessage = "";
                                ReadData.Add(TheReadData);
                            }
                            else
                            {
                                TheReadData.BuyTax = "";
                                TheReadData.BuyAmountTax = "";
                                TheReadData.BuyAmount = "";
                                TheReadData.DataMessage = "資料少於15個欄位";
                                ReadData.Add(TheReadData);
                            }
                        }
                        intReadCount = intReadCount + 1;
                    }
                    return YesData;
                }                
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }        
        #endregion

        #region  1.檢查發票配號--CheckInvoiceCount(int useNocount)   
        private bool CheckInvoiceCount(int useNocount)
        {
            bool YesData = true;
            IQueryable<InvoiceNoInterval> invData = InvNoIntervalData();
            if (invData.Count() > 0)
            {
                var _invData = invData.Select(o => new InvoiceNoAssignmented
                {
                    invNoInterval = o,
                    NumberCount = (o.EndNo - o.StartNo + 1),
                    NumberAssignmentedCount = o.InvoiceNoAssignments.Count,
                    NumberCanUseCount = ((o.EndNo - o.StartNo + 1) - o.InvoiceNoAssignments.Count)
                });
                int TheCanUseCount = 0;
                foreach (var toSum in _invData)
                {
                    TheCanUseCount = TheCanUseCount + toSum.NumberCanUseCount;
                }
                if (useNocount > TheCanUseCount)
                    YesData = false;                
            } 
            else
                YesData = false; 
            return YesData;
        }
         #endregion

        #region  2.檔案中Key不能重複---CheckSeriaKeyByFile()
        private bool CheckSeriaKeyByFile()
        {
             bool YesData = true;
            //以(SerialNumber +GoogleID)為Key,檢查Key不能重複
            var query = ReadData.AsQueryable().GroupBy(w => w.SerialNumber + w.GoogleID);
            query = query.Where (w => w.Count() > 1);
            if (query.Count() > 0)
            {
                YesData = false;
                foreach (var tempquery in query)
                {
                    foreach (ToReadData tempReadData in ReadData)
                    {
                        if((tempReadData.SerialNumber + tempReadData.GoogleID) == tempquery.Key)
                        {
                            tempReadData.DataMessage = "Key(序號+GoogleID)重複";
                        }
                    }
                }                
            }
            return YesData;
        }
        #endregion

        #region  3.欄位檢查------CheckData()   
        private bool CheckData()
        {                     
            foreach (ToReadData tempReadData in ReadData)
            {
                if (tempReadData.DataMessage.Length == 0)
                {
                    #region 第一~六欄檢查
                    if ((tempReadData.SerialNumber.Length != 11) || (ValidateString(tempReadData.SerialNumber, 20) == false))
                    {
                        tempReadData.DataMessage = "序號格式錯誤;";
                    }
                    else
                    {
                        if (CheckSeriaKeyByDb(tempReadData.SerialNumber + tempReadData.GoogleID))
                            tempReadData.DataMessage = "匯入資料重複;";
                    }

                    DateTime dtValue;
                    if (DateTime.TryParse(tempReadData.InvoiceDate, out dtValue) == false)
                    {
                        tempReadData.DataMessage = tempReadData.DataMessage + "日期格式錯誤;";
                    }
                    if ((tempReadData.GoogleID == "") || (ValidateString(tempReadData.GoogleID, 20) == false))
                    {
                        tempReadData.DataMessage = tempReadData.DataMessage + "GoogleID格式錯誤;";
                    }
                    if (tempReadData.GoodsItem == "")
                    {
                        tempReadData.DataMessage = tempReadData.DataMessage + "品項不得為空白;";
                    }
                    if ((tempReadData.GoodsPrice == "") || (ValidateString(tempReadData.GoodsPrice, 20) == false))
                    {
                        tempReadData.DataMessage = tempReadData.DataMessage + "單價格式錯誤;";
                    }
                    if ((tempReadData.GoodsCount == "") || (ValidateString(tempReadData.GoodsCount, 20) == false))
                    {
                        tempReadData.DataMessage = tempReadData.DataMessage + "數量格式錯誤;";
                    }
                    #endregion

                    #region 第七~九欄檢查
                    if (tempReadData.BuyAmount != "")
                    {
                        if (ValidateString(tempReadData.BuyAmount, 20) == false)
                            tempReadData.DataMessage = tempReadData.DataMessage + "金額格式錯誤;";
                    }
                    if (tempReadData.BuyerUniformNumber != "")
                    {
                        if (tempReadData.BuyerUniformNumber != "N/A")
                        {
                            if (tempReadData.BuyerUniformNumber.Length != 8)
                                tempReadData.DataMessage = tempReadData.DataMessage + "買受人統編長度不足8碼;";
                            if (ValidateString(tempReadData.BuyerUniformNumber, 20) == false)
                                tempReadData.DataMessage = tempReadData.DataMessage + "統編格式錯誤;";
                        }
                    }
                    if (tempReadData.BuyTax != "")
                    {
                        if (ValidateString(tempReadData.BuyTax, 20) == false)
                            tempReadData.DataMessage = tempReadData.DataMessage + "稅額格式錯誤;";
                    }
                    #endregion

                    #region 第九~十四欄檢查
                    if ((tempReadData.BuyAmountTax == "") || (ValidateString(tempReadData.BuyAmountTax, 20) == false))
                    {
                        tempReadData.DataMessage = tempReadData.DataMessage + "含稅金額式錯誤;";
                    }
                    if (tempReadData.BuyerName == "")
                    {
                        tempReadData.DataMessage = tempReadData.DataMessage + "名稱不得為空白;";
                    }
                    if (tempReadData.BuyerContact == "")
                    {
                        tempReadData.DataMessage = tempReadData.DataMessage + "聯絡人不得為空白;";
                    }
                    if (tempReadData.BuyerAddr == "")
                    {
                        tempReadData.DataMessage = tempReadData.DataMessage + "地址不得為空白;";
                    }
                    if (tempReadData.BuyerPhone == "")
                    {
                        tempReadData.DataMessage = tempReadData.DataMessage + "連絡電話不得為空白;";
                    }
                    if ((tempReadData.BuyerEmail == "") || (ValidateString(tempReadData.BuyerEmail, 16) == false))
                    {
                        tempReadData.DataMessage = tempReadData.DataMessage + "Email格式不正確;";
                    }
                    #endregion
                    //去掉最後一個";"
                    if (tempReadData.DataMessage.Length > 0)
                        if (tempReadData.DataMessage.Substring((tempReadData.DataMessage.Length - 1), 1) == ";")
                            tempReadData.DataMessage = tempReadData.DataMessage.Substring(0, (tempReadData.DataMessage.Length - 1));
                }
                
            }           
            bool YesData = true;
            foreach (ToReadData tempReadData in ReadData)
            {                   
                if(tempReadData.DataMessage !="")
                    YesData = false;
            }
            return YesData;
        }
        #endregion

        #region 3-1.資料庫中Key不能重複---CheckSeriaKeyByDb()
        private bool CheckSeriaKeyByDb(string SernoGoogleKey)
        {
            var mgr = dsEntity.CreateDataManager();
            Expression<Func<InvoicePurchaseOrder, bool>> queryExpr = w => true;
            queryExpr = queryExpr.And(w => w.OrderNo == SernoGoogleKey);
            IQueryable<InvoicePurchaseOrder> invData = mgr.GetTable<InvoicePurchaseOrder>().Where(queryExpr);

            if (invData.Count()>0)
                return true;
            else
                return false;
        }
        #endregion

        #region  4.分配發票號碼------ToShareNumber()
        private void ToShareNumber()
        {        
            IQueryable<InvoiceNoInterval> invData = InvNoIntervalData();
            ReadData = (List<ToReadData>)Session["ReadData"];
            int RunCount = ReadData.Count();                               //預計寫入筆數
            int WriteCount = 0;                                            //已經寫入筆數
            foreach (var invDataTmp in invData)  
            {
                int DataCount;
                int UsedCount;
                int UsedMax;
                string sUseTrackCode;
                sUseTrackCode = UseTrackCode(invDataTmp.IntervalID);       //讀取字軌
                DataCount = invDataTmp.EndNo - invDataTmp.StartNo + 1;     //該批發票號碼總筆數
                UsedCount = NumberUsedCount(invDataTmp.IntervalID);        //讀取當期已用字軌號碼之筆數 
                UsedMax = NumberUsedMax(invDataTmp.IntervalID);            //讀取當期已用字軌號碼之最大值 
                if (UsedMax == 0)
                {
                    UsedMax = invDataTmp.StartNo - 1;
                }
                if ( RunCount > 0)
                {
                    int toWriteCount = 0;
                    if ((DataCount - UsedCount) > 0)
                    {
                        if (RunCount > (DataCount - UsedCount))
                        {
                            int StartValue = UsedMax + 1;
                            int runSum = StartValue + (DataCount - UsedCount )-1;
                            for (int i = StartValue; i < (runSum + 1); i++)
                            {
                                ReadData[WriteCount].InvoiceNo = i.ToString("00000000");
                                ReadData[WriteCount].InvoiceTrackCode = sUseTrackCode;
                                ReadData[WriteCount].IntervalID = invDataTmp.IntervalID;
                                WriteCount = WriteCount + 1;
                                toWriteCount = toWriteCount + 1;
                            }
                            //RunCount = RunCount - WriteCount;
                        }
                        else
                        {
                            int StartValue = UsedMax + 1;
                            int runSum = RunCount + UsedMax;
                            for (int i = StartValue; i < (runSum + 1); i++)
                            {
                                ReadData[WriteCount].InvoiceNo = i.ToString("00000000");
                                ReadData[WriteCount].InvoiceTrackCode = sUseTrackCode;
                                ReadData[WriteCount].IntervalID = invDataTmp.IntervalID;
                                WriteCount = WriteCount + 1;
                                toWriteCount = toWriteCount + 1;
                            }
                            //RunCount = RunCount - WriteCount;
                        }
                        RunCount = RunCount - toWriteCount;
                    }
                }
            }
        }

        private int NumberUsedCount(int intIntervalID)
        {
            var mgr = dsEntity.CreateDataManager();
            Expression<Func<InvoiceNoAssignment, bool>> queryExpr = w => true;
            queryExpr = queryExpr.And(w => w.IntervalID == intIntervalID);
            IQueryable<InvoiceNoAssignment> invData = mgr.GetTable<InvoiceNoAssignment>().Where(queryExpr);
            var items = invData.GroupBy(w => new { w.InvoiceID, w.IntervalID, w.InvoiceNo});
            if (items != null)
                return items.Count();
            else
                return 0;
        }

        private string UseTrackCode(int intIntervalID)
        {
            var mgr = dsEntity.CreateDataManager();
            Expression<Func<InvoiceNoInterval, bool>> queryExpr = w => true;
            queryExpr = queryExpr.And(w => w.IntervalID == intIntervalID);
            IQueryable<InvoiceNoInterval> invData = mgr.GetTable<InvoiceNoInterval>().Where(queryExpr);
            if (invData.Count() > 0)
            {
                if (invData.First().InvoiceTrackCodeAssignment.InvoiceTrackCode.TrackCode != "")
                    return invData.First().InvoiceTrackCodeAssignment.InvoiceTrackCode.TrackCode;
                else
                    return "00";
            }
            else
            {
                return "00";
            }

        }

        private int NumberUsedMax(int intIntervalID)
        {
            var mgr = dsEntity.CreateDataManager();
            Expression<Func<InvoiceNoAssignment, bool>> queryExpr = w => true;
            queryExpr = queryExpr.And(w => w.IntervalID == intIntervalID);
            IQueryable<InvoiceNoAssignment> invData = mgr.GetTable<InvoiceNoAssignment>().Where(queryExpr);
            invData = invData.OrderByDescending(o => o.InvoiceNo);
            if (invData.Count() > 0)
            {
                if (invData.First().InvoiceNo.HasValue)
                    return invData.First().InvoiceNo.Value;
                else
                    return 0;
            }
            else
                return 0;

        }
        #endregion

        #region  5.檢核成功寫入資料庫--ToSaveDB()
        private void ToSaveDB()
        {
            //CheckSeriaKeyByDb
            bool bolToWriteDB = true;
            for (int i = 0; i < ReadData.Count(); i++)
            {
                if(CheckSeriaKeyByDb(ReadData[i].SerialNumber + (ReadData[i].GoogleID)))
                {
                                   
                    bolToWriteDB = false;
                    break;
                }                
            }
            if (bolToWriteDB == false)
            {
                lblEx.Text = "寫入錯誤,資料庫已存在該發票序號!!";
                lblEx.Visible = true;
                btnReset.Visible = true;
                btnAddCode.Visible = false;
                return;
            }
            
            Random randObj = new Random();
            for (int i = 0; i < ReadData.Count(); i++)
            {
                string strRandomNo = randObj.Next().ToString("0000");
                strRandomNo = strRandomNo.Substring(0, 4);
                var mgr = dsEntity.CreateDataManager();
                var item = new InvoiceItem
                {
                    CDS_Document = new CDS_Document
                    {
                        DocDate = DateTime.Now,
                        DocType = (int)Naming.DocumentTypeDefinition.E_Invoice,
                        DocumentOwner = new DocumentOwner {
                            OwnerID = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID
                        }
                    },
                    No = ReadData[i].InvoiceNo,
                    SellerID = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID,
                    TrackCode = ReadData[i].InvoiceTrackCode,
                    InvoiceDate = DateTime.Now,
                    InvoiceType = 3,
                    DonateMark = "0",
                    RandomNo = strRandomNo,
                    InvoiceBuyer = new InvoiceBuyer
                    {
                        CustomerID = ReadData[i].GoogleID,
                        //011
                        Name = ReadData[i].BuyerUniformNumber == "N/A" ? strRandomNo : ReadData[i].BuyerName,   
                        //008
                        ReceiptNo = (String.IsNullOrEmpty(ReadData[i].BuyerUniformNumber) || ReadData[i].BuyerUniformNumber == "N/A") ? "0000000000" : ReadData[i].BuyerUniformNumber,
                        CustomerName = ReadData[i].BuyerName,
                        ContactName = ReadData[i].BuyerContact,                                 //012
                        Address = ReadData[i].BuyerAddr,                                        //013
                        Phone = ReadData[i].BuyerPhone,                                         //014
                        EMail = ReadData[i].BuyerEmail                                          //015
                    },
                    InvoiceAmountType = new InvoiceAmountType
                    {
                        DiscountAmount = 0,
                        SalesAmount = decimal.Parse(ReadData[i].BuyAmount),                    //007
                        TaxAmount = decimal.Parse(ReadData[i].BuyTax),                         //009
                        TaxRate = 0.05m,
                        TaxType = 1,
                        TotalAmount = decimal.Parse(ReadData[i].BuyAmountTax),                 //010
                        TotalAmountInChinese = Utility.ValueValidity.MoneyShow(ReadData[i].BuyAmountTax)
                    },
                    InvoiceNoAssignment = new InvoiceNoAssignment
                    {
                        IntervalID = ReadData[i].IntervalID,
                        InvoiceNo = int.Parse(ReadData[i].InvoiceNo)
                    },
                    InvoicePurchaseOrder = new InvoicePurchaseOrder
                    {
                        //001;003
                        OrderNo = ReadData[i].SerialNumber + ReadData[i].GoogleID,
                        //002
                        PurchaseDate = DateTime.Parse( ReadData[i].InvoiceDate) 
                    },
                };

                InvoiceProduct product = new InvoiceProduct {Brief = ReadData[i].GoodsItem}; //004
                //InvoicePurchaseOrder InvOrder = new InvoicePurchaseOrder
                //{
                //    OrderNo = ReadData[i].SerialNumber + ReadData[i].GoogleID;
                //};

                product.InvoiceProductItem.Add(new InvoiceProductItem
                {
                    CostAmount = decimal.Parse(ReadData[i].GoodsPrice),  
                    //ItemNo = "",
                    Piece = int.Parse(ReadData[i].GoodsCount),                              //006
                     //PieceUnit = "",
                    UnitCost = decimal.Parse(ReadData[i].GoodsPrice),                       //005
                     //Remark = "",
                     TaxType = 5,
                     No = 0
                });                   

                item.InvoiceDetails.Add(new InvoiceDetail
                    {
                        InvoiceProduct = product
                    });
                mgr.GetTable<InvoiceItem>().InsertOnSubmit(item);                 
                mgr.SubmitChanges();
                ReadData[i].InvoiceID = item.InvoiceID;
                ReadData[i].DataMessage = "匯入成功";
            }

            //ReadData.Select(r => r.InvoiceID).SendGoogleInvoiceMail();
        }
        #endregion        

        private IQueryable<InvoiceNoInterval> InvNoIntervalData()
        {
            var mgr = dsEntity.CreateDataManager();
            Expression<Func<InvoiceNoInterval, bool>> queryExpr = w => true;
            queryExpr = queryExpr.And(w => w.InvoiceTrackCodeAssignment.SellerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID);
            //int intYear = int.Parse(DateTime.Now.Year.ToString());
            //int intMonth = int.Parse(DateTime.Now.Month.ToString());
            //int intPeriod;
            if ((intMonth % 2) == 0)
                intPeriod = intMonth / 2;
            else
                intPeriod = intMonth / 2 + 1;
            queryExpr = queryExpr.And(w => w.InvoiceTrackCodeAssignment.InvoiceTrackCode.Year == intYear);
            queryExpr = queryExpr.And(w => w.InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo == intPeriod);
            IQueryable<InvoiceNoInterval> invData = mgr.GetTable<InvoiceNoInterval>().Where(queryExpr);
            invData = invData.OrderBy(o => o.IntervalID);
            return invData;
        }

        private void ToBindData(bool bPaging)
        {
            if ((List<ToReadData>)Session["ReadData"] != null)
            {
                ReadData = (List<ToReadData>)Session["ReadData"];
            }
            
            if (ReadData.Count() > 0)
            {
                this.PagingControl1.RecordCount = ReadData.Count();
                this.PagingControl1.Visible = true;

                rpList.DataSource = bPaging ? ReadData.Skip(PagingControl1.CurrentPageIndex * PagingControl1.PageSize).Take(PagingControl1.PageSize) : ReadData;
                rpList.DataBind();
                DIV2.Visible = true;
                PagingControl1.Visible = true;

                int intSuccess = 0;
                foreach (var invDataTmp in ReadData)
                {
                    if (invDataTmp.DataMessage == "" || invDataTmp.DataMessage == "匯入成功")
                    {
                        intSuccess = intSuccess + 1;
                    }                    
                }

                lblTotalCount.Text = ReadData.Count.ToString();
                lblYesCount.Text = intSuccess.ToString();
                lblNoCount.Text = (ReadData.Count - intSuccess).ToString();
            }
            else
            {                
                this.PagingControl1.Visible = false;
                DIV2.Visible = false;
                PagingControl1.Visible = false;
                //btnPrint.Visible = false;
                //btnAddCode.Visible = false;
                //btnReset.Visible = false;
            }
            if (lblEx.Visible)
            {
                //btnPrint.Visible = true;
                btnAddCode.Visible = false;
                btnReset.Visible = false;
            }
            else
            {
                //btnPrint.Visible = false;
                btnAddCode.Visible = true;
                btnReset.Visible = true;
            }
        }

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
                //case 28: //驗證日期格式YYYY/MM/DD
                //    RegularExpressions = "^([2][0]\d{2}\/([0]\d|[1][0-2])\/([0-2]\d|[3][0-1]))$|^([2][0]\d{2}\/([0]\d|[1][0-2])\/([0-2]\d|[3][0-1]))$";
                //    break;
                case 29:  //驗證特殊字元
                    RegularExpressions  = "(?=.*[@#$%^&+=])";
                    break;
                default:
                    break;
            }

            Match m = Regex.Match(_value, RegularExpressions);
            if (m.Success)
                return true;
            else
                return false;
        }
        #endregion

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            Session["ReadData"] = null;
            FormDisplay(false);
            PagingControl1.CurrentPageIndex = 0;
            if (FileUpload.HasFile)
            {
                String fileName = Path.Combine(Logger.LogDailyPath, String.Format("{0:yyyyMMddHHmmssfff}.csv", DateTime.Now));
                FileUpload.SaveAs(fileName);
                FileUpload.PostedFile.InputStream.Seek(0, SeekOrigin.Begin);

                FormDisplay(false);
                if (!parseCSV())                                                   //0.讀CSV檔
                {
                    lblEx.Text = "讀取失敗,CSV檔案格式錯誤!!";
                    lblEx.Visible = true;
                    return;
                }
                if (!CheckData() && lblEx.Visible == false)                      //3.欄位檢查
                {
                    DataErrShow();
                    lblEx.Text = "寫入失敗,欄位格式錯誤!!";
                    lblEx.Visible = true;
                    return;
                }      
                if (!CheckInvoiceCount(ReadData.Count) && lblEx.Visible == false)  //1.檢查發票配號
                {
                    lblEx.Text = intYear.ToString() + "年" +
                        (intPeriod * 2 - 1).ToString() + "~" + 
                        (intPeriod * 2).ToString() + "期之發票號碼不足,請設定該期電子發票號碼!";
                    lblEx.Visible = true;
                    return;
                }
                if (!CheckSeriaKeyByFile() && lblEx.Visible == false)                    //2.Key不能重複
                {
                    DataErrShow();
                    lblEx.Text = "唯一識別碼Key重複!";
                    lblEx.Visible = true;
                    return;
                }
                //if (!CheckData() && lblEx.Visible == false)                      //3.欄位檢查
                //{
                //    DataErrShow();
                //    lblEx.Text = "寫入失敗,欄位格式錯誤!!";
                //    lblEx.Visible = true;
                //    return;
                //}                
                ToBindData(true);
                Session["ReadData"] = ReadData;
            }
        }

        protected void DataErrShow()
        {
            List<ToReadData> ErrReadData = new List<ToReadData>();
            int runCount = 0;
            foreach (ToReadData tempReadData in ReadData)
            {
                runCount = runCount + 1;
                if (tempReadData.DataMessage != "")
                {
                    tempReadData.DataMessage = "第" + runCount.ToString() + "筆: " + tempReadData.DataMessage;
                    ErrReadData.Add(tempReadData);
                }
            }
            string strTotalCount = ReadData.Count.ToString();
            string strYesCount = (ReadData.Count - ErrReadData.Count).ToString();
            string strNoCount = ErrReadData.Count.ToString();
            ReadData.Clear();
            ReadData = ErrReadData;
            Session["ReadData"] = ReadData;
            ToBindData(true);
            lblTotalCount.Text = strTotalCount;
            lblYesCount.Text = strYesCount;
            lblNoCount.Text = strNoCount;
            btnAddCode.Visible = false;
        }

        protected void btnAddCode_Click(object sender, EventArgs e)
        {
            try
            {
                //檢核成功寫入資料庫
                ToShareNumber();
                ToSaveDB();
                var tmpData = ReadData.Where(r => r.BuyerUniformNumber == "N/A");
                tmpData.Select(r => r.InvoiceID).SendIssuingNotification();
                ToBindData(true);
                btnPrint.Visible = true;
                btnExcell.Visible = btnPrint.Visible;
                btnAddCode.Visible = false;
                btnReset.Visible = false;
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                lblEx.Text ="寫入資料庫錯誤!請聯絡系統管理人員!";
                lblEx.Visible = true;
                btnReset.Visible = true;
                btnAddCode.Visible = false;
            }
            
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Session["ReadData"] = null;
            FormDisplay(false);
        }

        protected void btnExcell_Click(object sender, EventArgs e)
        {
            ReadData = (List<ToReadData>)Session["ReadData"];
            string[] StrCumName = { "序號", "GoogleID", "發票號碼", "買受人名稱", "買受人統編", "未稅金額", "稅額", "含稅金額", "匯入狀態" };
            ExportDataToCsv(ReadData, "InvoiceImportFile.csv", StrCumName, true);
//            Page.SaveControlAsDownload(DIV2, "InvoiceFile.xls");
        }

        #region 匯出csv檔案     
        /// <summary>
        /// Exports the data table to CSV.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="ColumnName">Name of the column.</param>
        /// <param name="HasColumnName">if set to <c>true</c> [has column name].</param>
        public void ExportDataToCsv(List<ToReadData> dt, string FileName, string[] ColumnName, bool HasColumnName)
        {
            string strValue = string.Empty;
            //CSV 匯出的標題 要先塞一樣的格式字串 充當標題
            if (HasColumnName == true)
                strValue = string.Join(",", ColumnName);
            for (int i = 0; i < dt.Count; i++)
            {
                strValue = strValue + Environment.NewLine;
                strValue = strValue + dt[i].SerialNumber + ",";
                strValue = strValue + dt[i].GoogleID + ",";
                strValue = strValue + dt[i].InvoiceTrackCode + dt[i].InvoiceNo + ",";
                strValue = strValue + dt[i].BuyerName + ",";
                strValue = strValue + dt[i].BuyerUniformNumber + ",";
                strValue = strValue + dt[i].BuyAmount + ",";
                strValue = strValue + dt[i].BuyTax + ",";
                strValue = strValue + dt[i].BuyAmountTax + ",";
                strValue = strValue + dt[i].DataMessage;
            }
            //存成檔案
            string strFile = FileName;
            if (!string.IsNullOrEmpty(strValue))
            {
                strValue = strValue.ToSimplified();
                //File.WriteAllText(strFile, strValue, Encoding.Default);
                Page.Response.Clear();
                Page.Response.Buffer = true;
                Page.Response.AddHeader("content-disposition", "attachment;filename=InvoiceFile.csv");
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

                
    }
}