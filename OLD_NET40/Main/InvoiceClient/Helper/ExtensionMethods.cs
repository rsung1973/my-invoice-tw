using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Xml;

using InvoiceClient.Properties;
using Model.Schema.TXN;
using Uxnet.Com.Security.UseCrypto;
using Utility;

namespace InvoiceClient.Helper
{
    public static class ExtensionMethods
    {
        public static XmlDocument Sign(this XmlDocument docMsg)
        {

            CryptoUtility.SignXml(docMsg, Settings.Default.SignerCspName,
                Settings.Default.SignerKeyPassword, AppSigner.SignerCertificate);

            return docMsg;
        }

        public static SignedCms Sign(this byte[] data,bool detached = false)
        {
            ContentInfo content = new ContentInfo(data);
            SignedCms signedCms = new SignedCms(content, detached);
            CmsSigner signer = new CmsSigner(AppSigner.SignerCertificate);

            signedCms.ComputeSignature(signer, true);
            return signedCms;
        }

        public static SignedCms SignFile(this String fileName, bool detached = false)
        {
            byte[] data;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                if (fs.Length > Int32.MaxValue)
                {
                    throw new Exception("檔案太大無法處理!!");
                }
                data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);
            }
            return data.Sign(detached);
        }

        public static Root CreateMessageToken(this object obj, String actionName)
        {
            return new Root
            {
                UXB2B = "電子發票系統",
                Request = new RootRequest
                {
                    actionName = actionName,
                    periodicalIntervalSpecified = true,
                    periodicalInterval = Settings.Default.AutoInvServiceInterval > 0 ? Settings.Default.AutoInvServiceInterval * 60 : 1800
                }
            };
        }

    }

    public interface ITabWorkItem
    {
        String TabName {  get; }
        String TabText {  get; }
    }
}
