<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateOrganizationCertificate.ascx.cs"
    Inherits="eIVOGo.Module.SAM.CreateOrganizationCertificate" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<div id="border_gray">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th class="Head_style_a" align="left">
                PKCS12(PFX)憑證檔:<br /><asp:FileUpload ID="PfxFile" runat="server" />
            </th>
            <td>
                PIN Code:<asp:TextBox ID="PIN" runat="server" TextMode="Password"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnConfirm" runat="server" Text="上載" 
                    CommandArgument='<%# Eval("CompanyID") %>' onclick="btnConfirm_Click" />
            </td>
        </tr>
        <tr>
            <th class="Head_style_a" colspan="2"><asp:Button ID="btnUpload" runat="server" Text="建立憑證資訊" CommandName="Upload" CommandArgument='<%# Eval("CompanyID") %>'
        OnClick="btnUpload_Click" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <input name="closewin" type="button" class="btn" value="關閉視窗" onclick="window.close();" />
    </th>
        </tr>
    </table>
    <!--表格 開始-->
    <!--表格 結束-->
    <br />
    <font color="red">
        <asp:Literal ID="msg" runat="server" EnableViewState="false"></asp:Literal></font>
</div>
<cc1:OrganizationDataSource ID="dsOrg" runat="server">
</cc1:OrganizationDataSource>
<uc1:SignContext ID="signContext" runat="server" />
