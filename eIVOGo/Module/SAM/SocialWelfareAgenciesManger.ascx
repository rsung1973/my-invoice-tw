<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SocialWelfareAgenciesManger.ascx.cs" Inherits="eIVOGo.Module.SAM.SocialWelfareAgenciesManger" %>
<%@ Register src="../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.Locale" TagPrefix="cc2" %>

<!--路徑名稱-->
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td width="30"><img runat="server" enableviewstate="false" id="img3" src="~/images/path_left.gif" alt="" width="30" height="29" /></td>
    <td bgcolor="#ecedd5">首頁 > 社福機構資料維護</td>
    <td width="18"><img runat="server" enableviewstate="false" id="img2" src="~/images/path_right.gif" alt="" width="18" height="29" /></td>
  </tr>
</table>
<!--交易畫面標題-->
<h1><img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />社福機構資料維護</h1>
<div id="border_gray">
      <asp:GridView ID="gvEntity" runat="server" AllowPaging="True"  Width="100%" CellPadding="0" GridLines="None" 
        AutoGenerateColumns="False" CssClass="table01" ClientIDMode="Static" 
        EnableViewState="false" >
        <Columns >    
        <asp:TemplateField HeaderText="社福機構統編"><ItemTemplate><%# ((dataType)Container.DataItem).OrgCode %></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="社福機構名稱"><ItemTemplate><%# ((dataType)Container.DataItem).OrgName%></ItemTemplate>
            <ItemStyle HorizontalAlign="Left" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="住址"><ItemTemplate><%# ((dataType)Container.DataItem).OrgAddr%></ItemTemplate>
            <ItemStyle HorizontalAlign="Left" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="電話"><ItemTemplate><%# ((dataType)Container.DataItem).OrgPhone%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="社福狀態"><ItemTemplate><%# ((dataType)Container.DataItem).OrgStatus%></ItemTemplate>
            <ItemStyle HorizontalAlign="center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="管理"><ItemTemplate >
            <asp:Button ID="btnUpdate" class="btn"  runat="server" CommandName="Select" Text="編輯" Enabled='<%# ((dataType)Container.DataItem).OrgCurrentLevel==(int)Naming.MemberStatusDefinition.Mark_To_Delete?false:true%>' OnClientClick='<%# Page.ClientScript.GetPostBackEventReference(this,String.Format("U:{0}", ((dataType)Container.DataItem).OrgID)) + "; return false;" %>'></asp:Button>
            <asp:Button ID="btnDel" class="btn"  runat="server" CommandName="Delete" Text='<%# ((dataType)Container.DataItem).OrgCurrentLevel==(int)Naming.MemberStatusDefinition.Mark_To_Delete?"啟用":"停用"%>' OnClientClick='<%# String.Format("if(confirm(\"確認{0}此筆資料?\")) {1} ",((dataType)Container.DataItem).OrgCurrentLevel==(int)Naming.MemberStatusDefinition.Mark_To_Delete?"啟用":"停用" , Page.ClientScript.GetPostBackEventReference(this, String.Format("D:{0}",((dataType)Container.DataItem).OrgID)))  + "; return false;" %>'></asp:Button></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        </Columns>
        <FooterStyle />
        <PagerStyle HorizontalAlign="right" />
        <SelectedRowStyle />
        <HeaderStyle />
        <AlternatingRowStyle CssClass="OldLace" />
            <PagerTemplate>
                <span>                    
                <uc1:PagingControl ID="pagingIndex" runat="server" />                    
                </span>
            </PagerTemplate>
        <RowStyle />
        <EditRowStyle />
    </asp:GridView>
    <center>
        <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
    </center>
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn">
        <asp:Button ID="btnAdd" Text="新增社福機構" CssClass="btn" runat="server" onclick="btnAdd_Click" />
        </td>
  </tr>
</table>
<!--按鈕-->
