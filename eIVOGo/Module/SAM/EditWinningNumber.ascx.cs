using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Model.DataEntity;
using Model.Security.MembershipManagement;
using Model.InvoiceManagement;
using Uxnet.Web.Module.Common;
using Uxnet.Web.WebUI;
using Utility;
using System.Text.RegularExpressions;

namespace eIVOGo.Module.SAM
{
    public partial class EditWinningNumber : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.PreviousPage != null && Page.PreviousPage.Items["id"] != null)
            {
                string[] data = Page.PreviousPage.Items["id"].ToString().Split('-');
                ViewState["year"] = data[0];
                ViewState["period"] = data[1];
            }

            if (!this.IsPostBack)
            {
                initControl();
                initializeData();
            }
        }

        #region "Initial"
        private void initControl()
        {
            this.ddlYear.Items.Clear();
            this.ddlRange.Items.Clear();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            for (int y = -5; y < 6; y++)
            {
                this.ddlYear.Items.Add(new ListItem(((year - 1911) + y).ToString() + "年", (year + y).ToString()));
            }

            for (int m = 1; m < 7; m += 1)
            {
                this.ddlRange.Items.Add(new ListItem((2 * m - 1).ToString("00") + "-" + (2 * m).ToString("00") + " 月", m.ToString()));
            }
            this.ddlYear.SelectedValue = year.ToString();
            this.ddlRange.SelectedValue = month % 2 == 0 ? (month / 2).ToString() : month.ToString();
        }

        private void initializeData()
        {
            using (InvoiceManager im = new InvoiceManager())
            {
                if (ViewState["year"] != null & ViewState["period"] != null)
                {
                    int year = int.Parse(ViewState["year"].ToString());
                    int period = int.Parse(ViewState["period"].ToString());
                    this.ddlYear.SelectedValue = year.ToString();
                    this.ddlRange.SelectedValue = period.ToString();

                    this.txtSpecialPrize.Text = string.Join(",", im.GetTable<UniformInvoiceWinningNumber>().Where(d => d.Year == year & d.Period == period & d.PrizeType == "特別獎").Select(d => d.WinningNO).ToArray());
                    this.txtGrandPrize.Text = string.Join(",", im.GetTable<UniformInvoiceWinningNumber>().Where(d => d.Year == year & d.Period == period & d.PrizeType == "特獎").Select(d => d.WinningNO).ToArray());
                    this.DynamicAddTextbox1.allTextboxValue = string.Join(",", im.GetTable<UniformInvoiceWinningNumber>().Where(d => d.Year == year & d.Period == period & d.PrizeType == "頭獎").Select(d => d.WinningNO).ToArray());
                    this.DynamicAddTextbox2.allTextboxValue = string.Join(",", im.GetTable<UniformInvoiceWinningNumber>().Where(d => d.Year == year & d.Period == period & d.PrizeType == "增開六獎").Select(d => d.WinningNO).ToArray());
                }
            }
        }
        #endregion

        #region "Button Event"
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            if (check())
            {
                if (InsertORUpdate())
                {
                    WebMessageBox.AjaxAlert(this, "儲存完成!!");
                    Response.Redirect("WinningNumbersManager.aspx");
                }
                else
                {
                    WebMessageBox.AjaxAlert(this, "系統錯誤!!");
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            initializeData();
            this.txtSpecialPrize.Text = "";
            this.txtGrandPrize.Text = "";

            foreach (Control ct in this.DynamicAddTextbox1.Controls)
            {
                if (ct is PlaceHolder)
                {
                    PlaceHolder ph = (PlaceHolder)ct;
                    foreach (Control ct1 in ph.Controls)
                    {
                        if (ct1 is TextBox)
                        {
                            TextBox tx = (TextBox)ct1;
                            tx.Text = "";
                        }
                    }
                }
            }

            foreach (Control ct in this.DynamicAddTextbox2.Controls)
            {
                if (ct is PlaceHolder)
                {
                    PlaceHolder ph = (PlaceHolder)ct;
                    foreach (Control ct1 in ph.Controls)
                    {
                        if (ct1 is TextBox)
                        {
                            TextBox tx = (TextBox)ct1;
                            tx.Text = "";
                        }
                    }
                }
            }
        }
        #endregion

        #region "Insert or Update"
        private Boolean InsertORUpdate()
        {
            Boolean result = true;
            try
            {
                using (InvoiceManager im = new InvoiceManager())
                {
                    if (ViewState["year"] != null & ViewState["period"] != null)
                    {
                        int year = int.Parse(ViewState["year"].ToString());
                        int period = int.Parse(ViewState["period"].ToString());
                        var oldNum = im.GetTable<UniformInvoiceWinningNumber>().Where(d => d.Year == year & d.Period == period);
                        im.GetTable<UniformInvoiceWinningNumber>().DeleteAllOnSubmit(oldNum);
                    }

                    string[] allNums = { this.txtSpecialPrize.Text, this.txtGrandPrize.Text, this.DynamicAddTextbox1.allTextboxValue, this.DynamicAddTextbox2.allTextboxValue };
                    int rank = 1;
                    int prizeType = 0;
                    for (int i = 0; i < allNums.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(allNums[i]))
                        {
                            string[] winNums = allNums[i].Split(',');
                            do
                            {
                                prizeType += 1;
                                for (int j = 0; j < winNums.Length; j++)
                                {
                                    UniformInvoiceWinningNumber uiwn = new UniformInvoiceWinningNumber();
                                    uiwn.Year = int.Parse(this.ddlYear.SelectedValue);
                                    uiwn.Period = int.Parse(this.ddlRange.SelectedValue);
                                    uiwn.Rank = rank;
                                    uiwn.WinningNO = i == 2 ? winNums[j].Substring(prizeType - 1) : winNums[j];
                                    uiwn.PrizeType = i == 0 ? "特別獎" : i == 1 ? "特獎" : i == 2 ? (prizeType == 1 ? "頭獎" : prizeType == 2 ? "二獎" : prizeType == 3 ? "三獎" : prizeType == 4 ? "四獎" : prizeType == 5 ? "五獎" : prizeType == 6 ? "六獎" : "") : i == 3 ? "增開六獎" : "";
                                    uiwn.Bonus = i == 0 ? 10000000 : i == 1 ? 2000000 : i == 2 ? (prizeType == 1 ? 200000 : prizeType == 2 ? 40000 : prizeType == 3 ? 10000 : prizeType == 4 ? 4000 : prizeType == 5 ? 1000 : prizeType == 6 ? 200 : 0) : i == 3 ? 200 : 0;
                                    im.GetTable<UniformInvoiceWinningNumber>().InsertOnSubmit(uiwn);
                                }
                                rank += 1;
                            } while (i == 2 && prizeType < 6);
                            prizeType = 0;
                        }
                    }

                    im.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result = false;
            }
            return result;
        }
        #endregion

        #region "Functions"
        private Boolean check()
        {
            Boolean result = true;
            
            Regex reg = new Regex("^\\d{8}$");
            if (!string.IsNullOrEmpty(this.txtSpecialPrize.Text) && !reg.IsMatch(this.txtSpecialPrize.Text))
            {
                WebMessageBox.AjaxAlert(this, "特別獎欄位請填八位數字!!");
                result = false;
            }
            else if (string.IsNullOrEmpty(this.txtGrandPrize.Text))
            {
                WebMessageBox.AjaxAlert(this, "請填入特獎號碼!!");
                result = false;
            }
            else if (!reg.IsMatch(this.txtGrandPrize.Text))
            {
                WebMessageBox.AjaxAlert(this, "特獎欄位請填八位數字!!");
                result = false;
            }

            if (string.IsNullOrEmpty(this.DynamicAddTextbox1.allTextboxValue))
            {
                WebMessageBox.AjaxAlert(this, "頭獎號碼最少一筆!!");
                result = false;
            }
            else
            {
                string[] value = this.DynamicAddTextbox1.allTextboxValue.Split(',');
                foreach (string s in value)
                {
                    if (!reg.IsMatch(s))
                    {
                        WebMessageBox.AjaxAlert(this, "頭獎欄位請填八位數字!!");
                        result = false;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.DynamicAddTextbox2.allTextboxValue))
            {
                Regex reg1 = new Regex("^\\d{3}$");
                string[] value = this.DynamicAddTextbox2.allTextboxValue.Split(',');
                foreach (string s in value)
                {
                    if (!reg1.IsMatch(s))
                    {
                        WebMessageBox.AjaxAlert(this, "增開六獎欄位請填三位數字!!");
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
        #endregion
    }
}