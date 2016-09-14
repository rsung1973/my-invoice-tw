using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using InvoiceClient.Agent;
using InvoiceClient.Properties;
using InvoiceClient.TransferManagement;
using Utility;

namespace InvoiceClient.Helper
{
    public class AppSigner
    {
        private X509Certificate2 _signerCert;

        private static AppSigner _instance = new AppSigner();

        private AppSigner()
        {

        }

        private void prepareCertificate()
        {
            if (!String.IsNullOrEmpty(Settings.Default.ActivationKey))
            {
                getCertificateRemote();
            }
            else
            {
                getCertificateLocal();
            }
        }

        private void getCertificateRemote()
        {
            Settings.Default.InvoiceTxnPath.CheckStoredPath();
            String certFile = Path.Combine(Settings.Default.InvoiceTxnPath, "UXSigner.pfx");
            if (!File.Exists(certFile))
            {
                WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();
                String certContent = invSvc.GetSignerCertificateContent(Settings.Default.ActivationKey);
                if (!String.IsNullOrEmpty(certContent))
                {
                    using (FileStream fs = new FileStream(certFile, FileMode.CreateNew, FileAccess.Write))
                    {
                        byte[] buf = Convert.FromBase64String(certContent);
                        fs.Write(buf, 0, buf.Length);
                        fs.Flush();
                        fs.Close();
                    }
                }
            }

            if (File.Exists(certFile))
            {
                Guid keyID;
                if (Guid.TryParse(Settings.Default.ActivationKey, out keyID))
                {
                    _signerCert = new X509Certificate2(certFile, keyID.ToString().Substring(0, 8));
                }
            }
        }

        private void getCertificateLocal()
        {
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            certStore.Open(OpenFlags.ReadOnly);

            foreach (X509Certificate2 signerCert in certStore.Certificates)
            {
                if (signerCert.Subject.IndexOf(Settings.Default.SignerSubjectName) >= 0)
                {
                    _signerCert = signerCert;
                    break;
                }
            }

            certStore.Close();
        }

        public static X509Certificate2 SignerCertificate
        {
            get
            {
                lock (_instance)
                {
                    if (_instance._signerCert == null)
                        _instance.prepareCertificate();
                    return _instance._signerCert;
                }
            }
        }

        public static byte[] CheckCertificate(String activationKey)
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();
            String certContent = invSvc.GetSignerCertificateContent(activationKey);
            if (!String.IsNullOrEmpty(certContent))
            {
                return Convert.FromBase64String(certContent);
            }
            return null;
        }

        public static bool ResetCertificate(String activationKey)
        {
            var rawData = CheckCertificate(activationKey);
            if (rawData != null)
            {
                Settings.Default["ActivationKey"] = activationKey;
                Settings.Default.InvoiceTxnPath.CheckStoredPath();
                String certFile = Path.Combine(Settings.Default.InvoiceTxnPath, "UXSigner.pfx");
                using (FileStream fs = new FileStream(certFile, FileMode.Create, FileAccess.Write))
                { 
                    fs.Write(rawData, 0, rawData.Length);
                    fs.Flush();
                    fs.Close();
                }
                _instance._signerCert = new X509Certificate2(rawData, activationKey.Substring(0, 8));
                ResetSellerReceiptNo();
                return true;
            }
            return false;
        }

        public static void ResetSellerReceiptNo()
        {
            var item = ServerInspector.GetRegisterdMember();
            if (item != null)
            {
                Settings.Default["SellerReceiptNo"] = item.ReceiptNo;
            }
        }

        public static void InstallRootCA()
        {
            if (!String.IsNullOrEmpty(Settings.Default.RootCA))
            {
                try
                {
                    X509Certificate2 ca = new X509Certificate2(Settings.Default.RootCA);
                    X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadWrite);
                    var cert = store.Certificates.Cast<X509Certificate2>().Where(c => c.Thumbprint == ca.Thumbprint).FirstOrDefault();
                    if (cert == null)
                    {
                        store.Add(ca);
                    }
                    store.Close();

                    store = new X509Store(StoreName.CertificateAuthority, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadWrite);
                    cert = store.Certificates.Cast<X509Certificate2>().Where(c => c.Thumbprint == ca.Thumbprint).FirstOrDefault();
                    if (cert == null)
                    {
                        store.Add(ca);
                    }
                    store.Close();

                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }

    }
}
