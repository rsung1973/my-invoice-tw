using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using eIVOGo.Module.Base;
using Model.DataEntity;
using Utility;
using Model.Locale;
using Uxnet.Web.WebUI;
using Model.Security.MembershipManagement;
using Business.Helper;

namespace eIVOGo.Module.EIVO
{
    public partial class CreateInvoiceV2 : EditEntityItemBase<EIVOEntityDataContext, InvoiceItem>
    {
        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            selectOrgData();
            this.lalBuyerMessage.Visible = false;
        }

        protected override void OnInit(EventArgs e)
        {
            initializeData();
            Page.MaintainScrollPositionOnPostBack = true;
            this.InvoiceItemDetailForm1.Done += new EventHandler(InvoiceItemDetailForm1_Done);
            this.PreRender += new EventHandler(CreateInvoice_PreRender);
            this.dsBuyerDataList.Done += dsBuyerDataList_Done;
        }

        void dsBuyerDataList_Done(object sender, EventArgs e)
        {            
            var mgr = this.dsEntity.CreateDataManager();
            var Buyer = mgr.GetTable<Organization>().Where(o => o.CompanyID == int.Parse(this.dsBuyerDataList.SelectedValue)).FirstOrDefault();
            this.txtBuyerName.Text = Buyer.CompanyName;
            this.BuyerCompany.Visible = false;
            selectOrgData();
            this.itemList.Visible = true;
            this.BuyerCompany.Visible = true;
        }

