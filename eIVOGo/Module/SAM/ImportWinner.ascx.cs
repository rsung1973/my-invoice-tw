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
using Model.DataEntity;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Utility;
using System.Text;

namespace eIVOGo.Module.SAM
{
    public partial class ImportWinner : System.Web.UI.UserControl
    {
        UserProfileMember _userProfile;

        [Serializable()] 
        public class dataType
        {
            public string TrackCode;
            public string No;
            public string CompanyName;
            public string ReceiptNo;
            public string WinningType;
            public int BonusDescription;
            public string DepositMK;
            public string DataType;
            public string isMatch;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        protected override void OnInit(EventArgs e)
        {
            gvEntity.PreRender += new EventHandler(gvEntity_PreRender);
        }

        #region "Page Control Event"
        protected void btnImport_Click(object sender, EventArgs e)
        {
            ViewState["CorrectData"] = null;
            ViewState["ErrorData"] = null;
            if (this.FileUpload1.FileName.Length >0)
            {
                string result = ParseData();
                if (result.Equals("ok"))
                {
                    getDataToGridview();
                }
                else if (result.Equals("ErrorData"))
                {
                    getDataToGridview();
                }
            }
            else
            {
                this.lblError.Text = "請選擇匯入檔案!!";
                this.lblError.Visible = true;
            }
        }
        #endregion

        #region "Gridview Event"
        void gvEntity_PreRender(object sender, EventArgs e)
        {
            getDataToGridview();
        }
        #endregion

