<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadMIG.ascx.cs" Inherits="eIVOGo.Module.EIVO.DownloadMIG" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="DataAccessLayer.basis" %>

<asp:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<uc5:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 下載MIG_XML" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="下載MIG_XML" />
<div id="divC0401" runat="server" visible="false">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="6" class="Head_style_a">發票基本資料</th>
        </tr>
        <tr>
            <th><a style="color: red">*</a>發票號碼</th>
            <td class="tdleft">
                <asp:TextBox ID="txtInvNo" runat="server"></asp:TextBox>
            </td>
            <th><a style="color: red">*</a>發票日期</th>
            <td class="tdleft">
                <asp:TextBox ID="txtInvDate" runat="server"></asp:TextBox>
            </td>
            <th><a style="color: red">*</a>發票時間</th>
            <td class="tdleft">
                <asp:TextBox ID="txtInvTime" runat="server"></asp:TextBox>
            </td>
        </tr>

        <!--///////////////////分隔線//////////////////////-->


        <tr>
            <th colspan="6" class="Head_style_a">賣方資料</th>
        </tr>
        <tr>
            <th><a style="color: red">*</a>賣方統編</th>
            <td class="tdleft">
                <asp:TextBox ID="txtSellerId" runat="server"></asp:TextBox>
            </td>
            <th><a style="color: red">*</a>賣方名稱</th>
            <td class="tdleft">
                <asp:TextBox ID="txtSellerName" runat="server"></asp:TextBox>
            </td>
            <th>負責人姓名</th>
            <td class="tdleft">
                <asp:TextBox ID="txtSellerPersonInCharge" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>電話號碼</th>
            <td class="tdleft">
                <asp:TextBox ID="txtSellerPhone" runat="server"></asp:TextBox>
            </td>
            <%--<th>營業人角色註記</th>
            <td class="tdleft">
                <asp:TextBox ID="txtSellerRoleMark" runat="server"></asp:TextBox>
            </td>--%>
            <th>電子郵件地址</th>
            <td class="tdleft" colspan="4">
                <asp:TextBox ID="txtSellerMail" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>傳真號碼</th>
            <td class="tdleft">
                <asp:TextBox ID="txtSellerFAX" runat="server"></asp:TextBox>
            </td>
            <th>賣方地址</th>
            <td class="tdleft" colspan="7">
                <asp:TextBox ID="txtSellerAddress" runat="server" Width="100%"></asp:TextBox>
            </td>
            <%--<th>客戶編號</th>
            <td class="tdleft">
                <asp:TextBox ID="txtSellerCustomerNumber" runat="server"></asp:TextBox>
            </td>--%>
        </tr>

        <!--///////////////////////分隔線////////////////////////////-->

        <tr>
            <th colspan="6" class="Head_style_a">買方資料</th>
        </tr>
        <tr>
            <th><a style="color: red">*</a>買方統編</th>
            <td class="tdleft">
                <asp:TextBox ID="txtBuyerId" runat="server"></asp:TextBox>
            </td>
            <th><a style="color: red">*</a>買方名稱</th>
            <td class="tdleft">
                <asp:TextBox ID="txtBuyerName" runat="server"></asp:TextBox>
            </td>
            <th>負責人姓名</th>
            <td class="tdleft">
                <asp:TextBox ID="txtBuyerPersonInCharge" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <%-- <th>營業人角色註記</th>
            <td class="tdleft">
                <asp:TextBox ID="txtBuyerRoleMark" runat="server"></asp:TextBox>
            </td>--%>
            <th>電話號碼</th>
            <td class="tdleft">
                <asp:TextBox ID="txtBuyerPhone" runat="server"></asp:TextBox>
            </td>
            <th>電子郵件地址</th>
            <td class="tdleft" colspan="4">
                <asp:TextBox ID="txtBuyerMail" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>傳真號碼</th>
            <td class="tdleft">
                <asp:TextBox ID="txtBuyerFAX" runat="server"></asp:TextBox>
            </td>
            <th>買方地址</th>
            <td class="tdleft" colspan="7">
                <asp:TextBox ID="txtBuyerAddress" runat="server" Width="100%"></asp:TextBox>
            </td>
            <%--<th>客戶編號</th>
            <td class="tdleft">
                <asp:TextBox ID="txtBuyerCustomerNumber" runat="server"></asp:TextBox>
            </td>--%>
        </tr>

        <!--//////////////////發票主體/////////////////////-->

        <tr>
            <th colspan="6" class="Head_style_a">發票資料</th>
        </tr>
        <%--        <tr>
            <th>發票檢查碼</th>
            <td class="tdleft">
                <asp:TextBox ID="txtCheckNo" runat="server"></asp:TextBox>
            </td>
            <th>買受人註記欄</th>
            <td class="tdleft">
                <asp:TextBox ID="txtBuyerRemark" runat="server"></asp:TextBox>
            </td>
            <th>總備註</th>
            <td class="tdleft">
                <asp:TextBox ID="txtMainRemark" runat="server"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <th>通關方式註記</th>
            <td class="tdleft">
                <asp:DropDownList ID="ddCustomerMark" runat="server">
                    <asp:ListItem Value="0" Text="無註記" />
                    <asp:ListItem Value="1" Text="非經海關出口" />
                    <asp:ListItem Value="2" Text="經海關出口" />
                </asp:DropDownList>
                <!--<asp:TextBox ID="txtCustomerMark" runat="server"></asp:TextBox>-->
            </td>
            <th><a style="color: red">*</a>發票類別</th>
            <td class="tdleft">
                <asp:DropDownList ID="ddInvoiceType" runat="server">
                    <asp:ListItem Value="01" Text="01_三聯式" />
                    <asp:ListItem Value="02" Text="02_二聯式" />
                    <asp:ListItem Value="03" Text="03_二聯式收銀機" />
                    <asp:ListItem Value="04" Text="04_特種稅額" />
                    <asp:ListItem Value="05" Text="05_電子計算機" />
                    <asp:ListItem Value="06" Text="06_三聯式收銀機" />
                    <asp:ListItem Value="07" Text="07_一般稅額計算之電子發票" />
                    <asp:ListItem Value="08" Text="08_特種稅額計算之電子發票" />
                </asp:DropDownList>
                <!--<asp:TextBox ID="txtInvoiceType" runat="server"></asp:TextBox>-->
            </td>

            <th><a style="color: red">*</a>列印註記</th>
            <td class="tdleft">
                <asp:DropDownList ID="ddPrintMark" runat="server">
                    <asp:ListItem Value="Y" Text="已列印" />
                    <asp:ListItem Value="N" Text="未列印" />
                </asp:DropDownList>
                <!--<asp:TextBox ID="txtPrintMark" runat="server"></asp:TextBox>-->
            </td>
            <%--<th>沖帳別</th>
            <td class="tdleft">
                <asp:TextBox ID="txtCategory" runat="server"></asp:TextBox>
            </td>
            <th>相關號碼</th>
            <td class="tdleft">
                <asp:TextBox ID="txtRelateNo" runat="server"></asp:TextBox>
            </td>--%>
        </tr>
        <tr>
            <th><a style="color: red">*</a>捐贈註記</th>
            <td class="tdleft">
                <asp:DropDownList ID="ddDonateMark" runat="server">
                    <asp:ListItem Value="0" Text="未捐贈" />
                    <asp:ListItem Value="1" Text="已捐贈" />
                </asp:DropDownList>
                <!--<asp:TextBox ID="txtDonateMark" runat="server"></asp:TextBox>-->
            </td>
            <th>捐贈對象</th>
            <td class="tdleft">
                <asp:TextBox ID="txtNPOBAN" runat="server"></asp:TextBox>
            </td>
            <th><a style="color: red">*</a>發票防偽隨機碼</th>
            <td class="tdleft">
                <asp:TextBox ID="txtRadomNo" runat="server"></asp:TextBox>
            </td>
            <%--<th>彙開註記</th>
            <td class="tdleft">
                <asp:TextBox ID="txtGroupMark" runat="server"></asp:TextBox>
            </td>--%>
        </tr>
        <tr>
            <th>載具類別號碼</th>
            <td class="tdleft">
                <asp:TextBox ID="txtCarryType" runat="server"></asp:TextBox>
            </td>
            <th>載具顯碼id</th>
            <td class="tdleft">
                <asp:TextBox ID="txtCarrierId1" runat="server"></asp:TextBox>
            </td>
            <th>載具隱碼id</th>
            <td class="tdleft">
                <asp:TextBox ID="txtCarrierId2" runat="server"></asp:TextBox>
            </td>
        </tr>

        <!--///////////////////分隔線//////////////////-->



        <!--/////////////////分隔線///////////////////////-->

        <tr>
            <th colspan="6" class="Head_style_a">發票金額</th>
        </tr>
        <tr>
            <th><a style="color: red">*</a>應稅銷售額合計(新台幣)</th>
            <td class="tdleft">
                <asp:TextBox ID="txtSaleAmount" runat="server"></asp:TextBox>
            </td>
            <th><a style="color: red">*</a>免稅銷售額合計(新台幣)</th>
            <td class="tdleft">
                <asp:TextBox ID="txtFreeTaxSaleAmount" runat="server"></asp:TextBox>
            </td>
            <th><a style="color: red">*</a>零稅率銷售額合計(新台幣)</th>
            <td class="tdleft">
                <asp:TextBox ID="txtZeroTaxSaleAmount" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><a style="color: red">*</a>課稅別</th>
            <td class="tdleft">
                <asp:DropDownList ID="ddTaxType" runat="server">
                    <asp:ListItem Value="1" Text="1_應稅" />
                    <asp:ListItem Value="2" Text="2_零稅率" />
                    <asp:ListItem Value="3" Text="3_免稅" />
                    <asp:ListItem Value="4" Text="4_應稅(特種稅率)" />
                    <asp:ListItem Value="9" Text="9_混合稅率" />
                </asp:DropDownList>
                <!--<asp:TextBox ID="txtTaxType" runat="server"></asp:TextBox>-->
            </td>
            <th><a style="color: red">*</a>稅率</th>
            <td class="tdleft">
                <asp:TextBox ID="txtTaxRate" runat="server"></asp:TextBox>
            </td>
            <th><a style="color: red">*</a>營業稅額</th>
            <td class="tdleft">
                <asp:TextBox ID="txtTaxAmount" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><a style="color: red">*</a>總計</th>
            <td class="tdleft">
                <asp:TextBox ID="txtTotalAmount" runat="server"></asp:TextBox>
            </td>
            <th>扣抵金額</th>
            <td class="tdleft">
                <asp:TextBox ID="txtDiscountAmount" runat="server"></asp:TextBox>
            </td>
            <th>幣別</th>
            <td class="tdleft">
                <asp:TextBox ID="txtCurrency" runat="server"></asp:TextBox>
            </td>
            <%--<th>原幣金額</th>
            <td class="tdleft">
                <asp:TextBox ID="txtOriginalAmount" runat="server"></asp:TextBox>
            </td>--%>
        </tr>
        <%--<tr>
            <th>匯率</th>
            <td class="tdleft">
                <asp:TextBox ID="txtExchangeRate" runat="server"></asp:TextBox>
            </td>            
        </tr>--%>
    </table>

    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="14" class="Head_style_a">發票品項</th>
        </tr>

        <%=InvDetail %>
    </table>

