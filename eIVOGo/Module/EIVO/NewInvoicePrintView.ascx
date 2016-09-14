<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewInvoicePrintView.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.NewInvoicePrintView" %>
<%@ Register Src="Item/InvoiceProductPrintView.ascx" TagName="InvoiceProductPrintView"
    TagPrefix="uc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Register Src="Item/InvoiceReceiptView.ascx" TagName="InvoiceReceiptView" TagPrefix="uc2" %>
<%@ Register Src="Item/InvoiceBalanceView.ascx" TagName="InvoiceBalanceView" TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/NewInvoiceProductPrintView.ascx" TagName="NewInvoiceProductPrintView" TagPrefix="uc4" %>

<% if(_item!=null) { %>
<div style="page-break-after: always">
    <div class="fspace">
        <div class="company" style="padding-top: 0.5cm;">
            <span class="title"><%= _item.InvoiceSeller.CustomerName  %></span><br />
            <p>
                <%= _item.InvoiceSeller.Address  %><br />
                <%= _item.Organization.OrganizationStatus.SetToOutsourcingCS == true ? "委外客服電話：0800-010-626" : ""%>
            </p>
        </div>
        <div class="customer">
            <p>
                <%= _buyer.PostCode %><br />
                <%= _buyer.Address %><br />
                <%=_item.InvoiceSeller.ReceiptNo == "27934855" ? _buyer.CustomerName + "  " + _buyer.ContactName : _buyer.ContactName%><%--<%= _buyer.ContactName %> --%>
            鈞啟
            </p>
        </div>
    </div>
    <div class="container">
        <div style="width: 7.2cm; height: 9cm; display: block; overflow: hidden; float: left;"></div>
        <div class="cutfield">
            <h3><%= _item.InvoiceSeller.CustomerName  %></h3>
            <h2>電子發票證明聯</h2>
            <h2><%= _item.InvoiceDate.Value.Year-1911 %>年<%= (_item.InvoiceDate.Value.Month % 2).Equals(0) ? String.Format("{0:00}-{1:00}", _item.InvoiceDate.Value.Month - 1, _item.InvoiceDate.Value.Month) : String.Format("{0:00}-{1:00}", _item.InvoiceDate.Value.Month, _item.InvoiceDate.Value.Month+1)%>月 </h2>
            <h2><%= _item.TrackCode + "-" + _item.No %></h2>
            <p>
                <%= String.Format("{0:yyyy-MM-dd HH:mm:ss}", _item.InvoiceDate.Value)%> <%= _buyer.IsB2C()?"": (_item.Organization.OrganizationStatus.SettingInvoiceType.Value == 8 ? "格式" : String.Format("格式 {0}", showState(_item.Organization.OrganizationStatus.SettingInvoiceType.Value))) %><br />
                隨機碼 <%= _item.RandomNo %>&nbsp;&nbsp;&nbsp;&nbsp;總計 <%= String.Format("{0:##,###,###,###}",_item.InvoiceAmountType.TotalAmount) %><br />
                賣方<%= _item.Organization.ReceiptNo%> <%= _buyer.IsB2C() ? null : String.Format("買方{0}", _buyer.ReceiptNo)%>
            </p>
            <div class="code1">
                <img id="barcode" alt="" runat="server" width="160" height="22" src="" />
            </div>
            <div class="code2">
                <img id="qrcode1" alt="" runat="server" width="60" height="60" src="" /><img id="qrcode2" alt="" runat="server" width="60" height="60" src="" />
            </div>
        </div>
    </div>
    <div class="listfield">
        <div class="content_box">
            <p class="productname">品名</p>
            <p class="quantity">數量</p>
            <p class="price">單價</p>
            <p class="totalPrice">小計</p>
        </div>
        <% int _itemIdx;
           for (_itemIdx = 0; _productItem!=null &&  _itemIdx < Math.Min(_productItem.Length, _ItemPagingCount);_itemIdx++)
           {
               var item = _productItem[_itemIdx];
            %>
        <div class="content_box">
            <p class="productname"><%= item.InvoiceProduct.Brief%></p>
            <p class="quantity"><%= String.Format("{0:##,###,###,###}", item.Piece)%></p>
            <p class="price"><%= String.Format("{0:##,###,###,###}", item.UnitCost) %></p>
            <p class="totalPrice"><%= String.Format("{0:##,###,###,###}", item.CostAmount)%></p>
        </div>
        <% } %>
        <% if (_productItem != null && _productItem.Length <= _FirstCheckCount)
           { %>
        <div class="content_box">
            <p style="border-top: 1px dotted #808080;">
                <span style="font-size: 12pt; font-weight: bold;">總計：<%=_item.InvoiceDetails.Count%>項&nbsp;&nbsp;金額：<%= String.Format("{0:##,###,###,##0}", _item.InvoiceAmountType.TotalAmount)%></span><br />
                課稅別：<%= (_item.InvoiceAmountType.TaxType == (byte)2 || _item.InvoiceAmountType.TaxType == (byte)3) ? "TZ" : "TX"%>&nbsp;&nbsp;<%= !_buyer.IsB2C() ? String.Format("應稅銷售額：{0:##,###,###,##0}  零稅率銷售額：{1:##,###,###,##0}  免稅銷售額：{2:##,###,###,##0}", ((_item.InvoiceAmountType.TaxType != (byte)2 && _item.InvoiceAmountType.TaxType != (byte)3) ? _item.InvoiceAmountType.SalesAmount : 0), (_item.InvoiceAmountType.TaxType == (byte)2 ? _item.InvoiceAmountType.SalesAmount : 0), (_item.InvoiceAmountType.TaxType == (byte)3 ? _item.InvoiceAmountType.SalesAmount : 0)) : ""%>
            &nbsp;&nbsp;<%= !_buyer.IsB2C() ? String.Format("稅額：{0:##,###,###,##0} ", _item.InvoiceAmountType.TaxAmount) : ""%>&nbsp;&nbsp;備註：<%= String.Join(";", _item.InvoiceDetails.Select(d => d.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark))%><br />
                退貨請憑電子發票證明聯辦理
            </p>
        </div>
        <% } %>
    </div>
</div>
<%--<p runat="server" style="page-break-after: always" visible="<%= printBack ? true : (!IsFinal ? true : false) %>"></p>--%>
    <% if(printBack || (_productItem!=null && _productItem.Length>_FirstCheckCount))  { %>
<div class="br" id="backPage" style="page-break-after: always">
    <div class="bspace listfield">
        <% for (; _itemIdx < Math.Min(_productItem.Length, _ItemPagingCount*2);_itemIdx++)
           {
               var item = _productItem[_itemIdx];
        %>
        <div class="content_box">
            <p class="productname"><%= item.InvoiceProduct.Brief%></p>
            <p class="quantity"><%= String.Format("{0:##,###,###,###}", item.Piece)%></p>
            <p class="price"><%= String.Format("{0:##,###,###,###}", item.UnitCost) %></p>
            <p class="totalPrice"><%= String.Format("{0:##,###,###,###}", item.CostAmount)%></p>
        </div>
        <% } %>
        <% if (_productItem.Length>_FirstCheckCount && _productItem.Length <= _SecondCheckCount)
           { %>
        <div class="content_box">
            <p style="border-top: 1px dotted #808080;">
                <span style="font-size: 12pt; font-weight: bold;">總計：<%=_item.InvoiceDetails.Count%>項&nbsp;&nbsp;金額：<%= String.Format("{0:##,###,###,##0}", _item.InvoiceAmountType.TotalAmount)%></span><br />
                課稅別：<%= (_item.InvoiceAmountType.TaxType == (byte)2 || _item.InvoiceAmountType.TaxType == (byte)3) ? "TZ" : "TX"%>&nbsp;&nbsp;<%= !_buyer.IsB2C() ? String.Format("應稅銷售額：{0:##,###,###,##0}  零稅率銷售額：{1:##,###,###,##0}  免稅銷售額：{2:##,###,###,##0}", ((_item.InvoiceAmountType.TaxType != (byte)2 && _item.InvoiceAmountType.TaxType != (byte)3) ? _item.InvoiceAmountType.SalesAmount : 0), (_item.InvoiceAmountType.TaxType == (byte)2 ? _item.InvoiceAmountType.SalesAmount : 0), (_item.InvoiceAmountType.TaxType == (byte)3 ? _item.InvoiceAmountType.SalesAmount : 0)) : ""%>
            &nbsp;&nbsp;<%= !_buyer.IsB2C() ? String.Format("稅額：{0:##,###,###,##0} ", _item.InvoiceAmountType.TaxAmount) : ""%>&nbsp;&nbsp;備註：<%= String.Join(";", _item.InvoiceDetails.Select(d => d.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark))%><br />
                退貨請憑電子發票證明聯辦理
            </p>
        </div>
        <% } %>
    </div>
    <% if(printBack) { %>
    <div class="container">
        <div style="width: 7.2cm; height: 9cm; display: block; overflow: hidden; float: right;"></div>
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
    <% } %>
</div>
    <% } %>
    <% if(_productItem!=null && _productItem.Length>_SecondCheckCount)  { %>
<div class="br" style="page-break-after: always">
    <div class="listfield">
        <% for (; _itemIdx < _productItem.Length;_itemIdx++)
           {
               var item = _productItem[_itemIdx];
        %>
        <div class="content_box">
            <p class="productname"><%= item.InvoiceProduct.Brief%></p>
            <p class="quantity"><%= String.Format("{0:##,###,###,###}", item.Piece)%></p>
            <p class="price"><%= String.Format("{0:##,###,###,###}", item.UnitCost) %></p>
            <p class="totalPrice"><%= String.Format("{0:##,###,###,###}", item.CostAmount)%></p>
        </div>
        <% } %>
        <div class="content_box">
            <p style="border-top: 1px dotted #808080;">
                <span style="font-size: 12pt; font-weight: bold;">總計：<%=_item.InvoiceDetails.Count%>項&nbsp;&nbsp;金額：<%= String.Format("{0:##,###,###,##0}", _item.InvoiceAmountType.TotalAmount)%></span><br />
                課稅別：<%= (_item.InvoiceAmountType.TaxType == (byte)2 || _item.InvoiceAmountType.TaxType == (byte)3) ? "TZ" : "TX"%>&nbsp;&nbsp;<%= !_buyer.IsB2C() ? String.Format("應稅銷售額：{0:##,###,###,##0}  零稅率銷售額：{1:##,###,###,##0}  免稅銷售額：{2:##,###,###,##0}", ((_item.InvoiceAmountType.TaxType != (byte)2 && _item.InvoiceAmountType.TaxType != (byte)3) ? _item.InvoiceAmountType.SalesAmount : 0), (_item.InvoiceAmountType.TaxType == (byte)2 ? _item.InvoiceAmountType.SalesAmount : 0), (_item.InvoiceAmountType.TaxType == (byte)3 ? _item.InvoiceAmountType.SalesAmount : 0)) : ""%>
            &nbsp;&nbsp;<%= !_buyer.IsB2C() ? String.Format("稅額：{0:##,###,###,##0} ", _item.InvoiceAmountType.TaxAmount) : ""%>&nbsp;&nbsp;備註：<%= String.Join(";", _item.InvoiceDetails.Select(d => d.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark))%><br />
                退貨請憑電子發票證明聯辦理
            </p>
        </div>
    </div>
</div>
        <% if((_productItem.Length-_SecondCheckCount) / 43 %2 == 0) { %>
<div class="br" style="page-break-after: always">&nbsp;</div>
        <% } %>
    <% } %>
<% } %>
<%--<p runat="server" style="page-break-after: always" visible="<%= printBack && !IsFinal %>"></p>--%>
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
    
    [System.ComponentModel.Bindable(true)]
    public String InvoiceNo
    {
        get
        {
            return _item != null ? _item.TrackCode + _item.No : null;
        }
        set
        {
            if (value != null && value.Length == 10)
            {
                var item = dsEntity.CreateDataManager().EntityList.Where(i => i.TrackCode == value.Substring(0, 2) && i.No == value.Substring(2)).FirstOrDefault();
                if (item != null)
                {
                    this.InvoiceID = item.InvoiceID;
                }
            }
        }
    }
    
</script>