        #region "Import and Get Data"
        /// <summary>
        /// 將下載的中獎清單內容Parse成DataList資料,並暫存在ViewState中
        /// </summary>
        /// <returns>傳回處理狀態,判斷下載的內容是否全數正確或有錯誤</returns>
        private string ParseData()
        {
            string result = "";
            try
            {
                List<dataType> CorrectData = new List<dataType>();
                List<dataType> ErrorData = new List<dataType>();
                byte[] myData = this.FileUpload1.PostedFile.InputStream.ReadLine();
                while (myData != null && myData.Length > 0)
                {
                    if (myData.Length >= 446)
                    {
                        _GovWinInvoiceNOList list = myData.ByteArrayToStructure<_GovWinInvoiceNOList>();
                        using (InvoiceManager im = new InvoiceManager())
                        {
                            var iwnTB = im.GetTable<InvoiceWinningNumber>();
                            var ipaTB = im.GetTable<InvoicePrintAssertion>();

                            var invoiceData = im.EntityList.Where(d => d.TrackCode.Equals(Encoding.Default.GetString(list.InvoiceAxle).Trim()) & d.No.Equals(Encoding.Default.GetString(list.InvoiceNumber).Trim())).FirstOrDefault();
                            if (invoiceData != null)
                            {
                                CorrectData.Add(new dataType
                                {
                                    TrackCode = Encoding.Default.GetString(list.InvoiceAxle).Trim(),
                                    No = Encoding.Default.GetString(list.InvoiceNumber).Trim(),
                                    CompanyName = Encoding.Default.GetString(list.Name).Trim(),
                                    ReceiptNo = Encoding.Default.GetString(list.BAN).Trim(),
                                    WinningType = Encoding.Default.GetString(list.PrizeType).Trim(),
                                    BonusDescription = int.Parse(Encoding.Default.GetString(list.PrizeAmt).Trim()),
                                    DepositMK = Encoding.Default.GetString(list.DepositMK).Trim(),
                                    DataType = Encoding.Default.GetString(list.DataType).Trim(),
                                    isMatch = "正確"
                                });
                            }
                            else
                            {
                                ErrorData.Add(new dataType
                                {
                                    TrackCode = Encoding.Default.GetString(list.InvoiceAxle).Trim(),
                                    No = Encoding.Default.GetString(list.InvoiceNumber).Trim(),
                                    CompanyName = Encoding.Default.GetString(list.Name).Trim(),
                                    ReceiptNo = Encoding.Default.GetString(list.BAN).Trim(),
                                    WinningType = Encoding.Default.GetString(list.PrizeType).Trim(),
                                    BonusDescription = int.Parse(Encoding.Default.GetString(list.PrizeAmt).Trim()),
                                    DepositMK = Encoding.Default.GetString(list.DepositMK).Trim(),
                                    DataType = Encoding.Default.GetString(list.DataType).Trim(),
                                    isMatch = "錯誤"
                                });
                            }
                        }
                    }
                    myData = this.FileUpload1.PostedFile.InputStream.ReadLine();
                }

                if (CorrectData.Count > 0 & ErrorData.Count < 1)
                {
                    ViewState["CorrectData"] = CorrectData;
                    if (ImportToDB(CorrectData))
                    { result = "ok"; }
                    else
                    {
                        result = "fail";
                        ViewState["CorrectData"] = null;
                    }
                }

                if (ErrorData.Count > 0)
                {
                    ViewState["ErrorData"] = ErrorData;
                    result = "ErrorData";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.lblError.Text = "系統錯誤:" + ex.Message;
                this.lblError.Visible = true;
            }
            return result;
        }

        /// <summary>
        /// 將暫存在ViewState中的資料轉存到頁面的GridView中
        /// </summary>
        private void getDataToGridview()
        {
            try
            {
                if (ViewState["CorrectData"] != null & ViewState["ErrorData"] == null)
                {
                    List<dataType> dl = (List<dataType>)ViewState["CorrectData"];

                    this.gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(this.gvEntity, 0);
                    this.gvEntity.DataSource = dl;
                    this.gvEntity.DataBind();
                    this.gvEntity.Columns[5].Visible = false;

                    PagingControl paging = (PagingControl)this.gvEntity.BottomPagerRow.Cells[0].FindControl("pagingIndex");
                    paging.RecordCount = dl.Count();
                    paging.CurrentPageIndex = this.gvEntity.PageIndex;

                    this.lblError.Text = "全數資料匯入完成!!";
                    this.lblError.Visible = true;
                }

                if (ViewState["ErrorData"] != null)
                {
                    List<dataType> dl = (List<dataType>)ViewState["ErrorData"];
                    this.gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(this.gvEntity, 0);
                    this.gvEntity.DataSource = dl;
                    this.gvEntity.DataBind();

                    PagingControl paging = (PagingControl)this.gvEntity.BottomPagerRow.Cells[0].FindControl("pagingIndex");
                    paging.RecordCount = dl.Count();
                    paging.CurrentPageIndex = this.gvEntity.PageIndex;

                    this.lblError.Text = "匯入資料有誤,請修正後重新匯入!!";
                    this.lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.lblError.Text = "系統錯誤:" + ex.Message;
                this.lblError.Visible = true;
            }
        }

        /// <summary>
        /// 將完整無誤的資料存入中獎清冊的DB中
        /// </summary>
        /// <param name="data">Parse出來正確的資料</param>
        /// <returns>回傳寫入DB是否成功或失敗</returns>
        private Boolean ImportToDB(List<dataType> data)
        {
            Boolean result = false;
            try
            {
                using (InvoiceManager im = new InvoiceManager())
                {
                    var iwnTB = im.GetTable<InvoiceWinningNumber>();
                    var ipaTB = im.GetTable<InvoicePrintAssertion>();
                    foreach (var d in data)
                    {
                        var invoiceData = im.EntityList.Where(i => i.TrackCode.Equals(d.TrackCode.Trim()) & i.No.Equals(d.No.Trim())).FirstOrDefault();
                        byte oddMonth;
                        byte evenMonth;
                        if (invoiceData.InvoiceDate.Value.Month % 2 == 0)
                        {
                            oddMonth = (byte)(invoiceData.InvoiceDate.Value.Month - 1);
                            evenMonth = (byte)invoiceData.InvoiceDate.Value.Month;
                        }
                        else
                        {
                            oddMonth = (byte)invoiceData.InvoiceDate.Value.Month;
                            evenMonth = (byte)(invoiceData.InvoiceDate.Value.Month + 1);
                        }

                        var winNumData=im.GetTable<InvoiceWinningNumber>().Where(w => w.InvoiceID == invoiceData.InvoiceID);
                        if (winNumData.Count() > 0)
                        {

                        }
                        else
                        {
                            iwnTB.InsertOnSubmit(new InvoiceWinningNumber
                            {
                                InvoiceID = invoiceData.InvoiceID,
                            });

                            if (d.DepositMK.Trim().Equals("Y"))
                            {
                                ipaTB.InsertOnSubmit(new InvoicePrintAssertion
                                {
                                    InvoiceID = invoiceData.InvoiceID,
                                    PrintDate = DateTime.Now
                                });
                            }
                        }
                    }
                    im.SubmitChanges();
                }
                result = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.lblError.Text = "系統錯誤:" + ex.Message;
                this.lblError.Visible = true;
            }
            return result;
        }
        #endregion

        public string WinningTypeTransform(object a)
        {
            string result = "";
            string _temp = ((string)a).Trim();
            if (_temp.Equals("0"))
                result = "特獎";
            else if (_temp.Equals("1"))
                result = "頭獎";
            else if (_temp.Equals("2"))
                result = "二獎";
            else if (_temp.Equals("3"))
                result = "三獎";
            else if (_temp.Equals("4"))
                result = "四獎";
            else if (_temp.Equals("5"))
                result = "五獎";
            else if (_temp.Equals("6"))
                result = "六獎";
            return result;
        }
    }    
}