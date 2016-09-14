using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.WebUI;
using System.IO;
using Utility;
using System.Text;

namespace eIVOGo.Published
{
    public partial class Native2Ascii : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (srcFile.HasFile && srcFile.PostedFile.ContentLength > 0)
            {
                Response.Clear();
                Response.ContentType = "message/rfc822";
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", Server.UrlEncode(srcFile.FileName)));

                Encoding enc = Encoding.GetEncoding(ddEncoding.SelectedValue);

                using (StreamReader sr = new StreamReader(srcFile.PostedFile.InputStream,enc))
                {
                    writeContent(sr,Response.Output);
                    sr.Close();
                }
                Response.End();
            }
            else
            {
                this.AjaxAlert("請輸入有效的檔案!!");
            }
        }

        private void writeContent(StreamReader sr,TextWriter output)
        {
            String line;
            if (cbToCN.Checked)
            {
                while ((line = sr.ReadLine()) != null)
                {
                    writeCode(output, line.ToSimplified());
                    output.WriteLine();
                }
            }
            else
            {
                while ((line = sr.ReadLine()) != null)
                {
                    writeCode(output, line);
                    output.WriteLine();
                }
            }
        }

        private void writeCode(TextWriter output, String line)
        {
            var charArray = line.ToCharArray();
            foreach (var ch in charArray)
            {
                if (ch > 127)
                {
                    output.Write(String.Format("\\u{0:x4}", (uint)ch));
                }
                else
                {
                    output.Write(ch);
                }
            }
        }

        protected void btnConvert_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textContent.Text))
            {
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    if (cbToCN.Checked)
                    {
                        writeCode(sw, textContent.Text.ToSimplified());
                    }
                    else
                    {
                        writeCode(sw, textContent.Text);
                    }
                    sw.Flush();
                    sw.Close();
                }
                codeText.InnerText = sb.ToString();
            }
        }
    }
}