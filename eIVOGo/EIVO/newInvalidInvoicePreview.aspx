<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="base_page.aspx.cs" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>
<%@ Register src="../Module/EIVO/NewInvalidInvoicePreview.ascx" tagname="NewInvalidInvoicePreview" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">
    public override void VerifyRenderingInServerForm(Control control)
    {}
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <uc2:NewInvalidInvoicePreview ID="NewInvalidInvoicePreview1" runat="server" />
</body>
</html>
