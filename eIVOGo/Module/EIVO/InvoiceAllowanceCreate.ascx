<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceAllowanceCreate.ascx.cs" Inherits="eIVOGo.Module.EIVO.InvoiceAllowanceCreate" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register src="../Common/SignContext.ascx" tagname="SignContext" tagprefix="uc1" %>
<%@ Register src="../Common/CalendarInput.ascx" tagname="CalendarInput" tagprefix="uc3" %>
<%@ Register src="../Common/CalendarInputDatePicker.ascx" tagname="CalendarInputDatePicker" tagprefix="uc4" %>
<%@ Register src="PNewInvalidInvoicePreview.ascx" tagname="PNewInvalidInvoicePreview" tagprefix="uc5" %>
<!--路徑名稱-->

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td width="30"><img runat="server" enableviewstate="false" id="img6" src="~/images/path_left.gif" alt="" width="30" height="29" /></td>
    <td bgcolor="#ecedd5">首頁 > 電子折讓單開立</td>
    <td width="18"><img runat="server" enableviewstate="false" id="img4" src="~/images/path_right.gif" alt="" width="18" height="29" /></td>
  </tr>
</table>
<!--交易畫面標題-->
<h1><img runat="server" enableviewstate="false" id="img5" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />電子折讓單開立</h1>
<div id="divQueryList" runat="server"  >
<!--表格 開始-->
<div id="border_gray">
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
  <tr>
    <th colspan="2" class="Head_style_a">查詢條件</th>
    </tr>
  
    <tr>
    <th width="20%">日期區間</th>
    <td class="tdleft">
    自 
        <uc4:CalendarInputDatePicker ID="CalendarInputDatePicker1" runat="server" />
    至&nbsp;
    &nbsp;<uc4:CalendarInputDatePicker ID="CalendarInputDatePicker2" runat="server" />
        </td>
  </tr>
  <tr >
    <th>發票號碼</th>
    <td class="tdleft"><asp:TextBox ID="InvoiceNo" type="text" class="textfield" size="20" runat="server" />
      </td>
  </tr>
  <tr >
    <th>訂單號碼</th>
    <td class="tdleft"><asp:TextBox ID="CheckNo" type="text" class="textfield" size="20" runat="server" />
      </td>
  </tr>

</table>
</div>

<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><asp:Button ID="btnQuery" class="btn" Text="查詢" 
            runat="server" onclick="btnQuery_Click"   />
        
      </td>
  </tr>
</table>
  <div id="divResult" visible="false" runat="server">
<h1><img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />查詢結果</h1>
  <div id="border_gray">
<asp:GridView ID="gvEntity" runat="server" EnableViewState="True" CssClass="table01"
    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
     Width="100%" onrowcommand="gvEntity_RowCommand">
    <AlternatingRowStyle CssClass="OldLace" />
    <Columns>
     <asp:TemplateField HeaderText="日期" >
            <ItemTemplate>
                <%# Utility .ValueValidity .ConvertChineseDate(Eval("InvoiceItem.InvoiceDate"))%>
            </ItemTemplate>
            <FooterStyle HorizontalAlign="Right" CssClass="total-count" />
        </asp:TemplateField>
      
        <asp:TemplateField HeaderText="開立發票營業人名稱" >
            <ItemTemplate>
                <%# Eval("Seller.CompanyName") %></ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
          <asp:TemplateField HeaderText="統編" >
            <ItemTemplate>
                <%# Eval("Seller.ReceiptNo") %>
            </ItemTemplate>
            <FooterStyle HorizontalAlign="Right" CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="訂單號碼" >
            <ItemTemplate>
                <%# Eval("InvoiceItem.CheckNo")%>
            </ItemTemplate>
            <FooterTemplate>
              </FooterTemplate>
            <FooterStyle HorizontalAlign="Right" CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="發票號碼" >
         <ItemTemplate>                    
                    <asp:LinkButton ID="lbtn" runat="server" Text='<%# String.Format("{0}{1}",Eval("InvoiceItem.TrackCode"), Eval("InvoiceItem.No"))%>' 
                     CausesValidation="false" CommandName="Edit" OnClientClick='<%# Page.ClientScript.GetPostBackEventReference(this, String.Format("S:{0}",Eval("InvoiceItem.InvoiceID").ToString())) + "; return false;" %>' />
                    </ItemTemplate>  
            <%--<ItemTemplate>
               <a href="NewInvalidInvoicePreview.aspx?id=<%#  Eval("InvoiceItem.InvoiceID")%>" target="_blank" ><%#  Eval("InvoiceItem.No")%></a>
            </ItemTemplate>--%>
            
            <FooterTemplate>
               &nbsp;&nbsp; 金額：
            </FooterTemplate>
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="金額" >
            <ItemTemplate>
                <%# String.Format("{0:##,###,###,##0.00}", Eval("TotalAmount"))%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <FooterTemplate></FooterTemplate>
           
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
         <asp:TemplateField HeaderText="捐贈單位" >
            <ItemTemplate>
                <%#  Eval("Donatory.CompanyName")%>
            </ItemTemplate>
            
            <FooterTemplate>
              </FooterTemplate>
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
         <asp:TemplateField  >
            <ItemTemplate>
                <asp:Button ID="btnManager"  runat="server" Text ="開立折讓" CommandName="Select"  CommandArgument='<%# Eval("InvoiceItem.No")%>' />
            </ItemTemplate>
          
            <FooterTemplate>
              </FooterTemplate>
            <FooterStyle CssClass="total-count" />
        </asp:TemplateField>
        
    </Columns>
    
    <PagerTemplate>
        
        <uc2:PagingControl ID="pagingList" runat="server" />
    </PagerTemplate>
