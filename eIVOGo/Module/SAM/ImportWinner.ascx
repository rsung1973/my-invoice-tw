<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportWinner.ascx.cs" Inherits="eIVOGo.Module.SAM.ImportWinner" %>
<%@ Register src="../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc2" %>

<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
    <tr>
        <td width="30"><img runat="server" id="img2" enableviewstate="false" src="~/images/path_left.gif" alt="" width="30" height="29" /></td>
        <td bgcolor="#ecedd5">首頁 > 中獎清冊匯入</td>
        <td width="18"><img  runat="server" id="img3" enableviewstate="false" src="~/images/path_right.gif" alt="" width="18" height="29" /></td>
    </tr>
</table>
<!--交易畫面標題-->
<h1><img runat="server" id="img1" enableviewstate="false" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />中獎清冊匯入</h1>
<div id="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th width="20%">中獎清冊匯入</th>
            <td class="tdleft">
                <asp:FileUpload ID="FileUpload1" CssClass="textfield" runat="server" />&nbsp;
                <asp:Button ID="btnImport" CssClass="btn" runat="server" Text="確認" onclick="btnImport_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%" GridLines="None" CellPadding="0" CssClass="table01" style="margin-top:10px;" AllowPaging="True" ClientIDMode="Static" EnableViewState="false" >
    <Columns>
        <asp:TemplateField HeaderText="發票號碼" > <ItemTemplate><%# ((dataType)Container.DataItem).TrackCode%><%# ((dataType)Container.DataItem).No%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="開立發票營業人" > <ItemTemplate><%# ((dataType)Container.DataItem).CompanyName%></ItemTemplate>  
            <ItemStyle HorizontalAlign="left" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="統編" > <ItemTemplate><%# ((dataType)Container.DataItem).ReceiptNo%></ItemTemplate>  
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="中獎獎別" Visible="false" > <ItemTemplate><%# WinningTypeTransform(((dataType)Container.DataItem).WinningType)%></ItemTemplate>  
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="中獎金額" Visible="false" > <ItemTemplate><%#String.Format("{0:0,0.00}", ((dataType)Container.DataItem).BonusDescription)%></ItemTemplate>  
            <ItemStyle HorizontalAlign="right" />
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="資料比對狀態" > <ItemTemplate><%# ((dataType)Container.DataItem).isMatch%></ItemTemplate>  
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField> 
    </Columns>         
    <FooterStyle />
    <PagerStyle />
    <SelectedRowStyle />
    <HeaderStyle />
    <AlternatingRowStyle CssClass="OldLace" />
        <PagerTemplate>
            <span>
            <uc2:PagingControl ID="pagingIndex" runat="server" />
            </span>
        </PagerTemplate>
    <RowStyle />
    <EditRowStyle />
    </asp:GridView>
    <div style="text-align:center;padding-top:10px;">
    <asp:Label ID="lblError" Visible="false" style="font-size:16px;color:Red;" runat="server"></asp:Label>
    </div>
    <!--表格 結束-->
</div>
