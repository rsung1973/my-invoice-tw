<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Supplier_Maintain.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Supplier_Maintain" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc4" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc4" %>
<%@ Register src="../Common/ClearInputField.ascx" tagname="ClearInputField" tagprefix="uc8" %>



<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 供應商資料維護" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="供應商資料維護" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="top_table">
    <tr>
        <td>
            <asp:Button ID="btnAdd" runat="server" Text="新增供應商" class="btn" OnClick="btnAdd_Click" />
        </td>
    </tr>
</table>
<div id="border_gray">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" id="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <tr>
            <th>
                統一編號
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtRno" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                供應商名稱
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox><font color="blue">可輸入關鍵字查詢</font>
            </td>
        </tr>
    </table>
</div>
<div id="div1" runat="server" visible="true">
    <table id="TableQuery" runat="server" width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnQuery" runat="server" Text="查詢" class="btn" OnClick="btnQuery_Click" />&nbsp;&nbsp;
                <uc8:ClearInputField ID="ClearInputField1" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div id="divImg" runat="server" visible="false">
    <h1>
        <img id="Img1" runat="server" src="~/images/icon_search.gif" width="29" height="28"
            border="0" align="absmiddle" />查詢結果</h1>
</div>
<div id="divResult" runat="server" visible="false" class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" CssClass="table01" AllowPaging="True"
        AllowSorting="True" AutoGenerateColumns="False" Width="100%" Style="text-align: center"
        OnRowCommand="gvEntity_RowCommand" OnRowDeleting="gvEntity_RowDeleting">
        <AlternatingRowStyle CssClass="OldLace" />
        <Columns>
            <asp:TemplateField HeaderText="供應商名稱">
                <ItemTemplate>
                    <%#  Eval("SUPPLIER_NAME")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="統一編號">
                <ItemTemplate>
                    <%# Eval("SUPPLIER_BAN")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="電話">
                <ItemTemplate>
                    <%# Eval("SUPPLIER_PHONE")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="聯絡人名稱">
                <ItemTemplate>
                    <%# Eval("SUPPLIER_CONTACT_NAME")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="聯絡人電子郵件">
                <ItemTemplate>
                    <%# Eval("CONTACT_EMAIL")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:Button ID="btnEdit" class="btn" runat="server" CommandName="Select" CommandArgument='<%# Eval("SUPPLIER_SN") %>'
                        Text="編輯"></asp:Button>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnDel" class="btn" runat="server" OnClientClick='if(confirm("確認刪除此筆資料?")){ return true;} else { return false; } '
                        CommandName="Delete" CommandArgument='<%# Eval("SUPPLIER_SN") %>' Text="刪除">
                    </asp:Button>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerTemplate>
            <uc2:PagingControl ID="pagingList" runat="server" />
        </PagerTemplate>
    </asp:GridView>
    <center>
        <asp:Label ID="lblError" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
    </center>
</div>
<!--表格 結束-->
