using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using eIVOGo.Helper;
using System.Threading;

namespace eIVOGo.Published
{
    public partial class TestPopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnPrint.PrintControls.Add(btnShow);
            btnWait.AttachWaitingMessage("正在處理中，請稍後...", true);
            btnWait0.AttachWaitingMessage("正在處理中，請稍後...", true);
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            //PupopModal1.Show();
            timeModal.Show();
        }

        protected void btnWait_Click(object sender, EventArgs e)
        {
            Thread.Sleep(10000);
        }

        protected void btnWait0_Click(object sender, EventArgs e)
        {
            Thread.Sleep(5000);
        }
    }
}