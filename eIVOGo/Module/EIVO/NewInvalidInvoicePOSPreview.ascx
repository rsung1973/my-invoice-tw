<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewInvalidInvoicePOSPreview.ascx.cs" Inherits="eIVOGo.Module.EIVO.NewInvalidInvoicePOSPreview" %>


<!--路徑名稱--><!--交易畫面標題-->
<h1><img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />發票</h1>
<div id="border_gray">
  <div id="invoice">
    <p class="idate">中華民國<asp:Label ID="lblInvoiceRange" CssClass="idata" runat="server"></asp:Label></p>
    <p class="inote">本單據僅供查詢發票資訊<br />不得列印本單據作為<br />領獎及其他憑據用途<br />
    發票號碼 <asp:Label ID="lblInvoiceNO" runat="server"></asp:Label></p>
    <p>營業人名稱：<asp:Label ID="lblSellerCompany" runat="server"></asp:Label></p>
    <p>營業人統一編號：<asp:Label ID="lblSellerReceiptNO" runat="server"></asp:Label></p>
    <p>營業人地址：<asp:Label ID="lblAddr" runat="server"></asp:Label></p>
    <p>開立日期：<asp:Label ID="lblInvoiceDate" runat="server"></asp:Label></p>
    <p>發票金額：<asp:Label ID="lblInvoicePrice" runat="server"></asp:Label></p>
    <p>載具類別：<asp:Label ID="lblCarrierType" runat="server"></asp:Label></p>
    <p>買受人營利事業統一編號：<asp:Label ID="lblBuyerReceiptNo" runat="server"></asp:Label></p>
  </div>
</div>
            <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%" 
                GridLines="None" CellPadding="0" CssClass="table01" ClientIDMode="Static" EnableViewState="false">
            <Columns>
                <asp:TemplateField HeaderText="序號" > <ItemTemplate><%# Container.DataItemIndex+1%></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="品名" > <ItemTemplate><%# Eval("Brief")%></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="數量" > <ItemTemplate><%# Eval("Piece")%></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="單位" > <ItemTemplate><%# Eval("PieceUnit")%></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="單價" > <ItemTemplate><%# String.Format("{0:0,0.00}",Eval("UnitCost"))%></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="金額" > <ItemTemplate><%# String.Format("{0:0,0.00}",Eval("CostAmount"))%></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="備註" > <ItemTemplate><%# Eval("Memo")%></ItemTemplate>  
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
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><span class="table-title">
      <input name="closewin" type="button" class="btn" value="關閉視窗" onclick="window.close();" />
    </span></td>
  </tr>
</table>
