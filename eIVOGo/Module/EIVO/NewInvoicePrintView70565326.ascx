<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewInvoicePrintView.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.NewInvoicePrintView" %>
<%@ Register Src="Item/InvoiceProductPrintView.ascx" TagName="InvoiceProductPrintView"
    TagPrefix="uc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Register Src="Item/InvoiceReceiptView.ascx" TagName="InvoiceReceiptView" TagPrefix="uc2" %>
<%@ Register Src="Item/InvoiceBalanceView.ascx" TagName="InvoiceBalanceView" TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/NewInvoiceProductPrintView70565326.ascx" TagName="NewInvoiceProductPrintView" TagPrefix="uc4" %>

<div style="page-break-after: always">
    <div class="fspace">
        <div class="company" style="padding-top: 0.5cm;">
            <span class="title"><%# _item.InvoiceSeller.CustomerName  %></span><br />
            <p>
                <%# _item.InvoiceSeller.Address  %><br />
                <%# _item.InvoiceSeller.Phone%>
            </p>
        </div>
        <div class="customer">
            <p>
                <%# _buyer.PostCode %><br />
                <%# _buyer.Address %><br />
                <%# _buyer.ContactName%>
            鈞啟
            </p>
        </div>
    </div>
    <div class="container">
        <div class="cutfield">
            <h3><%# _item.InvoiceSeller.CustomerName  %></h3>
            <h2>電子發票證明聯</h2>
            <h2><%# _item.InvoiceDate.Value.Year-1911 %>年<%# (_item.InvoiceDate.Value.Month % 2).Equals(0) ? String.Format("{0:00}-{1:00}", _item.InvoiceDate.Value.Month - 1, _item.InvoiceDate.Value.Month) : String.Format("{0:00}-{1:00}", _item.InvoiceDate.Value.Month, _item.InvoiceDate.Value.Month+1)%>月 </h2>
            <h2><%# _item.TrackCode + "-" + _item.No %></h2>
            <p>
                <%# String.Format("{0:yyyy-MM-dd HH:mm:ss}", _item.InvoiceDate.Value)%> <%# _buyer.IsB2C()?"": (_item.Organization.OrganizationStatus.SettingInvoiceType.Value == 8 ? "格式" : String.Format("格式 {0}", showState(_item.Organization.OrganizationStatus.SettingInvoiceType.Value))) %><br />
                隨機碼 <%# _item.RandomNo %>&nbsp;&nbsp;&nbsp;&nbsp;總計 <%# String.Format("{0:##,###,###,###}",_item.InvoiceAmountType.TotalAmount) %><br />
                賣方<%# _item.Organization.ReceiptNo%> <%# _buyer.IsB2C() ? null : String.Format("買方{0}", _buyer.ReceiptNo)%>
            </p>
            <div class="code1">
                <img id="barcode" alt="" runat="server" width="160" height="22" src="" /></div>
            <div class="code2">
                <img id="qrcode1" alt="" runat="server" width="60" height="60" src="" /><img id="qrcode2" alt="" runat="server" width="60" height="60" src="" />
            </div>
        </div>
        <div class="listfield" style="float:left;width:510px">
            <div class="content_box">
                <p class="productname" style="width:115px;">品名</p>
                <p class="quantity" style="width: 50px;">數量</p>
                <p class="price" style="width: 80px;">單價</p>
                <p class="totalPrice" style="width: 90px;">小計</p>
            </div>
            <asp:Repeater ID="rpList" runat="server" EnableViewState="false">
                <ItemTemplate>
                    <uc4:NewInvoiceProductPrintView ID="productView" runat="server" />
                </ItemTemplate>
            </asp:Repeater>
            <div class="content_box">
                <p style="border-top: 1px dotted #808080;">
                    <span style="font-size: 12pt; font-weight: bold;">總計：<%#_item.InvoiceDetails.Count %>項&nbsp;&nbsp;金額：<%# String.Format("{0:##,###,###,##0}", _item.InvoiceAmountType.TotalAmount)%></span><br />
                    課稅別：<%# (_item.InvoiceAmountType.TaxType == (byte)2 || _item.InvoiceAmountType.TaxType == (byte)3) ? "TZ" : "TX" %>&nbsp;&nbsp;<%# !_buyer.IsB2C() ? String.Format("應稅銷售額：{0:##,###,###,##0}  零稅率銷售額：{1:##,###,###,##0}  免稅銷售額：{2:##,###,###,##0}", ((_item.InvoiceAmountType.TaxType != (byte)2 && _item.InvoiceAmountType.TaxType != (byte)3) ? _item.InvoiceAmountType.SalesAmount : 0), (_item.InvoiceAmountType.TaxType == (byte)2 ? _item.InvoiceAmountType.SalesAmount : 0), (_item.InvoiceAmountType.TaxType == (byte)3 ? _item.InvoiceAmountType.SalesAmount : 0)) : ""%>
            &nbsp;&nbsp;<%# !_buyer.IsB2C() ? String.Format("稅額：{0:##,###,###,##0} ",  _item.InvoiceAmountType.TaxAmount) : ""%>&nbsp;&nbsp;<br />備註：<%# String.Join(";", _item.InvoiceDetails.Select(d => d.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark).ToArray().Where(s=>!String.IsNullOrEmpty(s))) %><br />
                    退貨請憑電子發票證明聯辦理
                </p>
            </div>
        </div>
    </div>
