using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.IO;

using Utility;

namespace eIVOGo.Published
{
    public partial class TestTripleDES : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (fData.HasFile && !String.IsNullOrEmpty(AuthCode.Text))
            {
                Response.Clear();
                Response.ContentType = "message/rfc822";
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0:yyyyMMddHHmmssfff}.DAT", DateTime.Now));

                String secCode = AuthCode.Text.PadRight(16, ' ');

                byte[] key = System.Text.ASCIIEncoding.ASCII.GetBytes(secCode);
                byte[] iv = System.Text.ASCIIEncoding.ASCII.GetBytes(secCode.Substring(5, 8));

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Mode = CipherMode.ECB;

                using (CryptoStream encStream = new CryptoStream(Response.OutputStream, tdes.CreateEncryptor(key, null), CryptoStreamMode.Write))
                {
                    fData.PostedFile.InputStream.CopyTo(encStream);
                    encStream.Flush();
                    encStream.Close();
                }

                Response.Flush();
                Response.End();
            }
        }

        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (fData.HasFile && !String.IsNullOrEmpty(AuthCode.Text))
            {
                Response.Clear();
                Response.ContentType = "message/rfc822";
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0:yyyyMMddHHmmssfff}.DAT", DateTime.Now));

                String secCode = AuthCode.Text.PadRight(16, ' ');

                byte[] key = System.Text.ASCIIEncoding.ASCII.GetBytes(secCode);
                byte[] iv = System.Text.ASCIIEncoding.ASCII.GetBytes(secCode.Substring(5, 8));

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Mode = CipherMode.ECB;

                using (CryptoStream decStream = new CryptoStream(Response.OutputStream, tdes.CreateDecryptor(key, null), CryptoStreamMode.Write))
                {
                    fData.PostedFile.InputStream.CopyTo(decStream);
                    decStream.Flush();
                    decStream.Close();
                }

                Response.Flush();
                Response.End();
            }

        }
    }
}