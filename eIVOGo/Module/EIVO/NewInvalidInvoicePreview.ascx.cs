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

namespace eIVOGo.Module.EIVO
{
    public partial class NewInvalidInvoicePreview : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;

            if (Request["id"] != null)
            {
                string id = Request["id"];
                buildQuery();
                getInvoiceData(id.Trim());
            }
         
        }

        public Expression<Func<InvoiceItem, bool>> QueryExpr
        {
            get;
            set;
        }

        protected virtual void buildQuery()
        {
            
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
                    this.lblInvoiceRange.Text = getDateRange(detail.InvoiceDate.Value);
                    this.lblInvoiceNO.Text = detail.TrackCode + (string.IsNullOrEmpty(detail.DonationID.ToString()) ? detail.No : detail.No.Substring(0, 5) + "***");
                    this.lblSellerCompany.Text = detail.Organization.CompanyName;
                    this.lblSellerReceiptNO.Text = detail.Organization.ReceiptNo;
                    this.lblAddr.Text = detail.Organization.Addr;
                    this.lblInvoiceDate.Text = Utility.ValueValidity.ConvertChineseDateString(detail.InvoiceDate.Value) + String.Format(" {0:00}:{1:00}:{2:00}", detail.InvoiceDate.Value.Hour, detail.InvoiceDate.Value.Minute, detail.InvoiceDate.Value.Second);
                    this.lblInvoicePrice.Text = String.Format("{0:0,0.00}", detail.InvoiceAmountType.TotalAmount);
                    this.lblCarrierType.Text = detail.InvoiceByHousehold == null ? "" : detail.InvoiceByHousehold.InvoiceUserCarrier.InvoiceUserCarrierType.CarrierType;
                    this.lblBuyerReceiptNo.Text = detail.InvoiceBuyer.ReceiptNo;

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
                        this.gvEntity.DataSource = data.ToList();
                        this.gvEntity.DataBind();
                    }
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
    }
}