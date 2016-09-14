<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaintainMenuNodes.ascx.cs" Inherits="OpenSite.module.sam.MaintainMenuNodes" %>
<%@ Register Src="~/Module/Common/WebPageTreeView.ascx" TagName="WebPageTreeView" TagPrefix="uc1" %>
<table width="750" class="inputText">
<tr>
    <td width="50%" valign="top" align="left">
        <asp:Panel ID="Panel1" runat="server" Height="100%" ScrollBars="Both" Width="100%">
            <uc1:WebPageTreeView ID="webPageTree" runat="server" SearchPattern=".+\.aspx|.+\.htm" />
        </asp:Panel>
    </td>
    <td valign="top" align="left">
        <asp:Panel ID="Panel2" runat="server" Height="100%" ScrollBars="Both" Width="100%">
            <asp:TreeView ID="menuTree" runat="server" CssClass="inputText" DataSourceID="dsMenu"
                ExpandDepth="1" ShowCheckBoxes="All" ShowLines="True" Font-Size="XX-Small" OnSelectedNodeChanged="menuTree_SelectedNodeChanged">
                <SelectedNodeStyle BackColor="Yellow" ForeColor="Red" />
                <DataBindings>
                    <asp:TreeNodeBinding DataMember="menuItem" TextField="value" />
                    <asp:TreeNodeBinding DataMember="workItem" TextField="value" />
                </DataBindings>
            </asp:TreeView>
            <asp:XmlDataSource ID="dsMenu" runat="server" EnableCaching="False">
            </asp:XmlDataSource>
            <asp:Button ID="btnDeleteNode" runat="server" CssClass="inputText" OnClick="btnDeleteNode_Click"
                Text="刪除節點" /><br />
            選單名稱 :
            <asp:TextBox ID="menuName" runat="server" CssClass="inputText" Columns="48"></asp:TextBox><br />
            工作網頁 :
            <asp:TextBox ID="menuUrl" runat="server" CssClass="inputText" Columns="48"></asp:TextBox><br />
            <asp:Button ID="btnConfirm" runat="server" CssClass="inputText" Text="修改節點" OnClick="btnConfirm_Click" /><br />
            <asp:RadioButton ID="rbMenuItem" runat="server" Checked="True" Text="選單項" CssClass="inputText" GroupName="G1" />
            <asp:RadioButton ID="rbWorkItem" runat="server" CssClass="inputText" GroupName="G1"
                Text="工作項" />
            <asp:RadioButton ID="rbRoot" runat="server" CssClass="inputText" GroupName="G1"
                Text="主功能項" /><br />
            <asp:Button ID="btnInsert" runat="server" CssClass="inputText" Text="插入節點" OnClick="btnInsert_Click" /></asp:Panel>
    </td>
</tr>
<tr>
<td colspan="2" align="center">
    <asp:Button ID="btnSave" runat="server" CssClass="inputText" Text="存檔" OnClick="btnSave_Click" />
    &nbsp;
    <asp:Button ID="btnCancel" runat="server" CssClass="inputText" Text="取消" OnClick="btnCancel_Click" /></td>
</tr>
</table>