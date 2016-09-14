using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eIVOGo.Published
{
    public partial class TestPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnPrint.PrintControls.Add(new Calendar());
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.Test.Text = getStringByByte(this.Test.Text, int.Parse(this.length.Text));
        }

        protected String getStringByByte(string data, int ByteLength)
        {
            string str = data;
            string hopestring = "";  //希望取得的字串
            string spp = "";
            int byteLength;
            for (int i = 0; i <= str.Length; i++)
            {
                spp = str.Substring(0, i);
                byteLength = System.Text.Encoding.Default.GetBytes(spp).Length;
                if (byteLength <= ByteLength)
                { hopestring = spp; }
                else
                {
                    hopestring = str.Substring(0, i - 1);
                    break;
                }
            }
            return hopestring;
        }
    }
}