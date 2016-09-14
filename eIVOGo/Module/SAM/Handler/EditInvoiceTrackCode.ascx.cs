using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Model.DataEntity;
using Uxnet.Web.WebUI;
using eIVOGo.Module.UI;

namespace eIVOGo.Module.SAM.Handler
{
    public partial class EditInvoiceTrackCode : System.Web.UI.UserControl
    {
        public event EventHandler Done;
        protected InvoiceTrackCode _dataItem;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Expression<Func<InvoiceTrackCode, bool>> QueryExpr
        {
            get;
            set;
        }

        public InvoiceTrackCode DataItem
        {
            get
            {
                return _dataItem;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            dsTrack.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<InvoiceTrackCode>>(dsTrack_Select);
            this.PreRender += new EventHandler(EditInvoiceTrackCode_PreRender);
        }

        void EditInvoiceTrackCode_PreRender(object sender, EventArgs e)
        {
            if (QueryExpr != null)
            {
                FormView1.DefaultMode = FormViewMode.Edit;
                FormView1.DataBind();
            }
        }

        void dsTrack_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<InvoiceTrackCode> e)
        {
            if (QueryExpr != null)
            {
                e.QueryExpr = QueryExpr;
            }
            else if (FormView1.DataKey.Value != null)
            {
                e.QueryExpr = o => o.TrackID == (int)FormView1.DataKey.Value;
            }
            else
            {
                e.QueryExpr = o => false;
            }
        }

        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            var mgr = dsTrack.CreateDataManager();

            _dataItem = new InvoiceTrackCode { };

            if (!validate(e.Values))
            {
                e.Cancel = true;
            }
            else
            {
                mgr.EntityList.InsertOnSubmit(_dataItem);
                mgr.SubmitChanges();
            }

        }

        private bool validate(System.Collections.Specialized.IOrderedDictionary values)
        {
            var mgr = dsTrack.CreateDataManager();

            String trackCode = checkTrackCode(values["TrackCode"] as String);
            if (trackCode == null)
            {
                return false;
            }

            _dataItem.TrackCode = trackCode;
            _dataItem.Year = ((TrackCodeYearSelector)FormView1.Row.FindControl("TrackCodeYear")).Year;
            _dataItem.PeriodNo = short.Parse(((DropDownList)FormView1.Row.FindControl("PeriodNo")).SelectedValue);

            if (mgr.EntityList.Count(o => o.TrackCode == _dataItem.TrackCode && o.PeriodNo == _dataItem.PeriodNo && o.Year == _dataItem.Year) > 0)
            {
                this.AjaxAlert("動作失敗!(該期間之輸入字軌已存在)!!");
                return false;
            }

            return true;
        }

        private string checkTrackCode(string code)
        {
            if (!String.IsNullOrEmpty(code) || code.Length == 2)
            {
                String trackCode = code.ToUpper();
                if (Regex.IsMatch(trackCode, "[A-Z]{2}"))
                {
                    return trackCode;
                }
            }

            this.AjaxAlert("字軌請輸入兩個英文字母!!");
            return null;
        }

        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            var mgr = dsTrack.CreateDataManager();

            _dataItem = mgr.EntityList.Where(o => o.TrackID == (int)FormView1.DataKey.Value).FirstOrDefault();
            if (_dataItem == null)
            {
                this.AjaxAlert("編輯的資料已不存在,請重新選取!!");
                e.Cancel = true;
                return;
            }

            if (!validate(e.NewValues))
            {
                e.Cancel = true;
            }
            else
            {
                mgr.SubmitChanges();
            }

        }

        protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                Response.Redirect("~/EIVO/track_number_list.aspx");
            }
        }
    }
}