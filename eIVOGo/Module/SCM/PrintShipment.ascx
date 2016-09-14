<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintShipment.ascx.cs" Inherits="eIVOGo.Module.SCM.PrintShipment" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register src="View/SingleShipmentPreview.ascx" tagname="SingleShipmentPreview" tagprefix="uc3" %>
<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 出貨單預覽" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="出貨單預覽" />
<uc3:SingleShipmentPreview ID="shipment" runat="server" />
<table border="0" cellspacing="0" cellpadding="0" width="100%">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnReturn" runat="server" Text="回上頁"  />
                &nbsp;&nbsp;
                <asp:Button ID="btnPrint" runat="server" Text="列印" onclick="btnPrint_Click" />
            </td>
        </tr>
    </tbody>
</table>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if(Page.PreviousPage!=null && Page.PreviousPage.Items["id"]!=null)
        {
            shipment.PrepareDataFromDB((int)Page.PreviousPage.Items["id"]);
        }
    }
</script>