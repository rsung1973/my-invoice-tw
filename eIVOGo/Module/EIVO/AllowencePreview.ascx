<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllowencePreview.ascx.cs" Inherits="eIVOGo.Module.EIVO.AllowencePreview" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<h1><img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />折讓</h1>
 <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%" 
                GridLines="None" CellPadding="0" CssClass="table01" 
         EnableViewState="false"  
        >
                <AlternatingRowStyle CssClass="OldLace" />
            <Columns>
            
                <asp:TemplateField HeaderText="序號" > <ItemTemplate><%#  Eval("invAitem.No")%></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="品名"  > <ItemTemplate><%# Eval("Brief")%></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="數量" > <ItemTemplate><%#String.Format("{0:#,0}", Eval("invAitem.Piece"))%></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="單位" > <ItemTemplate><%# Eval("invAitem.PieceUnit")%></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="單價" > <ItemTemplate><%# String.Format("{0:0,0.00}", Eval("UnitCost"))%></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="金額(不含稅之進貨額)" > <ItemTemplate><%# String.Format("{0:0,0.00}", Eval("invAitem.Amount"))%></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="營業稅額" > <ItemTemplate><%# String.Format("{0:#,0.00}", Eval("invAitem.Tax"))%></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
            </Columns>      
             <EmptyDataTemplate>
                <div align="center"><font color="red">查無資料!!</font></div>
            </EmptyDataTemplate>   
            <FooterStyle />
            <PagerStyle HorizontalAlign="Center" />
            <SelectedRowStyle />
            <HeaderStyle />
            <AlternatingRowStyle CssClass="OldLace" />
            <RowStyle />
            <EditRowStyle />
             <PagerTemplate>
        
                    <uc2:PagingControl ID="pagingList" runat="server" />
            </PagerTemplate>
            </asp:GridView>
            <!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><input type="button" name="closewindows" id="btnQuery" class="btn" value="關閉視窗" 
             onclick='javascript:window.opener=null;window.open("","_self");window.close();'   /></td>
  </tr>
</table>
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>