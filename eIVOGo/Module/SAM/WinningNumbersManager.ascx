<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WinningNumbersManager.ascx.cs" Inherits="eIVOGo.Module.SAM.WinningNumbersManager" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register src="~/Module/Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc2" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc3" %>

<!--路徑名稱-->
<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 > 電子發票中獎號碼維護" />
<!--交易畫面標題-->
<uc3:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="電子發票中獎號碼維護" />
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="top_table">
    <tr>
        <td>
            <asp:Button ID="btnAdd" runat="server" class="btn" Text="新增開獎號碼" OnClick="btnAdd_Click" />
        </td>
    </tr>
</table>
<div id="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <tr>
            <th nowrap="nowrap" width="150">
                發票年度（民國年）
            </th>
            <td class="tdleft">
                <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th nowrap="nowrap width="150">
                發票期別
            </th>
            <td class="tdleft">
                <asp:DropDownList ID="ddlRange" runat="server"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn" align="center">
            <asp:Button ID="btnQuery" runat="server" class="btn" OnClick="btnQuery_Click" Text="查詢" />
        </td>
    </tr>
</table>
<!--按鈕-->
    <div id="divResult" visible="false" runat="server">
        <h1><img runat="server" enableviewstate="false" id="img4" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />查詢結果</h1>
        <!--表格 開始-->
        <div id="border_gray">
            <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" 
                CssClass="table01" Width="100%" ClientIDMode="Static" 
                BorderWidth="0px" CellPadding="0" GridLines="None" AllowPaging="True" EnableViewState="false" >
                <Columns>
                    <asp:TemplateField HeaderText="發票年度"><ItemTemplate><%# (((UniInvoiceNO)Container.DataItem).year-1911).ToString()+"年" %></ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="發票期別"><ItemTemplate><%# ((UniInvoiceNO)Container.DataItem).period%></ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="特別獎"><ItemTemplate><%# ((UniInvoiceNO)Container.DataItem).SpecialPrize%></ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="特獎"><ItemTemplate><%# ((UniInvoiceNO)Container.DataItem).GrandPrize%></ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="頭獎"><ItemTemplate><%# ((UniInvoiceNO)Container.DataItem).FirstPrize%></ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="增開六獎"><ItemTemplate><%# ((UniInvoiceNO)Container.DataItem).AdditionalSixthPrize%></ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="管理">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CssClass="btn" Text="編輯"
                                onclientclick='<%# Page.ClientScript.GetPostBackEventReference(this, String.Format("U:{0}",((UniInvoiceNO)Container.DataItem).ID)) + "; return false;" %>' />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnDelete" runat="server" CssClass="btn" Text="刪除"
                                onclientclick='<%# String.Format("if(confirm(\"確認刪除此筆資料?\")) {0} ", Page.ClientScript.GetPostBackEventReference(this, String.Format("D:{0}",((UniInvoiceNO)Container.DataItem).ID)))  + "; return false;"%>' />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnMatchNum" runat="server" CssClass="btn" Text="啟動兌獎作業"
                                onclientclick='<%# Page.ClientScript.GetPostBackEventReference(this, String.Format("M:{0}",((UniInvoiceNO)Container.DataItem).ID)) + "; return false;" %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="OldLace" />
                <PagerTemplate>
                    <uc2:PagingControl ID="pagingList" runat="server" />
                </PagerTemplate>
            </asp:GridView>
            <center>
            <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
            </center>
        </div>
    </div>