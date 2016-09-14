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
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Utility;
using eIVOGo.Module.Base;
using eIVOGo.Module.EIVO;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.Inquiry
{
    public partial class InquireInvoiceAndAllowanceIntoPrint : InquireInvoiceAndAllowance
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //SellerID.Filter = o => o.OrganizationStatus.SetToPrintInvoice == true;
        }

        protected override void initializeData()
        {
            if (btnSearch.CommandArgument == "Query")
            {
                InvoiceAllowanceCheckList allowanceListView;
                InvoiceItemCheckList invoiceListView;
                switch (rdbSearchItem.SelectedIndex)
                {
                    case 0:
                        if (!String.IsNullOrEmpty(SellerID.SelectedValue))
                        {
                            invoiceListView = (InvoiceItemCheckList)this.LoadControl("~/Module/EIVO/InvoiceItemPrintList.ascx");
                            invoiceListView.InitializeAsUserControl(this.Page);
                            if (!String.IsNullOrEmpty(this.txtPageSize.Text)) invoiceListView.setPageSize = int.Parse(this.txtPageSize.Text.Trim());
                            invoiceListView.EmptyData += new EventHandler(invoiceListView_EmptyData);
                            if (rbInvoiceType.SelectedIndex == 0)//B2B
                            {
                                invoiceListView.QueryExpr = buildInvoiceItemQuery(i => i.InvoiceBuyer.ReceiptNo != "0000000000" && i.InvoiceCancellation == null);
                            }
                            else//B2C
                            {
                                var mgr = dsEntity.CreateDataManager().EntityList.Where(o => o.CompanyID == int.Parse(SellerID.SelectedValue));

                                invoiceListView.QueryExpr = buildInvoiceItemQuery(i => i.InvoiceBuyer.ReceiptNo == "0000000000" && i.InvoiceCancellation == null && i.DonateMark.Equals("0") &&
                                  (i.Organization.OrganizationStatus.PrintAll == true ||
                                  (i.InvoiceCarrier.CarrierType.Equals(EIVOPlatformFactory.DefaultUserCarrierType) &&//3J0001
                                  i.PrintMark.Equals("N") && i.InvoiceWinningNumber != null)) &&
                                    !i.CDS_Document.DocumentPrintLogs.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice));

                                //if (mgr.FirstOrDefault().OrganizationStatus.PrintAll.HasValue)
                                //{
                                //    if (mgr.FirstOrDefault().OrganizationStatus.PrintAll.Value)
                                //    {
                                //        invoiceListView.QueryExpr = buildInvoiceItemQuery(i => i.InvoiceBuyer.ReceiptNo == "0000000000" && i.InvoiceCancellation == null && i.DonateMark.Equals("0") && !i.CDS_Document.DocumentPrintLogs.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice));
                                //    }
                                //    else
                                //    {
                                //        invoiceListView.QueryExpr = buildInvoiceItemQuery(i => i.InvoiceBuyer.ReceiptNo == "0000000000" && i.InvoiceCancellation == null && i.DonateMark.Equals("0") && i.InvoiceWinningNumber != null && !i.CDS_Document.DocumentPrintLogs.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice));
                                //    }
                                //}
                                //else
                                //{
                                //    invoiceListView.QueryExpr = buildInvoiceItemQuery(i => i.InvoiceBuyer.ReceiptNo == "0000000000" && i.InvoiceCancellation == null && i.DonateMark.Equals("0") && i.InvoiceWinningNumber != null && !i.CDS_Document.DocumentPrintLogs.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice));
                                //}

                                //switch (rdbPriceType.SelectedIndex)
                                //{
                                //    case 0:
                                //        invoiceListView.QueryExpr = buildInvoiceItemQuery(i => i.InvoiceBuyer.ReceiptNo == "0000000000" && i.InvoiceCancellation == null && !i.CDS_Document.DocumentPrintLogs.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice));
                                //        break;
                                //    case 1:
                                //        invoiceListView.QueryExpr = buildInvoiceItemQuery(i => i.InvoiceBuyer.ReceiptNo == "0000000000" && i.InvoiceCancellation == null && i.InvoiceWinningNumber != null && !i.CDS_Document.DocumentPrintLogs.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice));
                                //        break;
                                //    case 2:
                                //        invoiceListView.QueryExpr = buildInvoiceItemQuery(i => i.InvoiceBuyer.ReceiptNo == "0000000000" && i.InvoiceCancellation == null && i.InvoicePaperRequest != null && !i.CDS_Document.DocumentPrintLogs.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice));
                                //        break;
                                //}
                            }
                            if (!String.IsNullOrEmpty(this.ddlAttach.SelectedValue))
                            {
                                if (this.ddlAttach.SelectedValue.Equals("0"))
                                {
                                    invoiceListView.QueryExpr = invoiceListView.QueryExpr.And(i => i.CDS_Document.Attachment.Count() > 0);
                                }
                                else
                                {
                                    invoiceListView.QueryExpr = invoiceListView.QueryExpr.And(i => i.CDS_Document.Attachment.Count() <= 0);
                                }
                            }

                            plResult.Controls.Add(invoiceListView);
                        }
                        else
                        {
                            this.AjaxAlert("請選擇公司名稱!!");
                            this.divResult.Visible = false;
                        }
                        break;
                    case 1:
                        if (!String.IsNullOrEmpty(SellerID.SelectedValue))
                        {
                            allowanceListView = (InvoiceAllowanceCheckList)this.LoadControl("~/Module/EIVO/InvoiceAllowancePrintList.ascx");
                            allowanceListView.InitializeAsUserControl(this.Page);
                            allowanceListView.EmptyData += new EventHandler(invoiceListView_EmptyData);
                            allowanceListView.QueryExpr = buildInvoiceAllowanceQuery(i => i.InvoiceAllowanceCancellation == null);
                            plResult.Controls.Add(allowanceListView);
                        }
                        else
                        {
                            this.AjaxAlert("請選擇公司名稱!!");
                            this.divResult.Visible = false;
                        }
                        break;
                    //case 2:
                    //    invoiceListView = (InvoiceItemCheckList)this.LoadControl("InvoiceItemQueryList.ascx");
                    //    invoiceListView.InitializeAsUserControl(this.Page);
                    //    invoiceListView.QueryExpr = buildInvoiceItemQuery(i => i.InvoiceCancellation != null);
                    //    plResult.Controls.Add(invoiceListView);
                    //    break;
                    //case 3:
                    //    allowanceListView = (InvoiceAllowanceCheckList)this.LoadControl("InvoiceAllowanceQueryList.ascx");
                    //    allowanceListView.InitializeAsUserControl(this.Page);
                    //    allowanceListView.QueryExpr = buildInvoiceAllowanceQuery(i => i.InvoiceAllowanceCancellation != null);
                    //    plResult.Controls.Add(allowanceListView);
                    //    break;
                    //case 4:
                    //    invoiceListView = (InvoiceItemCheckList)this.LoadControl("InvoiceItemQueryList.ascx");
                    //    invoiceListView.InitializeAsUserControl(this.Page);
                    //    invoiceListView.QueryExpr = buildInvoiceItemQuery(i => i.InvoiceCancellation == null && i.InvoiceWinningNumber != null);
                    //    plResult.Controls.Add(invoiceListView);
                    //    break;
                    case 4:
                        if (!String.IsNullOrEmpty(SellerID.SelectedValue))
                        {
                            invoiceListView = (InvoiceItemCheckList)this.LoadControl("~/Module/EIVO/InvoiceItemPrintList.ascx");
                            invoiceListView.InitializeAsUserControl(this.Page);
                            // invoiceListView.IDType = this.lblName.Text;
                           // if (this.DateFrom.HasValue & this.DateTo.HasValue) invoiceListView.hasDayRange = true;
                            invoiceListView.EmptyData += new EventHandler(invoiceListView_EmptyData);
                            invoiceListView.QueryExpr = buildInvoiceItemQuery(i => i.InvoiceCancellation == null && i.InvoiceWinningNumber != null && i.DonateMark == "0"
                                && !i.CDS_Document.DocumentPrintLogs.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice));
                            if (_userProfile.CurrentUserRole.RoleID != ((int)Naming.RoleID.ROLE_SYS))
                                invoiceListView.QueryExpr = invoiceListView.QueryExpr.And(i => i.Organization.CompanyID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID);
                           // if (!String.IsNullOrEmpty(this.txtGoogleID.Text)) invoiceListView.QueryExpr = invoiceListView.QueryExpr.And(i => i.InvoiceBuyer.CustomerID.Equals(this.txtGoogleID.Text.Trim()));
                            if (!String.IsNullOrEmpty(this.ddlAttach.SelectedValue))
                            {
                                if (this.ddlAttach.SelectedValue.Equals("0"))
                                {
                                    invoiceListView.QueryExpr = invoiceListView.QueryExpr.And(i => i.CDS_Document.Attachment.Count() > 0);
                                }
                                else
                                {
                                    invoiceListView.QueryExpr = invoiceListView.QueryExpr.And(i => i.CDS_Document.Attachment.Count() <= 0);
                                }
                            }
                            plResult.Controls.Add(invoiceListView);
                        }
                        else
                        {
                            this.AjaxAlert("請選擇公司名稱!!");
                            this.divResult.Visible = false;
                        }
                        break;
                    default:
                        ResultTitle.Visible = false;
                        this.lblError.Visible = true;
                        break;
                }
            }
        }


        //protected override Expression<Func<InvoiceItem, bool>> buildInvoiceItemQuery(Expression<Func<InvoiceItem, bool>> queryExpr)
        //{
        //    queryExpr = queryExpr.And(i => i.InvoicePrintQueue == null);
        //    return base.buildInvoiceItemQuery(queryExpr);
        //}

        void invoiceListView_EmptyData(object sender, EventArgs e)
        {
            ResultTitle.Visible = false;
        }

        protected void rdbSearchItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            trIType.Visible = rdbSearchItem.SelectedIndex == 0;
            if (!trIType.Visible)
            {
                //trPType.Visible = trIType.Visible;
                rbInvoiceType.SelectedIndex = 0;
                //rdbPriceType.SelectedIndex = 0;
            }
            if (rdbSearchItem.SelectedIndex == 0 || rdbSearchItem.SelectedIndex == 4)
                this.trIAttach.Visible = true;
            else
                this.trIAttach.Visible = false;
            resetContent();
        }

        protected void rbInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //trPType.Visible = rbInvoiceType.SelectedIndex == 1;
            //if (!trPType.Visible)
            //    rdbPriceType.SelectedIndex = 0;
            resetContent();
        }

        protected void resetContent()
        {
            btnSearch.CommandArgument = "";
            plResult.Controls.Clear();
            divResult.Visible = false;
        }
    }    
}