<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateCertificateIdentity.aspx.cs"
    Inherits="eIVOGo.SAM.CreateCertificateIdentity" StylesheetTheme="Visitor" %>

<%@ Register Src="../Module/SAM/CreateOrganizationCertificate.ascx" TagName="CreateOrganizationCertificate"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <uc1:CreateOrganizationCertificate ID="CreateOrganizationCertificate1" runat="server" />
    </div>
    </form>
</body>
</html>
