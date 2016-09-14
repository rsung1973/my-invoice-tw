using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uxnet.Com.Security.UseCrypto;
using Model.InvoiceManagement;
using Model.DataEntity;
using Model.Locale;

namespace Model.Helper
{
    public class PKCS7Log : dsPKCS7
    {
        public Naming.CACatalogDefinition Catalog { get; set; }
        public int? OwnerID { get; set; }
        public int? DocID { get; set; }
        public Naming.DocumentTypeDefinition? TypeID { get; set; }

        public CryptoUtility Crypto
        { get; set; }

        public override string GetFileName(string currentLogPath, string qName, ulong key)
        {
            String path = base.GetFileName(currentLogPath, qName, key);

            if(OwnerID.HasValue)
            {
                using (InvoiceManager mgr = new InvoiceManager())
                {
                    var item = new CALog
                    {
                        Catalog = (int)Catalog,
                        ContentPath = path,
                        LogDate = DateTime.Now,
                        TypeID = (int?)TypeID,
                        CompanyID = OwnerID,
                        DocID = DocID
                    };

                    //if (DocID.HasValue)
                    //{
                    //    item.DocID = DocID;
                    //}
                    //else
                    //{ 
                    //    item.CDS_Document = new CDS_Document
                    //    {
                    //        DocumentOwner = new DocumentOwner
                    //        {
                    //            OwnerID = OwnerID.Value
                    //        },
                    //        DocDate = DateTime.Now
                    //    };
                    //}

                    mgr.GetTable<CALog>().InsertOnSubmit(item);

                    mgr.SubmitChanges();
                }
            }
            else if (Crypto != null && Crypto.SignerCertificate != null)
            {
                using (InvoiceManager mgr = new InvoiceManager())
                {
                    var org = mgr.GetOrganizationByThumbprint(Crypto.SignerCertificate.Thumbprint);
                    if (org != null)
                    {
                        var item = new CALog
                        {
                            Catalog = (int)Catalog,
                            ContentPath = XmlSignature != null ? Crypto.CA_Log.DataSignature : path,
                            LogDate = DateTime.Now,
                            TypeID = (int?)TypeID,
                            CompanyID = org.CompanyID,
                            DocID = DocID
                        };

                        //if (DocID.HasValue)
                        //{
                        //    item.DocID = DocID;
                        //}
                        //else
                        //{ 
                        //    item.CDS_Document = new CDS_Document
                        //    {
                        //        DocumentOwner = new DocumentOwner
                        //        {
                        //            OwnerID = org.CompanyID
                        //        },
                        //        DocDate = DateTime.Now
                        //    };
                        //}
                        mgr.GetTable<CALog>().InsertOnSubmit(item);
                        mgr.SubmitChanges();
                    }
                }

            }

            return path;
        }
    }
}
