using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Utility;
using Uxnet.Web.WebUI;
namespace eIVOGo.Module.SYS
{
    public partial class UploadQRCodeKey : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.QRCodeKey.Text))
                {
                    using (FileStream fs = new FileStream(Path.Combine(Logger.LogPath, "ORCodeKey.txt"), FileMode.Create, FileAccess.Write))
                    {
                        byte[] buf = System.Text.Encoding.Default.GetBytes(this.QRCodeKey.Text);
                        fs.Write(buf, 0, buf.Length);
                        fs.Flush();
                        fs.Close();
                    }
                    this.AjaxAlert("QR Code金鑰已更新!!");
                }
                else
                {
                    this.AjaxAlert("請填入QR Code金鑰!!");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                btnConfirm.AjaxAlert("系統發生錯誤,錯誤原因:" + ex.Message);
            }
        }


    }
}