<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.EIVO.Item.InvoiceReceiptView" %>
<%@ Register Src="InvoiceProductPrintView.ascx" TagName="InvoiceProductPrintView"
    TagPrefix="uc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<table width="95%" border="0" align="center" cellpadding="0" cellspacing="0">
    <tr>
        <td colspan="2">
            <table width="100%" border="0" cellpadding="1" cellspacing="0">
                <tr>
                    <td align="center" class="title">
                        <%# _item.CDS_Document.DocumentOwner.Organization.CompanyName  %>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="63%">
                        <table width="100%" border="0" cellpadding="1" cellspacing="0">
                            <tr>
                                <td width="12%" align="right" nowrap="nowrap">
                                    發票號碼：
                                </td>
                                <td class="f_black">
                                    <%# _item.TrackCode + _item.No %>
                                </td>
                                <td rowspan="4" align="right" valign="top">
                                    <table border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td align="center" nowrap="nowrap" class="title">
                                                電子計算機統一發票 (
                                                <%# _item.CDS_Document.DocumentPrintLog.Any(l=>l.TypeID==(int)Model.Locale.Naming.DocumentTypeDefinition.E_Invoice)?"副本":"正本" %>
                                                )
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                中華民國&nbsp;<span class="f_black"><%# _item.InvoiceDate.Value.Year-1911 %></span>&nbsp;年&nbsp;<span
                                                    class="f_black"><%# String.Format("{0:00}",_item.InvoiceDate.Value.Month) %></span>&nbsp;月&nbsp;<span
                                                        class="f_black"><%# String.Format("{0:00}",_item.InvoiceDate.Value.Day)%></span>&nbsp;日
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" nowrap="nowrap">
                                    檢查號碼：
                                </td>
                                <td class="f_black">
                                    <%# _item.CheckNo %>
                                </td>
                            </tr>
                            <tr>
                                <td width="12%" align="right" nowrap="nowrap">
                                    買 受 人：
                                </td>
                                <td class="f_black">
                                    <%# _buyer.IsB2C() ? null : _buyer.CustomerName%>
                                </td>
                            </tr>
                            <tr>
                                <td width="12%" align="right" nowrap="nowrap">
                                    統一編號：
                                </td>
                                <td class="f_black">
                                    <%# _buyer.IsB2C() ? null : _buyer.ReceiptNo%>
                                </td>
                            </tr>
                            <tr>
                                <td width="12%" align="right" nowrap="nowrap">
                                    地 址：
                                </td>
                                <td class="f_black" colspan="2">
                                    <%--<%# _buyerOrg != null ? _buyerOrg.Addr : _buyer != null && !_buyer.IsB2C() ? _buyer.Address : null%>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <table border="0" cellpadding="0" cellspacing="0" style="margin-bottom: 3px;" class="table-or">
                            <tr>
                                <td colspan="3" align="center">
                                    買受人註記欄
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    區 分
                                </td>
                                <td align="center">
                                    進貨及費用
                                </td>
                                <td align="center">
                                    固定資產
                                </td>
                            </tr>
                            <tr>
                                <td align="center" nowrap="nowrap">
                                    得 扣 抵
                                </td>
                                <td align="center" class="f_black">
                                    <%# _item.BuyerRemark=="1"?"V":"&nbsp;" %>
                                </td>
                                <td align="center" class="f_black">
                                    <%# _item.BuyerRemark=="2"?"V":"&nbsp;" %>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    不得扣抵
                                </td>
                                <td align="center" class="f_black">
                                    <%# _item.BuyerRemark=="3"?"V":"&nbsp;" %>
                                </td>
                                <td align="center" class="f_black">
                                    <%# _item.BuyerRemark=="4"?"V":"&nbsp;" %>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="orcube">
                <tr>
                    <td height="126" valign="top">
                        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0" class="item_or">
                            <tr>
                                <th height="12">
                                    品名
                                </th>
                                <th height="12">
                                    數量
                                </th>
                                <th height="12">
                                    單價
                                </th>
                                <th height="12">
                                    金 額
                                </th>
                            </tr>
                            <asp:Repeater ID="rpList" runat="server" EnableViewState="false">
                                <ItemTemplate>
                                    <uc1:InvoiceProductPrintView ID="productView" runat="server" />
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <td valign="top">
                                    &nbsp;
                                </td>
                                <td align="right" valign="top">
                                    &nbsp;
                                </td>
                                <td align="right" valign="top">
                                    &nbsp;
                                </td>
                                <td align="right" valign="top">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-or" style="border-width: 0px;">
                            <tr>
                                <td colspan="7" align="center" class="or_boder_top">
                                    銷 售 額 合 計
                                </td>
                                <td width="120" align="right" class="f_black or_boder_top">
                                    <%# String.Format("{0:##,###,###,###.00}",_item.InvoiceAmountType.SalesAmount) %>
                                </td>
                            </tr>
                            <tr>
                                <td width="100" align="center" nowrap="nowrap">
                                    營業稅
                                </td>
                                <td width="80" align="center" nowrap="nowrap">
                                    應稅
                                </td>
                                <td width="25" align="center" nowrap="nowrap" class="f_black">
                                    <%# _item.InvoiceAmountType.TaxType == (byte)1 ? "V" : "&nbsp;"%>
                                </td>
                                <td width="80" align="center" nowrap="nowrap">
                                    零稅率
                                </td>
                                <td width="25" align="center" nowrap="nowrap" class="f_black">
                                    <%# _item.InvoiceAmountType.TaxType == (byte)2 ? "V" : "&nbsp;"%>
                                </td>
                                <td width="80" align="center" nowrap="nowrap">
                                    免稅
                                </td>
                                <td width="25" align="center" nowrap="nowrap" class="f_black">
                                    <%# _item.InvoiceAmountType.TaxType == (byte)3 ? "V" : "&nbsp;"%>
                                </td>
                                <td width="120" align="right" class="f_black">
                                    <%# String.Format("{0:##,###,###,###.00}",_item.InvoiceAmountType.TaxAmount) %>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7" align="center">
                                    總 計
                                </td>
                                <td width="120" align="right" class="f_black">
                                    <%# String.Format("{0:##,###,###,###.00}",_item.InvoiceAmountType.TotalAmount) %>
                                </td>
                            </tr>
                            <tr>
                                <td height="15" colspan="8" style="border-bottom-width: 0px;">
                                    總計新台幣： <span class="f_black">
                                        <%# _totalAmtChar[7] %></span> 仟 <span class="f_black">
                                            <%# _totalAmtChar[6] %></span> 佰 <span class="f_black">
                                                <%# _totalAmtChar[5] %></span> 拾 <span class="f_black">
                                                    <%# _totalAmtChar[4] %></span> 萬 <span class="f_black">
                                                        <%# _totalAmtChar[3] %></span> 仟 <span class="f_black">
                                                            <%# _totalAmtChar[2] %></span> 佰 <span class="f_black">
                                                                <%# _totalAmtChar[1] %></span> 拾 <span class="f_black">
                                                                    <%# _totalAmtChar[0] %></span> 元 <span class="f_black">零</span>
                                    角 <span class="f_black">零</span> 分 元整
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="140" valign="top">
                        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0" class="table-or"
                            style="border-width: 0px;">
                            <tr>
                                <th height="12" style="border-right-width: 0px;">
                                    備 註
                                </th>
                            </tr>
                            <tr>
                                <td height="40" valign="top" class="f_black" style="border-right-width: 0px;">
                                    <%# _item.Remark %><br />
                                    <span class="f_black" style="border-right-width: 0px;">
                                        <%# _buyer.IsB2C() ? String.Format("個人識別碼:{0}", _buyer.Name) : null%></span>
                                </td>
                            </tr>
                            <tr>
                                <th height="20" align="center" style="border-right-width: 0px;">
                                    營業人蓋用統一發票專用章
                                </th>
                            </tr>
                            <tr>
                                <td height="105" align="center" style="border-bottom-width: 0px; border-right-width: 0px;">
                                    <img id="sealImg" runat="server" src="~/Seal/53147117.jpg" width="112" height="100" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" border="0" cellpadding="1" cellspacing="0">
                <tr>
                    <td align="center">
                        第二聯：扣抵聯
                    </td>
                </tr>
                <tr>
                    <td>
                        ※應稅、零稅率、免稅之銷售額應分別開立統一發票，並應於各該欄打「<span class="contant">V</span>」。<br />
                        本發票依北區國稅板橋三字第1001000841號函准使用。<br />
                        買受人註記欄之註記方法：<br />
                        營業人購進貨物或勞務應先按其用途區分為「進貨及費用」與「固定資產」，其進項稅額，除營業稅法第19條第1項屬不可扣抵外，其餘均得扣抵，並在各該適當欄內打「<span
                            class="contant">V</span>」符號。
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
