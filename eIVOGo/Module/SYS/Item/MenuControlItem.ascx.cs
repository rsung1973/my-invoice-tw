using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;
using System.IO;
using Uxnet.Web.Module.SiteAction;

namespace eIVOGo.Module.SYS.Item
{
    public partial class MenuControlItem : System.Web.UI.UserControl
    {
        public event EventHandler Done;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.dsEntity.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<MenuControl>>(dsEntity_Select);
        }

        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<MenuControl> e)
        {
            if (QueryExpr != null)
            {
                e.QueryExpr = QueryExpr;
            }
            else
            {
                e.QueryExpr = r => false;
            }
        }

        public Expression<Func<MenuControl, bool>> QueryExpr
        { get; set; }

        public void Show()
        {
            dvEntity.ChangeMode(DetailsViewMode.Insert);
            this.ModalPopupExtender.Show();
        }

        public void BindData()
        {
            dvEntity.ChangeMode(DetailsViewMode.Edit);
            dvEntity.DataBind();
            this.ModalPopupExtender.Show();

        }

        protected void dvEntity_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                this.ModalPopupExtender.Hide();
            }
        }

        protected void dvEntity_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            try
            {
                FileUpload file = dvEntity.Rows[1].FindControl("MenuFile") as FileUpload;
                if (file != null && file.HasFile)
                {
                    String fileName = String.IsNullOrEmpty(file.FileName) ? String.Format("{0:yyyyMMddHHmmssfff}.xml", DateTime.Now) : Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(SiteMenuBar.MenuManager.StoredPath, fileName));

                    var mgr = dsEntity.CreateDataManager();
                    MenuControl item = new MenuControl
                    {
                        SiteMenu = fileName
                    };
                    mgr.EntityList.InsertOnSubmit(item);
                    mgr.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert(String.Format("新增資料失敗,原因:{0}", ex.Message));
            }
        }

        protected void dvEntity_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        protected void dvEntity_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        protected void dvEntity_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {

                FileUpload file = dvEntity.Rows[1].FindControl("MenuFile") as FileUpload;
                if (file != null && file.HasFile)
                {
                    String fileName = String.IsNullOrEmpty(file.FileName) ? String.Format("{0:yyyyMMddHHmmssfff}.xml", DateTime.Now) : Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(SiteMenuBar.MenuManager.StoredPath, fileName));
                    var mgr = dsEntity.CreateDataManager();
                    var item = mgr.EntityList.Where(r => r.MenuID == (int)e.Keys[0]).First();
                    item.SiteMenu = fileName;
                    mgr.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert(String.Format("修改資料失敗,原因:{0}", ex.Message));
            }
            
        }
    }
}