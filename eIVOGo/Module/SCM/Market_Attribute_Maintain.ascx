<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Market_Attribute_Maintain.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Market_Attribute_Maintain" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc4" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc4" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 網購通路平台屬性維護" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="網購通路平台屬性維護" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="top_table">
    <tr>
        <td>
            <asp:Button ID="btnAdd" runat="server" Text="新增網購通路平台屬性" class="btn" OnClick="btnAdd_Click" />
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
            <th width="30%">
                網購通路來源名稱
            </th>
            <td class="tdleft">
                <asp:DropDownList ID="ddlMarket" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" runat="server" Text="查詢" class="btn" OnClick="btnQuery_Click" />
        </td>
    </tr>
</table>
<div id="divResult" runat="server" visible="false">
    <h1>
        <img src="~/images/icon_search.gif" runat="server" width="29" height="28" border="0"
            align="absmiddle" />查詢結果</h1>
    <div class="border_gray">
        <asp:GridView ID="gvEntity" runat="server" CssClass="table01" AllowPaging="True"
            AllowSorting="True" AutoGenerateColumns="False" Width="100%" Style="text-align: center"
            OnRowCommand="gvEntity_RowCommand" OnRowDeleting="gvEntity_RowDeleting">
            <AlternatingRowStyle CssClass="OldLace" />
            <Columns>
                <asp:TemplateField HeaderText="網購通路來源名稱">
                    <ItemTemplate>
                        <%#  ((MARKET_ATTR)Container.DataItem).MARKET_RESOURCE_NAME%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="網購通路平台屬性名稱">
                    <ItemTemplate>
                        <%# ((MARKET_ATTR)Container.DataItem).MARKET_ATTR_NAME%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" class="btn" runat="server" CommandName="Select" CommandArgument='<%# ((MARKET_ATTR)Container.DataItem).MARKET_ATTR_SN%>'
                            Text="編輯"></asp:Button>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnDel" class="btn" runat="server" OnClientClick='if(confirm("確認刪除此筆資料?")){ return true;} else { return false; } '
                            CommandName="Delete" CommandArgument='<%# ((MARKET_ATTR)Container.DataItem).MARKET_ATTR_SN %>'
                            Text="刪除"></asp:Button>
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
</div>