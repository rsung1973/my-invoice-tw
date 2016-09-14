<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InquireEntity.ascx.cs" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/DataModelContainer.ascx" TagName="DataModelContainer"
    TagPrefix="uc8" %>
<asp:Button ID="btnAdd" runat="server" Text="新增料品" class="btn" OnClick="btnAdd_Click" />
<asp:Button ID="btnQuery" runat="server" Text="查詢" class="btn" OnClick="btnQuery_Click" />
<uc6:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
<uc8:DataModelContainer ID="modelItem" runat="server" />