</asp:GridView>
  <center>
            <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
            </center>
</div>
</div>
<!--表格 結束-->
</div>
<div id="divWorkForm" runat="server"  visible ="false">
   <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" 
                GridLines="None" CellPadding="0" CssClass="table01" 
         EnableViewState="True" 
        onrowcommand="GridView1_RowCommand">
                <AlternatingRowStyle CssClass="OldLace" />
            <Columns>
              <asp:TemplateField  ><HeaderTemplate ><asp:CheckBox ID="chk" 
        runat="server" AutoPostBack="true" oncheckedchanged="chk_CheckedChanged"   /></HeaderTemplate> <ItemTemplate><asp:CheckBox ID="chk" runat="server" /><asp:HiddenField ID="Item" runat="server" Value='<%# Eval("Item")%>' /><asp:HiddenField ID="ItemID" runat="server" Value='<%# Eval("ItemID")%>' /> </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
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
                <asp:TemplateField HeaderText="單價" > <ItemTemplate><%# String.Format("{0:0,0.00}", Eval("UnitCost"))%></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="金額" > <ItemTemplate><%# String.Format("{0:0,0.00}", Eval("CostAmount"))%></ItemTemplate>  
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="備註" > <ItemTemplate><%# Eval("Remark")%></ItemTemplate>  
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
            <br />
          
             <asp:GridView ID="GridView2" runat="server" 
        AutoGenerateColumns="False" Width="100%" 
                GridLines="Horizontal" CellPadding="3" 
        ClientIDMode="Static" BackColor="White" BorderColor="#E7E7FF" 
        BorderStyle="None" BorderWidth="1px">
                 <AlternatingRowStyle BackColor="#F7F7F7" />
            <Columns>
              <asp:TemplateField   HeaderStyle-CssClass="Head_style_a" HeaderText="已開立電子折讓單"  ItemStyle-HorizontalAlign="Center"    > <ItemTemplate  >
              <img src="~/images/paper.gif" width="20" height="25" border="0" align="absmiddle" runat="server"  /> <a href='AllowencePreview.aspx?id=<%#  Eval("ID")%>' target="_blank"><%#  Eval("No")%></a>
              </ItemTemplate> </asp:TemplateField> </Columns> 
                
    </asp:GridView> 
    
            <!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><asp:Button ID="btnAllowance" type="button" name="Add" 
            class="btn" Text="開立折讓單"  runat="server" onclick="btnAllowance_Click" />&nbsp;<asp:Button ID="btnCancell" type="button" name="Add" 
            class="btn" Text="取消"  runat="server" onclick="btnCancell_Click" /></td>
  </tr>
</table>
</div>
<uc5:PNewInvalidInvoicePreview ID="PNewInvalidInvoicePreview1" runat="server" />
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
<uc1:SignContext ID="SignContext1" runat="server" Catalog="開立折讓單" 
    UsePfxFile="False"     />
<asp:HiddenField ID="SignData" runat="server" /><asp:HiddenField ID="InvNo" runat="server" />
 </ContentTemplate>
    <Triggers>
        
        <asp:PostBackTrigger ControlID="btnQuery" />
        <asp:PostBackTrigger ControlID="gvEntity" />
        
    </Triggers>
</asp:UpdatePanel>
