using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Locale;
using Model.Security.MembershipManagement;
using Model.InvoiceManagement;
using Business.Helper;
using Model.DataEntity;
using Model.SCMDataEntity;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Utility;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SCM
{
    public partial class PurchaseOrderMangement : System.Web.UI.UserControl, IPostBackEventHandler
    {
        UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (!Page.IsPostBack)
            {
                initializeData();
            }
        }

        private void initializeData()
        {
            var mgr = this.dsPurchase.CreateDataManager();
            this.ddlWarehouse.Items.AddRange(mgr.GetTable<WAREHOUSE>().Select(wh => new ListItem(wh.WAREHOUSE_NAME, wh.WAREHOUSE_SN.ToString())).ToArray());
            this.ddlSupplier.Items.AddRange(mgr.GetTable<SUPPLIER>().Select(s => new ListItem(s.SUPPLIER_NAME, s.SUPPLIER_SN.ToString())).ToArray());
        }

        protected override void OnInit(EventArgs e)
        {
            this.PreRender+=new EventHandler(PurchaseOrderMangement_PreRender);
            this.dsPurchase.Select+=new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PURCHASE_ORDER>>(dsPurchase_Select);
            this.RefuseDocument.Done += new EventHandler(RefuseDocument_Done);
        }

        void RefuseDocument_Done(object sender, EventArgs e)
        {
            this.gvEntity.DataBind();
            this.AjaxAlert("該筆資料已刪除!!");            
        }

        void dsPurchase_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PURCHASE_ORDER> e)
        {
            if (!String.IsNullOrEmpty(this.btnQuery.CommandArgument))
            {
                var mgr = this.dsPurchase.CreateDataManager();

                IQueryable<PURCHASE_ORDER> po = mgr.EntityList;

                if (this.ddlWarehouse.SelectedIndex != 0)
                {
                    po = po.Where(p => p.WAREHOUSE.WAREHOUSE_SN == long.Parse(this.ddlWarehouse.SelectedValue));
                }

                if (this.ddlSupplier.SelectedIndex != 0)
                {
                    po = po.Where(p => p.SUPPLIER.SUPPLIER_SN == long.Parse(this.ddlSupplier.SelectedValue));
                }

                if (!string.IsNullOrEmpty(this.txtPurchaseNO.Text))
                {
                    po = po.Where(p => p.PURCHASE_ORDER_NUMBER.Equals(this.txtPurchaseNO.Text));
                }

                if (!string.IsNullOrEmpty(this.txtBarCode.Text))
                {
                    po = po.Where(p => p.PURCHASE_ORDER_DETAILS.Where(pod => pod.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_BARCODE.Equals(this.txtBarCode.Text)).Count() > 0);
                }

                if (!string.IsNullOrEmpty(this.txtProdName.Text))
                {
                    po = po.Where(p => p.PURCHASE_ORDER_DETAILS.Where(pod => pod.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_NAME.Contains(this.txtProdName.Text)).Count() > 0);
                }

                if (!string.IsNullOrEmpty(this.DateFrom.TextBox.Text) && !string.IsNullOrEmpty(this.DateTo.TextBox.Text))
                {
                    po = po.Where(p => p.PO_DATETIME.Value >= this.DateFrom.DateTimeValue & p.PO_DATETIME.Value <= this.DateTo.DateTimeValue.AddDays(1));
                }

                if (this.rdbCloseStatus.SelectedIndex != 0)
                {
                    po = po.Where(p => p.PO_STATUS == (this.rdbCloseStatus.SelectedIndex == 1 ? 1 : 0));
                }

                if (this.rdbVerifyStatus.SelectedIndex != 0)
                {
                    switch (this.rdbVerifyStatus.SelectedIndex)
                    {
                        case 1:
                            po = po.Where(p => p.CDS_Document.CurrentStep == (int)Naming.DocumentStepDefinition.待審核);
                            break;
                        case 2:
                            po = po.Where(p => p.CDS_Document.CurrentStep == (int)Naming.DocumentStepDefinition.已開立);
                            break;
                        case 3:
                            po = po.Where(p => p.CDS_Document.CurrentStep == (int)Naming.DocumentStepDefinition.已退回);
                            break;
                    }
                }

                if (this.rdbDeleteStatus.SelectedIndex != 0)
                {
                    switch (this.rdbDeleteStatus.SelectedIndex)
                    {
                        case 1:
                            po = po.Where(p => p.CDS_Document.CurrentStep == (int)Naming.DocumentStepDefinition.已刪除);
                            break;
                        case 2:
                            po = po.Where(p => p.CDS_Document.CurrentStep != (int)Naming.DocumentStepDefinition.已刪除);
                            break;
                    }
                }

                e.Query = po;

                this.lblRowCount.Text = po.Count().ToString();
                Decimal total = po.Sum(t => t.PO_TOTAL_AMOUNT) ?? 0;
                this.lblTotalSum.Text = total.ToString("0,0.00");
            }
            else
            {
                e.QueryExpr = p => false;
            }
        }

        #region "Button Event"
        protected void btnNewPurchase_Click(object sender, EventArgs e)
        {
            Response.Redirect("PurchaseOrder.aspx");
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.divResult.Visible = true;
            this.btnQuery.CommandArgument = "Query";
            this.gvEntity.DataBind();
            if (this.gvEntity.Rows.Count > 0)
            {
                this.countTable.Visible = true;
                this.lblError.Visible = false;
            }
            else
            {
                this.lblError.Text = "查無資料!!";
                this.lblError.Visible = true;
                this.countTable.Visible = false;
            }
        }
        #endregion

        #region "Gridview Event"
        protected void PageIndexChanged(object source, PageChangedEventArgs e)
        {
            gvEntity.PageIndex = e.NewPageIndex;
            gvEntity.DataBind();
        }

        void PurchaseOrderMangement_PreRender(object sender, EventArgs e)
        {
            if (gvEntity.BottomPagerRow != null)
            {
                PagingControl paging = (PagingControl)gvEntity.BottomPagerRow.Cells[0].FindControl("pagingList");
                paging.RecordCount = this.dsPurchase.CurrentView.LastSelectArguments.TotalRowCount;
                paging.CurrentPageIndex = gvEntity.PageIndex;
            }
        }

        public string TranslateCurrentStep(object o)
        {
            string name = "";
            int step = (int)o;
            if (step == (int)Naming.DocumentStepDefinition.預覽)
            {
                name = "待送審";
            }
            else if (step == (int)Naming.DocumentStepDefinition.待審核)
            {
                name = "待主管審核";
            }
            else if (step == (int)Naming.DocumentStepDefinition.已開立)
            {
                name = "審核完成已開立";
            }
            else if (step == (int)Naming.DocumentStepDefinition.已退回)
            {
                name = "<font color='red'>審核完成已退回</font>";
            }
            else
            {
                name = "N/A";
            }
            return name;
        }
        #endregion

        #region IPostBackEventHandler Members
        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.StartsWith("U:"))
            {
                doEdit(int.Parse(eventArgument.Substring(2)));
            }
            else if (eventArgument.StartsWith("D:"))
            {
                this.RefuseDocument.DocID = int.Parse(eventArgument.Substring(2));
                this.RefuseDocument.Show();
            }
            else if (eventArgument.StartsWith("C:"))
            {
                doSpecialClose(int.Parse(eventArgument.Substring(2)));
            }
            else if (eventArgument.StartsWith("P:"))
            {
                this.PupopPurchaseOrderDetail1.setDetail = eventArgument.Substring(2);
                this.PupopPurchaseOrderDetail1.Popup.Show();
            }
            else if (eventArgument.StartsWith("S:"))
            {
                this.PupopDeleteReason1.DocID = int.Parse(eventArgument.Substring(2));
                this.PupopDeleteReason1.Show();
            }
        }

        private void doEdit(int no)
        {
            Page.Items["PONO"] = no;
            Server.Transfer("PurchaseOrder.aspx");
        }

        private void doSpecialClose(int no)
        {
            var mgr = this.dsPurchase.CreateDataManager();
            mgr.EntityList.Where(p => p.PURCHASE_ORDER_SN == no).FirstOrDefault().PO_STATUS = 1;
            mgr.EntityList.Where(p => p.PURCHASE_ORDER_SN == no).FirstOrDefault().PO_CLOSED_MODE = 1;
            mgr.SubmitChanges();
            gvEntity.DataBind();
        }
        #endregion
    }    
}
