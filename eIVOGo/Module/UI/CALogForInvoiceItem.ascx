<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CALogForInvoiceItem.ascx.cs"
    Inherits="eIVOGo.Module.UI.CALogForInvoiceItem" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr class='<%# ItemType==ListItemType.AlternatingItem ? "OldLace":null %>'>
    <td align="center">
        <%# DataItem.CDS_Document.DocumentOwner.Organization.ReceiptNo%>
    </td>
    <td>
        <%# DataItem.CDS_Document.DocumentOwner.Organization.CompanyName%>
    </td>
    <td align="center">
        <%# ValueValidity.ConvertChineseDateTimeString(DataItem.LogDate)%>
    </td>
    <td width="15%" align="center">
        <asp:ImageButton ID="imgBtn" runat="server" ImageUrl="~/images/icon_ca.gif" CommandArgument="CAContentShow.aspx" />
        <%-- <a href="javascript:;" onclick="window.open('CAContentShow.aspx?logID=<%#DataItem.LogID%>','','toolbar=yes,scrollbars=1,width=780,height=500')">
            <img id="Img2" src="~/images/icon_ca.gif" runat="server" alt="" width="27" height="28"
                border="0" align="absmiddle" /></a>--%>
    </td>
</tr>
