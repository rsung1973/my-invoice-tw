using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Uxnet.Web.WebUI;
using Model.DataEntity;
using Model.Security.MembershipManagement;
using Utility;
using Model.Locale;
using Business.Helper;

namespace eIVOGo.SAM
{
    public partial class CreateCSV : System.Web.UI.Page
    {
        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            createCSV();
        }

        private void createCSV()
        {            
            string data = Request["printData"].ToString();
            String[] ar = data.Split(',');
            if (ar != null && ar.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                string file_name = "";
                file_name = String.Format("{0:yyyy-MM-dd}.csv", DateTime.Today);
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + file_name);                
                Response.ContentEncoding = Encoding.GetEncoding("UTF-16LE");
                sb.Append("SellerName,SellerAddress,CSNum,RcpAddress,Recipient,InvoiceNum,CheckNum,BuyerName,BuyerId,BuyerAddr,InvoicePT,InvoiceYear,InvoiceMonth,InvoiceDay,Description1,Quantity1,UnitPrice1,Amount1,Remark1,Description2,Quantity2,UnitPrice2,Amount2,Remark2,Description3,Quantity3,UnitPrice3,Amount3,Remark3,Description4,Quantity4,UnitPrice4,Amount4,Remark4,Description5,Quantity5,UnitPrice5,Amount5,Remark5,Description6,Quantity6,UnitPrice6,Amount6,Remark6,Description7,Quantity7,UnitPrice7,Amount7,Remark7,Description8,Quantity8,UnitPrice8,Amount8,Remark8,Description9,Quantity9,UnitPrice9,Amount9,Remark9,SalesAmount,TaxableType,ZeroTaxType,NoTaxType,TaxAmount,TotalAmount,TotalAmtNT,LetterNum,personID");
                sb.Append("\n");

                foreach (string id in ar)
                {
                    var item = dsEntity.CreateDataManager().EntityList.Where(i => i.InvoiceID == int.Parse(id)).FirstOrDefault();

                    sb.Append(item.Organization.CompanyName);//賣方公司名稱
                    sb.Append(",");
                    sb.Append(item.Organization.Addr);//賣方公司地址
                    sb.Append(",");
                    sb.Append(item.Organization.OrganizationStatus.SetToOutsourcingCS == true ? "委外客服電話：0800-010-026" : "");//委外客服電話
                    sb.Append(",");
                    sb.Append(item.InvoiceBuyer.Address);//收件人地址
                    sb.Append(",");
                    sb.Append(item.InvoiceBuyer.ContactName);//收件人名稱
                    sb.Append(",");
                    sb.Append(item.TrackCode + item.No);//發票號碼
                    sb.Append(",");
                    sb.Append(item.CheckNo);//檢查號碼
                    sb.Append(",");
                    sb.Append(item.InvoiceBuyer.IsB2C() ? "" : item.InvoiceBuyer.CustomerName);//買受人名稱
                    sb.Append(",");
                    sb.Append(item.InvoiceBuyer.IsB2C() ? "" : item.InvoiceBuyer.ReceiptNo);//買受人統編
                    sb.Append(",");
                    //sb.Append(item.InvoiceBuyer.IsB2C() ? "" : item.InvoiceBuyer.Address);//買受人地址(取消不顯示)
                    sb.Append("");//買受人地址
                    sb.Append(",");
                    sb.Append(item.CDS_Document.DocumentPrintLogs.Any(l => l.TypeID == (int)Model.Locale.Naming.DocumentTypeDefinition.E_Invoice) ? "副本" : "正本");//發票正副本
                    sb.Append(",");
                    sb.Append(item.InvoiceDate.Value.Year - 1911);//發票開立民國年
                    sb.Append(",");
                    sb.Append(String.Format("{0:00}", item.InvoiceDate.Value.Month));//發票開立月
                    sb.Append(",");
                    sb.Append(String.Format("{0:00}", item.InvoiceDate.Value.Day));//發票開立日
                    sb.Append(",");

                    //品名、數量、單價、金額、備註 (最多9筆)
                    int itemCount = 0;
                    foreach (var detail in item.InvoiceDetails)
                    {
                        if (detail.InvoiceProduct.InvoiceProductItem.Count() > 9)
                        {
                            sb.Append("請參閱附件檔");
                            for (int i = 0; i < 4; i++)
                            {
                                sb.Append(",");
                            }
                            itemCount += 1;
                            break;
                        }
                        else
                        {
                            sb.Append(String.Format("{0}.{1}", detail.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == detail.ProductID).FirstOrDefault().No + 1, detail.InvoiceProduct.Brief));
                            sb.Append(",");
                            sb.Append(String.Format("\"{0:##,###,###,###}\"", detail.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == detail.ProductID).FirstOrDefault().Piece));
                            sb.Append(",");
                            sb.Append(String.Format("\"{0:##,###,###,###}\"", detail.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == detail.ProductID).FirstOrDefault().UnitCost));
                            sb.Append(",");
                            sb.Append(String.Format("\"{0:##,###,###,###}\"", detail.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == detail.ProductID).FirstOrDefault().CostAmount));
                            sb.Append(",");
                            sb.Append(detail.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == detail.ProductID).FirstOrDefault().Remark);
                            sb.Append(",");
                            itemCount += 1;
                        }
                    }

                    //補細項的欄位
                    if (itemCount < 9)
                    {
                        for (int i = 0; i < (9 - itemCount) * 5; i++)
                        {
                            sb.Append(",");
                        }
                    }

                    sb.Append(String.Format("\"{0:##,###,###,###.00}\"", item.InvoiceAmountType.SalesAmount));//銷售額合計
                    sb.Append(",");
                    sb.Append(item.InvoiceAmountType.TaxType == (byte)1 ? "V" : "");//應稅別
                    sb.Append(",");
                    sb.Append(item.InvoiceAmountType.TaxType == (byte)2 ? "V" : "");//零稅率別
                    sb.Append(",");
                    sb.Append(item.InvoiceAmountType.TaxType == (byte)3 ? "V" : "");//免稅別
                    sb.Append(",");
                    sb.Append(String.Format("\"{0:##,###,###,###.00}\"", item.InvoiceAmountType.TaxAmount));//稅額
                    sb.Append(",");
                    sb.Append(String.Format("\"{0:##,###,###,###.00}\"", item.InvoiceAmountType.TotalAmount));//總計
                    sb.Append(",");

                    //總計新台幣
                    char[] _totalAmtChar = ((int)item.InvoiceAmountType.TotalAmount.Value).GetChineseNumberSeries(8);
                    sb.Append(String.Format("{0} 仟 {1} 佰 {2} 拾 {3} 萬 {4} 仟 {5} 佰 {6} 拾 {7} 元 零 角 零 分 元整",
                        _totalAmtChar[7].ToString(), _totalAmtChar[6].ToString(), _totalAmtChar[5].ToString(),
                        _totalAmtChar[4].ToString(), _totalAmtChar[3].ToString(), _totalAmtChar[2].ToString(),
                        _totalAmtChar[1].ToString(), _totalAmtChar[0].ToString()));
                    sb.Append(",");
                    sb.Append("本發票依" + (String.IsNullOrEmpty(item.Organization.OrganizationStatus.AuthorizationNo) ? "臺北市國稅局信義分局100年7月25日財北國稅信義營業字第1000213447號" : item.Organization.OrganizationStatus.AuthorizationNo) + "函核准使用。");//核准函號
                    sb.Append(",");
                    sb.Append(item.InvoiceBuyer.IsB2C() ? String.Format("個人識別碼:{0}", item.InvoiceBuyer.Name) : "");
                    sb.Append("\n");
                }

                //寫入PrintLog
                creatPrintLog(ar);

                //Response.Write("<meta http-equiv=Content-Type content=text/csv;charset=Unicode>");
                Response.Write(sb.ToString());
                Response.Flush();
                Response.End();
            }
          
        }

        void creatPrintLog(String[] IDs)
        {
            if (IDs != null && IDs.Count() > 0)
            {
                var mgr = dsEntity.CreateDataManager();
                var logTable = mgr.GetTable<DocumentPrintLog>();
                foreach (string id in IDs)
                {
                    InvoiceItem _item = mgr.EntityList.Where(i => i.InvoiceID == int.Parse(id)).FirstOrDefault();
                    if (!logTable.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice & l.DocID==_item.InvoiceID))
                    {
                        logTable.InsertOnSubmit(new DocumentPrintLog { 
                            DocID=_item.InvoiceID,
                            PrintDate = DateTime.Now,
                            UID = _userProfile.UID,
                            TypeID = (int)Naming.DocumentTypeDefinition.E_Invoice
                        });
                    }
                }
                mgr.SubmitChanges();
            }       

        }
    }
}