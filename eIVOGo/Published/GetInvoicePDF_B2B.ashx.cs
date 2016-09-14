using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Schema.TXN;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using Utility;
using Uxnet.Com.Security.UseCrypto;
using System.Net;

namespace eIVOGo.Published
{
    /// <summary>
    /// GetInvoicePDF_B2B 的摘要描述
    /// </summary>
    public class GetInvoicePDF_B2B : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/pdf";
            HttpResponse Response = context.Response;
            HttpRequest Request = context.Request;
            HttpServerUtility Server = context.Server;

            try
            {
                CryptoUtility crypto = new CryptoUtility();
                XmlDocument sellerInfo = new XmlDocument();
                sellerInfo.Load(Request.InputStream);
                if (crypto.VerifyXmlSignature(sellerInfo))
                {
                    using (InvoiceManager mgr = new InvoiceManager())
                    {
                        ///憑證資料檢查
                        ///
                        var token = mgr.GetTable<OrganizationToken>().Where(t => t.Thumbprint == crypto.SignerCertificate.Thumbprint).FirstOrDefault();
                        if (token != null)
                        {
                            Root root = sellerInfo.ConvertTo<Root>();

                            com.uxb2b.util.CipherDecipherSrv cipher = new com.uxb2b.util.CipherDecipherSrv();
                            var invoiceNo = cipher.decipher(Request.Params["QUERY_STRING"]);
                            var item = mgr.GetTable<InvoiceItem>().Where(i => i.TrackCode == invoiceNo.Substring(0, 2)
                                && i.No == invoiceNo.Substring(2, 8)
                                && i.SellerID == token.CompanyID).FirstOrDefault();

                            if (item != null)
                            {
                                String pdfFile = Path.Combine(Logger.LogPath.GetDateStylePath(item.InvoiceDate.Value), String.Format("{0}{1}.pdf", item.TrackCode, item.No));

                                if (File.Exists(pdfFile))
                                {
                                    Response.WriteFileAsDownload(pdfFile, null, false, "application/pdf");
                                }
                                else
                                {
                                    String tmpFile;
                                    using (WebClient client = new WebClient())
                                    {
                                        client.Encoding = System.Text.Encoding.UTF8;
                                        tmpFile = client.DownloadString(String.Format("{0}{1}?nameOnly=true&id={2}", eIVOGo.Properties.Settings.Default.mailLinkAddress, VirtualPathUtility.ToAbsolute("~/Published/PrintSingleInvoiceAsPDF.aspx"), item.InvoiceID));

                                    }
                                    File.Move(tmpFile, pdfFile);
                                    Response.WriteFileAsDownload(pdfFile, null, false, "application/pdf");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
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