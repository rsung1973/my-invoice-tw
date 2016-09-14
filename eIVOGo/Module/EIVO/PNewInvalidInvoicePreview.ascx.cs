using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;
using Model.InvoiceManagement;
using Uxnet.Com.Utility;
using Utility;
using Business.Helper;
using Model.Security.MembershipManagement;
using System.Linq.Expressions;
using Model.Locale;

namespace eIVOGo.Module.EIVO
{
    public partial class PNewInvalidInvoicePreview : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;

        public class dataType
        {
            public string Type;
            public string Year;
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
                buildQuery(id);
                //getInvoiceData(id);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        public Expression<Func<InvoiceItem, bool>> QueryExpr
        {
            get;
            set;
        }

        protected virtual void buildQuery(string id)
        {
            using (InvoiceManager im = new InvoiceManager())
            {
                var type = im.GetTable<CDS_Document>().Where(c => c.DocID == int.Parse(id)).FirstOrDefault().DocType;
                if (type == (int)Naming.DocumentTypeDefinition.E_Invoice)
                {
                    getInvoiceData(id);
                    this.divInvoice.Visible = true;
                    this.divAllowance.Visible = false;
                }
                else if (type == (int)Naming.DocumentTypeDefinition.E_Allowance)
                {
                    getAllowanceData(id);
                    this.divInvoice.Visible = false;
                    this.divAllowance.Visible = true;
                }
            }
        }

        private void getInvoiceData(string id)
        {
            try
            {
                using (InvoiceManager im = new InvoiceManager())
                {
                    Expression<Func<InvoiceItem, bool>> expr = i => i.InvoiceID == int.Parse(id);
                    if (QueryExpr != null)
                        expr = expr.And(QueryExpr);
                    var detail = im.EntityList.Where(expr).FirstOrDefault();
                    this.lblRandonNo.Text = detail.InvoiceBuyer.IsB2C() ? detail.InvoiceBuyer.Name : "";
                    this.lblInvoiceNO.Text = detail.TrackCode + (detail.DonateMark.Equals("0") ? detail.No : detail.No.Substring(0, 5) + "***");
                    this.lblInvoiceDate.Text = Utility.ValueValidity.ConvertChineseDateString(detail.InvoiceDate.Value);
                    this.lblSalesAmount.Text = String.Format("{0:0,0.00}", detail.InvoiceAmountType.SalesAmount);
                    this.lblTaxAmount.Text = String.Format("{0:0,0.00}", detail.InvoiceAmountType.TaxAmount);
                    this.lblTotalAmount.Text = String.Format("{0:0,0.00}", detail.InvoiceAmountType.TotalAmount);
                    this.lblBuyerReceiptNo.Text = detail.InvoiceBuyer.ReceiptNo.Equals("0000000000") ? "" : detail.InvoiceBuyer.ReceiptNo;
                    var data = from d in im.GetTable<InvoiceDetail>().Where(d => d.InvoiceID == detail.InvoiceID)
                               select new
                               {
                                   Brief = d.InvoiceProduct.Brief,
                                   Piece = d.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == d.ProductID).FirstOrDefault().Piece,
                                   PieceUnit = d.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == d.ProductID).FirstOrDefault().PieceUnit,
                                   UnitCost = d.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == d.ProductID).FirstOrDefault().UnitCost,
                                   CostAmount = d.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == d.ProductID).FirstOrDefault().CostAmount,
                                   Memo = d.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == d.ProductID).FirstOrDefault().Remark
                               };

                    if (data.Count() > 0)
                    {
                        this.lblError.Visible = false;
                        this.gvEntity.DataSource = data.ToList();
                        this.gvEntity.DataBind();
                    }
                    else
                    {
                        this.lblError.Visible = true;
                        this.lblError.Text = "查無明細資料!!";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.lblError.Text = "系統發生錯誤：" + ex.Message;
            }
        }

        private void getAllowanceData(string id)
        {
            try
            {
                using (InvoiceManager im = new InvoiceManager())
                {
                    var main = im.GetTable<InvoiceAllowance>().Where(i => i.AllowanceID == int.Parse(id)).FirstOrDefault();
                    this.lblAllowanceDate.Text = Utility.ValueValidity.ConvertChineseDateString(main.AllowanceDate.Value);
                    this.lblReceiptNo.Text = main.InvoiceAllowanceSeller.Organization.ReceiptNo;
                    this.lblCompanyName.Text = main.InvoiceAllowanceSeller.Organization.CompanyName;
                    this.lblCompanyAddr.Text = main.InvoiceAllowanceSeller.Organization.Addr;
                    var detail = im.GetTable<InvoiceAllowanceDetail>().Where(i => i.AllowanceID == int.Parse(id));
                    this.gvAllowance.DataSource = detail.ToList();
                   // this.gvAllowance.DataSource = detail.Select(i=>i.ItemID).ToList();
                    this.gvAllowance.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.lblError.Text = "系統發生錯誤：" + ex.Message;
            }
        }

        private string getDateRange(DateTime d)
        {
            string dayRange = "";
            int month = d.Month;
            if (month % 2 == 0)
            {
                dayRange = (d.Year - 1911).ToString() + "年 " + (d.Month - 1) + "-" + d.Month + "月";
            }
            else
            {
                dayRange = (d.Year - 1911).ToString() + "年 " + d.Month + "-" + (d.Month + 1) + "月";
            }
            return dayRange;
        }

        protected void gvAllowance_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell = new TableCell();
                HeaderCell.ForeColor=System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                HeaderCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#c99040");
                HeaderCell.Text = "開立發票";
                HeaderCell.ColumnSpan = 6;
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                HeaderCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#c99040");
                HeaderCell.Text = "退貨或折讓內容";
                HeaderCell.ColumnSpan = 5;
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderGridRow.Cells.Add(HeaderCell);

                this.gvAllowance.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }
        }
    }
}