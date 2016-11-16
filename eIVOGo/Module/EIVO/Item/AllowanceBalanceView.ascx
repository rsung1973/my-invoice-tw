<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllowanceBalanceView.ascx.cs" Inherits="eIVOGo.Module.EIVO.Item.AllowanceBalanceView" %>
<%@ Import Namespace="Model.DataEntity" %>
<div class="bk" style="height: 520px;">
    <!--div class="fspace"></div>
<p style="border-bottom:1px dotted #000; margin-bottom:20px"></p-->
    <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="2">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="50%">
                            <table width="100%" border="0" cellpadding="3" cellspacing="0" class="table-bk">
                                <tr>
                                    <td width="10%" rowspan="3" align="center" nowrap="nowrap" class="nobottom-border">
                                        原
                                        <br />
                                        開發<br />
                                        立票<br />
                                        銷單<br />
                                        貨位
                                    </td>
                                    <td width="12%" align="center" nowrap="nowrap">
                                        營利事業<br />
                                        統一編號
                                    </td>
                                    <td class="f_black">
                                        <%# _item.CDS_Document.DocumentOwner.Organization.ReceiptNo %>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="12%" align="center" nowrap="nowrap">
                                        名 稱
                                    </td>
                                    <td class="f_black">
                                        <%# _item.CDS_Document.DocumentOwner.Organization.CompanyName %>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="12%" align="center" nowrap="nowrap" class="nobottom-border">
                                        營業所在<br />
                                        地址
                                    </td>
                                    <td class="f_black nobottom-border">
                                        <%# _item.CDS_Document.DocumentOwner.Organization.Addr %>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="center" style="padding-bottom: 5px;">
                            <span class="title" style="line-height: 150%;">銷貨退回<br />
                                營業人 進貨退出貨折讓證明單</span><br />
                            中華民國&nbsp;<span class="f_black"><%# _item.AllowanceDate.Value.Year-1911 %></span>&nbsp;年&nbsp;<span class="f_black"><%# String.Format("{0:00}",_item.AllowanceDate.Value.Month) %></span>&nbsp;月&nbsp;<span
                                class="f_black"><%# String.Format("{0:00}",_item.AllowanceDate.Value.Day) %></span>&nbsp;日
                        </td>
                    </tr>
                </table>
                <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0" class="table-bk">
                    <tr>
                        <th colspan="6">
                            開立發票
                        </th>
                        <th colspan="5">
                            退貨或折讓內容
                        </th>
                        <th colspan="3">
                            課稅別<br />
                            (V)
                        </th>
                    </tr>
                    <tr>
                        <th width="20" height="12">
                            聯<br />
                            式
                        </th>
                        <th width="20" height="12">
                            年
                        </th>
                        <th width="20" height="12">
                            月
                        </th>
                        <th width="20" height="12">
                            日
                        </th>
                        <th width="20">
                            字<br />
                            軌
                        </th>
                        <th>
                            號碼
                        </th>
                        <th>
                            品名
                        </th>
                        <th>
                            數量
                        </th>
                        <th>
                            單價
                        </th>
                        <th>
                            金額<br />
                            (不含稅之進貨額)
                        </th>
                        <th>
                            營業稅額
                        </th>
                        <th width="20">
                            應<br />
                            稅
                        </th>
                        <th width="20">
                            零<br />
                            稅<br />
                            率
                        </th>
                        <th width="20">
                            免<br />
                            稅
                        </th>
                    </tr>
                    <asp:Repeater ID="rpList" runat="server" EnableViewState="false" >
                    <ItemTemplate><tr>
                        <td height="15" align="center">
                            三
                        </td>
                        <td align="center">
                            <%# ((InvoiceAllowanceItem)Container.DataItem).InvoiceDate.Value.Year-1911 %>
                        </td>
                        <td align="center">
                            <%# String.Format("{0:00}",((InvoiceAllowanceItem)Container.DataItem).InvoiceDate.Value.Month) %>
                        </td>
                        <td align="center">
                            <%# String.Format("{0:00}", ((InvoiceAllowanceItem)Container.DataItem).InvoiceDate.Value.Day) %>
                        </td>
                        <td align="center">
                            <%# ((InvoiceAllowanceItem)Container.DataItem).InvoiceNo.Substring(0,2) %>
                        </td>
                        <td>
                            <%# ((InvoiceAllowanceItem)Container.DataItem).InvoiceNo.Substring(2) %>
                        </td>
                        <td>
                            <%# ((InvoiceAllowanceItem)Container.DataItem).OriginalDescription %>
                        </td>
                        <td align="right">
                            <%# String.Format("{0:##,###,###,###,###}", ((InvoiceAllowanceItem)Container.DataItem).Piece) %>
                        </td>
                        <td align="right">
                            <%# String.Format("{0:##,###,###,###,###}", ((InvoiceAllowanceItem)Container.DataItem).UnitCost) %>
                        </td>
                        <td align="right">
                            <%# String.Format("{0:##,###,###,###,###}", ((InvoiceAllowanceItem)Container.DataItem).Amount) %>
                        </td>
                        <td align="right">
                            <%# String.Format("{0:##,###,###,###,###}", ((InvoiceAllowanceItem)Container.DataItem).Tax) %>
                        </td>
                        <td align="center">
                            <%# ((InvoiceAllowanceItem)Container.DataItem).TaxType==(byte)1?"V":null %>
                        </td>
                        <td align="center">
                            <%# ((InvoiceAllowanceItem)Container.DataItem).TaxType==(byte)2?"V":null %>
                        </td>
                        <td align="center">
                            <%# ((InvoiceAllowanceItem)Container.DataItem).TaxType==(byte)3?"V":null %>
                        </td>
                    </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9" align="center">
                            合 計
                        </td>
                        <td align="right">
                            <%# String.Format("{0:##,###,###,###,###}", _item.TotalAmount ) %>
                        </td>
                        <td align="right">
                            <%# String.Format("{0:##,###,###,###,###}", _item.TaxAmount ) %>
                        </td>
                        <td colspan="3" align="center">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0" class="table-bk"
                    style="border-width: 0px;">
                    <tr>
                        <td valign="top" style="border-bottom-width: 0px;">
                           <%-- <p>
                                第一聯：交付原銷貨人作為發生銷貨退回或折讓當月(期)銷項稅額之扣減憑證並依規定申報。</p>
                            <p>
                                第二聯：交付原銷貨人作為記帳憑證。</p>--%>
                            <p>
                                第三聯：交付進貨人作為進項稅額之扣減憑證。</p>
                            <%--<p>
                                第四聯：交付進貨人作為記帳憑證。</p>--%>
                            <p>
                                本證明單所列進或退出或折讓，確屬事實，特此證明。</p>
                            <p style="padding-left: 10px">
                                原進貨營業人(或原買受人)名稱：<%# _item.InvoiceAllowanceBuyer.Organization!=null?_item.InvoiceAllowanceBuyer.Organization.CompanyName:_item.InvoiceAllowanceBuyer.Name %></p>
                            <p style="padding-left: 10px">
                                營利事業統一編號：<%# _item.BuyerId %></p>
                            <p style="padding-left: 10px">
                                地址：<%# _item.InvoiceAllowanceBuyer.Organization!=null?_item.InvoiceAllowanceBuyer.Organization.Addr:null %></p>
                        </td>
                        <td width="140" valign="top" style="padding: 0px;">
                            <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0" class="table-bk"
                                style="border-width: 0px;">
                                <tr>
                                    <th height="20" align="center" style="border-right-width: 0px;">
                                        營業人蓋用統一發票專用章
                                    </th>
                                </tr>
                                <tr>
                                    <td height="105" align="center" style="border-bottom-width: 0px; border-right-width: 0px;">
                                        <div class="eivo_stamp">
                                            <%# _item.InvoiceAllowanceBuyer.CustomerName %><br />
                                            統一編號<br />
                                            <div class="notitle">
                                                <%# _item.InvoiceAllowanceBuyer.ReceiptNo %>
                                            </div>
                                            電話:<%# _item.InvoiceAllowanceBuyer.Phone %><br />
                                            <%# _item.InvoiceAllowanceBuyer.Address %><br />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
