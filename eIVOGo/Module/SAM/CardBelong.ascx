<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CardBelong.ascx.cs" Inherits="eIVOGo.Module.SAM.CardBelong" %>
<!--路徑名稱-->
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td width="30"><img runat="server" enableviewstate="false" id="img1" src="~/images/path_left.gif" alt="" width="30" height="29" /></td>
    <td bgcolor="#ecedd5">首頁 > 會員載具歸戶</td>
    <td width="18"><img runat="server" enableviewstate="false" id="img2" src="~/images/path_right.gif" alt="" width="18" height="29" /></td>
  </tr>
</table>
<!--交易畫面標題-->
<h1><img runat="server" enableviewstate="false" id="img3" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />會員載具歸戶</h1>
<div id="border_gray">
<!--表格 開始-->
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
  <tr>
    <th width="20%"> <span style="color: red">*</span>載具種類</th>
    <td class="tdleft">
        <asp:DropDownList ID="ddlcardType" runat="server" Height="21px" Width="120px">
        </asp:DropDownList>
  </tr>
  <tr id="uxb2b">
    <th> <span style="color: red">*</span>卡號</th>
    <td class="tdleft"><asp:TextBox ID="CardID" runat="server" ></asp:TextBox>
      （共20碼）<asp:Button ID="btnOk" runat="server" Text ="確定" class="btn" 
            onclick="btnOk_Click" />
        </td>
  </tr>
</table>
 <br />
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td  align="center" >
                   <asp:Label ID="lblmsg" runat="server"  ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
<!--表格 結束-->
</div>
<!--按鈕-->
<h1><img runat="server" enableviewstate="false" id="img4" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />已歸戶載具清單</h1>
<div id="border_gray">
  <!--表格 開始-->
  <asp:GridView ID="gvEntity" runat="server" AllowPaging="True"  Width="100%"
    BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" 
    CellPadding="2" ForeColor="Black" GridLines="None" 
    AutoGenerateColumns="False" onrowcommand="gvEntity_RowCommand" 
        onpageindexchanged="gvEntity_PageIndexChanged" 
        onpageindexchanging="gvEntity_PageIndexChanging" 
        onrowdeleting="gvEntity_RowDeleting">
    <Columns >
    <asp:TemplateField >
        <HeaderTemplate   >載具種類</HeaderTemplate> 
        <ItemTemplate><%# Eval("CareType") %></ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField >
        <HeaderTemplate   >載具卡號</HeaderTemplate> 
        <ItemTemplate><%# Eval("CareID") %></ItemTemplate>
    </asp:TemplateField>
    
   <asp:TemplateField >
            
             <ItemTemplate >
             <asp:Button ID="btnDel"  class="btn" runat="server" CommandName="Delete" CommandArgument='<%# Eval("CarrierID")%>' Text="刪除"></asp:Button>       
             </ItemTemplate> </asp:TemplateField>
      
    </Columns>
    <AlternatingRowStyle BackColor="PaleGoldenrod" />
    <FooterStyle BackColor="Tan" />
    <HeaderStyle BackColor="Tan" Font-Bold="True" />
    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
        HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
    <SortedAscendingCellStyle BackColor="#FAFAE7" />
    <SortedAscendingHeaderStyle BackColor="#DAC09E" />
    <SortedDescendingCellStyle BackColor="#E1DB9C" />
    <SortedDescendingHeaderStyle BackColor="#C2A47B" />
</asp:GridView>
  <!--表格 結束-->
</div>
