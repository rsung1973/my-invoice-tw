<%@ WebHandler Language="C#" Class="eIVOGo.Published.SMSInvoiceNotification" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using Model.InvoiceManagement;
using Model.DataEntity;
using Model.Locale;


namespace eIVOGo.Published
{
    /// <summary>
    ///SMSInvoiceNotification 的摘要描述
    /// </summary>
    public class SMSInvoiceNotification : IHttpHandler
    {
        HttpResponse Response;
        HttpRequest Request;

        public void ProcessRequest(HttpContext context)
        {
            Response = context.Response;
            Request = context.Request;
            
            Response.ContentType = "text/plain";

            int docID;
            if (!String.IsNullOrEmpty(Request["id"]) && int.TryParse(Request["id"], out docID))
            {
                using (InvoiceManager mgr = new InvoiceManager())
                {
                    var item = mgr.GetTable<CDS_Document>().Where(d => d.DocID == docID).FirstOrDefault();
                    if (item != null)
                    {
                        buildContent(item);
                    }
                }
            }
        }

        private void buildContent(CDS_Document item)
        {
            //StringBuilder sb = new StringBuilder("親愛的客戶您好, \r\n您在");
            StringBuilder sb = new StringBuilder();
            switch((Naming.DocumentTypeDefinition)item.DocType)
            {
                case Naming.DocumentTypeDefinition.E_Invoice:
                    //sb.Append(item.InvoiceItem.InvoiceSeller.CustomerName);
                    //sb.Append("採購之").Append(((Naming.B2BInvoiceDocumentTypeDefinition)item.DocType).ToString()).Append("已開立\r\n");
                    //sb.Append("發票號碼:").Append(String.Format("{0}{1}",item.InvoiceItem.TrackCode,item.InvoiceItem.No));
                    //sb.Append("\r\n").Append("開立日期:").Append(String.Format("{0:yyyyMMdd}",item.InvoiceItem.InvoiceDate));
                    //sb.Append("\r\n").Append("消費金額:$").Append(String.Format("{0:.##}",item.InvoiceItem.InvoiceAmountType.TotalAmount));
                    sb.Append(item.InvoiceItem.InvoiceSeller.CustomerName).Append("通知\r\n");
                    sb.Append("您的").Append(((Naming.B2BInvoiceDocumentTypeDefinition)item.DocType).ToString());
                    sb.Append(String.Format("{0}{1}", item.InvoiceItem.TrackCode, item.InvoiceItem.No)).Append("已開立\r\n");
                    sb.Append("消費金額:$").Append(String.Format("{0:.##}", item.InvoiceItem.InvoiceAmountType.TotalAmount)).Append("\r\n");
                    sb.Append("備註:").Append(String.Join("", item.InvoiceItem.InvoiceDetails.Select(d => d.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark)));
                    break;
                case Naming.DocumentTypeDefinition.E_InvoiceCancellation:
                    sb.Append(item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.CustomerName);
                    sb.Append("採購之").Append(((Naming.B2BInvoiceDocumentTypeDefinition)item.DocType).ToString()).Append("已開立\r\n");
                    sb.Append("發票號碼:").Append(String.Format("{0}{1}", item.DerivedDocument.ParentDocument.InvoiceItem.TrackCode, item.DerivedDocument.ParentDocument.InvoiceItem.No));
                    sb.Append("\r\n").Append("開立日期:").Append(String.Format("{0:yyyy/MM/dd}", item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceCancellation.CancelDate));
                    sb.Append("\r\n").Append("消費金額: $").Append(String.Format("{0:.##}", item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceAmountType.TotalAmount));
                    break;
                case Naming.DocumentTypeDefinition.E_Allowance :
                    sb.Append(item.InvoiceAllowance.InvoiceAllowanceBuyer.CustomerName);
                    sb.Append("採購之").Append(((Naming.B2BInvoiceDocumentTypeDefinition)item.DocType).ToString()).Append("已開立\r\n");
                    sb.Append("折讓單號碼:").Append(item.InvoiceAllowance.AllowanceNumber);
                    sb.Append("\r\n").Append("開立日期:").Append(String.Format("{0:yyyy/MM/dd}", item.InvoiceAllowance.AllowanceDate));
                    sb.Append("\r\n").Append("消費金額: $").Append(String.Format("{0:.##}", item.InvoiceAllowance.TotalAmount));
                    break;
                case Naming.DocumentTypeDefinition.E_AllowanceCancellation :
                    sb.Append(item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.CustomerName);
                    sb.Append("採購之").Append(((Naming.B2BInvoiceDocumentTypeDefinition)item.DocType).ToString()).Append("已開立\r\n");
                    sb.Append("折讓單號碼:").Append(item.DerivedDocument.ParentDocument.InvoiceAllowance.AllowanceNumber);
                    sb.Append("\r\n").Append("開立日期:").Append(String.Format("{0:yyyy/MM/dd}", item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceCancellation.CancelDate));
                    sb.Append("\r\n").Append("消費金額: $").Append(String.Format("{0:.##}", item.DerivedDocument.ParentDocument.InvoiceAllowance.TotalAmount));
                    break;
            }
            
            Response.Write(sb.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}