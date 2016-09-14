<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadQRCodeKey.ascx.cs"
    Inherits="eIVOGo.Module.SYS.UploadQRCodeKey" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc5" %>
<uc2:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; QR Code金鑰維護" />
<div class="border_gray">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th class="Head_style_a" align="left">
                QR Code金鑰:<br />
                
            </th>
            <td>
                <asp:TextBox ID="QRCodeKey" runat="server" ></asp:TextBox>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnConfirm" runat="server" Text="上載" 
                    onclick="btnConfirm_Click" />
            </td>
        </tr>
    </table>
    </div>
<uc5:ActionHandler ID="doConfirm" runat="server" />
