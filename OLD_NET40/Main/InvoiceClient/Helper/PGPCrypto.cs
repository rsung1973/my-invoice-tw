using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class PGPCrypto
    {

        private static PGPCrypto _instance = new PGPCrypto();

        private String[] _encryptCmd, _decryptCmd;

        private PGPCrypto()
        {
            initialize();
        }

        private void initialize()
        {
            String fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pgp_decrypt.txt");
            if (File.Exists(fileName))
            {
                _decryptCmd = File.ReadAllLines(fileName);

                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pgp_encrypt.txt");
                if (File.Exists(fileName))
                {
                    _encryptCmd = File.ReadAllLines(fileName);
                    Ready = true;
                }
            }

        }

        public bool Ready
        {
            get;
            private set;
        }

        public ProcessStartInfo CreateDecrypt(String inProgressPath, String encFilePath, String outFilePath = null)
        {
            if (_decryptCmd != null && _decryptCmd.Length > 2)
            {
                return new ProcessStartInfo
                {
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    FileName = _decryptCmd[0],
                    Arguments = outFilePath == null
                        ? String.Format(_decryptCmd[1], encFilePath)
                        : String.Format(_decryptCmd[2], outFilePath, encFilePath),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = inProgressPath
                };
            }
            return null;
        }

        public ProcessStartInfo CreateEncrypt(String fileName)
        {
            if (_decryptCmd != null && _decryptCmd.Length > 2)
            {
                return new ProcessStartInfo
                {
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    FileName = _encryptCmd[0],
                    Arguments = String.Format(_encryptCmd[1], fileName),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetDirectoryName(fileName)
                };
            }
            return null;
        }

        public void EncryptFile(String fileName)
        {
            var encrypt = CreateEncrypt(fileName);
            if (encrypt != null)
            {
                using (Process proc = new Process())
                {
                    proc.StartInfo = encrypt;
                    proc.Start();
                    proc.WaitForExit();
                }
            }
        }

        public static PGPCrypto Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}