        void CreateInvoice_PreRender(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (ContentItem.DataItem != null & ListItem.DataItem != null)
                {
                    this.InvoiceItemDetailForm1.ItemList = (List<InvoiceProductItem>)ListItem.DataItem;
                    List<String> data = (List<String>)ContentItem.DataItem;
                    if (BuyerCompany.Visible == true)
                    {
                        this.BuyerCompany.SelectedValue = data[1];
                    }
                    else
                    {
                        this.txtBuyerName.Text = data[1];
                    }
                    this.rbType.SelectedValue = data[2];
                    this.ddlBondedAreaConfirm.SelectedValue = data[3];
                    this.txtTaxRate.Text = data[4];
                    selectOrgData();
                    itemList.Visible = true;
                    CountTotal();
                }
            }
        }

        void InvoiceItemDetailForm1_Done(object sender, EventArgs e)
        {
            CountTotal();
        }

        private void initializeData()
        {
            this.txtTaxRate.Text = "5";
        }

        protected void selectOrgData()
        {
            Expression<Func<Organization, bool>> queryExpr = i => true;

            if (BuyerCompany.Visible == true)
            {
                if (!String.IsNullOrEmpty(BuyerCompany.SelectedValue))
                {
                    queryExpr = queryExpr.And(i => i.CompanyID.Equals(int.Parse(BuyerCompany.SelectedValue)));
                }
            }
            else if (!String.IsNullOrEmpty(txtBuyerName.Text))
            {
                queryExpr = queryExpr.And(i => i.CompanyName.Equals(txtBuyerName.Text));
            }
            else
            {
                queryExpr = i => false;
            }

            itemList.BuildQuery = table =>
            {
                var org = table.Context.GetTable<Organization>();
                return table.Join(org.Where(queryExpr), b => b.CompanyID, o => o.CompanyID, (b, o) => b);
            };

        }

        protected void BuyerCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.BuyerCompany.SelectedIndex.Equals(0))
            {
                itemList.Visible = false;
            }
            else
            {
                
                selectOrgData();
                
                itemList.Visible = true;

                var mgr = this.dsEntity.CreateDataManager();
                var org = mgr.GetTable<Organization>().Where(o => o.CompanyID == int.Parse(this.BuyerCompany.SelectedValue)).FirstOrDefault();
                this.txtBuyerName.Text = org.CompanyName;

                if (this.BuyerCompany.SelectedItem.Text.Contains("84613756"))
                {
                    this.extra1.Visible = true;
                    this.extra2.Visible = true;
                }
                else
                {
                    this.extra1.Visible = false;
                    this.extra2.Visible = false;
                }
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            var mgr = this.dsEntity.CreateDataManager();

            var Org = mgr.GetTable<Organization>().Where(o => o.CompanyName == this.txtBuyerName.Text).FirstOrDefault();
            if (chkInput())
            {
                List<String> data = new List<string>();
                data.Add("NonCSC");
                data.Add(this.NonCSC.Visible != true ? this.BuyerCompany.SelectedValue : Org.CompanyID.ToString());
                //data.Add(Org.CompanyID.ToString());
                data.Add(this.rbType.SelectedValue);
                data.Add(this.ddlBondedAreaConfirm.SelectedValue);
                data.Add(this.txtTaxRate.Text.Trim());
                if (this.BuyerCompany.Visible == true)
                {
                    if (this.BuyerCompany.SelectedItem.Text.Contains("84613756"))
                        data.Add(this.txtRepDept.Text + "," + this.txtRepName.Text + "," + this.txtRepEmployeeId.Text + "," + this.txtRepNumber.Text);
                }
                else
                {
                    if (this.txtBuyerName.Text.Contains("84613756"))
                    {
                        data.Add(this.txtRepDept.Text + "," + this.txtRepName.Text + "," + this.txtRepEmployeeId.Text + "," + this.txtRepNumber.Text);
                    }
                }
                this.ContentItem.DataItem = data;
                this.ListItem.DataItem = this.InvoiceItemDetailForm1.ItemList;
                Response.Redirect(NextAction.RedirectTo);
            }
        }

        private bool chkInput()
        {
            //if (!this.rbCSC.Checked && !this.rbNonCSC.Checked)
            //{
            //    this.AjaxAlert("請選擇發票開立類別!!");
            //    return false;
            //}
            if (this.rbType.SelectedIndex.Equals(1))
            {
                if (this.ddlBondedAreaConfirm.SelectedIndex.Equals(0))
                {
                    this.AjaxAlert("稅別為零稅率時，需選擇買受人簽署適用零稅率註記!!");
                    return false;
                }
            }
            else if (this.InvoiceItemDetailForm1.ItemList == null || this.InvoiceItemDetailForm1.ItemList.Count.Equals(0))
            {
                this.AjaxAlert("請填入發票明細!!");
                return false;
            }


            if (this.BuyerCompany.SelectedItem.Text.Contains("84613756"))
            {
                if (String.IsNullOrEmpty(this.txtRepDept.Text))
                {
                    this.AjaxAlert("買受人為中龍鋼鐵時，部門欄位不可空白!!");
                    return false;
                }
                else if (String.IsNullOrEmpty(this.txtRepName.Text))
                {
                    this.AjaxAlert("買受人為中龍鋼鐵時，姓名欄位不可空白!!");
                    return false;
                }
                else if (String.IsNullOrEmpty(this.txtRepEmployeeId.Text))
                {
                    this.AjaxAlert("買受人為中龍鋼鐵時，職編欄位不可空白!!");
                    return false;
                }
                else if (String.IsNullOrEmpty(this.txtRepNumber.Text))
                {
                    this.AjaxAlert("買受人為中龍鋼鐵時，合約/訂單/工令/採購案號欄位不可空白!!");
                    return false;
                }
            }

            if (this.NonCSC.Visible == true)
            {
                var mgr = this.dsEntity.CreateDataManager();
                var org = mgr.GetTable<Organization>().Where(o => o.CompanyName == this.txtBuyerName.Text).FirstOrDefault();

                if (org == null)
                {
                    this.AjaxAlert("請輸入買受人");
                    return false;
                }

                if (this.txtBuyerName.Text.Contains("84613756"))
                {
                    if (String.IsNullOrEmpty(this.txtRepDept.Text))
                    {
                        this.AjaxAlert("買受人為中龍鋼鐵時，部門欄位不可空白!!");
                        return false;
                    }
                    else if (String.IsNullOrEmpty(this.txtRepName.Text))
                    {
                        this.AjaxAlert("買受人為中龍鋼鐵時，姓名欄位不可空白!!");
                        return false;
                    }
                    else if (String.IsNullOrEmpty(this.txtRepEmployeeId.Text))
                    {
                        this.AjaxAlert("買受人為中龍鋼鐵時，職編欄位不可空白!!");
                        return false;
                    }
                    else if (String.IsNullOrEmpty(this.txtRepNumber.Text))
                    {
                        this.AjaxAlert("買受人為中龍鋼鐵時，合約/訂單/工令/採購案號欄位不可空白!!");
                        return false;
                    }
                }
            }

            return true;
        }

        protected void rbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.rbType.SelectedIndex.Equals(0))
            {
                this.txtTaxRate.Text = "5";
                this.ddlBondedAreaConfirm.SelectedIndex = 0;
                this.ddlBondedAreaConfirm.Enabled = false;
            }
            else
            {
                this.txtTaxRate.Text = "0";
                if (this.rbType.SelectedIndex.Equals(1))
                {
                    this.ddlBondedAreaConfirm.Enabled = true;
                }
                else
                {
                    this.ddlBondedAreaConfirm.SelectedIndex = 0;
                    this.ddlBondedAreaConfirm.Enabled = false;
                }
            }

            CountTotal();
        }

        void CountTotal()
        {
            Decimal TotalSum = this.InvoiceItemDetailForm1.ItemList.Sum(i => i.CostAmount.Value);
            this.txtTax.Text = (TotalSum * (Decimal.Parse(this.txtTaxRate.Text.Trim()) / 100)).ToString("#,0");
            this.txtSellTotal.Text = TotalSum.ToString("#,0");
            this.txtTotal.Text = (Decimal.Parse(this.txtTax.Text.Trim()) + TotalSum).ToString("#,0");
        }

        protected void rbCSC_CheckedChanged(object sender, EventArgs e)
        {
            //BuyerChosies.Visible = true;
            //this.BuyerCompany.Items.Clear();
            //var mgr = this.dsEntity.CreateDataManager();

            //if (rbCSC.Checked == true)
            //{
            //    this.BuyerCompany.Visible = true;
            //    this.NonCSC.Visible = false;
            //    //this.txtBuyerName.Visible = false;
            //    //this.btnBuyerSelect.Visible = false;

            //    this.BuyerCompany.Items.AddRange(mgr.GetTable<OrganizationCategory>().Where(o => o.CategoryID.Equals((int)Naming.CategoryID.COMP_E_INVOICE_B2B_BUYER)
            //        & o.Organization.OrganizationStatus.CurrentLevel.Value.Equals((int)Naming.MemberStatusDefinition.Checked) & o.Organization.GroupRole == true)
            //        .OrderBy(o => o.Organization.ReceiptNo)
            //        .Select(o => new ListItem(String.Format("{0} {1}", o.Organization.ReceiptNo, o.Organization.CompanyName), o.Organization.CompanyID.ToString()))
            //        .ToArray());
            //}
            //else if (rbNonCSC.Checked == true)
            //{
            //    this.BuyerCompany.Visible = true;
            //    this.NonCSC.Visible = true;
            //    //this.txtBuyerName.Visible = true;
            //    //this.btnBuyerSelect.Visible = true;
            //    this.BuyerCompany.Items.AddRange(mgr.GetTable<OrganizationCategory>().Where(o => o.CategoryID.Equals((int)Naming.CategoryID.COMP_E_INVOICE_B2B_BUYER)
            //        & o.Organization.OrganizationStatus.CurrentLevel.Value.Equals((int)Naming.MemberStatusDefinition.Checked) & o.Organization.GroupRole == false)
            //        .OrderBy(o => o.Organization.ReceiptNo)
            //        .Select(o => new ListItem(String.Format("{0} {1}", o.Organization.ReceiptNo, o.Organization.CompanyName), o.Organization.CompanyID.ToString()))
            //        .ToArray());
            //}
        }

        protected void btnBuyerSelect_Click(object sender, EventArgs e)
        {
            this.dsBuyerDataList.Show();
        }

        protected void txtBuyerName_TextChanged(object sender, EventArgs e)
        {
            var mgr = this.dsEntity.CreateDataManager();
            var org = mgr.GetTable<Organization>().Where(o => o.ReceiptNo == this.txtBuyerName.Text).FirstOrDefault();

            if (org == null)
            {
                this.txtBuyerName.Text = "";
                this.lalBuyerMessage.Visible = true;
                this.itemList.Visible = false;
            }
            else
            {
                this.lalBuyerMessage.Visible = false;
                this.txtBuyerName.Text = org.CompanyName;
                this.BuyerCompany.Visible = false;
                selectOrgData();
                this.itemList.Visible = true;
                this.BuyerCompany.Visible = true;
            }
        }
    }
}