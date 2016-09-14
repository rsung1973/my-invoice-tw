<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PupProdNameSelect.ascx.cs" Inherits="eIVOGo.Module.UI.PupProdNameSelect" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:UpdatePanel ID="Updatepanel1" runat="server">
    <ContentTemplate>
        <asp:TextBox ID="txtProdName" CssClass="textfield" runat="server"></asp:TextBox>
        <asp:Button ID="btnQueryProd" CssClass="btn" Text="查詢選擇" runat="server" 
            onclick="btnQueryProd_Click" />
        <font color="blue">可輸入關鍵字查詢</font>
        <asp:Button ID="btnHidden" runat="Server" Style="display: none" /> 
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:Panel ID="Panel1" runat="server" Style="display: none; width:650px; background-color:#ffffdd; border-width:3px; border-style:solid; border-color:Gray; padding:3px;">
            <asp:Panel ID="Panel3" runat="server" Style="cursor: move;background-color:#DDDDDD;border:solid 1px Gray;color:Black">
                <!--路徑名稱--><!--交易畫面標題-->
                <h1><img runat="server" enableviewstate="false" id="img2" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />料品選擇</h1>
                <div id="Div1">
                    <asp:CheckBoxList ID="chkProdList" runat="server" RepeatColumns="5" Width="100%" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                    <asp:RadioButtonList ID="rdbSocialWelfare" runat="server" RepeatColumns="3" Width="100%" RepeatDirection="Horizontal">
                    </asp:RadioButtonList>
                </div>
                <center><p><asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label></p></center>
                <!--按鈕-->
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="Bargain_btn"><span class="table-title">
                            <asp:Button ID="OkButton" runat="server" Text="OK" OnClick="OkButton_Click" />
                            <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
                        </span></td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" 
            TargetControlID="btnHidden"
            PopupControlID="Panel1" 
            BackgroundCssClass="modalBackground" 
            CancelControlID="CancelButton" 
            DropShadow="true"
            PopupDragHandleControlID="Panel3" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQueryProd" EventName="Click" />
        <asp:PostBackTrigger ControlID="OkButton" />
    </Triggers>
</asp:UpdatePanel>
<cc1:PurchaseDataSource ID="PurchaseDataSource1" runat="server">
</cc1:PurchaseDataSource>
