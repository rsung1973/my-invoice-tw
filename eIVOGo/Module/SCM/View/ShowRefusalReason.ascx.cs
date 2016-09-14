using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.WebUI;
using Model.SCMDataEntity;

namespace eIVOGo.Module.SCM.View
{
    public partial class ShowRefusalReason : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected DocumentProcessLog _item;

        public void Show(int docID)
        {
            
            _item = dsEntity.CreateDataManager().GetTable<DocumentProcessLog>().Where(d => d.DocID == docID && d.DocumentReasonForRefusal != null).OrderByDescending(d => d.StepDate).FirstOrDefault();
            if (_item != null)
            {
                this.ModalPopupExtender.Show();
                base.DataBind();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ModalPopupExtender.Hide();
        }

        public override void DataBind()
        {
            
        }

    }
}