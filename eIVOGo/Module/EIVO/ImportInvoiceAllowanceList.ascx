<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.EIVO.ImportInvoiceList" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc3" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc4" %>
<%@ Register Src="Item/InvoiceAllowanceUploadList.ascx" TagName="InvoiceAllowanceUploadList" TagPrefix="uc5" %>
<%@ Register src="../Common/PageAnchor.ascx" tagname="PageAnchor" tagprefix="uc6" %>
<%@ Import Namespace="Model.InvoiceManagement" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="Model.DataEntity" %>
<!--路徑名稱-->
<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 折讓單匯入" />
<!--交易畫面標題-->
<uc4:FunctionTitleBar ID="titleBar" runat="server" ItemName="折讓單匯入" />
<!--按鈕-->
<uc5:InvoiceAllowanceUploadList ID="uploadList" runat="server" />
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <uc3:PrintingButton2 ID="btnPrint" runat="server" />
        &nbsp;<asp:Button ID="btnExcel" runat="server" Text="匯出EXCEL檔" 
                onclick="btnExcel_Click" />
        </td>
    </tr>
</table>
<uc6:PageAnchor ID="NextAction" runat="server" RedirectTo="~/EIVO/InvoiceAllowanceUploadList.aspx" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        eIVOGo.Module.EIVO.Export.GoogleInvoiceCSVList item = (eIVOGo.Module.EIVO.Export.GoogleInvoiceCSVList)this.LoadControl("Export/GoogleInvoiceAllowanceCSVList.ascx");
        item.InitializeAsUserControl(Page);
        item.DataSource = ((GoogleAllowanceUploadManager)_mgr).ItemList;
        item.Export("Allowance");
    }
    
</script>