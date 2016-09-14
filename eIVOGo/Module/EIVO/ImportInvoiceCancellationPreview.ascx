<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportInvoiceCancellationPreview.ascx.cs" Inherits="eIVOGo.Module.EIVO.ImportInvoiceCancellationPreview" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc5" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc6" %>
<%@ Register src="../Common/PrintingButton2.ascx" tagname="PrintingButton2" tagprefix="uc4" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc4" %>
<%@ Register src="Item/InvoiceCancellationUploadList.ascx" tagname="InvoiceCancellationUploadList" tagprefix="uc7" %>
<%@ Register src="../Common/PageAnchor.ascx" tagname="PageAnchor" tagprefix="uc8" %>

<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 >作廢發票滙入" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="作廢發票滙入" />
<uc7:InvoiceCancellationUploadList ID="uploadList" 
    runat="server" />
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnAddCode" runat="server" Text="確定" OnClick="btnAddCode_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnReset" runat="server" Text="取消" OnClick="btnReset_Click" />
        </td>
    </tr>
</table>
<uc8:PageAnchor ID="NextAction" runat="server" RedirectTo="~/EIVO/InvoiceCancellationUploadList.aspx" />
<uc8:PageAnchor ID="PrevAction" runat="server" RedirectTo="~/EIVO/InvoiceCancellationImport.aspx" />
