<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Controllers" %>

<%  if (_item != null)
    { %>
<div style="page-break-after: always; width: 4.8cm; margin-left: 0cm; margin-right: 0cm">

    <div class="container" style="page-break-after: always; border-top: 0px; border-bottom: 0px">
        <table>
            <tr>
                <td>
                    <div class="cutfield" style="width: 5cm; border-top: 0px; border: 0px; font-weight: bold;">
                        <h3 style="width: 4.8cm; padding-top: 0px; font-weight: bold; height: 1.3cm;"><%=_item.SellerName  %></h3>
                        <h2>電子發票證明聯</h2>
                        <h2><%= _item.InvoiceDate.Value.Year-1911 %>年<%= (_item.InvoiceDate.Value.Month % 2).Equals(0) ? String.Format("{0:00}-{1:00}", _item.InvoiceDate.Value.Month - 1, _item.InvoiceDate.Value.Month) : String.Format("{0:00}-{1:00}", _item.InvoiceDate.Value.Month, _item.InvoiceDate.Value.Month+1)%>月 </h2>
                        <h2><%= _item.TrackCode + "-" + _item.No %></h2>
                        <p>
                            <%= String.Format("{0:yyyy-MM-dd HH:mm:ss}", _item.InvoiceDate.Value)%> <%= String.IsNullOrEmpty(_item.BuyerReceiptNo) ? "" : "格式 25" %><br />
                            隨機碼 <%= _item.RandomNo %> &nbsp;&nbsp;&nbsp;&nbsp; 總計 <%= String.Format("{0:##,###,###,###}",_item.TotalAmount) %><br />
                            賣方<%= _item.SellerReceiptNo%> <%= String.IsNullOrEmpty(_item.BuyerReceiptNo) ? null : "買方" + _item.BuyerReceiptNo %>
                        </p>
                        <div class="code1">
                            <%  Html.RenderPartial("~/Views/Module/InvoicePOSBarCode.ascx"); %>
                        </div>
                        <div class="code2">
                            <%  Html.RenderPartial("~/Views/Module/InvoicePOSQRCode.ascx"); %>
                        </div>
                    </div>
                </td>

            </tr>
        </table>
    </div>

    <div class="listfield" style="border-top: 0px; border-bottom: 0px">
        <table style="width: 4.8cm; font-size: 8pt; font-weight: bold;">
            <tr>
                <td style="width: 25%">


                    <p style="display: inline-block; padding: 2px 0px; margin: 0; font-size: 8pt; line-height: 1.5">品名</p>
                </td>
                <td style="width: 25%">
                    <p style="display: inline-block; padding: 2px 0px; margin: 0; font-size: 8pt; line-height: 1.5">數量</p>
                </td>
                <td style="width: 25%">
                    <p style="display: inline-block; padding: 2px 0px; margin: 0; font-size: 8pt; line-height: 1.5">單價</p>
                </td>
                <td style="width: 25%">
                    <p style="display: inline-block; padding: 2px 0px; margin: 0; font-size: 8pt; line-height: 1.5">小計</p>
                </td>
            </tr>

            <%  if (_item.Brief != null)
                {
                    int _itemIdx;
                    for (_itemIdx = 0; _itemIdx < _item.Brief.Length; _itemIdx++)
                    {            %>
            <tr>
                <td height="15" valign="top"><%= _item.Brief[_itemIdx] %></td>
                <td align="right" valign="top"><%= String.Format("{0:##,###,###,###}", _item.Piece[_itemIdx])%></td>
                <td align="right" valign="top"><%= String.Format("{0:##,###,###,###}", _item.UnitCost[_itemIdx]) %></td>
                <td align="right" valign="top"><%= String.Format("{0:##,###,###,###}", _item.CostAmount[_itemIdx])%></td>
            </tr>
            <%      }
                } %>
            <tr>
                <td colspan="4" style="font-size: 8pt;">

                    <p style="border-top: 1px dotted #808080;">
                        <span style="font-size: 8pt;">總計：<%=_item.Brief.Length%>項&nbsp;&nbsp;金額：<%= String.Format("{0:##,###,###,##0}", _item.TotalAmount)%></span><br />
                        課稅別：<%= (_item.TaxType == 2 || _item.TaxType == 3) ? "TZ" : "TX"%><br />
                <%  if (!String.IsNullOrEmpty(_item.BuyerReceiptNo))
                    {
                        decimal? salesAmt = 0;
                        decimal? zeroTaxAmt = 0;
                        decimal? freeTaxAmt = 0;
                        switch ((Naming.TaxTypeDefinition)_item.TaxType)
                        {
                            case Naming.TaxTypeDefinition.應稅:
                                salesAmt = _item.SalesAmount;
                                break;
                            case Naming.TaxTypeDefinition.零稅率:
                                zeroTaxAmt = _item.SalesAmount;
                                break;
                            case Naming.TaxTypeDefinition.免稅:
                                freeTaxAmt = _item.SalesAmount;
                                break;
                        }
                %>
                        應稅銷售額：<%= String.Format("{0:##,###,###,##0}",salesAmt) %><br />
                        零稅率銷售額：<%= String.Format("{0:##,###,###,##0}",zeroTaxAmt) %><br />
                        免稅銷售額：<%= String.Format("{0:##,###,###,##0}",freeTaxAmt) %><br />
                        稅額：<%= String.Format("{0:##,###,###,##0}",_item.TaxAmount) %><br />
                        <%  } %>
                備註：<%= _item.Remark %><br />
                        退貨請憑電子發票證明聯辦理
                    </p>
                </td>
            </tr>
        </table>
    </div>
</div>
<%--<%      if ((bool?)ViewBag.PrintBuyerAddr == true)
        { %>
            <div style="page-break-after: always; width: 4.8cm; margin-left: 0cm; margin-right: 0cm; padding-top: 0px;">
                <span style="font-family: Microsoft JhengHei; font-size: 1.2em; font-weight: bold;"><%= _buyer.PostCode %></span><br>
                <span style="font-family: Microsoft JhengHei; font-size: 1.2em; font-weight: bold;"><%= _buyer.Address %></span><br>
                <br>
                <span style="font-family: Microsoft JhengHei; font-size: 1.2em; font-weight: bold;"><%= _buyer.ContactName %> 鈞啟</span><br>
                <span style="font-size: 1.2em; font-weight: bold;">(No:<%=_item.No.Substring(0,5)+"***" %>)</span>
            </div>
<%      } %>--%>
<%  } %>
<%  if (_model != null)
    { %>
    <script>
        (function () {
            var beforePrint = function () {
            };

            var afterPrint = function () {
                window.close();
            };

            if (window.matchMedia) {
                var mediaQueryList = window.matchMedia('print');
                mediaQueryList.addListener(function (mql) {
                    if (mql.matches) {
                        beforePrint();
                    } else {
                        afterPrint();
                    }
                });
            }

            window.onbeforeprint = beforePrint;
            window.onafterprint = afterPrint;

            window.onload = function () {
                self.focus();
                self.print();
                //window.close();
            };
        })();
    </script>
<%  } %>

<script runat="server">

    ModelSource<InvoiceItem> models;
    InvoiceViewModel _item;
    InvoiceItem _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        _item = (InvoiceViewModel)ViewBag.ViewModel;
        _model = (InvoiceItem)this.Model;
    }

</script>
