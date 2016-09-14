using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using eIVOGo.Module.Base;
using Model.DataEntity;
using System.Web.UI.HtmlControls;
using Model.Schema.EIVO;
using Utility;
using System.IO;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;

namespace eIVOGo.Module.EIVO
{
    public partial class DownloadMIG : EditEntityItemBase<EIVOEntityDataContext, InvoiceItem>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                ShowInvDetail();
        }

        protected Model.Schema.MIG3_1.C0401.Invoice item;
        protected Model.Schema.TurnKey.C0701.VoidInvoice voiditem;
        protected DateTime _invoiceDate;
        public InvoiceItem _invoiceItem;
        public static string RegularExpressions = "^-?\\d{1,12}(.[0-9]{0,4})?$";
        public String InvDetail = "";
        public List<Model.Schema.MIG3_1.C0401.DetailsProductItem> items;
        List<string> productItem;
        String Brief = "";
        String Piece = "";
        String PieceUnit = "";
        String UnitCost = "";
        String CostAmount = "";
        String Remark = "";
        String RelateNumber = "";

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Server.Transfer("WebDownloadMIG.aspx");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Exception ex;
                if ((ex = CheckInvFormat()) != null)
                {
                    Response.Write("<Script language='Javascript'>");
                    Response.Write("alert('" + ex.Message + "')");
                    Response.Write("</" + "Script>");
                }
                else
                {
                    CreateInvoiceItem();

                    if (item != null)
                    {
                        string fileName = "C0401_" + txtInvNo.Text + ".xml";
                        string filePath = Logger.LogPath + "\\MIG_FILE\\";

                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);

                        filePath = filePath + fileName;

                        item.ConvertToXml().Save(filePath);

                        Response.WriteFileAsDownload(filePath);
                    }

                    if (voiditem != null)
                    {
                        string fileName = "C0701_" + txtVoidInvoiceNumber.Text + ".xml";
                        string filePath = Logger.LogPath + "\\MIG_FILE\\";

                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);

                        filePath = filePath + fileName;

                        voiditem.ConvertToXml().Save(filePath);

                        Response.WriteFileAsDownload(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private Exception CheckInvFormat()
        {
            //C0401
            if (divC0401.Visible == true)
            {
                if (!DateTime.TryParseExact(String.Format("{0}", txtInvTime.Text), "HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out _invoiceDate))
                {
                    return new Exception("時間格式為：HH:mm:ss");
                }

                if (!Regex.IsMatch(txtSaleAmount.Text, RegularExpressions))
                {
                    return new Exception("格式錯誤：應稅銷售額合計(新台幣)");
                }

                if (!Regex.IsMatch(txtFreeTaxSaleAmount.Text, RegularExpressions))
                {
                    return new Exception("格式錯誤：免稅銷售額合計(新台幣)");
                }

                if (!Regex.IsMatch(txtZeroTaxSaleAmount.Text, RegularExpressions))
                {
                    return new Exception("格式錯誤：零稅率銷售額合計(新台幣)");
                }

                if (!Regex.IsMatch(txtTotalAmount.Text, RegularExpressions))
                {
                    return new Exception("格式錯誤：總計");
                }

                if (!string.IsNullOrEmpty(txtDiscountAmount.Text))
                {
                    if (!Regex.IsMatch(txtDiscountAmount.Text, RegularExpressions))
                    {
                        return new Exception("格式錯誤：扣抵金額");
                    }
                }

                CreateInvDetail();
                String[] checkItem;
                for (int idx = 0; idx < productItem.Count; idx++)
                {
                    checkItem = productItem[idx].Split(',');

                    if (!Regex.IsMatch(checkItem[1].ToString(), RegularExpressions))
                    {
                        return new Exception("發票品項內容格式錯誤：數量");
                    }

                    if (!Regex.IsMatch(checkItem[3].ToString(), RegularExpressions))
                    {
                        return new Exception("發票品項內容格式錯誤：單價");
                    }

                    if (!Regex.IsMatch(checkItem[4].ToString(), RegularExpressions))
                    {
                        return new Exception("發票品項內容格式錯誤：金額");
                    }
                }
            }
            //C0701
            else
            {
                if (string.IsNullOrEmpty(txtVoidReason.Text))
                {
                    return new Exception("註銷原因：長度至少為1");
                }
                if (txtVoidReason.Text.Length > 20)
                {
                    return new Exception("註銷原因：長度超過20");
                }
                if (txtRemark.Text.Length > 200)
                {
                    return new Exception("備註：長度超過200");
                }
            }
            return null;

        }

        private void CreateInvoiceItem()
        {
            if (divC0401.Visible == true)
            {
                item = new Model.Schema.MIG3_1.C0401.Invoice()
                {
                    Main = new Model.Schema.MIG3_1.C0401.Main
                    {
                        InvoiceNumber = txtInvNo.Text,
                        InvoiceDate = txtInvDate.Text,
                        InvoiceTime = _invoiceDate,
                        Seller = new Model.Schema.MIG3_1.C0401.MainSeller
                        {
                            Identifier = txtSellerId.Text,
                            Name = txtSellerName.Text,
                            PersonInCharge = txtSellerPersonInCharge.Text,
                            TelephoneNumber = txtSellerPhone.Text,
                            EmailAddress = txtSellerMail.Text,
                            FacsimileNumber = txtSellerFAX.Text,
                            Address = txtSellerAddress.Text,
                        },
                        Buyer = new Model.Schema.MIG3_1.C0401.MainBuyer
                        {
                            Identifier = txtBuyerId.Text,
                            Name = txtBuyerName.Text,
                            PersonInCharge = txtBuyerPersonInCharge.Text,
                            TelephoneNumber = txtBuyerPhone.Text,
                            EmailAddress = txtBuyerMail.Text,
                            FacsimileNumber = txtBuyerFAX.Text,
                            Address = txtBuyerAddress.Text,
                        },
                        CustomsClearanceMark = String.IsNullOrEmpty(txtCustomerMark.Text) ? Model.Schema.MIG3_1.C0401.CustomsClearanceMarkEnum.Item1 : (Model.Schema.MIG3_1.C0401.CustomsClearanceMarkEnum)int.Parse(txtCustomerMark.Text),
                        InvoiceType = (Model.Schema.MIG3_1.C0401.InvoiceTypeEnum)(int.Parse(ddInvoiceType.SelectedValue)),
                        PrintMark = ddPrintMark.SelectedValue,
                        DonateMark = (Model.Schema.MIG3_1.C0401.DonateMarkEnum)int.Parse(ddDonateMark.SelectedValue),
                        NPOBAN = txtNPOBAN.Text,
                        RandomNumber = txtRadomNo.Text,
                        CarrierType = txtCarryType.Text,
                        CarrierId1 = txtCarrierId1.Text,
                        CarrierId2 = txtCarrierId2.Text,
                    },
                    Amount = new Model.Schema.MIG3_1.C0401.Amount
                    {
                        DiscountAmount = !String.IsNullOrEmpty(txtDiscountAmount.Text) ? long.Parse(txtDiscountAmount.Text) : 0,
                        SalesAmount = long.Parse(txtSaleAmount.Text),
                        FreeTaxSalesAmount = long.Parse(txtFreeTaxSaleAmount.Text),
                        ZeroTaxSalesAmount = long.Parse(txtZeroTaxSaleAmount.Text),
                        TaxAmount = (long)decimal.Parse(txtTaxAmount.Text),
                        TaxRate = decimal.Parse(txtTaxRate.Text),
                        TaxType = (Model.Schema.MIG3_1.C0401.TaxTypeEnum)int.Parse(ddTaxType.SelectedValue),
                        TotalAmount = long.Parse(txtTotalAmount.Text),
                    },
                };

                CreateInvDetail();
                if (productItem != null && productItem.Count > 0)
                {
                    items = new List<Model.Schema.MIG3_1.C0401.DetailsProductItem>();
                    String[] checkItem;

                    for (int idx = 0; idx < productItem.Count; idx++)
                    {
                        checkItem = productItem[idx].Split(',');
                        items.Add(new Model.Schema.MIG3_1.C0401.DetailsProductItem()
                        {
                            Description = checkItem[0],
                            Quantity = decimal.Parse(checkItem[1]),
                            Unit = checkItem[2],
                            UnitPrice = decimal.Parse(checkItem[3]),
                            Amount = decimal.Parse(checkItem[4]),
                            Remark = checkItem[5],
                            RelateNumber = checkItem[6],
                            SequenceNumber = idx.ToString(),
                        });
                    }
                }
                item.Details = items.ToArray();
            }
            else
            {
                voiditem = new Model.Schema.TurnKey.C0701.VoidInvoice()
                {
                    VoidInvoiceNumber = txtVoidInvoiceNumber.Text,
                    InvoiceDate = txtInvoiceDate.Text,
                    BuyerId = txtBuyerId2.Text,
                    SellerId = txtSellerId2.Text,
                    VoidDate = DateTime.Now.Date.ToString("yyyyMMdd"),
                    VoidTime = DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss"),                    
                    VoidReason = txtVoidReason.Text,
                    Remark = txtRemark.Text
                };
            }



        }


        public void CreateInvDetail()
        {
            int idx = 1;
            productItem = new List<string>();
            String invoiceProduct = "";
            while (Request.Form["Brief" + idx] != null)
            {
                invoiceProduct = "";
                invoiceProduct += (Brief = Request.Form["Brief" + idx]) + ",";
                invoiceProduct += (Piece = Request.Form["Piece" + idx]) + ",";
                invoiceProduct += (PieceUnit = Request.Form["PieceUnit" + idx]) + ",";
                invoiceProduct += (UnitCost = Request.Form["UnitCost" + idx]) + ",";
                invoiceProduct += (CostAmount = Request.Form["CostAmount" + idx]) + ",";
                invoiceProduct += (Remark = Request.Form["Remark" + idx]) + ",";
                invoiceProduct += (RelateNumber = Request.Form["RelateNumber" + idx]);

                productItem.Add(invoiceProduct);
                idx = idx + 1;
            }
        }

        public String ShowInvDetail()
        {
            InvDetail += "<tr>";
            InvDetail += "<th><a style='color:red'>*</a>品名</th>";
            InvDetail += "<th><a style='color:red'>*</a>數量</th>";
            InvDetail += "<th>單位</th>";
            InvDetail += "<th><a style='color:red'>*</a>單價</th>";
            InvDetail += "<th><a style='color:red'>*</a>金額</th>";
            //InvDetail += "<th>明細排列序號</th>";
            //InvDetail += "<td class='tdleft' runat='server'>" + productItems.InvoiceProduct.Brief + "</asp:TextBox><asp:TextBox</td>";
            InvDetail += "<th>單一欄位備註</th>";
            InvDetail += "<th>相關號碼</th>";

            InvDetail += "</tr>";
            int id = 1;

            if (_invoiceItem != null)
            {
                foreach (var productItems in _invoiceItem.InvoiceDetails)
                {
                    InvDetail += "<tr>";

                    InvDetail += "<td class='tdleft' runat='server'><input name='Brief" + id + "' id='Brief" + id + "' type='text' value='" + productItems.InvoiceProduct.Brief + "'></td>";
                    InvDetail += "<td class='tdleft' runat='server'><input name='Piece" + id + "' id='Piece" + id + "' type='text' value='" + (long)productItems.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Piece + "'></td>";
                    InvDetail += "<td class='tdleft' runat='server'><input name='PieceUnit" + id + "' id='PieceUnit" + id + "' type='text' value='" + productItems.InvoiceProduct.InvoiceProductItem.FirstOrDefault().PieceUnit + "'></td>";
                    InvDetail += "<td class='tdleft' runat='server'><input name='UnitCost" + id + "' id='UnitCost" + id + "' type='text' value='" + (long)productItems.InvoiceProduct.InvoiceProductItem.FirstOrDefault().UnitCost + "'></td>";
                    InvDetail += "<td class='tdleft' runat='server'><input name='CostAmount" + id + "' id='CostAmount" + id + "' type='text' value='" + (long)productItems.InvoiceProduct.InvoiceProductItem.FirstOrDefault().CostAmount + "'></td>";
                    InvDetail += "<td class='tdleft' runat='server'><input name='Remark" + id + "' id='Remark" + id + "' type='text' value='" + productItems.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark + "'></td>";
                    InvDetail += "<td class='tdleft' runat='server'><input name='RelateNumber" + id + "' id='RelateNumber" + id + "' type='text' value='" + productItems.InvoiceProduct.InvoiceProductItem.FirstOrDefault().RelateNumber + "'></td>";

                    InvDetail += "</tr>";
                    id = id + 1;
                }
            }
            else
            {
                CreateInvDetail();
                String[] checkItem;

                for (int idx = 0; idx < productItem.Count; idx++)
                {
                    checkItem = productItem[idx].Split(',');
                    InvDetail += "<tr>";

                    InvDetail += "<td class='tdleft' runat='server'><input name='Brief" + id + "' id='Brief" + id + "' type='text' value='" + checkItem[0] + "'></td>";
                    InvDetail += "<td class='tdleft' runat='server'><input name='Piece" + id + "' id='Piece" + id + "' type='text' value='" + checkItem[1] + "'></td>";
                    InvDetail += "<td class='tdleft' runat='server'><input name='PieceUnit" + id + "' id='PieceUnit" + id + "' type='text' value='" + checkItem[2] + "'></td>";
                    InvDetail += "<td class='tdleft' runat='server'><input name='UnitCost" + id + "' id='UnitCost" + id + "' type='text' value='" + checkItem[3] + "'></td>";
                    InvDetail += "<td class='tdleft' runat='server'><input name='CostAmount" + id + "' id='CostAmount" + id + "' type='text' value='" + checkItem[4] + "'></td>";
                    InvDetail += "<td class='tdleft' runat='server'><input name='Remark" + id + "' id='Remark" + id + "' type='text' value='" + checkItem[5] + "'></td>";
                    InvDetail += "<td class='tdleft' runat='server'><input name='RelateNumber" + id + "' id='RelateNumber" + id + "' type='text' value='" + checkItem[6] + "'></td>";

                    InvDetail += "</tr>";
                    id = id + 1;
                }
            }

            return InvDetail;
        }
    }
}