</div>

<br />
<div id="divC0701" runat="server" visible="false">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title" id="C0401">
        <tr>
             <th colspan="6" class="Head_style_a">發票基本資料</th>
        </tr>
        <tr>
            <th><a style="color: red">*</a>註銷發票號碼</th>
            <td class="tdleft">
                <asp:TextBox ID="txtVoidInvoiceNumber" runat="server" Enabled="false" ></asp:TextBox>
            </td>
            <th><a style="color: red">*</a>發票日期</th>
            <td class="tdleft">
                <asp:TextBox ID="txtInvoiceDate" runat="server"  Enabled="false" ></asp:TextBox>
            </td>
        </tr>
         <tr>
            <th><a style="color: red">*</a>買方統一編號</th>
            <td class="tdleft">
                <asp:TextBox ID="txtBuyerId2" runat="server"  Enabled="false" ></asp:TextBox>
            </td>
            <th><a style="color: red">*</a>賣方統一編號</th>
            <td class="tdleft">
                <asp:TextBox ID="txtSellerId2" runat="server"  Enabled="false" ></asp:TextBox>
            </td>
        </tr>
         <%--<tr>
            <th><a style="color: red">*</a>註銷日期</th>
            <td class="tdleft">
                <asp:TextBox ID="txtVoidDate" runat="server"></asp:TextBox>
            </td>
            <th><a style="color: red">*</a>註銷時間</th>
            <td class="tdleft">
                <asp:TextBox ID="txtVoidTime" runat="server"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <th><a style="color: red">*</a>註銷原因</th>
            <td class="tdleft" colspan="3">
                <asp:TextBox ID="txtVoidReason" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>備註</th>
            <td class="tdleft" colspan="3">
                <asp:TextBox ID="txtRemark" runat="server" Width="100%" TextMode="MultiLine" ></asp:TextBox>
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>

