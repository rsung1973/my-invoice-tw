<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PNewInvalidInvoicePreview.ascx.cs" Inherits="eIVOGo.Module.EIVO.PNewInvalidInvoicePreview" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<asp:Button ID="btnHidden" runat="Server" Style="display: none" />
<asp:HiddenField ID="HiddenField1" runat="server" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <!--路徑名稱-->
        <div id="divInvoice" runat="server">
            <uc1:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="發票" />
            <div id="border_gray">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                    <tr>
                        <th width="100">
                            發票號碼
                        </th>
                        <td class="tdleft">
                            <asp:Label ID="lblInvoiceNO" runat="server"></asp:Label>
                        </td>
                        <th width="100" nowrap="nowrap">
                            個人識別碼
                        </th>
                        <td class="tdleft">
                            <asp:Label ID="lblRandonNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th width="100">
                            未稅金額
                        </th>
                        <td class="tdleft">
                            <asp:Label ID="lblSalesAmount" runat="server"></asp:Label>
                        </td>
                        <th width="100">
                            稅 額
                        </th>
                        <td class="tdleft">
                            <asp:Label ID="lblTaxAmount" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th width="100">
                            含稅金額
                        </th>
                        <td class="tdleft">
                            <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                        </td>
                        <th width="100">
                            日 期
                        </th>
                        <td class="tdleft">
                            <asp:Label ID="lblInvoiceDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th width="100">
                            買受人統編
                        </th>
                        <td colspan="3" class="tdleft">
                            <asp:Label ID="lblBuyerReceiptNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
                    GridLines="None" CellPadding="0" CssClass="table01" ClientIDMode="Static" EnableViewState="false" AllowPaging="False">
                    <Columns>
                        <asp:TemplateField HeaderText="序號">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="品名">
                            <ItemTemplate>
                                <%# Eval("Brief")%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="數量">
                            <ItemTemplate>
                                <%# Eval("Piece")%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="單位">
                            <ItemTemplate>
                                <%# Eval("PieceUnit")%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="單價">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0.00}",Eval("UnitCost"))%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="金額">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0.00}",Eval("CostAmount"))%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="備註">
                            <ItemTemplate>
                                <%# Eval("Memo")%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle />
                    <PagerStyle HorizontalAlign="Center" />
                    <SelectedRowStyle />
                    <HeaderStyle />
                    <AlternatingRowStyle CssClass="OldLace" />
                    <RowStyle />
                    <EditRowStyle />
                </asp:GridView>
                <p><asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label></p>
            </div>
        </div>
        <div id="divAllowance" runat="server">
            <uc1:FunctionTitleBar ID="FunctionTitleBar2" runat="server" ItemName="折讓" />
            <div id="border_gray">
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title" >
                <tr>
                    <th width="100" nowrap="nowrap">折讓日期</th>
                    <td class="tdleft"><asp:Label ID="lblAllowanceDate" runat="server"></asp:Label></td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr>
                    <th width="100" colspan="4" class="Head_style_a">原開立銷貨發票單位</th>
                </tr>
                <tr>
                    <th width="100" nowrap="nowrap">統一編號</th>
                    <td width="30%" class="tdleft"><asp:Label ID="lblReceiptNo" runat="server"></asp:Label></td>
                    <th width="100" nowrap="nowrap">名　　稱</th>
                    <td class="tdleft"><asp:Label ID="lblCompanyName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th width="100">營業所在地址</th>
                    <td colspan="3" class="tdleft"><asp:Label ID="lblCompanyAddr" runat="server"></asp:Label></td>
                </tr>
            </table>
                <br />
                <asp:GridView ID="gvAllowance" runat="server" AutoGenerateColumns="False" Width="100%"
                    GridLines="None" CellPadding="0" CssClass="table01" ClientIDMode="Static" EnableViewState="false"  AllowPaging="False"
                    OnRowCreated="gvAllowance_RowCreated">
                    <Columns>
                        <asp:TemplateField HeaderText="聯式">
                            <ItemTemplate>
                                <%--<%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowance.InvoiceAllowanceBuyer.ReceiptNo.Equals("0000000000") ? "二" : "三"%>--%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="年">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.InvoiceDate.Value.Year%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="月">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.InvoiceDate.Value.Month%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="日">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.InvoiceDate.Value.Day%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="字軌">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.InvoiceNo.Substring(0,2)%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="號碼">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.InvoiceNo.Substring(2)%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="品名">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.OriginalDescription%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="數量">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.Piece%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="單價">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0.00}", ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.UnitCost) %></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="金額&lt;br/&gt;(不含稅之進貨額)">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0.00}", (((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.Amount - ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.Tax))%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="營業稅額">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0.00}", ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.Tax) %></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle />
                    <PagerStyle HorizontalAlign="Center" />
                    <SelectedRowStyle />
                    <HeaderStyle />
                    <AlternatingRowStyle CssClass="OldLace" />
                    <RowStyle />
                    <EditRowStyle />
                </asp:GridView>
            <!--表格 結束-->
            </div>
        </div>
        <!--按鈕-->
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="Bargain_btn">
                    <span class="table-title">
                        <asp:Button ID="CancelButton" CssClass="btn" runat="server" Text="關閉視窗" />
                    </span>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnHidden"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" CancelControlID="CancelButton"
    DropShadow="true" PopupDragHandleControlID="Panel3" />