<%--    <div class="container">
        <div class="listfield">--%>
            <div class="content_box">
                <img runat="server" id="stampDuty" src="~/Seal/StampDuty70565326.jpg" style="width: 4.126cm; height: 2.22cm" visible="<%# _item.InvoiceItemExtension!=null && _item.InvoiceItemExtension.StampDutyFlag==1 %>" />
            </div>
            <div class="content_box" runat="server" visible="<%# _item.InvoiceItemExtension!=null &&  _item.InvoiceItemExtension!=null %>">
                <p><%# _item.InvoiceItemExtension!=null && !String.IsNullOrEmpty(_item.InvoiceItemExtension.ProjectNo) ? "專案編號：" + _item.InvoiceItemExtension.ProjectNo : null %></p>
                <p><%# _item.InvoiceItemExtension!=null && !String.IsNullOrEmpty(_item.InvoiceItemExtension.PurchaseNo) ? "採購案號：" + _item.InvoiceItemExtension.PurchaseNo : null %></p>
            </div>
            <div id="infoDuty" runat="server" visible="<%#  _item.InvoiceItemExtension!=null && _item.InvoiceItemExtension.StampDutyFlag==1 %>" class="content_box">本印花稅總繳戳章依財政部臺北市稅捐稽徵處南港分處所104年1月23日北市稽南港甲字第10449534200號函核准使用</div>
<%--        </div>
    </div>--%>
</div>
<%--<p runat="server" style="page-break-after: always" visible="<%# printBack ? true : (!IsFinal ? true : false) %>"></p>--%>

<div class="br" id="backPage" runat="server" style="page-break-after: always" visible="<%# printBack %>">
    <div class="bspace"></div>
    <div class="container">
        <div class="cutfield" style="float: right;">
            <h3 class="notop">領獎收據</h3>
            <p class="sign">
                發票年期別：<br />
                發票字軌號碼：<br />
                金額：新台幣<br />
                中獎人：<br />
                身分證字號：<br />
                地址：<br />
                電話：
            </p>
            <h3 class="notop">紙本電子發票注意事項</h3>
            <p class="rule1">中獎人請於領獎期間內攜帶國民身分證及本收執聯向下列郵局兌領，逾期不得領獎，影本不得進行領獎。</p>
            <p class="rule">(一) 特別獎、特獎及頭獎為各直轄市及各縣、市之指定郵局。</p>
            <p class="rule">(二) 二獎、三獎、四獎、五獎及六獎為各地郵局。</p>

        </div>
    </div>
</div>
<%--<p runat="server" style="page-break-after: always" visible="<%# printBack && !IsFinal %>"></p>--%>
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
<script runat="server">
    bool printBack = false;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (!String.IsNullOrEmpty(Request["printBack"]))
        {
            bool.TryParse(Request["printBack"], out printBack);
        }
    }
</script>