<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn" align="center">
            <asp:Button ID="btnOK" runat="server" class="btn" Text="下載檔案" OnClick="btnOK_Click" Style="height: 21px" />
            &nbsp;
            <input name="Reset" type="reset" class="btn" value="重填" />
            &nbsp;
            <asp:Button ID="btnCancel" runat="server" class="btn" Text="取消" OnClick="btnCancel_Click" />
        </td>
    </tr>
</table>

<uc1:ActionHandler ID="doConfirm" runat="server" />
<cc1:InvoiceDataSource ID="dsEntity" runat="server"></cc1:InvoiceDataSource>

<script runat="server">

    string data;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (Page.PreviousPage != null)
        {
            data = Page.PreviousPage.Items["MIGTYPE"] as String;
            string InvID = data.Substring(0, data.IndexOf(","));
            string MIG_Type = data.Substring(data.IndexOf(",") + 1);

            var mgr = dsEntity.CreateDataManager();

            if (MIG_Type == "1")
            {
                divC0401.Visible = true;
                ShowInvData(mgr, MIG_Type, InvID);
            }
            else
            {
                divC0701.Visible = true;
                ShowInvData(mgr, MIG_Type, InvID);
            }
        }
    }

    void ShowInvData(GenericManager<EIVOEntityDataContext, InvoiceItem> mgr, String MIG_Type, String InvId)
    {
        _invoiceItem = mgr.EntityList.Where(i => i.InvoiceID == int.Parse(InvId)).FirstOrDefault();
        if (MIG_Type == "1" && _invoiceItem != null)
        {
            #region C0401

            txtInvNo.Text = _invoiceItem.TrackCode + _invoiceItem.No;
            txtInvDate.Text = _invoiceItem.InvoiceDate.Value.ToString("yyyyMMdd");
            txtInvTime.Text = _invoiceItem.InvoiceDate.Value.TimeOfDay.ToString();

            txtSellerId.Text = _invoiceItem.InvoiceSeller.ReceiptNo;
            txtSellerName.Text = _invoiceItem.InvoiceSeller.Organization.CompanyName;
            txtSellerPersonInCharge.Text = _invoiceItem.InvoiceSeller.PersonInCharge;
            //txtSellerRoleMark.Text
            txtSellerMail.Text = _invoiceItem.InvoiceSeller.EMail;
            txtSellerPhone.Text = _invoiceItem.InvoiceSeller.Phone;
            txtSellerFAX.Text = _invoiceItem.InvoiceSeller.Fax;
            //txtSellerCustomerNumber.Text

            txtBuyerId.Text = _invoiceItem.InvoiceBuyer.ReceiptNo;
            txtBuyerName.Text =  _invoiceItem.InvoiceBuyer.Name;
            txtBuyerPersonInCharge.Text = _invoiceItem.InvoiceBuyer.PersonInCharge;
            //txtBuyerRoleMark.Text
            txtBuyerMail.Text = _invoiceItem.InvoiceBuyer.EMail;
            txtBuyerPhone.Text = _invoiceItem.InvoiceBuyer.Phone;
            txtBuyerFAX.Text = _invoiceItem.InvoiceBuyer.Fax;
            //txtBuyerCustomerNumber.Text

            //txtCheckNo.Text
            //txtBuyerRemark.Text
            //txtMainRemark.Text

            //txtCustomerMark.Text = _invoiceItem.CustomsClearanceMark;
            if (String.IsNullOrEmpty(_invoiceItem.CustomsClearanceMark))
            {
                ddCustomerMark.Items[0].Selected = true;
            }
            else
            {
                for (int idx = 0; idx < ddCustomerMark.Items.Count; idx++)
                {
                    if (ddCustomerMark.Items[idx].Value == _invoiceItem.CustomsClearanceMark)
                    {
                        ddCustomerMark.Items[idx].Selected = true;
                    }
                }
            }

            //txtCategory.Text
            //txtRelateNo.Text

            for (int idx = 0; idx < ddInvoiceType.Items.Count; idx++)
            {
                if (ddInvoiceType.Items[idx].Value == ("0" + _invoiceItem.InvoiceType.ToString()))
                {
                    ddInvoiceType.Items[idx].Selected = true;
                }
            }

            //txtInvoiceType.Text = _invoiceItem.InvoiceType.ToString();
            //txtGroupMark.Text

            //txtDonateMark.Text = _invoiceItem.InvoiceDonation == null ? "0" : "1";
            for (int idx = 0; idx < ddDonateMark.Items.Count; idx++)
            {
                if (ddDonateMark.Items[idx].Value == _invoiceItem.DonateMark)
                {
                    ddDonateMark.Items[idx].Selected = true;
                }
            }

            if (_invoiceItem.InvoiceCarrier != null)
            {
                txtCarryType.Text = _invoiceItem.InvoiceCarrier.CarrierType;
                txtCarrierId1.Text = _invoiceItem.InvoiceCarrier.CarrierNo;
                txtCarrierId2.Text = _invoiceItem.InvoiceCarrier.CarrierNo2;
            }

            //txtPrintMark.Text = _invoiceItem.PrintMark;
            for (int idx = 0; idx < ddPrintMark.Items.Count; idx++)
            {
                if (ddPrintMark.Items[idx].Value == _invoiceItem.PrintMark)
                {
                    ddPrintMark.Items[idx].Selected = true;
                }
            }

            txtNPOBAN.Text = _invoiceItem.InvoiceDonation == null ? "" : _invoiceItem.InvoiceDonation.AgencyCode;
            txtRadomNo.Text = _invoiceItem.RandomNo;

            txtSaleAmount.Text = ((long)_invoiceItem.InvoiceAmountType.SalesAmount).ToString();
            txtFreeTaxSaleAmount.Text = ((long)_invoiceItem.InvoiceAmountType.SalesAmount).ToString();
            txtZeroTaxSaleAmount.Text = ((long)_invoiceItem.InvoiceAmountType.SalesAmount).ToString();

            //txtTaxType.Text = _invoiceItem.InvoiceAmountType.TaxType.ToString();
            for (int idx = 0; idx < ddTaxType.Items.Count; idx++)
            {
                if (ddTaxType.Items[idx].Value == (_invoiceItem.InvoiceAmountType.ToString()))
                {
                    ddTaxType.Items[idx].Selected = true;
                }
            }

            txtTaxRate.Text = _invoiceItem.InvoiceAmountType.TaxRate.ToString();
            txtTaxAmount.Text = _invoiceItem.InvoiceAmountType.TaxAmount.ToString();
            txtTotalAmount.Text = ((long)_invoiceItem.InvoiceAmountType.TotalAmount).ToString();
            txtDiscountAmount.Text = _invoiceItem.InvoiceAmountType.DiscountAmount.HasValue ? ((long)_invoiceItem.InvoiceAmountType.DiscountAmount).ToString() : "";
            //txtOriginalAmount.Text
            //txtExchangeRate.Text
            txtCurrency.Text = _invoiceItem.InvoiceAmountType.CurrencyType == null ? "" : _invoiceItem.InvoiceAmountType.CurrencyType.AbbrevName;

            ShowInvDetail();

            #endregion
        }
        else
        {
            #region C0701

            txtVoidInvoiceNumber.Text = _invoiceItem.TrackCode + _invoiceItem.No;
            txtInvoiceDate.Text = _invoiceItem.InvoiceDate.Value.ToString("yyyyMMdd");
            txtBuyerId2.Text = _invoiceItem.InvoiceBuyer.ReceiptNo;
            txtSellerId2.Text = _invoiceItem.InvoiceSeller.ReceiptNo;

            #endregion
        }
    }
        
</script>
