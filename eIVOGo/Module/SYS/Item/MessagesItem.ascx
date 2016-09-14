<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessagesItem.ascx.cs" Inherits="eIVOGo.Module.SYS.Item.MessagesItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc4" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 700px; z-index:100000 !important; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <!--路徑名稱-->
        <!--交易畫面標題-->
        <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="跑馬燈訊息維護" />
        <!--按鈕-->
        <div class="border_gray">
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr>
                    <th width="20%">
                        <font color="red">*</font> 訊息內容
                    </th>
                    <td class="tdleft" style="width:80%;">
                        <asp:TextBox ID="txtMsg" runat="server" Width="95%" Text='<%#_entity.MessageContents %>' TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th width="20%">
                        訊息顯示日期(起)
                    </th>
                    <td class="tdleft">
                        <uc2:CalendarInputDatePicker ID="DateFrom" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th width="20%">
                        訊息顯示日期(迄)
                    </th>
                    <td class="tdleft">
                       <uc2:CalendarInputDatePicker ID="DateTo" runat="server" /></td>
                </tr>
                <tr>
                    <th width="20%">
                        訊息永久顯示
                    </th>
                    <td class="tdleft">                    
                         <asp:CheckBox ID="chkShowForever" Checked='<%# _entity.AlwaysShow%>' runat="server" />
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="Bargain_btn" align="center">
                        <asp:Button ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            Text="確定" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="取消" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Panel>
<asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnPopup"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" DropShadow="true"
    PopupDragHandleControlID="Panel3" />
<cc1:SystemMessagesDataSource ID="dsEntity" runat="server">
</cc1:SystemMessagesDataSource>
<uc3:ActionHandler ID="doConfirm" runat="server" />
<uc4:DataModelCache ID="modelItem" runat="server" KeyName="MsgID" />