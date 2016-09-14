using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Configuration.Install;
using Utility;
using System.ServiceProcess;

namespace InvoiceClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /// SSL憑證信任設定
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;

            if (Environment.UserInteractive /*|| Debugger.IsAttached*/)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                ServiceBase[] services = 
                    {
                        new InvoiceClientService() 
                    };
                ServiceBase.Run(services);
            }
            
        }

        internal static void Install(bool undo, string[] args)
        {
            try
            {
                using (AssemblyInstaller inst = new AssemblyInstaller(typeof(Program).Assembly, args))
                {
                    IDictionary state = new Hashtable();
                    inst.UseNewContext = true;
                    try
                    {
                        if (undo)
                        {
                            inst.Uninstall(state);
                            MessageBox.Show("服務已移除!!", "服務設定", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            inst.Install(state);
                            inst.Commit(state);
                            MessageBox.Show("服務安裝成功!!", "服務設定", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch
                    {
                        try
                        {
                            inst.Rollback(state);
                        }
                        catch
                        {
                        }
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                MessageBox.Show("服務安裝失敗:\r\n" + ex.Message, "服務設定", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
